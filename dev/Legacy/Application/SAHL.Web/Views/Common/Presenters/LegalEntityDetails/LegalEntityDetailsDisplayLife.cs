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
    /// Life specific Legal Entity Display presenter. Enables and binds additional fields.
    /// </summary>
    public class LegalEntityDetailsDisplayLife : LegalEntityDetailsDisplayBase
    {
        private string insurableInterestDisplay;

        /// <summary>
        /// Presenter Constructor. Takes the <see cref="ILegalEntityDetails">view</see> and the <see cref="SAHLCommonBaseController">Controller</see> as parameters.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LegalEntityDetailsDisplayLife(ILegalEntityDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        /// <summary>
        /// ViewInitialised event handler. Sets additional properties and calls the necessary bind methods.
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        { 
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            // Bind the insurable interest controls (readonly)
            // get the CBO parent node of the current node (of type Account)
            CBOMenuNode parentAccountNode = base.Node.GetParentNodeByType(SAHL.Common.Globals.GenericKeyTypes.Account) as CBOMenuNode;

            if (parentAccountNode != null)
            {
                ILifeInsurableInterest lifeInsurableInterest = LegalEntity.GetInsurableInterest((int)parentAccountNode.GenericKey);
                if (lifeInsurableInterest != null)
                    insurableInterestDisplay = LookupRepository.LifeInsurableInterestTypes.BindableDictionary[lifeInsurableInterest.LifeInsurableInterestType.Key.ToString()];
            }

            _view.BindInsurableInterestReadonly(insurableInterestDisplay);

        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return; 

            _view.InsurableInterestDisplayVisible = true;
            _view.InsurableInterestUpdateVisible = false;
        }
    }
}
