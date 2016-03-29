using System;

using SAHL.Core.Data;
using SAHL.Core.Events;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.DomainProcess;

namespace SAHL.Services.DomainProcessManager.Tests
{
    public class FakeDomainModel : IDataModel
    {
        public Guid Id { get; set; }
    }

    public class FakeEvent1 : Event
    {
        public FakeEvent1(DateTime date) : base(date)
        {
            // Nothing to process
        }

        public FakeEvent1(Guid id, DateTime date) : base(id, date)
        {
            // Nothing to process
        }
    }

    public class FakeEvent2 : Event
    {
        public FakeEvent2(DateTime date) : base(date)
        {
            // Nothing to process
        }

        public FakeEvent2(Guid id, DateTime date) : base(id, date)
        {
            // Nothing to process
        }
    }

    public class FakeEvent3 : Event
    {
        public FakeEvent3(DateTime date) : base(date)
        {
            // Nothing to process
        }

        public FakeEvent3(Guid id, DateTime date) : base(id, date)
        {
            // Nothing to process
        }
    }

    public class FakeEvent4 : Event
    {
        public FakeEvent4(DateTime date) : base(date)
        {
            // Nothing to process
        }

        public FakeEvent4(Guid id, DateTime date) : base(id, date)
        {
            // Nothing to process
        }
    }

    public class FakeDomainProcess : DomainProcessBase<FakeDomainModel>,
                                     IDomainProcessEvent<FakeEvent1>,
                                     IDomainProcessEvent<FakeEvent2>,
                                     IDomainProcessEvent<FakeEvent4>
    {
        public bool Event1Completed { get; private set; }

        public bool Event2Completed { get; private set; }

        public bool StartResult { get; set; }

        public FakeDomainProcess(
              IRawLogger rawLogger
            , ILoggerSource loggerSource
            , ILoggerAppSource loggerAppSource)
            : base(rawLogger, loggerSource, loggerAppSource)
        {

        }

        public override void HandledEvent(IServiceRequestMetadata metadata)
        {
            // Nothing to handle
        }

        public override void Initialise(IDataModel dataModel)
        {
            // No initialisation required
        }

        public override void OnInternalStart()
        {
            this.SetStartResult(this.StartResult, this.StartResultData);
        }

        public override void RestoreState(IDataModel dataModel)
        {
            this.ProcessState = dataModel;
        }

        public void Handle(FakeEvent1 event1, IServiceRequestMetadata metadata)
        {
            this.Event1Completed = true;
        }

        public void Handle(FakeEvent2 event2, IServiceRequestMetadata metadata)
        {
            this.Event2Completed = true;
            this.OnCompleted(this.DomainProcessId);
        }

        public void Handle(FakeEvent4 event4, IServiceRequestMetadata metadata)
        {
            throw new Exception("Event 4 encountered an exception.");
        }

        public void SetStartResult(bool startResult, IDataModel dataModel)
        {
            var domainProcessStartResult = new DomainProcessStartResult(startResult, dataModel);
            this.StartTask.SetResult(domainProcessStartResult);
        }
    }
}