namespace AAPS.Api.Models;

public class Adocao
{
    public int Id { get; set; }
    public DateTime DataAdocao { get; set; }

    // Relacionamentos
    public Animal Animal { get; set; }
    public Usuario Usuario { get; set; }
    public Adotante Adotante { get; set; }
    public PontoAdocao PontoAdocao { get; set; }
}
