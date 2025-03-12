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
    #region ATRIBUTOS E CONSTRUTOR
    private readonly IPerfilAcessoService _perfilAcesso;

    public PerfilAcessoController(IPerfilAcessoService perfilAcesso)
    {
        _perfilAcesso = perfilAcesso;
    }

    #endregion

    // talvez excluir depois
    [HttpPost]
    public async Task CriarPerfil(string nomePerfil)
    {
        await _perfilAcesso.CriarPerfil(nomePerfil);
    }

    [HttpGet]
    public async Task<ActionResult<IAsyncEnumerable<IdentityRole>>> ObterPerfis()
    {
        var perfis = await _perfilAcesso.ObterPerfis();

        if (perfis is null)
        {
            return StatusCode(500, "Erro ao obter perfis.");
        }

        return Ok(perfis);
    }

    [HttpGet]
    public async Task<ActionResult<string>> ObterIdPerfilPorNome(string nome)
    {
        var perfil = await _perfilAcesso.ObterIdPorNomeAsync(nome);

        if (string.IsNullOrEmpty(perfil))
        {
            return BadRequest($"Perfil {nome} não encontrado.");
        }
        return Ok(perfil);
    }
}
