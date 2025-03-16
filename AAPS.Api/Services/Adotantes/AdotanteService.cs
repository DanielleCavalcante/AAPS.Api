using AAPS.Api.Context;
using AAPS.Api.Dtos.Adotante;
using AAPS.Api.Models;
using AAPS.Api.Services.Adotantes;
using Microsoft.EntityFrameworkCore;

public class AdotanteService : IAdotanteService
{
    private readonly AppDbContext _context;

    public AdotanteService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Adotante>> ObterAdotantes()
    {
        return await _context.Adotantes.ToListAsync();
    }

    public async Task<Adotante> ObterAdotantePorId(int id)
    {
        return await _context.Adotantes.FindAsync(id);
    }

    public async Task<IEnumerable<Adotante>> ObterAdotantePorNome(string nome)
    {
        IEnumerable<Adotante> adotantes;

        if (!string.IsNullOrEmpty(nome))
        {
            adotantes = await _context.Adotantes.Where(n => n.Nome.Contains(nome)).ToListAsync();
        }
        else
        {
            adotantes = await ObterAdotantes();
        }
        return adotantes;
    }

    public async Task CriarAdotante(AdotanteDto adotanteDto)
    {
        var adotante = new Adotante
        {
            Nome = adotanteDto.Nome,
            Rg = adotanteDto.Rg,
            Cpf = adotanteDto.Cpf,
            LocalTrabalho = adotanteDto.LocalTrabalho,
            Status = adotanteDto.Status,
            Facebook = adotanteDto.Facebook,
            Instagram = adotanteDto.Instagram,
            Logradouro = adotanteDto.Logradouro,
            Numero = adotanteDto.Numero,
            Complemento = adotanteDto.Complemento,
            Bairro = adotanteDto.Bairro,
            Uf = adotanteDto.Uf,
            Cidade = adotanteDto.Cidade,
            Cep = adotanteDto.Cep,
            SituacaoEndereco = adotanteDto.SituacaoEndereco
        };

        _context.Adotantes.Add(adotante);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAdotante(int id, AdotanteDto adotanteDto)
    {
        var buscaRegistro = await _context.Adotantes.FindAsync(id);

        if (buscaRegistro == null)
        {
            throw new KeyNotFoundException($"Adotante com Id {id} não foi encontrado.");
        }

        buscaRegistro.Nome = adotanteDto.Nome;
        buscaRegistro.Rg = adotanteDto.Rg;
        buscaRegistro.Cpf = adotanteDto.Cpf;
        buscaRegistro.LocalTrabalho = adotanteDto.LocalTrabalho;
        buscaRegistro.Status = adotanteDto.Status;
        buscaRegistro.Facebook = adotanteDto.Facebook;
        buscaRegistro.Instagram = adotanteDto.Instagram;
        buscaRegistro.Logradouro = adotanteDto.Logradouro;
        buscaRegistro.Numero = adotanteDto.Numero;
        buscaRegistro.Complemento = adotanteDto.Complemento;
        buscaRegistro.Bairro = adotanteDto.Bairro;
        buscaRegistro.Uf = adotanteDto.Uf;
        buscaRegistro.Cidade = adotanteDto.Cidade;
        buscaRegistro.Cep = adotanteDto.Cep;
        buscaRegistro.SituacaoEndereco = adotanteDto.SituacaoEndereco;

        _context.Entry(buscaRegistro).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task ExcluirAdotante(int id)
    {
        var excluirRegistro = await _context.Adotantes.FindAsync(id);

        if (excluirRegistro != null)
        {
            _context.Adotantes.Remove(excluirRegistro);
            _context.SaveChanges();
        }
    }
}
