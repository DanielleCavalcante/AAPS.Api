using AAPS.Api.Context;
using AAPS.Api.Dtos.Relatorio;
using AAPS.Api.Models.Enums;
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

        public DataTable[] ObterDadosRelatorio(FiltroRelatorioDto filtro)
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
                .Where(a => a.Adocoes.Any(ad => ad.Data >= filtro.DataInicio && ad.Data <= filtro.DataFim))
                .Include(a => a.Acompanhamentos)
                    .ThenInclude(ac => ac.Evento)
                .ToList();

            var resultado = new List<DataTable>();

            var incluirTodos = filtro.Tipo == null;

            if (filtro.Tipo == TipoRelatorio.Geral || incluirTodos)
            {
                var tabelaGeral = new DataTable("Adoções - Geral");
                tabelaGeral.Columns.Add("Categoria", typeof(string));
                tabelaGeral.Columns.Add("Quantidade", typeof(int));
                tabelaGeral.Rows.Add("Adotados", animaisFiltrados.Count());
                tabelaGeral.Rows.Add("Devolvidos", animaisFiltrados.Count(a =>
                    a.Acompanhamentos?.Any(ev =>
                        ev.Evento?.Descricao?.ToLower().Contains("devol") == true
                    ) == true
                ));
                resultado.Add(tabelaGeral);
            }

            if (filtro.Tipo == TipoRelatorio.PorEspecie || incluirTodos)
            {
                var tabelaEspecies = new DataTable("Adoções - Por Espécies");
                tabelaEspecies.Columns.Add("Espécie", typeof(string));
                tabelaEspecies.Columns.Add("Quantidade", typeof(int));
                var qtdGatos = animaisFiltrados.Count(a => a.Especie.ToLower().Trim() == "gato");
                var qtdCachorros = animaisFiltrados.Count(a => a.Especie.ToLower().Trim() == "cachorro");
                tabelaEspecies.Rows.Add("Gatos", qtdGatos);
                tabelaEspecies.Rows.Add("Cachorros", qtdCachorros);
                resultado.Add(tabelaEspecies);
            }

            if (filtro.Tipo == TipoRelatorio.BalancoMensal || incluirTodos)
            {
                var tabelaAdocoes = new DataTable("Adoções - Balanço Mensal");
                tabelaAdocoes.Columns.Add("Espécie", typeof(string));
                tabelaAdocoes.Columns.Add("Faixa Etária", typeof(string));
                tabelaAdocoes.Columns.Add("Quantidade", typeof(int));

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

                resultado.Add(tabelaAdocoes);

                var tabelaResgates = new DataTable("Resgates - Balanço Mensal");
                tabelaResgates.Columns.Add("Espécie", typeof(string));
                tabelaResgates.Columns.Add("Faixa Etária", typeof(string));
                tabelaResgates.Columns.Add("Quantidade", typeof(int));

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

                resultado.Add(tabelaResgates);
            }

            if (filtro.Tipo == TipoRelatorio.LevantamentoAnimais || incluirTodos)
            {
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

                resultado.Add(tabelaLevantamento);
            }

            if (resultado.Count == 0)
                throw new NotImplementedException($"Tipo de relatório '{filtro.Tipo}' não suportado.");

            return resultado.ToArray();

            //    switch (filtro.Tipo)
            //    {
            //        case TipoRelatorio.Geral:
            //            var tabelaGeral = new DataTable("Adoções - Geral");
            //            tabelaGeral.Columns.Add("Categoria", typeof(string));
            //            tabelaGeral.Columns.Add("Quantidade", typeof(int));
            //            tabelaGeral.Rows.Add("Adotados", animaisFiltrados.Count());
            //            tabelaGeral.Rows.Add("Devolvidos", animaisFiltrados.Count(a =>
            //                a.Acompanhamentos?.Any(ev =>
            //                    ev.Evento?.Descricao?.ToLower().Contains("devol") == true
            //                ) == true
            //            ));
            //            resultado.Add(tabelaGeral);
            //            break;

            //        case TipoRelatorio.PorEspecie:
            //            var tabelaEspecies = new DataTable("Adoções - Por Espécies");
            //            tabelaEspecies.Columns.Add("Espécie", typeof(string));
            //            tabelaEspecies.Columns.Add("Quantidade", typeof(int));
            //            var qtdGatos = animaisFiltrados.Count(a => a.Especie.ToLower().Trim() == "gato");
            //            var qtdCachorros = animaisFiltrados.Count(a => a.Especie.ToLower().Trim() == "cachorro");
            //            tabelaEspecies.Rows.Add("Gatos", qtdGatos);
            //            tabelaEspecies.Rows.Add("Cachorros", qtdCachorros);
            //            resultado.Add(tabelaEspecies);
            //            break;

            //        case TipoRelatorio.BalancoMensal:
            //            var tabelaAdocoes = new DataTable("Adoções - Balanço Mensal");
            //            tabelaAdocoes.Columns.Add("Espécie", typeof(string));
            //            tabelaAdocoes.Columns.Add("Faixa Etária", typeof(string));
            //            tabelaAdocoes.Columns.Add("Quantidade", typeof(int));

            //            var adocoesAgrupadas = animaisFiltrados
            //                .Select(a => new
            //                {
            //                    Especie = a.Especie.ToLower().Trim(),
            //                    FaixaEtaria = CalcularIdadeEmAnos(a.DataNascimento) < 1 ? "filhote" : "adulto"
            //                })
            //                .GroupBy(x => new { x.Especie, x.FaixaEtaria })
            //                .Select(g => new
            //                {
            //                    Especie = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(g.Key.Especie),
            //                    FaixaEtaria = g.Key.FaixaEtaria,
            //                    Quantidade = g.Count()
            //                });

            //            foreach (var item in adocoesAgrupadas)
            //            {
            //                tabelaAdocoes.Rows.Add(item.Especie, item.FaixaEtaria, item.Quantidade);
            //            }

            //            resultado.Add(tabelaAdocoes);

            //            var tabelaResgates = new DataTable("Resgates - Balanço Mensal");
            //            tabelaResgates.Columns.Add("Espécie", typeof(string));
            //            tabelaResgates.Columns.Add("Faixa Etária", typeof(string));
            //            tabelaResgates.Columns.Add("Quantidade", typeof(int));

            //            var resgatesAgrupados = animaisFiltrados
            //                .Where(a => a.Resgatado == true)
            //                .Select(a => new
            //                {
            //                    Especie = a.Especie.ToLower().Trim(),
            //                    FaixaEtaria = CalcularIdadeEmAnos(a.DataNascimento) < 1 ? "filhote" : "adulto"
            //                })
            //                .GroupBy(x => new { x.Especie, x.FaixaEtaria })
            //                .Select(g => new
            //                {
            //                    Especie = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(g.Key.Especie),
            //                    FaixaEtaria = g.Key.FaixaEtaria,
            //                    Quantidade = g.Count()
            //                });

            //            foreach (var item in resgatesAgrupados)
            //            {
            //                tabelaResgates.Rows.Add(item.Especie, item.FaixaEtaria, item.Quantidade);
            //            }

            //            resultado.Add(tabelaResgates);
            //            break;

            //        case TipoRelatorio.LevantamentoAnimais:
            //            var tabelaLevantamento = new DataTable("Levantamento de Animais");
            //            tabelaLevantamento.Columns.Add("Nome", typeof(string));
            //            tabelaLevantamento.Columns.Add("Espécie", typeof(string));
            //            tabelaLevantamento.Columns.Add("Raça", typeof(string));
            //            tabelaLevantamento.Columns.Add("Pelagem", typeof(string));
            //            tabelaLevantamento.Columns.Add("Sexo", typeof(string));
            //            tabelaLevantamento.Columns.Add("Idade", typeof(string));
            //            tabelaLevantamento.Columns.Add("Disponibilidade", typeof(string));
            //            tabelaLevantamento.Columns.Add("Resgatado", typeof(string));

            //            foreach (var animal in animaisFiltrados)
            //            {
            //                var idade = CalcularIdade(animal.DataNascimento);
            //                var resgatado = animal.Resgatado == true ? "Sim" : "Não";

            //                tabelaLevantamento.Rows.Add(animal.Nome, animal.Especie, animal.Raca, animal.Pelagem, animal.Sexo, idade, animal.Disponibilidade.ToString(), resgatado);
            //            }

            //            resultado.Add(tabelaLevantamento);
            //            break;

            //        default:
            //            throw new NotImplementedException($"Tipo de relatório '{filtro.Tipo}' não suportado.");
            //    }

            //return resultado.ToArray();
        }

        public byte[] GerarRelatorioPdf(FiltroRelatorioDto filtro)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var tabelas = ObterDadosRelatorio(filtro);
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
                        row.RelativeColumn(8).AlignMiddle().Text($"Relatório de {filtro.DataInicio:dd/MM/yyyy} até {filtro.DataFim:dd/MM/yyyy}")
                            .SemiBold()
                            .FontSize(20)
                            .FontColor(Colors.Black);
                    });

                    page.Content().Column(col =>
                    {
                        for (int i = 0; i < tabelas.Length; i++)
                        {
                            var tabela = tabelas[i];

                            // Só quebra de página se houver mais de uma tabela e for a de levantamento
                            if (i > 0 && tabela.TableName == "Levantamento de Animais")
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
                                        for (int j = 0; j < tabela.Columns.Count; j++)
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