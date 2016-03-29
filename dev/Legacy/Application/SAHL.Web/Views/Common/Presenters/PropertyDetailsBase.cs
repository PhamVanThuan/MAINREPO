using System;
using System.Data;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI.Events;
using System.Diagnostics.CodeAnalysis;

using SAHL.Common.UI;
using SAHL.Common.CacheData;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "No point making this Disposable - it's life is handled by UIP.")]
    public class PropertyDetailsBase : SAHLCommonBasePresenter<IPropertyDetails>
    {
        private int _genericKey, _genericKeyTypeKey;
        private CBOMenuNode _selectedNode;
        private IApplicationRepository _applicationRepo;
        private IAccountRepository _accountRepo;
        private IPropertyRepository _propertyRepo;
        private ILookupRepository _lookupRepo;
        private IList<IProperty> _lstProperties;
        private IEventList<IAddress> _lstPropertyAddresses;
        private IApplication _application;
        private IAccount _account;
        private IProperty _property;
        private IApplicationMortgageLoan _applicationMortgageLoan;
        private IMortgageLoanAccount _mortgageLoanAccount;
        private DataTable dtOwnerDetails, dtBondDetails;
        private string _currentDataProvider;

        private int _deedsOfficeKey;

        public int DeedsOfficeKey
        {
            get { return _deedsOfficeKey; }
            set { _deedsOfficeKey = value; }
        }

        private string _bondAccountNumber;

        public string BondAccountNumber
        {
            get { return _bondAccountNumber; }
            set { _bondAccountNumber = value; }
        }

        private string _lightStonePropertyID;

        public string LightStonePropertyID
        {
            get { return _lightStonePropertyID; }
            set { _lightStonePropertyID = value; }
        }

        private string _adCheckPropertyID;

        public string ADCheckPropertyID
        {
            get { return _adCheckPropertyID; }
            set { _adCheckPropertyID = value; }
        }

        public IPropertyRepository PropertyRepo
        {
            get { return _propertyRepo; }
            set { _propertyRepo = value; }
        }

        public ILookupRepository LookupRepo
        {
            get { return _lookupRepo; }
            set { _lookupRepo = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public PropertyDetailsBase(IPropertyDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {

            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            // Get the CBO Node    
            _selectedNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_selectedNode == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            _genericKey = _selectedNode.GenericKey;
            _genericKeyTypeKey = _selectedNode.GenericKeyTypeKey;

            _applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            _accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            _propertyRepo = RepositoryFactory.GetRepository<IPropertyRepository>();
            _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

            // setup events
            _view.OnPropertyAddressGridSelectedIndexChanged += (OnPropertyAddressGridSelectedIndexChanged);
            _view.OnCancelButtonClicked += (OnCancelButtonClicked);

            _lstProperties = new List<IProperty>();
            _lstPropertyAddresses = new EventList<IAddress>();

            // check the node type and get the relevant objects
            switch (_genericKeyTypeKey)
            {
                case (int)GenericKeyTypes.Account:
                    // get the account
                    _account = _accountRepo.GetAccountByKey(_genericKey);
                    if (_account != null && _account is IMortgageLoanAccount)
                    {
                        _mortgageLoanAccount = _account as IMortgageLoanAccount;
                        if (_mortgageLoanAccount.SecuredMortgageLoan.Property != null)
                            _lstProperties.Add(_mortgageLoanAccount.SecuredMortgageLoan.Property);
                    }
                    break;
                case (int)GenericKeyTypes.Offer:
                    // get the application
                    _application = _applicationRepo.GetApplicationByKey(_genericKey);
                    if (_application != null && _application is IApplicationMortgageLoan)
                    {
                        _applicationMortgageLoan = _application as IApplicationMortgageLoan;
                        if (_applicationMortgageLoan.Property != null)
                            _lstProperties.Add(_applicationMortgageLoan.Property);
                    }
                    break;
                case (int)GenericKeyTypes.Property:
                    // get the property
                    _property = _propertyRepo.GetPropertyByKey(_genericKey);
                    if (_property != null)
                        _lstProperties.Add(_property);
                    // retrieve the application the property is linked to
                    if (_selectedNode.ParentNode.GenericKeyTypeKey == (int)GenericKeyTypes.Offer)
                        _application = _applicationRepo.GetApplicationByKey(_selectedNode.ParentNode.GenericKey);
                    break;
                default:
                    break;
            }

            if (_lstProperties.Count > 0 && _lstProperties[0].Address != null)
                _lstPropertyAddresses.Add(_view.Messages, _lstProperties[0].Address);

            // bind the property addresses
            _view.BindPropertyAddressGrid(_lstPropertyAddresses);

            // bind the property details
            if (_lstProperties.Count > 0)
            {
                GetLatestPropertyData(_lstProperties[0]);
                _view.BindPropertyDetails(_lstProperties[0], _bondAccountNumber, _deedsOfficeKey, _lightStonePropertyID, _adCheckPropertyID, _currentDataProvider);

                // bind the deeds transfers
                _view.BindPropertyOwnersGrid(dtOwnerDetails);

                // bind the bond registrations
                _view.BindBondRegistrationsGrid(dtBondDetails);
            }
            else
            {
                _view.BindPropertyDetails(null, "", 0, "", "", "");
                _view.BindPropertyOwnersGrid(null);
                _view.BindBondRegistrationsGrid(null);
            }

        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;
        }

        /// <summary>
        /// Handles the event fired by the view when the Property Summary Grid Index is changed
        /// </summary>
        void OnPropertyAddressGridSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            for (int x = 0; x < _lstProperties.Count; x++)
            {
                if (_lstProperties[x].Key == int.Parse(e.Key.ToString()))
                {
                    GetLatestPropertyData(_lstProperties[x]);
                    _view.BindPropertyDetails(_lstProperties[x], _bondAccountNumber, _deedsOfficeKey, _lightStonePropertyID, _adCheckPropertyID, _currentDataProvider);
                    _view.BindPropertyOwnersGrid(dtOwnerDetails);
                    _view.BindBondRegistrationsGrid(dtBondDetails);
                    break;
                }
            }
        }

        void OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Cancel");
        }

        private void GetLatestPropertyData(IProperty property)
        {
            // get bond account number & deeds office key from property data
            _bondAccountNumber = "";
            _lightStonePropertyID = "";
            _adCheckPropertyID = "";
            _deedsOfficeKey = 0;
            _currentDataProvider = String.Empty;

            PropertyDataProviderDataServices enPropertyServiceProvider = PropertyDataProviderDataServices.SAHLPropertyManualValuation;

            // get the bond account number and deeds office from the latest property data record
            IPropertyData latestPropertyData;
            dtOwnerDetails = new DataTable();
            dtBondDetails = new DataTable();
            if (property.DataProvider != null)
            {               
                _currentDataProvider = property.DataProvider.Description;

                
                //Always show property details of latest Lightstone property
                enPropertyServiceProvider = PropertyDataProviderDataServices.LightstonePropertyIdentification;
                

                latestPropertyData = _propertyRepo.GetLatestPropertyData(property, enPropertyServiceProvider);
                DataSet dsPropertyData = null;

                if (latestPropertyData != null)
                    dsPropertyData = _propertyRepo.GetDataSetFromXML(latestPropertyData.Data);

                if (dsPropertyData != null && dsPropertyData.Tables.Count > 0)
                {
                    // get deeds data
                  
                    // check if there is a Transfers table
                    if (dsPropertyData.Tables[SAHL.Common.Constants.LightStone.TableNames.Transfers] != null)
                    {
                        DataTable dtOwnerAndBondDetails = dsPropertyData.Tables[SAHL.Common.Constants.LightStone.TableNames.Transfers].Copy();

                        #region Property Owner Details - sorted by descending RegistrationDate
                        dtOwnerDetails = dsPropertyData.Tables[SAHL.Common.Constants.LightStone.TableNames.Transfers].Clone();

                        // sort the datatable by RegistrationDate descending
                        DataRow[] sortedOwnerDetailRows = dtOwnerAndBondDetails.Select(null, SAHL.Common.Constants.LightStone.TransfersTable.RegistrationDate + " DESC");

                        DataTable dtOwnerDetailSorted = dtOwnerAndBondDetails.Copy();
                        // copy the sorted rows into a temp table
                        dtOwnerDetailSorted.Rows.Clear();
                        foreach (DataRow dr in sortedOwnerDetailRows)
                        {
                            dtOwnerDetailSorted.ImportRow(dr);
                        }

                        string tempDate = "";
                        int lastAddedRowIdx = -1;

                        foreach (DataRow dr in dtOwnerDetailSorted.Rows)
                        {
                            if ((FieldDataExists(dr, SAHL.Common.Constants.LightStone.TransfersTable.BuyersName) && dr[SAHL.Common.Constants.LightStone.TransfersTable.BuyersName].ToString().Length > 0)
                                || (FieldDataExists(dr, SAHL.Common.Constants.LightStone.TransfersTable.SellersName) && dr[SAHL.Common.Constants.LightStone.TransfersTable.SellersName].ToString().Length > 0))
                            {
                                // combine multiple applicant rows - grouping on RegistrationDate
                                if (FieldDataExists(dr, SAHL.Common.Constants.LightStone.TransfersTable.RegistrationDate))
                                {
                                    if (dr[SAHL.Common.Constants.LightStone.TransfersTable.RegistrationDate].ToString().CompareTo(tempDate) != 0)
                                    {
                                        // if there is a change in registration date - add a new row
                                        dtOwnerDetails.ImportRow(dr);
                                        lastAddedRowIdx++;
                                        tempDate = dr[SAHL.Common.Constants.LightStone.TransfersTable.RegistrationDate].ToString();
                                    }
                                    else
                                    {
                                        // if RegistrationDate is the same then append row 
                                        DataRow appendRow = dtOwnerDetails.Rows[lastAddedRowIdx];

                                        if (dtOwnerDetails.Columns.Contains(SAHL.Common.Constants.LightStone.TransfersTable.BuyersName))
                                        {
                                            if (!String.IsNullOrEmpty(dr[SAHL.Common.Constants.LightStone.TransfersTable.BuyersName].ToString()) && dr[SAHL.Common.Constants.LightStone.TransfersTable.BuyersName].ToString() != "&nbsp;")
                                            {
                                                // combine mulitple buyers
                                                if (!String.IsNullOrEmpty(appendRow[SAHL.Common.Constants.LightStone.TransfersTable.BuyersName].ToString()))
                                                    appendRow[SAHL.Common.Constants.LightStone.TransfersTable.BuyersName] = appendRow[SAHL.Common.Constants.LightStone.TransfersTable.BuyersName] + " & " + dr[SAHL.Common.Constants.LightStone.TransfersTable.BuyersName];
                                                else
                                                    appendRow[SAHL.Common.Constants.LightStone.TransfersTable.BuyersName] = dr[SAHL.Common.Constants.LightStone.TransfersTable.BuyersName].ToString();
                                            }
                                        }
                                        if (dtOwnerDetails.Columns.Contains(SAHL.Common.Constants.LightStone.TransfersTable.SellersName))
                                        {
                                            if (!String.IsNullOrEmpty(dr[SAHL.Common.Constants.LightStone.TransfersTable.SellersName].ToString()) && dr[SAHL.Common.Constants.LightStone.TransfersTable.SellersName].ToString() != "&nbsp;")
                                            {
                                                // combine mulitple sellers
                                                if (!String.IsNullOrEmpty(appendRow[SAHL.Common.Constants.LightStone.TransfersTable.SellersName].ToString()))
                                                    appendRow[SAHL.Common.Constants.LightStone.TransfersTable.SellersName] = appendRow[SAHL.Common.Constants.LightStone.TransfersTable.SellersName] + " & " + dr[SAHL.Common.Constants.LightStone.TransfersTable.SellersName];
                                                else
                                                    appendRow[SAHL.Common.Constants.LightStone.TransfersTable.SellersName] = dr[SAHL.Common.Constants.LightStone.TransfersTable.SellersName].ToString();
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        #endregion


                        #region Bond Details - sorted by descending Registration Date

                        dtBondDetails = dsPropertyData.Tables[SAHL.Common.Constants.LightStone.TableNames.Transfers].Clone();

                        // sort the datatable by registration date
                        DataRow[] sortedBondDetailsRows = dtOwnerAndBondDetails.Select(null, SAHL.Common.Constants.LightStone.TransfersTable.RegistrationDate + " DESC");

                        DataTable dtBondDetailsSorted = dtOwnerAndBondDetails.Copy();
                        // copy the sorted rows into a temp table
                        dtBondDetailsSorted.Rows.Clear();
                        foreach (DataRow dr in sortedBondDetailsRows)
                        {
                            dtBondDetailsSorted.ImportRow(dr);
                        }

                        // load the transfers & registrations into the relevant tables
                        tempDate = "";
                        string tempBondNumber = "";
                        lastAddedRowIdx = -1;

                        foreach (DataRow dr in dtBondDetailsSorted.Rows)
                        {
                            if ((FieldDataExists(dr, SAHL.Common.Constants.LightStone.TransfersTable.BondNumber) && dr[SAHL.Common.Constants.LightStone.TransfersTable.BondNumber].ToString().Length > 0))
                            {
                                // combine bond rows - grouping on registration date & bond number
                                if (FieldDataExists(dr, SAHL.Common.Constants.LightStone.TransfersTable.RegistrationDate))
                                {
                                    if (dr[SAHL.Common.Constants.LightStone.TransfersTable.RegistrationDate].ToString().CompareTo(tempDate) != 0
                                        || dr[SAHL.Common.Constants.LightStone.TransfersTable.BondNumber].ToString().CompareTo(tempBondNumber) != 0)
                                    {
                                        // if there is a change in registration date or bond number - add a new row
                                        dtBondDetails.ImportRow(dr);
                                        lastAddedRowIdx++;
                                        tempDate = dr[SAHL.Common.Constants.LightStone.TransfersTable.RegistrationDate].ToString();
                                        tempBondNumber = dr[SAHL.Common.Constants.LightStone.TransfersTable.BondNumber].ToString();
                                    }
                                }
                            }
                        }

                        #endregion
                    }
                   

                    // get bond account number and deeds office
                    if (property.DataProvider.Key == (int)DataProviders.SAHL)
                    {
                        if (dsPropertyData.Tables[0].Rows.Count > 0)
                        {
                            if (dsPropertyData.Tables[0].Columns.Contains("BondAccountNumber"))
                                _bondAccountNumber = dsPropertyData.Tables[0].Rows[0]["BondAccountNumber"].ToString();
                            if (dsPropertyData.Tables[0].Columns.Contains("DeedsOfficeKey"))
                            {
                                string officeKey = dsPropertyData.Tables[0].Rows[0]["DeedsOfficeKey"].ToString();
                                _deedsOfficeKey = String.IsNullOrEmpty(officeKey) ? 0 : Convert.ToInt32(officeKey);
                            }
                        }
                    }
                }

                //if (property.DataProvider.Key == (int)DataProviders.LightStone || property.DataProvider.Key == (int)DataProviders.AdCheck)
                //{
                //    // check for lightstone propertydata - only show LightStonePropertyID if the DataProvider is lightstone or adcheck
                //    latestPropertyData = _propertyRepo.GetLatestPropertyData(property, PropertyDataProviderDataServices.LightstonePropertyIdentification);
                //    _lightStonePropertyID = latestPropertyData == null ? "" : latestPropertyData.PropertyID;
                //}

                // #15155 - Always show the _lightStonePropertyID
                    latestPropertyData = _propertyRepo.GetLatestPropertyData(property, PropertyDataProviderDataServices.LightstonePropertyIdentification);
                    _lightStonePropertyID = latestPropertyData == null ? "" : latestPropertyData.PropertyID;



                // check for adcheck propertydata - always show ADCheckPropertyID if there is one
                latestPropertyData = _propertyRepo.GetLatestPropertyData(property, PropertyDataProviderDataServices.AdCheckPropertyIdentification);
                _adCheckPropertyID = latestPropertyData == null ? "" : latestPropertyData.PropertyID;
            }
        }

        /// <summary>
        /// Checks if a Field Exists in a dataset
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static bool FieldDataExists(DataRow dataRow, string fieldName)
        {
            try
            {
                if (dataRow[fieldName] == null) return false;
                return dataRow.GetColumnError(fieldName).Length <= 0;
            }
            catch (Exception)
            {
                // This will happen if the field does not exist...
                return false;
            }

        }


        protected bool ValidateOnViewInitialised()
        {
            // dont so the validation on the PropertyDetailsUpdateContacts presenter
            if (String.Compare(_view.ViewName,"App_PropertyDetailsUpdateContacts",true)==0)
                return true;

            if (_lstProperties != null && _lstProperties.Count > 0 && _application != null)
            {
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
                IRuleService svc = ServiceFactory.GetService<IRuleService>();
                svc.ExecuteRule(spc.DomainMessages, "PropertyNoUpdateOnOpenLoan", _lstProperties[0], _application);
            }

            if (_view.Messages.Count == 0)
                return true;
            else
                return false;
        }
    }
}
