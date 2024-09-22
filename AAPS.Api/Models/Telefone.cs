using System.ComponentModel.DataAnnotations;

namespace AAPS.Api.Models;

public class Telefone
{
    public int Id { get; set; }
    [Required]
    public string NumeroTelefone { get; set; }
    [Required]
    public string Responsavel { get; set; }
    [Required]
    public int AdotanteId { get; set; }
    [Required]
    public int DoadorId { get; set; }
    [Required]
    public int PontoAdocaoId { get; set; }

    // Relacionamentos
    public Adotante Adotante { get; set; }
    public PontoAdocao PontoAdocao { get; set; }
    public Doador Doador { get; set; }
}
