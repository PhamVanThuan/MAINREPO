using System;
using Machine.Specifications;
using SAHL.Services.Interfaces.ITC.Models;

namespace SAHL.Services.ITC.Server.Specs.Managers.ITC
{
    public class when_creating_an_itc_request_for_an_applicant : WithITCManager
    {
        private static ApplicantITCRequestDetailsModel applicantITCRequestDetails;
        private static ItcRequest result;

        private Establish context = () =>
        {
            applicantITCRequestDetails = new ApplicantITCRequestDetailsModel("Stewart",
                "Smith",
                new DateTime(1990, 11, 21),
                "90045689745632101",
                "Mr",
                "0315689741",
                "",
                "0725698741",
                "stewart@smith.com",
                "123 Park lane",
                "",
                "Appleville",
                "Durban",
                "1154");
        };

        private Because of = () =>
        {
            result = itcManager.CreateITCRequestForApplicant(applicantITCRequestDetails);
        };

        private It should_have_a_matching_Forename1 = () =>
        {
            result.Forename1.ShouldEqual(applicantITCRequestDetails.FirstName);
        };

        private It should_have_a_matching_Surname = () =>
        {
            result.Surname.ShouldEqual(applicantITCRequestDetails.Surname);
        };

        private It should_have_a_matching_BirthDate = () =>
        {
            result.BirthDate.ShouldEqual("19901121");
        };

        private It should_have_a_matching_IdentityNo1 = () =>
        {
            result.IdentityNo1.ShouldEqual(applicantITCRequestDetails.IdentityNumber);
        };

        private It should_have_a_matching_Title = () =>
        {
            result.Title.ShouldEqual(applicantITCRequestDetails.Title);
        };

        private It should_have_a_matching_HomeTelNo = () =>
        {
            result.HomeTelNo.ShouldEqual(applicantITCRequestDetails.HomePhoneNumber);
        };

        private It should_have_a_matching_WorkTelNo = () =>
        {
            result.WorkTelNo.ShouldEqual(applicantITCRequestDetails.WorkPhoneNumber);
        };

        private It should_have_a_matching_CellNo = () =>
        {
            result.CellNo.ShouldEqual(applicantITCRequestDetails.CellPhoneNumber);
        };

        private It should_have_a_matching_EmailAddress = () =>
        {
            result.EmailAddress.ShouldEqual(applicantITCRequestDetails.EmailAddress);
        };

        private It should_have_a_matching_AddressLine1 = () =>
        {
            result.AddressLine1.ShouldEqual(applicantITCRequestDetails.AddressLine1);
        };

        private It should_have_a_matching_AddressLine2 = () =>
        {
            result.AddressLine2.ShouldEqual(applicantITCRequestDetails.AddressLine2);
        };

        private It should_have_a_matching_Suburb = () =>
        {
            result.Suburb.ShouldEqual(applicantITCRequestDetails.Suburb);
        };

        private It should_have_a_matching_City = () =>
        {
            result.City.ShouldEqual(applicantITCRequestDetails.City);
        };

        private It should_have_a_matching_PostalCode = () =>
        {
            result.PostalCode.ShouldEqual(applicantITCRequestDetails.PostalCode);
        };
    }
}
