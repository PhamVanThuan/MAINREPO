using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.WCFServices.ComcorpConnector.Managers.ImagingRequest;
using System;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.ImagingRequestData
{
    public class when_saving_a_new_imaging_reference : WithCoreFakes
    {
        private static ImagingRequestDataManager dataManager;
        private static FakeDbFactory dbFactory;
        private static Guid imagingReference;
        private static int offerKey;
        private static int expectedDocuments;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            dataManager = new ImagingRequestDataManager(dbFactory);

            imagingReference = Guid.NewGuid();
            offerKey = 150;
            expectedDocuments = 5;
        };

        private Because of = () =>
        {
            dataManager.SaveNewImagingReference(imagingReference, offerKey, expectedDocuments);
        };

        private It should_write_the_new_imaging_reference_to_the_db = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert(Param<ComcorpImagingRequestDataModel>.Matches(y => y.ImagingReference == imagingReference)));
        };
    }
}