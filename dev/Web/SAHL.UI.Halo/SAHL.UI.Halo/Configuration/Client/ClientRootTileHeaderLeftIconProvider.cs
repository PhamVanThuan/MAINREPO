using SAHL.Core.BusinessModel.Enums;
using SAHL.UI.Halo.Models.Client;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client
{
    public class ClientRootTileHeaderLeftIconProvider : HaloTileHeaderIconProviderBase<ClientRootTileHeaderConfiguration>
    {
        public ClientRootTileHeaderLeftIconProvider()
            : base(HaloTileIconAlignment.Left)
        {
        }

        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            var model = dataModel as ClientRootModel;
            if (model == null) { return; }

            var iconName = "";

            if (model.LegalEntityTypeKey == LegalEntityType.NaturalPerson)
            {
                iconName = model.Gender == Gender.Male ? "fa-male" : "fa-female";

                switch (model.Status)
                {
                    case LegalEntityStatus.Alive:
                        iconName += " status-alive";
                        break;
                    case LegalEntityStatus.Deceased:
                        iconName += " status-deceased";
                        break;
                    default:
                        iconName += " status-disabled";
                        break;
                }

                if (model.CitizenTypeKey == CitizenType.SACitizen || model.CitizenTypeKey == CitizenType.SACitizen_NonResident)
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
