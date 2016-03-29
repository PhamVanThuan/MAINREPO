using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Attributes;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using Castle.ActiveRecord;
using SAHL.Common.Globals;
using Castle.ActiveRecord.Queries;
using SAHL.Common.Factories;
using SAHL.Common.Service.Interfaces;
using Castle.ActiveRecord.Framework.Internal;
using System.Collections;
using System.Reflection;
using System.Collections.ObjectModel;
using SAHL.Common.BusinessModel.Interfaces.ReadOnly;
using SAHL.Common.BusinessModel.ReadOnly;
using NHibernate.Criterion;
using System.Linq;


namespace SAHL.Common.BusinessModel.Repositories
{

    /// <summary>
    /// Repository containing cached data.  All properties are by default not loaded, and get loaded once accessed.  Once 
    /// loaded, the data will not get retrieved from the database again unless it is cleared out using the 
    /// <see cref="ResetLookup(LookupKeys)"/> or the <see cref="ResetAll()"/> methods.
    /// <para>
    /// <strong>Developers:</strong> 
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///         Do not cache ActiveRecord or ActiveRecord-based objects - there are very few cases 
    ///         where this is necessary (for example the RuleItems which are loaded by the RuleService)
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         In almost all cases a dictionary will suffice (an ISAHLDictionary) can be used, or if 
    ///         more than a key/description pair is required then use a basic read-only object (e.g. 
    ///         ICountryReadOnly).
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         When adding dependent objects, use internal string keys for the central dictionary - 
    ///         this is so that the ResetAll() method never needs to be changed - all it will entail 
    ///         is clearing the central dictionary.  Note, however, that dependent objects may still 
    ///         need to be catered for in the ResetLookup method.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         Always use <see cref="AddLookup(LookupKeys, object)"/> instead of manually adding to the 
    ///         lookup dictionary - this minimises the risk of concurrency issues and also is necessary 
    ///         for dependent objects.
    ///         </description>
    ///     </item>
    /// </list>
    /// </para>
    /// </summary>
    [FactoryType(typeof(ILookupRepository), LifeTime = FactoryTypeLifeTime.Singleton)]
    public class LookupRepository : ILookupRepository
    {

        #region Private Attributes

        // this variable stores ALL cached lookups against a key - this is so we can clear a cached item via 
        // a key instead of a separate method for each cached item
        private Dictionary<LookupKeys, object> _dictLookups = new Dictionary<LookupKeys, object>();

        // the following are collections that are dependent on other lookups (e.g. LanguagesTranslatable is dependent 
        // on Languages) and do not have a key of their own
        private IDictionary<string, IRuleItem> _ruleItemsByName;
        private List<ILanguageReadOnly> _languagesTranslatable = new List<ILanguageReadOnly>();
        private IDictionary<int, IEventList<IPriority>> _prioritiesByOSP = new Dictionary<int, IEventList<IPriority>>();
        private IDictionary<int, IDictionary<int, string>> _provincesByCountry = new Dictionary<int, IDictionary<int, string>>();

        #endregion

        #region Private Methods

        /// <summary>
        /// Helper method to add an item to the _dictLookups dictionary - this will check for 
        /// key existence before adding in case a lookup item is accessed simultaneously
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        private void AddLookup(LookupKeys key, object obj)
        {
            if (_dictLookups.ContainsKey(key))
                _dictLookups[key] = obj;
            else
                _dictLookups.Add(key, obj);
        }


        /// <summary>
        /// Loads a full list of objects from a database table.
        /// </summary>
        /// <typeparam name="TInterface">The interface type e.g. IAccount.</typeparam>
        /// <typeparam name="TBusinessModel">The businessmodel type e.g. Account.</typeparam>
        /// <typeparam name="TDAO">The DAO type e.g. Account_DAO.</typeparam>
        /// <param name="lookupKey">The unique key for the lookup.</param>
        /// <param name="keyProperty">The property that is the unique identifier for the lookup e.g. Key.</param>
        /// <param name="textProperty">The property that is used for display e.g. Description.</param>
        /// <returns>A list of all objects.</returns>
        private IEventList<TInterface> LoadAll<TInterface, TBusinessModel, TDAO>(LookupKeys lookupKey, string keyProperty, string textProperty) where TDAO : class
        {
            lock (syncObject)
            {
                if (!_dictLookups.ContainsKey(lookupKey))
                {
                    List<TDAO> list = new List<TDAO>();

                    ActiveRecordModel arm = ActiveRecordModel.GetModel(typeof(TDAO));
                    TDAO[] dao = null;
                    if (arm.ActiveRecordAtt.Lazy)
                    {
                        DetachedCriteria DC = DetachedCriteria.For<TDAO>();

                        //IList<BelongsToModel> listy = arm.BelongsTo;
                        foreach (BelongsToModel am in arm.BelongsTo)
                        {
                            DC.SetFetchMode(am.Property.Name, NHibernate.FetchMode.Eager);
                        }

                        //IList<HasAndBelongsToManyModel> listy1 = arm.HasAndBelongsToMany;
                        foreach (HasAndBelongsToManyModel am in arm.HasAndBelongsToMany)
                        {
                            if (am.HasManyAtt.Lazy == false)
                                DC.SetFetchMode(am.Property.Name, NHibernate.FetchMode.Eager);
                        }

                        //listy2 = arm.HasMany;
                        foreach (HasManyModel am in arm.HasMany)
                        {
                            if (am.HasManyAtt.Lazy == false)
                                DC.SetFetchMode(am.Property.Name, NHibernate.FetchMode.Eager);
                        }

                        //listy3 = arm.OneToOnes;
                        foreach (OneToOneModel am in arm.OneToOnes)
                        {
                            DC.SetFetchMode(am.Property.Name, NHibernate.FetchMode.Eager);
                        }

                        dao = ActiveRecordMediator<TDAO>.FindAll(DC);
                    }
                    else
                    {
                        dao = ActiveRecordMediator<TDAO>.FindAll();
                    }
                    list.AddRange(dao);
                    AddLookup(lookupKey, new DAOEventList<TDAO, TInterface, TBusinessModel>(list, keyProperty, textProperty));
                }
            }
            return (IEventList<TInterface>)_dictLookups[lookupKey];
        }

        /// <summary>
        /// Loads a dictionary of key/value pairs for a lookup.
        /// </summary>
        /// <typeparam name="TDAO">The DAO type to load.</typeparam>
        /// <typeparam name="TKEY">The type of the key, e.g. int.</typeparam>
        /// <typeparam name="TVALUE">The type of the value e.g. string</typeparam>
        /// <param name="lookupKey">The enumeration member for the lookup to load.</param>
        /// <param name="keyField">The primary key property of the entity, e.g. "Key".</param>
        /// <param name="valueField">The value property e.g. "Description".</param>
        /// <returns></returns>
        private ISAHLDictionary<TKEY, TVALUE> LoadDictionary<TDAO, TKEY, TVALUE>(LookupKeys lookupKey, string keyField, string valueField) where TDAO : class
        {
            if (!_dictLookups.ContainsKey(lookupKey))
            {
                ISAHLDictionary<TKEY, TVALUE> d = new SAHLDictionary<TKEY, TVALUE>();
                Type daoType = typeof(TDAO);

                ActiveRecordModel arm = ActiveRecordModel.GetModel(daoType);
                TDAO[] daoObjects = null;

                if (arm.ActiveRecordAtt.Lazy)
                {
                    DetachedCriteria dc = DetachedCriteria.For<TDAO>();
                    dc.SetFetchMode(keyField, NHibernate.FetchMode.Eager);
                    dc.SetFetchMode(valueField, NHibernate.FetchMode.Eager);
                    daoObjects = ActiveRecordMediator<TDAO>.FindAll(dc);
                }
                else
                {
                    daoObjects = ActiveRecordMediator<TDAO>.FindAll();
                }

                foreach (object obj in daoObjects)
                {
                    PropertyInfo piKey = daoType.GetProperty(keyField);
                    PropertyInfo piValue = daoType.GetProperty(valueField);
                    d.Add((TKEY)piKey.GetValue(obj, null), (TVALUE)piValue.GetValue(obj, null));
                }
                AddLookup(lookupKey, d);
            }
            return (ISAHLDictionary<TKEY, TVALUE>)_dictLookups[lookupKey];
        }

        #region fxcop

        // Loads a list of items for a lookup.
        //private IList<TKEY> LoadList<TDAO, TKEY>(LookupKeys lookupKey, string keyField) where TDAO : class
        //{
        //    if (!_dictLookups.ContainsKey(lookupKey))
        //    {
        //        IList<TKEY> list = new List<TKEY>();
        //        Type daoType = typeof(TDAO);

        //        ActiveRecordModel arm = ActiveRecordModel.GetModel(daoType);
        //        TDAO[] daoObjects = null;

