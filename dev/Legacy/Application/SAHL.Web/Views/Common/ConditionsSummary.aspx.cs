using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common
{
    public partial class ConditionsSummary : SAHLCommonBaseView,IConditionsSummary
    {

        /// <summary>
        /// Gets or Sets the txtDisplay value
        /// </summary>
        public string SettxtDisplay
        {
            set { txtDisplay.Text = value; }
            get { return txtDisplay.Text; }
        }

        /// <summary>
        /// Gets the txtDisplay.ClientID
        /// </summary>
        public string GettxtDisplayClientID
        {
            get { return txtDisplay.ClientID; }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RegisterClientScripts(System.Text.StringBuilder mBuilder)
        {
            if (!Page.ClientScript.IsClientScriptBlockRegistered(GetType(), "lstScripts"))
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "lstScripts", mBuilder.ToString(), true);

            listSelectedConditions.Attributes.Add("onchange", "cleartext(this.options[selectedIndex].text);");
        }

        /// <summary>
        /// Adds an item to the listbox
        /// </summary>
        /// <param name="li"></param>
        public void AddListBoxItem(ListItem li)
        {
            if (listSelectedConditions != null) listSelectedConditions.Items.Add(li);
        }



    }
}