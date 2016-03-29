using SAHL.UI.Halo.Models.Client;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.Detail
{
    public class ClientDetailRootTileHeaderLeftIconProvider : HaloTileHeaderIconProviderBase<ClientDetailRootTileHeaderConfiguration>
    {
        public ClientDetailRootTileHeaderLeftIconProvider()
            : base(HaloTileIconAlignment.Left)
        {
        }

        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            var model = dataModel as ClientRootModel;
            if (model == null) { return; }
            string iconName = "";
            if (model.LegalEntityTypeKey == Core.BusinessModel.Enums.LegalEntityType.NaturalPerson)
            {
                iconName = model.Gender == SAHL.Core.BusinessModel.Enums.Gender.Male ? "fa-male" : "fa-female";
                switch (model.Status)
                {
                    case SAHL.Core.BusinessModel.Enums.LegalEntityStatus.Alive:
                        iconName += " status-alive";
                        break;
                    case SAHL.Core.BusinessModel.Enums.LegalEntityStatus.Deceased:
                        iconName += " status-deceased";
                        break;
                    default:
                        iconName += " status-disabled";
                        break;
                }

                if (model.CitizenTypeKey == Core.BusinessModel.Enums.CitizenType.SACitizen || model.CitizenTypeKey == Core.BusinessModel.Enums.CitizenType.SACitizen_NonResident)
                {
                    this.HeaderIcons.Add("flag-icon flag-icon-za");
                }
            }
            else
            {
                iconName = "fa-building";
            }


            this.HeaderIcons.Add(iconName);
        }
    }
}