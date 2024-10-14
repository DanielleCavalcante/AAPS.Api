using AAPS.Api.Context;
using AAPS.Api.Dtos;
using AAPS.Api.Models;
using AAPS.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

public class DoadorService : IDoadorService
{
    private readonly AppDbContext _context;

    public DoadorService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Doador>> ObterDoadores()
    {
        return await _context.Doadores.ToListAsync();
    }

    public async Task<Doador> ObterDoadorPorId(int id)
    {
        return await _context.Doadores.FindAsync(id);
    }

    public async Task<IEnumerable<Doador>> ObterDoadorPorNome(string nome)
    {
        IEnumerable<Doador> doadores;

        if (!string.IsNullOrEmpty(nome))
        {
            doadores = await _context.Doadores.Where(n => n.Nome.Contains(nome)).ToListAsync();
        }
        else
        {
            doadores = await ObterDoadores();
        }
        return doadores;
    }

    public async Task CriarDoador(DoadorDto doadorDto)
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
        };

        _context.Doadores.Add(doador);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarDoador(int id, DoadorDto doadorDto)
    {
        var buscaRegistro = await _context.Doadores.FindAsync(id);

        if (buscaRegistro == null)
        {
            throw new KeyNotFoundException($"Doador com Id {id} não foi encontrado.");
        }

        buscaRegistro.Nome = doadorDto.Nome;
        buscaRegistro.Rg = doadorDto.Rg;
        buscaRegistro.Cpf = doadorDto.Cpf;
        buscaRegistro.Logradouro = doadorDto.Logradouro;
        buscaRegistro.Numero = doadorDto.Numero;
        buscaRegistro.Complemento = doadorDto.Complemento;
        buscaRegistro.Bairro = doadorDto.Bairro;
        buscaRegistro.Uf = doadorDto.Uf;
        buscaRegistro.Cidade = doadorDto.Cidade;
        buscaRegistro.Cep = doadorDto.Cep;

        _context.Entry(buscaRegistro).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task ExcluirDoador(int id)
    {
        var excluirRegistro = await _context.Doadores.FindAsync(id);

        if (excluirRegistro != null)
        {
            _context.Doadores.Remove(excluirRegistro);
            _context.SaveChanges();
        }
    }
}
