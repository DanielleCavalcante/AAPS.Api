namespace AAPS.Api.Models;

public class Telefone
{
    public int Id { get; set; }
    public string NumeroTelefone { get; set; }
    //public string Responsavel { get; set; }

    public int PessoaId { get; set; }
    public Pessoa Pessoa { get; set; }
}