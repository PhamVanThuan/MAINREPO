using System;
using System.Collections.Generic;
using System.Data;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Web.Views.Common.Interfaces;

using SAHL.Common.UI;

namespace SAHL.Web.Views.Common.Presenters.ValuationDetails
{
    public class ValuationAutomatedRequestPresenter : SAHLCommonBasePresenter<IValuationDetailsView>
    {
        private IPropertyRepository propertyrepository;
        private IApplicationRepository applicationrepository;
        private IAccountRepository accountrepository;
        private ILookupRepository lookuprepository;
        //private IApplicationMortgageLoan applicationMortgageLoan;
        
        private DataTable dsValuations = new DataTable();
        private DataTable dsProperties = new DataTable();
        private DataTable dsAddress = new DataTable();
        private CBOMenuNode node;
        //private InstanceNode instanceNode;
        private ILightStoneService _avm;
        private IApplication application;
        private IProperty property;
        private int requiredKey;
        private IReadOnlyEventList<ILegalEntity> LegalEntities;


        private ILightStoneService AVM
        {
            get 
            {
                if (_avm == null)
                    _avm = ServiceFactory.GetService<ILightStoneService>();
                return _avm; 
            }
        }

        protected ILookupRepository LookupRepository
        {
            get
            {
                if (lookuprepository == null)
                    lookuprepository = RepositoryFactory.GetRepository<ILookupRepository>();
                return lookuprepository;
            }
        }

        protected IPropertyRepository PropertyRepository
        {
            get
            {
                if (propertyrepository == null)
                    propertyrepository = RepositoryFactory.GetRepository<IPropertyRepository>();
                return propertyrepository;
            }
        }


        protected IApplicationRepository ApplicationRepository
        {
            get
            {
                if (applicationrepository == null)
                    applicationrepository = RepositoryFactory.GetRepository<IApplicationRepository>();
                return applicationrepository;
            }
        }


        protected IAccountRepository AccountRepository
        {
            get
            {
                if (accountrepository == null)
                    accountrepository = RepositoryFactory.GetRepository<IAccountRepository>();
                return accountrepository;
            }
        }


        public ValuationAutomatedRequestPresenter(IValuationDetailsView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
           
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;
            //Set up the Interface
            node = CBOManager.GetCurrentCBONode(View.CurrentPrincipal) as CBOMenuNode;

            View.ShowgrdValuations = true;
            View.ShowpnlValuations = true;
            View.ShowpnlInspectionContactDetails = false;
            View.ShowbtnViewDetail = false;
            View.ShowbtnCancel = false;
            View.ShowbtnSubmit = true;
            // set up the eventhandlers
            View.btnSubmitClicked += btnSubmitClicked;
            View.btnPropertyClicked += btnSelectPropertyClicked;

            View.SetPostBackTypegrdValuations = GridPostBackType.NoneWithClientSelect;

            if (!PrivateCacheData.ContainsKey("PropertiesIndex"))
                PrivateCacheData.Add("PropertiesIndex", 0);

        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;
            // Get the AccountKey/OfferKey from the CBO
            //if (node is InstanceNode)
            //{
            //    instanceNode = node as InstanceNode;
            //    requiredKey = Convert.ToInt32(instanceNode.GenericKey);
            //}
            //else
            //    requiredKey = Convert.ToInt32(node.GenericKey);
            //IEventList<IApplication> applications;
            //switch (node.GenericKeyTypeKey)
            //{
            //    case (int)GenericKeyTypes.Account:
            //        IMortgageLoanAccount account = (IMortgageLoanAccount)accountRepository.GetAccountByKey(requiredKey);
            //        property = account.SecuredMortgageLoan.Properties[0];
            //        applications = applicationRepository.GetApplicationByAccountKey(requiredKey);
            //        application = applications[applications.Count - 1];
            //        break;
            //    case (int)GenericKeyTypes.Property:
            //        property = propertyRepository.GetPropertyByKey(requiredKey);
            //        applications = property.AccountProperties[0].Applications;
            //        application = applications[applications.Count - 1];
            //        break;
            //    case (int)GenericKeyTypes.Offer:
            //        applicationMortgageLoan = applicationRepository.GetApplicationByKey(requiredKey) as IApplicationMortgageLoan;
            //        property = applicationMortgageLoan.Property;
            //        application = applicationRepository.GetApplicationByKey(requiredKey);
            //        break;
            //    default:
            //        break;
            //}

            requiredKey = 237127;
            property = PropertyRepository.GetPropertyByKey(requiredKey);

            application = ApplicationRepository.GetApplicationByKey(requiredKey);

            OfferRoleTypes[] roleTypes = new OfferRoleTypes[] { OfferRoleTypes.MainApplicant };
            LegalEntities = application.GetLegalEntitiesByRoleType(roleTypes, GeneralStatusKey.All);

            PrivateCacheData.Remove("LegalEntities");
            PrivateCacheData.Add("LegalEntities", LegalEntities);

            PrivateCacheData.Remove("application");
            PrivateCacheData.Add("application", application);

            PrivateCacheData.Remove("PropertyKey");
            PrivateCacheData.Add("PropertyKey", requiredKey);

            if (PrivateCacheData.ContainsKey("AddressDS"))
                View.BindPropertyGrid(PrivateCacheData["AddressDS"] as DataTable);
            else
                View.BindPropertyGrid((GetPropertyDT(property.Address)));

            if (GlobalCacheData.ContainsKey("ValuationDS"))
                dsValuations = GlobalCacheData["ValuationDS"] as DataTable;
            else
                PopulateValuationsDS(property.Valuations);

            View.BindgrdValuations(dsValuations);
        }

