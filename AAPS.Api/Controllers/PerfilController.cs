using AAPS.Api.Dtos.Senha;
using AAPS.Api.Models;
using AAPS.Api.Responses;
using AAPS.Api.Services.Senhas;
using AAPS.Api.Services.Voluntarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace AAPS.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PerfilController : Controller
{
    #region ATRIBUTOS E CONSTRUTOR

    private readonly ISenhaService _senhaService;
    private readonly IVoluntarioService _voluntarioService;
    private readonly UserManager<Voluntario> _userManager;

    public PerfilController(ISenhaService senhaService, IVoluntarioService voluntarioService, UserManager<Voluntario> userManager)
    {
        _senhaService = senhaService;
        _voluntarioService = voluntarioService;
        _userManager = userManager;
    }

    #endregion

    [HttpPut("{id:int}")]
    public async Task<IActionResult> AlterarSenha(int id, [FromBody] AlterarSenhaDto alterarSenhaDto)
    {
        if(id <= 0)
            return BadRequest(ApiResponse<object>.ErroResponse(new List<string> { "ID inválido." }, "Erro ao alterar senha."));

        var erros = new List<string>();

        if (string.IsNullOrEmpty(alterarSenhaDto.SenhaAtual))
            erros.Add("O campo 'Senha atual' é obrigatório!");
        if (string.IsNullOrEmpty(alterarSenhaDto.NovaSenha))
            erros.Add("O campo 'Nova senha' é obrigatório!");
        if (string.IsNullOrEmpty(alterarSenhaDto.ConfirmarNovaSenha))
            erros.Add("O campo 'Confirmar nova senha' é obrigatório!");

        var voluntario = await _userManager.FindByIdAsync(id.ToString());
        if (voluntario == null)
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { "Voluntário não encontrado." }, "Erro ao alterar senha."));

        var senhaCorreta = await _userManager.CheckPasswordAsync(voluntario, alterarSenhaDto.SenhaAtual);
        if (!senhaCorreta)
            return BadRequest(ApiResponse<object>.ErroResponse(new List<string> { "Senha atual incorreta." }, "Erro ao alterar senha."));

        if (alterarSenhaDto.NovaSenha != alterarSenhaDto.ConfirmarNovaSenha)
            erros.Add("As senhas não conferem!");

        if (!Regex.IsMatch(alterarSenhaDto.NovaSenha, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*(),.?"":{}|<>])[A-Za-z\d!@#$%^&*(),.?"":{}|<>]{8,}$"))
        {
            erros.Add("A senha deve ter pelo menos 8 caracteres, incluindo 1 letra maiúscula, 1 letra minúscula, 1 número e 1 caractere especial.");
        }

        if (erros.Count > 0)
        {
            return BadRequest(ApiResponse<object>.ErroResponse(erros, "Erro ao alterar senha"));
        }

        var resultado = await _senhaService.AlterarSenhaAsync(id, alterarSenhaDto);

        if (resultado)
        {
            return Ok(ApiResponse<object>.SucessoResponse(resultado, "Senha alterada com sucesso!"));
        }
        else
        {
            return BadRequest(ApiResponse<object>.ErroResponse(new List<string> { "Erro ao alterar a senha." }));
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> ObterPerfilPorId(int id)
    {
        var voluntario = await _voluntarioService.ObterVoluntarioPorId(id);

        if (voluntario is null)
        {
            return NotFound(ApiResponse<object>.ErroResponse(new List<string> { $"Voluntário de id = {id} não encontrado." }));
        }

        return Ok(ApiResponse<object>.SucessoResponse(voluntario));
    }
}