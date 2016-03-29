using System;
using System.Linq;

namespace Automation.Framework
{
    public class Valuations : WorkflowBase
    {
        public void InsertValuation(int keyValue)
        {
            base.InsertValuationForOffer(keyValue, false);
        }

        public void StartEzVal(int keyValue)
        {
            base.InsertValuationForOffer(keyValue, true);
            base.InsertXmlHistoryForOffer(keyValue);
        }

        public void EndEzVal(int keyValue)
        {
            var property = base.GetProperty(offerkey: keyValue);
            var valuation = base.GetValuations(property.PropertyKey).Where(x => x.ValuationDate > DateTime.Now.Subtract(TimeSpan.FromMinutes(1))).FirstOrDefault();
            base.UpdateX2Valuation(keyValue, property.PropertyKey, valuation.ValuationUserID, property.PropertyId, valuation.ValuationKey, valuation.ValuationDataProviderDataServiceKey);
        }
    }
}