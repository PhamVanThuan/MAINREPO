using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using System.Data;

namespace SAHL.Web.Views.Migrate.Interfaces
{
    public interface ICreateCase : IViewBase
    {
        /// <summary>
        /// Submit Button Event
        /// </summary>
        event EventHandler OnSubmitButtonClicked;

        int WizardPage { get; set; }

        int AccountKey { get; set; }

		/// <summary>
		/// Populate the Case Details
		/// </summary>
		void PopulateCaseDetails();

        DataTable ConsultantUsers { get; set; }

        /// <summary>
        /// A list of selected LE's in the view
        /// </summary>
        IDictionary<Int32, bool> ListDCLegalEntities { get; }

        /// <summary>
        /// 
        /// </summary>
        IMigrationDebtCounselling SelectedCase { get; set; }

        DateTime? Get171DateFromEworks { get; set; }
        DateTime? Get60DaysDate { get; set; }
    }
}
