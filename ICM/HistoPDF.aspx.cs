using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.UI;
using ICM.Dao;
using ICM.Utils;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

namespace ICM
{
    public partial class HistoPDF : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var contractsQuery = Request.QueryString["contracts"];
            var contracts = contractsQuery == null || contractsQuery.Count() == 0 ? new string[0] : contractsQuery.Split(new[]{';'}, StringSplitOptions.RemoveEmptyEntries);

            var personsQuery = Request.QueryString["persons"];
            var persons = personsQuery == null || personsQuery.Count() == 0 ? new string[0] : personsQuery.Split(new[]{';'}, StringSplitOptions.RemoveEmptyEntries);
            
            using (var m = new MemoryStream())
            {
                var document = new Document();

                try
                {
                    var writer = PdfWriter.GetInstance(document, m);
                    writer.CloseStream = false;

                    document.Open();

                    try
                    {
                        WriteDocument(document, contracts, persons);
                    } 
                    catch (SqlException exception)
                    {
                        document.Add(new Paragraph("Impossible de générer le PDF à cause d'une erreur serveur : " + exception.Message));
                    }
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

        private void WriteDocument(Document document, IEnumerable<string> contracts, IEnumerable<string> persons)
        {
            var title = "Historique " + Request.QueryString["year"];

            document.AddTitle(title);

            var titleFont = new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD);
            var subtitleFont = new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD);

            document.Add(new Paragraph(title, titleFont));

            document.Add(new Paragraph(" ", subtitleFont));
            document.Add(new Paragraph("Contrats", subtitleFont));

            var contractsDAO = new ContractsDAO();

            var contractsData = contracts.Select(contract => contract.ToInt()).Aggregate("<ul>", (current, id) => current + ("<li>" + contractsDAO.GetContractById(id) + "</li>")) + "</ul>";

            ParseHtml(document, contractsData);

            document.Add(new Paragraph(" ", subtitleFont));
            document.Add(new Paragraph("Personnes", subtitleFont));

            var personsDAO = new PersonsDAO();

            var personsData = persons.Select(person => person.ToInt()).Aggregate("<ul>", (current, id) => current + ("<li>" + personsDAO.GetPersonByID(id) + "</li>")) + "</ul>";

            ParseHtml(document, personsData);
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
    }
}