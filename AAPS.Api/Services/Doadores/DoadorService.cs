using AAPS.Api.Context;
using AAPS.Api.Dtos.Animais;
using AAPS.Api.Dtos.Doadores;
using AAPS.Api.Models;
using AAPS.Api.Models.Enums;
using AAPS.Api.Services.Doadores;
using Microsoft.EntityFrameworkCore;

public class DoadorService : IDoadorService
{
    #region ATRIBUTOS E CONSTRUTOR

    private readonly AppDbContext _context;

    public DoadorService(AppDbContext context)
    {
        _context = context;
    }

    #endregion

    public async Task<DoadorDto> CriarDoador(CriarDoadorDto doadorDto)
    {
        var doador = new Doador
        {
            Nome = doadorDto.Nome,
            Rg = doadorDto.Rg,
            Cpf = doadorDto.Cpf,
            Logradouro = doadorDto.Logradouro,
            Numero = doadorDto.Numero,
            Complemento = doadorDto.Complemento,
            Bairro = doadorDto.Bairro,
            Uf = doadorDto.Uf,
            Cidade = doadorDto.Cidade,
            Cep = doadorDto.Cep,
            Status = doadorDto.Status,
        };

        _context.Doadores.Add(doador);
        await _context.SaveChangesAsync();

        return new DoadorDto
        {
            Id = doador.Id,
            Nome = doadorDto.Nome,
            Rg = doadorDto.Rg,
            Cpf = doadorDto.Cpf,
            Logradouro = doadorDto.Logradouro,
            Numero = doadorDto.Numero,
            Complemento = doadorDto.Complemento,
            Bairro = doadorDto.Bairro,
            Uf = doadorDto.Uf,
            Cidade = doadorDto.Cidade,
            Cep = doadorDto.Cep,
            Status = doadorDto.Status,
        };
    }

    public async Task<IEnumerable<DoadorDto>> ObterDoadores(FiltroDoadorDto filtro)
    {
        var query = _context.Doadores.AsQueryable();

        if (!string.IsNullOrEmpty(filtro.Busca))
        {
            query = query.Where(p =>
                p.Nome.Contains(filtro.Busca.ToLower()) ||
                p.Cpf.Contains(filtro.Busca.ToLower()) ||
                p.Rg.Contains(filtro.Busca)
            );
        }

        if (filtro.Status.HasValue)
        {
            query = query.Where(a => a.Status == filtro.Status.Value);
        }

        var doadoresDto = await query
            .Select(d => new DoadorDto
            {
                Id = d.Id,
                Nome = d.Nome,
                Rg = d.Rg,
                Cpf = d.Cpf,
                Logradouro = d.Logradouro,
                Numero = d.Numero,
                Complemento = d.Complemento,
                Bairro = d.Bairro,
                Uf = d.Uf,
                Cidade = d.Cidade,
                Cep = d.Cep,
                Status = d.Status,
            })
            .ToListAsync();

        return doadoresDto;
    }

    public async Task<DoadorDto?> ObterDoadorPorId(int id)
    {
        var doador = await BuscarDoadorPorId(id);

        if (doador == null)
        {
            return null;
        }

        return new DoadorDto
        {
            Id = doador.Id,
            Nome = doador.Nome,
            Rg = doador.Rg,
            Cpf = doador.Cpf,
            Logradouro = doador.Logradouro,
            Numero = doador.Numero,
            Complemento = doador.Complemento,
            Bairro = doador.Bairro,
            Uf = doador.Uf,
            Cidade = doador.Cidade,
            Cep = doador.Cep,
            Status = doador.Status,
        };
    }

    public async Task<IEnumerable<Doador>> ObterDoadorPorNome(string nome)
    {
        return await BuscarDoadoresPorNome(nome).ToListAsync();
    }

    public async Task<DoadorDto?> AtualizarDoador(int id, AtualizarDoadorDto doadorDto)
    {
        var doador = await BuscarDoadorPorId(id);

        if (doador == null)
        {
            return null;
        }

        doador.Nome = string.IsNullOrEmpty(doadorDto.Nome) ? doador.Nome : doadorDto.Nome;
        doador.Rg = string.IsNullOrEmpty(doadorDto.Rg) ? doador.Rg : doadorDto.Rg;
        doador.Cpf = string.IsNullOrEmpty(doadorDto.Cpf) ? doador.Cpf : doadorDto.Cpf;
        doador.Logradouro = string.IsNullOrEmpty(doadorDto.Logradouro) ? doador.Logradouro : doadorDto.Logradouro;
        doador.Numero = doadorDto.Numero.HasValue ? doadorDto.Numero.Value : doador.Numero;
        doador.Complemento = string.IsNullOrEmpty(doadorDto.Complemento) ? doador.Complemento : doadorDto.Complemento;
        doador.Bairro = string.IsNullOrEmpty(doadorDto.Bairro) ? doador.Bairro : doadorDto.Bairro;
        doador.Uf = string.IsNullOrEmpty(doadorDto.Uf) ? doador.Uf : doadorDto.Uf;
        doador.Cidade = string.IsNullOrEmpty(doadorDto.Cidade) ? doador.Cidade : doadorDto.Cidade;
        doador.Cep = doadorDto.Cep.HasValue ? doadorDto.Cep.Value : doador.Cep;
        doador.Status = doadorDto.Status.HasValue ? doadorDto.Status.Value : doador.Status;

        await _context.SaveChangesAsync();

        var doadorAtualizado = new DoadorDto
        {
            Id = doador.Id,
            Nome = doador.Nome,
            Rg = doador.Rg,
            Cpf = doador.Cpf,
            Logradouro = doador.Logradouro,
            Numero = doador.Numero,
            Complemento = doador.Complemento,
            Bairro = doador.Bairro,
            Uf = doador.Uf,
            Cidade = doador.Cidade,
            Cep = doador.Cep,
            Status = doador.Status,
        };

        return doadorAtualizado;
    }

    public async Task<bool> ExcluirDoador(int id)
    {
        var doador = await BuscarDoadorPorId(id);

        if (doador is null)
        {
            return false;
        }

        doador.Status = StatusEnum.Inativo;

        //_context.Doadores.Remove(doador);
        await _context.SaveChangesAsync();

        return true;
    }

    #region MÉTODOS PRIVADAS

    private async Task<Doador?> BuscarDoadorPorId(int id)
    {
        var doador = await _context.Doadores.FindAsync(id);
        return doador;
    }

    private IQueryable<Doador> BuscarDoadoresPorNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            return _context.Doadores;
        }

        return _context.Doadores.Where(a => a.Nome.ToLower().Contains(nome.ToLower()));
    }

    #endregion
}
