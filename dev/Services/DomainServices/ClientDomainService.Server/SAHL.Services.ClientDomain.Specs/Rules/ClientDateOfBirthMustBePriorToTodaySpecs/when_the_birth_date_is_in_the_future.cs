using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Identity;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.RuleSpecs.ApplicantDateOfBirthMustBePriorToToday
{
    public class when_the_birth_date_is_in_the_future : WithCoreFakes
    {
        private static ClientDateOfBirthMustBePriorToTodayRule rule;
        private static NaturalPersonClientModel model;
        private static DateTime dateOfBirth;

        private Establish context = () =>
        {
            dateOfBirth = DateTime.Now.AddDays(1);
            messages = SystemMessageCollection.Empty();
            model = new NaturalPersonClientModel("8211045229080", string.Empty, SalutationType.Mr, "Clint", "Speed", "C", "Clint", Gender.Male,
            MaritalStatus.Married_AnteNuptualContract, PopulationGroup.African, CitizenType.SACitizen, dateOfBirth, Language.English,
            CorrespondenceLanguage.English, Education.Diploma, "031", "7657192", string.Empty, string.Empty, string.Empty, string.Empty,
            string.Empty, "test@sahomeloans.com"); 
            rule = new ClientDateOfBirthMustBePriorToTodayRule();
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_return_a_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("Client date of birth is invalid.");
        };
    }
}