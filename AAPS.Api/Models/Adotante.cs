using AAPS.Api.Models.Enums;

namespace AAPS.Api.Models;

public class Adotante
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string LocalTrabalho { get; set; }
    public string? Facebook { get; set; }
    public string? Instagram { get; set; }
    public BloqueioEnum Bloqueio { get; set; } = BloqueioEnum.Desbloquado;
    public string ObservacaoBloqueio { get; set; }

    public int PessoaId { get; set; }
    public Pessoa Pessoa { get; set; }
    public ICollection<Adocao> Adocoes { get; set; } = new List<Adocao>();
}