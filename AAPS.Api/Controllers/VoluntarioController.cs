using AAPS.Api.Dtos.Senha;
using AAPS.Api.Dtos.Voluntario;
using AAPS.Api.Responses;
using AAPS.Api.Services.Autenticacao;
using AAPS.Api.Services.Senhas;
using AAPS.Api.Services.Voluntarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AAPS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
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

        var resultado = await _voluntarioService.CriarVoluntario(voluntarioDto);

        if (!resultado)
        {
            return BadRequest(ApiResponse<object>.ErroResponse(new List<string> { "Erro ao criar voluntário." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(resultado, $"Usuário {voluntarioDto.UserName} cadastrado com sucesso!"));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("RedefinirSenha")]
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