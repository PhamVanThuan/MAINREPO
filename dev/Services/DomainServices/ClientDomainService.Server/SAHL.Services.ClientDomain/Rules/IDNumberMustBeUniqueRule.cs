using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.Interfaces.ClientDomain.Models;

namespace SAHL.Services.ClientDomain.Rules
{
    public class IdNumberMustBeUniqueRule : IDomainRule<INaturalPersonClientModel>
    {
        private IClientDataManager clientDataManager;
        private IValidationUtils validationUtils;

        public IdNumberMustBeUniqueRule(IClientDataManager clientDataManager, IValidationUtils validationUtils)
        {
            this.clientDataManager = clientDataManager;
            this.validationUtils = validationUtils;
        }

        public void ExecuteRule(Core.SystemMessages.ISystemMessageCollection messages, INaturalPersonClientModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.IDNumber))
            {
                if (validationUtils.ValidateIDNumber(model.IDNumber))
                {
                    var existingNaturalPersonClient = clientDataManager.FindExistingClientByIdNumber(model.IDNumber);
                    if (existingNaturalPersonClient != null)
                    {
                        messages.AddMessage(new SystemMessage(string.Format("A client with Identity Number {0} already exists.", model.IDNumber), SystemMessageSeverityEnum.Error));
                    }
                }
            }
        }
    }
}