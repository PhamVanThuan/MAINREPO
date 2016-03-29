using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.Interfaces.ClientDomain.Models;

namespace SAHL.Services.ClientDomain.Rules
{
    public class PassportNumberMustBeUniqueRule : IDomainRule<INaturalPersonClientModel>
    {
        private IValidationUtils validationUtils;
        private IClientDataManager clientDataManager;

        public PassportNumberMustBeUniqueRule(IValidationUtils validationUtils, IClientDataManager clientDataManager)
        {
            this.validationUtils = validationUtils;
            this.clientDataManager = clientDataManager;
        }

        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, INaturalPersonClientModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.PassportNumber))
            {
                if (this.validationUtils.ValidatePassportNumber(model.PassportNumber))
                {
                    var existingClient = this.clientDataManager.FindExistingClientByPassportNumber(model.PassportNumber);
                    if (existingClient != null)
                    {
                        messages.AddMessage(new SystemMessage(string.Format("A client with passport number {0} already exists", model.PassportNumber), SystemMessageSeverityEnum.Error));
                    }
                }
            }
        }
    }
}