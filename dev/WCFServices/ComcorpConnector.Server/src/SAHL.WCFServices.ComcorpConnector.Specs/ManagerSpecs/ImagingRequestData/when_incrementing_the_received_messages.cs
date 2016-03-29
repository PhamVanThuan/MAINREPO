using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Identity;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.WCFServices.ComcorpConnector.Managers.ImagingRequest;
using SAHL.WCFServices.ComcorpConnector.Managers.ImagingRequest.Statements;
using System;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.ImagingRequestData
{
    public class when_incrementing_the_received_messages : WithCoreFakes
    {
        private static ImagingRequestDataManager dataManager;
        private static Guid imagingReferenceGuid;
        private static FakeDbFactory dbFactory;

        private Establish context = () =>
        {
            imagingReferenceGuid = CombGuid.Instance.Generate();
            dbFactory = new FakeDbFactory();
            dataManager = new ImagingRequestDataManager(dbFactory);
        };

        private Because of = () =>
        {
            dataManager.IncrementMessagesReceived(imagingReferenceGuid);
        };

        private It should_increment_the_messages_received_column = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Update<ComcorpImagingRequestDataModel>(Param<IncrementMessagesReceived>.Matches(y =>
                y.ImagingReference == imagingReferenceGuid)));
        };
    }
}