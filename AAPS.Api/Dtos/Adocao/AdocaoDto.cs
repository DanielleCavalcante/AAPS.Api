namespace AAPS.Api.Dtos.Adocao
{
    public class AdocaoDto
    {
        public int Id { get; set; }
        public required DateTime Data { get; set; }

        public string NomeAdotante { get; set; }
        public required int AdotanteId { get; set; }

        public string NomeAnimal { get; set; }
        public required int AnimalId { get; set; }

        public string NomeVoluntario { get; set; }
        public required int VoluntarioId { get; set; }

        public string NomePontoAdocao { get; set; }
        public required int PontoAdocaoId { get; set; }
    }
}