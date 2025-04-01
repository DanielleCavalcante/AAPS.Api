using AAPS.Api.Context;
using AAPS.Api.Dtos.Animais;
using AAPS.Api.Dtos.Doadores;
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

    public async Task<List<string>> ValidarCriacaoPontoAdocao(CriarPontoAdocaoDto pontoAdocaoDto)
    {
        var erros = new List<string>();

        if (string.IsNullOrEmpty(pontoAdocaoDto.NomeFantasia))
            erros.Add("O campo 'Nome Fantasia' é obrigatório!");
        if (string.IsNullOrEmpty(pontoAdocaoDto.Responsavel))
            erros.Add("O campo 'Responsavel' é obrigatório!");
        if (string.IsNullOrEmpty(pontoAdocaoDto.Cnpj))
            erros.Add("O campo 'CNPJ' é obrigatório!");
        if (string.IsNullOrEmpty(pontoAdocaoDto.Logradouro))
            erros.Add("O campo 'Logradouro' é obrigatório!");
        if (string.IsNullOrEmpty(pontoAdocaoDto.Numero.ToString()) || pontoAdocaoDto.Numero <= 0)
            erros.Add("O campo 'Número' é obrigatório e deve ser maior que zero!");
        if (string.IsNullOrEmpty(pontoAdocaoDto.Bairro))
            erros.Add("O campo 'Bairro' é obrigatório!");
        if (string.IsNullOrEmpty(pontoAdocaoDto.Uf) || pontoAdocaoDto.Uf.Length != 2)
            erros.Add("O campo 'UF' é obrigatório!");
        if (string.IsNullOrEmpty(pontoAdocaoDto.Cidade))
            erros.Add("O campo 'Cidade' é obrigatório!");
        if (pontoAdocaoDto.Cep <= 0 || pontoAdocaoDto.Cep.ToString().Length != 8)
            erros.Add("O campo 'CEP' é obrigatório e deve ter exatamente 8 dígitos!");

        var pontoAdocaoExistente = await _context.PontosAdocao
            .Where(p =>
                p.NomeFantasia == pontoAdocaoDto.NomeFantasia &&
                p.Responsavel == pontoAdocaoDto.Responsavel &&
                p.Cnpj == pontoAdocaoDto.Cnpj &&
                p.Logradouro == pontoAdocaoDto.Logradouro &&
                p.Numero == pontoAdocaoDto.Numero &&
                p.Complemento == pontoAdocaoDto.Complemento &&
                p.Bairro == pontoAdocaoDto.Bairro &&
                p.Uf == pontoAdocaoDto.Uf &&
                p.Cidade == pontoAdocaoDto.Cidade &&
                p.Cep == pontoAdocaoDto.Cep
            )
            .FirstOrDefaultAsync();

        if (pontoAdocaoExistente != null)
        {
            erros.Add($"Ponto de adoção já cadastrado. Código {pontoAdocaoExistente.Id}");
        }

        return erros;
    }

    public List<string> ValidarAtualizacaoPontoAdocao(AtualizaPontoAdocaoDto pontoAdocaoDto)
    {
        var erros = new List<string>();

        if (pontoAdocaoDto.NomeFantasia != null && string.IsNullOrEmpty(pontoAdocaoDto.NomeFantasia))
            erros.Add("O campo 'Nome Fantasia' não pode ser vazio!");
        if (pontoAdocaoDto.Responsavel != null && string.IsNullOrEmpty(pontoAdocaoDto.Responsavel))
            erros.Add("O campo 'Responsavel' não pode ser vazio!");
        if (pontoAdocaoDto.Cnpj != null && string.IsNullOrEmpty(pontoAdocaoDto.Cnpj))
            erros.Add("O campo 'CNPJ' não pode ser vazio!");
        if (pontoAdocaoDto.Logradouro != null && string.IsNullOrEmpty(pontoAdocaoDto.Logradouro))
            erros.Add("O campo 'Logradouro' não pode ser vazio!");
        if (pontoAdocaoDto.Numero != null && pontoAdocaoDto.Numero <= 0)
            erros.Add("O campo 'Número' é obrigatório e deve ser maior que zero!");
        if (pontoAdocaoDto.Bairro != null && string.IsNullOrEmpty(pontoAdocaoDto.Bairro))
            erros.Add("O campo 'Bairro' não pode ser vazio!");
        if (pontoAdocaoDto.Uf != null && (string.IsNullOrEmpty(pontoAdocaoDto.Uf) || pontoAdocaoDto.Uf.Length != 2))
            erros.Add("O campo 'UF' não pode ser vazio!");
        if (pontoAdocaoDto.Cidade != null && string.IsNullOrEmpty(pontoAdocaoDto.Cidade))
            erros.Add("O campo 'Cidade' não pode ser vazio!");
        if (pontoAdocaoDto.Cep != null && (pontoAdocaoDto.Cep <= 0 || pontoAdocaoDto.Cep.ToString().Length != 8))
            erros.Add("O campo 'CEP' é obrigatório e deve ter exatamente 8 dígitos!");

        return erros;
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