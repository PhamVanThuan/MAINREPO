using System.Data;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common
{
    public partial class NTUReasons : SAHLCommonBaseView, INTUReasons
    {

        /// <summary>
        /// Bind the Properties Grid
        /// </summary>
        /// <param name="DT"></param>
        public void BindgrdHistory(DataTable DT)
        {
            grdReasons.AutoGenerateColumns = false;
            grdReasons.AddGridBoundColumn("BusinessArea", "Business Area", Unit.Percentage(15), HorizontalAlign.Left, true);
            grdReasons.AddGridBoundColumn("Reason", "Reason", Unit.Percentage(30), HorizontalAlign.Left, true);
            grdReasons.AddGridBoundColumn("Comment", "Comment", Unit.Percentage(30), HorizontalAlign.Left, true);
            grdReasons.AddGridBoundColumn("Date", "Date", Unit.Percentage(15), HorizontalAlign.Left, true);
            grdReasons.AddGridBoundColumn("User", "User", Unit.Percentage(10), HorizontalAlign.Left, true);
            grdReasons.DataSource = DT;
            grdReasons.DataBind();
        }


        /// <summary>
        /// Set the Header/Grouping Text
        /// </summary>
        public string PanelHeader
        {
            get
            {
                return pnlHistory.GroupingText;
            }
            set
            {
                pnlHistory.GroupingText = value;
            }
        }
    }
}