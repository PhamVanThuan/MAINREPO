using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace SAHL.Web.Controls
{
    public class CapitecImageTemplate : ITemplate
    {
        public void InstantiateIn(Control container)
        {
            ASPxImage image = new ASPxImage();
            GridViewDataItemTemplateContainer gridContainer = (GridViewDataItemTemplateContainer)container;
            string isCapitec = DataBinder.Eval(gridContainer.DataItem, "IsCapitec").ToString();
            if (isCapitec == "1" || isCapitec == "True")
                image.ImageUrl = "../../Images/capitec.ico"; // capitec
            else
                image.ImageUrl = "../../Images/favicon.ico"; // sahls
            container.Controls.Add(image);
        }
    }
}