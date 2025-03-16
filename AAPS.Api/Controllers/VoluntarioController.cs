using AAPS.Api.Dtos.Voluntarios;
using AAPS.Api.Services;
using AAPS.Api.Services.Autenticacao;
using AAPS.Api.Services.Voluntarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("CriarVoluntario")]
        public async Task<IActionResult> CriarVoluntario([FromBody] CriarVoluntarioDto voluntarioDto)
        {
            var erros = new List<string>();

            if (string.IsNullOrWhiteSpace(voluntarioDto.NomeCompleto))
                erros.Add("O campo 'Nome' é obrigatório!");
            if (string.IsNullOrWhiteSpace(voluntarioDto.NomeUsuario))
                erros.Add("O campo 'Nome de Usuário' é obrigatório!");
            if (string.IsNullOrWhiteSpace(voluntarioDto.Cpf))
                erros.Add("O campo 'CPF' é obrigatório!");
            if (string.IsNullOrWhiteSpace(voluntarioDto.Email))
                erros.Add("O campo 'Email' é obrigatório!");
            if (string.IsNullOrWhiteSpace(voluntarioDto.Telefone))
                erros.Add("O campo 'Telefone' é obrigatório!");
            if (string.IsNullOrWhiteSpace(voluntarioDto.Acesso))
                erros.Add("O campo 'Acesso' é obrigatório!");
            if (string.IsNullOrWhiteSpace(voluntarioDto.Senha))
                erros.Add("O campo 'Senha' é obrigatório!");

            if (erros.Any())
            {
                return BadRequest(erros);
            }

            if (voluntarioDto.Senha != voluntarioDto.ConfirmarSenha)
            {
                return BadRequest("As senhas não conferem!");
            }

            var resultado = await _voluntarioService.RegistrarVoluntario(voluntarioDto);

            if (!resultado)
            {
                return BadRequest("Erro ao criar o usuário.");
            }

            return Ok($"Usuário {voluntarioDto.NomeUsuario} criado com sucesso!");
        }

        [HttpPost("SolicitarResetSenha")]
        public async Task<IActionResult> SolicitarResetSenha([FromBody] SolicitarResetSenhaDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserName) || string.IsNullOrWhiteSpace(dto.Telefone))
                return BadRequest("Username e telefone são obrigatórios.");

            var voluntario = await _voluntarioService.BuscarUsuarioPorUsernameETelefoneAsync(dto.UserName, dto.Telefone);

            if (voluntario == null)
                return NotFound("Usuário não encontrado com os dados informados.");

            var admins = await _voluntarioService.ObterAdministradoresAsync();

            if (!admins.Any())
                return StatusCode(500, "Nenhum administrador encontrado para receber a solicitação.");

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

            return Ok("Solicitação enviada aos administradores.");
        }


        [HttpPost("RedefinirSenha")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RedefinirSenha([FromBody] ResetarSenhaDto dto)
        {
            if (dto.VoluntarioId <= 0)
                return BadRequest("ID do voluntário inválido.");

            try
            {
                bool sucesso = await _voluntarioService.RedefinirSenha(dto.VoluntarioId);
                if (sucesso)
                    return Ok("Senha redefinida com sucesso.");

                return NotFound("Voluntário não encontrado ou erro ao redefinir senha.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }
}
