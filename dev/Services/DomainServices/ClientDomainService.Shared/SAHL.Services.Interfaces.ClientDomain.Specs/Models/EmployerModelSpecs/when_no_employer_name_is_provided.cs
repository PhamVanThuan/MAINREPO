using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Interfaces.ClientDomain.Specs.Models.EmployerModelSpecs
{
    public class when_no_employer_name_is_provided : WithFakes
    {
        private static string employerName, telephoneNumber, telephoneCode, contactPerson, contactEmail;
        private static int? employerKey;
        private static EmployerBusinessType employerBusinessType;
        private static EmploymentSector employmentSector;
        private static EmployerModel model;
        private static Exception ex;

        private Establish context = () =>
        {
            employerName = string.Empty;
            telephoneNumber = string.Empty;
            telephoneCode = string.Empty;
            contactPerson = string.Empty;
            contactEmail = string.Empty;
            employerKey = null;
            employerBusinessType = EmployerBusinessType.Company;
            employmentSector = EmploymentSector.FinancialServices;
        };

        private Because of = () =>
        {
            ex = Catch.Exception(() => {
                model = new EmployerModel(employerKey, employerName, telephoneCode, telephoneNumber, contactPerson, contactEmail, employerBusinessType, employmentSector);
            });
        };
        private It should_throw_a_validation_exception = () =>
        {
            ex.ShouldBeOfExactType(typeof(ValidationException));
        };

        private It should_contain_a_message = () =>
        {
            ex.Message.ShouldEqual("The EmployerName field is required.");
        };
    }
}