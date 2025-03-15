using AAPS.Api.Dtos;
using AAPS.Api.Models;

namespace AAPS.Api.Services.Interfaces;
    public interface IPontoAdocaoService
    {
        Task<IEnumerable<PontoAdocao>> ObterPontosAdocao();
        Task<PontoAdocao> ObterPontoAdocaoPorId(int id);
        Task<IEnumerable<PontoAdocao>> ObterPontoAdocaoPorNome(string nome);
        Task CriarPontoAdocao(PontoAdocaoDto pontoAdocaoDto);
        Task AtualizarPontoAdocao(int id, PontoAdocaoDto pontoAdocaoDto);
        Task ExcluirPontoAdocao(int id);
    }
