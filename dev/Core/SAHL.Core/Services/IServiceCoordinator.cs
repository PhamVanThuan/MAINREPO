using SAHL.Core.SystemMessages;
using System;

namespace SAHL.Core.Services
{
    public interface IServiceCoordinatorExecutor
    {
        ISystemMessageCollection Run();
    }

    public interface IServiceCoordinatorBuilder
    {
        IServiceCoordinatorBuilder Then(Func<ISystemMessageCollection> serviceAction, Func<ISystemMessageCollection> compensationAction);

        IServiceCoordinatorExecutor EndSequence();

        IServiceCoordinatorBuilder ThenWithNoCompensationAction(Func<ISystemMessageCollection> serviceAction);
    }

    public interface IServiceCoordinator
    {
        IServiceCoordinatorBuilder StartSequence(Func<ISystemMessageCollection> serviceAction, Func<ISystemMessageCollection> compensationAction);
    }
}