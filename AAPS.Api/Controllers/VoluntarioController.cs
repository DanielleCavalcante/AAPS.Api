using AAPS.Api.Dtos.Senha;
using AAPS.Api.Dtos.Voluntario;
using AAPS.Api.Responses;
using AAPS.Api.Services.Autenticacao;
using AAPS.Api.Services.Senhas;
using AAPS.Api.Services.Voluntarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AAPS.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
public class VoluntarioController : Controller
{
    #region ATRIBUTOS E CONSTRUTOR

    private readonly IVoluntarioService _voluntarioService;
    private readonly IAutenticacaoService _autenticacaoService;
    private readonly ISenhaService _senhaService;

    public VoluntarioController(IVoluntarioService voluntario, IAutenticacaoService autenticacao, ISenhaService senhaService)
    {
        _voluntarioService = voluntario;
        _autenticacaoService = autenticacao;
        _senhaService = senhaService;
    }

    #endregion

    [HttpPost]
    public async Task<IActionResult> CriarVoluntario([FromBody] CriarVoluntarioDto voluntarioDto)
    {
        var erros = await _voluntarioService.ValidarCriacaoVoluntario(voluntarioDto);

        if (erros.Count > 0)
        {
            return BadRequest(ApiResponse<object>.ErroResponse(erros, "Erro ao registrar voluntário!"));
        }

        //if (voluntarioDto.Senha != voluntarioDto.ConfirmarSenha)
        //{
        //    return BadRequest(ApiResponse<object>.ErroResponse(
        //        new List<string> { "As senhas não conferem!" },
        //        "Erro na validação das senhas"));
        //}

        var voluntario = await _voluntarioService.CriarVoluntario(voluntarioDto);

        if (!voluntario)
        {
            return BadRequest(ApiResponse<object>.ErroResponse(new List<string> { "Erro ao criar voluntário." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(voluntario, $"Usuário {voluntarioDto.UserName} cadastrado com sucesso!"));
    }

    [HttpGet]
    public async Task<ActionResult<IAsyncEnumerable<VoluntarioDto>>> ObterVoluntarios([FromQuery] FiltroVoluntarioDto filtro)
    {
        var voluntarios = await _voluntarioService.ObterVoluntarios(filtro);

        if (voluntarios is null || !voluntarios.Any())
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { "Nenhum voluntário foi encontrado." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(voluntarios));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> ObterVoluntarioPorId(int id)
    {
        var voluntario = await _voluntarioService.ObterVoluntarioPorId(id);

        if (voluntario is null)
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { $"Voluntário de id = {id} não encontrado." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(voluntario));
    }

    [HttpGet]
    public async Task<ActionResult<IAsyncEnumerable<VoluntarioDto>>> ObterVoluntariosAtivos()
    {
        var voluntarios = await _voluntarioService.ObterVoluntariosAtivos();

        if (voluntarios is null)
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { "Nenhum voluntário foi encontrado." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(voluntarios));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> AtualizarVoluntario(int id, [FromBody] AtualizarVoluntarioDto voluntarioDto)
    {
        var erros = _voluntarioService.ValidarAtualizacaoVoluntario(voluntarioDto);

        if (erros.Count > 0)
        {
            return BadRequest(ApiResponse<object>.ErroResponse(erros, "Erro ao atualizar voluntário!"));
        }

        var voluntario = await _voluntarioService.AtualizarVoluntario(id, voluntarioDto);

        if (voluntario is null)
        {
            return BadRequest(ApiResponse<object>.ErroResponse(new List<string> { $"Voluntário de id = {id} não encontrado." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(voluntario, $"Usuário {voluntarioDto.UserName} atualizado com sucesso!"));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> ExcluirVoluntario(int id)
    {
        bool voluntario = await _voluntarioService.ExcluirVoluntario(id);

        if (!voluntario)
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { $"Voluntário de id = {id} não encontrado." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse($"Voluntário de id = {id} excluído com sucesso!"));
    }

    [HttpPost]
    public async Task<IActionResult> ResetarSenha([FromBody] ResetarSenhaDto dto)
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
            bool sucesso = await _senhaService.ResetarSenha(dto.VoluntarioId);
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