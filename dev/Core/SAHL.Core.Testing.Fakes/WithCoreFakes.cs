using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Events;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using System;
using System.Collections.Generic;

namespace SAHL.Core.Testing
{
    public abstract class WithCoreFakes : WithFakes
    {
        protected static IServiceCommandRouter serviceCommandRouter;
        protected static ISystemMessageCollection messages;
        protected static ICombGuid combGuid;
        protected static IUnitOfWorkFactory unitOfWorkFactory;
        protected static IUnitOfWork unitOfWork;
        protected static ILinkedKeyManager linkedKeyManager;
        protected static IEventRaiser eventRaiser;
        protected static IServiceRequestMetadata serviceRequestMetaData;
        protected static IServiceCoordinator serviceCoordinator;

        private Establish context = () =>
        {
            serviceCommandRouter = An<IServiceCommandRouter>();
            messages = An<ISystemMessageCollection>();
            combGuid = An<ICombGuid>();
            eventRaiser = An<IEventRaiser>();
            unitOfWorkFactory = An<IUnitOfWorkFactory>();
            unitOfWork = An<IUnitOfWork>();
            serviceRequestMetaData = An<IServiceRequestMetadata>();
            linkedKeyManager = An<ILinkedKeyManager>();
            serviceCoordinator = An<IServiceCoordinator>();
            unitOfWorkFactory.WhenToldTo(x => x.Build()).Return(unitOfWork);
        };
    }
}