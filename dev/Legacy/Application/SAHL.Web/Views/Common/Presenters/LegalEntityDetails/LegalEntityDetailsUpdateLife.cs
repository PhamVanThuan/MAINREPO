using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces;


namespace SAHL.Web.Views.Common.Presenters.LegalEntityDetails
{
    /// <summary>
    /// 
    /// </summary>
    public class LegalEntityDetailsUpdateLife : LegalEntityDetailsUpdateBase
    {
        /// <summary>
        /// Presenter constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LegalEntityDetailsUpdateLife(ILegalEntityDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            ILifeInsurableInterest lifeInsurableInterest = null;
            string lifeInsurableInterestKey = String.Empty;

            // disable the ajax functionality so that the users cannot use the idnumber ajax to "pull in" another legal entities information
            //_view.DisableAjaxFunctionality = true;

            LoadLegalEntityFromCBO();

            // get the CBO parent node of the current node (of type Account)
            CBOMenuNode cboMenuNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (cboMenuNode != null)
                cboMenuNode = cboMenuNode.GetParentNodeByType(SAHL.Common.Globals.GenericKeyTypes.Account) as CBOMenuNode;

            if (cboMenuNode != null)
            {
                lifeInsurableInterest = LegalEntity.GetInsurableInterest((int)cboMenuNode.GenericKey);
                lifeInsurableInterestKey =  lifeInsurableInterest != null ? lifeInsurableInterest.Key.ToString() : "0";
            }

            // Bind additional life-specific stuff
            _view.BindInsurableInterestType(LookupRepository.LifeInsurableInterestTypes.BindableDictionary, lifeInsurableInterestKey);

            BindLegalEntity();

        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return; 

            View.NonContactDetailsDisabled = true;
        }

        protected override void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            // The base will attempt to navigate, so save first
            base.OnSubmitButtonClicked(sender, e);
        }
    }
}