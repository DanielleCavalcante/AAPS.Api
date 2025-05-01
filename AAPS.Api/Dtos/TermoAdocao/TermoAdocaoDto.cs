namespace AAPS.Api.Dtos.TermoAdocao
{
    public class TermoAdocaoDto
    {
        public string Nome { get; set; }
        public string Rg { get; set; }
        public string Cpf { get; set; }
        public string Endereco { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Telefone1 { get; set; }
        public string Telefone2 { get; set; }
        public string LocalDeTrabalho { get; set; }
        public string TipoResidencia { get; set; } // Casa / Apto
        public string ImovelProprio { get; set; } // Própria / Alugada
        public string Facebook { get; set; }
        public string Instagram { get; set; }

        public string TipoAnimal { get; set; } // Cão / Gato
        public string Cor { get; set; }
        public string Sexo { get; set; } // F / M
        public int Idade { get; set; }
        public bool Vermifugado { get; set; }
        public bool Vacinado { get; set; }
        public bool Castrado { get; set; }

        public string DoadorNome { get; set; }
        public string DoadorTelefone { get; set; }

        public string PontoAdocao { get; set; }

        public string Assinatura { get; set; } = "__________________________________________";
    }
}