        //        if (arm.ActiveRecordAtt.Lazy)
        //        {
        //            DetachedCriteria dc = DetachedCriteria.For<TDAO>();
        //            dc.SetFetchMode(keyField, NHibernate.FetchMode.Eager);
        //            daoObjects = ActiveRecordMediator<TDAO>.FindAll(dc);
        //        }
        //        else
        //        {
        //            daoObjects = ActiveRecordMediator<TDAO>.FindAll();
        //        }

        //        foreach (object obj in daoObjects)
        //        {
        //            PropertyInfo piKey = daoType.GetProperty(keyField);
        //            list.Add((TKEY)piKey.GetValue(obj, null));
        //        }
        //        AddLookup(lookupKey, list);
        //    }
        //    return ((List<TKEY>)_dictLookups[lookupKey]).AsReadOnly();
        //}
        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// This will remove a lookup item from memory, forcing it to be reloaded the next time it is accessed.
        /// </summary>
        /// <param name="key">One of the keys in <see cref="LookupKeys"/></param>
        public void ResetLookup(LookupKeys key)
        {
            if (_dictLookups.ContainsKey(key))
                _dictLookups.Remove(key);

            // clear any dependent objects if necessary
            switch (key)
            {
                case LookupKeys.Countries:
                    _provincesByCountry.Clear();
                    break;
                case LookupKeys.Languages:
                    _languagesTranslatable.Clear();
                    break;
                case LookupKeys.Priorities:
                    _prioritiesByOSP.Clear();
                    break;
            }

        }

        /// <summary>
        /// Removes all lookup items from memory, forcing them to be reloaded the next time they are accessed.
        /// </summary>
        public void ResetAll()
        {
            _dictLookups.Clear();
            _prioritiesByOSP.Clear();
            _provincesByCountry.Clear();
            _languagesTranslatable.Clear();
        }
        #endregion

        /// <summary>
        /// Gets a dictionary of AccountStatus Key/Description values.
        /// </summary>
        public IDictionary<int, string> AccountStatuses
        {
            get
            {
                return LoadDictionary<AccountStatus_DAO, int, string>(LookupKeys.AccountStatuses, "Key", "Description");
            }
        }

        /// <summary>
        /// Gets a dictionary containing all <see cref="IAddressFormat"/> Key/Description values.
        /// </summary>
        /// <returns></returns>
        public ISAHLDictionary<int, string> AddressFormats
        {
            get
            {
                return LoadDictionary<AddressFormat_DAO, int, string>(LookupKeys.AddressFormats, "Key", "Description");
            }
        }

        /// <summary>
        /// Gets a list of all ADUserGroups in the FeatureGroup table.
        /// </summary>
        public string[] ADUserGroups
        {
            get
            {
                LookupKeys lookupKey = LookupKeys.ADUserGroups;

                if (!_dictLookups.ContainsKey(lookupKey))
                {
                    AddLookup(lookupKey, FeatureGroup.FindAllGroups());
                }
                return (string[])_dictLookups[lookupKey];
            }
        }

        /// <summary>
        /// Returns a dictionary containing a list of address format values for a specified 
        ///<see cref="IAddressType"/>.
        /// </summary>
        /// <param name="addressType"></param>
        /// <returns></returns>
        public ISAHLDictionary<int, string> AddressFormatsByAddressType(AddressTypes addressType)
        {
            ISAHLDictionary<int, string> dict = new SAHLDictionary<int, string>();
            if (addressType == SAHL.Common.Globals.AddressTypes.Residential)
            {
                dict.Add(((int)SAHL.Common.Globals.AddressFormats.Street), AddressFormats[(int)SAHL.Common.Globals.AddressFormats.Street]);
                dict.Add(((int)SAHL.Common.Globals.AddressFormats.FreeText), AddressFormats[(int)SAHL.Common.Globals.AddressFormats.FreeText]);
            }
            else
            {
                dict.Add(((int)SAHL.Common.Globals.AddressFormats.Box), AddressFormats[(int)SAHL.Common.Globals.AddressFormats.Box]);
                dict.Add(((int)SAHL.Common.Globals.AddressFormats.ClusterBox), AddressFormats[(int)SAHL.Common.Globals.AddressFormats.ClusterBox]);
                dict.Add(((int)SAHL.Common.Globals.AddressFormats.PostNetSuite), AddressFormats[(int)SAHL.Common.Globals.AddressFormats.PostNetSuite]);
                dict.Add(((int)SAHL.Common.Globals.AddressFormats.PrivateBag), AddressFormats[(int)SAHL.Common.Globals.AddressFormats.PrivateBag]);
                dict.Add(((int)SAHL.Common.Globals.AddressFormats.Street), AddressFormats[(int)SAHL.Common.Globals.AddressFormats.Street]);
            }
            return dict;
        }

        /// <summary>
        /// Gets a dictionary containing all <see cref="IAddressType"/> Key/Description values.
        /// </summary>
        public IDictionary<int, string> AddressTypes
        {
            get
            {
                return LoadDictionary<AddressType_DAO, int, string>(LookupKeys.AddressTypes, "Key", "Description");
            }
        }

        public IEventList<IApplicationDeclarationAnswer> ApplicationDeclarationAnswers
        {
            get
            {
                return LoadAll<IApplicationDeclarationAnswer, ApplicationDeclarationAnswer, ApplicationDeclarationAnswer_DAO>(LookupKeys.ApplicationDeclarationAnswers, "Key", "Description");
            }
        }

        public IEventList<IAreaClassification> AreaClassifications
        {
            get
            {
                return LoadAll<IAreaClassification, AreaClassification, AreaClassification_DAO>(LookupKeys.AreaClassifications, "Key", "Description");
            }
        }

        public IEventList<IAssetLiabilityType> AssetLiabilityTypes
        {
            get
            {
                return LoadAll<IAssetLiabilityType, AssetLiabilityType, AssetLiabilityType_DAO>(LookupKeys.AssetLiabilityTypes, "Key", "Description");
            }
        }

        public IEventList<IAssetLiabilitySubType> AssetLiabilitySubTypes
        {
            get
            {
                return LoadAll<IAssetLiabilitySubType, AssetLiabilitySubType, AssetLiabilitySubType_DAO>(LookupKeys.AssetLiabilitySubTypes, "Key", "Description");
            }
        }

