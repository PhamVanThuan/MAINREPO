using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.Capitec;
using System;

namespace SAHL.Services.Capitec.Specs.ApplicantITCServiceSpecs
{
    public class when_getting_an_itc_by_id : with_applicant_itc_service
    {
        private static Guid itcID;
        private static ITCDataModel itc;
        private static ITCDataModel result;

        private Establish context = () =>
        {
            itcID = Guid.Parse("{454FD352-28FC-45DC-851F-BBBDE8BF0EC5}");
            itc = new ITCDataModel(itcID, new DateTime(2015, 01, 09), "Invalid");
            applicantItcDataService.WhenToldTo(x => x.GetItcById(itcID)).Return(itc);
        };

        private Because of = () =>
        {
            result = applicantITCService.GetITC(itcID);
        };

        private It should_get_the_itc = () =>
        {
            applicantItcDataService.WasToldTo(x => x.GetItcById(itcID));
        };

        private It should_return_the_itc = () =>
        {
            result.ShouldEqual(itc);
        };
    }
}