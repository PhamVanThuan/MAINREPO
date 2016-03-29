using System;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Globals;
using System.Linq;
using SAHL.Web.Views.Common.Models.Affordability;

namespace SAHL.Web.Views.Common.Presenters.AffordabilityDetailsPresenters
{
    public class AffordabilityDetailsOverall : AffordabilityDetailsBase
    {
        public AffordabilityDetailsOverall(IAffordabilityDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;


            OfferRoleTypes[] roleTypes = new OfferRoleTypes[] {
                    OfferRoleTypes.MainApplicant,
                    OfferRoleTypes.Suretor,
                    OfferRoleTypes.LeadMainApplicant,
                    OfferRoleTypes.LeadSuretor};

            IReadOnlyEventList<ILegalEntity> lstLegalEntities = Application.GetLegalEntitiesByRoleType(roleTypes);

            IList<SAHL.Web.Views.Common.Models.Affordability.AffordabilityModel> affordabilityModel = new List<SAHL.Web.Views.Common.Models.Affordability.AffordabilityModel>();
            foreach (var legalEntity in lstLegalEntities)
            {
               
                foreach (var item in legalEntity.LegalEntityAffordabilities.Where(x => x.Application.Key == Application.Key))
                {
                    var AffordabilityModel = new AffordabilityModel(item.AffordabilityType.Key, item.Description, false, item.Amount, (AffordabilityTypeGroups)item.AffordabilityType.AffordabilityTypeGroup.Key, item.AffordabilityType.Description, item.AffordabilityType.Sequence);
                    if (affordabilityModel.Where(x => x.Key == AffordabilityModel.Key).FirstOrDefault() != null)
                    {
                        affordabilityModel.Update(x => x.Amount = (x.Key == item.AffordabilityType.Key) ? x.Amount + item.Amount : x.Amount);
                    }
                    else
                    {
                        affordabilityModel.Add(AffordabilityModel);
                    }
                }

                _view.Affordability = affordabilityModel.OrderBy(x => x.AffordabilityTypeGroups);
            }

            _view.ReadOnly = true;

            _view.BindControls();

        }
    }
}
