namespace AAPS.Api.Dtos.Animais
{
    public class CriarAnimalDto
    {
        public required string Nome { get; set; }
        public required string Especie { get; set; }
        public required string Raca { get; set; }
        public required string Pelagem { get; set; }
        public required string Sexo { get; set; }
        public required DateTime DataNascimento { get; set; }
        public required bool Status { get; set; }
        public required int DoadorId { get; set; }
    }
}
