using AAPS.Api.Dtos.Autenticacao;
using AAPS.Api.Services.Autenticacao;
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

    public AutenticacaoController(IConfiguration configuration, IAutenticacaoService autenticacao)
    {
        _configuration = configuration ??
            throw new ArgumentNullException(nameof(configuration));

        _autenticacaoService = autenticacao ??
            throw new ArgumentNullException(nameof(autenticacao));
    }

    #endregion

    [HttpPost("LoginUsuario")]
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
}