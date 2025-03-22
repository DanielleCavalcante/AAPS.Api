using AAPS.Api.Dtos.Voluntarios;
using AAPS.Api.Responses;
using AAPS.Api.Services;
using AAPS.Api.Services.Autenticacao;
using AAPS.Api.Services.Voluntarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace AAPS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class VoluntarioController : Controller
    {
        #region ATRIBUTOS E CONSTRUTOR

        private readonly IVoluntarioService _voluntarioService;
        private readonly IAutenticacaoService _autenticacaoService;
        private readonly EmailService _emailService;

        public VoluntarioController(IVoluntarioService voluntario, IAutenticacaoService autenticacao, EmailService emailService)
        {
            _voluntarioService = voluntario;
            _autenticacaoService = autenticacao;
            _emailService = emailService;
        }

        #endregion

        [Authorize(Roles = "Admin")]
        [HttpPost("CriarVoluntario")]
        public async Task<IActionResult> CriarVoluntario([FromBody] CriarVoluntarioDto voluntarioDto)
        {
            var erros = new List<string>();

            if (string.IsNullOrEmpty(voluntarioDto.Nome))
                erros.Add("O campo 'Nome' é obrigatório!");
            if (string.IsNullOrEmpty(voluntarioDto.UserName))
                erros.Add("O campo 'Nome de Usuário' é obrigatório!");
            if (string.IsNullOrEmpty(voluntarioDto.Cpf))
                erros.Add("O campo 'CPF' é obrigatório!");
            if (string.IsNullOrEmpty(voluntarioDto.Email))
                erros.Add("O campo 'Email' é obrigatório!");
            if (string.IsNullOrEmpty(voluntarioDto.Telefone))
                erros.Add("O campo 'Telefone' é obrigatório!");
            if (string.IsNullOrEmpty(voluntarioDto.Status.ToString()))
                erros.Add("O campo 'Status' é obrigatório!");
            if (string.IsNullOrEmpty(voluntarioDto.Acesso))
                erros.Add("O campo 'Acesso' é obrigatório!");
            if (string.IsNullOrEmpty(voluntarioDto.Senha))
                erros.Add("O campo 'Senha' é obrigatório!");

            if (!Regex.IsMatch(voluntarioDto.Senha, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*(),.?"":{}|<>])[A-Za-z\d!@#$%^&*(),.?"":{}|<>]{8,}$"))
            {
                erros.Add("A senha deve ter pelo menos 8 caracteres, incluindo 1 letra maiúscula, 1 letra minúscula e 1 número.");
            }

            var usuarioExistente = await _voluntarioService.ObterVoluntarioPorUserName(voluntarioDto.UserName);
            if (usuarioExistente != null)
            {
                erros.Add("O nome de usuário já está em uso.");
            }

            if (erros.Count > 0)
            {
                return BadRequest(ApiResponse<object>.ErroResponse(erros, "Erro ao registrar voluntário!"));
            }

            if (voluntarioDto.Senha != voluntarioDto.ConfirmarSenha)
            {
                return BadRequest(ApiResponse<object>.ErroResponse(
                    new List<string> { "As senhas não conferem!" },
                    "Erro na validação das senhas"));
            }

            var resultado = await _voluntarioService.RegistrarVoluntario(voluntarioDto);

            if (!resultado)
            {
                return BadRequest(ApiResponse<object>.ErroResponse(new List<string> { "Erro ao criar voluntário." }));
            }

            return Ok(ApiResponse<object>.SucessoResponse(resultado, $"Usuário {voluntarioDto.UserName} cadastrado com sucesso!"));
        }

        [HttpPost("SolicitarResetSenha")]
        public async Task<IActionResult> SolicitarResetSenha([FromBody] SolicitarResetSenhaDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserName) || string.IsNullOrWhiteSpace(dto.Telefone))
                return BadRequest(ApiResponse<object>.ErroResponse(
                    new List<string> { "Nome de usuário e telefone são obrigatórios!" }, "Erro ao enviar solicitação."));

            var voluntario = await _voluntarioService.BuscarVoluntarioPorUsernameETelefoneAsync(dto.UserName, dto.Telefone);

            if (voluntario == null)
                return NotFound(ApiResponse<object>.ErroResponse(new List<string> { "Usuário não encontrado." }));

            var admins = await _voluntarioService.ObterAdministradoresAsync();

            if (!admins.Any())
                return BadRequest(ApiResponse<object>.ErroResponse(
                    new List<string> { "Nenhum administrador encontrado para receber a solicitação." }, "Erro ao enviar solicitação."));

            foreach (var admin in admins)
            {
                if (!string.IsNullOrWhiteSpace(admin.Email))
                {
                    await _emailService.EnviarEmailAsync(
                        admin.Email,
                        "Solicitação de Reset de Senha",
                        $"O voluntário {voluntario.UserName} solicitou um reset de senha. Contato: {voluntario.PhoneNumber}"
                    );
                }
            }

            return Ok(ApiResponse<object>.SucessoResponse("Solicitação enviada com sucesso!"));
        }

        [HttpPost("RedefinirSenha")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RedefinirSenha([FromBody] ResetarSenhaDto dto)
        {
            if (dto.VoluntarioId <= 0)
                return BadRequest(ApiResponse<object>.ErroResponse(
                    new List<string> { "ID do voluntário inválido." }, "Erro ao tentar resetar senha."));

            var usuarioExistente = await _voluntarioService.ObterVoluntarioPorId(dto.VoluntarioId);
            if (usuarioExistente == null)
            {
                return BadRequest(ApiResponse<object>.ErroResponse(
                    new List<string> { "O usuário especificado não existe." }, "Usuário não encontrado"));
            }

            try
            {
                bool sucesso = await _voluntarioService.RedefinirSenha(dto.VoluntarioId);
                if (sucesso)
                    return Ok(ApiResponse<object>.SucessoResponse("Senha redefinida com sucesso."));

                return NotFound(ApiResponse<object>.ErroResponse(
                    new List<string> { "Erro ao redefinir senha." }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }
}