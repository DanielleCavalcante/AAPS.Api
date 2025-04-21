using AAPS.Api.Context;
using AAPS.Api.Dtos.Relatorio;
using AAPS.Api.Models.Enums;
using AAPS.Api.Services.Animais;
using System.Data;
using System.Globalization;

namespace AAPS.Api.Services.Relatorios
{
    public class RelatorioService : IRelatorioService
    {
        #region ATRIBUTOS E CONSTRUTOR

        private readonly AppDbContext _context;

        public RelatorioService(AppDbContext context)
        {
            _context = context;
        }

        #endregion

        public DataTable[] ObterDadosRelatorio(FiltroRelatorioDto filtro)
        {
            var dataInicio = new DateTime(filtro.Ano, filtro.Mes, 1);

            //var ultimoDiaDoMes = new DateTime(filtro.Ano, filtro.Mes, DateTime.DaysInMonth(filtro.Ano, filtro.Mes));
            //var dataFim = DateTime.Now.Date < ultimoDiaDoMes ? DateTime.Now.Date : ultimoDiaDoMes; - somente o mes informado

            var dataFim = DateTime.Now.Date; // data atual

            var query = _context.Animais.AsQueryable();

            if (!string.IsNullOrEmpty(filtro.Especie))
            {
                query = query.Where(a => a.Especie.ToLower().Trim() == filtro.Especie.ToLower().Trim());
            }

            if (!string.IsNullOrEmpty(filtro.FaixaEtaria))
            {
                var dataLimite = DateTime.Now.AddYears(-1);

                if (filtro.FaixaEtaria.ToLower() == "filhote")
                    query = query.Where(a => a.DataNascimento > dataLimite);
                else if (filtro.FaixaEtaria.ToLower() == "adulto")
                    query = query.Where(a => a.DataNascimento <= dataLimite);
            }

            var animaisFiltrados = query
                .Where(a => a.Adocoes.Any(ad => ad.Data >= dataInicio && ad.Data <= dataFim))
                .ToList();

            // tabelas a serem exibidas:

            var table1 = new DataTable("Adoções - Geral");
            table1.Columns.Add("Categoria", typeof(string));
            table1.Columns.Add("Quantidade", typeof(int));
            table1.Rows.Add("Adotados", animaisFiltrados.Count());
            //table1.Rows.Add("Devolvidos", animaisFiltrados.Where(a => a.Devolvido == DevolvidoEnum.Devolvido).Count());
            table1.Rows.Add("Devolvidos", animaisFiltrados.Count(a => a.Acompanhamentos.Any(ev => ev.Evento.Descricao.ToLower().Contains("devol"))));

            var table2 = new DataTable("Adoções - Por Espécies");
            table2.Columns.Add("Espécie", typeof(string));
            table2.Columns.Add("Quantidade", typeof(int));
            var qtdGatos = animaisFiltrados.Count(a => a.Especie.ToLower().Trim() == "gato");
            var qtdCachorros = animaisFiltrados.Count(a => a.Especie.ToLower().Trim() == "cachorro");
            table2.Rows.Add("Gatos", qtdGatos);
            table2.Rows.Add("Cachorros", qtdCachorros);

            var table3 = new DataTable("Levantamento de Animais");
            table3.Columns.Add("Nome", typeof(string));
            table3.Columns.Add("Espécie", typeof(string));
            table3.Columns.Add("Raça", typeof(string));
            table3.Columns.Add("Pelagem", typeof(string));
            table3.Columns.Add("Sexo", typeof(string));
            table3.Columns.Add("Idade", typeof(string));
            table3.Columns.Add("Disponibilidade", typeof(string));
            //table3.Columns.Add("Resgatado por", typeof(string));

            foreach (var animal in animaisFiltrados)
            {
                var idade = CalcularIdade(animal.DataNascimento);
                //var resgatadoPor = animal.Pessoa?.EhVoluntario == true ? "Voluntário" : "Externo";

                table3.Rows.Add(animal.Nome, animal.Especie, animal.Raca, animal.Pelagem, animal.Sexo, idade, animal.Disponibilidade.ToString());
            }

            //var table4 = new DataTable("Balanço Mensal");
            //table4.Columns.Add("Categoria", typeof(string));
            //table4.Columns.Add("Espécie", typeof(string));
            //table4.Columns.Add("Faixa Etária", typeof(string));
            //table4.Columns.Add("Quantidade", typeof(int));

            //// Adoções agrupadas por espécie e faixa etária
            //var adocoesAgrupadas = animaisFiltrados
            //    .Select(a => new
            //    {
            //        Especie = a.Especie.ToLower().Trim(),
            //        FaixaEtaria = CalcularIdadeEmAnos(a.DataNascimento) < 1 ? "filhote" : "adulto"
            //    })
            //    .GroupBy(x => new { x.Especie, x.FaixaEtaria })
            //    .Select(g => new
            //    {
            //        Categoria = "Adoções",
            //        Especie = g.Key.Especie,
            //        FaixaEtaria = g.Key.FaixaEtaria,
            //        Quantidade = g.Count()
            //    });

            //// Adicionando as adoções ao balanço mensal
            //foreach (var item in adocoesAgrupadas)
            //{
            //    table4.Rows.Add(item.Categoria, CultureInfo.CurrentCulture.TextInfo.ToTitleCase(item.Especie), item.FaixaEtaria, item.Quantidade);
            //}

            //// Resgates agrupados por espécie e faixa etária
            //var resgatesAgrupados = animaisFiltrados
            //    //.Where(a => a.Pessoa != null && a.Pessoa.EhDoador == true)
            //    .Select(a => new
            //    {
            //        Especie = a.Especie.ToLower().Trim(),
            //        FaixaEtaria = CalcularIdadeEmAnos(a.DataNascimento) < 1 ? "filhote" : "adulto"
            //    })
            //    .GroupBy(x => new { x.Especie, x.FaixaEtaria })
            //    .Select(g => new
            //    {
            //        Categoria = "Resgates",
            //        Especie = g.Key.Especie,
            //        FaixaEtaria = g.Key.FaixaEtaria,
            //        Quantidade = g.Count()
            //    });

            //// Adicionando os resgates ao balanço mensal
            //foreach (var item in resgatesAgrupados)
            //{
            //    table4.Rows.Add(item.Categoria, CultureInfo.CurrentCulture.TextInfo.ToTitleCase(item.Especie), item.FaixaEtaria, item.Quantidade);
            //}

            var tabelaAdoções = new DataTable("Adoções");
            tabelaAdoções.Columns.Add("Espécie", typeof(string));
            tabelaAdoções.Columns.Add("Faixa Etária", typeof(string));
            tabelaAdoções.Columns.Add("Quantidade", typeof(int));

            var tabelaResgates = new DataTable("Resgates");
            tabelaResgates.Columns.Add("Espécie", typeof(string));
            tabelaResgates.Columns.Add("Faixa Etária", typeof(string));
            tabelaResgates.Columns.Add("Quantidade", typeof(int));

            var adocoesAgrupadas = animaisFiltrados
                .Select(a => new
                {
                    Especie = a.Especie.ToLower().Trim(),
                    FaixaEtaria = CalcularIdadeEmAnos(a.DataNascimento) < 1 ? "filhote" : "adulto"
                })
                .GroupBy(x => new { x.Especie, x.FaixaEtaria })
                .Select(g => new
                {
                    Especie = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(g.Key.Especie),
                    FaixaEtaria = g.Key.FaixaEtaria,
                    Quantidade = g.Count()
                });

            foreach (var item in adocoesAgrupadas)
            {
                tabelaAdoções.Rows.Add(item.Especie, item.FaixaEtaria, item.Quantidade);
            }

            var resgatesAgrupados = animaisFiltrados
                .Where(a => a.Pessoa != null && a.Pessoa.Cpf == "11122233344") // .EVoluntario == true
                .Select(a => new
                {
                    Especie = a.Especie.ToLower().Trim(),
                    FaixaEtaria = CalcularIdadeEmAnos(a.DataNascimento) < 1 ? "filhote" : "adulto"
                })
                .GroupBy(x => new { x.Especie, x.FaixaEtaria })
                .Select(g => new
                {
                    Especie = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(g.Key.Especie),
                    FaixaEtaria = g.Key.FaixaEtaria,
                    Quantidade = g.Count()
                });

            foreach (var item in resgatesAgrupados)
            {
                tabelaResgates.Rows.Add(item.Especie, item.FaixaEtaria, item.Quantidade);
            }

            return new DataTable[] { table1, table2, table3, tabelaAdoções, tabelaResgates };
        }

        #region MÉTODOS PRIVADOS

        private string CalcularIdade(DateTime? dataNascimento)
        {
            if (!dataNascimento.HasValue) return "Não informado";

            var hoje = DateTime.Now;
            var idadeAnos = hoje.Year - dataNascimento.Value.Year;
            if (dataNascimento.Value.Date > hoje.AddYears(-idadeAnos)) idadeAnos--;

            var idadeMeses = ((hoje.Year - dataNascimento.Value.Year) * 12) + hoje.Month - dataNascimento.Value.Month;
            if (dataNascimento.Value.Day > hoje.Day) idadeMeses--;

            if (idadeAnos == 0)
                return idadeMeses == 1 ? "1 mês" : $"{idadeMeses} meses";

            return idadeAnos == 1 ? "1 ano" : $"{idadeAnos} anos";
        }

        private int CalcularIdadeEmAnos(DateTime? dataNascimento)
        {
            if (!dataNascimento.HasValue) return 0;

            var hoje = DateTime.Today;
            var idade = hoje.Year - dataNascimento.Value.Year;

            if (dataNascimento.Value.Date > hoje.AddYears(-idade)) idade--;

            return idade;
        }

        #endregion
    }
}