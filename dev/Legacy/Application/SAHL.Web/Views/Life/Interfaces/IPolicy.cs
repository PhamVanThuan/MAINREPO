using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI;
using System.Collections.Generic;

namespace SAHL.Web.Views.Life.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPolicy : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnAddLifeButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnRemoveLifeButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnRecalculatePremiumsButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnPremiumCalculatorButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnAcceptPlanButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnDeclinePlanButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnQuoteRequiredButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnConsideringButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnContactPersonSelectedIndexChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountLifePolicy"></param>
        /// <param name="loanAccount"></param>
        /// <param name="mortgageLoanVariableFS"></param>
        /// <param name="mortgageLoanFixedFS"></param>
        /// <param name="hocFS"></param>
        /// <param name="lifeIsConditionOfLoan"></param>
        /// <param name="contactNumber"></param>
        void BindPolicyDetails(IAccountLifePolicy accountLifePolicy, SAHL.Common.BusinessModel.Interfaces.IAccount loanAccount, IMortgageLoan mortgageLoanVariableFS, IMortgageLoan mortgageLoanFixedFS, IHOC hocFS, bool lifeIsConditionOfLoan, string contactNumber);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assuredLives"></param>
        void BindAssuredLivesGrid(IReadOnlyEventList<ILegalEntity> assuredLives);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assuredLives"></param>
        /// <param name="contactPersonKey"></param>
        void BindContactPersons(IReadOnlyEventList<ILegalEntity> assuredLives, int contactPersonKey);

        /// <summary>
        /// The LegalEntityKey of the selected Contact Preson
        /// </summary>
        int ContactPersonKey { get;}

        /// <summary>
        /// Sets whether to show/hide workflow header
        /// </summary>
        bool ShowWorkFlowHeader { set;}
        /// <summary>
        /// Sets whether to show/hide workflow buttons
        /// </summary>
        bool WorkFlowButtonsVisible { set;}
        /// <summary>
        /// Sets whether to show/hide 'Add Life' button
        /// </summary>
        bool ShowAddLifeButton { set;}
        /// <summary>
        /// Sets whether to show/hide 'Remove Life' button
        /// </summary>
        bool ShowRemoveLifeButton { set;}
        /// <summary>
        /// Sets whether to show/hide 'Recalculate Premiums' button
        /// </summary>
        bool ShowRecalculatePremiumsButton { set;}
        /// <summary>
        /// Sets whether to show/hide 'Premium Calculator' button
        /// </summary>
        bool ShowPremiumCalculatorButton { set;}
        /// <summary>
        /// Sets whether to enable/disable the Contact Name dropdown list
        /// </summary>
        bool ContactPersonEnabled { set;}
        /// <summary>
        /// Gets/Sets whether we are in Confirm Mode
        /// </summary>
        bool ConfirmMode { get; set;}
        /// <summary>
        /// Sets whether to show/hide the Reassurance fields
        /// </summary>
        bool ShowReassuranceFields { get; set;}
        /// <summary>
        /// Sets whether to show/hide the Claim Status information
        /// </summary>
        bool ShowClaimStatusInformation { set;}
        /// <summary>
        /// Sets the Loan Pipeline Status
        /// </summary>
        string LoanPipelineStatus { set;}

        bool ManualLifePolicyPaymentVisible { set; }
    }
}