        private DataTable GetPropertyDT(IAddress Address)
        {
            // Setup the Valuations Table 
            dsAddress.Reset();
            dsAddress.Columns.Add("Address");
            dsAddress.TableName = "AddressDS";

            DataRow valRow = dsAddress.NewRow();
            valRow["Address"] = Address.GetFormattedDescription(AddressDelimiters.Space);
            dsAddress.Rows.Add(valRow);

            PrivateCacheData.Remove("AddressDS");
            PrivateCacheData.Add("AddressDS", dsAddress);

            return dsAddress;


        }


        void btnSubmitClicked(object sender, EventArgs e)
        {

            // check that no Previous automated valuations are pending
            // check that there are no automated valuations less than three months old
            //0= Existing; 1=Valauation Available Less than 3 months 2 = None; 3=None Less than 3 Months

            bool CanCreateNewValuation = false;
            string ReturnMessage = "";
            int propertyKey = Convert.ToInt32(PrivateCacheData["PropertyKey"]);
            property = PropertyRepository.GetPropertyByKey(propertyKey);
            IEventList<IValuation> valuations = property.Valuations;
            if (valuations.Count > 0)
            {
                // existing valuation
                for (int i = 0; i < valuations.Count; i++)
                    if (valuations[i].ValuationDataProviderDataService.Key == 3) // Automated Lightstone Valuation
                        if (valuations[i].ValuationStatus.Key == 1) //1=pending 2=complete
                        {
                            CanCreateNewValuation = false;
                            ReturnMessage = "There is currently an automated valuation pending. You cannot request a new vautomated aluation.";
                            break;
                        }
                        else
                        {
                            CanCreateNewValuation = true;
                            //TODO Thie code below is correct remove the bool setting above and uncomment below
                            //if (valuations[i].ValuationDate > DateTime.Now.AddMonths(-3))
                            //{
                            //    // valuation done in the last 3 months navigate to it. its still valid.
                            //    GlobalCacheData.Remove("ValuationKey");
                            //    GlobalCacheData.Add("ValuationKey", valuations[i].Key, new List<ICacheObjectLifeTime>());
                            //    PrivateCacheData.Remove("Property");
                            //    View.Navigator.Navigate("Lightstone");
                            //}
                            //else
                            //{
                            //    // valuation done more than 3 months ago . redo
                            //    CanCreateNewValuation = true;
                            //    break;
                            //}
                        }
            }
            else // No valuations - start a new Lightstone Valuation....
                CanCreateNewValuation = true;

            if (CanCreateNewValuation)
            {
                LegalEntities = PrivateCacheData["LegalEntities"] as IReadOnlyEventList<ILegalEntity>;
                if (GetLightstonePropertyList(LegalEntities))
                {
                    // set up the interface
                    View.ShowbtnSubmit = false;
                    View.ShowbtnProperty = true;
                    View.EnablegrdValuations = false;
                }
                else
                {
                    // set up the interface
                    View.ShowbtnSubmit = false;
                    View.ShowbtnProperty = false;
                    View.EnablegrdValuations = false;
                }
            }
            else
            {
                View.ShowlblErrorMessage = true;
                View.SetlblErrorMessage = ReturnMessage;
            }
        }


