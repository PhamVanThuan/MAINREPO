using SAHL.Services.FinancialDomain.Managers.Models;
using System.Collections.Generic;
namespace SAHL.Services.FinancialDomain.Managers
{
    public class FinancialManager : IFinancialManager
    {
        private readonly IFinancialDataManager financialDataManager;

        public FinancialManager(IFinancialDataManager financialDataManager)
        {
            this.financialDataManager = financialDataManager;
        }

        public void SetApplicationInformationSPVKey(int applicationInformationKey, int SPVKey)
        {
            var offerInformationData = financialDataManager.GetApplicationInformationVariableLoan(applicationInformationKey);
            offerInformationData.SPVKey = SPVKey;
            financialDataManager.SaveOfferInformationVariableLoan(offerInformationData);
        }

        public void SyncApplicationAttributes(int applicationNumber, IEnumerable<GetOfferAttributesModel> determinedOfferAttributes)
        {
            foreach (var determinedAttr in determinedOfferAttributes)
            {
                if (financialDataManager.ApplicationHasAttribute(applicationNumber, determinedAttr.OfferAttributeTypeKey) && determinedAttr.Remove)
                {
                    financialDataManager.RemoveApplicationAttribute(applicationNumber, determinedAttr.OfferAttributeTypeKey);
                }

                if (!financialDataManager.ApplicationHasAttribute(applicationNumber, determinedAttr.OfferAttributeTypeKey) && !determinedAttr.Remove) 
                {
                    financialDataManager.AddApplicationAttribute(applicationNumber, determinedAttr.OfferAttributeTypeKey);
                }
            }
        }
    }
}