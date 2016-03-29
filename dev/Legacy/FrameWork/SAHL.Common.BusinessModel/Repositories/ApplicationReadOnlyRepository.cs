using System.Linq;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Aspects;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Attributes;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(IApplicationReadOnlyRepository))]
    [ReadUncommittedTransactionAspect]
    public class ApplicationReadOnlyRepository : IApplicationReadOnlyRepository
    {
        private IApplicationRepository applicationrepository;

        public ApplicationReadOnlyRepository(IApplicationRepository applicationrepository)
        {
            this.applicationrepository = applicationrepository;
        }

        public ApplicationReadOnlyRepository()
        {
            if (applicationrepository == null)
            {
                applicationrepository = RepositoryFactory.GetRepository<IApplicationRepository>();
            }
        }

        public int GetApplicationTypeFromApplicationKey(int applicationKey)
        {
            using (TransactionScope scope = new TransactionScope(TransactionMode.New, System.Data.IsolationLevel.ReadUncommitted, OnDispose.Commit))
            {
                int appTypeKey = (from app in Castle.ActiveRecord.Framework.ActiveRecordLinq.AsQueryable<Application_DAO>() where app.Key == applicationKey select app.ApplicationType.Key).Single<int>();

                return appTypeKey;
            }
        }

        public IApplication GetApplicationByKey(int applicationkey)
        {
            return this.applicationrepository.GetApplicationByKey(applicationkey);
        }

        public IApplicationLife GetApplicationLifeByKey(int lifeApplicationkey)
        {
            return this.applicationrepository.GetApplicationLifeByKey(lifeApplicationkey);
        }

        public ICallback GetLatestCallBackByApplicationKey(int applicationKey, bool openCallbacksOnly)
        {
            return this.applicationrepository.GetLatestCallBackByApplicationKey(applicationKey, openCallbacksOnly);
        }
    }
}