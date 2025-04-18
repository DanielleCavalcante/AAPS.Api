using AAPS.Api.Dtos.Autenticacao;
using AAPS.Api.Dtos.Senha;
using AAPS.Api.Responses;
using AAPS.Api.Services;
using AAPS.Api.Services.Autenticacao;
using AAPS.Api.Services.Voluntarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AAPS.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AutenticacaoController : Controller
{
    #region ATRIBUTOS E CONSTRUTOR

    private readonly IConfiguration _configuration;
    private readonly IAutenticacaoService _autenticacaoService;
    private readonly IVoluntarioService _voluntarioService;
    private readonly EmailService _emailService;

    public AutenticacaoController(IConfiguration configuration, IAutenticacaoService autenticacao, IVoluntarioService voluntarioService, EmailService emailService)
    {
        _configuration = configuration ??
            throw new ArgumentNullException(nameof(configuration));

        _autenticacaoService = autenticacao ??
            throw new ArgumentNullException(nameof(autenticacao));
        _voluntarioService = voluntarioService;
        _emailService = emailService;
    }

    #endregion

    [HttpPost("Login")]
    public async Task<ActionResult<TokenDto>> Login([FromBody] LoginDto infoUsuario)
    {
        var tokenDto = await _autenticacaoService.LoginComToken(infoUsuario);

        if (tokenDto != null)
        {
            return Ok(tokenDto);
        }
        else
        {
            ModelState.AddModelError("LoginUsuario", "Login inválido!");
            return BadRequest(ModelState);
        }
    }

    [Authorize]
    [HttpPost("Logout")]
    public async Task<IActionResult> Logout()
    {
        await _autenticacaoService.Logout();
        return Ok(new { mensagem = "Logout realizado com sucesso." });
    }

    [HttpPost("SolicitarResetSenha")]
    public async Task<IActionResult> SolicitarResetSenha([FromBody] SolicitarResetSenhaDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.UserName) || string.IsNullOrWhiteSpace(dto.Telefone))
            return BadRequest(ApiResponse<object>.ErroResponse(
                new List<string> { "Nome de usuário e telefone são obrigatórios!" }, "Erro ao enviar solicitação."));

        var voluntario = await _voluntarioService.ObterVoluntarioPorUsernameETelefone(dto.UserName, dto.Telefone);

        if (voluntario == null)
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        var admins = await _voluntarioService.ObterAdministradores();

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

    [HttpGet()]
    [Authorize]
    public IActionResult ValidarToken()
    {
        return Ok();
    }
}