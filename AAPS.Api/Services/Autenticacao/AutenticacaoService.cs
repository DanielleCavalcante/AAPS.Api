using AAPS.Api.Context;
using AAPS.Api.Dtos.Autenticacao;
using AAPS.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AAPS.Api.Services.Autenticacao;

public class AutenticacaoService : IAutenticacaoService
{
    #region ATRIBUTOS E CONSTRUTOR

    private readonly SignInManager<Voluntario> _signInManager;
    private readonly UserManager<Voluntario> _userManager;
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AutenticacaoService(SignInManager<Voluntario> signInManager, UserManager<Voluntario> userManager, AppDbContext context, IConfiguration configuration)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _context = context;
        _configuration = configuration;
    }

    #endregion

    public async Task<bool> Autenticacao(LoginDto loginDto)
    {
        var resultado = await _signInManager
            .PasswordSignInAsync(loginDto.NomeUsuario, loginDto.Senha, false, lockoutOnFailure: false);
        // cookie de entrada false(não fica salvo o login depois de sair), não bloquear conta se falhar o login.

        return resultado.Succeeded;
    }

    public async Task<TokenDto> LoginComToken(LoginDto infoUsuario)
    {
        var usuario = await _userManager.FindByNameAsync(infoUsuario.NomeUsuario);
        if (usuario == null)
        {
            return null;
        }

        var resultado = await _signInManager.PasswordSignInAsync(usuario, infoUsuario.Senha, false, lockoutOnFailure: false);
        if (resultado.Succeeded)
        {
            return await GerarToken(usuario);
        }

        return null;
    }

    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }

    #region MÉTODOS PRIVADOS

    private async Task<TokenDto> GerarToken(Voluntario usuario)
    {
        var usuarioAplicado = await _userManager.FindByNameAsync(usuario.UserName);

        var roles = await _userManager.GetRolesAsync(usuarioAplicado);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));  // Adiciona a role ao token
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        return new TokenDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token)
        };
    }

    #endregion
}
