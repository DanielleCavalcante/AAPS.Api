namespace AAPS.Api.Dtos.Animais
{
    public class CriarAnimalDto
    {
        public required string Nome { get; set; } = string.Empty;
        public required string Especie { get; set; } = string.Empty;
        public required string Raca { get; set; } = string.Empty;
        public required string Pelagem { get; set; } = string.Empty;
        public required string Sexo { get; set; } = string.Empty;
        public required DateTime DataNascimento { get; set; } = DateTime.Now;
        public required bool Status { get; set; } = true;
        public required int DoadorId { get; set; } = 0;
    }
}
