using AAPS.Api.Dtos.TermoAdocao;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.IO;

namespace AAPS.Api.Services.TermoAdocao
{
    public class TermoAdocaoService : ITermoAdocaoService
    {
        #region ATRIBUTOS E CONSTRUTOR

        private const string LogoPath = "Assets/aaps_logo.png";

        #endregion

        public byte[] GerarPdf(TermoAdocaoDto dto)
        {
            using (var memoryStream = new MemoryStream())

            using (PdfWriter writer = new PdfWriter(memoryStream))
            {
                var pdfDocument = new PdfDocument(writer);
                var document = new Document(pdfDocument, PageSize.A4);

                var fontNormal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                var fontBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                var fontItalic = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_OBLIQUE);

                document.SetFont(fontNormal);

                // Logo
                ImageData imageData = ImageDataFactory.Create(LogoPath);
                Image logo = new Image(imageData).ScaleAbsolute(100, 100);
                document.Add(logo);

                // Título
                document.Add(new Paragraph("Termo de Adoção Responsável")
                    .SetFont(fontBold)
                    .SetFontSize(16)
                    .SetTextAlignment(TextAlignment.CENTER));
                document.Add(new Paragraph("\n\n"));

                // Dados pessoais
                document.Add(new Paragraph($"Nome: {dto.Nome}"));
                document.Add(new Paragraph($"RG: {dto.Rg}    CPF: {dto.Cpf}"));
                document.Add(new Paragraph($"Endereço: {dto.Endereco}"));
                document.Add(new Paragraph($"Bairro: {dto.Bairro}    Cidade: {dto.Cidade}"));
                document.Add(new Paragraph($"Telefone(s): {dto.Telefone1} / {dto.Telefone2}"));
                document.Add(new Paragraph($"Local de trabalho: {dto.LocalDeTrabalho}"));
                document.Add(new Paragraph($"Casa: ({dto.TipoResidencia})    Imóvel: ({dto.ImovelProprio})"));
                document.Add(new Paragraph($"Facebook: {dto.Facebook}    Instagram: {dto.Instagram}"));
                document.Add(new Paragraph("\n"));

                // Dados do animal
                document.Add(new Paragraph($"Estou adotando e assumindo total responsabilidade pelo animal:"));
                document.Add(new Paragraph($"{dto.TipoAnimal} | Cor: {dto.Cor} | Sexo: {dto.Sexo} | Idade: {dto.Idade}"));
                document.Add(new Paragraph($"Vermifugado: {(dto.Vermifugado ? "Sim" : "Não")}    Vacinado: {(dto.Vacinado ? "Sim" : "Não")}    Castrado: {(dto.Castrado ? "Sim" : "Não")}"));
                document.Add(new Paragraph($"Doador: {dto.DoadorNome} | Telefone: {dto.DoadorTelefone}"));
                document.Add(new Paragraph("\n\n"));

                // Compromissos
                document.Add(new Paragraph("* AUTORIZO A DIVULGAÇÃO DA MINHA IMAGEM NAS REDES SOCIAIS, CONFORME A NECESSIDADE DA ONG.").SetFont(fontItalic));
                document.Add(new Paragraph("LEIA ANTES DE ASSINAR").SetFont(fontBold));
                document.Add(new Paragraph("Comprometo-me a:"));
                document.Add(new Paragraph("1. Dar alimentação adequada e levar ao veterinário com urgência sempre que necessário, iniciando a vacinação com vacinas importadas."));
                document.Add(new Paragraph("2. Dar abrigo limpo, seguro, sem correntes e protegido do clima. Não deixar sozinho na rua."));
                document.Add(new Paragraph("3. Manter a vacinação e realizar a castração obrigatória."));
                document.Add(new Paragraph("4. Proteger contra leishmaniose com coleiras ou pipetas."));
                document.Add(new Paragraph("5. Avisar a ONG em caso de mudança de posse, óbito ou qualquer problema com o animal."));
                document.Add(new Paragraph("\nEstou ciente de que:"));
                document.Add(new Paragraph("1. Não estou adotando por impulso e me comprometo com a adaptação."));
                document.Add(new Paragraph("2. Maus tratos ou abandono são crime."));
                document.Add(new Paragraph("3. A ONG poderá acompanhar o animal."));
                document.Add(new Paragraph("4. Não devo devolvê-lo sem motivos sérios."));
                document.Add(new Paragraph("5. Minha imagem pode ser usada conforme necessidade da ONG."));
                document.Add(new Paragraph("\n\n"));

                // Assinatura
                document.Add(new Paragraph($"Local: _______________________     Data: {DateTime.Now:dd/MM/yyyy}"));
                document.Add(new Paragraph($"Assinatura: _______________________"));

                document.Close();
                pdfDocument.Close();
                    
                return memoryStream.ToArray();
            }
        }
    }
}