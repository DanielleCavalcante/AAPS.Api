using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations;

namespace AAPS.Api.Dtos;

public class AdotanteDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Rg { get; set; }
    public string Cpf { get; set; }
    public string LocalTrabalho { get; set; }
    public bool Status { get; set; }
    public string Facebook { get; set; }
    public string Instagram { get; set; }
    public string Logradouro { get; set; }
    public int Numero { get; set; }
    public string Complemento { get; set; }
    public string Bairro { get; set; }
    public string Uf { get; set; }
    public string Cidade { get; set; }
    public int Cep { get; set; }
    public string SituacaoEndereco { get; set; }

}
