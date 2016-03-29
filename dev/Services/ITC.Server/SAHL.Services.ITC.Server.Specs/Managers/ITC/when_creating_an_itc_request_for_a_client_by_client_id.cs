using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.AddressDomain.Model;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Services.Interfaces.ITC.Models;

namespace SAHL.Services.ITC.Server.Specs.Managers.ITC
{
    public class when_creating_an_itc_request_for_a_client_by_client_id : WithITCManager
    {
        private static ItcRequest result;
        private static List<GetClientStreetAddressByClientKeyQueryResult> clientAddressList;
        private static ClientDetailsQueryResult clientDetail;

        private Establish context = () =>
        {
            clientAddressList = clientAddresses.ToList();

            clientDetail = clientDetails.First();
        };

        private Because of = () =>
        {
            result = itcManager.CreateITCRequestForApplicant(clientDetails.First(), clientAddresses);
        };

        private It should_have_a_matching_Forename1 = () =>
        {
            result.Forename1.ShouldEqual(clientDetail.FirstNames);
        };

        private It should_have_a_matching_Surname = () =>
        {
            result.Surname.ShouldEqual(clientDetail.Surname);
        };

        private It should_have_a_matching_BirthDate = () =>
        {
            result.BirthDate.ShouldEqual(clientDetail.DateOfBirth.Value.ToString("yyyyMMdd"));
        };

        private It should_have_a_matching_IdentityNo1 = () =>
        {
            result.IdentityNo1.ShouldEqual(clientDetail.IDNumber);
        };

        private It should_have_a_matching_Title = () =>
        {
            result.Title.ShouldEqual(((SalutationType)clientDetail.SalutationKey).ToString());
        };

        private It should_have_a_matching_HomeTelNo = () =>
        {
            result.HomeTelNo.ShouldEqual(string.Format("{0}{1}", clientDetail.HomePhoneCode, clientDetail.HomePhoneNumber));
        };

        private It should_have_a_matching_WorkTelNo = () =>
        {
            result.WorkTelNo.ShouldEqual(string.Format("{0}{1}", clientDetail.WorkPhoneCode, clientDetail.WorkPhoneNumber));
        };

        private It should_have_a_matching_CellNo = () =>
        {
            result.CellNo.ShouldEqual(clientDetail.CellPhoneNumber);
        };

        private It should_have_a_matching_EmailAddress = () =>
        {
            result.EmailAddress.ShouldEqual(clientDetail.EmailAddress);
        };

        private It should_have_a_matching_AddressLine1 = () =>
        {
            result.AddressLine1.ShouldEqual(clientAddressList[0].StreetNumber);
        };

        private It should_have_a_matching_AddressLine2 = () =>
        {
            result.AddressLine2.ShouldEqual(clientAddressList[0].StreetName);
        };

        private It should_have_a_matching_Suburb = () =>
        {
            result.Suburb.ShouldEqual(clientAddressList[0].Suburb);
        };

        private It should_have_a_matching_City = () =>
        {
            result.City.ShouldEqual(clientAddressList[0].City);
        };

        private It should_have_a_matching_PostalCode = () =>
        {
            result.PostalCode.ShouldEqual(clientAddressList[0].PostalCode);
        };

        private It should_have_a_matching_Address2Line1 = () =>
        {
            result.Address2Line1.ShouldEqual(clientAddressList[1].StreetNumber);
        };

        private It should_have_a_matching_Address2Line2 = () =>
        {
            result.Address2Line2.ShouldEqual(clientAddresses.ToArray()[1].StreetName);
        };

        private It should_have_a_matching_Address2Suburb = () =>
        {
            result.Address2Suburb.ShouldEqual(clientAddresses.ToArray()[1].Suburb);
        };

        private It should_have_a_matching_Address2PostalCode = () =>
        {
            result.Address2PostalCode.ShouldEqual(clientAddresses.ToArray()[1].PostalCode);
        };

        private It should_have_a_matching_Address2City = () =>
        {
            result.Address2City.ShouldEqual(clientAddresses.ToArray()[1].City);
        };
    }
}
