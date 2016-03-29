using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Globals;
using System.Linq;

namespace SAHL.Web.Views.Common
{
    public partial class LegalEntityApplications : SAHLCommonBaseView, ILegalEntityApplications
    {
        private string _gridHeading;
        //IOrganisationStructureRepository osRepo;
        private enum GridColumnPositions
        {
            ApplicationKey = 0,
            AccountKey = 1,
            ApplicationType = 2,
            Consultant = 3,
            Status = 4,
            StartDate = 5,
            EndDate = 6
        }

        /// <summary>
        /// DataRowBound event for Applications Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ApplicationsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            TableCellCollection cells = e.Row.Cells;

            if (e.Row.DataItem != null)
            {
                IApplication application = e.Row.DataItem as IApplication;

                cells[(int)GridColumnPositions.ApplicationKey].Text = application.Key.ToString();
                cells[(int)GridColumnPositions.AccountKey].Text = application.ReservedAccount.Key.ToString();
                cells[(int)GridColumnPositions.ApplicationType].Text = application.ApplicationType.Description;

                IApplicationRole appRole = null;
                IWorkflowRole wfRole = null;
                
                switch (application.ApplicationType.Key)
                {
                    case (int)OfferTypes.FurtherLoan:
                    case (int)OfferTypes.FurtherAdvance:
                    case (int)OfferTypes.ReAdvance:
                        appRole = application.GetLatestApplicationRoleByType(OfferRoleTypes.FLProcessorD);
                        break;
                    case (int)OfferTypes.UnsecuredLending:
                        wfRole = X2Repository.GetWorkflowRoleForGenericKey(application.Key, (int)WorkflowRoleTypes.PLConsultantD, (int)GeneralStatuses.Active).FirstOrDefault();
                        break;
                    default:
                        appRole = application.GetLatestApplicationRoleByType(OfferRoleTypes.BranchConsultantD);
                        break;
                }

                if (appRole != null)
                {
                    string consultant = appRole.LegalEntity.GetLegalName(LegalNameFormat.InitialsOnly);
                    if (!String.IsNullOrEmpty(consultant))
                        cells[(int)GridColumnPositions.Consultant].Text = consultant;
                    else
                        cells[(int)GridColumnPositions.Consultant].Text = "Unknown";
                }

                if (wfRole != null)
                {
                    string consultant = wfRole.LegalEntity.GetLegalName(LegalNameFormat.InitialsOnly);
                    if (!String.IsNullOrEmpty(consultant))
                        cells[(int)GridColumnPositions.Consultant].Text = consultant;
                    else
                        cells[(int)GridColumnPositions.Consultant].Text = "Unknown";
                }
                
                cells[(int)GridColumnPositions.Status].Text = application.ApplicationStatus.Description;
                cells[(int)GridColumnPositions.StartDate].Text = application.ApplicationStartDate.HasValue ? application.ApplicationStartDate.Value.ToString(SAHL.Common.Constants.DateFormat) : "";
                cells[(int)GridColumnPositions.EndDate].Text = application.ApplicationEndDate.HasValue ? application.ApplicationEndDate.Value.ToString(SAHL.Common.Constants.DateFormat) : "";
            }
        }

        protected void ApplicationsGrid_GridDoubleClick(object sender, GridSelectEventArgs e)
        {
            btnShowDetails_Click(sender, e);
        }

        protected void btnShowDetails_Click(object sender, EventArgs e)
        {
            int applicationKey = Convert.ToInt32(ApplicationsGrid.Rows[ApplicationsGrid.SelectedIndex].Cells[(int)GridColumnPositions.ApplicationKey].Text);
            OnSelectButtonClicked(sender, new KeyChangedEventArgs(applicationKey));
        }

        #region ILegalEntityApplications Members

        public event KeyChangedEventHandler OnSelectButtonClicked;

        public string GridHeading
        {
            get { return _gridHeading; }
            set { _gridHeading = value; }
        }

        /// <summary>
        /// Bind Applications Grid
        /// 
        /// </summary>
        public void BindApplicationsGrid(IEventList<IApplication> lstApplications)
        {
            if (lstApplications.Count == 0)
                btnShowDetails.Visible = false;

            ApplicationsGrid.HeaderCaption = String.IsNullOrEmpty(_gridHeading) ? "Legal Entity Applications" : _gridHeading;

            ApplicationsGrid.Columns.Clear();

            ApplicationsGrid.AddGridBoundColumn("", "App No.", Unit.Percentage(10), HorizontalAlign.Left, true);
            ApplicationsGrid.AddGridBoundColumn("", "Loan No.", Unit.Percentage(10), HorizontalAlign.Left, true);
            ApplicationsGrid.AddGridBoundColumn("", "Application Type", Unit.Percentage(20), HorizontalAlign.Left, true);
            ApplicationsGrid.AddGridBoundColumn("", "Consultant", Unit.Percentage(30), HorizontalAlign.Left, true);
            ApplicationsGrid.AddGridBoundColumn("", "Status", Unit.Percentage(10), HorizontalAlign.Left, true);
            ApplicationsGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Start Date", false, Unit.Percentage(10), HorizontalAlign.Left, true);
            ApplicationsGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "End Date", false, Unit.Percentage(10), HorizontalAlign.Left, true);

            // sort by latest application first
            lstApplications.Sort(delegate(IApplication a1, IApplication a2)
            { return a2.ApplicationStartDate.Value.CompareTo(a1.ApplicationStartDate.Value); });

            ApplicationsGrid.DataSource = lstApplications;
            ApplicationsGrid.DataBind();
        }

        #endregion

        protected IX2Repository _x2Repository;
        public IX2Repository X2Repository
        {
            get
            {
                if (_x2Repository == null)
                    _x2Repository = RepositoryFactory.GetRepository<IX2Repository>();

                return _x2Repository;
            }
        }
    }
}