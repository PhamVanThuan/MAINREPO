using SAHL.Core.Data;
using SAHL.Core.Data.Context.Configuration;
using SAHL.Core.IoC;
using StructureMap;

namespace SAHL.Config.Data.Dapper
{
    public class SqlRepositoryFactoryConfig : IStartable
    {
        public void Start()
        {
            DbContextConfiguration.Instance.RepositoryFactory = ObjectFactory.GetInstance<ISqlRepositoryFactory>();
        }
    }
}