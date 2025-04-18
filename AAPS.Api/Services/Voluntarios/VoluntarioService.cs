using AAPS.Api.Context;
using AAPS.Api.Dtos.Voluntario;
using AAPS.Api.Models;
using AAPS.Api.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace AAPS.Api.Services.Voluntarios;

public class VoluntarioService : IVoluntarioService
{
    #region ATRIBUTOS E CONSTRUTOR

    private readonly AppDbContext _context;
    private readonly EmailService _emailService;
    private readonly UserManager<Voluntario> _userManager;
    private readonly SignInManager<Voluntario> _signInManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public VoluntarioService(AppDbContext context, UserManager<Voluntario> userManager, SignInManager<Voluntario> signInManager, EmailService emailService, RoleManager<IdentityRole<int>> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
        _roleManager = roleManager;
    }

    #endregion

    public async Task<bool> CriarVoluntario(CriarVoluntarioDto voluntarioDto)
    {
        var pessoa = new Pessoa
        {
            Nome = voluntarioDto.Nome,
            Cpf = voluntarioDto.Cpf,
            Tipo = TipoPessoaEnum.Voluntario,
            Status = voluntarioDto.Status,
        };

        _context.Pessoas.Add(pessoa);
        await _context.SaveChangesAsync();

        var voluntario = new Voluntario
        {
            UserName = voluntarioDto.UserName,
            Email = voluntarioDto.Email,
            PhoneNumber = voluntarioDto.PhoneNumber,
            SecurityStamp = Guid.NewGuid().ToString(),
            PessoaId = pessoa.Id
        };

        var senhaPadrao = "Aaps@123";

        var resultado = await _userManager.CreateAsync(voluntario, senhaPadrao);

        if (!resultado.Succeeded)
        {
            return false;
        }

        if (resultado.Succeeded)
        {
            await _userManager.AddToRoleAsync(voluntario, voluntarioDto.Acesso);
            await _signInManager.SignInAsync(voluntario, isPersistent: false); // não persistir cookie de login
        }

        return resultado.Succeeded;
    }

    public async Task<IEnumerable<VoluntarioDto>> ObterVoluntarios(FiltroVoluntarioDto filtro)
    {
        var query = _context.Voluntarios
            .Include(v => v.Pessoa)
            .Where(v => v.Pessoa.Tipo == TipoPessoaEnum.Voluntario)
            .AsQueryable();

        if (!string.IsNullOrEmpty(filtro.Busca))
        {
            string buscaLower = filtro.Busca.ToLower();

            query = query.Where(a =>
                a.Pessoa.Nome.ToLower().Contains(buscaLower) ||
                a.Pessoa.Rg.Contains(buscaLower) ||
                a.Pessoa.Cpf.Contains(buscaLower) ||
                a.UserName.Contains(buscaLower) ||
                a.Email.Contains(buscaLower)
            );
        }

        if (filtro.Status.HasValue)
        {
            query = query.Where(a => a.Pessoa.Status == filtro.Status.Value);
        }

        var voluntarios = await query.ToListAsync();
        var voluntarioDto = new List<VoluntarioDto>();

        foreach (var v in voluntarios)
        {
            var roles = await _userManager.GetRolesAsync(v);
            voluntarioDto.Add(new VoluntarioDto
            {
                Id = v.Id,
                Nome = v.Pessoa.Nome,
                Cpf = v.Pessoa.Cpf,
                Status = v.Pessoa.Status,
                UserName = v.UserName,
                Email = v.Email,
                PhoneNumber = v.PhoneNumber,
                Acesso = roles.FirstOrDefault()
            });
        }

        return voluntarioDto;
    }

    public async Task<VoluntarioDto?> ObterVoluntarioPorId(int id)
    {
        var voluntario = await BuscarVoluntarioPorId(id);

        var role = await _userManager.GetRolesAsync(voluntario);

        if (voluntario == null)
        {
            return null;
        }

        return new VoluntarioDto
        {
            Id = voluntario.Id,
            Nome = voluntario.Pessoa.Nome,
            Cpf = voluntario.Pessoa.Cpf,
            Status = voluntario.Pessoa.Status,
            UserName = voluntario.UserName,
            Email = voluntario.Email,
            PhoneNumber = voluntario.PhoneNumber,
            Acesso = role.FirstOrDefault()
        };
    }

    public async Task<IEnumerable<VoluntarioDto>> ObterVoluntariosAtivos()
    {
        var voluntarios = await _context.Voluntarios
            .Include(v => v.Pessoa)
            .Where(v => v.Pessoa.Status == StatusEnum.Ativo && v.Pessoa.Tipo == TipoPessoaEnum.Voluntario)
            .Select(v => new VoluntarioDto
            {
                Id = v.Id,
                Nome = v.Pessoa.Nome,
                Cpf = v.Pessoa.Cpf,
                Status = v.Pessoa.Status,
                UserName = v.UserName,
                Email = v.Email,
                PhoneNumber = v.PhoneNumber,
                Acesso = string.Empty // ver se vai precisar retornar a role depois
            })
            .ToListAsync();

        return voluntarios;
    }

