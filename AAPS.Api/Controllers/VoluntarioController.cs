using AAPS.Api.Dtos.Senha;
using AAPS.Api.Dtos.Voluntarios;
using AAPS.Api.Responses;
using AAPS.Api.Services;
using AAPS.Api.Services.Autenticacao;
using AAPS.Api.Services.Voluntarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AAPS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class VoluntarioController : Controller
    {
        #region ATRIBUTOS E CONSTRUTOR

        private readonly IVoluntarioService _voluntarioService;
        private readonly IAutenticacaoService _autenticacaoService;
        private readonly EmailService _emailService;
        //private readonly WhatsAppService _whatsAppService;

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
            var erros = await _voluntarioService.ValidarCriacaoVoluntario(voluntarioDto);

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

        //[HttpPost("SolicitarResetSenhaWhatsApp")]
        //public async Task<IActionResult> SolicitarResetSenhaWhatsApp([FromBody] SolicitarResetSenhaDto dto)
        //{
        //    if (string.IsNullOrWhiteSpace(dto.UserName) || string.IsNullOrWhiteSpace(dto.Telefone))
        //        return BadRequest(ApiResponse<object>.ErroResponse(
        //            new List<string> { "Nome de usuário e telefone são obrigatórios!" }, "Erro ao enviar solicitação."));

        //    var voluntario = await _voluntarioService.BuscarVoluntarioPorUsernameETelefoneAsync(dto.UserName, dto.Telefone);

        //    if (voluntario == null)
        //        return NotFound(ApiResponse<object>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        //    // Gera código de recuperação (expirável)
        //    var codigoRecuperacao = await _autenticacaoService.GerarCodigoRecuperacao(voluntario.Id);

        //    // Envia código via WhatsApp
        //    var sucessoEnvio = await _whatsAppService.EnviarCodigoRecuperacaoAsync(dto.Telefone, codigoRecuperacao);

        //    if (!sucessoEnvio)
        //        return BadRequest(ApiResponse<object>.ErroResponse(new List<string> { "Falha ao enviar código pelo WhatsApp." }));

        //    return Ok(ApiResponse<object>.SucessoResponse("Código de recuperação enviado com sucesso!"));
        //}

        //[HttpPost("ConfirmarResetSenha")]
        //public async Task<IActionResult> ConfirmarResetSenha([FromBody] ConfirmarResetSenhaDto dto)
        //{
        //    if (string.IsNullOrWhiteSpace(dto.UserName) || string.IsNullOrWhiteSpace(dto.Telefone) ||
        //        string.IsNullOrWhiteSpace(dto.CodigoRecuperacao) || string.IsNullOrWhiteSpace(dto.NovaSenha) ||
        //        string.IsNullOrWhiteSpace(dto.ConfirmarNovaSenha))
        //    {
        //        return BadRequest(ApiResponse<object>.ErroResponse(
        //            new List<string> { "Todos os campos são obrigatórios!" }, "Erro ao redefinir senha."));
        //    }

        //    if (dto.NovaSenha != dto.ConfirmarNovaSenha)
        //    {
        //        return BadRequest(ApiResponse<object>.ErroResponse(
        //            new List<string> { "As senhas não coincidem!" }, "Erro ao redefinir senha."));
        //    }

        //    var voluntario = await _voluntarioService.BuscarVoluntarioPorUsernameETelefoneAsync(dto.UserName, dto.Telefone);

        //    if (voluntario == null)
        //        return NotFound(ApiResponse<object>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        //    // Valida código de recuperação
        //    var codigoValido = _autenticacaoService.ValidarCodigoRecuperacao(voluntario.Id, dto.CodigoRecuperacao);

        //    //if (!codigoValido)
        //    //    return BadRequest(ApiResponse<object>.ErroResponse(new List<string> { "Código inválido ou expirado." }));

        //    // Atualiza senha
        //    var sucesso = await _voluntarioService.AlterarSenhaAsync(voluntario.Id, dto.NovaSenha);

        //    if (!sucesso)
        //        return BadRequest(ApiResponse<object>.ErroResponse(new List<string> { "Erro ao redefinir senha." }));

        //    return Ok(ApiResponse<object>.SucessoResponse("Senha redefinida com sucesso!"));
        //}

    }
}