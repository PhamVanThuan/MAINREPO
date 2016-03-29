using Machine.Specifications;
using Machine.Fakes;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.ClientDomain.CommandHandlers;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Rules;
using SAHL.Services.Interfaces.ClientDomain.Models;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.BusinessModel.Enums;
using NSubstitute;

namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.AddNaturalPersonClient
{
    public class when_adding_with_uppercase_names : WithCoreFakes
    {
        private static IClientDataManager clientDataManager;
        private static AddNaturalPersonClientCommand command;
        private static AddNaturalPersonClientCommandHandler handler;
        private static LegalEntityDataModel legalEntityDataModel;

        private static string submittedFirstNames = "BOBBY CHRISTINA";
        private static string submittedSurname = "BROWN";
        private static string submittedPreferredName = null;

        private static string expectedFirstNames = "Bobby Christina";
        private static string expectedSurname = "Brown";
        private static string expectedPreferredName = null;

        private Establish context = () =>
        {
            var cellNumber = "0845552212";
            var domainRuleManager = An<IDomainRuleManager<INaturalPersonClientModel>>();
            var validationUtils = An<IValidationUtils>();

            clientDataManager = An<IClientDataManager>();

            NaturalPersonClientModel naturalPersonClientModel = new NaturalPersonClientModel
                (null, null, null, submittedFirstNames, submittedSurname, null, submittedPreferredName, null, null, null, null,
                 DateTime.MinValue, null, null, null, null, null, null, null, null, null, cellNumber, null);

            var educationKey = naturalPersonClientModel.Education.HasValue ? 
                              (int)naturalPersonClientModel.Education.Value : (int)Education.Unknown;

            legalEntityDataModel = new LegalEntityDataModel((int)LegalEntityType.NaturalPerson, null, null, null, 
                        DateTime.MinValue, null, naturalPersonClientModel.FirstName, null
                        , naturalPersonClientModel.Surname, naturalPersonClientModel.PreferredName, null
                        , null, null, null, null, null
                        , DateTime.MinValue, null, null, null, null, cellNumber, null, null, null,
                        null, null, (int)LegalEntityStatus.Alive, null, null, null, null,
                        educationKey, null, (int)CorrespondenceLanguage.English, null);

            command = new AddNaturalPersonClientCommand(naturalPersonClientModel);
            handler = new AddNaturalPersonClientCommandHandler(clientDataManager, linkedKeyManager,
                                                               eventRaiser, domainRuleManager, unitOfWorkFactory, 
                                                               validationUtils);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_add_the_new_legal_entity_with_capitalised_names = () =>
        {
            clientDataManager.WasToldTo(x => x.AddNewLegalEntity(Arg.Is<LegalEntityDataModel>
                (c => c.IDNumber == null &&
                    c.FirstNames == expectedFirstNames &&
                        c.Surname == expectedSurname &&
                            c.PreferredName == expectedPreferredName
                )));
        };
    }
}
