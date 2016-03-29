using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;

namespace SAHL.Services.ClientDomain.Specs.Rules.IDNumberMustBeUniqueSpecs
{
    public class when_the_ID_number_does_not_exist : WithFakes
    {
        private static IdNumberMustBeUniqueRule rule;
        private static IClientDataManager clientDataManager;
        private static NaturalPersonClientModel model;
        private static LegalEntityDataModel legalEntityDataModel;
        private static ISystemMessageCollection messages;
        private static IValidationUtils validationUtils;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            legalEntityDataModel = null;
            model = new NaturalPersonClientModel("8211045229080", string.Empty, SalutationType.Mr, "Clint", "Speed", "C", "Clint", Gender.Male,
            MaritalStatus.Married_AnteNuptualContract, PopulationGroup.African, CitizenType.SACitizen, DateTime.Now.AddYears(-31), Language.English,
            CorrespondenceLanguage.English, Education.Diploma, "031", "7657192", string.Empty, string.Empty, string.Empty, string.Empty,
            string.Empty, "test@sahomeloans.com");
            clientDataManager = An<IClientDataManager>();
            validationUtils = An<IValidationUtils>();
            rule = new IdNumberMustBeUniqueRule(clientDataManager, validationUtils);
            validationUtils.WhenToldTo(x => x.ValidateIDNumber(model.IDNumber)).Return(true);
            clientDataManager.WhenToldTo(x => x.FindExistingClientByIdNumber(model.IDNumber)).Return(legalEntityDataModel);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_return_no_error_messages = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}