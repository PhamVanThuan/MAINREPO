using System;
using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ITC.Commands;
using SAHL.Services.ITC.Common;
using SAHL.Services.ITC.Managers.Itc;
using SAHL.Services.ITC.TransUnion;

namespace SAHL.Services.ITC.CommandHandlers
{
    public class PerformCapitecITCCheckCommandHandler : IServiceCommandHandler<PerformCapitecITCCheckCommand>
    {
        private IItcManager itcManager;
        private ITransUnionService transUnionService;
        private IUnitOfWorkFactory unitOfWorkFactory;
        private IITCCommon itcCommon;

        public PerformCapitecITCCheckCommandHandler(IItcManager itcManager, ITransUnionService transUnionService, IITCCommon itcCommon)
        {
            this.itcManager = itcManager;
            this.transUnionService = transUnionService;
            this.itcCommon = itcCommon;
        }

        public ISystemMessageCollection HandleCommand(PerformCapitecITCCheckCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection systemMessages = SystemMessageCollection.Empty();

            var itcRequest = itcManager.CreateITCRequestForApplicant(command.ApplicantITCRequestDetails);
            try
            {
                var itcResponse = itcCommon.PerformITCRequest(itcManager, transUnionService, itcRequest, systemMessages);
                if (itcResponse != null)
                {
                    itcManager.SaveITC(command.ItcID, itcResponse);
                }
            }
            catch (Exception runtimeException)
            {
                itcManager.LogFailedITCRequestAndResponse(itcRequest, null, this.GetType().Name);
                throw runtimeException;
            }

            return systemMessages;
        }
    }
}