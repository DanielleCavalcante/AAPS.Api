using AAPS.Api.Context;
using AAPS.Api.Models;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.ConstrainedExecution;
using AAPS.Api.Services.PontoAdocao;
using AAPS.Api.Dtos.PontoAdocao;
public class PontoAdocaoService : IPontoAdocaoService
{
    private readonly AppDbContext _context;

    public PontoAdocaoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PontoAdocao>> ObterPontosAdocao()
    {
        return await _context.PontosAdocao.ToListAsync();
    }

    public async Task<PontoAdocao> ObterPontoAdocaoPorId(int id)
    {
        return await _context.PontosAdocao.FindAsync(id);
    }

    public async Task<IEnumerable<PontoAdocao>> ObterPontoAdocaoPorNome(string nome)
    {
        IEnumerable<PontoAdocao> pontosAdocao;

        if (!string.IsNullOrEmpty(nome))
        {
            pontosAdocao = await _context.PontosAdocao.Where(n => n.NomeFantasia.Contains(nome)).ToListAsync();
        }
        else
        {
            pontosAdocao = await ObterPontosAdocao();
        }
        return pontosAdocao;
    }

    public async Task CriarPontoAdocao(PontoAdocaoDto pontoAdocaoDto)
    {
        var pontoAdocao = new PontoAdocao
        {
            NomeFantasia = pontoAdocaoDto.NomeFantasia,
            Responsavel = pontoAdocaoDto.Responsavel,
            Cnpj = pontoAdocaoDto.Cnpj,
            Logradouro = pontoAdocaoDto.Logradouro,
            Numero = pontoAdocaoDto.Numero,
            Complemento = pontoAdocaoDto.Complemento,
            Bairro = pontoAdocaoDto.Bairro,
            Uf = pontoAdocaoDto.Uf,
            Cidade = pontoAdocaoDto.Cidade,
            Cep = pontoAdocaoDto.Cep
        };

        _context.PontosAdocao.Add(pontoAdocao);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarPontoAdocao(int id, PontoAdocaoDto pontoAdocaoDto)
    {
        var buscaRegistro = await _context.PontosAdocao.FindAsync(id);

        if (buscaRegistro == null)
        {
            throw new KeyNotFoundException($"Ponto de adoção com Id {id} não foi encontrado.");
        }

        buscaRegistro.NomeFantasia = pontoAdocaoDto.NomeFantasia;
        buscaRegistro.Responsavel = pontoAdocaoDto.Responsavel;
        buscaRegistro.Cnpj = pontoAdocaoDto.Cnpj;
        buscaRegistro.Logradouro = pontoAdocaoDto.Logradouro;
        buscaRegistro.Numero = pontoAdocaoDto.Numero;
        buscaRegistro.Complemento = pontoAdocaoDto.Complemento;
        buscaRegistro.Bairro = pontoAdocaoDto.Bairro;
        buscaRegistro.Uf = pontoAdocaoDto.Uf;
        buscaRegistro.Cidade = pontoAdocaoDto.Cidade;
        buscaRegistro.Cep = pontoAdocaoDto.Cep;

        _context.Entry(buscaRegistro).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task ExcluirPontoAdocao(int id)
    {
        var excluirRegistro = await _context.PontosAdocao.FindAsync(id);

        if(excluirRegistro != null)
        {
            _context.PontosAdocao.Remove(excluirRegistro);
            _context.SaveChanges();
        }
    }
}
