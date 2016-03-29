using SAHL.Core;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.DomainProcess;
using SAHL.Core.Web.Services;
using System;

namespace SAHL.Services.DomainProcessManager.Data
{
    public interface IDomainProcessRepository
    {
        void Add<T>(T domainProcess) where T : class, IDomainProcess;

        void Update<T>(T domainProcess) where T : class, IDomainProcess;

        void Delete<T>(T domainProcess) where T : class, IDomainProcess;

        T Find<T>(Guid domainProcessId) where T : class, IDomainProcess;
    }

    public class DomainProcessRepository : IDomainProcessRepository
    {
        private readonly IJsonActivator jsonActivator;
        private readonly IIocContainer iocContainer;

        public DomainProcessRepository(IJsonActivator jsonActivator, IIocContainer iocContainer)
        {
            if (jsonActivator == null) { throw new ArgumentNullException("jsonActivator"); }
            if (iocContainer == null) { throw new ArgumentNullException("iocContainer"); }

            this.jsonActivator = jsonActivator;
            this.iocContainer = iocContainer;
        }

        public void Add<T>(T domainProcess) where T : class, IDomainProcess
        {
            if (domainProcess == null) { throw new ArgumentNullException("domainProcess"); }

            var domainProcessDataModel = this.CreateDomainProcessDataModel(domainProcess);

            using (var dbContext = new Db().InAppContext())
            {
                dbContext.Insert(domainProcessDataModel);
                dbContext.Complete();
            }
        }

        public void Update<T>(T domainProcess) where T : class, IDomainProcess
        {
            if (domainProcess == null) { throw new ArgumentNullException("domainProcess"); }

            var domainProcessDataModel = this.CreateDomainProcessDataModel(domainProcess);
            domainProcessDataModel.DateModified = DateTime.Now;

            using (var dbContext = new Db().InAppContext())
            {
                dbContext.Update(domainProcessDataModel);
                dbContext.Complete();
            }
        }

        public void Delete<T>(T domainProcess) where T : class, IDomainProcess
        {
            if (domainProcess == null) { throw new ArgumentNullException("domainProcess"); }

            using (var dbContext = new Db().InAppContext())
            {
                dbContext.DeleteWhere<T>("DomainProcessId = @DomainProcessId", domainProcess.DomainProcessId);
                dbContext.Complete();
            }
        }

        public T Find<T>(Guid domainProcessId) where T : class, IDomainProcess
        {
            if (domainProcessId == Guid.Empty) { throw new ArgumentNullException("domainProcessId"); }

            T domainProcess = null;
            DomainProcessDataModel dataModel = null;

            using (var dbContext = new Db().InReadOnlyAppContext())
            {
                dataModel = dbContext.GetByKey<DomainProcessDataModel, Guid>(domainProcessId);
                dbContext.Complete();
            }

            if (dataModel != null)
            {
                domainProcess = this.CreateDomainProcess<T>(dataModel);
            }

            return domainProcess;
        }

        private DomainProcessDataModel CreateDomainProcessDataModel<T>(T domainProcess) where T : class, IDomainProcess
        {
            var domainProcessType = domainProcess.GetType().AssemblyQualifiedName;
            var processState = jsonActivator.SerializeObject(domainProcess.ProcessState);
            var dataModel = jsonActivator.SerializeObject(domainProcess.GetDataModel());

            var startResultData = jsonActivator.SerializeObject(domainProcess.StartResultData);

            var status                 = (int)domainProcess.Status;
            var statusReason           = domainProcess.StatusReason;
            var domainProcessDataModel = new DomainProcessDataModel(domainProcess.DomainProcessId, domainProcessType, processState, startResultData, status, statusReason,
                                                                    dataModel, domainProcess.DateCreated, domainProcess.DateModified);
            return domainProcessDataModel;
        }

        private T CreateDomainProcess<T>(DomainProcessDataModel domainProcessDataModel) where T : class, IDomainProcess
        {
            var domainProcessType = Type.GetType(domainProcessDataModel.DomainProcessType);
            var domainProcess = this.iocContainer.GetInstance(domainProcessType) as T;
            if (domainProcess == null) { return null; }

            domainProcess.DomainProcessId = domainProcessDataModel.DomainProcessId;
            domainProcess.DateCreated = domainProcessDataModel.DateCreated;
            domainProcess.DateModified = domainProcessDataModel.DateModified;
            
            domainProcess.StartResultData = jsonActivator.DeserializeObject<IDataModel>(domainProcessDataModel.StartResultData);

            var datamodel = jsonActivator.DeserializeObject<IDataModel>(domainProcessDataModel.DataModel);
            domainProcess.SetDataModel(datamodel);

            var processStateData = jsonActivator.DeserializeObject<IDataModel>(domainProcessDataModel.ProcessState);
            domainProcess.RestoreState(processStateData);

            return domainProcess;
        }
    }
}