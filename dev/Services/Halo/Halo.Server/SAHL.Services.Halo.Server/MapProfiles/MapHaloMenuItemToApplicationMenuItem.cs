using AutoMapper;

using SAHL.Core;
using SAHL.Services.Interfaces.Halo;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.Services.Halo.Server
{
    public class MapHaloMenuItemToApplicationMenuItem : Profile, IAutoMapperProfile
    {
        public override string ProfileName
        {
            get { return this.GetType().Name; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<IHaloMenuItem, ApplicationMenuItem>()
                .ForMember(model => model.Name, expression => expression.MapFrom(configuration => configuration.Name))
                .ForMember(model => model.Sequence, expression => expression.MapFrom(configuration => configuration.Sequence))
                .ForMember(model => model.ModuleName, expression => expression.MapFrom(configuration => configuration.ModuleName))
                .ForMember(model => model.IsTileBased, expression => expression.MapFrom(configuration => configuration.IsTileBased))
                .ForMember(model => model.NonTilePageState, expression => expression.MapFrom(configuration => configuration.NonTilePageState));
        }
    }
}
