namespace AAPS.Api.Models;

public class PontoAdocao
{
    public int Id { get; set; }
    public string NomeFantasia { get; set; }
    public string Responsavel { get; set; }
    public string Cnpj { get; set; }

    public int PessoaId { get; set; }
    public Pessoa Pessoa { get; set; }
    public ICollection<Adocao>? Adocoes { get; set; } = new List<Adocao>();
}