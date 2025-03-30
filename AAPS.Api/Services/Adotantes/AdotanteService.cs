using AAPS.Api.Context;
using AAPS.Api.Dtos.Adotante;
using AAPS.Api.Dtos.Adotantes;
using AAPS.Api.Models;
using AAPS.Api.Models.Enums;
using AAPS.Api.Services.Adotantes;
using Microsoft.EntityFrameworkCore;

public class AdotanteService : IAdotanteService
{
    #region ATRIBUTOS E CONSTRUTOR

    private readonly AppDbContext _context;

    public AdotanteService(AppDbContext context)
    {
        _context = context;
    }

    #endregion

    public async Task<AdotanteDto> CriarAdotante(CriarAdotanteDto adotanteDto)
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
            SituacaoEndereco = adotanteDto.SituacaoEndereco,
            Bloqueio = adotanteDto.Bloqueio
        };

        _context.Adotantes.Add(adotante);
        await _context.SaveChangesAsync();

        return new AdotanteDto
        {
            Id = adotante.Id,
            Nome = adotante.Nome,
            Rg = adotante.Rg,
            Cpf = adotante.Cpf,
            LocalTrabalho = adotante.LocalTrabalho,
            Status = adotante.Status,
            Facebook = adotante.Facebook,
            Instagram = adotante.Instagram,
            Logradouro = adotante.Logradouro,
            Numero = adotante.Numero,
            Complemento = adotante.Complemento,
            Bairro = adotante.Bairro,
            Uf = adotante.Uf,
            Cidade = adotante.Cidade,
            Cep = adotante.Cep,
            SituacaoEndereco = adotante.SituacaoEndereco,
            Bloqueio = adotante.Bloqueio,
        };
    }

    public async Task<IEnumerable<AdotanteDto>> ObterAdotantes(FiltroAdotanteDto filtro)
    {
        var query = _context.Adotantes.AsQueryable();

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

        if (filtro.Bloqueio.HasValue)
        {
            query = query.Where(a => a.Bloqueio == filtro.Bloqueio.Value);
        }

        var adotantesDto = await query
            .Select(a => new AdotanteDto
            {
                Id = a.Id,
                Nome = a.Nome,
                Rg = a.Rg,
                Cpf = a.Cpf,
                LocalTrabalho = a.LocalTrabalho,
                Status = a.Status,
                Facebook = a.Facebook,
                Instagram = a.Instagram,
                Logradouro = a.Logradouro,
                Numero = a.Numero,
                Complemento = a.Complemento,
                Bairro = a.Bairro,
                Uf = a.Uf,
                Cidade = a.Cidade,
                Cep = a.Cep,
                SituacaoEndereco = a.SituacaoEndereco,
                Bloqueio = a.Bloqueio
            })
            .ToListAsync();

        return adotantesDto;
    }

    public async Task<AdotanteDto?> ObterAdotantePorId(int id)
    {
        var adotante = await BuscarAdotantePorId(id);

        if (adotante == null)
        {
            return null;
        }

        return new AdotanteDto
        {
            Id = adotante.Id,
            Nome = adotante.Nome,
            Rg = adotante.Rg,
            Cpf = adotante.Cpf,
            LocalTrabalho = adotante.LocalTrabalho,
            Status = adotante.Status,
            Facebook = adotante.Facebook,
            Instagram = adotante.Instagram,
            Logradouro = adotante.Logradouro,
            Numero = adotante.Numero,
            Complemento = adotante.Complemento,
            Bairro = adotante.Bairro,
            Uf = adotante.Uf,
            Cidade = adotante.Cidade,
            Cep = adotante.Cep,
            SituacaoEndereco = adotante.SituacaoEndereco,
            Bloqueio = adotante.Bloqueio
        };
    }

    public async Task<IEnumerable<Adotante>> ObterAdotantePorNome(string nome)
    {
        return await BuscarAdotantePorNome(nome).ToListAsync();
    }

    public async Task<AdotanteDto?> AtualizarAdotante(int id, AtualizarAdotanteDto adotanteDto)
    {
        var adotante = await BuscarAdotantePorId(id);

        if (adotante == null)
        {
            return null;
        }

        adotante.Nome = string.IsNullOrEmpty(adotanteDto.Nome) ? adotante.Nome : adotanteDto.Nome;
        adotante.Rg = string.IsNullOrEmpty(adotanteDto.Rg) ? adotante.Rg : adotanteDto.Rg;
        adotante.Cpf = string.IsNullOrEmpty(adotanteDto.Cpf) ? adotante.Cpf : adotanteDto.Cpf;
        adotante.LocalTrabalho = string.IsNullOrEmpty(adotanteDto.LocalTrabalho) ? adotante.LocalTrabalho : adotanteDto.LocalTrabalho;
        adotante.Status = adotanteDto.Status.HasValue ? adotanteDto.Status.Value : adotante.Status;
        adotante.Facebook = string.IsNullOrEmpty(adotanteDto.Facebook) ? adotante.Facebook : adotanteDto.Facebook;
        adotante.Instagram = string.IsNullOrEmpty(adotanteDto.Instagram) ? adotante.Instagram : adotanteDto.Instagram;
        adotante.Logradouro = string.IsNullOrEmpty(adotanteDto.Logradouro) ? adotante.Logradouro : adotanteDto.Logradouro;
        adotante.Numero = adotanteDto.Numero.HasValue ? adotanteDto.Numero.Value : adotante.Numero;
        adotante.Complemento = string.IsNullOrEmpty(adotanteDto.Complemento) ? adotante.Complemento : adotanteDto.Complemento;
        adotante.Bairro = string.IsNullOrEmpty(adotanteDto.Bairro) ? adotante.Bairro : adotanteDto.Bairro;
        adotante.Uf = string.IsNullOrEmpty(adotanteDto.Uf) ? adotante.Uf : adotanteDto.Uf;
        adotante.Cidade = string.IsNullOrEmpty(adotanteDto.Cidade) ? adotante.Cidade : adotanteDto.Cidade;
        adotante.Cep = adotanteDto.Cep.HasValue ? adotanteDto.Cep.Value : adotante.Cep;
        adotante.SituacaoEndereco = string.IsNullOrEmpty(adotanteDto.SituacaoEndereco) ? adotante.SituacaoEndereco : adotanteDto.SituacaoEndereco;
        adotante.Bloqueio = adotanteDto.Bloqueio.HasValue ? adotanteDto.Bloqueio.Value : adotante.Bloqueio;

        await _context.SaveChangesAsync();

        var adotanteAtualizado = new AdotanteDto
        {
            Id = adotante.Id,
            Nome = adotante.Nome,
            Rg = adotante.Rg,
            Cpf = adotante.Cpf,
            LocalTrabalho = adotante.LocalTrabalho,
            Status = adotante.Status,
            Facebook = adotante.Facebook,
            Instagram = adotante.Instagram,
            Logradouro = adotante.Logradouro,
            Numero = adotante.Numero,
            Complemento = adotante.Complemento,
            Bairro = adotante.Bairro,
            Uf = adotante.Uf,
            Cidade = adotante.Cidade,
            Cep = adotante.Cep,
            SituacaoEndereco = adotante.SituacaoEndereco,
            Bloqueio = adotante.Bloqueio
        };

        return adotanteAtualizado;
    }

    public async Task<bool> ExcluirAdotante(int id)
    {
        var adotante = await BuscarAdotantePorId(id);

        if (adotante == null)
        {
            return false;
        }

        adotante.Status = StatusEnum.Inativo; //

        //_context.Adotantes.Remove(adotante);
        await _context.SaveChangesAsync();

        return true;
    }

    #region MÉTODOS PRIVADOS

    private async Task<Adotante?> BuscarAdotantePorId(int id)
    {
        var adotante = await _context.Adotantes.FindAsync(id);
        return adotante;
    }

    private IQueryable<Adotante> BuscarAdotantePorNome(string nome)
    {
        return _context.Adotantes.Where(a => a.Nome.Contains(nome));
    }

    #endregion
}