using System.ComponentModel.DataAnnotations;

namespace AAPS.Api.Models;

public class Adocao
{
    public int Id { get; set; } 
    public DateTime DataAdocao { get; set; } = DateTime.MinValue;
    public int AdotanteId { get; set; } = 0;
    public int AnimalId { get; set; } = 0;
    public int VoluntarioId { get; set; } = 0;
    public int PontoAdocaoId { get; set; } = 0;

    // Relacionamentos
    public Animal Animal { get; set; }
    public Voluntario Voluntario { get; set; }
    public Adotante Adotante { get; set; }
    public PontoAdocao PontoAdocao { get; set; }
}
