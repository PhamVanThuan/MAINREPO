using System;
using System.Linq;
using System.Collections.Generic;

using AutoMapper;

using SAHL.Core;
using SAHL.UI.Halo.Shared;
using SAHL.Services.Interfaces.Halo;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.Services.Halo.Server
{
    public class MapHaloApplicationConfigurationToHaloApplicationModel : Profile, IAutoMapperProfile
    {
        private readonly ITileConfigurationRepository tileConfigurationRepository;

        public MapHaloApplicationConfigurationToHaloApplicationModel(ITileConfigurationRepository tileConfigurationRepository)
        {
            if (tileConfigurationRepository == null) { throw new ArgumentNullException("tileConfigurationRepository"); }
            this.tileConfigurationRepository = tileConfigurationRepository;
        }

        public override string ProfileName
        {
            get { return this.GetType().Name; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<IHaloApplicationConfiguration, HaloApplicationModel>()
                .ForMember(model => model.Name, expression => expression.MapFrom(configuration => configuration.Name))
                .ForMember(model => model.Sequence, expression => expression.MapFrom(configuration => configuration.Sequence))
                .ForMember(model => model.Modules, expression => expression.MapFrom(configuration => this.RetrieveApplicationModuleConfigurations(configuration)));
        }

        private IEnumerable<HaloModuleTileModel> RetrieveApplicationModuleConfigurations(IHaloApplicationConfiguration applicationConfiguration)
        {
            var moduleConfigurations = tileConfigurationRepository.FindApplicationModuleConfigurations(applicationConfiguration);
            if (moduleConfigurations == null || !moduleConfigurations.Any()) { new List<HaloModuleModel>(); }

            return moduleConfigurations.Select(moduleConfiguration => Mapper.Map<HaloModuleModel>(moduleConfiguration))
                                       .Select(moduleModel => Mapper.Map<HaloModuleTileModel>(moduleModel))
                                       .ToList();
        }
    }
}
