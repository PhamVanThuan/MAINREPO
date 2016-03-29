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
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.Life.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPremiumQuote : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCalculateButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnAddButtonClicked;


        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lifePolicy"></param>
        /// <param name="monthlyInstalment"></param>
        /// <param name="loanAccount"></param>
        /// <param name="hocFS"></param>
        void BindPremiumDetails(ILifePolicy lifePolicy, double monthlyInstalment, SAHL.Common.BusinessModel.Interfaces.IAccount loanAccount, IHOC hocFS);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationLife"></param>
        /// <param name="loanAccount"></param>
        /// <param name="hocFS"></param>
        void BindPremiumDetails(IApplicationLife applicationLife, SAHL.Common.BusinessModel.Interfaces.IAccount loanAccount, IHOC hocFS);


        /// <summary>
        /// Binds the Assured Lives Grid data
        /// </summary>
        void BindAssuredLivesGrid();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lifePolicyTypes"></param>
        void BindLifePolicyTypes(IEventList<ILifePolicyType> lifePolicyTypes);

        /// <summary>
        /// The list of Legal Entities to bind to the grid
        /// </summary>
        IList<ILegalEntity> lstLegalEntities { get; set; }

        /// <summary>
        /// The list of ages for the selected Legal Entities 
        /// </summary>
        string SelectedAgeList { get; set;}

        /// <summary>
        /// Sets whether to show/hide workflow header
        /// </summary>
        bool ShowWorkFlowHeader { set;}

        /// <summary>
        /// Gets/Sets the LegalEntity Name entered by user
        /// </summary>
        string LegalEntityName { get; set;}

        /// <summary>
        /// Gets/Sets the DateOfBirth entered by user
        /// </summary>
        DateTime? DateOfBirth { get; set;}

        /// <summary>
        /// Gets/Sets the IPBenefitMaxAge
        /// </summary>
        int IPBenefitMaxAge { get; set;}

        /// <summary>
        /// 
        /// </summary>
        int PolicyTypeSelectedValue { get;set;}
    }
}
