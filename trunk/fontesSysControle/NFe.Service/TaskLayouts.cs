using iTextSharp.text;
using iTextSharp.text.pdf;
using NFe.Components;
using NFe.Settings;
using System;
using System.Collections.Generic;
using System.IO;

namespace NFe.Service
{
    public class TaskLayouts : TaskAbst
    {
        public override void Execute()
        {
            Exception exx = null;
            var uext = NFe.Components.Propriedade.Extensao(Components.Propriedade.TipoEnvio.pedLayouts);
            string ExtRetorno = (NomeArquivoXML.ToLower().EndsWith(".xml") ? uext.EnvioXML : uext.EnvioTXT);
            string finalArqErro = uext.EnvioXML.Replace(".xml", ".err");

            string pastaRetorno = Propriedade.PastaGeralRetorno;
            string arqRetorno = pastaRetorno + "\\" + Functions.ExtrairNomeArq(NomeArquivoXML, null) + Propriedade.Extensao(Propriedade.TipoEnvio.pedLayouts).RetornoXML;

            Functions.DeletarArquivo(arqRetorno);
            Functions.DeletarArquivo(Propriedade.PastaGeralRetorno + "\\" + Functions.ExtrairNomeArq(NomeArquivoXML, null) + finalArqErro);

            try
            {
                using (Document document = new Document(PageSize.A4, 20f, 20f, 80f, 10f))
                {
                    var pdfWriter = PdfWriter.GetInstance(document, new FileStream(arqRetorno, FileMode.Create));

                    pdfWriter.PageEvent = new ITextEvents();

                    document.AddTitle("Layout CENFe");
                    document.AddSubject("Layout de arquivos texto");
                    document.AddCreator(Propriedade.DescricaoAplicacao);
                    document.AddAuthor(ConfiguracaoApp.NomeEmpresa);

                    document.SetPageSize(PageSize.A4.Rotate());
                    document.Open();
                    PdfPTable table = null;// new PdfPTable(3);
                    int n = 0;
                    foreach (Propriedade.TipoEnvio item in Enum.GetValues(typeof(Propriedade.TipoEnvio)))
                    {
                        if ((n % 20) == 0 || table == null)
                        {
                            if (table != null)
                            {
                                document.Add(table);
                                document.NewPage();
                            }
                            document.Add(new Phrase("Extensões permitidas" + (n > 0 ? "..." : ""), new Font(Font.FontFamily.HELVETICA, 18)));
                            table = new PdfPTable(3);
                            table.HorizontalAlignment = 0;
                            table.TotalWidth = document.PageSize.GetRight(41);
                            table.LockedWidth = true;
                            table.PaddingTop = 2;
                            table.SetWidths(new float[] { 40f, 40f, 100f });
                            table.AddCell(titulo("Envios"));
                            table.AddCell(titulo("Retornos"));
                            table.AddCell(titulo("Descrição"));
                        }
                        ++n;
                        var EXT = Propriedade.Extensao(item);
                        table.AddCell(texto(fmtname(EXT.EnvioXML) + fmtname(EXT.EnvioTXT, " ou ")));
                        table.AddCell(texto(fmtname(EXT.RetornoXML) + fmtname(EXT.RetornoTXT, " ou ")));
                        table.AddCell(texto(EXT.descricao));
                    }
                    if (table != null)
                    {
                        document.Add(table);
                        document.NewPage();
                    }
                    /// Layout NFe/NFCe
                    n = 0;
                    table = null;
                    foreach (KeyValuePair<string, string> key in new NFe.ConvertTxt.ConversaoTXT().LayoutTXT)
                    {
                        if (key.Key.Contains("_"))
                            /// só considera o layout da versao >= 3.1
                            if (key.Key.Contains("_200")) continue;

                        if ((n % 24) == 0 || table == null)
                        {
                            if (table != null)
                            {
                                document.Add(table);
                                document.NewPage();
                            }
                            document.Add(new Phrase("Layout de notas NF-e/NFc-e" + (n > 0 ? "..." : ""), new Font(Font.FontFamily.HELVETICA, 18)));
                            table = new PdfPTable(2);
                            table.AddCell(titulo("Segmento"));
                            table.AddCell(titulo("Formato"));
                            table.HorizontalAlignment = 0;
                            table.TotalWidth = document.PageSize.GetRight(41);
                            table.LockedWidth = true;
                            table.PaddingTop = 2;
                            table.SetWidths(new float[] { 8f, 100f });
                        }
                        ++n;
                        if (key.Key.Contains("_"))
                            table.AddCell(texto(key.Key.Substring(0, key.Key.IndexOf("_"))));
                        else
                            table.AddCell(texto(key.Key));
                        table.AddCell(texto(key.Value.ToString().Substring(1)));
                    }
                    if (table != null)
                    {
                        document.Add(table);
                        document.NewPage();
                    }
                    document.Close();
                }
            }
            catch (DocumentException de)
            {
                exx = de;
            }
            catch (IOException ioe)
            {
                exx = ioe;
            }
            catch (Exception ex)
            {
                exx = ex;
            }
            finally
            {
                Functions.DeletarArquivo(NomeArquivoXML);
                if (exx != null)
                {
                    Functions.DeletarArquivo(arqRetorno);
                    try
                    {
                        NFe.Service.TFunctions.GravarArqErroServico(NomeArquivoXML, ExtRetorno, finalArqErro, exx);
                    }
                    catch { }
                }
            }
        }
        string fmtname(string value, string prefix = "")
        {
            if (string.IsNullOrEmpty(value)) return "";
            if (value.StartsWith("-")) return prefix + "???" + value;
            return prefix + value;
        }
        PdfPCell titulo(string value)
        {
            PdfPCell cell = new PdfPCell(new Phrase(value, FontFactory.GetFont(FontFactory.COURIER, 10, Font.BOLD)));
            cell.BackgroundColor = new BaseColor(0xC0, 0xC0, 0xC0);
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_CENTER;
            return cell;
        }
        PdfPCell texto(string value)
        {
            PdfPCell cell = new PdfPCell(new Phrase(value, FontFactory.GetFont(FontFactory.COURIER, 10, Font.NORMAL)));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            return cell;
        }
    }
    public class ITextEvents : PdfPageEventHelper
    {
        PdfContentByte cb;
        PdfTemplate headerTemplate, footerTemplate;
        BaseFont bf = null;
        DateTime PrintTime = DateTime.Now;
        #region Fields
        private string _header;
        #endregion
    }
}
