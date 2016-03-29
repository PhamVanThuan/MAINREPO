using AutoMapper;

using SAHL.Core;
using SAHL.Services.Interfaces.Halo;

namespace SAHL.Services.Halo.Server
{
    public class MapHaloModuleModelToHaloModuleTileModel : Profile, IAutoMapperProfile
    {
        public override string ProfileName
        {
            get { return this.GetType().Name; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<HaloModuleModel, HaloModuleTileModel>()
                .ForMember(model => model.RootTileConfigurations, expression => expression.Ignore())
                .ForMember(model => model.AllRoots, expression => expression.Ignore())
                .ForMember(model => model.ChildTileConfigurations, expression => expression.Ignore())
                .ForMember(model => model.ModuleParameters, expression => expression.Ignore())
                .ForMember(model => model.RoleModel, expression => expression.Ignore());
        }
    }
}
