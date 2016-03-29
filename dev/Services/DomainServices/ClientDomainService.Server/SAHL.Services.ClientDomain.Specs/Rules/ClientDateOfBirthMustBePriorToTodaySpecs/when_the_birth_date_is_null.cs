using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.ClientDomain.Specs.Rules.ClientDateOfBirthMustBePriorToTodaySpecs
{
    public class when_the_birth_date_is_null : WithCoreFakes
    {
        private static ClientDateOfBirthMustBePriorToTodayRule rule;
        private static NaturalPersonClientModel model;
        private static DateTime dateOfBirth;
        private Establish context = () =>
        {
            dateOfBirth = Convert.ToDateTime(null);
            messages = SystemMessageCollection.Empty();
            model = new NaturalPersonClientModel("8211045229080", string.Empty, SalutationType.Mr, "Clint", "Speed", "C", "Clint", Gender.Male,
                MaritalStatus.Married_AnteNuptualContract, PopulationGroup.African, CitizenType.SACitizen, dateOfBirth, Language.English,
                CorrespondenceLanguage.English, Education.Diploma, "031", "7657192", string.Empty, string.Empty, string.Empty, string.Empty,
                string.Empty, "test@sahomeloans.com"); rule = new ClientDateOfBirthMustBePriorToTodayRule();
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_return_no_messages = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}
