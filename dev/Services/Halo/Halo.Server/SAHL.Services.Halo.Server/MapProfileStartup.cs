using System;
using System.Linq;

using AutoMapper;

using SAHL.Core;
using SAHL.Core.IoC;

namespace SAHL.Services.Halo.Server
{
    public class MapProfileStartup : IStartable
    {
        private readonly IIocContainer iocContainer;

        public MapProfileStartup(IIocContainer iocContainer)
        {
            if (iocContainer == null) { throw new ArgumentNullException("iocContainer"); }
            this.iocContainer = iocContainer;
        }

        public void Start()
        {
            var mapperConfiguration = this.iocContainer.GetInstance<IConfiguration>();
            if (mapperConfiguration == null)
            {
                throw new Exception("AutoMapper Configuration not found in Ioc Container");
            }

            mapperConfiguration.ConstructServicesUsing(this.iocContainer.GetInstance);

            var automapperProfiles  = this.iocContainer.GetAllInstances<IAutoMapperProfile>();
            foreach (var mapProfile in automapperProfiles.Select(profile => profile as Profile))
            {
                mapperConfiguration.AddProfile(mapProfile);
            }

            Mapper.AssertConfigurationIsValid();
        }
    }
}
