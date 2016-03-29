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
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.Origination.Interfaces
{
    public interface IApplicationWizardApplicant : IViewBase
    {

        #region Events

        event EventHandler OnCancelButtonClicked;
        event EventHandler OnNextButtonClicked;
        event EventHandler OnBackButtonClicked;

        #endregion

        #region Methods

        /// <summary>
        /// What it says on the tin
        /// </summary>
        /// <param name="applicationSource"></param>
        void PopulateMarketingSource(IEventList<IApplicationSource> applicationSource);

        /// <summary>
        /// Binds an existing legal entity to the controls
        /// </summary>
        /// <param name="legalEntity"></param>
        /// <param name="application"></param>
        void BindExistingLegalEntityAndApplication(ILegalEntity legalEntity, IApplication application);

        #endregion

        #region Properties

        #region LegalEntity

        /// <summary>
        /// 
        /// </summary>
        int ExistingLegalEntityKey
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        string LEFirstNames
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        string LESurname
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        string LEIDNumber
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        string PhoneCode
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        string PhoneNumber
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        int NumberOfApplicants
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        string MarketingSource
        {
            get;
            set;
        }

        #endregion

        bool ShowBackButton { set;}

        bool ShowCancelButton { set;}

        #endregion
    }
}
