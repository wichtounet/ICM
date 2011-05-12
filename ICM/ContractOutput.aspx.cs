using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using ICM.Dao;
using ICM.Utils;
using System.Xml.Xsl;

namespace ICM
{
    public partial class ContractOutput : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["contract"] != null)
            {
                var id = Request.QueryString["contract"].ToInt();

                Context.Response.ClearHeaders();
                Context.Response.ClearContent();
                Context.Response.AppendHeader("Pragma", "no-cache");
                Context.Response.AppendHeader("Cache-Control", "no-cache");
                Context.Response.CacheControl = "no-cache";
                Context.Response.Expires = -1;
                Context.Response.ExpiresAbsolute = new DateTime(1900, 1, 1);
                Context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Context.Response.Write(CreateContractOuput(id));
                Context.Response.Flush();
                Context.Response.End();
            }
        }

        private string CreateContractOuput(int id)
        {
            XmlDocument xmlDoc = new ContractsDAO().getContractXMLById(id);
            string outputHtml = ConvertXML(xmlDoc, Server.MapPath("contract.xslt"), new XsltArgumentList());

            return outputHtml;
        }

        public static string ConvertXML(XmlDocument InputXMLDocument, string XSLTFilePath, XsltArgumentList XSLTArgs)
        {
            System.IO.StringWriter sw = new System.IO.StringWriter();
            XslCompiledTransform xslTrans = new XslCompiledTransform();
            xslTrans.Load(XSLTFilePath);
            xslTrans.Transform(InputXMLDocument.CreateNavigator(), XSLTArgs, sw);
            return sw.ToString();
        }  
    }
}