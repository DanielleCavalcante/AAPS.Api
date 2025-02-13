using AAPS.Api.Services.Interfaces;
using AAPS.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AAPS.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContaController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IAutenticacaoService _autenticacao;

    public ContaController(IConfiguration configuration, IAutenticacaoService autenticacao)
    {
        _configuration = configuration ??
            throw new ArgumentNullException(nameof(configuration));

        _autenticacao = autenticacao ??
            throw new ArgumentNullException(nameof(autenticacao));
    }

    [HttpPost("CriarUsuario")]
    public async Task<ActionResult<UsuarioToken>> CriarUsuario([FromBody] RegistroViewModel modelo)
    {
        if (modelo.Senha != modelo.ConfirmarSenha)
        {
            ModelState.AddModelError("ConfirmarSenha", "As senhas não conferem!");
            return BadRequest(ModelState);
        }

        var resultado = await _autenticacao.RegistrarUsuario(modelo.NomeCompleto, modelo.NomeUsuario, modelo.Cpf, modelo.Email, modelo.Telefone, modelo.Senha, modelo.Acesso);

        if (resultado)
        {
            return Ok($"Usuário {modelo.NomeUsuario} criado com sucesso!");
        }
        else
        {
            ModelState.AddModelError("CriarUsuario", "Erro ao criar usuário!");
            return BadRequest(ModelState);
        }
    }

    [HttpPost("LoginUsuario")]
    public async Task<ActionResult<UsuarioToken>> Login([FromBody] LoginViewModel infoUsuario)
    {
        var resultado = await _autenticacao.Autenticacao(infoUsuario.NomeUsuario, infoUsuario.Senha);

        if (resultado)
        {
            return GenerateToken(infoUsuario);
        }
        else
        {
            ModelState.AddModelError("LoginUsuario", "Login inválido!");
            return BadRequest(ModelState);
        }
    }

    #region MÉTODOS PRIVADOS
    private ActionResult<UsuarioToken> GenerateToken(LoginViewModel infoUsuario)
    {
        var claims = new[]
        {
           new Claim("nomeUsuario", infoUsuario.NomeUsuario),
           new Claim("meuToken", "Token AAPS"),
           new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        var credencial = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

        var expiracao = DateTime.UtcNow.AddMinutes(30);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiracao,
            signingCredentials: credencial);

        return new UsuarioToken()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiracao = expiracao,
        };
    }
    #endregion
}
