﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.Capitec;
using System;

namespace SAHL.Services.Capitec.Specs.ApplicantITCServiceSpecs
{
    public class when_linking_an_itc_and_person_itc_is_the_same : with_applicant_itc_service
    {
        private static Guid itcID, personID;
        private static DateTime itcDate;
        private static PersonITCDataModel personITC;

        private Establish context = () =>
        {
            itcID = Guid.Parse("{454FD352-28FC-45DC-851F-BBBDE8BF0EC5}");
            personID = Guid.Parse("{9B301048-CEF7-4AE3-9D9E-403CAFFB72CC}");
            itcDate = new DateTime(2015, 01, 09);
            personITC = new PersonITCDataModel(personID, itcID, itcDate);
            applicantItcDataService.WhenToldTo(x => x.GetPersonITC(personID)).Return(personITC);
        };

        private Because of = () =>
        {
            applicantITCService.LinkItcToPerson(personID, itcID, itcDate);
        };

        private It should_check_if_a_person_itc_exists = () =>
        {
            applicantItcDataService.WasToldTo(x => x.GetPersonITC(personID));
        };

        private It should_not_update_the_person_itc_record = () =>
        {
            applicantItcDataService.WasNotToldTo(x => x.UpdatePersonItc(Param.IsAny<Guid>(), Param.IsAny<Guid>(), Param.IsAny<DateTime>()));
        };

        private It should_not_save_a_new_person_itc_record = () =>
        {
            applicantItcDataService.WasNotToldTo(x => x.SavePersonItc(Param.IsAny<Guid>(), Param.IsAny<Guid>(), Param.IsAny<DateTime>()));
        };
    }
}