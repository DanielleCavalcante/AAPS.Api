using System.ComponentModel.DataAnnotations;

namespace AAPS.Api.Models;

public class Telefone
{
    public int Id { get; set; }
    public string NumeroTelefone { get; set; } = string.Empty;
    public string Responsavel { get; set; } = string.Empty;
    public int AdotanteId { get; set; } = 0;
    public int DoadorId { get; set; } = 0;
    public int PontoAdocaoId { get; set; } = 0;

    // Relacionamentos
    public Adotante Adotante { get; set; }
    public PontoAdocao PontoAdocao { get; set; }
    public Doador Doador { get; set; }
}
