using AAPS.Api.Models;
using AAPS.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AAPS.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PerfilAcessoController : Controller
{
    private readonly IPerfilAcessoService _perfilAcesso;

    public PerfilAcessoController(IPerfilAcessoService perfilAcesso)
    {
        _perfilAcesso = perfilAcesso;
    }

    [HttpGet]
    [Route("ObterTodos")]
    public async Task<ActionResult<IAsyncEnumerable<IdentityRole>>> ObterPerfis()
    {
        var perfis = await _perfilAcesso.ObterPerfis();
        return Ok(perfis);
    }

    [HttpGet]
    [Route("ObterIdPorNome")]
    public async Task<ActionResult<string>> ObterIdPerfilPorNome(string nome)
    {
        var perfil = _perfilAcesso.ObterIdPorNomeAsync(nome);
        return Ok(perfil);
    }

    [HttpPost]
    [Route("Criar")]
    public async Task CriarPerfil(string nomePerfil)
    {
        await _perfilAcesso.CriarPerfil(nomePerfil);
    }
}
