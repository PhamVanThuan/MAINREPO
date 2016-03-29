using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface IConditionsSummary : IViewBase
    {


        /// <summary>
        /// Gets or Sets the txtDisplay value
        /// </summary>
        string SettxtDisplay { set; get;}

        /// <summary>
        /// Gets the txtDisplay.ClientID
        /// </summary>
        string GettxtDisplayClientID { get;}

        /// <summary>
        /// Register the Client Javascripts
        /// </summary>
        /// <param name="mBuilder"></param>
        void RegisterClientScripts(System.Text.StringBuilder mBuilder);

        /// <summary>
        /// Adds an item to the listbox
        /// </summary>
        /// <param name="li"></param>
        void AddListBoxItem(ListItem li);
    }
}
