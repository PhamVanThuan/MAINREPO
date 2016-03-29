using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Application;
using SAHL.DomainProcessManager.Models;
using System;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.Rules
{
    public class VendorMustExistForVendorCodeRule : IDomainRule<ApplicationCreationModel>
    {
        private IApplicationDataManager applicationDataManager;

        public VendorMustExistForVendorCodeRule(IApplicationDataManager applicationDataManager)
        {
            this.applicationDataManager = applicationDataManager;
        }

        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, ApplicationCreationModel ruleModel)
        {
            bool vendorExists = applicationDataManager.DoesSuppliedVendorExist(ruleModel.VendorCode);
            if (!vendorExists)
            {
                messages.AddMessage(new SystemMessage(string.Format("A vendor with code: {0} does not exist in SA Home Loans database.", ruleModel.VendorCode), SystemMessageSeverityEnum.Error));
            }
        }
    }
}