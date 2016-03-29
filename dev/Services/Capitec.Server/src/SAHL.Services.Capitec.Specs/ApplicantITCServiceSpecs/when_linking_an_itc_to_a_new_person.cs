using Machine.Fakes;
using Machine.Specifications;
using System;

namespace SAHL.Services.Capitec.Specs.ApplicantITCServiceSpecs
{
    public class when_linking_an_itc_to_a_new_person : with_applicant_itc_service
    {
        private static Guid itcID, personID;
        private static DateTime itcDate;

        private Establish context = () =>
        {
            itcID = Guid.Parse("{454FD352-28FC-45DC-851F-BBBDE8BF0EC5}");
            personID = Guid.Parse("{9B301048-CEF7-4AE3-9D9E-403CAFFB72CC}");
            itcDate = new DateTime(2015, 01, 09);
        };

        private Because of = () =>
        {
            applicantITCService.LinkItcToPerson(personID, itcID, itcDate);
        };

        private It should_check_if_a_person_itc_exists = () =>
        {
            applicantItcDataService.WasToldTo(x => x.GetPersonITC(personID));
        };

        private It should_save_a_person_itc_record = () =>
        {
            applicantItcDataService.WasToldTo(x => x.SavePersonItc(personID, itcID, itcDate));
        };
    }
}