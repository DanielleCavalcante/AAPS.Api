namespace AAPS.Api.Dtos.Adocao
{
    public class CriarAdocaoDto
    {
        public DateTime Data { get; set; } = DateTime.MinValue;
        public bool Cancelada { get; set; } = false;
        public int AdotanteId { get; set; } = 0;
        public int AnimalId { get; set; } = 0;
        public int VoluntarioId { get; set; } = 0;
        public int PontoAdocaoId { get; set; } = 0;
    }
}