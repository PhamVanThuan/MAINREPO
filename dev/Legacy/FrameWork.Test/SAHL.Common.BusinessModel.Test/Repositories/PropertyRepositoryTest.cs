using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Castle.ActiveRecord;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Test;
using SAHL.Test.DAOHelpers;
using SAHL.Common.CacheData;
using SAHL.Common.Security;
using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class PropertyRepositoryTest : TestBase
    {
        [SetUp()]
        public void Setup()
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            spc.DomainMessages.Clear();
        }

        [TearDown]
        public void TearDown()
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            spc.DomainMessages.Clear();
        }

        IPropertyRepository _repo;

        public IPropertyRepository PropertyRepository
        {
            get
            {
                if (_repo == null)
                    _repo = RepositoryFactory.GetRepository<IPropertyRepository>();

                return _repo;
            }
        }

        [Test]
        public void GetPropertyByKeyTest()
        {
            using (new SessionScope())
            {
                Property_DAO prop = Property_DAO.FindFirst();
                IProperty property = PropertyRepository.GetPropertyByKey(prop.Key);
                Assert.IsNotNull(property);
            }
        }

        [Test]
        public void GetPropertyByAddressKeyTest()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 ad.addressKey
                from [2am].[dbo].property prop (nolock)
                join [2am].[dbo].Address ad (nolock)
                    on prop.addressKey = ad.addressKey
                where ad.addressKey > 1";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int addressKey = Convert.ToInt32(o);

                List<IProperty> propList = PropertyRepository.GetPropertyByAddressKey(addressKey);
                Assert.IsNotNull(propList);
                Assert.IsTrue(propList.Count > 0);
            }
        }

        [Test]
        public void ValuationsCollections()
        {
            using (new SessionScope())
            {
                string query = "select top 1 v.ValuationKey from [2AM].[dbo].[Valuation] v (nolock) "
                    + "join [2AM].[dbo].[ValuationOutbuilding] vo (nolock)  on vo.ValuationKey = v.ValuationKey ";
                DataTable DT = base.GetQueryResults(query);

                ValuationDiscriminatedSAHLManual_DAO vm = ValuationDiscriminatedSAHLManual_DAO.Find(DT.Rows[0][0]);
                int x = vm.ValuationOutBuildings.Count;
            }
        }

        /// <summary>
        /// Tests the retrieval of valuations off a property.
        /// </summary>
        [Test]
        public void Valuations2()
        {
            using (new SessionScope())
            {
                string query = "select * from [2AM].[dbo].[Valuation] (nolock) where PropertyKey = 82608";
                DataTable DT = base.GetQueryResults(query);

                Property_DAO prop = Property_DAO.Find(82608);
                IList<Valuation_DAO> list = prop.Valuations;

                Assert.That(list.Count == DT.Rows.Count);
            }
        }

        [Test]
        public void GetValuationByPropertyKey()
        {
            using (new SessionScope())
            {
                string query = @"select top 1 propertykey, count(propertykey) from [2am].[dbo].Valuation (nolock)
								group by propertykey
								order by count(ValuationKey) desc";

                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count > 0);

                int key = Convert.ToInt32(DT.Rows[0][0]);
                int count = Convert.ToInt32(DT.Rows[0][1]);

                IReadOnlyEventList<IValuation> list = PropertyRepository.GetValuationByPropertyKey(key);

                Assert.That(list.Count == count);
            }
        }

        [Test]
        public void GetValuatorByKey()
        {
            using (new SessionScope())
            {
                string query = "select v.* from [2am].[dbo].[Valuator] v (nolock) "
                    + "where v.ValuatorKey in (select top 1 ValuatorKey from [2am].[dbo].[Valuator] (nolock)) ";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count > 0);

                IDomainMessageCollection Messages = new DomainMessageCollection();

                IValuator v = PropertyRepository.GetValuatorByKey(Convert.ToInt32(DT.Rows[0][0]));

                Assert.That(DT.Rows.Count == 1);
            }
        }

        [Test]
        public void GetValuatorsByOriginationSource()
        {
            using (new SessionScope())
            {
                string query = @"select count(*) from [2am]..OriginationSourceValuator osv (nolock)
                            join [2am]..Valuator v (nolock) on v.ValuatorKey = osv.ValuatorKey
                            where osv.OriginationSourceKey = " + (int)Globals.OriginationSources.SAHomeLoans;

                int count = Convert.ToInt32(DBHelper.ExecuteScalar(query));

                IDomainMessageCollection Messages = new DomainMessageCollection();

                List<IValuator> valuators = PropertyRepository.GetValuatorsByOriginationSource((int)Globals.OriginationSources.SAHomeLoans);

                Assert.That(valuators.Count == count);
            }
        }

        [Test]
        public void GetPropertyByAccountKey()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 a.AccountKey
                from [2am].[dbo].[Account] a (nolock)
                inner join [2am].[dbo].[financialService] mfs (nolock)
	                on a.AccountKey = mfs.AccountKey
                inner join [2am].[fin].[MortgageLoan] ml (nolock)
	                on mfs.FinancialServiceKey = ml.FinancialServiceKey
                inner join [2am].[dbo].[Property] p (nolock)
	                on p.PropertyKey = ml.PropertyKey";

                int PropertyKey = 0;
                ParameterCollection parameters = new ParameterCollection();
                Object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), parameters);

                if (o != null)
                    PropertyKey = Convert.ToInt32(o);

                IProperty property = PropertyRepository.GetPropertyByAccountKey(PropertyKey);
                Assert.IsNotNull(property);
            }
        }

        [Test]
        public void FindMatchingProperty()
        {
            using (new SessionScope())
            {
                string query = @"select top 1 PropertyKey from [2am].[dbo].[Property] (nolock) where addresskey is not null order by PropertyKey";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);

                Property_DAO prop = Property_DAO.Find(Convert.ToInt32(DT.Rows[0][0]));

                prop.ErfMetroDescription = null;

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IProperty match = BMTM.GetMappedType<Property>(prop);
                IProperty iprop = PropertyRepository.FindMatchingProperty(match);

                Assert.That(iprop != null);
            }
        }

        [Test]
        public void SaveValuation()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                string query = "select top 1 * from Valuation (nolock) where ValuationDataProviderDataServiceKey = 1 and IsActive = 1";
                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 0)
                    Assert.Ignore("No data available");

                int key = Convert.ToInt32(DT.Rows[0][0]);
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IValuation valuation = BMTM.GetMappedType<ValuationDiscriminatedSAHLManual>(ValuationDiscriminatedSAHLManual_DAO.Find(key));

                string data = valuation.Data;
                valuation.Data = "SaveTest";

                PropertyRepository.SaveValuation(valuation);

                valuation.Data = data;
                PropertyRepository.SaveValuation(valuation);

                Assert.That(valuation.Key > 0);
            }
        }

        [Test]
        public void SavePropertyAccessDetails()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                string query = "select top 1 p.PropertyKey from Property p (nolock) where p.PropertyKey not in (select PropertyKey from PropertyAccessDetails (nolock) )";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);

                int PropertyKey = Convert.ToInt32(DT.Rows[0][0]);

                IPropertyAccessDetails pad = PropertyRepository.CreateEmptyPropertyAccessDetails();

                pad.Contact1 = "test";
                pad.Contact1Phone = "test";
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                pad.Property = BMTM.GetMappedType<Property>(Property_DAO.Find(PropertyKey));

                PropertyRepository.SavePropertyAccessDetails(pad);
                Assert.That(pad.Key > 0);
                PropertyAccessDetails_DAO dao = PropertyAccessDetails_DAO.Find(pad.Key);
                Assert.That(dao != null);
            }
        }

        [Test]
        public void CreateAndSaveValuation()
        {
            IValuation valuation = null;
            IValuationDiscriminatedSAHLManual v = null;

            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                try
                {
                    string query = "select top 1 * from Valuation (nolock) where ValuationDataProviderDataServiceKey = 1 and ValuationHOCValue > 0";
                    DataTable DT = base.GetQueryResults(query);
                    Assert.That(DT.Rows.Count == 1);

                    int key = Convert.ToInt32(DT.Rows[0][0]);
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    valuation = BMTM.GetMappedType<ValuationDiscriminatedSAHLManual>(ValuationDiscriminatedSAHLManual_DAO.Find(key) as ValuationDiscriminatedSAHLManual_DAO);

                    v = (IValuationDiscriminatedSAHLManual)PropertyRepository.CreateEmptyValuation(SAHL.Common.Globals.ValuationDataProviderDataServices.SAHLManualValuation);
                    v.ValuationDate = valuation.ValuationDate;
                    v.ValuationAmount = valuation.ValuationAmount;
                    v.ValuationHOCValue = valuation.ValuationHOCValue;
                    v.ValuationUserID = valuation.ValuationUserID;
                    v.HOCThatchAmount = valuation.HOCThatchAmount;
                    v.HOCConventionalAmount = valuation.HOCConventionalAmount;
                    v.HOCShingleAmount = valuation.HOCShingleAmount;
                    v.Property = valuation.Property;
                    v.Valuator = valuation.Valuator;
                    v.ValuationClassification = valuation.ValuationClassification;
                    v.ValuationEscalationPercentage = valuation.ValuationEscalationPercentage;
                    v.ValuationUserID = "TestUser";
                    v.Data = valuation.Data;
                    v.IsActive = valuation.IsActive;
                    v.ValuationStatus = valuation.ValuationStatus;

                    PropertyRepository.SaveValuation(v);
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.ToString());
                }
            }
        }

        [Test]
        public void AdcheckValuationUpdateHOC()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                // So I broke the query up into stages
                string query = @"select v.ValuationKey as [ValuationKey], o.OfferKey, ml.PropertyKey as [PropertyKey]
	                                    from HOC (nolock) h
	                                    join FinancialService hfs (nolock) on h.FinancialServiceKey = hfs.FinancialServiceKey
	                                    join fin.MortgageLoan ml (nolock) on ml.FinancialServiceKey = hfs.ParentFinancialServiceKey

	                                    join FinancialService fs (nolock) on hfs.ParentFinancialServiceKey = fs.FinancialServiceKey
	                                    join Account acc (nolock) on fs.AccountKey = acc.AccountKey

	                                    join [2am].dbo.Valuation v (nolock) on ml.PropertyKey = v.PropertyKey

										join [2am].dbo.Offer (nolock) o on o.ReservedAccountKey = fs.AccountKey
										and o.OfferStatusKey = 1

	                                    where v.IsActive = 1 and h.HOCInsurerKey = 2 and v.ValuationDataProviderDataServiceKey = 4 and acc.AccountStatusKey = 3
                                        Order by fs.FinancialServiceKey desc";

                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 0)
                    Assert.Ignore("No data available");

                int valuationKey = Convert.ToInt32(DT.Rows[0][0]);
                int applicationKey = Convert.ToInt32(DT.Rows[0][1]);

                PropertyRepository.AdcheckValuationUpdateHOC(valuationKey, applicationKey);
            }
        }

        [Test]
        public void DeleteValuationCottage()
        {
            string sql = @"Select Top 1 ValuationKey
                           From [2AM].[dbo].[ValuationCottage]";

            using (new SessionScope())
            {
                int iValuationKey = 0;
                ParameterCollection parameters = new ParameterCollection();
                Object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), parameters);

                if (o != null)
                {
                    iValuationKey = Convert.ToInt32(o);
                }

                PropertyRepository.DeleteValuationCottage(iValuationKey);
            }
        }

        [Test]
        public void DeleteValuationCombinedThatch()
        {
            string sql = @"Select Top 1 ValuationKey
                           From [2AM].[dbo].[ValuationCombinedThatch]";

            using (new SessionScope())
            {
                int iValuationKey = 0;
                ParameterCollection parameters = new ParameterCollection();
                Object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), parameters);

                if (o != null)
                {
                    iValuationKey = Convert.ToInt32(o);
                }
                PropertyRepository.DeleteValuationCombinedThatch(iValuationKey);
            }
        }

        [Test]
        public void GetPropertyByHOCKeyTest()
        {
            using (new SessionScope())
            {
                string testSQL = @"select top 1 hfs.FinancialServiceKey
                from [2am].[dbo].[HOC] h (nolock)
                inner join [2am].[dbo].[financialService] hfs (nolock)  on h.FinancialServiceKey = hfs.FinancialServiceKey
                inner join [2am].[dbo].[Account] ha (nolock) on ha.AccountKey = hfs.AccountKey
                inner join [2am].[dbo].[Account] pa (nolock) on pa.AccountKey = ha.ParentAccountKey
                inner join [2am].[dbo].[financialService] mfs (nolock) on mfs.AccountKey = pa.AccountKey
                inner join [2am].[fin].[MortgageLoan] ml (nolock) on mfs.FinancialServiceKey = ml.FinancialServiceKey
                inner join [2am].[dbo].[Property] p (nolock)  on p.PropertyKey = ml.PropertyKey";
                int FinancialServiceKey = 0;
                ParameterCollection parameters = new ParameterCollection();
                Object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(testSQL, typeof(GeneralStatus_DAO), parameters);
                if (o != null)
                {
                    FinancialServiceKey = Convert.ToInt32(o);
                }
                IHOCRepository hocRepo = RepositoryFactory.GetRepository<IHOCRepository>();
                IHOC hoc = hocRepo.GetHOCByKey(FinancialServiceKey);
                IProperty property = PropertyRepository.GetPropertyByHOC(hoc);
                Assert.That(property != null);
            }
        }

        [Test]
        public void GetApplicationByHOCAccountKeyTest()
        {
            using (new SessionScope())
            {
                string testSQL = @"SELECT TOP 1 a.AccountKey
                FROM [2AM].[DBO].[Offer] ofr (nolock)
                INNER JOIN [2AM].[DBO].[OfferAccountRelationship] oar (nolock) on ofr.OfferKey = oar.OfferKey
                INNER JOIN [2AM].[DBO].[Account] a (nolock) on oar.AccountKey = a.AccountKey 
                    and a.RRR_ProductKey = 3";
                int HOCAccountKey = 0;
                ParameterCollection parameters = new ParameterCollection();
                Object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(testSQL, typeof(GeneralStatus_DAO), parameters);
                if (o != null)
                {
                    HOCAccountKey = Convert.ToInt32(o);
                }
                IApplication application = PropertyRepository.GetApplicationByHOCAccountKey(HOCAccountKey);
                Assert.That(application != null);
            }
        }

        [Test]
        public void GetPropertyByApplicationKeyTest()
        {
            using (new SessionScope())
            {
                string testSQL = @"SELECT TOP 1 ofr.OfferKey
                FROM [2AM].[DBO].[Offer] ofr (nolock)
                INNER JOIN [2am].[dbo].[OfferMortgageLoan] oml (nolock)  ON oml.OfferKey = ofr.OfferKey
                INNER JOIN [2am].[dbo].[Property] p (nolock) on p.PropertyKey = oml.PropertyKey";
                int ApplicationKey = 0;
                ParameterCollection parameters = new ParameterCollection();
                Object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(testSQL, typeof(GeneralStatus_DAO), parameters);
                if (o != null)
                {
                    ApplicationKey = Convert.ToInt32(o);
                }
                IProperty property = PropertyRepository.GetPropertyByApplicationKey(ApplicationKey);
                Assert.That(property != null);
            }
        }

        [Test]
        public void CreateEmptyProperty()
        {
            using (new SessionScope())
            {
                IProperty IProp = PropertyRepository.CreateEmptyProperty();
                Assert.IsNotNull(IProp);
            }
        }

        [Test]
        public void CreateEmptyPropertyAccessDetails()
        {
            using (new SessionScope())
            {
                IPropertyAccessDetails IPropAccess = PropertyRepository.CreateEmptyPropertyAccessDetails();
                Assert.IsNotNull(IPropAccess);
            }
        }

        [Test]
        public void CreateEmptyPropertyData()
        {
            using (new SessionScope())
            {
                IPropertyData IPropData = PropertyRepository.CreateEmptyPropertyData();
                Assert.IsNotNull(IPropData);
            }
        }

        [Test]
        public void CreateEmptyPropertyTitleDeed()
        {
            using (new SessionScope())
            {
                IPropertyTitleDeed IPropTitleDeed = PropertyRepository.CreateEmptyPropertyTitleDeed();
                Assert.IsNotNull(IPropTitleDeed);
            }
        }

        [Test]
        public void CreateEmptyValuationCombinedThatch()
        {
            using (new SessionScope())
            {
                IValuationCombinedThatch IValuation = PropertyRepository.CreateEmptyValuationCombinedThatch();
                Assert.IsNotNull(IValuation);
            }
        }

        [Test]
        public void CreateEmptyValuationCottage()
        {
            using (new SessionScope())
            {
                IValuationCottage IValuation = PropertyRepository.CreateEmptyValuationCottage();
                Assert.IsNotNull(IValuation);
            }
        }

        [Test]
        public void CreateEmptyValuationImprovement()
        {
            using (new SessionScope())
            {
                IValuationImprovement IValuation = PropertyRepository.CreateEmptyValuationImprovement();
                Assert.IsNotNull(IValuation);
            }
        }

        [Test]
        public void CreateEmptyValuationMainBuilding()
        {
            using (new SessionScope())
            {
                IValuationMainBuilding IValuation = PropertyRepository.CreateEmptyValuationMainBuilding();
                Assert.IsNotNull(IValuation);
            }
        }

        [Test]
        public void CreateEmptyValuationOutbuilding()
        {
            using (new SessionScope())
            {
                IValuationOutbuilding IValuation = PropertyRepository.CreateEmptyValuationOutbuilding();
                Assert.IsNotNull(IValuation);
            }
        }

        [Test]
        public void GetPropertyTitleDeedByTitleDeedNumberTest()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 ptd.TitleDeedNumber, prop.PropertyKey
                from PropertyTitleDeed ptd (nolock)
                join Property prop (nolock)
	                on prop.PropertyKey = ptd.PropertyKey
                where ptd.TitleDeedNumber is not null";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());

                string TitleDeedNumber = ds.Tables[0].Rows[0][0].ToString();
                int PropertyKey = Convert.ToInt32(ds.Tables[0].Rows[0][1]);

                IPropertyTitleDeed propertyTitleDeed = PropertyRepository.GetPropertyTitleDeedByTitleDeedNumber(PropertyKey, TitleDeedNumber);
                Assert.IsNotNull(propertyTitleDeed);
            }
        }

        [Test]
        public void GetDataProviderDataServiceByKeyTest()
        {
            using (new SessionScope())
            {
                DataProviderDataServices dpds = DataProviderDataServices.LightstonePhysicalValuation;
                IDataProviderDataService dataProviderDataService = PropertyRepository.GetDataProviderDataServiceByKey(dpds);
                Assert.IsNotNull(dataProviderDataService);
            }
        }

        [Test]
        public void GetPropertyDataProviderDataServiceByKeyTest()
        {
            using (new SessionScope())
            {
                PropertyDataProviderDataServices pdpds = PropertyDataProviderDataServices.LightstonePropertyIdentification;
                IPropertyDataProviderDataService propertyDataProviderDataService = PropertyRepository.GetPropertyDataProviderDataServiceByKey(pdpds);
                Assert.IsNotNull(propertyDataProviderDataService);
            }
        }

        [Test]
        public void GetValuationDataProviderDataServiceByKeyTest()
        {
            using (new SessionScope())
            {
                ValuationDataProviderDataServices vdpds = ValuationDataProviderDataServices.SAHLManualValuation;
                IValuationDataProviderDataService valuationDataProviderDataService = PropertyRepository.GetValuationDataProviderDataServiceByKey(vdpds);
                Assert.IsNotNull(valuationDataProviderDataService);
            }
        }

        [Test]
        public void SaveValuationWithoutValidationErrorsTest()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                string sql = @" select top 1 val.ValuationKey
                                from [2am].[dbo].OfferMortgageLoan oml (nolock)
                                join [2am].[dbo].Offer ofr (nolock)
                                    on ofr.OfferKey = oml.OfferKey
                                join [2am].[dbo].OfferInformation oi (nolock)
                                    on oi.OfferKey = ofr.OfferKey
                                join [2am].[dbo].Valuation val (nolock)
                                    on oml.PropertyKey = val.PropertyKey
                                where
                                    val.IsActive =1
                                        and
                                    ofr.OfferStatusKey = 1
                                        and
                                    oi.OfferInformationTypeKey <> 3
                                order by newid()";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int valuationKey = Convert.ToInt32(o);
                Valuation_DAO[] val = Valuation_DAO.FindAllByProperty("Key", valuationKey);

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IValuation valuation = BMTM.GetMappedType<IValuation, Valuation_DAO>(val[0]);
                PropertyRepository.SaveValuationWithoutValidationErrors(valuation);
                Assert.IsNotNull(valuation);
            }
        }

        [Test]
        public void SaveValuationTest()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                string sql = @"select top 1 val.valuationKey
                from [2am].[dbo].Account acc (nolock)
                inner join [2am].[dbo].FinancialService fs (nolock) on acc.AccountKey = fs.AccountKey
                inner join [2am].[fin].MortgageLoan ml (nolock) on ml.financialServiceKey = fs.financialServiceKey
                inner join [2am].[dbo].Property prop (nolock) on ml.propertyKey = prop.propertyKey
                inner join [2am].[dbo].Valuation val (nolock) on val.propertyKey = prop.propertyKey
                where acc.accountStatusKey = 1
                and fs.FinancialServiceTypeKey = 1
                and val.isactive = 1
                and val.ValuationDataProviderDataServiceKey = 1
                and val.ValuationHOCValue > 0";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int valuationKey = Convert.ToInt32(o);
                Valuation_DAO[] val = Valuation_DAO.FindAllByProperty("Key", valuationKey);

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IValuation valuation = BMTM.GetMappedType<IValuation, Valuation_DAO>(val[0]);
                PropertyRepository.SaveValuation(valuation);
                Assert.IsNotNull(valuation);
            }
        }

        [Test]
        public void GetSuburbByKeyTest()
        {
            using (new SessionScope())
            {
                Suburb_DAO sub = Suburb_DAO.FindFirst();
                ISuburb suburb = PropertyRepository.GetSuburbByKey(sub.Key);
                Assert.IsNotNull(suburb);
            }
        }

        [Test]
        public void GetValuationByKeyTest()
        {
            using (new SessionScope())
            {
                Valuation_DAO val = Valuation_DAO.FindFirst();
                IValuation valuation = PropertyRepository.GetValuationByKey(val.Key);
                Assert.IsNotNull(valuation);
            }
        }

        [Test]
        public void GetValuationsByPropertyKeyDateSortedTest()
        {
            using (new SessionScope())
            {
                string query = @"select top 1 propertykey, count(propertykey) from Valuation
								group by propertykey
								order by count(ValuationKey) desc";

                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count > 0);

                int key = Convert.ToInt32(DT.Rows[0][0]);
                int count = Convert.ToInt32(DT.Rows[0][1]);

                IEventList<IValuation> valuations = PropertyRepository.GetValuationsByPropertyKeyDateSorted(key);
                Assert.IsNotNull(valuations);
                Assert.IsTrue(valuations.Count > 0);
                Assert.IsTrue(valuations.Count == count);
            }
        }

        [Test]
        public void GetActiveValuationByPropertyKeyTest()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                string sql = @"select top 1 prop.propertyKey
                from property prop (nolock)
                inner join valuation val (nolock)
	                on val.propertyKey = prop.propertyKey
                where val.isactive = 1
                and val.ValuationStatusKey = 2";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int propertyKey = Convert.ToInt32(o);

                IValuation valuation = PropertyRepository.GetActiveValuationByPropertyKey(propertyKey);
                Assert.IsNotNull(valuation);
            }
        }

        [Test]
        public void GetValuatorByDescriptionTest()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 le.RegisteredName
                from valuator val (nolock)
                join LegalEntity le (nolock)
	                on le.LegalEntityKey = val.LegalEntityKey";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                string registeredName = Convert.ToString(o);

                IValuator valuator = PropertyRepository.GetValuatorByDescription(registeredName);
                Assert.IsNotNull(valuator);
            }
        }

        [Test]
        public void GetValuatorByLEKeyTest()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 le.LegalEntityKey
                from valuator val (nolock)
                join LegalEntity le (nolock)
	                on le.LegalEntityKey = val.LegalEntityKey";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int LegalEntityKey = Convert.ToInt32(o);

                IValuator valuator = PropertyRepository.GetValuatorByLEKey(LegalEntityKey);
                Assert.IsNotNull(valuator);
            }
        }

        [Test]
        public void GetActiveValuatorsTest()
        {
            using (new SessionScope())
            {
                IEventList<IValuator> valuators = PropertyRepository.GetActiveValuators();
                Assert.IsNotNull(valuators);
                Assert.IsTrue(valuators.Count > 0);
            }
        }

        [Test]
        public void GetDataProviderKeyFromDataProviderDataServiceTest()
        {
            using (new SessionScope())
            {
                DataProviderDataService_DAO dpds = DataProviderDataService_DAO.FindFirst();
                int testKey = dpds.DataProvider.Key;
                int dataProviderKey = PropertyRepository.GetDataProviderKeyFromDataProviderDataService(dpds.Key);
                Assert.IsTrue(testKey == dataProviderKey);
            }
        }

        [Test]
        public void GetPropertyAccessDetailsByPropertyKeyTest()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 p.PropertyKey
                from [2am].dbo.Property p (nolock)
                join [2am].dbo.PropertyAccessDetails pad (nolock)
                    on p.PropertyKey = pad.PropertyKey";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int propertyKey = Convert.ToInt32(o);
                IReadOnlyEventList<IPropertyAccessDetails> padList = PropertyRepository.GetPropertyAccessDetailsByPropertyKey(propertyKey);
                Assert.IsNotNull(padList);
                Assert.IsTrue(padList.Count > 0);
            }
        }

        [Test]
        public void GetDataSetFromXMLTest()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 v.valuationKey
                from [2am].dbo.Account acc (nolock)
                join [2am].dbo.FinancialService fs (nolock)
                    on acc.AccountKey = fs.AccountKey
                join [2am].[fin].MortgageLoan ml (nolock)
                    on fs.FinancialServiceKey = ml.FinancialServiceKey
                join [2am].dbo.Valuation v (nolock)
                    on ml.PropertyKey = v.PropertyKey
                join [2am].dbo.Account hacc (nolock)
                    on hacc.ParentAccountKey = acc.AccountKey
                join [2am].dbo.FinancialService hfs (nolock)
                    on hfs.AccountKey = hacc.AccountKey
                join [2am].dbo. hoc h (nolock)
                    on hfs.FinancialServiceKey = h.FinancialServiceKey
                where acc.AccountStatusKey = 1 and v.ValuationDataProviderDataServiceKey = 4
                and v.IsActive = 1 and hacc.AccountStatusKey = 1 and h.HOCInsurerKey = 2";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int valuationKey = Convert.ToInt32(o);
                IValuation val = PropertyRepository.GetValuationByKey(valuationKey);
                DataSet ds = PropertyRepository.GetDataSetFromXML(val.Data);
                Assert.IsNotNull(ds);
                Assert.IsNotNull(ds.Tables);
                Assert.IsTrue(ds.Tables.Count > 0);
            }
        }

        [Test]
        public void GetLatestPropertyDataTest()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 p.PropertyKey,pdpds.PropertyDataProviderDataServiceKey
                from [2am].dbo.Property p (nolock)
                join [2am].dbo.PropertyData pd (nolock)
	                on p.PropertyKey = pd.PropertyKey
                join [2am].dbo.PropertyDataProviderDataService pdpds (nolock)
	                on pd.PropertyDataProviderDataServiceKey = pdpds.PropertyDataProviderDataServiceKey";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int propertyKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                string propertyDataProviderDataServiceKey = Convert.ToString(ds.Tables[0].Rows[0][1]);
                IProperty property = PropertyRepository.GetPropertyByKey(propertyKey);
                PropertyDataProviderDataServices pdpds = (PropertyDataProviderDataServices)Enum.Parse(typeof(PropertyDataProviderDataServices), propertyDataProviderDataServiceKey);
                IPropertyData propertyData = PropertyRepository.GetLatestPropertyData(property, pdpds);
                Assert.IsNotNull(propertyData);
            }
        }

        [Test]
        public void AdcheckValuationUpdateHOCTestOL()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                string sql = @"select top 1 v.ValuationKey, ofr.offerKey
                from [2am].dbo.Account acc (nolock)
                join [2am].dbo.Offer ofr (nolock)
	                on ofr.accountKey = acc.accountKey
                join [2am].dbo.FinancialService fs (nolock)
                    on acc.AccountKey = fs.AccountKey
                join [2am].[fin].MortgageLoan ml (nolock)
                    on fs.FinancialServiceKey = ml.FinancialServiceKey
                join [2am].dbo.Valuation v (nolock)
                    on ml.PropertyKey = v.PropertyKey
                join [2am].dbo.Account hacc (nolock)
                    on hacc.ParentAccountKey = acc.AccountKey
                join [2am].dbo.FinancialService hfs (nolock)
                    on hfs.AccountKey = hacc.AccountKey
                join [2am].dbo.hoc h (nolock)
                    on hfs.FinancialServiceKey = h.FinancialServiceKey
                where acc.AccountStatusKey = 3 and v.ValuationDataProviderDataServiceKey = 4
                and v.IsActive = 1 and hacc.AccountStatusKey = 3 and h.HOCInsurerKey = 2
                and ofr.offerStatusKey = 1";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int valuationKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    int offerKey = Convert.ToInt32(ds.Tables[0].Rows[0][1]);
                    PropertyRepository.AdcheckValuationUpdateHOC(valuationKey, offerKey);
                    Assert.IsTrue(true);
                }
                else
                {
                    //TODO: NODATATEST
                    //Assert.Fail("There are no valid keys for this test");
                }
            }
        }

        [Test]
        public void GetPropertyAccesDetailsByPropertyKeyTest()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 p.PropertyKey
                from [2am].dbo.Property p (nolock)
                join [2am].dbo.PropertyAccessDetails pad (nolock)
	                on p.propertyKey = pad.propertyKey";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int propertyKey = Convert.ToInt32(o);
                IEventList<IPropertyAccessDetails> propList = PropertyRepository.GetPropertyAccesDetailsByPropertyKey(propertyKey);
                Assert.IsNotNull(propList);
                Assert.IsTrue(propList.Count > 0);
            }
        }

        [Test]
        public void GetValuatorDataFromXMLHistoryTest()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 genericKey
                from [2am].dbo.XMLHistory (nolock)
                where genericKeyTypeKey = 2
                and convert(nvarchar(max),xmldata) like '%AdCheck_RequestValuation_Request%'";
                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int genericKey = Convert.ToInt32(o);
                DataTable dt = PropertyRepository.GetValuatorDataFromXMLHistory((int)GenericKeyTypes.Offer, genericKey, "AdCheck");
                Assert.IsNotNull(dt);
                Assert.IsTrue(dt.Rows.Count > 0);
            }
        }

        [Test]
        public void BuildSAHLPropertyDataXMLTest()
        {
            using (new SessionScope())
            {
                string bondAccountNumber = "1";
                int deedsOfficeKey = 1;
                string data = PropertyRepository.BuildSAHLPropertyDataXML(bondAccountNumber, deedsOfficeKey);
                Assert.IsNotNull(data);
                Assert.IsTrue(data.Length > 0);
            }
        }

        [Test]
        public void CalculateCombinedThatchValueTest()
        {
            using (new SessionScope())
            {
                IValuationMainBuilding valMainBuilding = _mockery.StrictMock<IValuationMainBuilding>();
                IValuationCottage valCottage = _mockery.StrictMock<IValuationCottage>();
                IValuationOutbuilding valOutBuilding = _mockery.StrictMock<IValuationOutbuilding>();
                IEventList<IValuationOutbuilding> valOutBuildings = new EventList<IValuationOutbuilding>();
                IValuationRoofType valRoofType = _mockery.StrictMock<IValuationRoofType>();

                //
                SetupResult.For(valRoofType.Key).Return((int)ValuationRoofTypes.Thatch);

                //
                SetupResult.For(valMainBuilding.ValuationRoofType).Return(valRoofType);
                SetupResult.For(valCottage.ValuationRoofType).Return(valRoofType);
                SetupResult.For(valOutBuilding.ValuationRoofType).Return(valRoofType);

                //
                SetupResult.For(valMainBuilding.Rate).Return(1);
                SetupResult.For(valMainBuilding.Extent).Return(1);

                //
                SetupResult.For(valCottage.Rate).Return(1);
                SetupResult.For(valCottage.Extent).Return(1);

                //
                SetupResult.For(valOutBuilding.Rate).Return(1);
                SetupResult.For(valOutBuilding.Extent).Return(1);

                //
                valOutBuildings.Add(null, valOutBuilding);

                //
                _mockery.ReplayAll();

                //
                double val = PropertyRepository.CalculateCombinedThatchValue(valMainBuilding, valCottage, valOutBuildings);
                Assert.IsTrue(val > 0);
            }
        }

        [Test]
        public void CheckForAutomatedValuationsTest()
        {
            using (new SessionScope())
            {
                int propertyKey = 0;
                int testKey = PropertyRepository.CheckForAutomatedValuations(propertyKey);
                Assert.IsTrue(testKey == 0);
            }
        }

        [Test]
        public void HasAdCheckPropertyIDTest()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 p.PropertyKey
                from [2am].dbo.Property p (nolock)
                join [2am].dbo.PropertyData pd (nolock)
	                on p.PropertyKey = pd.PropertyKey
                join [2am].dbo.PropertyDataProviderDataService pdpds (nolock)
	                on pd.PropertyDataProviderDataServiceKey = pdpds.PropertyDataProviderDataServiceKey
                join [2am].dbo.DataProviderDataService dpds (nolock)
	                on pdpds.DataProviderDataServiceKey = dpds.DataProviderDataServiceKey
                join [2am].dbo.DataProvider dp (nolock)
	                on dpds.DataProviderKey = dp.DataProviderKey
                where dp.DataProviderKey = 3";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int propertyKey = Convert.ToInt32(o);
                bool test = PropertyRepository.HasAdCheckPropertyID(propertyKey);
                Assert.IsTrue(test);
            }
        }

        [Test]
        public void HasLightStonePropertyIDTest()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 p.PropertyKey
                from [2am].dbo.Property p (nolock)
                join [2am].dbo.PropertyData pd (nolock)
	                on p.PropertyKey = pd.PropertyKey
                join [2am].dbo.PropertyDataProviderDataService pdpds (nolock)
	                on pd.PropertyDataProviderDataServiceKey = pdpds.PropertyDataProviderDataServiceKey
                join [2am].dbo.DataProviderDataService dpds (nolock)
	                on pdpds.DataProviderDataServiceKey = dpds.DataProviderDataServiceKey
                join [2am].dbo.DataProvider dp (nolock)
	                on dpds.DataProviderKey = dp.DataProviderKey
                where dp.DataProviderKey = 2";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int propertyKey = Convert.ToInt32(o);
                bool test = PropertyRepository.HasLightStonePropertyID(propertyKey);
                Assert.IsTrue(test);
            }
        }

        [Test]
        public void SaveAddressTest()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 p.propertyKey
                from [2am].dbo.Property p (nolock)
                join [2am].dbo.address ad (nolock)
	                on p.addressKey = ad.addressKey";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int propertyKey = Convert.ToInt32(o);

                IProperty property = PropertyRepository.GetPropertyByKey(propertyKey);
                IAddress address = property.Address;

                PropertyRepository.SaveAddress(property, address);
                Assert.IsTrue(true);
            }
        }

        [Test]
        public void GetActiveValuatorsFilteredTest()
        {
            using (new SessionScope())
            {
                IEventList<IValuator> valuators = PropertyRepository.GetActiveValuatorsFiltered((int)GeneralStatuses.Active);
                Assert.IsTrue(valuators.Count > 0);
            }
        }

        [Test]
        public void GetPropertyKeyByOfferKeyTest()
        {
            using (new SessionScope(FlushAction.Never))
            {
                Int32 propkey;

                string sql = @"select top 1 OfferKey, PropertyKey from OfferMortgageLoan (nolock) where PropertyKey is not null";
                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    propkey = PropertyRepository.GetPropertyKeyByOfferKey(Convert.ToInt32(ds.Tables[0].Rows[0][0]));

                    Assert.AreEqual(propkey, Convert.ToInt32(ds.Tables[0].Rows[0][1]));
                }
            }
        }

        [Test]
        public void LightStoneValuationDoneWithinLast2Months()
        {
            using (new SessionScope())
            {
                bool res = PropertyRepository.LightStoneValuationDoneWithinLast2Months(-1);
                Assert.IsFalse(res);

                //make sure there is some test data....
                string sql = @"update Valuation
                    set ValuationDate = GetDate() - 2
                    where ValuationKey in
                    (
	                    select top 1 ValuationKey from Valuation where ValuationDataProviderDataServiceKey = 3
	                    order by 1
                    )";

                CastleTransactionsServiceHelper.ExecuteNonQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

                //Get a ValuationDiscriminatedLightstoneAVM_DAO less than 60 days previous
                sql = @"select PropertyKey from Valuation v
                    where ValuationDate > GetDate()-60
                    and ValuationDataProviderDataServiceKey = 3
                    and PropertyKey is not null";

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

                if (obj != null)
                {
                    res = PropertyRepository.LightStoneValuationDoneWithinLast2Months(Convert.ToInt32(obj));
                    Assert.IsTrue(res);
                }

                //Get a ValuationDiscriminatedLightstoneAVM_DAO older than 60 days
                sql = @"select top 1 PropertyKey from Valuation v
                    where ValuationDataProviderDataServiceKey = 3
                    and PropertyKey is not null
                    Group by PropertyKey
                    Having  MAX(ValuationDate) < GetDate()-60";

                obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

                if (obj != null)
                {
                    res = PropertyRepository.LightStoneValuationDoneWithinLast2Months(Convert.ToInt32(obj));
                    Assert.IsFalse(res);
                }
            }
        }

        [Test]
        public void SavePropertyData()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                IPropertyData busObj = new PropertyData(PropertyData_DAO.FindFirst());
                PropertyRepository.SavePropertyData(busObj);
            }
        }

        [Test]
        public void SavePropertyTitleDeed()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                IPropertyTitleDeed busObj = new PropertyTitleDeed(PropertyTitleDeed_DAO.FindFirst());
                PropertyRepository.SavePropertyTitleDeed(busObj);
            }
        }

        [Test]
        public void TestGetValuationsAndReasons()
        {
            using (new SessionScope())
            {
                var sql = "select top 1 PropertyKey from Valuation order by ValuationDate desc";
                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), null);
                int propertyKey = Convert.ToInt32(obj);
                DataTable data = PropertyRepository.GetValuationsAndReasons(propertyKey);
                if (data.Rows.Count == 0)
                {
                    Assert.Ignore("No data to perform test: TestGetValuationsAndReasons");
                }

                sql = string.Format(@"select v.ValuationKey
                        from [2AM].dbo.Valuation v
                        left join [2AM].dbo.Reason r (nolock) on r.GenericKey = v.ValuationKey
                        left join [2AM].dbo.ReasonDefinition rd (nolock) on r.ReasonDefinitionKey = rd.ReasonDefinitionKey
                        left join [2AM].dbo.ReasonType rt (nolock) on rd.ReasonTypeKey = rt.ReasonTypeKey and rt.ReasonTypeGroupKey = 19
                        where PropertyKey = {0}", propertyKey);

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

                Assert.AreEqual(ds.Tables[0].Rows.Count, data.Rows.Count);
            }
        }

        [Test]
        public void TestGetComcorpOfferPropertyDetails_success()
        {
            using (new SessionScope(FlushAction.Never))
            {
                var sql = @"select top 1 OfferKey from [2am]..ComcorpOfferPropertyDetails";

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

                if (obj == null)
                    Assert.Ignore("No data to perform test: TestGetComcorpOfferPropertyDetails");

                int OfferKey;
                IComcorpOfferPropertyDetails comcorpOfferPropertyDetails = null;

                if (int.TryParse(obj.ToString(), out OfferKey))
                {
                    comcorpOfferPropertyDetails = PropertyRepository.GetComcorpOfferPropertyDetails(OfferKey);
                }

                Assert.IsNotNull(comcorpOfferPropertyDetails);
            }
        }

        [Test]
        public void TestGetComcorpOfferPropertyDetails_fail()
        {
            IComcorpOfferPropertyDetails comcorpOfferPropertyDetails = PropertyRepository.GetComcorpOfferPropertyDetails(-1);
            Assert.IsNull(comcorpOfferPropertyDetails);
        }
    }
}