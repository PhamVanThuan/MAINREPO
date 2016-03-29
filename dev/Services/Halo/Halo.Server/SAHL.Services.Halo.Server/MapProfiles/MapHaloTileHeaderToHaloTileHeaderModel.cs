using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using AutoMapper;

using SAHL.Core;
using SAHL.Core.Data;
using SAHL.UI.Halo.Shared;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.Services.Interfaces.Halo.Models.Configuration;

namespace SAHL.Services.Halo.Server.MapProfiles
{
    public class MapHaloTileHeaderToHaloTileHeaderModel : Profile, IAutoMapperProfile
    {
        private readonly ITileConfigurationRepository tileConfigurationRepository;

        public MapHaloTileHeaderToHaloTileHeaderModel(ITileConfigurationRepository tileConfigurationRepository)
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
            Mapper.CreateMap<IHaloTileHeader, HaloTileHeaderModel>()
                .ForMember(model => model.Text, expression => expression.Ignore())
                .ForMember(model => model.Icons, expression => expression.Ignore())
                .ForMember(model => model.Data, expression => expression.Ignore())
                .AfterMap((header, model) =>
                    {
                        model.Text  = this.RetrieveHeaderText(header, model.Data);
                        model.Icons = this.RetrieveHeaderIcons(header, model.Data);
                    });
        }

        private string RetrieveHeaderText(IHaloTileHeader tileHeader, IDataModel dataModel)
        {
            var headerText = new StringBuilder();
            var textProviders = this.tileConfigurationRepository.FindAllTileHeaderTextProviders(tileHeader);
            if (textProviders == null) { return headerText.ToString(); }

            foreach (var currentTextProvider in textProviders)
            {
                currentTextProvider.Execute(dataModel);
                headerText.Append(currentTextProvider.HeaderText);
            }

            return headerText.ToString();
        }

        private IEnumerable<HaloTileHeaderIconModel> RetrieveHeaderIcons(IHaloTileHeader tileHeader, IDataModel dataModel)
        {
            var iconTextModels = new List<HaloTileHeaderIconModel>();

            var iconProviders = this.tileConfigurationRepository.FindAllTileHeaderIconProviders(tileHeader);
            if (iconProviders == null) { return null; }

            foreach (var currentIconProvider in iconProviders)
            {
                currentIconProvider.Execute(dataModel);
                iconTextModels.AddRange(currentIconProvider.HeaderIcons.Select(headerIcon => new HaloTileHeaderIconModel
                                                                                        {
                                                                                            Alignment = currentIconProvider.IconAlignment, 
                                                                                            IconName  = headerIcon
                                                                                        }));
            }

            return iconTextModels;
        }
    }
}
