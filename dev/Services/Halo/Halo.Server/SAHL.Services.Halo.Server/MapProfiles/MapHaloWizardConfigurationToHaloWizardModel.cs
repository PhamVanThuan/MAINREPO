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
    public class MapHaloWizardConfigurationToHaloWizardModel : Profile, IAutoMapperProfile
    {
        private readonly ITileWizardConfigurationRepository wizardConfigurationRepository;

        public MapHaloWizardConfigurationToHaloWizardModel(ITileWizardConfigurationRepository wizardConfigurationRepository)
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
            Mapper.CreateMap<IHaloWizardTileConfiguration, HaloWizardModel>()
                .ForMember(model => model.Name, expression => expression.MapFrom(configuration => configuration.Name))
                .ForMember(model => model.WizardType, expression => expression.MapFrom(configuration => configuration.WizardType))
                .ForMember(model => model.WizardPages, expression => expression.Ignore())
                .AfterMap((configuration, model) =>
                    {
                        var pageConfigurations = this.wizardConfigurationRepository.FindWizardTilePageConfigurations(configuration);
                        if (pageConfigurations == null || !pageConfigurations.Any()) { return; }

                        model.WizardPages = this.CreateHaloWizardPageModels(pageConfigurations);
                    });
        }

        private IEnumerable<HaloWizardPageModel> CreateHaloWizardPageModels(IEnumerable<IHaloWizardTilePageConfiguration> pageConfigurations)
        {
            var pageModels = new List<HaloWizardPageModel>();

            foreach (var pageConfiguration in pageConfigurations)
            {
                var wizardPageModel = Mapper.Map<HaloWizardPageModel>(pageConfiguration);
                pageModels.Add(wizardPageModel);
            }

            return pageModels.OrderBy(model => model.Sequence);
        }
    }
}
