using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Applicant.Statements;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.Applicant
{
    public class when_getting_the_date_of_birth_for_an_applicant : WithFakes
    {
        private static ApplicantDataManager applicantDataManager;
        private static FakeDbFactory fakeDbFactory;
        private static DateTime dateOfBirth;
        private static DateTime? result;
        private static int clientKey;
        private Establish context = () =>
        {
            clientKey = 2343423;
            dateOfBirth = DateTime.Now.AddYears(-49);
            fakeDbFactory = new FakeDbFactory();
            applicantDataManager = new ApplicantDataManager(fakeDbFactory);
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectOne(Param.IsAny<GetClientDateOfBirthStatement>())).Return(dateOfBirth);
        };

        private Because of = () =>
        {
            result = applicantDataManager.GetClientDateOfBirth(clientKey);
        };

        private It should_return_the_date_of_birth = () =>
        {
            result.ShouldEqual(dateOfBirth);
        };

        private It should_use_the_client_key_provided_in_the_sql_statement = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne(Arg.Is<GetClientDateOfBirthStatement>(
                y => y.ClientKey == clientKey)));
        };
    }
}