        public IEventList<ICBOMenu> CBOMenus
        {
            get
            {
                if (!_dictLookups.ContainsKey(LookupKeys.CBOMenus))
                {
                    List<CBOMenu_DAO> list = new List<CBOMenu_DAO>();
                    CBOMenu_DAO[] dao = CBOMenu_DAO.FindAll();
                    list.AddRange(dao);
                    list.Sort(
                      delegate(CBOMenu_DAO c1, CBOMenu_DAO c2)
                      {
                          return c1.Sequence.CompareTo(c2.Sequence);
                      });
                    AddLookup(LookupKeys.CBOMenus, new DAOEventList<CBOMenu_DAO, ICBOMenu, CBOMenu>(list, "Key", "Description"));
                }
                return (IEventList<ICBOMenu>)_dictLookups[LookupKeys.CBOMenus];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEventList<IAffordabilityType> AffordabilityTypes
        {
            get
            {
                return LoadAll<IAffordabilityType, AffordabilityType, AffordabilityType_DAO>(LookupKeys.AffordabilityTypes, "Key", "Description");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEventList<IApplicationType> ApplicationTypes
        {
            get
            {
                return LoadAll<IApplicationType, ApplicationType, ApplicationType_DAO>(LookupKeys.ApplicationTypes, "Key", "Description");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public IEventList<IApplicantType> ApplicantTypes
        {
            get
            {
                return LoadAll<IApplicantType, ApplicantType, ApplicantType_DAO>(LookupKeys.ApplicantTypes, "Key", "Description");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEventList<IApplicationSource> ApplicationSources
        {
            get
            {
                return LoadAll<IApplicationSource, ApplicationSource, ApplicationSource_DAO>(LookupKeys.ApplicationSources, "Key", "Description");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEventList<IApplicationAttributeType> ApplicationAttributesTypes
        {
            get
            {
                return LoadAll<IApplicationAttributeType, ApplicationAttributeType, ApplicationAttributeType_DAO>(LookupKeys.ApplicationAttributesTypes, "Key", "Description");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public IEventList<IApplicationRoleAttributeType> ApplicationRoleAttributesTypes
        {
            get
            {
                return LoadAll<IApplicationRoleAttributeType, ApplicationRoleAttributeType, ApplicationRoleAttributeType_DAO>(LookupKeys.ApplicationRoleAttributesTypes, "Key", "Description");
            }

        }

        /// <summary>
        /// Gets a dictionary of application role types.
        /// </summary>
        public IDictionary<int, string> ApplicationRoleTypes
        {
            get
            {
                return LoadDictionary<ApplicationRoleType_DAO, int, string>(LookupKeys.ApplicationRoleTypes, "Key", "Description");
            }
        }

        public IEventList<IContextMenu> ContextMenus
        {
            get
            {
                return LoadAll<IContextMenu, ContextMenu, ContextMenu_DAO>(LookupKeys.ContextMenus, "Key", "Description");
            }
        }

        public IEventList<IApplicationStatus> ApplicationStatuses
        {
            get
            {
                return LoadAll<IApplicationStatus, ApplicationStatus, ApplicationStatus_DAO>(LookupKeys.ApplicationStatuses, "Key", "Description");
            }
        }

        public IEventList<IAttorney> Attorneys
        {
            get
            {
                return LoadAll<IAttorney, Attorney, Attorney_DAO>(LookupKeys.Attorneys, "Key", "Key");
            }
        }


        public IEventList<IACBType> BankAccountTypes
        {
            get
            {
                return LoadAll<IACBType, ACBType, ACBType_DAO>(LookupKeys.BankAccountTypes, "Key", "ACBTypeDescription");
            }
        }

        public IEventList<IACBBranch> BankBranches
        {
            get
            {
                return LoadAll<IACBBranch, ACBBranch, ACBBranch_DAO>(LookupKeys.BankBranches, "Key", "ACBBranchDescription");
            }
        }

        public IEventList<IACBBank> Banks
        {
            get
            {
                return LoadAll<IACBBank, ACBBank, ACBBank_DAO>(LookupKeys.Banks, "Key", "ACBBankDescription");
            }
        }

        public IEventList<IBatchTransactionStatus> BatchTransactionStatuses
        {
            get
            {
                return LoadAll<IBatchTransactionStatus, BatchTransactionStatus, BatchTransactionStatus_DAO>(LookupKeys.BatchTransactionStatuses, "Key", "Description");
            }
        }

        public IEventList<IBulkBatchStatus> BulkBatchStatuses
        {
            get
            {
                return LoadAll<IBulkBatchStatus, BulkBatchStatus, BulkBatchStatus_DAO>(LookupKeys.BulkBatchStatuses, "Key", "Description");
            }
        }

        public IEventList<IBulkBatchType> BulkBatchTypes
        {
            get
            {
                return LoadAll<IBulkBatchType, BulkBatchType, BulkBatchType_DAO>(LookupKeys.BulkBatchTypes, "Key", "Description");
            }
        }

        public IEventList<ICancellationType> CancellationTypes
        {
            get
            {
                return LoadAll<ICancellationType, CancellationType, CancellationType_DAO>(LookupKeys.CancellationTypes, "Key", "Description");
            }
        }

        public IEventList<ICapStatus> CapStatuses
        {
            get
            {
                return LoadAll<ICapStatus, CapStatus, CapStatus_DAO>(LookupKeys.CapStatuses, "Key", "Description");
            }
        }

        public IDictionary<int, IDictionary<int, string>> CBOInputGenericTypes
        {
            get
            {
                if (!_dictLookups.ContainsKey(LookupKeys.CBOInputGenericTypes))
                {
                    IDictionary<int, IDictionary<int, string>> cboInputGenericTypes = new Dictionary<int, IDictionary<int, string>>();
                    InputGenericType_DAO[] IGTs = InputGenericType_DAO.FindAll();
                    for (int i = 0; i < IGTs.Length; i++)
                    {
                        if (!cboInputGenericTypes.ContainsKey(IGTs[i].CoreBusinessObjectMenu.Key))
                        {
                            cboInputGenericTypes.Add(IGTs[i].CoreBusinessObjectMenu.Key, new Dictionary<int, string>());
                        }
                        IDictionary<int, string> igts = cboInputGenericTypes[IGTs[i].CoreBusinessObjectMenu.Key];
                        if (!igts.ContainsKey(IGTs[i].GenericKeyTypeParameter.GenericKeyType.Key))
                            igts.Add(IGTs[i].GenericKeyTypeParameter.GenericKeyType.Key, IGTs[i].GenericKeyTypeParameter.ParameterName);
                    }

                    AddLookup(LookupKeys.CBOInputGenericTypes, cboInputGenericTypes);
                }
                return (IDictionary<int, IDictionary<int, string>>)_dictLookups[LookupKeys.CBOInputGenericTypes];
            }
        }

        public IEventList<ICitizenType> CitizenTypes
        {
            get
            {
                return LoadAll<ICitizenType, CitizenType, CitizenType_DAO>(LookupKeys.CitizenTypes, "Key", "Description");
            }
        }

        public IEventList<IDisbursementType> DisbursementTypes
        {
            get
            {
                return LoadAll<IDisbursementType, DisbursementType, DisbursementType_DAO>(LookupKeys.DisbursementTypes, "Key", "Description");
            }
        }

        public IEventList<IExpenseType> ExpenseTypes
        {
            get
            {
                return LoadAll<IExpenseType, ExpenseType, ExpenseType_DAO>(LookupKeys.ExpenseTypes, "Key", "Description");
            }
        }

        public IEventList<IParameterType> ParameterTypes
        {
            get
            {
                return LoadAll<IParameterType, ParameterType, ParameterType_DAO>(LookupKeys.ParameterTypes, "Key", "CSharpDataType");
            }
        }

        public IEventList<IReportParameterType> ReportParameterTypes
        {
            get
            {
                return LoadAll<IReportParameterType, ReportParameterType, ReportParameterType_DAO>(LookupKeys.ReportParameterTypes, "Key", "Description");
            }
        }

        public IEventList<IControl> Controls
        {
            get
            {
                return LoadAll<IControl, Control, Control_DAO>(LookupKeys.Controls, "ControlDescription", "Key");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEventList<ICorrespondenceMedium> CorrespondenceMediums
        {
            get
            {
                return LoadAll<ICorrespondenceMedium, CorrespondenceMedium, CorrespondenceMedium_DAO>(LookupKeys.CorrespondenceMediums, "Key", "Description");
            }
        }

        /// <summary>
        /// Gets a list of countries.
        /// </summary>
        public IDictionary<int, ICountryReadOnly> Countries
        {
            get
            {
                if (!_dictLookups.ContainsKey(LookupKeys.Countries))
                {
                    Country_DAO[] countries = Country_DAO.FindAll();
                    Dictionary<int, ICountryReadOnly> dict = new Dictionary<int, ICountryReadOnly>();

                    foreach (Country_DAO c in countries)
                    {
                        dict.Add(c.Key, new CountryReadOnly(c.Key, c.Description, c.AllowFreeTextFormat));
                    }

                    _dictLookups.Add(LookupKeys.Countries, dict);
                }

                return (IDictionary<int, ICountryReadOnly>)_dictLookups[LookupKeys.Countries];
            }
        }

        /// <summary>
        /// Returns an IEventList of DebitOrderDays
        /// </summary>
        public IEventList<IDebitOrderDay> DebitOrderDays
        {
            get
            {
                return LoadAll<IDebitOrderDay, DebitOrderDay, DebitOrderDay_DAO>(LookupKeys.DebitOrderDays, "Key", "Day");
            }
        }

        /// <summary>
        /// Returns an IEventList of DeedsOffices
        /// </summary>
        public IEventList<IDeedsOffice> DeedsOffice
        {
            get
            {
                return LoadAll<IDeedsOffice, SAHL.Common.BusinessModel.DeedsOffice, DeedsOffice_DAO>(LookupKeys.DeedsOffice, "Key", "Description");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEventList<IDeedsPropertyType> DeedsPropertyTypes
        {
            get
            {
                return LoadAll<IDeedsPropertyType, DeedsPropertyType, DeedsPropertyType_DAO>(LookupKeys.DeedsPropertyTypes, "Key", "Description");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEventList<IDetailClass> DetailClasses
        {
            get
            {
                return LoadAll<IDetailClass, DetailClass, DetailClass_DAO>(LookupKeys.DetailClasses, "Key", "Description");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEventList<IDetailType> DetailTypes
        {
            get
            {
                return LoadAll<IDetailType, DetailType, DetailType_DAO>(LookupKeys.DetailTypes, "Key", "Description");
            }
        }

        public IEventList<IDisbursementStatus> DisbursementStatuses
        {
            get
            {
                return LoadAll<IDisbursementStatus, DisbursementStatus, DisbursementStatus_DAO>(LookupKeys.DisbursementStatuses, "Key", "Description");
            }
        }

        public IEventList<IApplicationOriginator> ApplicationOriginators
        {
            get
            {
                return LoadAll<IApplicationOriginator, ApplicationOriginator, ApplicationOriginator_DAO>(LookupKeys.ApplicationOriginators, "Key", "Key");
            }
        }

        public IEventList<IEducation> Educations
        {
            get
            {
                return LoadAll<IEducation, Education, Education_DAO>(LookupKeys.Educations, "Key", "Description");
            }
        }

        public IEventList<IEmployerBusinessType> EmployerBusinessTypes
        {
            get
            {
                return LoadAll<IEmployerBusinessType, EmployerBusinessType, EmployerBusinessType_DAO>(LookupKeys.EmployerBusinessTypes, "Key", "Description");
            }
        }

        public IEventList<IEmploymentSector> EmploymentSectors
        {
            get
            {
                return LoadAll<IEmploymentSector, EmploymentSector, EmploymentSector_DAO>(LookupKeys.EmploymentSectors, "Key", "Description");
            }
        }

        public IEventList<IEmploymentStatus> EmploymentStatuses
        {
            get
            {
                return LoadAll<IEmploymentStatus, EmploymentStatus, EmploymentStatus_DAO>(LookupKeys.EmploymentStatuses, "Key", "Description");
            }
        }

        public IEventList<IEmploymentType> EmploymentTypes
        {
            get
            {
                return LoadAll<IEmploymentType, EmploymentType, EmploymentType_DAO>(LookupKeys.EmploymentTypes, "Key", "Description");
            }
        }

        public IEventList<IExternalRoleType> ExternalRoleTypes
        {
            get
            {
                return LoadAll<IExternalRoleType, ExternalRoleType, ExternalRoleType_DAO>(LookupKeys.ExternalRoleTypes, "Key", "Description");
            }
        }

        public IEventList<IFeatureGroup> FeatureGroups
        {
            get
            {
                return LoadAll<IFeatureGroup, FeatureGroup, FeatureGroup_DAO>(LookupKeys.FeatureGroups, "Key", "ADUserGroup");
            }
        }

        public IEventList<IFinancialServiceGroup> FinancialServiceGroups
        {
            get
            {
                return LoadAll<IFinancialServiceGroup, FinancialServiceGroup, FinancialServiceGroup_DAO>(LookupKeys.FinancialServiceGroups, "Key", "Description");
            }
        }

        public IEventList<IFinancialServicePaymentType> FinancialServicePaymentTypes
        {
            get
            {
                return LoadAll<IFinancialServicePaymentType, FinancialServicePaymentType, FinancialServicePaymentType_DAO>(LookupKeys.FinancialServicePaymentTypes, "Key", "Description");
            }
        }

        public IEventList<IFutureDatedChangeType> FutureDatedChangeTypes
        {
            get
            {
                return LoadAll<IFutureDatedChangeType, FutureDatedChangeType, FutureDatedChangeType_DAO>(LookupKeys.FutureDatedChangeTypes, "Key", "Description");
            }
        }

        public IEventList<IGender> Genders
        {
            get
            {
                return LoadAll<IGender, Gender, Gender_DAO>(LookupKeys.Genders, "Key", "Description");
            }
        }

        /// <summary>
        /// Gets a dictionary of <see cref="IGeneralStatus"/> key/objects.  The actual objects themselves can 
        /// be stored as there are no entity type properties.
        /// </summary>
        public IDictionary<GeneralStatuses, IGeneralStatus> GeneralStatuses
        {
            get
            {
                return GetGeneralStatuses(SAHL.Common.Globals.GeneralStatuses.Pending);
            }
        }

        public IDictionary<GeneralStatuses, IGeneralStatus> GetGeneralStatuses(params GeneralStatuses[] exclusions)
        {
            if (!_dictLookups.ContainsKey(LookupKeys.GeneralStatuses))
            {
                IDictionary<GeneralStatuses, IGeneralStatus> dg = new Dictionary<GeneralStatuses, IGeneralStatus>();
                IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                foreach (GeneralStatus_DAO dao in GeneralStatus_DAO.FindAll())
                {
                    IGeneralStatus gs = bmtm.GetMappedType<IGeneralStatus>(dao);
                    dg.Add((GeneralStatuses)dao.Key, gs);
                }
                AddLookup(LookupKeys.GeneralStatuses, dg);
            }
            var generalStatuses = (IDictionary<GeneralStatuses, IGeneralStatus>)_dictLookups[LookupKeys.GeneralStatuses];
            if (exclusions != null && generalStatuses.Count > 0)
            {
                generalStatuses = generalStatuses.Where(x => !exclusions.Contains((Globals.GeneralStatuses)x.Key)).ToDictionary(y => y.Key, y => y.Value);
            }
            return generalStatuses;
        }

        // Gets a dictionary of IContentType key/objects.  The actual objects themselves can 
        // be stored as there are no entity type properties.
        //public IDictionary<ContentTypes, IContentType> ContentTypes
        //{
        //    get
        //    {
        //        if (!_dictLookups.ContainsKey(LookupKeys.ContentTypes))
        //        {
        //            IDictionary<ContentTypes, IContentType> dg = new Dictionary<ContentTypes, IContentType>();
        //            IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
        //            foreach (ContentType_DAO dao in ContentType_DAO.FindAll())
        //            {
        //                IContentType gs = bmtm.GetMappedType<IContentType>(dao);
        //                dg.Add((ContentTypes)dao.Key, gs);
        //            }
        //            AddLookup(LookupKeys.GeneralStatuses, dg);
        //        }
        //        return (IDictionary<ContentTypes, IContentType>)_dictLookups[LookupKeys.ContentTypes];
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        public IEventList<IGenericKeyType> GenericKeyType
        {
            get
            {
                return LoadAll<IGenericKeyType, SAHL.Common.BusinessModel.GenericKeyType, GenericKeyType_DAO>(LookupKeys.GenericKeyType, "Key", "Description");
            }
        }

        public IEventList<IHelpDeskCategory> HelpDeskCategories
        {
            get
            {
                return LoadAll<IHelpDeskCategory, HelpDeskCategory, HelpDeskCategory_DAO>(LookupKeys.HelpDeskCategories, "Key", "Description");
            }
        }

        public IList<IHelpDeskCategory> HelpDeskCategoriesActive(int selectedHelpDeskCategoryKey)
        {
            IEventList<IHelpDeskCategory> helpDeskCategories = new EventList<IHelpDeskCategory>(HelpDeskCategories.Where(x => x.GeneralStatus.Key == (int)Common.Globals.GeneralStatuses.Active));

            if (selectedHelpDeskCategoryKey != 0)
            {
                ICommonRepository commonRepo = RepositoryFactory.GetRepository<ICommonRepository>();
                IHelpDeskCategory selectedHelpDeskCategoryItem = commonRepo.GetByKey<IHelpDeskCategory>(selectedHelpDeskCategoryKey);

                if (selectedHelpDeskCategoryItem != null)
                {
                    if (selectedHelpDeskCategoryItem.GeneralStatus.Key == (int)Common.Globals.GeneralStatuses.Inactive)
                    {
                        helpDeskCategories.Add(null, selectedHelpDeskCategoryItem);
                    }
                }
            }

            List<IHelpDeskCategory> helpDeskCategoryList = new List<IHelpDeskCategory>();
            helpDeskCategoryList.AddRange(helpDeskCategories);
            helpDeskCategoryList.Sort(delegate(IHelpDeskCategory hdc1, IHelpDeskCategory hdc2)
            {
                return hdc1.Description.CompareTo(hdc2.Description);
            });

            if (helpDeskCategoryList.Count > 0)
            {
                return helpDeskCategoryList;
            }
            else
                return null;
        }

        public IEventList<IHOCConstruction> HOCConstruction
        {
            get
            {
                return LoadAll<IHOCConstruction, SAHL.Common.BusinessModel.HOCConstruction, HOCConstruction_DAO>(LookupKeys.HOCConstruction, "Key", "Description");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEventList<IHOCInsurer> HOCInsurers
        {
            get
            {
                return LoadAll<IHOCInsurer, HOCInsurer, HOCInsurer_DAO>(LookupKeys.HOCInsurers, "Key", "Description");
            }
        }

        public IEventList<IHOCRoof> HOCRoof
        {
            get
            {
                return LoadAll<IHOCRoof, SAHL.Common.BusinessModel.HOCRoof, HOCRoof_DAO>(LookupKeys.HOCRoof, "Key", "Description");
            }
        }

        public IEventList<IHOCStatus> HOCStatus
        {
            get
            {
                return LoadAll<IHOCStatus, SAHL.Common.BusinessModel.HOCStatus, HOCStatus_DAO>(LookupKeys.HOCStatus, "Key", "Description");
            }
        }

        public IEventList<IHOCSubsidence> HOCSubsidence
        {
            get
            {
                return LoadAll<IHOCSubsidence, SAHL.Common.BusinessModel.HOCSubsidence, HOCSubsidence_DAO>(LookupKeys.HOCSubsidence, "Key", "Description");
            }
        }

        public IEventList<IInsurer> Insurers
        {
            get
            {
                return LoadAll<IInsurer, Insurer, Insurer_DAO>(LookupKeys.Insurers, "Key", "Description");
            }
        }

        /// <summary>
        /// Gets a list of languages stored by key.
        /// </summary>
        public IDictionary<int, ILanguageReadOnly> Languages
        {
            get
            {
                if (!_dictLookups.ContainsKey(LookupKeys.Languages))
                {
                    Language_DAO[] languages = Language_DAO.FindAll();
                    Dictionary<int, ILanguageReadOnly> dict = new Dictionary<int, ILanguageReadOnly>();

                    foreach (Language_DAO l in languages)
                    {
                        dict.Add(l.Key, new LanguageReadOnly(l.Key, l.Description, l.Translatable));
                    }

                    _dictLookups.Add(LookupKeys.Languages, dict);
                }

                return (IDictionary<int, ILanguageReadOnly>)_dictLookups[LookupKeys.Languages];
            }
        }

        /// <summary>
        /// Gets a list of translatable languages.
        /// </summary>
        public ReadOnlyCollection<ILanguageReadOnly> LanguagesTranslatable
        {
            get
            {
                if (_languagesTranslatable.Count == 0)
                {
                    // run through the translatable languages
                    foreach (ILanguageReadOnly lang in Languages.Values)
                    {
                        if (lang.Translatable)
                            _languagesTranslatable.Add(lang);
                    }
                }
                return _languagesTranslatable.AsReadOnly();
            }
        }

        public IEventList<ILegalEntityExceptionStatus> LegalEntityExceptionStatuses
        {
            get
            {
                return LoadAll<ILegalEntityExceptionStatus, LegalEntityExceptionStatus, LegalEntityExceptionStatus_DAO>(LookupKeys.LegalEntityExceptionStatuses, "Key", "Description");
            }
        }

        public IEventList<ILegalEntityRelationshipType> LegalEntityRelationshipTypes
        {
            get
            {
                return LoadAll<ILegalEntityRelationshipType, LegalEntityRelationshipType, LegalEntityRelationshipType_DAO>(LookupKeys.LegalEntityRelationshipTypes, "Key", "Description");
            }
        }

        public IEventList<ILegalEntityStatus> LegalEntityStatuses
        {
            get
            {
                return LoadAll<ILegalEntityStatus, LegalEntityStatus, LegalEntityStatus_DAO>(LookupKeys.LegalEntityStatuses, "Key", "Description");
            }
        }

        public IEventList<ILegalEntityType> LegalEntityTypes
        {
            get
            {
                return LoadAll<ILegalEntityType, LegalEntityType, LegalEntityType_DAO>(LookupKeys.LegalEntityTypes, "Key", "Description");
            }
        }

        public IEventList<ILifeInsurableInterestType> LifeInsurableInterestTypes
        {
            get
            {
                return LoadAll<ILifeInsurableInterestType, LifeInsurableInterestType, LifeInsurableInterestType_DAO>(LookupKeys.LifeInsurableInterestTypes, "Key", "Description");
            }
        }

        public IEventList<ILifePolicyType> LifePolicyTypes
        {
            get
            {
                return LoadAll<ILifePolicyType, LifePolicyType, LifePolicyType_DAO>(LookupKeys.LifePolicyTypes, "Key", "Description");
            }
        }

        public IEventList<ILifePolicyStatus> LifePolicyStatuses
        {
            get
            {
                return LoadAll<ILifePolicyStatus, LifePolicyStatus, LifePolicyStatus_DAO>(LookupKeys.LifePolicyStatuses, "Key", "Description");
            }
        }

        public IEventList<IMargin> Margins
        {
            get
            {
                return LoadAll<IMargin, Margin, Margin_DAO>(LookupKeys.Margins, "Key", "Description");
            }
        }

        public IEventList<IMarketingOption> MarketingOptions
        {
            get
            {
                return LoadAll<IMarketingOption, MarketingOption, MarketingOption_DAO>(LookupKeys.MarketingOptions, "Key", "Description");
            }
        }

        /// <summary>
        /// Gets all active marketing options.
        /// </summary>
        public IDictionary<int, string> MarketingOptionsActive
        {
            get
            {
                if (!_dictLookups.ContainsKey(LookupKeys.MarketingOptionsActive))
                {
                    Dictionary<int, string> dict = new Dictionary<int, string>();
                    string HQL = "from MarketingOption_DAO mo where mo.GeneralStatus.Key = 1";
                    SimpleQuery<MarketingOption_DAO> q = new SimpleQuery<MarketingOption_DAO>(HQL);
                    MarketingOption_DAO[] res = q.Execute();
                    foreach (MarketingOption_DAO mo in res)
                    {
                        dict.Add(mo.Key, mo.Description);
                    }
                    AddLookup(LookupKeys.MarketingOptionsActive, dict);
                }
                return (IDictionary<int, string>)_dictLookups[LookupKeys.MarketingOptionsActive];
            }
        }



        public IDictionary<int, string> EmploymentSectorsActive
        {
            get
            {
                if (!_dictLookups.ContainsKey(LookupKeys.EmploymentSectorsActive))
                {
                    Dictionary<int, string> dict = new Dictionary<int, string>();
                    string HQL = "from EmploymentSector_DAO es where es.GeneralStatusKey = 1 ORDER BY Description";
                    SimpleQuery<EmploymentSector_DAO> q = new SimpleQuery<EmploymentSector_DAO>(HQL);
                    EmploymentSector_DAO[] res = q.Execute();
                    foreach (EmploymentSector_DAO es in res)
                    {
                        dict.Add(es.Key, es.Description);
                    }
                    AddLookup(LookupKeys.EmploymentSectorsActive, dict);
                }
                return (IDictionary<int, string>)_dictLookups[LookupKeys.EmploymentSectorsActive];
            }
        }






        public IEventList<IMarketRate> MarketRates
        {
            get
            {
                return LoadAll<IMarketRate, MarketRate, MarketRate_DAO>(LookupKeys.MarketRates, "Key", "Description");
            }
        }

        public IEventList<IMaritalStatus> MaritalStatuses
        {
            get
            {
                return LoadAll<IMaritalStatus, MaritalStatus, MaritalStatus_DAO>(LookupKeys.MaritalStatuses, "Key", "Description");
            }
        }

        public IEventList<IMessageType> MessageTypes
        {
            get
            {
                return LoadAll<IMessageType, MessageType, MessageType_DAO>(LookupKeys.MessageTypes, "Key", "Description");
            }
        }

        public IEventList<IMortgageLoanPurpose> MortgageLoanPurposes
        {
            get
            {
                return LoadAll<IMortgageLoanPurpose, MortgageLoanPurpose, MortgageLoanPurpose_DAO>(LookupKeys.MortgageLoanPurposes, "Key", "Description");
            }
        }

        public IEventList<IOnlineStatementFormat> OnlineStatementFormats
        {
            get
            {
                return LoadAll<IOnlineStatementFormat, OnlineStatementFormat, OnlineStatementFormat_DAO>(LookupKeys.OnlineStatementFormats, "Key", "Description");
            }
        }

        public IEventList<IOrganisationStructureOriginationSource> OrganisationStructureOrgStructure
        {
            get
            {
                return LoadAll<IOrganisationStructureOriginationSource, OrganisationStructureOriginationSource, OrganisationStructureOriginationSource_DAO>(LookupKeys.OrganisationStructureOriginationSources, "Key", "Key");
            }
        }

        public IEventList<IOrganisationStructure> OrganisationStructure
        {
            get
            {
                return LoadAll<IOrganisationStructure, SAHL.Common.BusinessModel.OrganisationStructure, OrganisationStructure_DAO>(LookupKeys.OrganisationStructures, "Key", "Description");
            }
        }

        public IEventList<IOrganisationType> OrganisationTypes
        {
            get
            {
                return LoadAll<IOrganisationType, OrganisationType, OrganisationType_DAO>(LookupKeys.OrganisationTypes, "Key", "Description");
            }
        }

        public IEventList<IOriginationSource> OriginationSources
        {
            get
            {
                return LoadAll<IOriginationSource, OriginationSource, OriginationSource_DAO>(LookupKeys.OriginationSources, "Key", "Description");
            }
        }

        public IEventList<IPopulationGroup> PopulationGroups
        {
            get
            {
                return LoadAll<IPopulationGroup, PopulationGroup, PopulationGroup_DAO>(LookupKeys.PopulationGroups, "Key", "Description");
            }
        }

        public IEventList<IProduct> Products
        {
            get
            {
                return LoadAll<IProduct, Product, Product_DAO>(LookupKeys.Products, "Key", "Description");
            }
        }

        public IEventList<IPropertyType> PropertyTypes
        {
            get
            {
                return LoadAll<IPropertyType, PropertyType, PropertyType_DAO>(LookupKeys.PropertyTypes, "Key", "Description");
            }
        }

        public IEventList<ITitleType> TitleTypes
        {
            get
            {
                return LoadAll<ITitleType, TitleType, TitleType_DAO>(LookupKeys.TitleTypes, "Key", "Description");
            }
        }

        public IEventList<IOccupancyType> OccupancyTypes
        {
            get
            {
                return LoadAll<IOccupancyType, OccupancyType, OccupancyType_DAO>(LookupKeys.OccupancyTypes, "Key", "Description");
            }
        }

        /// <summary>
        /// Implements <see cref="ILookupRepository.ProvincesByCountry"/>.
        /// </summary>
        /// <param name="countryKey"></param>
        /// <returns></returns>
        public IDictionary<int, string> ProvincesByCountry(int countryKey)
        {
            if (!_provincesByCountry.ContainsKey(countryKey))
            {
                IDictionary<int, string> dict = new Dictionary<int, string>();
                Country_DAO country = Country_DAO.Find(countryKey);
                foreach (Province_DAO p in country.Provinces)
                {
                    dict.Add(p.Key, p.Description);
                }
                _provincesByCountry.Add(countryKey, dict);
            }
            return _provincesByCountry[countryKey];
        }

        /// <summary>
        /// Implements <see cref="ILookupRepository.PrioritiesByOSP"/>.
        /// </summary>
        /// <param name="originationSourceProductKey"></param>
        /// <returns></returns>
        public IEventList<IPriority> PrioritiesByOSP(int originationSourceProductKey)
        {
            if (!_prioritiesByOSP.ContainsKey(originationSourceProductKey))
            {
                IEventList<IPriority> priorities = new EventList<IPriority>();
                OriginationSourceProduct_DAO osp = OriginationSourceProduct_DAO.Find(originationSourceProductKey);
                IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();

                foreach (Priority_DAO p in osp.Priorities)
                {
                    priorities.Add(null, bmtm.GetMappedType<IPriority>(p));
                }

                _prioritiesByOSP.Add(originationSourceProductKey, priorities);
            }
            return _prioritiesByOSP[originationSourceProductKey];
        }

        //public IEventList<IOriginationSourceProduct> OriginationSourceProducts
        //{
        //    get
        //    {
        //        return LoadAll<IOriginationSourceProduct, OriginationSourceProduct, OriginationSourceProduct_DAO>(LookupKeys.OriginationSourceProducts, "Key", "Description");
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        public IEventList<IReasonDescription> ReasonDescriptions
        {
            get
            {
                return LoadAll<IReasonDescription, ReasonDescription, ReasonDescription_DAO>(LookupKeys.ReasonDescriptions, "Key", "Description");
            }
        }

        public IEventList<IReasonType> ReasonTypes
        {
            get
            {
                return LoadAll<IReasonType, ReasonType, ReasonType_DAO>(LookupKeys.ReasonTypes, "Key", "Description");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEventList<IRecurringTransactionType> RecurringTransactionTypes
        {
            get
            {
                return LoadAll<IRecurringTransactionType, RecurringTransactionType, RecurringTransactionType_DAO>(LookupKeys.RecurringTransactionTypes, "Key", "Description");
            }
        }

        /// <summary>
        /// Returns an EventList of ReasonTypeGroups
        /// </summary>
        public IEventList<IReasonTypeGroup> ReasonTypeGroups
        {
            get
            {
                return LoadAll<IReasonTypeGroup, ReasonTypeGroup, ReasonTypeGroup_DAO>(LookupKeys.ReasonTypeGroups, "Key", "Description");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEventList<IRemunerationType> RemunerationTypes
        {
            get
            {
                return LoadAll<IRemunerationType, RemunerationType, RemunerationType_DAO>(LookupKeys.RemunerationTypes, "Key", "Description");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEventList<IReportFormatType> ReportFormatTypes
        {
            get
            {
                return LoadAll<IReportFormatType, ReportFormatType, ReportFormatType_DAO>(LookupKeys.ReportFormatTypes, "Key", "Description");
            }

        }

        public IEventList<IReportGroup> ReportGroups
        {
            get
            {
                return LoadAll<IReportGroup, ReportGroup, ReportGroup_DAO>(LookupKeys.ReportGroups, "Key", "Description");
            }
        }

        public IEventList<IResidenceStatus> ResidenceStatuses
        {
            get
            {
                return LoadAll<IResidenceStatus, ResidenceStatus, ResidenceStatus_DAO>(LookupKeys.ResidenceStatuses, "Key", "Description");
            }
        }

        public IEventList<IRoleType> RoleTypes
        {
            get
            {
                return LoadAll<IRoleType, RoleType, RoleType_DAO>(LookupKeys.RoleTypes, "Key", "Description");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEventList<IRuleItem> RuleItems
        {
            get
            {
                return LoadAll<IRuleItem, RuleItem, RuleItem_DAO>(LookupKeys.RuleItems, "Key", "Name");
            }
        }


        static object syncObject = new object();
        /// <summary>
        /// Gets rule items, sorted by name.
        /// </summary>
        public IDictionary<string, IRuleItem> RuleItemsByName
        {
            get
            {
                //GaryD: Locking this to make it Thread safe
                lock (syncObject)
                {
                    if (_ruleItemsByName == null ||
                    !_dictLookups.ContainsKey(LookupKeys.RuleItems))    // dependent on RuleItems!
                    {
                        _ruleItemsByName = new Dictionary<string, IRuleItem>();
                        foreach (IRuleItem ruleItem in this.RuleItems)
                            _ruleItemsByName.Add(ruleItem.Name, ruleItem);
                    }
                }

                return _ruleItemsByName;
            }
        }


        internal class RuleSorter : IComparer<IRuleItem>
        {

            #region IComparer<IRuleItem> Members

            public int Compare(IRuleItem x, IRuleItem y)
            {
                return string.Compare(x.Name, y.Name);
            }

            #endregion
        }
        internal List<IRuleItem> _RuleItemList = null;
        public IList<IRuleItem> RuleItemList
        {
            get
            {
                if (null == _RuleItemList)
                {
                    _RuleItemList = new List<IRuleItem>();
                    foreach (IRuleItem ri in RuleItems)
                    {
                        _RuleItemList.Add(ri);
                    }
                    _RuleItemList.Sort(new RuleSorter());
                }
                return _RuleItemList;
            }
        }

        public IEventList<ISalutation> Salutations
        {
            get
            {
                return LoadAll<ISalutation, Salutation, Salutation_DAO>(LookupKeys.Salutations, "Key", "Description");
            }
        }

        public IEventList<ISPV> SPVList
        {
            get
            {
                return LoadAll<ISPV, SPV, SPV_DAO>(LookupKeys.SPVs, "Key", "Description");
            }
        }

        public IEventList<IStageDefinitionGroup> StageDefinitionGroups
        {
            get
            {
                return LoadAll<IStageDefinitionGroup, StageDefinitionGroup, StageDefinitionGroup_DAO>(LookupKeys.StageDefinitionGroups, "Key", "Description");
            }
        }

        //public IEventList<ISubsidyProvider> SubsidyProviders
        //{
        //    get
        //    {
        //        return LoadAll<ISubsidyProvider, SubsidyProvider, SubsidyProvider_DAO>(LookupKeys.SubsidyProviders, "Key", "Description");
        //    }
        //}

        public IEventList<ISubsidyProviderType> SubsidyProviderTypes
        {
            get
            {
                return LoadAll<ISubsidyProviderType, SubsidyProviderType, SubsidyProviderType_DAO>(LookupKeys.SubsidyProviderTypes, "Key", "Description");
            }
        }


        public IEventList<ITransactionType> TransactionTypes
        {
            get
            {
                return LoadAll<ITransactionType, TransactionType, TransactionType_DAO>(LookupKeys.TransactionTypes, "Key", "Description");
            }
        }

        //public IEventList<IUIStatement> uiStatementTypes
        //{
        //    get
        //    {
        //        return LoadAll<IUIStatement, UIStatementType, UIStatementType_DAO>(LookupKeys.UIStatementTypes, "Key", "StatementName");
        //    }
        //}

        public IEventList<IDataProvider> DataProviders
        {
            get
            {
                return LoadAll<IDataProvider, DataProvider, DataProvider_DAO>(LookupKeys.DataProviders, "Key", "Description");
            }
        }

        //public IEventList<IDataProviderDataService> DataProviderDataServices
        //{
        //    get
        //    {
        //        DataProviderDataService d;
        //        return LoadAll<IDataProviderDataService, DataProviderDataService, DataProviderDataService_DAO>(LookupKeys.DataProviderDataServices, "Key", "Description");
        //    }
        //}

        public IEventList<IValuationDataProviderDataService> ValuationDataProviderDataServices
        {
            get
            {
                return LoadAll<IValuationDataProviderDataService, ValuationDataProviderDataService, ValuationDataProviderDataService_DAO>(LookupKeys.ValuationDataProviderDataServices, "Key", "Key");
            }
        }

        public IEventList<IPropertyDataProviderDataService> PropertyDataProviderDataServices
        {
            get
            {
                return LoadAll<IPropertyDataProviderDataService, PropertyDataProviderDataService, PropertyDataProviderDataService_DAO>(LookupKeys.PropertyDataProviderDataServices, "Key", "Key");
            }
        }

        public IEventList<IDataService> DataServices
        {
            get
            {
                return LoadAll<IDataService, DataService, DataService_DAO>(LookupKeys.DataServices, "Key", "Description");
            }
        }

        public IEventList<IValuationStatus> ValuationStatus
        {
            get
            {
                return LoadAll<IValuationStatus, SAHL.Common.BusinessModel.ValuationStatus, ValuationStatus_DAO>(LookupKeys.ValuationStatuses, "Key", "Description");
            }
        }

        public IEventList<IValuationClassification> ValuationClassification
        {
            get
            {
                return LoadAll<IValuationClassification, SAHL.Common.BusinessModel.ValuationClassification, ValuationClassification_DAO>(LookupKeys.ValuationClassifications, "Key", "Key");
            }
        }


        public IEventList<IValuationRoofType> ValuationRoofTypes
        {
            get
            {
                return LoadAll<IValuationRoofType, ValuationRoofType, ValuationRoofType_DAO>(LookupKeys.ValuationRoofTypes, "Key", "Key");
            }
        }

        public IEventList<IValuationImprovementType> ValuationImprovementType
        {
            get
            {
                return LoadAll<IValuationImprovementType, SAHL.Common.BusinessModel.ValuationImprovementType, ValuationImprovementType_DAO>(LookupKeys.ValuationImprovementTypes, "Key", "Key");
            }
        }

        public IDictionary<string, IWorkflowMenu> WorkflowMenus
        {
            get
            {
                if (!_dictLookups.ContainsKey(LookupKeys.WorkflowMenus))
                {
                    Dictionary<string, IWorkflowMenu> dict = new Dictionary<string, IWorkflowMenu>();
                    WorkflowMenu_DAO[] items = WorkflowMenu_DAO.FindAll();
                    IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    foreach (WorkflowMenu_DAO wfm in items)
                    {
                        string key = wfm.StateName + "_" + wfm.WorkflowName + "_" + wfm.ProcessName;
                        dict.Add(key, bmtm.GetMappedType<IWorkflowMenu>(wfm));
                    }
                    AddLookup(LookupKeys.WorkflowMenus, dict);
                }
                return (IDictionary<string, IWorkflowMenu>)_dictLookups[LookupKeys.WorkflowMenus];
            }
        }

        public IEventList<IApplicationInformationType> ApplicationInformationTypes
        {
            get
            {
                return LoadAll<IApplicationInformationType, ApplicationInformationType, ApplicationInformationType_DAO>(LookupKeys.ApplicationInformationTypes, "Key", "Description");
            }
        }

        public IEventList<ICategory> Categories
        {
            get
            {
                return LoadAll<ICategory, Category, Category_DAO>(LookupKeys.Categories, "Key", "Description");
            }
        }

        public IEventList<IQuickCashPaymentType> QuickCashPaymentTypes
        {
            get
            {
                return LoadAll<IQuickCashPaymentType, QuickCashPaymentType, QuickCashPaymentType_DAO>(LookupKeys.QuickCashPaymentTypes, "Key", "Description");
            }
        }

        /// <summary>
        /// Returns an EventList of ImportStatuses
        /// </summary>
        public IEventList<IImportStatus> ImportStatuses
        {
            get
            {
                return LoadAll<IImportStatus, ImportStatus, ImportStatus_DAO>(LookupKeys.ImportStatuses, "Key", "Description");
            }
        }

        /// <summary>
        /// Returns RuleExclusionSets
        /// </summary>
        public IEventList<IRuleExclusionSet> RuleExclusionSets
        {
            get
            {
                return LoadAll<IRuleExclusionSet, RuleExclusionSet, RuleExclusionSet_DAO>(LookupKeys.RuleExclusionSets, "Key", "Description");
            }
        }

        /// <summary>
        /// Returns AccountIndications
        /// </summary>
        public IEventList<IAccountIndication> AccountIndications
        {
            get
            {
                return LoadAll<IAccountIndication, AccountIndication, AccountIndication_DAO>(LookupKeys.AccountIndications, "Key", "AccountIndicator");
            }
        }
        internal class RulSetSorter : IComparer<IWorkflowRuleSet>
        {
            #region IComparer<IWorkflowRuleSet> Members

            public int Compare(IWorkflowRuleSet x, IWorkflowRuleSet y)
            {
                return string.Compare(x.Name, y.Name);
            }

            #endregion
        }
        internal List<IWorkflowRuleSet> _RuleSetList = null;
        public IList<IWorkflowRuleSet> WorkflowRuleSetSorted
        {
            get
            {
                if (null == _RuleSetList)
                {
                    _RuleSetList = new List<IWorkflowRuleSet>();
                    foreach (IWorkflowRuleSet w in WorkflowRuleSets)
                    {
                        _RuleSetList.Add(w);
                    }
                    _RuleSetList.Sort(new RulSetSorter());
                }
                return _RuleSetList;
            }
        }

        public IEventList<IWorkflowRuleSet> WorkflowRuleSets
        {
            get
            {
                return LoadAll<IWorkflowRuleSet, WorkflowRuleSet, WorkflowRuleSet_DAO>(LookupKeys.WorkflowRuleSets, "Key", "Name");

            }
        }

        public IEventList<IEmploymentConfirmationSource> EmploymentConfirmationSources
        {
            get
            {
                return LoadAll<IEmploymentConfirmationSource, EmploymentConfirmationSource, EmploymentConfirmationSource_DAO>(LookupKeys.EmploymentConfirmationSources, "Key", "Description");

            }
        }

        public IEventList<IEmploymentVerificationProcessType> EmploymentVerificationProcessTypes
        {
            get
            {
                return LoadAll<IEmploymentVerificationProcessType, EmploymentVerificationProcessType, EmploymentVerificationProcessType_DAO>(LookupKeys.EmploymentVerificationProcessTypes, "Key", "Description");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEventList<IRiskMatrixDimension> RiskMatrixDimensions
        {
            get
            {
                return LoadAll<IRiskMatrixDimension, RiskMatrixDimension, RiskMatrixDimension_DAO>(LookupKeys.RiskMatrixDimensions, "Key", "Description");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<SAHL.Common.Globals.CreditScoreDecisions, ICreditScoreDecision> CreditScoreDecisions
        {
            get
            {
                if (!_dictLookups.ContainsKey(LookupKeys.CreditScoreDecisions))
                {
                    Dictionary<SAHL.Common.Globals.CreditScoreDecisions, ICreditScoreDecision> dict = new Dictionary<SAHL.Common.Globals.CreditScoreDecisions, ICreditScoreDecision>();
                    CreditScoreDecision_DAO[] items = CreditScoreDecision_DAO.FindAll();
                    IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    foreach (CreditScoreDecision_DAO itm in items)
                    {
                        string key = Enum.GetName(typeof(SAHL.Common.Globals.CreditScoreDecisions), itm.Key);
                        SAHL.Common.Globals.CreditScoreDecisions Key = (SAHL.Common.Globals.CreditScoreDecisions)Enum.Parse(typeof(SAHL.Common.Globals.CreditScoreDecisions), key);

                        dict.Add(Key, bmtm.GetMappedType<ICreditScoreDecision>(itm));
                    }
                    AddLookup(LookupKeys.CreditScoreDecisions, dict);
                }
                return (IDictionary<SAHL.Common.Globals.CreditScoreDecisions, ICreditScoreDecision>)_dictLookups[LookupKeys.CreditScoreDecisions];
            }
        }


        /// <summary>
        /// Gets a dictionary of <see cref="IMarketingOptionRelevance"/> key/objects.  The actual objects themselves can 
        /// be stored as there are no entity type properties.
        /// </summary>
        public IDictionary<MarketingOptionRelevances, IMarketingOptionRelevance> MarketingOptionRelevances
        {
            get
            {
                if (!_dictLookups.ContainsKey(LookupKeys.MarketingOptionRelevance))
                {
                    IDictionary<MarketingOptionRelevances, IMarketingOptionRelevance> no = new Dictionary<MarketingOptionRelevances, IMarketingOptionRelevance>();
                    IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    foreach (MarketingOptionRelevance_DAO dao in MarketingOptionRelevance_DAO.FindAll())
                    {
                        IMarketingOptionRelevance mor = bmtm.GetMappedType<IMarketingOptionRelevance>(dao);
                        no.Add((MarketingOptionRelevances)dao.Key, mor);
                    }
                    AddLookup(LookupKeys.MarketingOptionRelevance, no);
                }
                return (IDictionary<MarketingOptionRelevances, IMarketingOptionRelevance>)_dictLookups[LookupKeys.MarketingOptionRelevance];
            }
        }

        /// <summary>
        /// Gets a dictionary of <see cref="IProposalStatus"/> key/objects.  The actual objects themselves can 
        /// be stored as there are no entity type properties.
        /// </summary>
        public IDictionary<ProposalStatuses, IProposalStatus> ProposalStatuses
        {
            get
            {
                if (!_dictLookups.ContainsKey(LookupKeys.ProposalStatuses))
                {
                    IDictionary<ProposalStatuses, IProposalStatus> dg = new Dictionary<ProposalStatuses, IProposalStatus>();
                    IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    foreach (ProposalStatus_DAO dao in ProposalStatus_DAO.FindAll())
                    {
                        IProposalStatus gs = bmtm.GetMappedType<IProposalStatus>(dao);
                        dg.Add((ProposalStatuses)dao.Key, gs);
                    }
                    AddLookup(LookupKeys.ProposalStatuses, dg);
                }
                return (IDictionary<ProposalStatuses, IProposalStatus>)_dictLookups[LookupKeys.ProposalStatuses];
            }
        }

        /// <summary>
        /// Gets a dictionary of <see cref="IDebtCounsellingStatus"/> key/objects.  The actual objects themselves can 
        /// be stored as there are no entity type properties.
        /// </summary>
        public IDictionary<DebtCounsellingStatuses, IDebtCounsellingStatus> DebtCounsellingStatuses
        {
            get
            {
                if (!_dictLookups.ContainsKey(LookupKeys.DebtCounsellingStatuses))
                {
                    IDictionary<DebtCounsellingStatuses, IDebtCounsellingStatus> dg = new Dictionary<DebtCounsellingStatuses, IDebtCounsellingStatus>();
                    IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    foreach (DebtCounsellingStatus_DAO dao in DebtCounsellingStatus_DAO.FindAll())
                    {
                        IDebtCounsellingStatus gs = bmtm.GetMappedType<IDebtCounsellingStatus>(dao);
                        dg.Add((DebtCounsellingStatuses)dao.Key, gs);
                    }
                    AddLookup(LookupKeys.DebtCounsellingStatuses, dg);
                }
                return (IDictionary<DebtCounsellingStatuses, IDebtCounsellingStatus>)_dictLookups[LookupKeys.DebtCounsellingStatuses];
            }
        }
        /// <summary>
        /// Gets a dictionary of <see cref="IProposalType"/> key/objects.  The actual objects themselves can 
        /// be stored as there are no entity type properties.
        /// </summary>
        public IDictionary<ProposalTypes, IProposalType> ProposalTypes
        {
            get
            {
                if (!_dictLookups.ContainsKey(LookupKeys.ProposalTypes))
                {
                    IDictionary<ProposalTypes, IProposalType> dg = new Dictionary<ProposalTypes, IProposalType>();
                    IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    foreach (ProposalType_DAO dao in ProposalType_DAO.FindAll())
                    {
                        IProposalType gs = bmtm.GetMappedType<IProposalType>(dao);
                        dg.Add((ProposalTypes)dao.Key, gs);
                    }
                    AddLookup(LookupKeys.ProposalTypes, dg);
                }
                return (IDictionary<ProposalTypes, IProposalType>)_dictLookups[LookupKeys.ProposalTypes];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<int, string> CourtTypes
        {
            get
            {
                if (!_dictLookups.ContainsKey(LookupKeys.CourtTypes))
                {
                    Dictionary<int, string> dict = new Dictionary<int, string>();
                    string HQL = "from CourtType_DAO c order by c.Description";
                    SimpleQuery<CourtType_DAO> q = new SimpleQuery<CourtType_DAO>(HQL);
                    CourtType_DAO[] res = q.Execute();
                    foreach (CourtType_DAO mo in res)
                    {
                        dict.Add(mo.Key, mo.Description);
                    }
                    AddLookup(LookupKeys.CourtTypes, dict);
                }
                return (IDictionary<int, string>)_dictLookups[LookupKeys.CourtTypes];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<int, string> HearingTypes
        {
            get
            {
                if (!_dictLookups.ContainsKey(LookupKeys.HearingTypes))
                {
                    Dictionary<int, string> dict = new Dictionary<int, string>();
                    string HQL = "from HearingType_DAO h order by h.Description";
                    SimpleQuery<HearingType_DAO> q = new SimpleQuery<HearingType_DAO>(HQL);
                    HearingType_DAO[] res = q.Execute();
                    foreach (HearingType_DAO mo in res)
                    {
                        dict.Add(mo.Key, mo.Description);
                    }
                    AddLookup(LookupKeys.HearingTypes, dict);
                }
                return (IDictionary<int, string>)_dictLookups[LookupKeys.HearingTypes];
            }
        }

        /// <summary>
        /// Litigation Attorney Role Types
        /// </summary>
        public IDictionary<int, string> LitigationAttorneyRoleTypes
        {
            get
            {
                if (!_dictLookups.ContainsKey(LookupKeys.LitigationExternalRoleTypes))
                {
                    Dictionary<int, string> dict = new Dictionary<int, string>();
                    string HQL = "from ExternalRoleType_DAO ert where ert.ExternalRoleTypeGroup.Key = 3";
                    SimpleQuery<ExternalRoleType_DAO> q = new SimpleQuery<ExternalRoleType_DAO>(HQL);
                    ExternalRoleType_DAO[] res = q.Execute();
                    foreach (ExternalRoleType_DAO mo in res)
                    {
                        dict.Add(mo.Key, mo.Description);
                    }
                    AddLookup(LookupKeys.LitigationExternalRoleTypes, dict);
                }
                return (IDictionary<int, string>)_dictLookups[LookupKeys.LitigationExternalRoleTypes];
            }
        }

        public IEventList<IFinancialAdjustmentType> FinancialAdjustmentTypes
        {
            get
            {
                return LoadAll<IFinancialAdjustmentType, FinancialAdjustmentType, FinancialAdjustmentType_DAO>(LookupKeys.FinancialAdjustmentTypes, "Key", "Description");
            }
        }

        public IEventList<IFinancialAdjustmentSource> FinancialAdjustmentSources
        {
            get
            {
                return LoadAll<IFinancialAdjustmentSource, FinancialAdjustmentSource, FinancialAdjustmentSource_DAO>(LookupKeys.FinancialAdjustmentSources, "Key", "Description");
            }
        }


        public IEventList<IFinancialAdjustmentStatus> FinancialAdjustmentStatuses
        {
            get { return LoadAll<IFinancialAdjustmentStatus, FinancialAdjustmentStatus, FinancialAdjustmentStatus_DAO>(LookupKeys.FinancialAdjustmentStatuses, "Key", "Description"); }
        }

        public IDictionary<int, string> ClaimTypes
        {
            get
            {
                if (!_dictLookups.ContainsKey(LookupKeys.ClaimTypes))
                {
                    Dictionary<int, string> dict = new Dictionary<int, string>();
                    string HQL = "from ClaimType_DAO ct order by ct.Key";
                    SimpleQuery<ClaimType_DAO> q = new SimpleQuery<ClaimType_DAO>(HQL);
                    ClaimType_DAO[] res = q.Execute();
                    foreach (ClaimType_DAO mo in res)
                    {
                        dict.Add(mo.Key, mo.Description);
                    }
                    AddLookup(LookupKeys.ClaimTypes, dict);
                }
                return (IDictionary<int, string>)_dictLookups[LookupKeys.ClaimTypes];
            }
        }

        public IDictionary<int, string> ClaimStatuses
        {
            get
            {
                if (!_dictLookups.ContainsKey(LookupKeys.ClaimStatuses))
                {
                    Dictionary<int, string> dict = new Dictionary<int, string>();
                    string HQL = "from ClaimStatus_DAO cs order by cs.Key";
                    SimpleQuery<ClaimStatus_DAO> q = new SimpleQuery<ClaimStatus_DAO>(HQL);
                    ClaimStatus_DAO[] res = q.Execute();
                    foreach (ClaimStatus_DAO mo in res)
                    {
                        dict.Add(mo.Key, mo.Description);
                    }
                    AddLookup(LookupKeys.ClaimStatuses, dict);
                }
                return (IDictionary<int, string>)_dictLookups[LookupKeys.ClaimStatuses];
            }
        }

        public void ClearRuleCache()
        {
            this.RuleItemList.Clear();
            this.RuleItemsByName.Clear();
            this.ResetLookup(LookupKeys.RuleItems);
            this.ResetLookup(LookupKeys.RuleExclusionSets);
            
        }

        public IDictionary<int, string> DisabilityTypes
        {
            get
            {
                if (!_dictLookups.ContainsKey(LookupKeys.DisabilityTypes))
                {
                    Dictionary<int, string> dictionary = new Dictionary<int, string>();
                    string HQL = "from DisabilityType_DAO order by Description";
                    SimpleQuery<DisabilityType_DAO> query = new SimpleQuery<DisabilityType_DAO>(HQL);
                    DisabilityType_DAO[] results = query.Execute();
                    foreach (DisabilityType_DAO item in results)
                    {
                        dictionary.Add(item.Key, item.Description);
                    }
                    AddLookup(LookupKeys.DisabilityTypes, dictionary);
                }
                return (IDictionary<int, string>)_dictLookups[LookupKeys.DisabilityTypes];
            }
        }


        public IEventList<IAffordabilityAssessmentStatus> AffordabilityAssessmentStatuses
        {
            get
            {
                return LoadAll<IAffordabilityAssessmentStatus, AffordabilityAssessmentStatus, AffordabilityAssessmentStatus_DAO>(LookupKeys.AffordabilityAssessmentStatuses, "Key", "Description");
            }
        }
    }
}
