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
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Collections;


namespace SAHL.Web.Views.Common.Presenters.ManualValuationMainDwellingDetails
{
    /// <summary>
    /// 
    /// </summary>
    public class MainDwellingDetailsBaseOrig : SAHLCommonBasePresenter<IValuationManualPropertyDetailsView>
    {
        protected CBOMenuNode _node;
        protected IEventList<IProperty> _properties;
        protected IValuationDiscriminatedSAHLManual valManual;
        protected IPropertyRepository propRepo;
        protected int _genericKey;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public MainDwellingDetailsBaseOrig(IValuationManualPropertyDetailsView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            if (_view.IsMenuPostBack)
                GlobalCacheData.Remove(ViewConstants.ValuationManual);

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            propRepo = RepositoryFactory.GetRepository<IPropertyRepository>();

            if (GlobalCacheData.ContainsKey(ViewConstants.ValuationManual))
                valManual = GlobalCacheData[ViewConstants.ValuationManual] as IValuationDiscriminatedSAHLManual;
            else
            {
                int ValuationKey;

                if (GlobalCacheData.ContainsKey(ViewConstants.SelectedValuationKey))
                    ValuationKey = Convert.ToInt32(GlobalCacheData[ViewConstants.SelectedValuationKey]);
                else
                    ValuationKey = _properties[0].Valuations[0].Key;

                valManual = propRepo.GetValuationByKey(ValuationKey) as IValuationDiscriminatedSAHLManual;
            }

            _properties = new EventList<IProperty>();
            _properties.Add(_view.Messages, valManual.Property);

            _view.Properties = _properties;
        }

    }
}
