namespace AAPS.Api.Dtos.Animais
{
    public class FiltroAnimalDto
    {
        public string? Busca { get; set; }
        public string? Especie { get; set; }
        public string? Sexo { get; set; }
        public bool? Status { get; set; }
    }
}
