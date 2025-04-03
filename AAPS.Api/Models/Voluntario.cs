using Microsoft.AspNetCore.Identity;

namespace AAPS.Api.Models;

public class Voluntario : IdentityUser<int>
{
    public int PessoaId { get; set; }
    public Pessoa Pessoa { get; set; }
    public ICollection<Adocao>? Adocoes { get; set; } = new List<Adocao>();
}