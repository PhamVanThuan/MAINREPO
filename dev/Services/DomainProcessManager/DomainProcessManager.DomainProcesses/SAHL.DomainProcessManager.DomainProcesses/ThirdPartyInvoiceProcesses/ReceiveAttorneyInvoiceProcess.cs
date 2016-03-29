using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Exchange;
using SAHL.Core.Exchange.Provider;
using SAHL.Core.Identity;
using SAHL.Core.Logging;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement.ThirdPartyInvoice;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Communication;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SAHL.DomainProcessManager.DomainProcesses.ThirdPartyInvoiceProcess
{
    public class ReceiveAttorneyInvoiceProcess : ReceiveThirdPartyInvoiceDomainProcess<ReceiveAttorneyInvoiceProcessModel>
    {
        public ReceiveAttorneyInvoiceProcess(IAcceptThirdPartyInvoiceStateMachine acceptThirdPartyInvoiceStateMachine,
                                      IFinanceDomainServiceClient financeDomainService, ICombGuid combGuidGenerator,
                                      ICommunicationManager communicationManager,
                                      IRawLogger rawLogger, ILoggerSource loggerSource, ILoggerAppSource loggerAppSource)
            : base(acceptThirdPartyInvoiceStateMachine, financeDomainService, combGuidGenerator, communicationManager, (int)ThirdPartyType.Attorney,
                   rawLogger, loggerSource, loggerAppSource)
        {
        }

        public override void Initialise(IDataModel dataModel)
        {
            if (dataModel == null) { throw new ArgumentNullException("dataModel"); }
            if (!(dataModel is ReceiveThirdPartyInvoiceProcessModel))
            {
                throw new Exception(string.Format("Invalid Data Model. Expected {0} but received {1}", typeof(ReceiveThirdPartyInvoiceProcessModel).Name, dataModel.GetType().Name));
            }

            base.ProcessState = acceptThirdPartyInvoiceStateMachine;
        }

        public override void RestoreState(IDataModel dataModel)
        {
            //TODO: Changes made to the Domain Process Manager. To be implemented
            throw new NotImplementedException();
        }

        public override void OnInternalStart()
        {
            var serviceRequestMetadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, combGuidGenerator.Generate());

            var attorneyInvoiceDocumentModel = new AttorneyInvoiceDocumentModel(
                     this.DataModel.LoanNumber.ToString()
                   , this.DataModel.DateReceived
                   , DateTime.Now
                   , this.DataModel.FromEmailAddress
                   , this.DataModel.EmailSubject
                   , this.DataModel.InvoiceFileName
                   , this.DataModel.InvoiceFileExtension
                   , "Invoices"
                   , this.DataModel.FileContentAsBase64
                );

            var command = new AcceptThirdPartyInvoiceCommand(this.DataModel.LoanNumber, attorneyInvoiceDocumentModel, this.thirdPartyTypeKey);
            this.acceptThirdPartyInvoiceStateMachine.SystemMessages.Aggregate(this.financeDomainService.PerformCommand(command, serviceRequestMetadata));
            if (acceptThirdPartyInvoiceStateMachine.SystemMessages.HasErrors)
            {
                var attachments = new List<IMailAttachment> { new MailAttachment {
                    AttachmentName = string.Format("{0}.{1}", this.DataModel.InvoiceFileName, this.DataModel.InvoiceFileExtension),
                    ContentAsBase64 = this.DataModel.FileContentAsBase64, AttachmentType = string.Format(".{0}", this.DataModel.InvoiceFileExtension) } };
                communicationManager.SendProcessingFailedEmailToLossControl(this.DataModel.EmailSubject, attachments, acceptThirdPartyInvoiceStateMachine.SystemMessages);
                CompletedWithErrors(acceptThirdPartyInvoiceStateMachine.SystemMessages);
            }
        }
        protected void CompletedWithErrors(ISystemMessageCollection messages)
        {
            var errorMessages = new StringBuilder();
            foreach (var message in messages.AllMessages)
            {
                errorMessages.AppendLine(message.Message);
            }
            base.OnCompletedWithErrors(this.DomainProcessId, errorMessages.ToString());
        }
    }
}