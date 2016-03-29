using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface IApplicants : IViewBase
    {
        /// <summary>
        /// Reloads the LegalEntity Details section as the user navigates through the grid.
        /// </summary>
        event KeyChangedEventHandler OnGridLegalEntitySelected;

        /// <summary>
        /// Add the LegalEntity from the Account Roles or Debt Counselling External Role.
        /// </summary>
        event KeyChangedEventHandler OnAddButtonClicked;

        /// <summary>
        /// Removes the LegalEntity from the Account Roles or Debt Counselling External Role.
        /// </summary>
        event KeyChangedEventHandler OnRemoveButtonClicked;

        /// <summary>
        /// Abandon the current action and (normally) revert to display mode.
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// Sets whether the ApplicantDetails section is visible.
        /// </summary>
        bool ApplicantDetailsVisible { set; }

        /// <summary>
        /// Sets whether the Company Details section is visible.
        /// </summary>
        bool ApplicantDetailsCompanyVisible { set; }

        /// <summary>
        /// Sets whether the NaturalPerson Details section is visible.
        /// </summary>
        bool ApplicantDetailsNaturalPersonVisible { set; }

        /// <summary>
        /// Sets whether the Buttons section is visible.
        /// </summary>
        bool ButtonsVisible { set; }

        /// <summary>
        /// Sets whether the Buttons are enabled.
        /// </summary>
        bool RemoveButtonEnabled { set; }

        /// <summary>
        /// Sets whether the Buttons are enabled.
        /// </summary>
        bool AddButtonEnabled { set; }

        /// <summary>
        /// Sets whether the Buttons are enabled.
        /// </summary>
        bool CancelButtonEnabled { set; }

        int SelectedLegalEntityKey { get; set;}
        int SelectedApplicationRoleTypeKey { get; set;}
        int SelectedApplicationRoleKey { get; set; }

        string GridHeading { set; }

        /// <summary>
        /// Sets whether the linked debt counselling accounts warning message is visible.
        /// </summary>
        bool LinkedDebtCounsellingAccountsWarningMessageVisible { set; }

        string LinkedDebtCounsellingAccountsWarningMessage { set; }
        
        /// <summary>
        /// Binds a list of Applicants to the grid.
        /// </summary>
        /// <param name="ApplicationRoles"></param>
        void BindOfferApplicantsGrid(IList<IApplicationRole> ApplicationRoles);

        /// <summary>
        /// Binds a list of Applicants to the grid.
        /// </summary>
        /// <param name="AccountRoles"></param>
        void BindAccountApplicantsGrid(IList<IRole> AccountRoles);

        /// <summary>
        /// Binds the details of the Legal Entity.
        /// </summary>
        /// <param name="LegalEntity"></param>
        void BindLegalEntityDetails(ILegalEntity LegalEntity);

        /// <summary>
        /// The heading for the Account Information Column
        /// </summary>
        /// <param name="description"></param>
        string InformationColumnDescription { set; }
    }
}
