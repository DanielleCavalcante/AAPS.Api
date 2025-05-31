using AAPS.Api.Context;
using AAPS.Api.Dtos.Autenticacao;
using AAPS.Api.Models;
using AAPS.Api.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

    //public async Task<bool> Autenticacao(LoginDto loginDto)
    //{
    //    var resultado = await _signInManager
    //        .PasswordSignInAsync(loginDto.UserName, loginDto.Senha, false, lockoutOnFailure: false);
    //    // cookie de entrada false(não fica salvo o login depois de sair), não bloquear conta se falhar o login.

    //    return resultado.Succeeded;
    //}

    public async Task<TokenDto> LoginComToken(LoginDto infoUsuario)
    {
        var usuario = await _userManager.Users
            .Include(v => v.Pessoa)
            .Where(v =>
                v.Pessoa.Status == StatusEnum.Ativo)
            .FirstOrDefaultAsync(v =>
                v.UserName == infoUsuario.UserName);
        //.FindByNameAsync(infoUsuario.UserName);

        if (usuario == null)
        {
            return null;
        }

        var resultado = await _signInManager
            .PasswordSignInAsync(usuario, infoUsuario.Senha, false, lockoutOnFailure: false);

        if (resultado.Succeeded)
        {
            var roles = await _userManager.GetRolesAsync(usuario);

            var token = await GerarToken(usuario);

            return token;
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
            new Claim("role", roles.FirstOrDefault()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );

        return new TokenDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = token.ValidTo,
            Role = roles.FirstOrDefault(),
            UsuarioId = usuarioAplicado.Id,
            NomeUsuario = usuarioAplicado.Pessoa.Nome
        };
    }

    #endregion
}