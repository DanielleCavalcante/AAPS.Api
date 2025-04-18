using AAPS.Api.Context;
using AAPS.Api.Dtos.Adocao;
using AAPS.Api.Dtos.Animal;
using AAPS.Api.Dtos.PontoAdocao;
using AAPS.Api.Models;
using AAPS.Api.Models.Enums;
using AAPS.Api.Services.Adotantes;
using AAPS.Api.Services.Animais;
using AAPS.Api.Services.PontosAdocao;
using AAPS.Api.Services.Voluntarios;
using Microsoft.EntityFrameworkCore;

namespace AAPS.Api.Services.Adocoes
{
    public class AdocaoService : IAdocaoService
    {
        #region ATRIBUTOS E CONSTRUTOR

        private readonly AppDbContext _context;
        private readonly IAdotanteService _adotanteService;
        private readonly IAnimalService _animalService;
        private readonly IVoluntarioService _voluntarioService;
        private readonly IPontoAdocaoService _pontoAdocaoService;

        public AdocaoService(AppDbContext context, IAdotanteService adotanteService, IAnimalService animalService, IVoluntarioService voluntarioService, IPontoAdocaoService pontoAdocaoService)
        {
            _context = context;
            _adotanteService = adotanteService;
            _animalService = animalService;
            _voluntarioService = voluntarioService;
            _pontoAdocaoService = pontoAdocaoService;
        }

        #endregion

        public async Task<AdocaoDto> CriarAdocao(CriarAdocaoDto adocaoDto)
        {
            var adotante = await _adotanteService.ObterAdotantePorId(adocaoDto.AdotanteId);

            if (adotante == null || adotante.Status == StatusEnum.Inativo || adotante.Bloqueio == BloqueioEnum.Bloqueado)
            {
                return null;
            }

            //var animal = await _animalService.ObterAnimalPorId(adocaoDto.AnimalId);

            var animal = await _context.Animais.FirstOrDefaultAsync(a => a.Id == adocaoDto.AnimalId);

            if (animal == null || animal.Status == StatusEnum.Inativo || animal.Disponibilidade == DisponibilidadeEnum.Adotado)
            {
                return null;
            }

            var voluntario = await _voluntarioService.ObterVoluntarioPorId(adocaoDto.VoluntarioId);

            if (voluntario == null || voluntario.Status == StatusEnum.Inativo)
            {
                return null;
            }

            var pontoAdocao = await _pontoAdocaoService.ObterPontoAdocaoPorId(adocaoDto.PontoAdocaoId);

            if (pontoAdocao == null || pontoAdocao.Status == StatusEnum.Inativo)
            {
                return null;
            }

            var adocao = new Adocao
            {
                Data = adocaoDto.Data,
                AdotanteId = adocaoDto.AdotanteId,
                AnimalId = adocaoDto.AnimalId,
                VoluntarioId = adocaoDto.VoluntarioId,
                PontoAdocaoId = adocaoDto.PontoAdocaoId
            };

            _context.Adocoes.Add(adocao);

            animal.Disponibilidade = DisponibilidadeEnum.Adotado;
            await _context.SaveChangesAsync();

            return new AdocaoDto
            {
                Id = adocao.Id,
                Data = adocao.Data,
                AdotanteId = adocao.AdotanteId,
                AnimalId = adocao.AnimalId,
                VoluntarioId = adocao.VoluntarioId,
                PontoAdocaoId = adocao.PontoAdocaoId
            };
        }

        public async Task<IEnumerable<AdocaoDto>> ObterAdocoes(FiltroAdocaoDto filtro)
        {
            var query = _context.Adocoes
                .Include(a => a.Adotante)
                .Include(a => a.Animal)
                .Include(a => a.Voluntario)
                .Include(a => a.PontoAdocao)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filtro.Busca))
            {
                string buscaLower = filtro.Busca.ToLower();

                query = query.Where(a =>
                    a.Adotante.Pessoa.Nome.ToLower().Contains(buscaLower) ||
                    a.Animal.Pessoa.Nome.Contains(buscaLower) ||
                    a.Voluntario.Pessoa.Nome.Contains(buscaLower) ||
                    a.PontoAdocao.Pessoa.Nome.Contains(buscaLower)
                );
            }

            var adocoesDto = await query
                .Select(a => new AdocaoDto
                {
                    Id = a.Id,
                    Data = a.Data,
                    AdotanteId = a.AdotanteId,
                    AnimalId = a.AnimalId,
                    VoluntarioId = a.VoluntarioId,
                    PontoAdocaoId = a.PontoAdocaoId
                })
                .ToListAsync();

