using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.Managers.Applicant;
using SAHL.Services.Capitec.Managers.ITC;
using SAHL.Services.Capitec.Managers.Lookup;
using SAHL.Services.Interfaces.Capitec.Commands;
using SAHL.Services.Interfaces.Capitec.Common;
using SAHL.Services.Interfaces.ITC;
using SAHL.Services.Interfaces.ITC.Commands;
using SAHL.Services.Interfaces.ITC.Queries;
using System.Linq;
using System;

namespace SAHL.Services.Capitec.CommandHandlers.Internal
{
    public class PerformITCForApplicantCommandHandler : IServiceCommandHandler<PerformITCForApplicantCommand>
    {
        private IUnitOfWorkFactory unitOfWorkFactory;
        private IITCManager itcManager;
        private IITCDataManager itcDataManager;
        private IItcServiceClient itcServiceClient;
        private ILookupManager lookupService;
        private IApplicantDataManager applicantDataService;

        public PerformITCForApplicantCommandHandler(IUnitOfWorkFactory unitOfWorkFactory, IApplicantDataManager applicantDataService,
            IITCManager itcManager, IITCDataManager itcDataManager,IItcServiceClient itcServiceClient, ILookupManager lookupService)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.applicantDataService = applicantDataService;
            this.itcManager = itcManager;
            this.itcDataManager = itcDataManager;
            this.itcServiceClient = itcServiceClient;
            this.lookupService = lookupService;
        }

        public ISystemMessageCollection HandleCommand(PerformITCForApplicantCommand command, IServiceRequestMetadata metadata)
        {
            var systemMessages = SystemMessageCollection.Empty();

            string identityNumber = command.Applicant.Information.IdentityNumber;

            var applicantAnsweredYes = this.applicantDataService.DidApplicantAnswerYesToDeclaration(
                command.ApplicantID, DeclarationDefinitions.AllowCreditBureauCheck);
            if (!applicantAnsweredYes)
            {
                throw new InvalidOperationException("The applicant did not agree to the 'Allow credit bureau check' declaration.");
            }
         
            if (itcManager.GetValidITCModelForPerson(identityNumber) == null)
            {
                var itcID = lookupService.GenerateCombGuid();
                var itcRequest = itcManager.CreateApplicantITCRequest(command.Applicant);
                var performITCCommand = new PerformCapitecITCCheckCommand(itcID, itcRequest);
                var itcMessages = itcServiceClient.PerformCommand(performITCCommand, new ServiceRequestMetadata());
                if(itcMessages.HasErrors)
                {
                    return itcMessages;
                }

                var getITCQuery = new GetITCQuery(itcID);
                itcServiceClient.PerformQuery(getITCQuery);
                var validITC = getITCQuery.Result.Results.FirstOrDefault();

                if(validITC == null)
                { 
                    systemMessages.AddMessage(new SystemMessage("Failed to perform the itc check", SystemMessageSeverityEnum.Error));
                    return systemMessages;
                }

                itcDataManager.SaveITC(getITCQuery.ItcID, validITC.ITCDate, validITC.ITCData);
                var personID = applicantDataService.GetPersonID(identityNumber);
                itcManager.LinkItcToPerson(personID, itcID, validITC.ITCDate);
            }

            return systemMessages;
        }
    }
}