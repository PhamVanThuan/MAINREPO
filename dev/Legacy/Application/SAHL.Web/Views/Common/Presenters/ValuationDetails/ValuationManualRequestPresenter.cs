using System;
using System.Collections.Generic;
using System.Data;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.ValuationDetails
{
    public class ValuationManualRequestPresenter : SAHLCommonBasePresenter<IValuationDetailsView>
    {
        private IAddressRepository addressRepository;
        private IPropertyRepository propertyRepository;
        private IApplication offer;
        private IApplicationRepository applicationRepository;
        private DataTable dsValuations = new DataTable();
        private readonly DataTable dsAddress = new DataTable();
        //private ILookupRepository lookupRepository;
        private InstanceNode instanceNode;
        private CBOMenuNode node;
        //private IEventList<IProperty> propertylist;

        private IProperty property;
        private IReadOnlyEventList<IValuation> valuations;
        private int requiredKey;
        private int propertyKey;

        protected IAddressRepository AddressRepository
        {
            get
            {
                if (addressRepository == null)
                    addressRepository = RepositoryFactory.GetRepository<IAddressRepository>();
                return addressRepository;
            }
        }

        protected IPropertyRepository PropertyRepository
        {
            get
            {
                if (propertyRepository == null)
                    propertyRepository = RepositoryFactory.GetRepository<IPropertyRepository>();
                return propertyRepository;
            }
        }

        public ValuationManualRequestPresenter(IValuationDetailsView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            node = CBOManager.GetCurrentCBONode(View.CurrentPrincipal) as CBOMenuNode;

            //Set up the Interface
            View.ShowbtnViewDetail = true;
            View.ShowbtnSubmit = true;
            View.ShowgrdValuations = true;
            View.ShowpnlValuations = true;
            View.ShowbtnViewDetail = false;
            View.ShowpnlInspectionContactDetails = true;
            // set up the eventhandlers
            View.btnSubmitClicked += btnSubmitClicked;

            if (!PrivateCacheData.ContainsKey("PropertiesIndex"))
                PrivateCacheData.Add("PropertiesIndex", 0);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;
            // Get the CBO Node
            //int instanceID;

            //Get the AccountKey/OfferKey from the CBO
            if (node is InstanceNode)
            {
                instanceNode = node as InstanceNode;
                requiredKey = Convert.ToInt32(instanceNode.GenericKey);
            }
            else
            {
                requiredKey = Convert.ToInt32(node.GenericKey);
            }

            if (node.GenericKeyTypeKey == (int)GenericKeyTypes.Account)
            {
                IMortgageLoanRepository mlRepo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
                IMortgageLoan mla = mlRepo.GetMortgageloanByAccountKey(requiredKey);
                propertyKey = mla.Property.Key;
            }
            else // node.GenericKeyTypeKey == 2  Offer
                propertyKey = propertyRepository.GetPropertyKeyByOfferKey(requiredKey);

            //requiredKey = 691814; // OfferKey

            applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();
            propertyRepository = RepositoryFactory.GetRepository<IPropertyRepository>();

            offer = applicationRepository.GetApplicationByKey(requiredKey);

            GlobalCacheData.Remove("propertyKey");
            GlobalCacheData.Add("propertyKey", propertyKey, new List<ICacheObjectLifeTime>());

            property = propertyRepository.GetPropertyByKey(propertyKey);
            valuations = propertyRepository.GetValuationByPropertyKey(propertyKey);
            if (property.Address != null)
                if (PrivateCacheData.ContainsKey("AddressDS"))
                    View.BindPropertyGrid(PrivateCacheData["AddressDS"] as DataTable);
                else
                    View.BindPropertyGrid((GetPropertyDT(property.Address)));

            if (GlobalCacheData.ContainsKey("ValuationDS"))
                dsValuations = GlobalCacheData["ValuationDS"] as DataTable;
            else
                PopulateValuationsDS(valuations);

            View.BindgrdValuations(dsValuations);
            SetupPropertyAccessDetails();
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

        //void PropertiesGridSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        //{
        //    PrivateCacheData.Remove("PropertiesIndex");
        //    PrivateCacheData.Add("PropertiesIndex", View.PropertyItemIndex);
        //}

        private void btnSubmitClicked(object sender, EventArgs e)
        {
            //Do Checks on fields (Business Rules)
            string ReturnMessage = "";
            View.ShowlblErrorMessage = false;

            if (CheckContactEntries(ref ReturnMessage))
            {
                GlobalCacheData.Remove("Contact2Details");
                //Store Information in PropertyAccessDetails
                TransactionScope txn = new TransactionScope();
                try
                {
                    // Save all of the objects to the Database
                    AddPropertyAccessDetails();
                    txn.VoteCommit();
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

                //Create a Valuations Record (For Adcheck valuation - mark status pending)
                AddValuationsRecord();

                //Create Workflow Case in ValualtionsWorkflow pointing to valuations record created above

                //Navigate to ValuationDetailsViewPresenter Screen - Showing Pending (Add Status to Grid)
                View.Navigator.Navigate("ValuationDetailsView");
            }
            else
            {
                // Setup the error messages and return
                View.SetlblErrorMessage = ReturnMessage;
                View.ShowlblErrorMessage = true;
            }
        }

        private void AddValuationsRecord()
        {
            propertyKey = Convert.ToInt32(GlobalCacheData["propertyKey"]);
            IPropertyRepository propertyRep = RepositoryFactory.GetRepository<IPropertyRepository>();
            ILookupRepository lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
            IProperty prop = propertyRep.GetPropertyByKey(propertyKey);
            IValuationDiscriminatedSAHLManual valuation = propertyRep.CreateEmptyValuation(ValuationDataProviderDataServices.SAHLManualValuation) as IValuationDiscriminatedSAHLManual;

            if (valuation != null)
            {
                valuation.Property = prop;
                valuation.IsActive = false;
                valuation.ValuationStatus = lookupRepository.ValuationStatus.ObjectDictionary[((int)ValuationStatuses.Pending).ToString()];
                valuation.ValuationUserID = View.CurrentPrincipal.Identity.Name;
                // already wrapped in a transaction
                propertyRep.SaveValuation(valuation);
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void SetupPropertyAccessDetails()
        {
            if (!GlobalCacheData.ContainsKey("propertyAccessdetails"))
            {
                propertyKey = Convert.ToInt32(GlobalCacheData["propertyKey"]);
                IEventList<IPropertyAccessDetails> pad = PropertyRepository.GetPropertyAccesDetailsByPropertyKey(propertyKey);

                if (pad.Count > 0)
                {
                    GlobalCacheData.Remove("propertyAccessdetails");
                    GlobalCacheData.Add("propertyAccessdetails", pad[0], new List<ICacheObjectLifeTime>());

                    // there is already a contact record... populate the view with it..
                    View.SettxtContact1Name = pad[0].Contact1;
                    View.SettxtContact1MobilePhone = pad[0].Contact1MobilePhone;
                    View.SettxtContact1Phone = pad[0].Contact1Phone;
                    View.SettxtContact1WorkPhone = pad[0].Contact1WorkPhone;
                    View.SettxtContact2Name = pad[0].Contact2;
                    View.SettxtContact2Phone = pad[0].Contact2Phone;
                }
            }
        }

        private void AddPropertyAccessDetails()
        {
            propertyKey = Convert.ToInt32(GlobalCacheData["propertyKey"]);
            IPropertyRepository propertyRep = RepositoryFactory.GetRepository<IPropertyRepository>();
            IProperty prop = propertyRep.GetPropertyByKey(propertyKey);
            IPropertyAccessDetails propertyAccessdetails = PropertyRepository.CreateEmptyPropertyAccessDetails();

            if (GlobalCacheData.ContainsKey("propertyAccessdetails"))
            {
                IEventList<IPropertyAccessDetails> PAD = PropertyRepository.GetPropertyAccesDetailsByPropertyKey(propertyKey);
                propertyAccessdetails = PAD[0];
            }
            else
                propertyAccessdetails.Property = prop;

            propertyAccessdetails.Contact1 = View.SettxtContact1Name;
            propertyAccessdetails.Contact1MobilePhone = View.SettxtContact1MobilePhone;
            propertyAccessdetails.Contact1Phone = View.SettxtContact1Phone;
            propertyAccessdetails.Contact1WorkPhone = View.SettxtContact1WorkPhone;
            propertyAccessdetails.Contact2 = View.SettxtContact2Name;
            propertyAccessdetails.Contact2Phone = View.SettxtContact2Phone;

            PropertyRepository.SavePropertyAccessDetails(propertyAccessdetails);
        }

        private bool CheckContactEntries(ref string Inputstring)
        {
            bool retval = true;
            if (View.SettxtContact1Name.Length == 0)
            {
                Inputstring += "Contact 1 Name is required. ";
                retval = false;
            }
            if (View.SettxtContact1Phone.Length == 0 & View.SettxtContact1WorkPhone.Length == 0 & View.SettxtContact1MobilePhone.Length == 0)
            {
                Inputstring += "At least one of Contact 1's Home phone number, Work phone number or Cell phone number must be provided. ";
                retval = false;
            }
            if (retval)
                if (!GlobalCacheData.ContainsKey("Contact2Details"))
                {
                    // If it contains the key, its already checked once
                    if (View.SettxtContact2Name.Length == 0)
                        Inputstring += "If you have a second contact, please add them. If not, ignore this message and click Instruct Valuer again.";
                    GlobalCacheData.Add("Contact2Details", true, new List<ICacheObjectLifeTime>());
                    retval = false;
                }
            return retval;
        }

        private void PopulateValuationsDS(IReadOnlyEventList<IValuation> Valuations)
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
                    valRow["ValuationPurpose"] = offer.ApplicationType.Description;
                    valRow["RequestedBy"] = Valuations[i].ValuationUserID;
                    valRow["ValuationType"] = Valuations[i].ValuationDataProviderDataService.DataProviderDataService.DataProvider.Description;
                    valRow["XMLData"] = Valuations[i].Data;
                    dsValuations.Rows.Add(valRow);
                }

            GlobalCacheData.Remove("ValuationDS");
            GlobalCacheData.Add("ValuationDS", dsValuations, new List<ICacheObjectLifeTime>());
        }

        /*private static int GetPropertyKey(IEnumerable<IProperty> proplist)
        {
            // This is a hack. But there will currently only be one property per account
            int retval = 0;
            foreach (IProperty prop in proplist)
            {
                retval = prop.Key;
            }
            return retval;
        }*/
    }
}