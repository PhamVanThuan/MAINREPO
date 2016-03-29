using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.Rules.IDNumberMustBeUniqueSpecs
{
    public class when_the_ID_number_is_invalid : WithFakes
    {
        private static IdNumberMustBeUniqueRule rule;
        private static IValidationUtils validationUtils;
        private static IClientDataManager clientDataManager;
        private static ISystemMessageCollection messages;
        private static NaturalPersonClientModel model;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            model = new NaturalPersonClientModel("8211045229084", string.Empty, SalutationType.Mr, "Clint", "Speed", "C", "Clint", Gender.Male,
            MaritalStatus.Married_AnteNuptualContract, PopulationGroup.African, CitizenType.SACitizen, DateTime.Now.AddYears(-31), Language.English,
            CorrespondenceLanguage.English, Education.Diploma, "031", "7657192", string.Empty, string.Empty, string.Empty, string.Empty,
            string.Empty, "test@sahomeloans.com");
            validationUtils = An<IValidationUtils>();
            clientDataManager = An<IClientDataManager>();
            validationUtils.WhenToldTo(x => x.ValidateIDNumber(Param.IsAny<string>())).Return(false);
            rule = new IdNumberMustBeUniqueRule(clientDataManager, validationUtils);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_not_check_if_the_ID_number_exists = () =>
        {
            clientDataManager.WasNotToldTo(x => x.FindExistingClientByIdNumber(model.IDNumber));
        };

        private It should_not_return_a_message = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}