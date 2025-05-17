namespace AAPS.Api.Dtos.Adocao
{
    public class AtualizarAdocaoDto
    {
        public DateTime? Data { get; set; }
        public bool? Cancelada { get; set; }
        public int? AdotanteId { get; set; }
        public int? AnimalId { get; set; }
        public int? VoluntarioId { get; set; }
        public int? PontoAdocaoId { get; set; }
    }
}