            return adocoesDto;
        }

        public async Task<AdocaoDto?> ObterAdocaoPorId(int id)
        {
            var adocao = await BuscarAdocaoPorId(id);

            if (adocao == null)
            {
                return null;
            }

            return new AdocaoDto
            {
                Id = adocao.Id,
                Data = adocao.Data,
                AdotanteId = adocao.AdotanteId,
                AnimalId = adocao.AnimalId,
                VoluntarioId = adocao.VoluntarioId,
                PontoAdocaoId = adocao.PontoAdocaoId
            };
        }

        public async Task<AdocaoDto> AtualizarAdocao(int id, AtualizarAdocaoDto adocaoDto)
        {
            var adocao = await BuscarAdocaoPorId(id);

            if (adocao is null)
            {
                return null;
            }

            adocao.Data = adocaoDto.Data.HasValue ? adocaoDto.Data.Value : adocao.Data;
            adocao.AdotanteId = adocaoDto.AdotanteId.HasValue ? adocaoDto.AdotanteId.Value : adocao.AdotanteId;
            adocao.AnimalId = adocaoDto.AnimalId.HasValue ? adocaoDto.AnimalId.Value : adocao.AnimalId;
            adocao.VoluntarioId = adocaoDto.VoluntarioId.HasValue ? adocaoDto.VoluntarioId.Value : adocao.VoluntarioId;
            adocao.PontoAdocaoId = adocaoDto.PontoAdocaoId.HasValue ? adocaoDto.PontoAdocaoId.Value : adocao.PontoAdocaoId;

            await _context.SaveChangesAsync();

            return new AdocaoDto
            {
                Id = adocao.Id,
                Data = adocao.Data,
                AdotanteId = adocao.AdotanteId,
                AnimalId = adocao.AnimalId,
                VoluntarioId = adocao.VoluntarioId,
                PontoAdocaoId = adocao.PontoAdocaoId
            };
        }

        public async Task<bool> ExcluirAdocao(int id)
        {
            var adocao = await BuscarAdocaoPorId(id);

            if (adocao == null)
            {
                return false;
            }

            _context.Adocoes.Remove(adocao);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<string>> ValidarCriacaoAdocao(CriarAdocaoDto adocaoDto)
        {
            var erros = new List<string>();

            if (string.IsNullOrEmpty(adocaoDto.Data.ToString()) || adocaoDto.Data == DateTime.MinValue)
                erros.Add("O campo 'Data' é obrigatório!");

            if (string.IsNullOrEmpty(adocaoDto.AdotanteId.ToString()) || adocaoDto.AdotanteId <= 0)
                erros.Add("O campo 'Adotante' é obrigatório!");

            if (string.IsNullOrEmpty(adocaoDto.AnimalId.ToString()) || adocaoDto.AnimalId <= 0)
                erros.Add("O campo 'Animal' é obrigatório!");

            if (string.IsNullOrEmpty(adocaoDto.VoluntarioId.ToString()) || adocaoDto.VoluntarioId <= 0)
                erros.Add("O campo 'Voluntário' é obrigatório!");

            if (string.IsNullOrEmpty(adocaoDto.PontoAdocaoId.ToString()) || adocaoDto.PontoAdocaoId <= 0)
                erros.Add("O campo 'Ponto de Adoção' é obrigatório!");

            return erros;
        }

        public List<string> ValidarAtualizacaoAdocao(AtualizarAdocaoDto adocaoDto)
        {
            var erros = new List<string>();

            if (adocaoDto.Data != null && adocaoDto.Data == DateTime.MinValue && string.IsNullOrWhiteSpace(adocaoDto.Data.ToString()))
                erros.Add("O campo 'Data' não pode estar vazio!");

            if (adocaoDto.AdotanteId != null && string.IsNullOrWhiteSpace(adocaoDto.AdotanteId.ToString()))
                erros.Add("O campo 'Adotante' não pode ser menor ou igual a zero!");

            if (adocaoDto.AnimalId != null && string.IsNullOrWhiteSpace(adocaoDto.AnimalId.ToString()))
                erros.Add("O campo 'Animal' não pode ser menor ou igual a zero!");

            if (adocaoDto.VoluntarioId != null && string.IsNullOrWhiteSpace(adocaoDto.VoluntarioId.ToString()))
                erros.Add("O campo 'Voluntário' não pode ser menor ou igual a zero!");

            if (adocaoDto.PontoAdocaoId != null && string.IsNullOrWhiteSpace(adocaoDto.PontoAdocaoId.ToString()))
                erros.Add("O campo 'Ponto de Adoção' não pode ser menor ou igual a zero!");

            return erros;
        }

        #region MÉTODOS PRIVADOS

        private async Task<Adocao?> BuscarAdocaoPorId(int id)
        {
            var adocao = await _context.Adocoes.FindAsync(id);
            return adocao;
        }

        #endregion
    }
}