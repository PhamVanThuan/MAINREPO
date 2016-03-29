using SAHL.Common.BusinessModel.Config;
using SAHL.Common.BusinessModel.Configuration;
using SAHL.Common.BusinessModel.Interfaces.Configuration;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Batch.PersonalLoanService
{
    public class BootStrapper
    {
        static bool IsActiveRecordInitialised;

        public static void ConfigureStructureMap()
        {
            if (!IsActiveRecordInitialised)
            {
                InfrastructureSetUp();
            }

            ObjectFactory.Configure(x =>
            {
                x.AddRegistry(new PersonalLoanServiceRegistry());
            });
        }

        private static void InfrastructureSetUp()
        {
            IActiveRecordConfigurationProvider configProvider = new ActiveRecordConfigFileConfigurationProvider();
            var initiliser = new ActiveRecordInitialiser(configProvider);
            initiliser.InitialiseActiveRecord();
            IsActiveRecordInitialised = true;
        }
    }
}
