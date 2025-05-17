namespace AAPS.Api.Models;

public class Adocao
{
    public int Id { get; set; }
    public DateTime Data { get; set; } = DateTime.MinValue;
    public bool Cancelada { get; set; }

    public int AdotanteId { get; set; }
    public int AnimalId { get; set; }
    public int VoluntarioId { get; set; }
    public int PontoAdocaoId { get; set; }

    public Animal Animal { get; set; }
    public Adotante Adotante { get; set; }
    public PontoAdocao PontoAdocao { get; set; }
    public Voluntario Voluntario { get; set; }
}