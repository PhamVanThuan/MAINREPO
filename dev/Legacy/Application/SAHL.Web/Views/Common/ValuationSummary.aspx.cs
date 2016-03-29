using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using System.Xml;
using System.IO;
using System.Xml.Xsl;

namespace SAHL.Web.Views.Common
{
    public partial class ValuationSummary : SAHLCommonBaseView, IValuationSummary
    {
        public event EventHandler btnCancelClicked;

        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// On Init
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!ShouldRunPage)
                return;
        }

        /// <summary>
        /// Render XML
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="styleSheet"></param>
        public void RenderXml(string xml, string styleSheet)
        {
            XslTransform xsltTransformation = new XslTransform();
            xsltTransformation.Load(XmlReader.Create(new StringReader(styleSheet)));

            panelXML.DocumentContent = xml;
            panelXML.Transform = xsltTransformation;
        }
    }
}