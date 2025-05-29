using AAPS.Api.Dtos.TermoAdocao;

namespace AAPS.Api.Services.TermoAdocao
{
    public interface ITermoAdocaoService
    {
        //byte[] GerarPdf(TermoAdocaoDto dto);
        Task<byte[]> GerarPdf(int adocaoId); //TermoAdocaoDto dto
    }
}