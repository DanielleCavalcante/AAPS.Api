namespace AAPS.Api.Models;

public class Telefone
{
    public int Id { get; set; }
    public string NumeroTelefone { get; set; } = string.Empty;
    public string Responsavel { get; set; } = string.Empty; // se já tem aqui precisa em ponto de adoção?
    public int AdotanteId { get; set; }
    public int DoadorId { get; set; }
    public int PontoAdocaoId { get; set; }

    public Adotante Adotante { get; set; } = new Adotante();
    public PontoAdocao PontoAdocao { get; set; } = new PontoAdocao();
    public Doador Doador { get; set; } = new Doador();
}
