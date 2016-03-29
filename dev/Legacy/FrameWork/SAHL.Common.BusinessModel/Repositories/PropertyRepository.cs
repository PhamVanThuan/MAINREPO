using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.DataTransferObjects;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.DomainMessages;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Service.Interfaces.DataSets;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(IPropertyRepository))]
    public class PropertyRepository : AbstractRepositoryBase, IPropertyRepository
    {
        public PropertyRepository()
        {
            if (castleTransactionService == null)
            {
                castleTransactionService = new CastleTransactionsService();
            }
        }

        public PropertyRepository(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public IProperty GetPropertyByKey(int Key)
        {
            return base.GetByKey<IProperty, Property_DAO>(Key);
        }

        public List<IProperty> GetPropertyByAddressKey(int Key)
        {
            const string HQL = "from Property_DAO p where p.Address.Key = ?";
            SimpleQuery<Property_DAO> pq = new SimpleQuery<Property_DAO>(HQL, Key);
            Property_DAO[] props = pq.Execute();

            List<IProperty> list = new List<IProperty>();

            for (int i = 0; i < props.Length; i++)
            {
                IProperty ip = new Property(props[i]);
                list.Add(ip);
            }

            return list;
        }

        public IProperty CreateEmptyProperty()
        {
            return base.CreateEmpty<IProperty, Property_DAO>();
        }

        public void SaveProperty(IProperty property)
        {
            base.Save<IProperty, Property_DAO>(property);
        }

        public IProperty FindMatchingProperty(IProperty property)
        {
            if (property == null || property.Address == null)
                throw new Exception("Null property or null address");

            List<IProperty> props = GetPropertyByAddressKey(property.Address.Key);

            for (int i = 0; i < props.Count; i++)
            {
                if ((property.ErfNumber == props[i].ErfNumber || (String.IsNullOrEmpty(property.ErfNumber) && String.IsNullOrEmpty(props[i].ErfNumber)))
                && (property.ErfPortionNumber == props[i].ErfPortionNumber || (String.IsNullOrEmpty(property.ErfPortionNumber) && String.IsNullOrEmpty(props[i].ErfPortionNumber)))
                && (property.SectionalSchemeName == props[i].SectionalSchemeName || (String.IsNullOrEmpty(property.SectionalSchemeName) && String.IsNullOrEmpty(props[i].SectionalSchemeName)))
                && (property.SectionalUnitNumber == props[i].SectionalUnitNumber || (String.IsNullOrEmpty(property.SectionalUnitNumber) && String.IsNullOrEmpty(props[i].SectionalUnitNumber)))
                && (property.ErfSuburbDescription == props[i].ErfSuburbDescription || (String.IsNullOrEmpty(property.ErfSuburbDescription) && String.IsNullOrEmpty(props[i].ErfSuburbDescription)))
                && (property.ErfMetroDescription == props[i].ErfMetroDescription || (String.IsNullOrEmpty(property.ErfMetroDescription) && String.IsNullOrEmpty(props[i].ErfMetroDescription))))
                {
                    return props[i];
                }
            }

            return null;
        }

        public IPropertyData CreateEmptyPropertyData()
        {
            return base.CreateEmpty<IPropertyData, PropertyData_DAO>();
        }

        public IPropertyTitleDeed CreateEmptyPropertyTitleDeed()
        {
            return base.CreateEmpty<IPropertyTitleDeed, PropertyTitleDeed_DAO>();
        }

        public void SavePropertyTitleDeed(IPropertyTitleDeed propertyTitleDeed)
        {
            base.Save<IPropertyTitleDeed, PropertyTitleDeed_DAO>(propertyTitleDeed);
        }

        public IPropertyTitleDeed GetPropertyTitleDeedByTitleDeedNumber(int PropertyKey, string TitleDeedNumber)
        {
            string HQL = "from PropertyTitleDeed_DAO ptd where ptd.Property.Key = ? and ptd.TitleDeedNumber = ? order by ptd.Key desc";
            SimpleQuery<PropertyTitleDeed_DAO> query = new SimpleQuery<PropertyTitleDeed_DAO>(HQL, PropertyKey, TitleDeedNumber);
            PropertyTitleDeed_DAO[] ptd = query.Execute();

            if (ptd.Length > 0)
                return new PropertyTitleDeed(ptd[0]);

            return null;
        }

        public void SavePropertyData(IPropertyData propertyData)
        {
            base.Save<IPropertyData, PropertyData_DAO>(propertyData);
        }

        public IPropertyAccessDetails CreateEmptyPropertyAccessDetails()
        {
            return base.CreateEmpty<IPropertyAccessDetails, PropertyAccessDetails_DAO>();
        }

        public void SavePropertyAccessDetails(IPropertyAccessDetails propertyAccessDetails)
        {
            base.Save<IPropertyAccessDetails, PropertyAccessDetails_DAO>(propertyAccessDetails);
        }

        public IPropertyDataProviderDataService GetPropertyDataProviderDataServiceByKey(PropertyDataProviderDataServices pdpds)
        {
            int key = (int)pdpds;
            return base.GetByKey<IPropertyDataProviderDataService, PropertyDataProviderDataService_DAO>(key);
        }

        public IDataProviderDataService GetDataProviderDataServiceByKey(DataProviderDataServices dpds)
        {
            int key = (int)dpds;
            return base.GetByKey<IDataProviderDataService, DataProviderDataService_DAO>(key);
        }

        public IValuationDataProviderDataService GetValuationDataProviderDataServiceByKey(ValuationDataProviderDataServices vdpds)
        {
            int key = (int)vdpds;
            return base.GetByKey<IValuationDataProviderDataService, ValuationDataProviderDataService_DAO>(key);
        }

        public void SaveValuationWithoutValidationErrors(IValuation valuation)
        {
            Valuation_DAO dao = (Valuation_DAO)((IDAOObject)valuation).GetDAOObject();

            if (valuation.IsActive)
            {
                //make sure any other active valuations are deactivated
                if (dao.Property.Valuations != null)
                {
                    // add the rule exclusion set
                    SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                    spc.ExclusionSets.Add(RuleExclusionSets.ValuationUpdatePreviousToInactive);
                    for (int i = 0; i < dao.Property.Valuations.Count; i++)
                    {
                        Valuation_DAO valDAO = dao.Property.Valuations[i];

                        // dont update the current valuation record that we are saving - this will be done outside the loop further down
                        if (valDAO.IsActive && valDAO.Key != valuation.Key)
                        {
                            valDAO.IsActive = false;
                            valDAO.SaveAndFlush();
                        }
                    }

                    // remove the rule exclusion set
                    spc.ExclusionSets.Remove(RuleExclusionSets.ValuationUpdatePreviousToInactive);
                }

                //Need to recalc for Valuation Changes
                foreach (IApplication app in GetApplicationsForProperty(valuation.Property.Key))
                {
                    IApplicationMortgageLoan appML = app as IApplicationMortgageLoan;
                    if (appML != null && appML.IsOpen)
                        appML.CalculateApplicationDetail(false, false);
                }

                //have to make sure we update PropertyValuation in OfferInformationVariableLoan if IsActive is set
                string HQL = "select ai from ApplicationMortgageLoanDetail_DAO amld join amld.Application.ApplicationInformations ai where amld.Property.Key = ? and amld.Application.ApplicationStatus.Key = 1 and ai.ApplicationInformationType.Key <> 3 order by ai.Key desc";
                SimpleQuery<ApplicationInformation_DAO> q = new SimpleQuery<ApplicationInformation_DAO>(HQL, valuation.Property.Key);
                ApplicationInformation_DAO[] res = q.Execute();
                if (res.Length > 0)
                {
                    foreach (ApplicationInformation_DAO aiDAO in res) //Further Lending could have more than one open application
                    {
                        aiDAO.ApplicationInformationVariableLoan.PropertyValuation = valuation.ValuationAmount;

                        // Change in active valuation must result in recalc of application detail because of the change in risk (LTV)
                        IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();

                        IApplication App = BMTM.GetMappedType<IApplication>(aiDAO.Application);

                        // recalculate application detail
                        IApplicationMortgageLoan appML = App as IApplicationMortgageLoan;
                        if (appML != null && appML.IsOpen)
                            appML.CalculateApplicationDetail(false, false);

                        aiDAO.ApplicationInformationVariableLoan.SaveAndFlush();
                    }
                }

                dao.IsActive = true;
            }

            dao.SaveAndFlush();
        }

        public void SaveValuation(IValuation valuation)
        {
            SaveValuationWithoutValidationErrors(valuation);

            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        public ISuburb GetSuburbByKey(int Key)
        {
            return base.GetByKey<ISuburb, Suburb_DAO>(Key);
        }

        public IValuation GetValuationByKey(int Key)
        {
            return base.GetByKey<IValuation, Valuation_DAO>(Key);
        }

        public IValuation CreateEmptyValuation(ValuationDataProviderDataServices vdpds)
        {
            IValuation val = null;

            switch (vdpds)
            {
                case ValuationDataProviderDataServices.SAHLManualValuation:
                    val = new ValuationDiscriminatedSAHLManual(new ValuationDiscriminatedSAHLManual_DAO());
                    break;

                case ValuationDataProviderDataServices.SAHLClientEstimate:
                    val = new ValuationDiscriminatedSAHLClientEstimate(new ValuationDiscriminatedSAHLClientEstimate_DAO());
                    break;

                case ValuationDataProviderDataServices.LightstoneAutomatedValuation:
                    val = new ValuationDiscriminatedLightstoneAVM(new ValuationDiscriminatedLightstoneAVM_DAO());
                    break;

                case ValuationDataProviderDataServices.LightstonePhysicalValuation:
                    val = new ValuationDiscriminatedLightStonePhysical(new ValuationDiscriminatedLightStonePhysical_DAO());
                    break;

                case ValuationDataProviderDataServices.AdCheckPhysicalValuation:
                    val = new ValuationDiscriminatedAdCheckPhysical(new ValuationDiscriminatedAdCheckPhysical_DAO());
                    break;

                case ValuationDataProviderDataServices.AdCheckDesktopValuation:
                    val = new ValuationDiscriminatedAdCheckDesktop(new ValuationDiscriminatedAdCheckDesktop_DAO());
                    break;

                default:

                    // throw some kind of error.
                    break;
            }

            return val;
        }

        public IReadOnlyEventList<IValuation> GetValuationByPropertyKey(int PropertyKey)
        {
            Property_DAO property = ActiveRecordBase<Property_DAO>.Find(PropertyKey);

            if (property != null)
                return new ReadOnlyEventList<IValuation>(new DAOEventList<Valuation_DAO, IValuation, Valuation>(property.Valuations));

            return null;
        }

        public IEventList<IValuation> GetValuationsByPropertyKeyDateSorted(int PropertyKey)
        {
            string HQL = "from Valuation_DAO v where v.Property.Key = ? order by v.ValuationDate desc";
            SimpleQuery<Valuation_DAO> q = new SimpleQuery<Valuation_DAO>(HQL, PropertyKey);

            Valuation_DAO[] res = q.Execute();

            IEventList<IValuation> list = new DAOEventList<Valuation_DAO, IValuation, Valuation>(res);
            if (list.Count > 0)
                return new EventList<IValuation>(list);

            return null;
        }

        public IValuation GetActiveValuationByPropertyKey(int PropertyKey)
        {
            string HQL = "FROM Valuation_DAO v WHERE v.Property.Key = ? and v.IsActive = 1 and v.ValuationStatus.Key = 2";
            SimpleQuery<Valuation_DAO> q = new SimpleQuery<Valuation_DAO>(HQL, PropertyKey);
            Valuation_DAO[] v = q.Execute();

            if (v == null || v.Length == 0)
                return null;

            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return BMTM.GetMappedType<IValuation, Valuation_DAO>(v[0]);
        }

        public IValuation GetLatestValuationByPropertyKey(int PropertyKey)
        {
            string HQL = "FROM Valuation_DAO v WHERE v.Property.Key = ?";
            SimpleQuery<Valuation_DAO> q = new SimpleQuery<Valuation_DAO>(HQL, PropertyKey);
            Valuation_DAO[] v = q.Execute();

            if (v == null || v.Length == 0)
                return null;

            Valuation_DAO val = v.OrderByDescending(x => x.Key).FirstOrDefault();

            if (val != null)
            {
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return BMTM.GetMappedType<IValuation, Valuation_DAO>(val);
            }

            return null;
        }

        public IValuator GetValuatorByKey(int Key)
        {
            return base.GetByKey<IValuator, Valuator_DAO>(Key);
        }

        public IValuator GetValuatorByDescription(string valuatorCompanyDescription)
        {
            const string sql = @"select v.* FROM [2AM].[dbo].[Valuator] v
                                 join [2AM].[dbo].LegalEntity le on le.LegalEntityKey = v.LegalEntityKey
                                 where le.RegisteredName = ?";
            SimpleQuery<Valuator_DAO> q = new SimpleQuery<Valuator_DAO>(QueryLanguage.Sql, sql, valuatorCompanyDescription);
            q.AddSqlReturnDefinition(typeof(Valuator_DAO), "v");
            Valuator_DAO[] valuators = q.Execute();

            if (valuators == null || valuators.Length == 0)
                return null;

            return new Valuator(valuators[0]);
        }

        public IValuator GetValuatorByLEKey(int LEKey)
        {
            const string HQL = "from Valuator_DAO d where d.LegalEntity.Key=?";
            SimpleQuery<Valuator_DAO> q = new SimpleQuery<Valuator_DAO>(HQL, LEKey);
            Valuator_DAO[] arr = q.Execute();
            if (arr == null || arr.Length == 0)
                return null;
            return new Valuator(arr[0]);
        }

        public IEventList<IValuator> GetActiveValuators()
        {
            const int GeneralStatus = (int)GeneralStatuses.Active;
            const string CQL = "from Valuator_DAO v WHERE v.GeneralStatus.Key = ?";
            SimpleQuery<Valuator_DAO> query = new SimpleQuery<Valuator_DAO>(CQL, GeneralStatus);
            Valuator_DAO[] VAL = query.Execute();
            return new EventList<IValuator>(new DAOEventList<Valuator_DAO, IValuator, Valuator>(VAL));
        }

        public IEventList<IValuator> GetActiveValuatorsFiltered(int generalStatusKey)
        {
            int legalEntityTypeKey = (int)LegalEntityTypes.Company;
            string sql = string.Format(UIStatementRepository.GetStatement("Repositories.PropertyRepository", "GetActiveValuatorsFiltered"), legalEntityTypeKey, generalStatusKey);
            SimpleQuery<Valuator_DAO> query = new SimpleQuery<Valuator_DAO>(QueryLanguage.Sql, sql);
            query.AddSqlReturnDefinition(typeof(Valuator_DAO), "VAL");
            query.SetParameterList("regNames", SAHL.Common.Constants.Valuators.GetFilteredValuators());
            Valuator_DAO[] VAL = query.Execute();
            return new EventList<IValuator>(new DAOEventList<Valuator_DAO, IValuator, Valuator>(VAL));
        }

        public List<IValuator> GetValuatorsByOriginationSource(int originationSourceKey)
        {
            string sql = UIStatementRepository.GetStatement("Property", "GetValuatorsByOriginationSource");

            SimpleQuery<Valuator_DAO> q = new SimpleQuery<Valuator_DAO>(QueryLanguage.Sql, sql);
            q.AddSqlReturnDefinition(typeof(Valuator_DAO), "v");
            q.SetParameter("OriginationSourceKey", originationSourceKey);
            Valuator_DAO[] valuators = q.Execute();

            List<IValuator> list = new List<IValuator>();

            for (int i = 0; i < valuators.Length; i++)
            {
                IValuator val = new Valuator(valuators[i]);
                list.Add(val);
            }

            return list;
        }

        public bool LightStoneValuationDoneWithinLast2Months(int PropertyKey)
        {
            DateTime dt = DateTime.Now.Subtract(new TimeSpan(60, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second));

            string HQL = "from Valuation_DAO v where v.Property.Key = ? and v.ValuationDate > ?";
            SimpleQuery<Valuation_DAO> query = new SimpleQuery<Valuation_DAO>(HQL, PropertyKey, dt);
            Valuation_DAO[] v = query.Execute();

            for (int i = 0; i < v.Length; i++)
            {
                ValuationDiscriminatedLightstoneAVM_DAO avm = v[i] as ValuationDiscriminatedLightstoneAVM_DAO;

                if (avm != null)
                    return true;
            }

            return false;
        }

        public IProperty GetPropertyByAccountKey(int AccountKey)
        {
            IAccountRepository accrep = RepositoryFactory.GetRepository<IAccountRepository>();
            IMortgageLoanAccount account = (IMortgageLoanAccount)accrep.GetAccountByKey(AccountKey);
            return account.SecuredMortgageLoan.Property;
        }

        [Obsolete("Can use domain - speak to Matts")]
        public int GetPropertyKeyByOfferKey(int OfferKey)
        {
            int retval = 0;
            string HQL = "from ApplicationMortgageLoanDetail_DAO v where v.Key = ?";
            SimpleQuery<ApplicationMortgageLoanDetail_DAO> query = new SimpleQuery<ApplicationMortgageLoanDetail_DAO>(HQL, OfferKey);
            ApplicationMortgageLoanDetail_DAO[] o = ActiveRecordBase.ExecuteQuery(query) as ApplicationMortgageLoanDetail_DAO[];

            if (o != null) retval = o[0].Property.Key;
            return retval;
        }

        public int CheckForAutomatedValuations(int PropertyKey)
        {
            int retval = 0;

            return retval;
        }

        public IReadOnlyEventList<IPropertyAccessDetails> GetPropertyAccessDetailsByPropertyKey(int PropertyKey)
        {
            string HQL = "from PropertyAccessDetails_DAO v where v.Property.Key = ?";
            SimpleQuery<PropertyAccessDetails_DAO> q = new SimpleQuery<PropertyAccessDetails_DAO>(HQL, PropertyKey);

            PropertyAccessDetails_DAO[] res = q.Execute();

            IEventList<IPropertyAccessDetails> list = new DAOEventList<PropertyAccessDetails_DAO, IPropertyAccessDetails, PropertyAccessDetails>(res);
            return new ReadOnlyEventList<IPropertyAccessDetails>(list);
        }

        public IValuationCottage CreateEmptyValuationCottage()
        {
            return base.CreateEmpty<IValuationCottage, ValuationCottage_DAO>();
        }

        public IValuationImprovement CreateEmptyValuationImprovement()
        {
            return base.CreateEmpty<IValuationImprovement, ValuationImprovement_DAO>();
        }

        public IValuationMainBuilding CreateEmptyValuationMainBuilding()
        {
            return base.CreateEmpty<IValuationMainBuilding, ValuationMainBuilding_DAO>();
        }

        public IValuationOutbuilding CreateEmptyValuationOutbuilding()
        {
            return base.CreateEmpty<IValuationOutbuilding, ValuationOutbuilding_DAO>();
        }

        public IValuationCombinedThatch CreateEmptyValuationCombinedThatch()
        {
            return base.CreateEmpty<IValuationCombinedThatch, ValuationCombinedThatch_DAO>();
        }

        public void DeleteValuationCottage(int ValuationKey)
        {
            const string query = "DELETE FROM [2AM].[dbo].[ValuationCottage] WHERE ValuationKey = @ValuationKey;";

            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@ValuationKey", ValuationKey));

            castleTransactionService.ExecuteNonQueryOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);
        }

        public void DeleteValuationCombinedThatch(int ValuationKey)
        {
            const string query = "DELETE FROM [2AM].[dbo].[ValuationCombinedThatch] WHERE ValuationKey = @ValuationKey;";

            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@ValuationKey", ValuationKey));

            castleTransactionService.ExecuteNonQueryOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);
        }

        public string BuildSAHLPropertyDataXML(string bondAccountNumber, int deedsOfficeKey)
        {
            if (bondAccountNumber == null)
                bondAccountNumber = "";

            string XML;

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("BondAccountNumber", Type.GetType("System.String"));
            dt.Columns.Add("DeedsOfficeKey", Type.GetType("System.Int32"));
            DataRow dr = dt.NewRow();
            dr[0] = bondAccountNumber;
            dr[1] = deedsOfficeKey;
            dt.Rows.Add(dr);
            ds.Tables.Add(dt);

            XML = ds.GetXml();

            return XML;
        }

        public IPropertyData GetLatestPropertyData(IProperty property, PropertyDataProviderDataServices propertyDataProviderDataServices)
        {
            IPropertyData propertyData = null;

            string propertyDataProviderDataServicesKey = ((int)propertyDataProviderDataServices).ToString();

            string HQL = "from PropertyData_DAO PD where PD.Property.Key = ? AND PD.PropertyDataProviderDataService.Key = ? order by PD.InsertDate desc";
            SimpleQuery query = new SimpleQuery(typeof(PropertyData_DAO), HQL, property.Key, propertyDataProviderDataServicesKey);
            query.SetQueryRange(1); // select one record

            object o = ActiveRecordBase.ExecuteQuery(query);
            PropertyData_DAO[] propertyDatas = o as PropertyData_DAO[];

            if (propertyDatas != null && propertyDatas.Length > 0)
                propertyData = new PropertyData(propertyDatas[0]);

            return propertyData;
        }

        public DataSet GetDataSetFromXML(string xml)
        {
            System.IO.StringReader stringReader = new System.IO.StringReader(xml);

            DataSet ds = new DataSet();
            ds.ReadXml(stringReader, XmlReadMode.Auto);

            return ds;
        }

        public void SaveAddress(IProperty property, IAddress address)
        {
            // first, we search to see if the address exists
            IAddressRepository addressRepository = RepositoryFactory.GetRepository<IAddressRepository>();

            // run a check to see if the address exists - if it does the address variable will be populated
            // with the existing address and we don't have to do anything, otherwise it's a new address
            // and we need to save it
            if (address.Key == 0 && !addressRepository.AddressExists(ref address))
            {
                Address_DAO daoAddress = (Address_DAO)((IDAOObject)address).GetDAOObject();
                daoAddress.SaveAndFlush();
            }

            // we should have a valid property and address by now
            if (address.Key <= 0 || property.Key <= 0)
                throw new Exception("Invalid address or property.");

            // link the address to the property
            property.Address = address;

            // save the property
            SaveProperty(property);

            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        public IEventList<IApplication> GetApplicationsForProperty(int PropertyKey)
        {
            string SQL = string.Format("select oml.offerkey from [2am]..offerMortgageloan oml join [2am]..property p on p.propertykey=oml.propertykey and p.propertykey={0}", PropertyKey);
            DataTable dt = new DataTable("OFFERKEYS");
            Helper.FillFromQuery(dt, SQL, Helper.GetSQLDBConnection(), null);
            Application_DAO[] Apps = new Application_DAO[dt.Rows.Count];
            for (int i = 0; i < Apps.Length; i++)
            {
                Apps[i] = ActiveRecordBase<Application_DAO>.Find(Convert.ToInt32(dt.Rows[i][0]));
            }
            return new DAOEventList<Application_DAO, IApplication, Application>(Apps);
        }

        public int GetDataProviderKeyFromDataProviderDataService(int DataProviderDataServiceKey)
        {
            DataProviderDataService_DAO dpds = ActiveRecordBase<DataProviderDataService_DAO>.Find(DataProviderDataServiceKey);
            if (dpds != null) return dpds.DataProvider.Key;
            return -1;
        }

        public IEventList<IPropertyAccessDetails> GetPropertyAccesDetailsByPropertyKey(int PropertyKey)
        {
            const string CQL = "from PropertyAccessDetails_DAO c where c.Property.Key = ?";
            SimpleQuery<PropertyAccessDetails_DAO> query = new SimpleQuery<PropertyAccessDetails_DAO>(CQL, PropertyKey);
            PropertyAccessDetails_DAO[] pad = query.Execute();
            return new DAOEventList<PropertyAccessDetails_DAO, IPropertyAccessDetails, PropertyAccessDetails>(pad);
        }

        public DataTable GetValuatorDataFromXMLHistory(int GenericKeyTypeKey, int GenericKey, string dataProvider)
        {
            // find the last AdCheck Valuation for this offer
            //DataSet dsValuatorData = new DataSet();
            const string CQL = "from XMLHistory_DAO xml where xml.GenericKey = ?";
            SimpleQuery<XMLHistory_DAO> query = new SimpleQuery<XMLHistory_DAO>(CQL, GenericKey);
            XMLHistory_DAO[] xmlhistory = query.Execute();

            xmlhistory = xmlhistory.Where(x => x.GenericKeyType.Key == GenericKeyTypeKey).OrderByDescending(x => x.InsertDate).ToArray();

            string header = string.Empty;
            if (!string.IsNullOrEmpty(dataProvider))
                header = string.Format("<{0}", dataProvider).ToLower();

            //int tableindex = 0;
            for (int i = 0; i < xmlhistory.Length; i++)
            {
                //tableindex = i;
                string XMLData = xmlhistory[i].XmlData;

                if (string.IsNullOrEmpty(header) || XMLData.ToLower().StartsWith(header))
                {
                    DataSet dsValuatorData = InitialiseXMLDataset(XMLData);
                    if (dsValuatorData != null && dsValuatorData.Tables.Count > 0 && dsValuatorData.Tables[0].TableName == "AdCheck_RequestValuation_Request")
                        return dsValuatorData.Tables["Parameters"];
                }
            }
            return new DataTable();
        }

        public string GetXMLHistoryData(int GenericKeyTypeKey, int GenericKey, string dataProvider, string methodName)
        {
            const string HQL = "from XMLHistory_DAO xml where xml.GenericKey = ?";
            SimpleQuery<XMLHistory_DAO> query = new SimpleQuery<XMLHistory_DAO>(HQL, GenericKey);
            XMLHistory_DAO[] xmlhistory = query.Execute();

            xmlhistory = xmlhistory.Where(x => x.GenericKeyType.Key == GenericKeyTypeKey).OrderByDescending(x => x.InsertDate).ToArray();

            string header = string.Format("<{0}.{1}", dataProvider, methodName).ToLower();

            for (int i = 0; i < xmlhistory.Length; i++)
            {
                string xml = xmlhistory[i].XmlData;

                if (xml.ToLower().StartsWith(header))
                    return xml;
            }

            return null;
        }

        public IXMLHistory GetXMLHistoryByKey(int XMLHistoryKey)
        {
            return base.GetByKey<IXMLHistory, XMLHistory_DAO>(XMLHistoryKey);
        }

        private static DataSet InitialiseXMLDataset(string XMLData)
        {
            DataSet DS = new DataSet();
            if (XMLData != null)
            {
                StringReader TextReader = new StringReader(XMLData);
                DS.ReadXml(TextReader, XmlReadMode.Auto);
            }
            return DS;
        }

        public void AdcheckValuationUpdateHOC(int valuationKey, int applicationKey)
        {
            Valuation_DAO val = Valuation_DAO.Find(valuationKey);

            if (val.Data == null)
                throw new Exception("The Data field of the Valuation record is null.");

            if (val.ValuationDataProviderDataService.Key != 4)
                throw new Exception("The ValuationDataProviderDataServiceKey for this valuation is not AdCheckPhysical.");

            DataSet ds = InitialiseXMLDataset(val.Data);

            DataTable response = null;

            //DataTable cottage = null;
            DataTable _improvements = null;
            DataTable otherImprovements = null;
            DataTable _insurance = null;

            if (ds.Tables.Contains("Response"))
                response = ds.Tables["Response"];

            if (response == null || response.Rows.Count == 0)
                throw new Exception("The Response table has no data");

            if (ds.Tables.Contains("Val_Improvements"))
                _improvements = ds.Tables["Val_Improvements"];

            if (ds.Tables.Contains("val_other_improvements_collection"))
                otherImprovements = ds.Tables["val_other_improvements_collection"];

            if (ds.Tables.Contains("Val_Insurance"))
                _insurance = ds.Tables["Val_Insurance"];

            if (_insurance == null || _insurance.Rows.Count == 0)
                throw new Exception("The Insurance table has no data");

            double mainrate = Convert.ToDouble(_insurance.Rows[0]["main_rate"]);
            double mainmeterage = Convert.ToDouble(_insurance.Rows[0]["main_square_meterage"]);

            double mainVal = mainrate * mainmeterage;
            double outVal = 0;
            double cottageVal = 0;

            double outrate = _insurance.Rows[0]["out_rate"] != DBNull.Value ? Convert.ToDouble(_insurance.Rows[0]["out_rate"]) : 0;
            double outmeterage = _insurance.Rows[0]["out_square_meterage"] != DBNull.Value ? Convert.ToDouble(_insurance.Rows[0]["out_square_meterage"]) : 0;
            outVal = outrate * outmeterage;

            double cottagerate = _insurance.Rows[0]["cottage_rate"] != DBNull.Value ? Convert.ToDouble(_insurance.Rows[0]["cottage_rate"]) : 0;
            double cottagemeterage = _insurance.Rows[0]["cottage_square_meterage"] != DBNull.Value ? Convert.ToDouble(_insurance.Rows[0]["cottage_square_meterage"]) : 0;
            cottageVal = cottagerate * cottagemeterage;

            double totalSumInsured = mainVal + outVal + cottageVal;

            int mainRoofType = _insurance.Rows[0]["main_roof_type_id"] != DBNull.Value ? Convert.ToInt32(_insurance.Rows[0]["main_roof_type_id"]) : 0;
            int outRoofType = _insurance.Rows[0]["out_roof_type_id"] != DBNull.Value ? Convert.ToInt32(_insurance.Rows[0]["out_roof_type_id"]) : 0;
            int cottageRoofType = _insurance.Rows[0]["cottage_roof_type_id"] != DBNull.Value ? Convert.ToInt32(_insurance.Rows[0]["cottage_roof_type_id"]) : 0;

            double thatchAmount = 0;
            double convAmount = 0;

            switch (mainRoofType)
            {
                case 1:
                case 3:
                case 4:
                    convAmount += mainVal;
                    break;

                case 2:
                case 5:
                case 6:
                    thatchAmount += mainVal;
                    break;

                case 0:
                    break;

                default:
                    throw new Exception("The main_roof_type_id field of the Insurance table contains an unsupported value");
            }

            switch (outRoofType)
            {
                case 1:
                case 3:
                case 4:
                    convAmount += outVal;
                    break;

                case 2:
                case 5:
                case 6:
                    thatchAmount += outVal;
                    break;

                case 0:
                    break;

                default:
                    throw new Exception("The out_roof_type_id field of the Insurance table contains an unsupported value");
            }

            switch (cottageRoofType)
            {
                case 1:
                case 3:
                case 4:
                    convAmount += cottageVal;
                    break;

                case 2:
                case 5:
                case 6:
                    thatchAmount += cottageVal;
                    break;

                case 0:
                    break;

                default:
                    throw new Exception("The cottage_roof_type_id field of the Insurance table contains an unsupported value");
            }

            //int subsidence = 1;
            //int construction = 1;
            //int status = 0;
            //string username = "AdCheck Valuer";
            int roofNumber = 2; //conventional

            if (thatchAmount > 0)
            {
                if (convAmount > 0)
                    roofNumber = 3; //partial
                else
                    roofNumber = 1; //thatch
            }

            double totalImprovementsVal = 0;

            if (_improvements != null)
                totalImprovementsVal = _improvements.Rows[0]["total_improvements_value"] != DBNull.Value ? Convert.ToDouble(_improvements.Rows[0]["total_improvements_value"]) : 0;

            double percentage20 = (totalSumInsured + totalImprovementsVal) * 0.2;

            double thatchOtherImprovements = 0;

            if (otherImprovements != null)
            {
                foreach (DataRow row in otherImprovements.Rows)
                {
                    int roofType = row["val_other_improvement_roof_type_id"] != DBNull.Value ? Convert.ToInt32(row["val_other_improvement_roof_type_id"]) : 0;

                    if (roofType == 2 || roofType == 5 || roofType == 6)
                    {
                        double rate = row["rate"] != DBNull.Value ? Convert.ToDouble(row["rate"]) : 0;
                        double meterage = row["square_meterage"] != DBNull.Value ? Convert.ToDouble(row["square_meterage"]) : 0;
                        thatchOtherImprovements += rate * meterage;
                    }
                }
            }

            thatchAmount = (thatchAmount + thatchOtherImprovements);
            totalSumInsured = (totalSumInsured + totalImprovementsVal + percentage20);
            convAmount = totalSumInsured - thatchAmount;

            if (totalSumInsured <= 0)
                return;

            //now update the valuation
            val.HOCConventionalAmount = convAmount;
            val.HOCRoof = HOCRoof_DAO.Find(roofNumber);

            //val.HOCShingleAmount;
            val.HOCThatchAmount = thatchAmount;
            val.ValuationHOCValue = totalSumInsured;
            val.SaveAndFlush();

            //check for open account
            bool openAccount = false;

            IHOCRepository hocRepo = RepositoryFactory.GetRepository<IHOCRepository>();
            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            IAccountHOC accHOC = hocRepo.RetrieveHOCByOfferKey(applicationKey, ref openAccount);
            IHOC hoc = null;

            if (accHOC != null)
                hoc = accHOC.HOC;

            //update hoc if insurer = sahl
            if (hoc != null
                && hoc.HOCInsurer.Key == (int)HOCInsurers.SAHLHOC
                && (hoc.HOCStatus.Key == (int)HocStatuses.Open || hoc.HOCStatus.Key == (int)HocStatuses.PaidUpwithHOC))
            {
                double total = (val.HOCConventionalAmount.HasValue ? val.HOCConventionalAmount.Value : 0)
                            + (val.HOCShingleAmount.HasValue ? val.HOCShingleAmount.Value : 0)
                            + (val.HOCThatchAmount.HasValue ? val.HOCThatchAmount.Value : 0);

                hoc.HOCConventionalAmount = val.HOCConventionalAmount.HasValue ? val.HOCConventionalAmount.Value : 0;
                hoc.HOCShingleAmount = val.HOCShingleAmount.HasValue ? val.HOCShingleAmount.Value : 0;
                hoc.HOCThatchAmount = val.HOCThatchAmount.HasValue ? val.HOCThatchAmount.Value : 0;
                hoc.SetHOCTotalSumInsured(total);
                hoc.HOCRoof = BMTM.GetMappedType<IHOCRoof, HOCRoof_DAO>(val.HOCRoof);
                hoc.UserID = "AdCheck Valuation";
                hoc.ChangeDate = DateTime.Now;
                hocRepo.SaveHOC(hoc);
                hocRepo.UpdateHOCPremium(hoc.Key);
                hoc.Refresh();
                hocRepo.UpdateHOCWithHistory(null, hoc.HOCInsurer.Key, hoc, 'V');
            }
        }

        public void ValuationUpdateHOC(int valuationKey, GenericKeyTypes genericKeyTypeKey, int genericKey)
        {
            var valuation = GetValuationByKey(valuationKey);
            ValuationUpdateHOC(valuation, genericKeyTypeKey, genericKey);
        }

        public void ValuationUpdateHOC(IValuation valuation, GenericKeyTypes genericKeyTypeKey, int genericKey)
        {
            bool openAccount = false;
            IHOCRepository hocRepository = RepositoryFactory.GetRepository<IHOCRepository>();

            IAccountHOC hocAccount = null;
            switch (genericKeyTypeKey)
            {
                case GenericKeyTypes.Account:
                    hocAccount = hocRepository.RetrieveHOCByAccountKey(genericKey);
                    break;

                case GenericKeyTypes.Offer:
                    hocAccount = hocRepository.RetrieveHOCByOfferKey(genericKey, ref openAccount);
                    break;
            }
            ValuationUpdateHOC(valuation, hocAccount);
        }

        private void ValuationUpdateHOC(IValuation valuation, IAccountHOC accountHOC)
        {
            IHOCRepository hocRepository = RepositoryFactory.GetRepository<IHOCRepository>();
            IHOC hoc = null;

            if (accountHOC != null)
                hoc = accountHOC.HOC;

            //update hoc if insurer = sahl
            if (hoc != null
                && hoc.HOCInsurer.Key == (int)HOCInsurers.SAHLHOC
                && (hoc.HOCStatus.Key == (int)HocStatuses.Open || hoc.HOCStatus.Key == (int)HocStatuses.PaidUpwithHOC))
            {
                double total = (valuation.HOCConventionalAmount.HasValue ? valuation.HOCConventionalAmount.Value : 0)
                            + (valuation.HOCShingleAmount.HasValue ? valuation.HOCShingleAmount.Value : 0)
                            + (valuation.HOCThatchAmount.HasValue ? valuation.HOCThatchAmount.Value : 0);

                hoc.HOCConventionalAmount = valuation.HOCConventionalAmount.HasValue ? valuation.HOCConventionalAmount.Value : 0;
                hoc.HOCShingleAmount = valuation.HOCShingleAmount.HasValue ? valuation.HOCShingleAmount.Value : 0;
                hoc.HOCThatchAmount = valuation.HOCThatchAmount.HasValue ? valuation.HOCThatchAmount.Value : 0;
                hoc.SetHOCTotalSumInsured(total);
                hoc.HOCRoof = valuation.HOCRoof;
                hoc.UserID = string.Format("{0} {1}", valuation.ValuationDataProviderDataService.DataProviderDataService.DataProvider.Description, valuation.ValuationDataProviderDataService.DataProviderDataService.DataService.Description);
                hoc.ChangeDate = DateTime.Now;
                hocRepository.SaveHOC(hoc);
                hocRepository.UpdateHOCPremium(hoc.Key);
                hoc.Refresh();
                hocRepository.UpdateHOCWithHistory(null, hoc.HOCInsurer.Key, hoc, 'V');
            }
        }

        public double CalculateCombinedThatchValue(IValuationMainBuilding valMainBuilding, IValuationCottage valCottage, IEventList<IValuationOutbuilding> valOutBuildings)
        {
            double combinedThatchValue = 0;

            if (valMainBuilding != null)
            {
                if (valMainBuilding.ValuationRoofType != null && valMainBuilding.ValuationRoofType.Key == (int)ValuationRoofTypes.Thatch)
                    combinedThatchValue += (Convert.ToDouble(valMainBuilding.Rate) * Convert.ToDouble(valMainBuilding.Extent));
            }
            if (valCottage != null)
            {
                if (valCottage.ValuationRoofType != null && valCottage.ValuationRoofType.Key == (int)ValuationRoofTypes.Thatch)
                    combinedThatchValue += (Convert.ToDouble(valCottage.Rate) * Convert.ToDouble(valCottage.Extent));
            }
            for (int i = 0; i < valOutBuildings.Count; i++)
            {
                if (valOutBuildings[i].ValuationRoofType.Key == (int)ValuationRoofTypes.Thatch)
                    combinedThatchValue += (Convert.ToDouble(valOutBuildings[i].Rate) * Convert.ToDouble(valOutBuildings[i].Extent));
            }
            return combinedThatchValue;
        }

        public bool HasAdCheckPropertyID(int propertyKey)
        {
            const string HQL = "from PropertyData_DAO PD where PD.Property.Key = ? AND PD.PropertyDataProviderDataService.DataProviderDataService.DataProvider.Key = ? order by PD.InsertDate desc";
            SimpleQuery query = new SimpleQuery(typeof(PropertyData_DAO), HQL, propertyKey, (int)DataProviders.AdCheck);
            query.SetQueryRange(1); // select one record
            object o = ActiveRecordBase.ExecuteQuery(query);
            PropertyData_DAO[] propertyDatas = o as PropertyData_DAO[];

            if (propertyDatas != null && propertyDatas.Length > 0)
                return true;

            return false;
        }

        public bool HasLightStonePropertyID(int propertyKey)
        {
            const string HQL = "from PropertyData_DAO PD where PD.Property.Key = ? AND PD.PropertyDataProviderDataService.DataProviderDataService.DataProvider.Key = ? order by PD.InsertDate desc";
            SimpleQuery query = new SimpleQuery(typeof(PropertyData_DAO), HQL, propertyKey, (int)DataProviders.LightStone);
            query.SetQueryRange(1); // select one record
            object o = ActiveRecordBase.ExecuteQuery(query);
            PropertyData_DAO[] propertyDatas = o as PropertyData_DAO[];

            if (propertyDatas != null && propertyDatas.Length > 0)
                return true;

            return false;
        }

        public IProperty GetPropertyByHOC(IHOC hoc)
        {
            if (hoc.Key > 0)
            {
                // The HOC is already created and the AccountRelationship can be used to retrieve the Property
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                string sql = UIStatementRepository.GetStatement("Repositories.PropertyRepository", "GetPropertyByHOCKey");
                SimpleQuery<Property_DAO> query = new SimpleQuery<Property_DAO>(QueryLanguage.Sql, sql, hoc.Key, hoc.Key);
                query.AddSqlReturnDefinition(typeof(Property_DAO), "p");
                query.SetQueryRange(1);
                Property_DAO[] res = query.Execute();

                if (res != null && res.Length > 0)
                {
                    return BMTM.GetMappedType<IProperty, Property_DAO>(res[0]);
                }
                else
                    return null;
            }
            else
            {
                // If the HOC is being "ADDED" it will be done via the Workflow with the parent being an Application.
                // We can safely go via the HOC Account to retrieve the Application and in turn the Property.
                IApplication application = GetApplicationByHOCAccountKey(hoc.FinancialService.Account.Key);
                return GetPropertyByApplicationKey(application.Key);
            }
        }

        public IApplication GetApplicationByHOCAccountKey(int AccountKey)
        {
            string sql = UIStatementRepository.GetStatement("Repositories.PropertyRepository", "GetApplicationByHOCAccountKey");
            SimpleQuery<Application_DAO> query = new SimpleQuery<Application_DAO>(QueryLanguage.Sql, sql, AccountKey);
            query.AddSqlReturnDefinition(typeof(Application_DAO), "ofr");
            Application_DAO[] res = query.Execute();

            if (res != null && res.Length > 0)
            {
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return BMTM.GetMappedType<IApplication, Application_DAO>(res[0]);
            }
            else
                return null;
        }

        public IProperty GetPropertyByApplicationKey(int ApplicationKey)
        {
            string sql = UIStatementRepository.GetStatement("Repositories.PropertyRepository", "GetPropertyByApplicationKey");
            SimpleQuery<Property_DAO> query = new SimpleQuery<Property_DAO>(QueryLanguage.Sql, sql, ApplicationKey);
            query.AddSqlReturnDefinition(typeof(Property_DAO), "p");
            Property_DAO[] res = query.Execute();

            if (res != null && res.Length > 0)
            {
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return BMTM.GetMappedType<IProperty, Property_DAO>(res[0]);
            }
            else
                return null;
        }

        public void GetLightStoneValuationForWorkFlow(int ApplicationKey, string ADUser)
        {
            IProperty property = GetPropertyByApplicationKey(ApplicationKey);

            GetLightStoneValuation(ApplicationKey, GenericKeyTypes.Offer, property, ADUser);
        }

        private void GetLightStoneValuation(int genericKey, GenericKeyTypes gkType, IProperty property, string ADUser)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

            //do valuation
            ILookupRepository lrepo = RepositoryFactory.GetRepository<ILookupRepository>();
            ILightStoneService AVM = ServiceFactory.GetService<ILightStoneService>();

            //get the PropID
            int propID = Convert.ToInt32(GetLightStonePropertyID(property));
            DataSet valuationDS = null;
            try
            {
                //updated to do this in a single method call....
                valuationDS = AVM.ReturnValuation(genericKey, (int)gkType, propID, 1, 1, false);
            }
            catch (Exception ex)
            {
                spc.DomainMessages.Add(new Warning("The LightStone web service call failed. Unable to retrieve Valuation data.", ex.Message));
                return;
            }

            IValuationDiscriminatedLightstoneAVM val = CreateEmptyValuation(ValuationDataProviderDataServices.LightstoneAutomatedValuation) as IValuationDiscriminatedLightstoneAVM;
            val.Data = valuationDS.GetXml();
            val.IsActive = false;
            val.Property = property;
            val.HOCRoof = lrepo.HOCRoof.ObjectDictionary["2"];

            if (valuationDS.Tables.Contains("Valuation") && valuationDS.Tables["Valuation"].Rows.Count > 0 && valuationDS.Tables["Valuation"].Columns.Contains("PredictedPrice") && valuationDS.Tables["Valuation"].Rows[0]["PredictedPrice"] != DBNull.Value)
                val.ValuationAmount = Convert.ToDouble(valuationDS.Tables["Valuation"].Rows[0]["PredictedPrice"]);
            else
                val.ValuationAmount = 0;

            val.ValuationHOCValue = val.ValuationAmount > 0 ? val.ValuationAmount : 1;

            //val.ValuationDataProviderDataService; //this is the discriminator value

            val.ValuationDate = DateTime.Now;
            val.ValuationEscalationPercentage = 0;
            val.ValuationMunicipal = 0;
            val.ValuationStatus = lrepo.ValuationStatus.ObjectDictionary[((int)ValuationStatuses.Complete).ToString()];

            // NB for the line below: SPC.Principal.Identity.Name; this is a thread id when called from WorkFlow
            val.ValuationUserID = ADUser;
            val.Valuator = GetValuatorByKey(50);

            spc.ExclusionSets.Add(RuleExclusionSets.ValuationUpdatePreviousToInactive);
            SaveValuation(val);
            spc.ExclusionSets.Clear();
        }

        public string PreviousLightStoneUniqueID(int propertyKey)
        {
            var pRepo = RepositoryFactory.GetRepository<IPropertyRepository>();

            //get valuation
            IValuation val = pRepo.GetLatestValuationByPropertyKey(propertyKey);

            if (val != null && val.ValuationDataProviderDataService.DataProviderDataService.Key == (int)DataProviderDataServices.LightstonePhysicalValuation && val.Data != null)
            {
                //get xmlhistorykey from data
                XDocument xdoc = XDocument.Parse(val.Data);
                XElement xe = xdoc.Root.Descendants().Where(x => x.Name.LocalName == "UniqueID").FirstOrDefault();

                if (xe != null)
                {
                    return xe.Value;
                }
            }

            return String.Empty;
        }

        public string GetLightStonePropertyID(IProperty property)
        {
            if (property != null && property.PropertyDatas.Count > 0)
            {
                // Find LightStone Property Id
                return property.PropertyDatas
                                        .Where(x => x.PropertyDataProviderDataService.Key == (int)PropertyDataProviderDataServices.LightstonePropertyIdentification)
                                        .Select(x => x.PropertyID).FirstOrDefault();
            }

            return String.Empty;
        }

        public void RequestLightStoneValuation(IPropertyAccessDetails propAD, IProperty prop, int gKey, int gkTypeKey, bool isReview, DateTime assessmentDate, String reason, String instructions, out string lightstonePropertyId)
        {
            var sp = SAHLPrincipal.GetCurrent();
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(sp);

            var lightstoneService = ServiceFactory.GetService<ILightStoneService>();
            var propRepo = RepositoryFactory.GetRepository<IPropertyRepository>();

            DataSet resultDS = null;
            try
            {
                string prevUniqueID = null;

                //get valuation
                if (isReview)
                    prevUniqueID = propRepo.PreviousLightStoneUniqueID(prop.Key);

                #region PopulateAndSendValuationRequest

                lightstonePropertyId = propRepo.GetLightStonePropertyID(prop);

                if (!String.IsNullOrEmpty(lightstonePropertyId))
                {
                    LightstoneValidatedProperty.PropertyDetailsDataTable lsTable = new LightstoneValidatedProperty.PropertyDetailsDataTable();
                    LightstoneValidatedProperty.PropertyDetailsRow row = lsTable.NewPropertyDetailsRow();

                    row.UniqueID = 0;

                    if (!string.IsNullOrEmpty(prevUniqueID))
                        row.PreviousUniqueID = int.Parse(prevUniqueID);

                    row.SAHLUser = sp.Identity.Name;
                    row.LightstonePropertyID = lightstonePropertyId;
                    row.PropertyTitleType = prop.DeedsPropertyType.Key == 1 ? "Freehold" : "Sectional Title"; //this should be based on DeedsPropertyType and is either Freehold (erf) or Sectional Title (unit)
                    row.ValuationReason = reason;
                    row.ValuationInstructions = string.Format("{0}:{1} {2}", Enum.GetName(typeof(GenericKeyTypes), gkTypeKey), gKey, instructions);
                    row.ValuationRequiredDate = assessmentDate.ToString();
                    row.Contact1 = propAD.Contact1;
                    row.Contact1MobilePhone = propAD.Contact1MobilePhone;
                    row.Contact1Phone = propAD.Contact1Phone;
                    row.Contact1WorkPhone = propAD.Contact1WorkPhone;
                    row.Contact2 = propAD.Contact2;
                    row.Contact2Phone = propAD.Contact2Phone;

                    lsTable.AddPropertyDetailsRow(row);

                    resultDS = lightstoneService.RequestValuationForLightstoneValidatedProperty(gKey, gkTypeKey, lsTable);
                }
                else
                {
                    LightstoneNonValidatedProperty.PropertyDetailsDataTable lsTable = new LightstoneNonValidatedProperty.PropertyDetailsDataTable();
                    LightstoneNonValidatedProperty.PropertyDetailsRow row = lsTable.NewPropertyDetailsRow();

                    IAddressStreet address = prop.Address as IAddressStreet;

                    row.UniqueID = 0;

                    if (!string.IsNullOrEmpty(prevUniqueID))
                        row.PreviousUniqueID = int.Parse(prevUniqueID);

                    row.SAHLUser = sp.Identity.Name.Replace("SAHL\\", "");
                    row.PropertyTitleType = prop.DeedsPropertyType.Key == 1 ? "Freehold" : "Sectional Title"; //this should be based on DeedsPropertyType and is either Freehold (erf) or Sectional Title (unit)
                    row.ValuationReason = reason;
                    row.ValuationInstructions = string.Format("{0}:{1} {2}", Enum.GetName(typeof(GenericKeyTypes), gkTypeKey), gKey, instructions);
                    row.ValuationRequiredDate = assessmentDate.ToString();

                    row.Contact1 = propAD.Contact1;
                    row.Contact1MobilePhone = propAD.Contact1MobilePhone;
                    row.Contact1Phone = propAD.Contact1Phone;
                    row.Contact1WorkPhone = propAD.Contact1WorkPhone;
                    row.Contact2 = propAD.Contact2;
                    row.Contact2Phone = propAD.Contact2Phone;

                    row.UnitNumber = address.UnitNumber;
                    row.BuildingName = address.BuildingName;
                    row.BuildingNumber = address.BuildingNumber;
                    row.StreetName = address.StreetName;
                    row.StreetNumber = address.StreetNumber;
                    row.Suburb = address.RRR_SuburbDescription;
                    row.City = address.RRR_CityDescription;
                    row.Province = address.RRR_ProvinceDescription;
                    row.Country = address.RRR_CountryDescription;
                    row.PostalCode = address.RRR_PostalCode;

                    row.ErfMetroDescription = prop.ErfMetroDescription;
                    row.ErfNumber = prop.ErfNumber;
                    row.ErfPortionNumber = prop.ErfPortionNumber;
                    row.PropertyDescription1 = prop.PropertyDescription1;
                    row.PropertyDescription2 = prop.PropertyDescription2;
                    row.PropertyDescription3 = prop.PropertyDescription3;
                    row.SectionalSchemeName = prop.SectionalSchemeName;
                    row.SectionalUnitNumber = prop.SectionalUnitNumber;

                    lsTable.AddPropertyDetailsRow(row);

                    resultDS = lightstoneService.RequestValuationForLightstoneNonValidatedProperty(gKey, gkTypeKey, lsTable);
                }

                if (resultDS == null)
                    throw new Exception("Null response dataset received");

                if (resultDS.Tables.Count == 0 || resultDS.Tables[0].Rows.Count == 0)
                    throw new Exception("Empty response dataset received");

                DataTable dt = resultDS.Tables[0];

                if (!dt.Columns.Contains("UniqueID") || !dt.Columns.Contains("Successful") || !dt.Columns.Contains("ErrorMessage"))
                    throw new Exception("The results table is missing required columns");

                DataRow rr = dt.Rows[0];

                string successful = Convert.ToString(rr["Successful"]);
                string errorMessage = Convert.ToString(rr["ErrorMessage"]);

                if (string.IsNullOrEmpty(successful))
                    throw new Exception(string.Format("Missing Success indicator in dataset: {0}", resultDS.GetXml()));

                bool success = Boolean.Parse(successful);

                if (!success)
                {
                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        throw new Exception("The request was unsuccessful, but no ErrorMessage was returned");
                    }
                    else
                    {
                        throw new Exception(string.Format("Response ErrorMessage: {0}", errorMessage));
                    }
                }

                #endregion PopulateAndSendValuationRequest
            }
            catch (Exception ex)
            {
                spc.DomainMessages.Add(new Error("Lightstone WebService error, please contact a system administrator", "Lightstone WebService error, please contact a system administrator"));
                spc.DomainMessages.Add(new Error(ex.Message, ex.Message));
                throw;
            }
        }

        public IValuationDiscriminatedLightStonePhysical CreateValuationLightStone(IProperty prop)
        {
            var sp = SAHLPrincipal.GetCurrent();
            var propRepo = RepositoryFactory.GetRepository<IPropertyRepository>();
            var lkRepo = RepositoryFactory.GetRepository<ILookupRepository>();

            //Create and save a new valuation

            #region Valuation

            IValuationDiscriminatedLightStonePhysical valuation = propRepo.CreateEmptyValuation(ValuationDataProviderDataServices.LightstonePhysicalValuation) as IValuationDiscriminatedLightStonePhysical;
            valuation.Property = prop;
            valuation.ValuationStatus = lkRepo.ValuationStatus.ObjectDictionary[((int)ValuationStatuses.Pending).ToString()];
            valuation.IsActive = false;
            valuation.ValuationUserID = sp.Identity.Name.Replace(@"SAHL\", "");
            valuation.HOCRoof = lkRepo.HOCRoof.ObjectDictionary["2"];//Conventional Roof Type
            valuation.ValuationDate = DateTime.Now;

            propRepo.SaveValuation(valuation);

            return valuation;

            #endregion Valuation
        }

        public DataTable GetValuationsAndReasons(int propertyKey)
        {
            string query = UIStatementRepository.GetStatement("Repositories.PropertyRepository", "GetValuationsAndReasons");
            ParameterCollection pc = new ParameterCollection();
            pc.Add(new SqlParameter("@PropertyKey", propertyKey));
            return castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), pc).Tables[0];
        }

        public IComcorpOfferPropertyDetails GetComcorpOfferPropertyDetails(int offerKey)
        {
            string query = @"SELECT [SellerIDNo],[SAHLOccupancyType],[SAHLPropertyType],[SAHLTitleType],[SectionalTitleUnitNo],[ComplexName],[StreetNo],[StreetName],[Suburb],[City],[Province],
                                [PostalCode],[ContactCellphone],[ContactName],[NamePropertyRegistered],[StandErfNo],[PortionNo]
                            FROM [2AM].[dbo].[ComcorpOfferPropertyDetails]
                            WHERE OfferKey = @OfferKey";

            ParameterCollection pc = new ParameterCollection();
            pc.Add(new SqlParameter("@OfferKey", offerKey));
            DataTable dtComcorpOfferPropertyDetails = castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), pc).Tables[0];

            if (dtComcorpOfferPropertyDetails == null ||
                dtComcorpOfferPropertyDetails.Rows.Count != 1 ||
                dtComcorpOfferPropertyDetails.Rows[0].ItemArray.Length != 17)
                return null;

            IComcorpOfferPropertyDetails comcorpOfferPropertyDetails = new ComcorpOfferPropertyDetails()
            {
                SellerIDNo = dtComcorpOfferPropertyDetails.Rows[0].ItemArray[0].ToString(),
                SAHLOccupancyType = dtComcorpOfferPropertyDetails.Rows[0].ItemArray[1].ToString(),
                SAHLPropertyType = dtComcorpOfferPropertyDetails.Rows[0].ItemArray[2].ToString(),
                SAHLTitleType = dtComcorpOfferPropertyDetails.Rows[0].ItemArray[3].ToString(),
                SectionalTitleUnitNo = dtComcorpOfferPropertyDetails.Rows[0].ItemArray[4].ToString(),
                ComplexName = dtComcorpOfferPropertyDetails.Rows[0].ItemArray[5].ToString(),
                StreetNo = dtComcorpOfferPropertyDetails.Rows[0].ItemArray[6].ToString(),
                StreetName = dtComcorpOfferPropertyDetails.Rows[0].ItemArray[7].ToString(),
                Suburb = dtComcorpOfferPropertyDetails.Rows[0].ItemArray[8].ToString(),
                City = dtComcorpOfferPropertyDetails.Rows[0].ItemArray[9].ToString(),
                Province = dtComcorpOfferPropertyDetails.Rows[0].ItemArray[10].ToString(),
                PostalCode = dtComcorpOfferPropertyDetails.Rows[0].ItemArray[11].ToString(),
                ContactCellphone = dtComcorpOfferPropertyDetails.Rows[0].ItemArray[12].ToString(),
                ContactName = dtComcorpOfferPropertyDetails.Rows[0].ItemArray[13].ToString(),
                NamePropertyRegistered = dtComcorpOfferPropertyDetails.Rows[0].ItemArray[14].ToString(),
                StandErfNo = dtComcorpOfferPropertyDetails.Rows[0].ItemArray[15].ToString(),
                PortionNo = dtComcorpOfferPropertyDetails.Rows[0].ItemArray[16].ToString()
            };

            return comcorpOfferPropertyDetails;
        }
    }
}