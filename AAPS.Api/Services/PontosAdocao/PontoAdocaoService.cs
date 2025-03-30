using AAPS.Api.Context;
using AAPS.Api.Dtos.PontoAdocao;
using AAPS.Api.Dtos.PontosAdocao;
using AAPS.Api.Models;
using AAPS.Api.Services.PontosAdocao;
using Microsoft.EntityFrameworkCore;
public class PontoAdocaoService : IPontoAdocaoService
{
    #region ATRIBUTOS E CONSTRUTOR

    private readonly AppDbContext _context;

    public PontoAdocaoService(AppDbContext context)
    {
        _context = context;
    }

    #endregion

    public async Task<PontoAdocaoDto> CriarPontoAdocao(CriarPontoAdocaoDto pontoAdocaoDto)
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

        return new PontoAdocaoDto
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
    }

    public async Task<IEnumerable<PontoAdocaoDto>> ObterPontosAdocao(FiltroPontoAdocaoDto filtro)
    {
        var query = _context.PontosAdocao.AsQueryable();

        if (!string.IsNullOrEmpty(filtro.Busca))
        {
            // busca por NomeFantasia, Responsavel e Cnpj
            query = query.Where(p =>
                p.NomeFantasia.Contains(filtro.Busca.ToLower()) ||
                p.Responsavel.Contains(filtro.Busca.ToLower()) ||
                p.Cnpj.Contains(filtro.Busca)
            );
        }

        var pontoAdocaoDto = await query
            .Select(p => new PontoAdocaoDto
            {
                Id = p.Id,
                NomeFantasia = p.NomeFantasia,
                Responsavel = p.Responsavel,
                Cnpj = p.Cnpj,
                Logradouro = p.Logradouro,
                Numero = p.Numero,
                Complemento = p.Complemento,
                Bairro = p.Bairro,
                Uf = p.Uf,
                Cidade = p.Cidade,
                Cep = p.Cep
            })
            .ToListAsync();

        return pontoAdocaoDto;
    }

    public async Task<PontoAdocaoDto?> ObterPontoAdocaoPorId(int id)
    {
        var pontoAdocao = await BuscarPontoAdocaoPorId(id);

        if (pontoAdocao == null)
        {
            return null;
        }

        return new PontoAdocaoDto
        {
            Id = pontoAdocao.Id,
            NomeFantasia = pontoAdocao.NomeFantasia,
            Responsavel = pontoAdocao.Responsavel,
            Cnpj = pontoAdocao.Cnpj,
            Logradouro = pontoAdocao.Logradouro,
            Numero = pontoAdocao.Numero,
            Complemento = pontoAdocao.Complemento,
            Bairro = pontoAdocao.Bairro,
            Uf = pontoAdocao.Uf,
            Cidade = pontoAdocao.Cidade,
            Cep = pontoAdocao.Cep
        };
    }

    public async Task<IEnumerable<PontoAdocao>> ObterPontoAdocaoPorNome(string nome)
    {
        return await BuscarPontoAdocaoPorNome(nome).ToListAsync();
    }

    public async Task<PontoAdocaoDto> AtualizarPontoAdocao(int id, AtualizaPontoAdocaoDto pontoAdocaoDto)
    {
        var pontoAdocao = await BuscarPontoAdocaoPorId(id);

        if (pontoAdocao is null)
        {
            return null;
        }

        pontoAdocao.NomeFantasia = string.IsNullOrEmpty(pontoAdocaoDto.NomeFantasia) ? pontoAdocao.NomeFantasia : pontoAdocaoDto.NomeFantasia;
        pontoAdocao.Responsavel = string.IsNullOrEmpty(pontoAdocaoDto.Responsavel) ? pontoAdocao.Responsavel : pontoAdocaoDto.Responsavel;
        pontoAdocao.Cnpj = string.IsNullOrEmpty(pontoAdocaoDto.Cnpj) ? pontoAdocao.Cnpj : pontoAdocaoDto.Cnpj;
        pontoAdocao.Logradouro = string.IsNullOrEmpty(pontoAdocaoDto.Logradouro) ? pontoAdocao.Logradouro : pontoAdocaoDto.Logradouro;
        pontoAdocao.Numero = pontoAdocaoDto.Numero.HasValue ? pontoAdocaoDto.Numero.Value : pontoAdocao.Numero;
        pontoAdocao.Complemento = string.IsNullOrEmpty(pontoAdocaoDto.Complemento) ? pontoAdocao.Complemento : pontoAdocaoDto.Complemento;
        pontoAdocao.Bairro = string.IsNullOrEmpty(pontoAdocaoDto.Bairro) ? pontoAdocao.Bairro : pontoAdocaoDto.Bairro;
        pontoAdocao.Uf = string.IsNullOrEmpty(pontoAdocaoDto.Uf) ? pontoAdocao.Uf : pontoAdocaoDto.Uf;
        pontoAdocao.Cidade = string.IsNullOrEmpty(pontoAdocaoDto.Cidade) ? pontoAdocao.Cidade : pontoAdocaoDto.Cidade;
        pontoAdocao.Cep = pontoAdocaoDto.Cep.HasValue ? pontoAdocaoDto.Cep.Value : pontoAdocao.Cep;

        await _context.SaveChangesAsync();

        var pontoAdocaoAtualizado = new PontoAdocaoDto
        {
            Id = pontoAdocao.Id,
            NomeFantasia = pontoAdocao.NomeFantasia,
            Responsavel = pontoAdocao.Responsavel,
            Cnpj = pontoAdocao.Cnpj,
            Logradouro = pontoAdocao.Logradouro,
            Numero = pontoAdocao.Numero,
            Complemento = pontoAdocao.Complemento,
            Bairro = pontoAdocao.Bairro,
            Uf = pontoAdocao.Uf,
            Cidade = pontoAdocao.Cidade,
            Cep = pontoAdocao.Cep
        };

        return pontoAdocaoAtualizado;
    }

    public async Task<bool> ExcluirPontoAdocao(int id)
    {
        var pontoAdocao = await BuscarPontoAdocaoPorId(id);

        if (pontoAdocao == null)
        {
            return false;
        }

        _context.PontosAdocao.Remove(pontoAdocao);
        await _context.SaveChangesAsync();

        return true;
    }

    #region MÉTODOS PRIVADOS

    private async Task<PontoAdocao?> BuscarPontoAdocaoPorId(int id)
    {
        var pontoAdocao = await _context.PontosAdocao.FindAsync(id);
        return pontoAdocao;
    }

    private IQueryable<PontoAdocao> BuscarPontoAdocaoPorNome(string nome)
    {
        return _context.PontosAdocao.Where(p => p.NomeFantasia.Contains(nome));
    }

    #endregion
}