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
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Common.Presenters
{
    public class LegalEntityAssetLiabilityApplicationSummary : LegalEntityAssetLiabilityBase
    {
        IEventList<ILegalEntityAssetLiability> leAssetLiabs = new EventList<ILegalEntityAssetLiability>();

        public LegalEntityAssetLiabilityApplicationSummary(ILegalEntityAssetLiabilityDetails view, SAHLCommonBaseController controller)
        : base(view, controller)
    
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;


            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            
            IApplication app = appRepo.GetApplicationByKey(_genericKey);

            IEventList<ILegalEntityAssetLiability> appAssets = new EventList<ILegalEntityAssetLiability>();

            foreach (IApplicationRole role in app.ApplicationRoles)
            {
                if (role.ApplicationRoleType.ApplicationRoleTypeGroup.Key == (int)SAHL.Common.Globals.OfferRoleTypeGroups.Client)
                {
                    for (int i = 0; i < role.LegalEntity.LegalEntityAssetLiabilities.Count; i++)
                    {
                        // Re TRAC #12275 - only add active assets and liabilities to appAssets
                        if (role.LegalEntity.LegalEntityAssetLiabilities[i].GeneralStatus.Key == (int)GeneralStatuses.Active)
                            appAssets.Add(_view.Messages,role.LegalEntity.LegalEntityAssetLiabilities[i]);
                    }
                }
            }

            _view.ApplicationSummaryMode = true;

            RemoveDuplicateAssets(appAssets);

            if (appAssets.Count > 0)
                _view.BindAssetLiabilityGrid(_view.ViewName, leAssetLiabs);

            _view.OngrdAssetLiabilitySelectedIndexChanged += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OngrdAssetLiabilitySelectedIndexChanged);

        }

        protected void RemoveDuplicateAssets(IEventList<ILegalEntityAssetLiability> appAssets)
        {
            Dictionary<int, string> uniqueStore = new Dictionary<int, string>();

            IEventList<ILegalEntityAssetLiability> leAssetLiabsDuplicates = new EventList<ILegalEntityAssetLiability>();
            
            foreach (ILegalEntityAssetLiability currValue in appAssets)
            {
                if (!uniqueStore.ContainsKey(currValue.AssetLiability.Key))
                {
                    uniqueStore.Add(currValue.AssetLiability.Key, currValue.LegalEntity.DisplayName);
                    leAssetLiabs.Add(_view.Messages, currValue);
                }
                else
                    leAssetLiabsDuplicates.Add(_view.Messages, currValue);
            }

            _view.AssetLiabilityDuplicates = leAssetLiabsDuplicates;
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;

            if (leAssetLiabs != null && leAssetLiabs.Count > 0)
                _view.BindDisplayPanel(leAssetLiabs[gridSelectedIndex]);
        }

        protected void _view_OngrdAssetLiabilitySelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            gridSelectedIndex = Convert.ToInt32(e.Key);
        }
    }
}
