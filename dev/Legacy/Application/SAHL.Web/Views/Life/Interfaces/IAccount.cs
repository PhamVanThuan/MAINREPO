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
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.Life.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAccount : IViewBase
    {
        /// <summary>
        /// Binds the data to the ApplicantGrid
        /// </summary>
        /// <param name="lstLegalEntities"></param>
        /// <param name="accountKey"></param>
        void BindApplicantGrid(IReadOnlyEventList<ILegalEntity> lstLegalEntities, int accountKey);

        /// <summary>
        /// Binds the loan summary controls.
        /// </summary>
        /// <param name="loanAccount"></param>
        /// <param name="applicationMortgageLoan"></param>
        /// <param name="applicationInformationVariableLoan"></param>
        /// <param name="applicationInformationVarifixLoan"></param>
        /// <param name="mortgageLoanVariable"></param>
        /// <param name="mortgageLoanFixed"></param>
        void BindLoanSummaryControls(SAHL.Common.BusinessModel.Interfaces.IAccount loanAccount, IApplicationMortgageLoan applicationMortgageLoan, IApplicationInformationVariableLoan applicationInformationVariableLoan, IApplicationInformationVarifixLoan applicationInformationVarifixLoan, IMortgageLoan mortgageLoanVariable, IMortgageLoan mortgageLoanFixed);
    }
}
