using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.ITC;
using SAHL.Services.Interfaces.ITC.Models;
using SAHL.Services.Interfaces.ITC.Queries;
using System;
using System.Collections.Generic;

namespace SAHL.Services.Capitec.Specs.ApplicantITCServiceSpecs
{
    public class when_getting_an_itc_profile_and_none_exist : with_applicant_itc_service
    {
        private static Guid itcID;
        private static ItcProfile result;

        private Establish context = () =>
        {
            itcID = Guid.Parse("{454FD352-28FC-45DC-851F-BBBDE8BF0EC5}");

            itcServiceClient.WhenToldTo<IItcServiceClient>(x => x.PerformQuery(Param<GetCapitecITCProfileQuery>.Matches(m => m.ItcID == itcID))).Callback<GetCapitecITCProfileQuery>(query =>
            {
                query.Result = new ServiceQueryResult<GetITCProfileQueryResult>(new List<GetITCProfileQueryResult>());
            });
        };

        private Because of = () =>
        {
            result = applicantITCService.GetITCProfile(itcID);
        };

        private It should_query_the_itc_service = () =>
        {
            itcServiceClient.WasToldTo(x => x.PerformQuery(Param<GetCapitecITCProfileQuery>.Matches(m => m.ItcID == itcID)));
        };

        private It should_return_nothing = () =>
        {
            result.ShouldBeNull();
        };
    }
}