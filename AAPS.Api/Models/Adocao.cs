using System.ComponentModel.DataAnnotations;

namespace AAPS.Api.Models;

public class Adocao
{
    public int Id { get; set; }
    [Required]
    public DateTime DataAdocao { get; set; }
    [Required]
    public int AdotanteId { get; set; }
    [Required]
    public int AnimalId { get; set; }
    [Required]
    public int UsuarioId { get; set; }
    [Required]
    public int PontoAdocaoId { get; set; }

    // Relacionamentos
    public Animal Animal { get; set; }
    public Usuario Usuario { get; set; }
    public Adotante Adotante { get; set; }
    public PontoAdocao PontoAdocao { get; set; }
}
