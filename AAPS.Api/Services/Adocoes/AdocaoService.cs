using AAPS.Api.Context;
using AAPS.Api.Dtos.Acompanhamento;
using AAPS.Api.Dtos.Adocao;
using AAPS.Api.Models;
using AAPS.Api.Models.Enums;
using AAPS.Api.Services.Acompanhamentos;
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
        private readonly IAcompanhamentoService _acompanhamentoService;

        public AdocaoService(AppDbContext context, IAdotanteService adotanteService, IAnimalService animalService, IVoluntarioService voluntarioService, IPontoAdocaoService pontoAdocaoService, IAcompanhamentoService acompanhamentoService)
        {
            _context = context;
            _adotanteService = adotanteService;
            _animalService = animalService;
            _voluntarioService = voluntarioService;
            _pontoAdocaoService = pontoAdocaoService;
            _acompanhamentoService = acompanhamentoService;
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
                Cancelada = false,
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
                Cancelada = adocao.Cancelada,
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
                    a.Animal.Nome.Contains(buscaLower) ||
                    a.Voluntario.Pessoa.Nome.Contains(buscaLower) ||
                    a.PontoAdocao.NomeFantasia.Contains(buscaLower)
                );
            }

            if (filtro.Cancelada.HasValue)
            {
                query = query.Where(a => a.Cancelada == filtro.Cancelada.Value);
            }

            var adocoesDto = await query
                .Select(a => new AdocaoDto
                {
                    Id = a.Id,
                    Data = a.Data,
                    Cancelada = a.Cancelada,
                    NomeAdotante = a.Adotante.Pessoa.Nome,
                    AdotanteId = a.AdotanteId,
                    NomeAnimal = a.Animal.Nome,
                    AnimalId = a.AnimalId,
                    NomeVoluntario = a.Voluntario.Pessoa.Nome,
                    VoluntarioId = a.VoluntarioId,
                    NomePontoAdocao = a.PontoAdocao.NomeFantasia,
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
                Cancelada = adocao.Cancelada,
                AdotanteId = adocao.AdotanteId,
                NomeAdotante = adocao.Adotante.Pessoa.Nome,
                AnimalId = adocao.AnimalId,
                NomeAnimal = adocao.Animal.Nome,
                VoluntarioId = adocao.VoluntarioId,
                NomeVoluntario = adocao.Voluntario.Pessoa.Nome,
                PontoAdocaoId = adocao.PontoAdocaoId,
                NomePontoAdocao = adocao.PontoAdocao.NomeFantasia
            };
        }

        public async Task<AdocaoDto> AtualizarAdocao(int id, AtualizarAdocaoDto adocaoDto)
        {
            var adocao = await BuscarAdocaoPorId(id);
            if (adocao is null)
            {
                return null;
            }

            var animalAnteriorId = adocao.AnimalId;

            adocao.Data = adocaoDto.Data.HasValue ? adocaoDto.Data.Value : adocao.Data;
            //adocao.Cancelada = adocaoDto.Cancelada.HasValue ? adocaoDto.Cancelada.Value : adocao.Cancelada;
            adocao.AdotanteId = adocaoDto.AdotanteId.HasValue ? adocaoDto.AdotanteId.Value : adocao.AdotanteId;
            adocao.AnimalId = adocaoDto.AnimalId.HasValue ? adocaoDto.AnimalId.Value : adocao.AnimalId;
            adocao.VoluntarioId = adocaoDto.VoluntarioId.HasValue ? adocaoDto.VoluntarioId.Value : adocao.VoluntarioId;
            adocao.PontoAdocaoId = adocaoDto.PontoAdocaoId.HasValue ? adocaoDto.PontoAdocaoId.Value : adocao.PontoAdocaoId;

            if (adocao.AnimalId != animalAnteriorId)
            {
                var animalAnterior = await _context.Animais.FirstOrDefaultAsync(a => a.Id == animalAnteriorId);
                if (animalAnterior != null)
                {
                    animalAnterior.Disponibilidade = DisponibilidadeEnum.Disponivel;
                }
            }

            var animalAtual = await _context.Animais.FirstOrDefaultAsync(a => a.Id == adocao.AnimalId);

            animalAtual.Disponibilidade = DisponibilidadeEnum.Adotado;

            await _context.SaveChangesAsync();

            return new AdocaoDto
            {
                Id = adocao.Id,
                Data = adocao.Data,
                Cancelada = adocao.Cancelada,
                AdotanteId = adocao.AdotanteId,
                AnimalId = adocao.AnimalId,
                VoluntarioId = adocao.VoluntarioId,
                PontoAdocaoId = adocao.PontoAdocaoId
            };

            //var animalAtual = await _context.Animais
            //   .Include(a => a.AnimalEventos) // para acessar os eventos sem nova query
            //   .FirstOrDefaultAsync(a => a.Id == adocao.AnimalId);


            //if (animalAtual != null)
            //{
            //    if (adocaoDto.Devolvido == true)
            //    {
            //        animalAtual.Disponibilidade = DisponibilidadeEnum.Disponivel;
            //        animalAtual.Devolvido = DevolvidoEnum.Devolvido;
            //    }
            //    else
            //    {
            //        animalAtual.Disponibilidade = DisponibilidadeEnum.Adotado;
            //    }
            //}

            //if (animalAtual != null)
            //{
            //    var foiDevolvido = await _context.AnimalEventos
            //        .AnyAsync(e => e.AnimalId == animalAtual.Id && e.Evento.Trim().ToLower() == "devolvido");

            //    animalAtual.Disponibilidade = foiDevolvido
            //        ? DisponibilidadeEnum.Disponivel
            //        : DisponibilidadeEnum.Adotado;
            //}
        }

        //public async Task<bool> ExcluirAdocao(int id)
        //{
        //    var adocao = await BuscarAdocaoPorId(id);

        //    if (adocao == null)
        //    {
        //        return false;
        //    }

        //    _context.Adocoes.Remove(adocao);
        //    await _context.SaveChangesAsync();

        //    return true;
        //}

        public async Task<AdocaoDto> CancelarAdocao(int id, CancelarAdocaoDto cancelamentoDto)
        {
            var adocao = await BuscarAdocaoPorId(id);
            if (adocao == null) return null;

            adocao.Cancelada = true;

            var animal = await _context.Animais.FirstOrDefaultAsync(a => a.Id == adocao.AnimalId);
            if (animal != null)
            {
                animal.Disponibilidade = DisponibilidadeEnum.Disponivel;
            }

            var acompanhamentoDto = new CriarAcompanhamentoDto
            {
                Data = cancelamentoDto.DataAcompanhamento,
                Observacao = cancelamentoDto.Observacao,
                AnimalId = adocao.AnimalId,
                EventoId = cancelamentoDto.EventoId
            };

            await _acompanhamentoService.CriarAcompanhamento(acompanhamentoDto);

            var adotante = await _context.Adotantes.FirstOrDefaultAsync(a => a.Id == adocao.AdotanteId);
            if (adotante != null)
            {
                adotante.Bloqueio = BloqueioEnum.Bloqueado;
            }

            await _context.SaveChangesAsync();


            var teste = adotante;
            return new AdocaoDto
            {
                Id = adocao.Id,
                Data = adocao.Data,
                Cancelada = adocao.Cancelada,
                AdotanteId = adocao.AdotanteId,
                AnimalId = adocao.AnimalId,
                VoluntarioId = adocao.VoluntarioId,
                PontoAdocaoId = adocao.PontoAdocaoId
            };
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

        public async Task<List<string>> ValidarAtualizacaoAdocao(AtualizarAdocaoDto adocaoDto)
        {
            var erros = new List<string>();

            if (adocaoDto.Data != null && adocaoDto.Data == DateTime.MinValue && string.IsNullOrWhiteSpace(adocaoDto.Data.ToString()))
                erros.Add("O campo 'Data' não pode estar vazio!");
            //if (adocaoDto.Cancelada != null && string.IsNullOrWhiteSpace(adocaoDto.Cancelada.ToString()))
            //    erros.Add("O campo 'Cancelada' não pode ter ser vazio!");

            if (adocaoDto.AdotanteId != null)
            {
                if (string.IsNullOrWhiteSpace(adocaoDto.AdotanteId.ToString()) || adocaoDto.AdotanteId <= 0)
                {
                    erros.Add("O campo 'Adotante' não pode ser vazio!");
                }
                else
                {
                    var adotante = await _adotanteService.ObterAdotantePorId(adocaoDto.AdotanteId.Value);
                    if (adotante == null)
                        erros.Add("Adotante não encontrado.");
                    else if (adotante.Status == StatusEnum.Inativo)
                        erros.Add("Adotante está inativo.");
                    else if (adotante.Bloqueio == BloqueioEnum.Bloqueado)
                        erros.Add("Adotante está bloqueado.");
                }
            }

            if (adocaoDto.AnimalId != null)
            {
                if (string.IsNullOrWhiteSpace(adocaoDto.AnimalId.ToString()) || adocaoDto.AnimalId <= 0)
                {
                    erros.Add("O campo 'Animal' não pode ser vazio!");
                }
                else
                {
                    var animal = await _context.Animais.FirstOrDefaultAsync(a => a.Id == adocaoDto.AnimalId.Value);
                    if (animal == null)
                        erros.Add("Animal não encontrado.");
                    else if (animal.Status == StatusEnum.Inativo)
                        erros.Add("Animal está inativo.");
                    //else if (animal.Disponibilidade == DisponibilidadeEnum.Adotado)
                    //    erros.Add("Animal já está adotado.");
                }
            }

            if (adocaoDto.VoluntarioId != null)
            {
                if (string.IsNullOrWhiteSpace(adocaoDto.VoluntarioId.ToString()) || adocaoDto.VoluntarioId <= 0)
                {
                    erros.Add("O campo 'Voluntário' não pode ser vazio!");
                }
                else
                {
                    var voluntario = await _voluntarioService.ObterVoluntarioPorId(adocaoDto.VoluntarioId.Value);
                    if (voluntario == null)
                        erros.Add("Voluntário não encontrado.");
                    else if (voluntario.Status == StatusEnum.Inativo)
                        erros.Add("Voluntário está inativo.");
                }
            }

            if (adocaoDto.PontoAdocaoId != null)
            {
                if (string.IsNullOrWhiteSpace(adocaoDto.PontoAdocaoId.ToString()) || adocaoDto.PontoAdocaoId <= 0)
                {
                    erros.Add("O campo 'Ponto de Adoção' não pode ser vazio!");
                }
                else
                {
                    var pontoAdocao = await _pontoAdocaoService.ObterPontoAdocaoPorId(adocaoDto.PontoAdocaoId.Value);
                    if (pontoAdocao == null)
                        erros.Add("Ponto de adoção não encontrado.");
                    else if (pontoAdocao.Status == StatusEnum.Inativo)
                        erros.Add("Ponto de adoção está inativo.");
                }
            }

            return erros;
        }

        #region MÉTODOS PRIVADOS

        private async Task<Adocao?> BuscarAdocaoPorId(int id)
        {
            return await _context.Adocoes
                .Include(a => a.Adotante)
                    .ThenInclude(a => a.Pessoa)
                .Include(a => a.Animal)
                .Include(a => a.Voluntario)
                    .ThenInclude(a => a.Pessoa)
                .Include(a => a.PontoAdocao)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        #endregion
    }
}