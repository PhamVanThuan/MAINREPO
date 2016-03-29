using System;
using System.Threading.Tasks;

using SAHL.Core.Data;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Core.DomainProcess
{
    public delegate void DomainProcessEventhandler(Guid domainProcessId);

    public interface IDomainProcess : IDataModel
    {
        event EventHandler PersistState;
        event DomainProcessEventhandler Completed;
        event DomainProcessEventhandler ErrorOccurred;

        Guid DomainProcessId { get; set; }
        DateTime? DateCreated { get; set; }
        DateTime? DateModified { get; set; }
        IDataModel ProcessState { get; set; }
        IDataModel StartResultData { get; set; }
        DomainProcessStatus Status { get; }
        string StatusReason { get; }

        void Initialise(IDataModel dataModel);
        Task<IDomainProcessStartResult> Start(IDataModel dataModel, string eventToWaitFor);
        void OnInternalStart();
        void HandleEvent(IEvent @event, IServiceRequestMetadata metadata);

        IDataModel GetDataModel();
        void SetDataModel(IDataModel dataModel);
        void RestoreState(IDataModel dataModel);
    }

    public interface IDomainProcess<TModel> : IDomainProcess where TModel : class, IDataModel
    {
        TModel DataModel { get; set; }
    }
}