    public async Task<VoluntarioDto?> ObterVoluntarioPorUsernameETelefone(string username, string telefone)
    {
        var voluntario = await _context.Voluntarios
            .Include(v => v.Pessoa)
            .Where(v => v.Pessoa.Tipo == TipoPessoaEnum.Voluntario)
            .FirstOrDefaultAsync(v => v.UserName == username &&
                v.PhoneNumber == telefone &&
                v.Pessoa.Status == StatusEnum.Ativo); // retornar só se tiver ativo - ver se é isso mesmo

        var role = await _userManager.GetRolesAsync(voluntario);

        if (voluntario == null)
        {
            return null;
        }

        return new VoluntarioDto
        {
            Id = voluntario.Id,
            Nome = voluntario.Pessoa.Nome,
            Cpf = voluntario.Pessoa.Cpf,
            Status = voluntario.Pessoa.Status,
            UserName = voluntario.UserName,
            Email = voluntario.Email,
            PhoneNumber = voluntario.PhoneNumber,
            Acesso = role.FirstOrDefault()
        };
    }

    public async Task<List<VoluntarioDto>> ObterAdministradores()
    {
        var admins = await _userManager.GetUsersInRoleAsync("Admin");

        var voluntarioDto = new List<VoluntarioDto>();

        foreach (var admin in admins)
        {
            var voluntario = await BuscarVoluntarioPorId(admin.Id);

            if (voluntario != null)
            {
                voluntarioDto.Add(new VoluntarioDto
                {
                    Id = voluntario.Id,
                    Nome = voluntario.Pessoa.Nome,
                    Cpf = voluntario.Pessoa.Cpf,
                    Status = voluntario.Pessoa.Status,
                    UserName = voluntario.UserName,
                    Email = voluntario.Email,
                    PhoneNumber = voluntario.PhoneNumber,
                    Acesso = "Admin"
                });
            }
        }

        return voluntarioDto;
    }

    public async Task<VoluntarioDto?> ObterVoluntarioPorUserName(string username)
    {
        var voluntario = await _context.Voluntarios
            .Include(v => v.Pessoa)
            .Where(v => v.Pessoa.Tipo == TipoPessoaEnum.Voluntario)
            .FirstOrDefaultAsync(v => v.UserName == username);

        if (voluntario == null)
        {
            return null;
        }

        var role = await _userManager.GetRolesAsync(voluntario);

        return new VoluntarioDto
        {
            Id = voluntario.Id,
            Nome = voluntario.Pessoa.Nome,
            Cpf = voluntario.Pessoa.Cpf,
            Status = voluntario.Pessoa.Status,
            UserName = voluntario.UserName,
            Email = voluntario.Email,
            PhoneNumber = voluntario.PhoneNumber,
            Acesso = role.FirstOrDefault()
        };
    }

    public async Task<VoluntarioDto> AtualizarVoluntario(int id, AtualizarVoluntarioDto voluntarioDto)
    {
        var voluntario = await BuscarVoluntarioPorId(id);

        if (voluntario is null)
        {
            return null;
        }

        voluntario.Pessoa.Nome = string.IsNullOrWhiteSpace(voluntarioDto.Nome) ?
            voluntario.Pessoa.Nome : voluntarioDto.Nome;
        voluntario.Pessoa.Cpf = string.IsNullOrWhiteSpace(voluntarioDto.Cpf) ?
            voluntario.Pessoa.Cpf : voluntarioDto.Cpf;
        voluntario.Pessoa.Status = voluntarioDto.Status ?? voluntario.Pessoa.Status;

        voluntario.UserName = string.IsNullOrWhiteSpace(voluntarioDto.UserName) ?
            voluntario.UserName : voluntarioDto.UserName;
        voluntario.Email = string.IsNullOrWhiteSpace(voluntarioDto.Email) ?
            voluntario.Email : voluntarioDto.Email;
        voluntario.PhoneNumber = string.IsNullOrWhiteSpace(voluntarioDto.PhoneNumber) ?
            voluntario.PhoneNumber : voluntarioDto.PhoneNumber;

        if (!string.IsNullOrWhiteSpace(voluntarioDto.Acesso))
        {
            var roleAtual = await _userManager.GetRolesAsync(voluntario);

            if (!roleAtual.Contains(voluntarioDto.Acesso))
            {
                await _userManager.RemoveFromRolesAsync(voluntario, roleAtual);
                await _userManager.AddToRoleAsync(voluntario, voluntarioDto.Acesso);
            }
        }

        await _context.SaveChangesAsync();

        var role = (await _userManager.GetRolesAsync(voluntario)).FirstOrDefault();

        return new VoluntarioDto
        {
            Id = voluntario.Id,
            Nome = voluntario.Pessoa.Nome,
            Cpf = voluntario.Pessoa.Cpf,
            Status = voluntario.Pessoa.Status,
            UserName = voluntario.UserName,
            Email = voluntario.Email,
            PhoneNumber = voluntario.PhoneNumber,
            Acesso = role
        };
    }

