namespace AAPS.Api.Models;

public class Telefone
{
    public int Id { get; set; }
    public string NumeroTelefone { get; set; }
    public string Responsavel { get; set; }
    public int AdotanteId { get; set; }
    public int DoadorId { get; set; }
    public int PontoAdocaoId { get; set; }

    public Adotante Adotante { get; set; }
    public PontoAdocao PontoAdocao { get; set; }
    public Doador Doador { get; set; }
}