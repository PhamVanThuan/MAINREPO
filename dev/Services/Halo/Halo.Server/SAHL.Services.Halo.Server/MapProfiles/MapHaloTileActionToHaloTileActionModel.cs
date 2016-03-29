using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using AutoMapper;

using SAHL.Core;
using SAHL.Services.Interfaces.Halo;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.Services.Halo.Server.MapProfiles
{
    public class MapHaloTileActionToHaloTileActionModel : Profile, IAutoMapperProfile
    {
        public MapHaloTileActionToHaloTileActionModel()
        {
        }

        public override string ProfileName
        {
            get { return this.GetType().Name; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<IHaloTileAction, HaloTileActionModel>()
                .Include<IHaloTileActionDrilldown, HaloTileActionModel>()
                .Include<IHaloTileActionEdit, HaloTileActionModel>()
                .Include<IHaloTileActionWizard, HaloTileActionModel>()
                .ForMember(model => model.Name, expression => expression.MapFrom(configuration => configuration.Name))
                .ForMember(model => model.IconName, expression => expression.MapFrom(configuration => configuration.IconName))
                .ForMember(model => model.Group, expression => expression.MapFrom(configuration => configuration.Group))
                .ForMember(model => model.Sequence, expression => expression.MapFrom(configuration => configuration.Sequence))
                .ForMember(model => model.ActionType, expression => expression.MapFrom(configuration => this.RetrieveActionType(configuration)))
                .ForMember(model => model.TileConfiguration, expression => expression.Ignore())
                .ForMember(model => model.WizardTileConfiguration, expression => expression.Ignore())
                .ForMember(model => model.RootTileConfigurationName, expression => expression.MapFrom(configuration => this.RetrieveDrilldownActionRootTileConfigurationName(configuration)))
                .ForMember(model => model.ProcessName, expression => expression.Ignore())
                .ForMember(model => model.WorkflowName, expression => expression.Ignore())
                .ForMember(model => model.InstanceId, expression => expression.Ignore())
                .AfterMap((action, model) =>
                    {
                        model.TileConfiguration = action.TileConfiguration;

                        if (action is IHaloTileActionWizard)
                        {
                            model.WizardTileConfiguration = ((IHaloTileActionWizard) action).WizardTileConfiguration;
                        }
                    });

            Mapper.CreateMap<IHaloWorkflowAction, HaloTileActionModel>()
                .ForMember(model => model.Name, expression => expression.MapFrom(configuration => configuration.Name))
                .ForMember(model => model.IconName, expression => expression.MapFrom(configuration => configuration.IconName))
                .ForMember(model => model.Group, expression => expression.MapFrom(configuration => configuration.Group))
                .ForMember(model => model.Sequence, expression => expression.MapFrom(configuration => configuration.Sequence))
                .ForMember(model => model.ActionType, expression => expression.MapFrom(configuration => this.RetrieveActionType(configuration)))
                .ForMember(model => model.TileConfiguration, expression => expression.Ignore())
                .ForMember(model => model.WizardTileConfiguration, expression => expression.Ignore())
                .ForMember(model => model.RootTileConfigurationName, expression => expression.Ignore())
                .ForMember(model => model.ProcessName, expression => expression.MapFrom(configuration => configuration.ProcessName))
                .ForMember(model => model.WorkflowName, expression => expression.MapFrom(configuration => configuration.WorkflowName))
                .ForMember(model => model.InstanceId, expression => expression.MapFrom(configuration => configuration.InstanceId));
        }

        private string RetrieveActionType(IHaloAction tileAction)
        {
            if (tileAction is IHaloTileActionDrilldown)
            {
                return "DrillDown";
            }

            if (tileAction is IHaloTileActionEdit)
            {
                return "Edit";
            }

            if (tileAction is IHaloTileActionWizard)
            {
                return "Wizard";
            }

            if (tileAction is IHaloWorkflowAction)
            {
                return "Workflow";
            }

            return "Unknown";
        }

        private string RetrieveDrilldownActionRootTileConfigurationName(IHaloTileAction tileAction)
        {
            var drilldownAction = tileAction as IHaloTileActionDrilldown;
            return drilldownAction == null 
                        ? string.Empty 
                        : drilldownAction.RootTileConfiguration.Name;
        }
    }
}