    public async Task<bool> ExcluirVoluntario(int id)
    {
        var voluntario = await BuscarVoluntarioPorId(id);

        if (voluntario == null)
        {
            return false;
        }

        voluntario.Pessoa.Status = StatusEnum.Inativo;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<string>> ValidarCriacaoVoluntario(CriarVoluntarioDto voluntarioDto)
    {
        var erros = new List<string>();

        if (string.IsNullOrEmpty(voluntarioDto.Nome))
            erros.Add("O campo 'Nome' é obrigatório!");
        if (string.IsNullOrEmpty(voluntarioDto.Cpf))
            erros.Add("O campo 'CPF' é obrigatório!");
        if (string.IsNullOrEmpty(voluntarioDto.Status.ToString()) || !Enum.IsDefined(typeof(StatusEnum), voluntarioDto.Status))
            erros.Add("O campo 'Status' é obrigatório!");

        if (string.IsNullOrEmpty(voluntarioDto.UserName))
            erros.Add("O campo 'Nome de Usuário' é obrigatório!");
        if (string.IsNullOrEmpty(voluntarioDto.Email))
            erros.Add("O campo 'Email' é obrigatório!");
        if (string.IsNullOrEmpty(voluntarioDto.PhoneNumber))
            erros.Add("O campo 'Telefone' é obrigatório!");

        //if (string.IsNullOrEmpty(voluntarioDto.Acesso))
        //    erros.Add("O campo 'Acesso' é obrigatório!");
        //if (string.IsNullOrEmpty(voluntarioDto.Senha))
        //    erros.Add("O campo 'Senha' é obrigatório!");
        //if (voluntarioDto.Senha != voluntarioDto.ConfirmarSenha)
        //    erros.Add("As senhas não conferem!");

        if (!await _roleManager.RoleExistsAsync(voluntarioDto.Acesso))
        {
            erros.Add("A role informada não existe.");
        }

        //if (!Regex.IsMatch(voluntarioDto.Senha, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*(),.?"":{}|<>])[A-Za-z\d!@#$%^&*(),.?"":{}|<>]{8,}$"))
        //{
        //    erros.Add("A senha deve ter pelo menos 8 caracteres, incluindo 1 letra maiúscula, 1 letra minúscula, 1 número e 1 caractere especial.");
        //}

        var usuarioExistente = await ObterVoluntarioPorUserName(voluntarioDto.UserName);
        if (usuarioExistente != null)
        {
            erros.Add("O nome de usuário já está em uso.");
        }

        var voluntarioExistente = await _userManager.Users
            .Where(v =>
                v.Pessoa.Cpf == voluntarioDto.Cpf &&
                v.Email == voluntarioDto.Email &&
                v.PhoneNumber == voluntarioDto.PhoneNumber
            )
            .FirstOrDefaultAsync();

        if (voluntarioExistente != null)
        {
            erros.Add($"Voluntário já cadastrado. Código {voluntarioExistente.Id}");
        }

        return erros;
    }

    public List<string> ValidarAtualizacaoVoluntario(AtualizarVoluntarioDto voluntarioDto)
    {
        var erros = new List<string>();

        if (voluntarioDto.Nome != null && string.IsNullOrWhiteSpace(voluntarioDto.Nome))
            erros.Add("O campo 'Nome' não pode ser vazio!");
        if (voluntarioDto.Cpf != null && string.IsNullOrWhiteSpace(voluntarioDto.Cpf))
            erros.Add("O campo 'CPF' não pode ser vazio!");
        if (voluntarioDto.Status != null && string.IsNullOrWhiteSpace(voluntarioDto.Status.ToString()))
            erros.Add("O campo 'Status' não pode ter ser vazio!");

        if (voluntarioDto.UserName != null && string.IsNullOrWhiteSpace(voluntarioDto.UserName))
            erros.Add("O campo 'Nome de Usuário' não pode ser vazio!");
        if (voluntarioDto.Email != null && string.IsNullOrWhiteSpace(voluntarioDto.Email))
            erros.Add("O campo 'Email' não pode ser vazio!");
        if (voluntarioDto.PhoneNumber != null && string.IsNullOrWhiteSpace(voluntarioDto.PhoneNumber))
            erros.Add("O campo 'Telefone' não pode ser vazio!");
        if (voluntarioDto.Acesso != null && string.IsNullOrWhiteSpace(voluntarioDto.Acesso))
            erros.Add("O campo 'Acesso' não pode ser vazio!");

        return erros;
    }

    #region MÉTODOS PRIVADOS

    private async Task<Voluntario?> BuscarVoluntarioPorId(int id)
    {
        return await _userManager.Users
            .Include(v => v.Pessoa)
            .Where(v => v.Pessoa.Tipo == TipoPessoaEnum.Voluntario)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    #endregion
}