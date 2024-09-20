namespace AAPS.Api.Models;

public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public string Senha { get; set; }
    public char Nivel { get; set; } // TODO: rever - pode ser int e indicar com 1 - admin, 2 - default
    public string Status { get; set; }

    // Relacionamentos
    public ICollection<Adocao> Adocoes { get; set; }
}
