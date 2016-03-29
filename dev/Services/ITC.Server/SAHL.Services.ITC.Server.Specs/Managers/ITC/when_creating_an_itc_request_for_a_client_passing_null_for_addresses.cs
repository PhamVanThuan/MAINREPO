using Machine.Specifications;
using SAHL.Services.Interfaces.AddressDomain.Model;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.ITC.Models;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ITC.Server.Specs.Managers.ITC
{
    public class when_creating_an_itc_request_for_a_client_passing_null_for_addresses : WithITCManager
    {
        private static ItcRequest result;
        private static IEnumerable<GetClientStreetAddressByClientKeyQueryResult> clientAddressList;
        private static ClientDetailsQueryResult clientDetail;

        private Establish context = () =>
        {
            clientAddressList = null;

            clientDetail = clientDetails.First();
        };

        private Because of = () =>
        {
            result = itcManager.CreateITCRequestForApplicant(clientDetails.First(), clientAddressList);
        };

        private It should_not_populate_the_request = () =>
        {
            result.ShouldBeNull();
        };
    }
}