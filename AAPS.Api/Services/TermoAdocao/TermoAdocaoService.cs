using AAPS.Api.Services.Acompanhamentos;
using AAPS.Api.Services.Adocoes;
using AAPS.Api.Services.Adotantes;
using AAPS.Api.Services.Animais;
using AAPS.Api.Services.Doadores;
using AAPS.Api.Services.PontosAdocao;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace AAPS.Api.Services.TermoAdocao
{
    public class TermoAdocaoService : ITermoAdocaoService
    {
        #region ATRIBUTOS E CONSTRUTOR

        private readonly IAdocaoService _adocaoService;
        private readonly IAdotanteService _adotanteService;
        private readonly IAnimalService _animalService;
        private readonly IPontoAdocaoService _pontoAdocaoService;
        private readonly IDoadorService _doadorService;
        private readonly IAcompanhamentoService _acompanhamentoService;

        private const string LogoPath = "Assets/aaps_logo.png";
        private const string FacebookIconPath = "Assets/icon_facebook.png";
        private const string InstagramIconPath = "Assets/icon_instagram.png";

        public TermoAdocaoService(IAdocaoService adocaoService, IAdotanteService adotanteService, IAnimalService animalService, IPontoAdocaoService pontoAdocaoService, IDoadorService doadorService, IAcompanhamentoService acompanhamentoService)
        {
            _adocaoService = adocaoService;
            _adotanteService = adotanteService;
            _animalService = animalService;
            _pontoAdocaoService = pontoAdocaoService;
            _doadorService = doadorService;
            _acompanhamentoService = acompanhamentoService;
        }

        #endregion

        public async Task<byte[]> GerarPdf(int adocaoId)
        {
            var adocao = await _adocaoService.ObterAdocaoPorId(adocaoId);
            var adotante = await _adotanteService.ObterAdotantePorId(adocao.AdotanteId);
            var animal = await _animalService.ObterAnimalPorId(adocao.AnimalId);
            var pontoAdocao = await _pontoAdocaoService.ObterPontoAdocaoPorId(adocao.PontoAdocaoId);
            var doador = await _doadorService.ObterDoadorPorId(animal.DoadorId);
            var acompanhamentos = await _acompanhamentoService.ObterAcompanhamentosPorAnimalId(animal.Id);

            using (var memoryStream = new MemoryStream())
            using (PdfWriter writer = new PdfWriter(memoryStream))
            {
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf, PageSize.A4);
                document.SetMargins(10, 30, 10, 30);

                var fontNormal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                var fontBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                // CABEÇALHO
                var header = new Table(3).UseAllAvailableWidth();
                // Logo à esquerda
                var logo = new Image(ImageDataFactory.Create(LogoPath)).ScaleAbsolute(50, 50);
                header.AddCell(new Cell().Add(logo).SetBorder(Border.NO_BORDER).SetVerticalAlignment(VerticalAlignment.MIDDLE));

                // Título centralizado
                var titleCell = new Cell()
                    .Add(new Paragraph("Termo de Adoção Responsável")
                        .SetFont(fontBold)
                        .SetFontSize(12)
                        .SetTextAlignment(TextAlignment.CENTER))
                    .SetBorder(Border.NO_BORDER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE);
                header.AddCell(titleCell);

                // Número à direita
                var numberCell = new Cell()
                    .Add(new Paragraph($"Nº: {adocao.Id}")
                        .SetFont(fontNormal)
                        .SetFontSize(10)
                        .SetTextAlignment(TextAlignment.RIGHT))
                    .SetBorder(Border.NO_BORDER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .SetPaddingRight(10f);
                header.AddCell(numberCell);

                document.Add(header);

                document.SetFont(fontNormal).SetFontSize(9);

                // DADOS DO ADOTANTE EM FORMATO DE TABELA
                var spacingStyle = new Style().SetMarginBottom(0).SetMarginLeft(0);

                // Nome (linha única)
                document.Add(new Paragraph("Nome: " + adotante.Nome).AddStyle(spacingStyle));

                // RG e CPF em colunas com alinhamento space-between
                var docTable = new Table(UnitValue.CreatePercentArray(new float[] { 45, 55 })).UseAllAvailableWidth();
                docTable.AddCell(new Cell().Add(new Paragraph("RG: " + adotante.Rg)).SetBorder(Border.NO_BORDER));
                docTable.AddCell(new Cell().Add(new Paragraph("CPF: " + adotante.Cpf)).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
                document.Add(docTable);

                // Endereço
                document.Add(new Paragraph("Endereço: " + adotante.Logradouro).AddStyle(spacingStyle));

                // Bairro e Cidade em colunas com alinhamento space-between
                var localTable = new Table(UnitValue.CreatePercentArray(new float[] { 45, 55 })).UseAllAvailableWidth();
                localTable.AddCell(new Cell().Add(new Paragraph("Bairro: " + adotante.Bairro)).SetBorder(Border.NO_BORDER));
                localTable.AddCell(new Cell().Add(new Paragraph("Cidade: " + adotante.Cidade)).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
                document.Add(localTable);

                document.Add(new Paragraph("Telefones: " + adotante.Celular + " ou " + adotante.Contato).AddStyle(spacingStyle));
                document.Add(new Paragraph("Local de trabalho: " + adotante.LocalTrabalho).AddStyle(spacingStyle));

                // 1. Tipo Moradia formatado
                string situacao = adotante.SituacaoEndereco?.Trim() ?? "";
                string xPropria = situacao.Equals("Própria", StringComparison.OrdinalIgnoreCase) ? "X" : " ";
                string xAlugada = situacao.Equals("Alugada", StringComparison.OrdinalIgnoreCase) ? "X" : " ";
                document.Add(new Paragraph($"Tipo Moradia: Própria: ( {xPropria} ) Alugada: ( {xAlugada} )").AddStyle(spacingStyle));

                // Redes sociais em colunas com alinhamento space-between
                var redesTable = new Table(UnitValue.CreatePercentArray(new float[] { 45, 55 })).UseAllAvailableWidth();
                redesTable.AddCell(new Cell().Add(new Paragraph("Facebook: " + adotante.Facebook)).SetBorder(Border.NO_BORDER));
                redesTable.AddCell(new Cell().Add(new Paragraph("Instagram: " + adotante.Instagram)).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
                document.Add(redesTable.SetMarginBottom(0)); // Reduzindo margem inferior

                // 4. Espaço menor entre adotante e animal
                // ANIMAL
                document.Add(new Paragraph("ESTOU ADOTANDO E ASSUMINDO TOTAL RESPONSABILIDADE PELO ANIMAL:")
                    .SetFont(fontBold)
                    .SetMarginTop(3)); // Reduzindo espaço superior

                // 1. Novo formato para dados do animal
                string especie = animal.Especie?.Trim() ?? "";
                string xCao = especie.Equals("Cão", StringComparison.OrdinalIgnoreCase) ||
                              especie.Equals("Cachorro", StringComparison.OrdinalIgnoreCase) ? "X" : " ";
                string xGato = especie.Equals("Gato", StringComparison.OrdinalIgnoreCase) ? "X" : " ";

                string sexo = animal.Sexo?.Trim() ?? "";
                string xF = sexo.StartsWith("F", StringComparison.OrdinalIgnoreCase) ? "X" : " ";
                string xM = sexo.StartsWith("M", StringComparison.OrdinalIgnoreCase) ? "X" : " ";

                string idade = animal.DataNascimento.HasValue
                    ? CalcularIdade(animal.DataNascimento.Value)
                    : "Data de nascimento não informada";

                // Nova tabela com o formato especificado
                var animalTable = new Table(UnitValue.CreatePercentArray(new float[] { 25, 25, 25, 25 })).UseAllAvailableWidth();
                animalTable.AddCell(new Cell().Add(new Paragraph($"Espécie: Cão ( {xCao} ) Gato ( {xGato} )")).SetBorder(Border.NO_BORDER));
                animalTable.AddCell(new Cell().Add(new Paragraph($"Cor: {animal.Pelagem}")).SetBorder(Border.NO_BORDER));
                animalTable.AddCell(new Cell().Add(new Paragraph($"Sexo: F ( {xF} ) M ( {xM} )")).SetBorder(Border.NO_BORDER));
                animalTable.AddCell(new Cell().Add(new Paragraph($"Idade: {idade}")).SetBorder(Border.NO_BORDER));
                document.Add(animalTable);

                // 3. Vermifugado, Vacinado, Castrado com space-between
                bool vermifugado = acompanhamentos?.Any(a => a.Descricao.Contains("vermif", StringComparison.OrdinalIgnoreCase)) ?? false;
                bool vacinado = acompanhamentos?.Any(a => a.Descricao.Contains("vacin", StringComparison.OrdinalIgnoreCase)) ?? false;
                bool castrado = acompanhamentos?.Any(a => a.Descricao.Contains("castr", StringComparison.OrdinalIgnoreCase)) ?? false;

                var saudeTable = new Table(UnitValue.CreatePercentArray(new float[] { 1, 1, 1 })).UseAllAvailableWidth();
                saudeTable.AddCell(new Cell().Add(new Paragraph($"Vermifugado ( {(vermifugado ? "X" : " ")} )")).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
                saudeTable.AddCell(new Cell().Add(new Paragraph($"Vacinado ( {(vacinado ? "X" : " ")} )")).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.CENTER));
                saudeTable.AddCell(new Cell().Add(new Paragraph($"Castrado ( {(castrado ? "X" : " ")} )")).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT));
                document.Add(saudeTable);

                // 1. Dados do doador/telefone e local/data em colunas
                var doadorInfoTable = new Table(UnitValue.CreatePercentArray(new float[] { 1, 1 })).UseAllAvailableWidth();
                doadorInfoTable.AddCell(new Cell().Add(new Paragraph("Doador: " + doador.Nome)).SetBorder(Border.NO_BORDER));
                doadorInfoTable.AddCell(new Cell().Add(new Paragraph("Telefone: " + doador.Celular)).SetBorder(Border.NO_BORDER));
                document.Add(doadorInfoTable);

                var localDataTable = new Table(UnitValue.CreatePercentArray(new float[] { 1, 1 })).UseAllAvailableWidth();
                localDataTable.AddCell(new Cell().Add(new Paragraph("Local: " + pontoAdocao.NomeFantasia)).SetBorder(Border.NO_BORDER));
                localDataTable.AddCell(new Cell().Add(new Paragraph("Data: " + adocao.Data.ToString("dd/MM/yyyy"))).SetBorder(Border.NO_BORDER));
                document.Add(localDataTable);

                // Assinatura com linha de separação
                document.Add(new Paragraph("Assinatura: ________________________________")
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetMarginTop(10)
                    .SetMarginBottom(3));

                // Linha de separação visível
                document.Add(new LineSeparator(new DashedLine())
                    .SetMarginTop(5)
                    .SetMarginBottom(5));

                // Autorização
                document.Add(new Paragraph("*AUTORIZO A DIVULGAÇÃO DA MINHA IMAGEM NAS REDES SOCIAIS, CONFORME A NECESSIDADE DA ONG.")
                    .SetFont(fontBold)
                    .SetMarginTop(5)
                    .SetTextAlignment(TextAlignment.CENTER));

                // Segundo Header
                var secondHeader = new Table(UnitValue.CreatePercentArray(new float[] { 1, 2, 1 })).UseAllAvailableWidth();
                secondHeader.AddCell(new Cell().Add(new Image(ImageDataFactory.Create(LogoPath)).ScaleAbsolute(40, 40)).SetBorder(Border.NO_BORDER));
                secondHeader.AddCell(new Cell().Add(new Paragraph("LEIA ANTES DE ASSINAR\nTermo de Adoção Responsável")
                    .SetFont(fontBold)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(10)).SetBorder(Border.NO_BORDER));
                secondHeader.AddCell(new Cell().Add(new Paragraph($"Nº: {adocao.Id}")
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetFontSize(9)).SetBorder(Border.NO_BORDER));
                document.Add(secondHeader);

                // Texto de compromisso
                foreach (var trecho in GetTrechosCompromisso())
                    document.Add(FormatarParagrafoCompromisso(trecho, fontNormal, fontBold));

                // 1. Dados finais em colunas
                var doadorFinalTable = new Table(UnitValue.CreatePercentArray(new float[] { 1, 1 })).UseAllAvailableWidth();
                doadorFinalTable.AddCell(new Cell().Add(new Paragraph("Doador: " + doador.Nome)).SetBorder(Border.NO_BORDER));
                doadorFinalTable.AddCell(new Cell().Add(new Paragraph("Contato: " + doador.Celular)).SetBorder(Border.NO_BORDER));
                document.Add(doadorFinalTable.SetMarginTop(10)); // Mesmo espaçamento

                var localDataFinalTable = new Table(UnitValue.CreatePercentArray(new float[] { 1, 1 })).UseAllAvailableWidth();
                localDataFinalTable.AddCell(new Cell().Add(new Paragraph("Local: " + pontoAdocao.NomeFantasia)).SetBorder(Border.NO_BORDER));
                localDataFinalTable.AddCell(new Cell().Add(new Paragraph("Data: " + adocao.Data.ToString("dd/MM/yyyy"))).SetBorder(Border.NO_BORDER));
                document.Add(localDataFinalTable);

                // 5. Rodapé redes sociais com ícones maiores
                var redesFooter = new Table(UnitValue.CreatePercentArray(new float[] { 1, 1 })).UseAllAvailableWidth();

                var facebookInnerTable = new Table(UnitValue.CreatePercentArray(new float[] { 0, 1 }))
                    .UseAllAvailableWidth()
                    .SetBorder(Border.NO_BORDER);

                facebookInnerTable.AddCell(new Cell().SetBorder(Border.NO_BORDER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .Add(new Image(ImageDataFactory.Create(FacebookIconPath))
                        .ScaleAbsolute(30, 30)));

                facebookInnerTable.AddCell(new Cell().SetBorder(Border.NO_BORDER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .Add(new Paragraph(" facebook.com/anjoseeprotetores")));

                var facebookCell = new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(facebookInnerTable);

                var instagramInnerTable = new Table(UnitValue.CreatePercentArray(new float[] { 0, 1 }))
                    .UseAllAvailableWidth()
                    .SetBorder(Border.NO_BORDER);

                instagramInnerTable.AddCell(new Cell().SetBorder(Border.NO_BORDER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .Add(new Image(ImageDataFactory.Create(InstagramIconPath))
                        .ScaleAbsolute(30, 30)));

                instagramInnerTable.AddCell(new Cell().SetBorder(Border.NO_BORDER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .Add(new Paragraph(" @anjoseprotetores")));

                var instagramCell = new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(instagramInnerTable);

                redesFooter.AddCell(facebookCell);
                redesFooter.AddCell(instagramCell);
                document.Add(redesFooter);

                document.Close();
                return memoryStream.ToArray();
            }
        }

        // ---------------- MÉTODOS AUXILIARES ------------------

        private string CalcularIdade(DateTime dataNascimento)
        {
            DateTime hoje = DateTime.Today;
            int anos = hoje.Year - dataNascimento.Year;
            int meses = hoje.Month - dataNascimento.Month;

            if (hoje.Day < dataNascimento.Day)
                meses--;

            if (meses < 0)
            {
                anos--;
                meses += 12;
            }

            if (anos > 0)
                return $"{anos} {(anos == 1 ? "ano" : "anos")}";

            if (meses > 0)
                return $"{meses} {(meses == 1 ? "mês" : "meses")}";

            return "menos de 1 mês";
        }

        private Paragraph FormatarParagrafoCompromisso(string texto, PdfFont fontNormal, PdfFont fontBold)
        {
            var palavrasNegrito = new[] {
                "Comprometo-me a", "atendimento veterinário imediato", "vacinas importadas",
                "animais resgatados e não podemos garantir o seu estado de saúde", "maus tratos, portanto CRIME", "Negligência", "CRIME",
                "vermifugação", "castração é obrigatória", "negligência, portanto CRIME", "sem o aval da ONG", "ONG fará acompanhamento",
                "Estou ciente de que", "vacinas anuais (raiva e viroses, importadas), vermifugação", "caso não cumpra responderá por negligência, portanto CRIME"
            };

            var p = new Paragraph().SetTextAlignment(TextAlignment.JUSTIFIED).SetFont(fontNormal).SetFontSize(9);

            foreach (var frase in palavrasNegrito)
            {
                if (texto.Contains(frase, StringComparison.OrdinalIgnoreCase))
                {
                    int index = texto.IndexOf(frase, StringComparison.OrdinalIgnoreCase);
                    p.Add(new Text(texto.Substring(0, index)));
                    p.Add(new Text(frase).SetFont(fontBold));
                    texto = texto.Substring(index + frase.Length);
                }
            }

            p.Add(new Text(texto));
            return p.SetMargin(0);
        }

        private string[] GetTrechosCompromisso() => new[]
        {
            "Comprometo-me a:",
            "1-Dar alimentação adequada e de boa qualidade, de acordo com a espécie e idade, assim como: atendimento veterinário imediato, iniciando o processo das vacinas importadas já, seguindo a orientação do veterinário. Lembrando que se trata de animais resgatados e não podemos garantir o seu estado de saúde.",
            "2-Dar abrigo adequado, limpo e seco, com espaço suficiente para suas necessidades, não deixando exposto o tempo todo, ao sol, frio ou chuva, nunca amarrado ou preso em espaço muito limitado, considerado maus tratos, portanto CRIME. Não deixar dar voltinhas sozinho, pode ser atropelado, se perder, ser atacado, envenenado, isso é Negligência, portanto CRIME.",
            "3- Procurar um médico veterinário regularmente, para as vacinas anuais (raiva e viroses, importadas), vermifugação, ou em caso de doença visando assegurar a saúde do animal adotado. Lembrando que sua castração é obrigatória (tanto em machos, como em fêmeas), caso não cumpra responderá por negligência, portanto CRIME.",
            "4-Proteger o animal contra a Leishmaniose coleira ou pipetas específicas. A sorologia e vacinas deverão ser feitas conforme orientação veterinária.",
            "5- Em hipótese alguma o animal deve ser abandonado, ou doado para outra pessoa, sem o aval da ONG, o termo está no seu nome e quem responderá pelo animal é você. Em caso de mudança ou morte do animal a ONG, deverá ser comunicada. A ONG fará acompanhamento da adoção e visitas, você deverá encaminhar, sempre, fotos do(s) animal(is), carteira de vacinação e comunicar a castração. Qualquer irregularidade poderá responder na justiça.",
            "Estou ciente de que:",
            "1- Não estou adotando o animal por impulso ou pena, sei que viverá de 15 a 20 anos, será membro da família, e caberá na mudança, todos da família estão de acordo, caberá nas finanças, terei paciência com a adaptação e prestarei toda assistência necessária ao animal.",
            "2- O não cumprimento dos itens mencionados acima, poderão ser interpretados como maus tratos pelo(a) doador(a)/ONG a qualquer tempo e processo judicial.",
            "3- Maus tratos e abandono é CRIME e estarei sujeito às penas previstas pela Lei Federal de Proteção Animal nº9605/1998 artigo 32 e nº14064/20.",
            "4- Não devolverei o animal, pois ele é uma vida que tem sentimentos e não é uma mercadoria.",
            "5- Que a minha imagem será divulgada nas redes sociais, conforme a NECESSIDADE DA ONG."
        };
    }
}