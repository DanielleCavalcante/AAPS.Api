namespace AAPS.Api.Dtos.Adocao
{
    public class AdocaoCompletaDto
    {
        public int Id { get; set; }
        public required DateTime Data { get; set; }
        public required bool Cancelada { get; set; }

        public required int VoluntarioId { get; set; }
        public string NomeVoluntario { get; set; }

        public required int AdotanteId { get; set; }
        public string NomeAdotante { get; set; }
        public string RgAdotante { get; set; }
        public string CpfAdotante { get; set; }
        public string CelularAdotante { get; set; }

        public required int AnimalId { get; set; }
        public string NomeAnimal { get; set; }
        public string EspecieAnimal { get; set; }
        public string IdadeAnimal { get; set; }
        public string SexoAnimal { get; set; }
        public string PelagemAnimal { get; set; }
        public int DoadorId { get; set; }
        public string NomeDoador { get; set; }
        public string TelefoneDoador { get; set; }

        public required int PontoAdocaoId { get; set; }
        public string NomePontoAdocao { get; set; }
    }
}