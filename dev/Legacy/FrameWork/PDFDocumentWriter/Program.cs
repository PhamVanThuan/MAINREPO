using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using System.IO;
using System.Collections;
using iTextSharp.text;
using PDFDocumentWriter.DataAccess;
using PDFDocumentWriter.Data;
using System.Data;
using PDFUtils.PDFWriting;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;

namespace PDFDocumentWriter
{
    class Program
    {
        /*
         * select rs.ReportStatementKey, rs.ReportName, rs.Description,bla.TemplateGenerationOrder, d.*
from reportstatement rs
join ReportStatementDocumentTemplate bla on rs.ReportStatementKey=bla.reportStatementKey
join DocumentTemplate d on bla.DocumentTemplateKey=d.DocumentTemplateKey
where rs.ReportStatementKey > 4003
order by ReportName,TemplateGenerationOrder
         * 
         * --delete from reportparameter where reportstatementkey>4003
--delete from ReportStatementDocumentTemplate
--delete from reportstatement where reportstatementkey>4003
--delete from documenttemplate
        */
        static void Main(string[] args)
        {
            try
            {
                Controller c = new Controller();
                //c.OnError += new EventHandler(c_OnError);
                //if (c.Start())
                //{
                //    Console.WriteLine("Started");
                //}
                //else
                //{
                //    Console.WriteLine("poked");
                //}
                //Console.ReadLine();
                //c.Stop();
                byte[] byImg = new byte[0];

                

                string OutName = string.Empty;
                // new purchase varifix
                PDFGenerator generator = new PDFGenerator();
                //List<string> Fields = generator.GetPDFKeys(@"\\sahls216s\PDF\Legal Agreements\Templates\Client and Loan information\VARIFIX Client and Loan information.pdf");
                //using (StreamWriter fs = File.CreateText(@".\Files.txt"))
                //{
                //    foreach (string s in Fields)
                //    {
                //        fs.WriteLine(s);
                //    }
                //}
                PDFGenerationObject obj = null;
                
                // Varifix - 761688
                Dictionary<string, object> Params = new Dictionary<string, object>();
                Params.Add("AccountKey", 1435519);
                obj = new PDFGenerationObject(Params, 4007);
                OutName = generator.GenerateDocument(obj);

                // new purchase int only
                Params = new Dictionary<string, object>();
                Params.Add("AccountKey", 2285545);
                Params.Add("PurposeNumber", 3);
                obj = new PDFGenerationObject(Params, 4005);
                //OutName = generator.GenerateDocument(obj);

                // New Purchase Std Variable
                Params = new Dictionary<string, object>();
                Params.Add("AccountKey", 2236642);
                Params.Add("PurposeNumber", 3);
                obj = new PDFGenerationObject(Params, 4006);
                //OutName = generator.GenerateDocument(obj);

                // Quote
                Params = new Dictionary<string, object>();
                Params.Add("AccountKey", 2262722);
                obj = new PDFGenerationObject(Params, 4010);
                OutName = generator.GenerateDocument(obj);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        static void c_OnError(object sender, EventArgs e)
        {
            Console.WriteLine(((ErrorEventArgs)e).ex.ToString());
        }
    }
}
