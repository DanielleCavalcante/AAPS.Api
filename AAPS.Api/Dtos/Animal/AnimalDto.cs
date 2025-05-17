using AAPS.Api.Models.Enums;

namespace AAPS.Api.Dtos.Animal;

public class AnimalDto
{
    public int Id { get; set; } 
    public required string Nome { get; set; }
    public required string Especie { get; set; }
    public required string Raca { get; set; }
    public required string Pelagem { get; set; }
    public required string Sexo { get; set; }
    public DateTime? DataNascimento { get; set; }
    public required StatusEnum Status { get; set; }
    public required DisponibilidadeEnum Disponibilidade { get; set; }
    public required bool Resgatado { get; set; }
    public required int DoadorId { get; set; }
    public string NomeDoador { get; set; }
    //public List<string> Telefones { get; set; }
}