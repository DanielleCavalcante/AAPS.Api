using AAPS.Api.Context;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
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

        public DataTable[] ObterDadosRelatorio(DateTime dataInicio, DateTime dataFim)
        {
            //var partesInicio = filtro.DataInicio.Split('/');
            //int mesInicio = int.Parse(partesInicio[0]);
            //int anoInicio = int.Parse(partesInicio[1]);
            //var dataInicio = new DateTime(anoInicio, mesInicio, 1);

            //var partesFim = filtro.DataFim.Split('/');
            //int mesFim = int.Parse(partesFim[0]);
            //int anoFim = int.Parse(partesFim[1]);
            //var ultimoDiaFim = DateTime.DaysInMonth(anoFim, mesFim);
            //var dataFim = new DateTime(anoFim, mesFim, ultimoDiaFim);

            //var ultimoDiaDoMes = new DateTime(filtro.Ano, filtro.Mes, DateTime.DaysInMonth(filtro.Ano, filtro.Mes));
            //var dataFim = DateTime.Now.Date < ultimoDiaDoMes ? DateTime.Now.Date : ultimoDiaDoMes; - somente o mes informado

            //var dataInicio = new DateTime(filtro.Ano, filtro.Mes, 1);
            //var dataFim = DateTime.Now.Date; // data atual

            var query = _context.Animais.AsQueryable();

            var animaisFiltrados = query
                .Where(a => a.Adocoes.Any(ad => ad.Data >= dataInicio && ad.Data <= dataFim))
                .Include(a => a.Acompanhamentos)
                    .ThenInclude(ac => ac.Evento)
                .ToList();

            var tabelaGeral = new DataTable("Adoções - Geral");
            tabelaGeral.Columns.Add("Categoria", typeof(string));
            tabelaGeral.Columns.Add("Quantidade", typeof(int));
            tabelaGeral.Rows.Add("Adotados", animaisFiltrados.Count());
            tabelaGeral.Rows.Add("Devolvidos", animaisFiltrados.Count(a =>
                a.Acompanhamentos?.Any(ev =>
                    ev.Evento?.Descricao?.ToLower().Contains("devol") == true
                ) == true
            ));

            var tabelaEspecies = new DataTable("Adoções - Por Espécies");
            tabelaEspecies.Columns.Add("Espécie", typeof(string));
            tabelaEspecies.Columns.Add("Quantidade", typeof(int));
            var qtdGatos = animaisFiltrados.Count(a => a.Especie.ToLower().Trim() == "gato");
            var qtdCachorros = animaisFiltrados.Count(a => a.Especie.ToLower().Trim() == "cachorro");
            tabelaEspecies.Rows.Add("Gatos", qtdGatos);
            tabelaEspecies.Rows.Add("Cachorros", qtdCachorros);

            var tabelaLevantamento = new DataTable("Levantamento de Animais");
            tabelaLevantamento.Columns.Add("Nome", typeof(string));
            tabelaLevantamento.Columns.Add("Espécie", typeof(string));
            tabelaLevantamento.Columns.Add("Raça", typeof(string));
            tabelaLevantamento.Columns.Add("Pelagem", typeof(string));
            tabelaLevantamento.Columns.Add("Sexo", typeof(string));
            tabelaLevantamento.Columns.Add("Idade", typeof(string));
            tabelaLevantamento.Columns.Add("Disponibilidade", typeof(string));
            tabelaLevantamento.Columns.Add("Resgatado", typeof(string));

            foreach (var animal in animaisFiltrados)
            {
                var idade = CalcularIdade(animal.DataNascimento);
                var resgatado = animal.Resgatado == true ? "Sim" : "Não";

                tabelaLevantamento.Rows.Add(animal.Nome, animal.Especie, animal.Raca, animal.Pelagem, animal.Sexo, idade, animal.Disponibilidade.ToString(), resgatado);
            }

            var tabelaAdocoes = new DataTable("Adoções - Balanço Mensal");
            tabelaAdocoes.Columns.Add("Espécie", typeof(string));
            tabelaAdocoes.Columns.Add("Faixa Etária", typeof(string));
            tabelaAdocoes.Columns.Add("Quantidade", typeof(int));

            var tabelaResgates = new DataTable("Resgates - Balanço Mensal");
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
                tabelaAdocoes.Rows.Add(item.Especie, item.FaixaEtaria, item.Quantidade);
            }

            var resgatesAgrupados = animaisFiltrados
                .Where(a => a.Resgatado == true)
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

            return new DataTable[] { tabelaGeral, tabelaEspecies, tabelaAdocoes, tabelaResgates, tabelaLevantamento };
        }

        public byte[] GerarRelatorioPdf(DateTime dataInicio, DateTime dataFim)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var tabelas = ObterDadosRelatorio(dataInicio, dataFim);

            var caminhoLogo = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "aaps_logo.png");

            var documento = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(50);
                    page.Size(PageSizes.A4);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Row(row =>
                    {
                        row.RelativeColumn(1)
                        .Container()
                        .Height(40)
                        .Image(caminhoLogo);
                        row.RelativeColumn(8).AlignMiddle().Text($"Relatório de {dataInicio:dd/MM/yyyy} até {dataFim:dd/MM/yyyy}")
                            .SemiBold()
                            .FontSize(20)
                            .FontColor(Colors.Black);
                    });

                    page.Content().Column(col =>
                    {
                        foreach (var tabela in tabelas)
                        {
                            if (tabela.TableName == "Levantamento de Animais")
                            {
                                col.Item().PageBreak();
                            }

                            col.Item()
                            .Container()
                            .PaddingBottom(20)
                            .PaddingTop(20)
                            .Text(tabela.TableName)
                            .Bold()
                            .FontSize(16);

                            col.Item()
                                .Container()
                                .PaddingTop(5)
                                .PaddingBottom(15)
                                .Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        for (int i = 0; i < tabela.Columns.Count; i++)
                                        {
                                            columns.RelativeColumn();
                                        }
                                    });

                                    table.Header(header =>
                                    {
                                        foreach (DataColumn coluna in tabela.Columns)
                                        {
                                            header.Cell().Element(CellStyle).Text(coluna.ColumnName).SemiBold();
                                        }
                                    });

                                    foreach (DataRow linha in tabela.Rows)
                                    {
                                        foreach (var valor in linha.ItemArray)
                                        {
                                            table.Cell().Element(CellStyle).Text(valor?.ToString() ?? "");
                                        }
                                    }

                                    IContainer CellStyle(IContainer container) =>
                                        container
                                            .Border(1)
                                            .BorderColor(Colors.Grey.Lighten2)
                                            .Padding(5);
                                });
                        }
                    });
                });
            });

            return documento.GeneratePdf();
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