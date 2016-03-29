using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinancialDomain.Managers;
using SAHL.Services.Interfaces.FinancialDomain.Commands.Internal;
using System.Linq;

namespace SAHL.Services.FinancialDomain.CommandHandlers.Internal
{
    public class DetermineSPVQueryHandler : IServiceQueryHandler<DetermineSPVQuery>
    {
        private IFinancialDataManager financialDataManager;

        public DetermineSPVQueryHandler(IFinancialDataManager financialDataManager)
        {
            this.financialDataManager = financialDataManager;
        }

        public ISystemMessageCollection HandleQuery(DetermineSPVQuery query)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            var determinedAttributes = financialDataManager.DetermineApplicationAttributes(query.ApplicationNumber, query.LTV, query.EmploymentType, query.HouseholdIncome, query.IsStaffLoan, query.IsGEPF);
            var offerAttributes = determinedAttributes.Where(x => x.Remove == false)
                                                      .Select(s => s.OfferAttributeTypeKey);

            var offerAttributesCSV = string.Join(",", offerAttributes);

            var determinedSPV = this.financialDataManager.GetValidSPV(query.LTV, offerAttributesCSV);

            if (!string.IsNullOrEmpty(determinedSPV.Message))
            {
                messages.AddMessage(new SystemMessage(determinedSPV.Message, SystemMessageSeverityEnum.Error));
            }
            else
            {
                query.Result = new ServiceQueryResult<int>(new int[] { determinedSPV.ValidSPVKey });
            }

            return messages;
        }
    }
}