using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.WCFServices.ComcorpConnector.Managers.ImagingRequest;
using SAHL.WCFServices.ComcorpConnector.Managers.ImagingRequest.Statements;
using System;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.ImagingRequestData
{
    public class when_checking_if_a_request_exists_and_it_does : WithCoreFakes
    {
        private static ImagingRequestDataManager dataManager;
        private static FakeDbFactory dbFactory;
        private static Guid imagingReference;
        private static ComcorpImagingRequestDataModel imagingRequest;
        private static bool result;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            dataManager = new ImagingRequestDataManager(dbFactory);

            imagingReference = Guid.NewGuid();
            imagingRequest = new ComcorpImagingRequestDataModel(25, imagingReference, 4, 2);
            dbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectOne(Param<DoesImagingRequestExistForReference>.Matches(y =>
                    y.ImagingReference == imagingReference))).Return(1);
        };

        private Because of = () =>
        {
            result = dataManager.DoesImagingReferenecExist(imagingReference);
        };

        private It should_select_the_imaging_request = () =>
        {
            dbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x =>
                x.SelectOne(Param<DoesImagingRequestExistForReference>.Matches(y =>
                    y.ImagingReference == imagingReference)));
        };

        private It should_return_the_result = () =>
        {
            result.ShouldBeTrue();
        };
    }
}