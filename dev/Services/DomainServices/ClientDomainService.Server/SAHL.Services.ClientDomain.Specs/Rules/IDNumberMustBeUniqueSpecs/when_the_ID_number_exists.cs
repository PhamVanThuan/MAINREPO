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
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.Rules.IDNumberMustBeUniqueSpecs
{
    public class when_the_ID_number_exists : WithFakes
    {
        private static IdNumberMustBeUniqueRule rule;
        private static IClientDataManager clientDataManager;
        private static NaturalPersonClientModel model;
        private static LegalEntityDataModel naturalPersonClient;
        private static ISystemMessageCollection messages;
        private static IValidationUtils validationUtils;
        private static string idNumber;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            naturalPersonClient = new LegalEntityDataModel(1, null, null, null, null, DateTime.Now, null, "Clint", "C", "Speed", "", idNumber,
                                                           "", "", "", "", "", null, "", "", "", "", "", "", "", "", "", null, null, "", null,
                                                           "", null, null, null, 1, null);
            idNumber = "8211045229080";
            model = new NaturalPersonClientModel(idNumber, string.Empty, SalutationType.Mr, "Clint", "Speed", "C", "Clint", Gender.Male,
            MaritalStatus.Married_AnteNuptualContract, PopulationGroup.African, CitizenType.SACitizen, DateTime.Now.AddYears(-31), Language.English,
            CorrespondenceLanguage.English, Education.Diploma, "031", "7657192", string.Empty, string.Empty, string.Empty, string.Empty,
            string.Empty, "test@sahomeloans.com");
            clientDataManager = An<IClientDataManager>();
            validationUtils = An<IValidationUtils>();
            rule = new IdNumberMustBeUniqueRule(clientDataManager, validationUtils);
            clientDataManager.WhenToldTo(x => x.FindExistingClientByIdNumber(model.IDNumber)).Return(naturalPersonClient);
            validationUtils.WhenToldTo(x => x.ValidateIDNumber(model.IDNumber)).Return(true);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_ensure_the_ID_number_is_valid = () =>
        {
            validationUtils.WasToldTo
                (x => x.ValidateIDNumber(model.IDNumber));
        };

        private It should_check_if_a_client_with_the_same_ID_exists = () =>
        {
            clientDataManager.WasToldTo(x => x.FindExistingClientByIdNumber(model.IDNumber));
        };

        private It should_return_an_error_message = () =>
        {
            messages.ErrorMessages().First().Message.
                ShouldEqual(string.Format("A client with Identity Number {0} already exists.", idNumber));
        };
    }
}