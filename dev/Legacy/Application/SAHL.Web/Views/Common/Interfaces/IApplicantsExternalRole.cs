using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface IApplicantsExternalRole : IViewBase
    {
        /// <summary>
        /// Reloads the LegalEntity Details section as the user navigates through the grid.
        /// </summary>
        event KeyChangedEventHandler OnGridLegalEntitySelected;

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

        int SelectedLegalEntityKey { get; set; }

        string GridHeading { set; }

        /// <summary>
        /// Binds a list of Applicants to the grid.
        /// </summary>
        void BindExternalRoleApplicantsGrid(IList<SAHL.Common.BusinessModel.Interfaces.IExternalRole> externalRoles);

        /// <summary>
        /// Binds the details of the Legal Entity.
        /// </summary>
        /// <param name="LegalEntity"></param>
        void BindLegalEntityDetails(ILegalEntity LegalEntity);

        /// <summary>
        /// The heading for the Account Information Column
        /// </summary>
        string InformationColumnDescription { set; }
    }
}