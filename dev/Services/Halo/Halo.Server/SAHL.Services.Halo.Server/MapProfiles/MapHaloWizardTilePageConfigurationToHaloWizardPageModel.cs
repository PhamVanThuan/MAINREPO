using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using AutoMapper;

using SAHL.Core;
using SAHL.UI.Halo.Shared.Repository;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.Services.Interfaces.Halo.Models.Configuration;

namespace SAHL.Services.Halo.Server.MapProfiles
{
    public class MapHaloWizardTilePageConfigurationToHaloWizardPageModel : Profile, IAutoMapperProfile
    {
        private readonly ITileWizardConfigurationRepository wizardConfigurationRepository;

        public MapHaloWizardTilePageConfigurationToHaloWizardPageModel(ITileWizardConfigurationRepository wizardConfigurationRepository)
        {
            if (wizardConfigurationRepository == null) { throw new ArgumentNullException("wizardConfigurationRepository"); }
            this.wizardConfigurationRepository = wizardConfigurationRepository;
        }

        public override string ProfileName
        {
            get { return this.GetType().Name; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<IHaloWizardTilePageConfiguration, HaloWizardPageModel>()
                .ForMember(model => model.Name, expression => expression.MapFrom(configuration => configuration.Name))
                .ForMember(model => model.WizardPageType, expression => expression.MapFrom(configuration => configuration.WizardPageType))
                .ForMember(model => model.Sequence, expression => expression.MapFrom(configuration => configuration.Sequence))
                .ForMember(model => model.ContentMessage, expression => expression.MapFrom(configuration => configuration.ContentMessage))
                .ForMember(model => model.PageState, expression => expression.Ignore())
                .ForMember(model => model.ContentModel, expression => expression.Ignore())
                .AfterMap((configuration, model) =>
                    {
                        model.PageState    = this.RetrievePageState(configuration);
                        model.ContentModel = this.RetrievePageDataModel(configuration);
                    });
        }

        private IHaloTileModel RetrievePageDataModel(IHaloWizardTilePageConfiguration configuration)
        {
            return configuration == null 
                        ? null 
                        : this.wizardConfigurationRepository.FindWizardPageDataModel(configuration);
        }

        private string RetrievePageState(IHaloWizardTilePageConfiguration configuration)
        {
            if (configuration == null) { return null; }

            var tilePageState = this.wizardConfigurationRepository.FindWizardPageState(configuration);
            return tilePageState == null 
                        ? null 
                        : tilePageState.GetType().FullName;
        }
    }
}
