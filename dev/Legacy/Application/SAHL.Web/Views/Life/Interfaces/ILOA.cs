using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.Life.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILOA : IViewBase
    {   
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnNextButtonClicked;

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="offerFS"></param>
        ///// <param name="offerMortgageLoan"></param>
        ///// <param name="offerInformationVariableLoan"></param>
        ///// <param name="offerInformationVarifixLoan"></param>
        ///// <param name="mortgageLoanVariable"></param>
        ///// <param name="mortgageLoanFixed"></param>
        ///// <param name="hoc"></param>
        /// <summary>
        /// Binds the LOA Details data to the appropriate controls
        /// </summary>
        /// <param name="applicationLife"></param>
        /// <param name="applicationMortgageLoan"></param>
        /// <param name="applicationInformationVariableLoan"></param>
        /// <param name="applicationInformationVarifixLoan"></param>
        /// <param name="mortgageLoanVariable"></param>
        /// <param name="mortgageLoanFixed"></param>
        /// <param name="accountHOC"></param>
        void BindLOADetails(IApplication applicationLife, IApplicationMortgageLoan applicationMortgageLoan, IApplicationInformationVariableLoan applicationInformationVariableLoan, IApplicationInformationVarifixLoan applicationInformationVarifixLoan, IMortgageLoan mortgageLoanVariable, IMortgageLoan mortgageLoanFixed, IAccountHOC accountHOC);

        /// <summary>
        /// Binds the Loan Conditions to the grid
        /// </summary>
        /// <param name="loanConditions"></param>
        void BindLoanConditions(IList<string> loanConditions);


        /// <summary>
        /// 
        /// </summary>
        string EmailBody { get; set;}
    }
}
