using AAPS.Api.Models.Enums;

namespace AAPS.Api.Dtos.Animal
{
    public class AtualizarAnimalDto
    {
        public string? Nome { get; set; }
        public string? Especie { get; set; }
        public string? Raca { get; set; }
        public string? Pelagem { get; set; }
        public string? Sexo { get; set; }
        public DateTime? DataNascimento { get; set; }
        public StatusEnum? Status { get; set; }
        public DisponibilidadeEnum? Disponibilidade { get; set; }
        public bool? Resgatado { get; set; }
        public int? DoadorId { get; set; }
    }
}