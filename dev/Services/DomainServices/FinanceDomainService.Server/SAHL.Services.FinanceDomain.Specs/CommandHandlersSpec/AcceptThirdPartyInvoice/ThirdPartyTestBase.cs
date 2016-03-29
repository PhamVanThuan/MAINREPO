using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.FinanceDomain.CommandHandlers;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.DocumentManager.Models;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.AcceptThirdPartyInvoice
{
    public class ThirdPartyTestBase : WithCoreFakes
    {
        protected static IDomainQueryServiceClient domainQueryServiceClient;
        protected static int accountNumber;
        protected static Interfaces.FinanceDomain.Model.AttorneyInvoiceDocumentModel invoiceDocument;
        protected static AcceptThirdPartyInvoiceCommandHandler commandHandler;
        protected static AcceptThirdPartyInvoiceCommand command;
        protected static string loanNumber;
        protected static string mailSubject;
        protected static string mailFrom;
        protected static DateTime dateReceived;
        protected static string fileExtension;
        protected static int thirdPartyInvoiceKey;
        protected static string fileName;
        protected static InvoiceAttachment attachment;
        protected static IDomainRuleManager<IAccountRuleModel> domainRuleManager;
        protected static IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;

        private Establish context = () =>
        {
            serviceCoordinator = new ServiceCoordinator();
            domainQueryServiceClient = An<IDomainQueryServiceClient>();
            domainRuleManager = An<IDomainRuleManager<IAccountRuleModel>>();
            thirdPartyInvoiceDataManager = An<IThirdPartyInvoiceDataManager>();
            loanNumber = "12345";
            thirdPartyInvoiceKey = 1001;
            mailFrom = "lawyer@attorneys.co.za";
            mailSubject = "342 My invoice";
            fileName = "My Invoice Document.pdf";
            fileExtension = "pdf";
            dateReceived = DateTime.Now.AddDays(-2);
            attachment = new InvoiceAttachment(fileName, fileExtension, "Something here");
            accountNumber = 2210012;
            invoiceDocument = new Interfaces.FinanceDomain.Model.AttorneyInvoiceDocumentModel(loanNumber, dateReceived,
                       DateTime.Now, mailFrom, mailSubject, fileName, fileExtension, "", attachment.FileContents);
            messages = SystemMessageCollection.Empty();
            command = new AcceptThirdPartyInvoiceCommand(accountNumber, invoiceDocument, (int)ThirdPartyType.Attorney);
            commandHandler = new AcceptThirdPartyInvoiceCommandHandler(unitOfWorkFactory, domainRuleManager, domainQueryServiceClient,
                serviceCommandRouter, linkedKeyManager, eventRaiser, serviceCoordinator, thirdPartyInvoiceDataManager);
            linkedKeyManager.WhenToldTo(x => x.RetrieveLinkedKey(Param.IsAny<Guid>())).Return(thirdPartyInvoiceKey);
        };
    }
}