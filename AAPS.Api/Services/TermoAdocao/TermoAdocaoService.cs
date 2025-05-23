using AAPS.Api.Dtos.TermoAdocao;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace AAPS.Api.Services.TermoAdocao
{
    public class TermoAdocaoService : ITermoAdocaoService
    {
        private const string LogoPath = "Assets/aaps_logo.png";
        private const string FacebookIconPath = "Assets/icon_facebook.png";
        private const string InstagramIconPath = "Assets/icon_instagram.png";

        public byte[] GerarPdf(TermoAdocaoDto dto)
        {
            using (var memoryStream = new MemoryStream())

            using (PdfWriter writer = new PdfWriter(memoryStream))
            {
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf, PageSize.A4);
                document.SetMargins(10, 30, 10, 30);

                var fontNormal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                var fontBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                var fontItalic = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_OBLIQUE);

                // Dados do adotante
                AddJustifiedField(document, "Nome", dto.Nome, 60);
                AddSplitField(document, "RG", dto.Rg, 25, "CPF", dto.Cpf, 25);
                AddJustifiedField(document, "Endereço", dto.Endereco, 60);
                AddSplitField(document, "Bairro", dto.Bairro, 25, "Cidade", dto.Cidade, 25);
                AddJustifiedField(document, "Telefones", $"{dto.Telefone1} ou {dto.Telefone2}", 50);
                AddSplitField(document, "Local de trabalho", dto.LocalDeTrabalho, 40, "Telefone", "_________", 20);
                AddCheckOptions(document, new[] {
                ("Casa", dto.TipoResidencia == "Casa"),
                ("Apto", dto.TipoResidencia == "Apartamento"),
                ("Própria", dto.ImovelProprio == "Sim"),
                ("Alugada", dto.ImovelProprio == "Não")
            });
                AddJustifiedField(document, "Facebook", dto.Facebook, 25, "Instagram", dto.Instagram, 25);

                // Animal
                document.Add(new Paragraph("ESTOU ADOTANDO E ASSUMINDO TOTAL RESPONSABILIDADE PELO ANIMAL:")
                    .SetFont(fontBold)
                    .SetTextAlignment(TextAlignment.JUSTIFIED));

                AddCheckOptions(document, new[] {
                ("Cão", dto.TipoAnimal == "Cão"),
                ("Gato", dto.TipoAnimal == "Gato")
            }, "Cor", dto.Cor, "Sexo: F", dto.Sexo == "Fêmea", "M", dto.Sexo == "Macho", "Idade", dto.Idade);

                AddCheckOptions(document, new[] {
                ("Vermifugado", dto.Vermifugado),
                ("Vacinado", dto.Vacinado),
                ("Castrado", dto.Castrado)
            });

                AddSplitField(document, "Doador", dto.DoadorNome, 35, "Telefone", dto.DoadorTelefone, 35);
                AddSplitField(document, "Local", dto.PontoAdocao, 35, "Data", dto.DataAdocao.ToString("dd/MM/yyyy"), 35);
                document.Add(new Paragraph("Assinatura: ________________________________").SetTextAlignment(TextAlignment.RIGHT));

                // Autorização
                document.Add(new Paragraph("*AUTORIZO A DIVULGAÇÃO DA MINHA IMAGEM NAS REDES SOCIAIS, CONFORME A NECESSIDADE DA ONG.")
                    .SetFont(fontBold)
                    .SetTextAlignment(TextAlignment.JUSTIFIED));
                document.Add(new Paragraph(new string('-', 110)).SetTextAlignment(TextAlignment.CENTER).SetFontSize(8).SetMarginTop(2).SetMarginBottom(4));











                // Segundo Header
                var secondHeader = new Table(UnitValue.CreatePercentArray(new float[] { 1, 2, 1 })).UseAllAvailableWidth();
                secondHeader.AddCell(new Cell().Add(new Image(ImageDataFactory.Create(LogoPath)).ScaleAbsolute(40, 40)).SetBorder(Border.NO_BORDER));
                secondHeader.AddCell(new Cell().Add(new Paragraph("LEIA ANTES DE ASSINAR\nTermo de Adoção Responsável")
                    .SetFont(fontBold)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(10)).SetBorder(Border.NO_BORDER));
                secondHeader.AddCell(new Cell().Add(new Paragraph($"Nº: {dto.NumeroTermo}")
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetFontSize(9)).SetBorder(Border.NO_BORDER));
                document.Add(secondHeader);

                // Texto de compromisso
                foreach (var trecho in GetTrechosCompromisso())
                    document.Add(FormatarParagrafoCompromisso(trecho, fontNormal, fontBold));

                // Doador, Local, Data
                AddSplitField(document, "Doador", dto.DoadorNome, 35, "Contato", dto.DoadorTelefone, 35);
                AddSplitField(document, "Local", dto.PontoAdocao, 35, "Data", dto.DataAdocao.ToString("dd/MM/yyyy"), 35);

                // Rodapé com redes sociais
                var redes = new Table(UnitValue.CreatePercentArray(new float[] { 1, 1 })).UseAllAvailableWidth();
                redes.AddCell(new Cell().Add(new Paragraph().Add(new Image(ImageDataFactory.Create(FacebookIconPath)).ScaleAbsolute(14, 14))
                    .Add(" facebook.com/anjoseeprotetores").SetFontSize(8)).SetBorder(Border.NO_BORDER));
                redes.AddCell(new Cell().Add(new Paragraph().Add(new Image(ImageDataFactory.Create(InstagramIconPath)).ScaleAbsolute(14, 14))
                    .Add(" @anjoseprotetores").SetFontSize(8)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER));
                document.Add(redes);

                document.Close();
                return memoryStream.ToArray();
            }












            // CABEÇALHO
            //var logo = new Image(ImageDataFactory.Create(LogoPath)).ScaleAbsolute(50, 50);
            //var header = new Table(2).UseAllAvailableWidth();
            //header.AddCell(new Cell().Add(logo).SetBorder(Border.NO_BORDER));
            //var headerText = new Paragraph()
            //    .Add(new Text("Termo de Adoção Responsável\n").SetFont(fontBold).SetFontSize(12))
            //    .Add(new Text("Nº: " + dto.NumeroTermo).SetFont(fontNormal).SetFontSize(10));
            //header.AddCell(new Cell().Add(headerText).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT));
            //document.Add(header);

            //document.SetFont(fontNormal).SetFontSize(9);

            //// Dados do adotante
            //document.Add(new Paragraph("Nome: " + dto.Nome + " ______________________________").SetTextAlignment(TextAlignment.JUSTIFIED));
            //document.Add(new Paragraph("RG: " + dto.Rg + " _____________       CPF: " + dto.Cpf + " _______________").SetTextAlignment(TextAlignment.JUSTIFIED));
            //document.Add(new Paragraph("Endereço: " + dto.Endereco + " __________________________________________").SetTextAlignment(TextAlignment.JUSTIFIED));
            //document.Add(new Paragraph("Bairro: " + dto.Bairro + " _____________________     Cidade: " + dto.Cidade + " __________________").SetTextAlignment(TextAlignment.JUSTIFIED));
            //document.Add(new Paragraph("Telefones: " + dto.Telefone1 + " ou " + dto.Telefone2 + " __________________________").SetTextAlignment(TextAlignment.JUSTIFIED));
            //document.Add(new Paragraph("Local de trabalho: " + dto.LocalDeTrabalho + " __________________   Telefone: __________").SetTextAlignment(TextAlignment.JUSTIFIED));
            //document.Add(new Paragraph($"Casa: ( {(dto.TipoResidencia == "Casa" ? "X" : " ")} )  Apto: ( {(dto.TipoResidencia == "Apartamento" ? "X" : " ")} )   Própria: ( {(dto.ImovelProprio == "Sim" ? "X" : " ")} )  Alugada: ( {(dto.ImovelProprio == "Não" ? "X" : " ")} )").SetTextAlignment(TextAlignment.JUSTIFIED));
            //document.Add(new Paragraph("Facebook: " + dto.Facebook + "     Instagram: " + dto.Instagram).SetTextAlignment(TextAlignment.JUSTIFIED));

            //// Animal
            //document.Add(new Paragraph("\n").SetFontSize(5));
            //document.Add(new Paragraph("ESTOU ADOTANDO E ASSUMINDO TOTAL RESPONSABILIDADE PELO ANIMAL:").SetFont(fontBold).SetTextAlignment(TextAlignment.JUSTIFIED));
            //var xCao = dto.TipoAnimal.Equals("Cão", StringComparison.OrdinalIgnoreCase) ? "X" : " ";
            //var xGato = dto.TipoAnimal.Equals("Gato", StringComparison.OrdinalIgnoreCase) ? "X" : " ";
            //var xF = dto.Sexo == "Fêmea" ? "X" : " ";
            //var xM = dto.Sexo == "Macho" ? "X" : " ";
            //document.Add(new Paragraph($"Cão ( {xCao} )  Gato ( {xGato} )   Cor: {dto.Cor} __________   Sexo: F ( {xF} ) M ( {xM} )   Idade: {dto.Idade} ______").SetTextAlignment(TextAlignment.JUSTIFIED));
            //document.Add(new Paragraph($"Vermifugado ( {(dto.Vermifugado ? "X" : " ")} )    Vacinado ( {(dto.Vacinado ? "X" : " ")} )    Castrado ( {(dto.Castrado ? "X" : " ")} )").SetTextAlignment(TextAlignment.JUSTIFIED));
            //document.Add(new Paragraph("Doador: " + dto.DoadorNome + "     Telefone: " + dto.DoadorTelefone).SetTextAlignment(TextAlignment.JUSTIFIED));
            //document.Add(new Paragraph($"Local: {dto.PontoAdocao} __________________   Data: {dto.DataAdocao:dd/MM/yyyy}").SetTextAlignment(TextAlignment.JUSTIFIED));
            //document.Add(new Paragraph("Assinatura: ___________________________").SetTextAlignment(TextAlignment.RIGHT));

            //// Autorização
            //document.Add(new Paragraph("*AUTORIZO A DIVULGAÇÃO DA MINHA IMAGEM NAS REDES SOCIAIS, CONFORME A NECESSIDADE DA ONG.").SetFont(fontBold).SetTextAlignment(TextAlignment.JUSTIFIED));
            //document.Add(new Paragraph(new string('-', 100)).SetTextAlignment(TextAlignment.CENTER));

            //// Segundo cabeçalho
            //var secondHeader = new Table(2).UseAllAvailableWidth();
            //secondHeader.AddCell(new Cell().Add(new Image(ImageDataFactory.Create(LogoPath)).ScaleAbsolute(50, 50)).SetBorder(Border.NO_BORDER));
            //var secondHeaderText = new Paragraph()
            //    .Add(new Text("LEIA ANTES DE ASSINAR\n").SetFont(fontBold).SetFontSize(11))
            //    .Add(new Text("Nº: " + dto.NumeroTermo + "\nTermo de Adoção Responsável").SetFont(fontNormal).SetFontSize(9));
            //secondHeader.AddCell(new Cell().Add(secondHeaderText).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT));
            //document.Add(secondHeader);

            //// Compromissos
            //string[] compromissos = new[]
            //{
            //    "Comprometo-me a:",
            //    "1-Dar alimentação adequada e de boa qualidade, de acordo com a espécie e idade, assim como: atendimento veterinário imediato, iniciando o processo das vacinas importadas já, seguindo a orientação do veterinário. Lembrando que se trata de animais resgatados e não podemos garantir o seu estado de saúde.",
            //    "2-Dar abrigo adequado, limpo e seco, com espaço suficiente para suas necessidades, não deixando exposto o tempo todo, ao sol, frio ou chuva, nunca amarrado ou preso em espaço muito limitado, considerado maus tratos, portanto CRIME. Não deixar dar voltinhas sozinho, pode ser atropelado, se perder, ser atacado, envenenado, isso é Negligência, portanto CRIME.",
            //    "3- Procurar um médico veterinário regularmente, para as vacinas anuais (raiva e viroses, importadas), vermifugação, ou em caso de doença visando assegurar a saúde do animal adotado. Lembrando que sua castração é obrigatória (tanto em machos, como em fêmeas), caso não cumpra responderá por negligência, portanto CRIME.",
            //    "4-Proteger o animal contra a Leishmaniose coleira ou pipetas especificas. A sorologia e vacinas deverão ser feitas conforme orientação veterinária.",
            //    "5- Em hipótese alguma o animal deve ser abandonado, ou doado para outra pessoa, sem o aval da ONG, o termo está no seu nome e quem responderá pelo animal é você. Em caso de mudança ou morte do animal a ONG, deverá ser comunicada. A ONG fará acompanhamento da adoção e visitas, você deverá encaminhar, sempre, fotos do(s) animal(is), carteira de vacinação e comunicar a castração. Qualquer irregularidade poderá responder na justiça.",
            //    "Estou ciente de que:",
            //    "1- Não estou adotando o animal por impulso ou pena, sei que viverá de 15 a 20 anos, será membro da família, e caberá na mudança, todos da família estão de acordo, caberá nas finanças, terei paciência com a adaptação e prestarei toda assistência necessária ao animal.",
            //    "2- O não cumprimento dos itens mencionados acima, poderão ser interpretados como maus tratos pelo(a) doador(a)/ONG a qualquer tempo e processo judicial.",
            //    "3- Maus tratos e abandono é CRIME e estarei sujeito às penas previstas pela Lei Federal de Proteção Animal nº9605/1998 artigo 32 e nº14064/20.",
            //    "4- Não devolverei o animal, pois ele é uma vida que tem sentimentos e não é uma mercadoria.",
            //    "5- Que a minha imagem será divulgada nas redes sociais, conforme a NECESSIDADE DA ONG."
            //};

            //foreach (var texto in compromissos)
            //    document.Add(new Paragraph(texto).SetTextAlignment(TextAlignment.JUSTIFIED));

            //document.Add(new Paragraph($"Doador: {dto.DoadorNome}     Contato: {dto.DoadorTelefone}").SetTextAlignment(TextAlignment.JUSTIFIED));
            //document.Add(new Paragraph($"Local: {dto.PontoAdocao} __________________   Data: {dto.DataAdocao:dd/MM/yyyy}").SetTextAlignment(TextAlignment.JUSTIFIED));

            //// Rodapé com redes sociais centralizadas
            //var footerTable = new Table(2).SetHorizontalAlignment(HorizontalAlignment.CENTER);
            //footerTable.AddCell(new Cell().Add(new Image(ImageDataFactory.Create(FacebookIconPath)).ScaleToFit(10, 10)).SetBorder(Border.NO_BORDER));
            //footerTable.AddCell(new Cell().Add(new Paragraph("facebook.com/anjoseeprotetores").SetFontSize(8)).SetBorder(Border.NO_BORDER));
            //footerTable.AddCell(new Cell().Add(new Image(ImageDataFactory.Create(InstagramIconPath)).ScaleToFit(10, 10)).SetBorder(Border.NO_BORDER));
            //footerTable.AddCell(new Cell().Add(new Paragraph("@anjoseprotetores").SetFontSize(8)).SetBorder(Border.NO_BORDER));
            //document.Add(footerTable);

            //document.Close();
            //return memoryStream.ToArray();
        }

        // ---------------- MÉTODOS AUXILIARES ------------------

        private void AddJustifiedField(Document doc, string label, string value, int underlineLength)
        {
            doc.Add(new Paragraph($"{label}: {value} {new string('_', underlineLength)}").SetTextAlignment(TextAlignment.JUSTIFIED).SetMargin(0));
        }

        private void AddJustifiedField(Document doc, string label1, string value1, int len1, string label2, string value2, int len2)
        {
            doc.Add(new Paragraph($"{label1}: {value1} {new string('_', len1)}     {label2}: {value2} {new string('_', len2)}")
                .SetTextAlignment(TextAlignment.JUSTIFIED).SetMargin(0));
        }

        private void AddSplitField(Document doc, string label1, string value1, int len1, string label2, string value2, int len2)
        {
            doc.Add(new Paragraph($"{label1}: {value1} {new string('_', len1)}     {label2}: {value2} {new string('_', len2)}")
                .SetTextAlignment(TextAlignment.JUSTIFIED).SetMargin(0));
        }

        private void AddCheckOptions(Document doc, (string label, bool marcado)[] opcoes)
        {
            string linha = string.Join("    ", opcoes.Select(o => $"{o.label}: ( {(o.marcado ? "X" : " ")} )"));
            doc.Add(new Paragraph(linha).SetTextAlignment(TextAlignment.JUSTIFIED).SetMargin(0));
        }

        private void AddCheckOptions(Document doc, (string label, bool marcado)[] opcoes, string label1, string valor1, string label2, bool marcado2, string label3, bool marcado3, string label4, string valor4)
        {
            string linha = $"{opcoes[0].label} ({(opcoes[0].marcado ? "X" : " ")})   {opcoes[1].label} ({(opcoes[1].marcado ? "X" : " ")})   {label1}: {valor1} ______   {label2} ({(marcado2 ? "X" : " ")}) {label3} ({(marcado3 ? "X" : " ")})   {label4}: {valor4} ______";
            doc.Add(new Paragraph(linha).SetTextAlignment(TextAlignment.JUSTIFIED).SetMargin(0));
        }

        private Paragraph FormatarParagrafoCompromisso(string texto, PdfFont fontNormal, PdfFont fontBold)
        {
            var palavrasNegrito = new[] {
                "Comprometo-me a", "atendimento veterinário imediato", "vacinas importadas",
                "animais resgatados e não podemos garantir o seu estado de saúde", "maus tratos, portanto CRIME", "Negligência", "CRIME",
                "vermifugação", "castração é obrigatória", "negligência, portanto CRIME", "sem o aval da ONG", "ONG fará acompanhamento",
                "Estou ciente de que"
            };

            var p = new Paragraph().SetTextAlignment(TextAlignment.JUSTIFIED).SetFont(fontNormal).SetFontSize(9);

            foreach (var frase in palavrasNegrito)
            {
                if (texto.Contains(frase, StringComparison.OrdinalIgnoreCase))
                {
                    p.Add(new Text(frase).SetFont(fontBold));
                    texto = texto.Replace(frase, "", StringComparison.OrdinalIgnoreCase);
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
    };
}