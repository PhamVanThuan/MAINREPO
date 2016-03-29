using System.Data;
using System.Reflection;
using System.ServiceModel.Activation;

namespace SAHL.Web.Services.Public
{
    
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Valuation : IValuation
    {
        ValuationServiceInternal.ValuationClient valuationClient = new ValuationServiceInternal.ValuationClient();

        /// <summary>
        /// Calls the Internal SubmitCompletedValuationLightstone webservice
        /// </summary>
        /// <param name="UniqueID"></param>
        /// <param name="XMLData"></param>
        /// <returns>LightstoneWebServiceResult Dataset</returns>
        public DataSet SubmitCompletedValuationLightstone(int UniqueID, string XMLData)
        {
            var dsReturnResult = valuationClient.SubmitCompletedValuationLightstone(UniqueID, XMLData);

            return dsReturnResult;
        }

        /// <summary>
        /// Calls the Internal SubmitAmendedValuationLightstone webservice
        /// </summary>
        /// <param name="UniqueID"></param>
        /// <param name="XMLData"></param>
        /// <returns>LightstoneWebServiceResult Dataset</returns>
        public DataSet SubmitAmendedValuationLightstone(int UniqueID, string XMLData)
        {
            var dsReturnResult = valuationClient.SubmitAmendedValuationLightstone(UniqueID, XMLData);

            return dsReturnResult;
        }

        /// <summary>
        /// Calls the Internal SubmitRejectedValuationLightstone webservice
        /// </summary>
        /// <param name="UniqueID"></param>
        /// <param name="XMLData"></param>
        /// <returns>LightstoneWebServiceResult Dataset</returns>
        public DataSet SubmitRejectedValuationLightstone(int UniqueID, string XMLData)
        {
            var dsReturnResult = valuationClient.SubmitRejectedValuationLightstone(UniqueID, XMLData);

            return dsReturnResult;
        }
    }
}
