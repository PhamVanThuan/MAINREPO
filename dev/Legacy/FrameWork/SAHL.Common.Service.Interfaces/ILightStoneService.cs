using System.Data;
using SAHL.Common.Service.Interfaces.DataSets;

namespace SAHL.Common.Service.Interfaces
{
    public interface ILightStoneService
    {
        DataTable ReturnProperties(int genericKey, int genericKeyTypeKey, string LegalName, string IDNumber, string FirstName, string SecondName, string Surname, string Suburb, string Street, string StreetType, string StreetNo);

        //string ConfirmProperty(int genericKey, int genericKeyTypeKey, int PropertyID);
        DataSet ReturnTransferData(int genericKey, int genericKeyTypeKey, int propertyID);

        //DataSet ReturnValuation(int genericKey, int genericKeyTypeKey, string PropertyID, double PurchasePrice, double LoanAmount, bool IsSwitchOrFurtherLoan);
        DataSet ReturnValuation(int genericKey, int genericKeyTypeKey, int PropertyID, double PurchasePrice, double LoanAmount, bool IsSwitchOrFurtherLoan);

        /// <summary>
        /// Requests 1 or more physical property valuations from the EzVal system when we have a LightstonePropertyID
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyTypeKey"></param>
        /// <param name="propertyDetails"></param>
        /// <returns>
        /// The method results in a dataset being passed back with a single table [results]:
        /// The following fields exist in the table
        /// Id              Type
        /// UniqueID        System.String (the XMLHistoryKey sent in the request)
        /// Successful      System.String (either "True" of "False")
        /// ErrorMessage    System.String
        /// </returns>
        DataSet RequestValuationForLightstoneValidatedProperty(int genericKey, int genericKeyTypeKey, LightstoneValidatedProperty.PropertyDetailsDataTable propertyDetails);

        /// <summary>
        /// Requests 1 or more physical property valuations from the EzVal system when we do not have a LightstonePropertyID
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyTypeKey"></param>
        /// <param name="propertyDetails"></param>
        /// <returns></returns>
        DataSet RequestValuationForLightstoneNonValidatedProperty(int genericKey, int genericKeyTypeKey, LightstoneNonValidatedProperty.PropertyDetailsDataTable propertyDetails);

        /// <summary>
        ///
        /// </summary>
        /// <param name="GenericKey"></param>
        /// <param name="GenericKeyTypeKey"></param>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="requestDS"></param>
        /// <param name="responseDS"></param>
        /// <returns></returns>
        bool GenerateXMLHistory(int GenericKey, int GenericKeyTypeKey, string url, string method, DataSet requestDS, DataSet responseDS);
    }
}