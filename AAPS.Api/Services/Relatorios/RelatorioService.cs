using AAPS.Api.Context;
using AAPS.Api.Dtos.Relatorio;
using AAPS.Api.Models.Enums;
using AAPS.Api.Services.Animais;
using System.Data;

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

            var animaisFiltrados = query
                .Where(a => a.Adocoes.Any(ad => ad.Data >= dataInicio && ad.Data <= dataFim))
                .ToList();

            var table1 = new DataTable("Adoções - Geral");
            table1.Columns.Add("Categoria", typeof(string));
            table1.Columns.Add("Quantidade", typeof(int));
            table1.Rows.Add("Adotados", animaisFiltrados.Count());
            table1.Rows.Add("Devolvidos", animaisFiltrados.Where(a => a.Devolvido == DevolvidoEnum.Devolvido).Count());

            var table2 = new DataTable("Adoções - Por Espécies");
            table2.Columns.Add("Espécie", typeof(string));
            table2.Columns.Add("Quantidade", typeof(int));
            var qtdGatos = animaisFiltrados.Count(a => a.Especie.ToLower().Trim() == "gato");
            var qtdCachorros = animaisFiltrados.Count(a => a.Especie.ToLower().Trim() == "cachorro");
            table2.Rows.Add("Gato", qtdGatos);
            table2.Rows.Add("Cachorro", qtdCachorros);

            var table3 = new DataTable("Levantamento de Animais");
            table3.Columns.Add("Nome", typeof(string));
            table3.Columns.Add("Espécie", typeof(string));
            table3.Columns.Add("Raça", typeof(string));
            table3.Columns.Add("Pelagem", typeof(string));
            table3.Columns.Add("Sexo", typeof(string));
            table3.Columns.Add("Idade", typeof(string));
            table3.Columns.Add("Disponibilidade", typeof(string));

            foreach (var animal in animaisFiltrados)
            {
                var idade = CalcularIdade(animal.DataNascimento);
                table3.Rows.Add(animal.Nome, animal.Especie, animal.Raca, animal.Pelagem, animal.Sexo, idade, animal.Disponibilidade.ToString());
            }

            return new DataTable[] { table1, table2, table3 };
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

        #endregion
    }
}