using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.Capitec;
using System;
using System.Collections.Generic;

namespace SAHL.Services.Capitec.Specs.ApplicantITCServiceSpecs
{
    public class when_getting_a_valid_itc_and_none_exist : with_applicant_itc_service
    {
        private static string identityNumber;
        private static List<ITCDataModel> itcs;
        private static ITCDataModel result;

        private Establish context = () =>
        {
            itcs = new List<ITCDataModel>
            {
                new ITCDataModel(Guid.Parse("{454FD352-28FC-45DC-851F-BBBDE8BF0EC5}"), DateTime.Now.AddMonths(-3), "Invalid"),
                new ITCDataModel(Guid.Parse("{9B301048-CEF7-4AE3-9D9E-403CAFFB72CC}"), DateTime.Now.AddMonths (-2), "Invalid"),
                new ITCDataModel(Guid.Parse("{53B9959F-3EBF-4BA7-A688-B5C4490D26ED}"), DateTime.Now.AddDays(-32), "Invalid"),
            };
            applicantItcDataService.WhenToldTo(x => x.GetItcModelsForPerson(identityNumber)).Return(itcs);
        };

        private Because of = () =>
        {
            result = applicantITCService.GetValidITCModelForPerson(identityNumber);
        };

        private It should_get_the_itcs_linked_to_that_applicant = () =>
        {
            applicantItcDataService.WasToldTo(x => x.GetItcModelsForPerson(identityNumber));
        };

        private It should_return_nothing = () =>
        {
            result.ShouldBeNull();
        };
    }
}