        void btnSelectPropertyClicked(object sender, EventArgs e)
        {
            int propertyKey = Convert.ToInt32(PrivateCacheData["PropertyKey"]) ;
            property = PropertyRepository.GetPropertyByKey(propertyKey);
            dsProperties = PrivateCacheData["LightstoneProperties"] as DataTable;
            int propertyindex = (int)PrivateCacheData["PropertiesIndex"];

            if (dsProperties != null)
            {
                string LightstonePropertyID = Convert.ToString(dsProperties.Rows[propertyindex][0]);

                if (CreateNewLightStoneValuation(property, LightstonePropertyID))
                {
                    PrivateCacheData.Remove("LightstoneProperties");
                    PrivateCacheData.Remove("Property");
                    View.Navigator.Navigate("Submit");
                }
                else
                {
                    const string ReturnMessage = "There has been an error attempting to create an automated Valuation. Please try again later.";
                    View.ShowbtnSubmit = false;
                    View.ShowbtnProperty = false;
                    View.EnablegrdValuations = false;
                    View.ShowlblErrorMessage = true;
                    View.SetlblErrorMessage = ReturnMessage;
                }
            }
        }

        /// <summary>
        ///  Get a list of Matching Properties from Lightstone
        /// </summary>
        /// <param name="LE"></param>
        /// <returns></returns>
        private bool GetLightstonePropertyList(IReadOnlyEventList<ILegalEntity> LE)
        {
            bool retval = false;
            string IDNumber = null;
            if (LE.Count > 0)
                IDNumber = LE[0].LegalNumber;

            try
            {
                dsProperties = AVM.ReturnProperties(node.GenericKey, node.GenericKeyTypeKey, null, IDNumber, null, null, null, null, null, null, null);

                if (dsProperties.Rows.Count > 0)
                {
                    PrivateCacheData.Remove("LightstoneProperties");
                    PrivateCacheData.Add("LightstoneProperties", dsProperties);
                    retval = true;
                }
            }
            catch (Exception ex)
            {
                retval = false;
                View.Messages.Add(new Warning("The LightStone web service call failed with error: " + ex.Message + "\n The Service is Offline - Please try again later.", ex.Message));
            }

            return retval;
        }

        /// <summary>
        ///  creates a New Lightstone Valuation for the property
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        /// <param name="LightstonepropertyID"></param>
        private bool CreateNewLightStoneValuation(IProperty prop, string LightstonepropertyID)
        {
            bool retval = false;

            try
            {
                //string LightstonepropertyGUID = AVM.ConfirmProperty(node.GenericKey, node.GenericKeyTypeKey, Convert.ToInt32(LightstonepropertyID));
                //DataSet dsNewValuation = AVM.ReturnValuation(node.GenericKey, node.GenericKeyTypeKey, LightstonepropertyGUID, 1, 1, false);

                DataSet dsNewValuation = AVM.ReturnValuation(node.GenericKey, node.GenericKeyTypeKey, Convert.ToInt32(LightstonepropertyID), 1, 1, false);
                AddNewValuationRequest(prop, dsNewValuation);
                retval = true;
            }
            catch (Exception ex)
            {
                View.Messages.Add(new Warning("The LightStone Valuation Service failed with error: " + ex.Message + "\n", ex.Message));
            }
            
            return retval;
        }

