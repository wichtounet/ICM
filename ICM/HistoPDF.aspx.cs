using System;
using System.IO;
using System.Web.UI;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

namespace ICM
{
    public partial class HistoPDF : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (var m = new MemoryStream())
            {
                var document = new Document();

                try
                {
                    var writer = PdfWriter.GetInstance(document, m);
                    writer.CloseStream = false;

                    document.Open();

                    WriteDocument(document);
                }
                catch (DocumentException ex)
                {
                    Console.Error.WriteLine(ex.StackTrace);
                    Console.Error.WriteLine(ex.Message);
                }

                document.Close();

                Response.Clear();
                Response.ContentType = "Application/pdf";
                Response.BinaryWrite(m.GetBuffer());
                Response.End();
            }
        }

        private static void WriteDocument(Document document)
        {
            var titleFont = new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD);

            var titleParagraph = new Paragraph("Historique", titleFont);

            document.Add(titleParagraph);

            
        }

        private static void ParseHtml(Document document, string str)
        {
            var reader = new StringReader(str);

            var worker = HTMLWorker.ParseToList(reader, new StyleSheet());

            foreach (var e in worker)
            {
                document.Add(e);
            }
        }

        /*public static string AsCommaSeparatedList(EntitySet<LinksItem> items)
        {
            var str = "";
            var index = 0;

            foreach (var item in items)
            {
                var link = "<a href=\"" + item.Link + "\">" + item.Title + "</a>";

                str += index++ > 0 ? ", " + link : link;
            }

            return str;
        }

        public static string AsCommaSeparatedList(EntitySet<RegistrationsItem> items)
        {
            var str = "";
            var index = 0;

            foreach (var item in items)
            {
                str += index++ > 0 ? ", " + item.Student : item.Student;
            }

            return str;
        }*/
    }
}