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
    ///<summary>
    /// This page enable the user to show a page of the contract. 
    ///</summary>
    /// <remarks>Vincent Ischi</remarks>
    public partial class ContractOutput : System.Web.UI.Page
    {
        /// <summary>
        /// Show the contract description. 
        /// </summary>
        /// <param name="sender">The sender of the events</param>
        /// <param name="e">The args of the event</param>
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

        /// <summary>
        /// Create a HTML page with contract
        /// </summary>
        /// <param name="id">ID of contract</param>
        /// <returns>A new HTML page</returns>
        private string CreateContractOuput(int id)
        {
            XmlDocument xmlDoc = new ContractsDAO().getContractXMLById(id);
            string outputHtml = ConvertXML(xmlDoc, Server.MapPath("contract.xslt"), new XsltArgumentList());

            return outputHtml;
        }

        /// <summary>
        /// Convert a XML file to a HTML page with XSLT transformation
        /// </summary>
        /// <param name="InputXMLDocument">XML input</param>
        /// <param name="XSLTFilePath">XSLT file path</param>
        /// <param name="XSLTArgs">List of XSLT args</param>
        /// <returns>A HTML page</returns>
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