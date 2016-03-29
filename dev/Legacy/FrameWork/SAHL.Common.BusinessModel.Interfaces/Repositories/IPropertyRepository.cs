using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using System;
using System.Collections.Generic;
using System.Data;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IPropertyRepository
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        IProperty GetPropertyByKey(int Key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        List<IProperty> GetPropertyByAddressKey(int Key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="property"></param>
        void SaveProperty(IProperty property);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IProperty CreateEmptyProperty();

        /// <summary>
        ///
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        IProperty FindMatchingProperty(IProperty property);

        /// <summary>
        ///
        /// </summary>
        /// <param name="PropertyKey"></param>
        /// <returns></returns>
        IReadOnlyEventList<IValuation> GetValuationByPropertyKey(int PropertyKey);

        IValuation GetLatestValuationByPropertyKey(int PropertyKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="PropertyKey"></param>
        /// <returns></returns>
        IEventList<IValuation> GetValuationsByPropertyKeyDateSorted(int PropertyKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="ValuatorKey"></param>
        /// <returns></returns>
        IValuator GetValuatorByKey(int ValuatorKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="valuatorCompanyDescription"></param>
        /// <returns></returns>
        IValuator GetValuatorByDescription(string valuatorCompanyDescription);

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        ISuburb GetSuburbByKey(int Key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        IValuation GetValuationByKey(int Key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="LEKey"></param>
        /// <returns></returns>
        IValuator GetValuatorByLEKey(int LEKey);

        /// <summary>
        /// Return a List of Active Valuators
        /// </summary>
        /// <returns></returns>
        IEventList<IValuator> GetActiveValuators();

        /// <summary>
        /// Return a List of Valuators filtered by OriginationSource
        /// </summary>
        /// <param name="originationSourceKey"></param>
        /// <returns></returns>
        List<IValuator> GetValuatorsByOriginationSource(int originationSourceKey);

        string GetLightStonePropertyID(IProperty prop);

        /// <summary>
        ///
        /// </summary>
        /// <param name="valuation"></param>
        void SaveValuationWithoutValidationErrors(IValuation valuation);

        /// <summary>
        ///
        /// </summary>
        /// <param name="valuation"></param>
        void SaveValuation(IValuation valuation);

        /// <summary>
        ///
        /// </summary>
        /// <param name="vdpds"></param>
        /// <returns></returns>
        IValuation CreateEmptyValuation(ValuationDataProviderDataServices vdpds);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IPropertyData CreateEmptyPropertyData();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IPropertyTitleDeed CreateEmptyPropertyTitleDeed();

        void SavePropertyTitleDeed(IPropertyTitleDeed propertyTitleDeed);

        IPropertyTitleDeed GetPropertyTitleDeedByTitleDeedNumber(int PropertyKey, string TitleDeedNumber);

        /// <summary>
        ///
        /// </summary>
        /// <param name="propertyData"></param>
        void SavePropertyData(IPropertyData propertyData);

        /// <summary>
        ///
        /// </summary>
        /// <param name="dpds"></param>
        /// <returns>IDataProviderDataService</returns>
        IDataProviderDataService GetDataProviderDataServiceByKey(DataProviderDataServices dpds);

        /// <summary>
        ///
        /// </summary>
        /// <param name="vdpds"></param>
        /// <returns></returns>
        IValuationDataProviderDataService GetValuationDataProviderDataServiceByKey(ValuationDataProviderDataServices vdpds);

        /// <summary>
        ///
        /// </summary>
        /// <param name="pdpds"></param>
        /// <returns></returns>
        IPropertyDataProviderDataService GetPropertyDataProviderDataServiceByKey(PropertyDataProviderDataServices pdpds);

        /// <summary>
        ///
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <returns></returns>
        IProperty GetPropertyByAccountKey(int AccountKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="OfferKey"></param>
        /// <returns></returns>
        int GetPropertyKeyByOfferKey(int OfferKey);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IPropertyAccessDetails CreateEmptyPropertyAccessDetails();

        /// <summary>
        ///
        /// </summary>
        /// <param name="propertyAccessDetails"></param>
        void SavePropertyAccessDetails(IPropertyAccessDetails propertyAccessDetails);

        /// <summary>
        ///
        /// </summary>
        /// <param name="PropertyKey"></param>
        /// <returns></returns>
        int CheckForAutomatedValuations(int PropertyKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="PropertyKey"></param>
        /// <returns></returns>
        IReadOnlyEventList<IPropertyAccessDetails> GetPropertyAccessDetailsByPropertyKey(int PropertyKey);

        IValuationMainBuilding CreateEmptyValuationMainBuilding();

        IValuationCottage CreateEmptyValuationCottage();

        /// <summary>
        /// This method is used to remove a ValuationCottage record
        /// </summary>
        /// <param name="ValuationKey"></param>
        void DeleteValuationCottage(int ValuationKey);

        /// <summary>
        /// This method is used to remove a ValuationCombinedThatch record
        /// </summary>
        /// <param name="ValuationKey"></param>
        void DeleteValuationCombinedThatch(int ValuationKey);

        IValuationImprovement CreateEmptyValuationImprovement();

        IValuationOutbuilding CreateEmptyValuationOutbuilding();

        IValuationCombinedThatch CreateEmptyValuationCombinedThatch();

        /// <summary>
        /// Method to build the XML to store in the Data column of the PropertyData table for SAHL dataprovider
        /// </summary>
        /// <param name="bondAccountNumber"></param>
        /// <param name="deedsOfficeKey"></param>
        /// <returns>string</returns>
        string BuildSAHLPropertyDataXML(string bondAccountNumber, int deedsOfficeKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="property"></param>
        /// <param name="propertyDataProviderDataServices"></param>
        /// <returns></returns>
        IPropertyData GetLatestPropertyData(IProperty property, PropertyDataProviderDataServices propertyDataProviderDataServices);

        /// <summary>
        /// Method to return the specified XML as a dataset
        /// </summary>
        /// <param name="xml"></param>
        /// <returns>DataSet</returns>
        DataSet GetDataSetFromXML(string xml);

        /// <summary>
        /// Saves an address against a property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="address">The address to add.  If this is a new address, it will be created in the database.</param>
        void SaveAddress(IProperty property, IAddress address);

        IEventList<IApplication> GetApplicationsForProperty(int PropertyKey);

        /// <summary>
        /// Get the Data Provider Key from the Data PRovider Data Service
        /// </summary>
        /// <param name="DataProviderDataServiceKey"></param>
        /// <returns></returns>
        int GetDataProviderKeyFromDataProviderDataService(int DataProviderDataServiceKey);

        /// <summary>
        /// Get the Property Access Details for a Property
        /// </summary>
        /// <param name="PropertyKey"></param>
        /// <returns></returns>
        IEventList<IPropertyAccessDetails> GetPropertyAccesDetailsByPropertyKey(int PropertyKey);

        /// <summary>
        /// Return A Datatable containing the Valuator XML History
        /// </summary>
        /// <param name="GenericKeyTypeKey"></param>
        /// <param name="GenericKey"></param>
        /// <param name="dataProvider"></param>
        /// <returns></returns>
        DataTable GetValuatorDataFromXMLHistory(int GenericKeyTypeKey, int GenericKey, string dataProvider);

        /// <summary>
        /// self-explanatory
        /// </summary>
        /// <param name="XMLHistoryKey"></param>
        /// <returns></returns>
        IXMLHistory GetXMLHistoryByKey(int XMLHistoryKey);

        /// <summary>
        /// get the latest xmlhistory data matching the criteria
        /// </summary>
        /// <param name="GenericKeyTypeKey"></param>
        /// <param name="GenericKey"></param>
        /// <param name="dataProvider"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        string GetXMLHistoryData(int GenericKeyTypeKey, int GenericKey, string dataProvider, string methodName);

        /// <summary>
        /// Checks if a LightStone AVM was done on the given property within the last 2 months
        /// </summary>
        /// <param name="PropertyKey"></param>
        /// <returns></returns>
        bool LightStoneValuationDoneWithinLast2Months(int PropertyKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="valuationKey"></param>
        /// <param name="applicationKey"></param>
        void AdcheckValuationUpdateHOC(int valuationKey, int applicationKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="PropertyKey"></param>
        /// <returns></returns>
        IValuation GetActiveValuationByPropertyKey(int PropertyKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="valMainBuilding"></param>
        /// <param name="valCottage"></param>
        /// <param name="valOutBuildings"></param>
        /// <returns></returns>
        double CalculateCombinedThatchValue(IValuationMainBuilding valMainBuilding, IValuationCottage valCottage, IEventList<IValuationOutbuilding> valOutBuildings);

        /// <summary>
        /// Confirms that the property has an Ad Check ID
        /// </summary>
        bool HasAdCheckPropertyID(int propertyKey);

        /// <summary>
        /// Confirms that the property has a Lightstone ID
        /// </summary>
        bool HasLightStonePropertyID(int propertyKey);

        /// <summary>
        /// Retrieves Property via the HOC
        /// </summary>
        /// <param name="hoc"></param>
        /// <returns></returns>
        IProperty GetPropertyByHOC(IHOC hoc);

        /// <summary>
        ///
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <returns></returns>
        IApplication GetApplicationByHOCAccountKey(int AccountKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <returns></returns>
        IProperty GetPropertyByApplicationKey(int ApplicationKey);

        /// <summary>
        /// Retrieves a filtered list of Valuators
        /// </summary>
        /// <param name="generalStatusKey"></param>
        /// <returns></returns>
        IEventList<IValuator> GetActiveValuatorsFiltered(int generalStatusKey);

        /// <summary>
        /// Get the LightStone Valuation via the service.
        /// This is used by the DomainService, which uses the executing thread id
        /// to reference the SPC. ADUser is therefor a required input.
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <param name="ADUser"></param>
        void GetLightStoneValuationForWorkFlow(int ApplicationKey, string ADUser);

        string PreviousLightStoneUniqueID(int propertyKey);

        void RequestLightStoneValuation(IPropertyAccessDetails propAD, IProperty prop, int gKey, int gkTypeKey, bool isReview, DateTime assessmentDate, String reason, String instructions, out string lightstonePropertyId);

        IValuationDiscriminatedLightStonePhysical CreateValuationLightStone(IProperty prop);

        DataTable GetValuationsAndReasons(int propertyKey);

        void ValuationUpdateHOC(int valuationKey, GenericKeyTypes genericKeyTypeKey, int genericKey);

        void ValuationUpdateHOC(IValuation valuation, GenericKeyTypes genericKeyTypeKey, int genericKey);

        IComcorpOfferPropertyDetails GetComcorpOfferPropertyDetails(int offerKey);
    }
}