        private void AddNewValuationRequest(IProperty prop, DataSet dsNewValuation)
        {
            ExclusionSets.Add(RuleExclusionSets.ValuationUpdatePreviousToInactive);
            IValuationDiscriminatedLightstoneAVM val = PropertyRepository.CreateEmptyValuation(ValuationDataProviderDataServices.LightstoneAutomatedValuation) as IValuationDiscriminatedLightstoneAVM;
            if (val != null)
            {
                val.Data = dsNewValuation.GetXml();
                val.IsActive = false;
                val.Property = prop;

                if (dsNewValuation.Tables.Contains("Valuation") && dsNewValuation.Tables["Valuation"].Rows.Count > 0)
                    val.ValuationAmount = Convert.ToDouble(dsNewValuation.Tables["Valuation"].Rows[0]["PredictedPrice"]);
                else
                    val.ValuationAmount = 0;

                //val.ValuationDataProviderDataService; //this is the discriminator value
                val.ValuationDate = DateTime.Now;
                val.ValuationEscalationPercentage = 0;
                val.ValuationHOCValue = val.ValuationAmount;
                val.ValuationMunicipal = 0;
                val.ValuationStatus = LookupRepository.ValuationStatus.ObjectDictionary[((int)ValuationStatuses.Complete).ToString()];
                val.ValuationUserID = View.CurrentPrincipal.Identity.Name;
                val.Valuator = PropertyRepository.GetValuatorByKey(50);

                TransactionScope txn = new TransactionScope();
                try
                {
                    // Save all of the objects to the Database
                    PropertyRepository.SaveValuation(val);
                    txn.VoteCommit();
                    ExclusionSets.Remove(RuleExclusionSets.ValuationUpdatePreviousToInactive);
                }

                catch (Exception)
                {
                    txn.VoteRollBack();
                    if (_view.IsValid)
                        throw;
                }

                finally
                {
                    txn.Dispose();
                }

            }

        }

        void PopulateValuationsDS(IEventList<IValuation> Valuations)
        {
            // Setup the Valuations Table 
            dsValuations.Reset();
            dsValuations.Columns.Add("Key");
            dsValuations.Columns.Add("DataServiceProviderKey");
            dsValuations.Columns.Add("Valuer");
            dsValuations.Columns.Add("ValuationDate");
            dsValuations.Columns.Add("ValuationAmount");
            dsValuations.Columns.Add("HOCValuation");
            dsValuations.Columns.Add("ValuationPurpose");
            dsValuations.Columns.Add("RequestedBy");
            dsValuations.Columns.Add("ValuationType");
            dsValuations.Columns.Add("XMLData");
            dsValuations.TableName = "Valuations";

            if (Valuations.Count > 0)
                for (int i = 0; i < Valuations.Count; i++)
                {
                    DataRow valRow = dsValuations.NewRow();
                    valRow["Key"] = Valuations[i].Key;
                    valRow["DataServiceProviderKey"] = Valuations[i].ValuationDataProviderDataService.DataProviderDataService.Key;
                    valRow["Valuer"] = Valuations[i].Valuator.LegalEntity.DisplayName;
                    valRow["ValuationDate"] = Convert.ToString(Valuations[i].ValuationDate.ToShortDateString());
                    valRow["ValuationAmount"] = Convert.ToString(Valuations[i].ValuationAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat));
                    valRow["HOCValuation"] = Convert.ToString(Valuations[i].ValuationHOCValue.Value.ToString(SAHL.Common.Constants.CurrencyFormat));
                    valRow["ValuationPurpose"] = "";
                    valRow["RequestedBy"] = Valuations[i].ValuationUserID;
                    valRow["ValuationType"] = Valuations[i].ValuationDataProviderDataService.DataProviderDataService.DataProvider.Description;
                    valRow["XMLData"] = Valuations[i].Data;
                    dsValuations.Rows.Add(valRow);
                }

            GlobalCacheData.Remove("ValuationDS");
            GlobalCacheData.Add("ValuationDS", dsValuations, new List<ICacheObjectLifeTime>());
        }

    }
}

