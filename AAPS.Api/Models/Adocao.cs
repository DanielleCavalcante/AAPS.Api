namespace AAPS.Api.Models;

public class Adocao
{
    public int Id { get; set; }
    public DateTime Data { get; set; } = DateTime.MinValue;
    public int AdotanteId { get; set; }
    public int AnimalId { get; set; }
    public int VoluntarioId { get; set; }
    public int PontoAdocaoId { get; set; }

    public Animal Animal { get; set; } = new Animal();
    public Voluntario Voluntario { get; set; } = new Voluntario();
    public Adotante Adotante { get; set; } = new Adotante();
    public PontoAdocao PontoAdocao { get; set; } = new PontoAdocao();
}
