using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Model;
using SAHL.Services.Interfaces.DomainQuery.Queries;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.AddApplicationMailingAddress
{
    public class when_unit_of_work_is_provided : WithCoreFakes
    {
        private static AddApplicationMailingAddressCommand command;
        private static AddApplicationMailingAddressCommandHandler handler;
        private static ApplicationMailingAddressModel model;
        private static IDomainQueryServiceClient domainQueryService;
        private static IApplicationDataManager applicationDataManager;
        private static IApplicantDataManager applicantDataManager;
        private static int addressKey, clientKey;
        private static IDomainRuleManager<ApplicationMailingAddressModel> domainRuleManager;

        private Establish context = () =>
        {
            applicationDataManager = An<IApplicationDataManager>();
            applicantDataManager = An<IApplicantDataManager>();
            domainQueryService = An<IDomainQueryServiceClient>();
            domainRuleManager = An<IDomainRuleManager<ApplicationMailingAddressModel>>();

            addressKey = 232;
            clientKey = 6398;
            handler = new AddApplicationMailingAddressCommandHandler(domainQueryService, applicationDataManager, domainRuleManager, unitOfWorkFactory, eventRaiser, applicantDataManager);
            model = new ApplicationMailingAddressModel(1234567, 12345, CorrespondenceLanguage.English, OnlineStatementFormat.PDFFormat,
                CorrespondenceMedium.Email, 123587, true);
            command = new AddApplicationMailingAddressCommand(model);
            applicationDataManager.WhenToldTo(x => x.DoesApplicationMailingAddressExist(command.model.ApplicationNumber)).Return(false);

            applicationDataManager.WhenToldTo(x => x.DoesOpenApplicationExist(command.model.ApplicationNumber)).Return(true);

            var clientAddressQueryResult = new ServiceQueryResult<GetClientAddressQueryResult>(
                new GetClientAddressQueryResult[]
                {
                    new GetClientAddressQueryResult() { AddressKey = addressKey, ClientKey = clientKey }
                });

            domainQueryService.WhenToldTo(d => d.PerformQuery(Param.IsAny<GetClientAddressQuery>()))
                .Return<GetClientAddressQuery>(y => { y.Result = clientAddressQueryResult; return messages; });
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_create_unit_of_work = () =>
        {
            unitOfWorkFactory.WasToldTo(x => x.Build());
        };

        private It should_complete_unit_of_work = () =>
        {
            unitOfWork.WasToldTo(x => x.Complete());
        };
    }
}