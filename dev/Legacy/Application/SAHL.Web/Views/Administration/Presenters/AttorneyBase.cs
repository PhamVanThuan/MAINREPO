using System;
using System.Collections.Generic;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.CacheData;

namespace SAHL.Web.Views.Administration.Presenters
{
	/// <summary>
	/// Attorney Base
	/// </summary>
	public class AttorneyBase : SAHLCommonBasePresenter<SAHL.Web.Views.Administration.Interfaces.IAttorney>
	{
		protected bool _Adding;
		protected bool _Editing;
		protected IAttorney _attorney;
		protected ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
		protected IAddressRepository addressRepo = RepositoryFactory.GetRepository<IAddressRepository>();
        private List<ICacheObjectLifeTime> _lifeTimes;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
		public AttorneyBase(SAHL.Web.Views.Administration.Interfaces.IAttorney view, SAHLCommonBaseController controller)
			: base(view, controller)
		{
			_view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
			_view.OnDeedsOfficeSelectedIndexChanged += new KeyChangedEventHandler(_view_OnDeedsOfficeSelectedIndexChanged);
			_view.OnAttorneySelectedIndexChanged += new KeyChangedEventHandler(_view_OnAttorneySelectedIndexChanged);
			_view.OnAddressTypeSelectedIndexChanged += new KeyChangedEventHandler(_view_OnAddressTypeSelectedIndexChanged);
			_view.OnAddressFormatSelectedIndexChanged += new KeyChangedEventHandler(_view_OnAddressFormatSelectedIndexChanged);
			_view.SelectAddressButtonClicked += new KeyChangedEventHandler(_view_SelectAddressButtonClicked);
		}

		#region Events

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void OnViewInitialised(object sender, EventArgs e)
		{
			base.OnViewInitialised(sender, e);
			if (!_view.ShouldRunPage) return;

			BindDeedsOfficeDropDown();
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

			if (_view.IsPostBack)
			{
				if (GlobalCacheData.ContainsKey("AddressKey"))
				{
					IAddress addressSelected = addressRepo.GetAddressByKey((int)GlobalCacheData["AddressKey"]);
					this.GlobalCacheData.Remove("AddressKey");
					if (addressSelected != null)
						_view.BindAttorneyAddress(addressSelected);
				}
			}
			else
			{
				_view.Set_pnlAttorneyDetailsVisibility = false;
				_view.Set_pnlAddressVisibility = false;
			}
		}

		protected void _view_OnDeedsOfficeSelectedIndexChanged(object sender, EventArgs e)
		{
			if (_view.GetSet_ddlDeedsOffice != -1)
			{
				if (this.GlobalCacheData.ContainsKey("DeedsOfficeKey"))
				{
					if (this.GlobalCacheData["DeedsOfficeKey"].ToString() != _view.GetSet_ddlDeedsOffice.ToString())
					{
						this.GlobalCacheData["DeedsOfficeKey"] = _view.GetSet_ddlDeedsOffice;
						if (this.GlobalCacheData.ContainsKey("AttorneyKey"))
							this.GlobalCacheData.Remove("AttorneyKey");
						BindAttorneyDropDown((int)this.GlobalCacheData["DeedsOfficeKey"]);
						ClearAttorneyControls();
					}
				}
				else
				{
					this.GlobalCacheData.Add("DeedsOfficeKey", _view.GetSet_ddlDeedsOffice,LifeTimes);
                    
				}
			}
		}

		protected void _view_OnAttorneySelectedIndexChanged(object sender, KeyChangedEventArgs e)
		{
			if (_view.GetSet_ddlAttorney != -1)
			{
				if (this.GlobalCacheData.ContainsKey("AttorneyKey"))
				{
					if (this.GlobalCacheData["AttorneyKey"].ToString() != _view.GetSet_ddlAttorney.ToString())
					{
						this.GlobalCacheData["AttorneyKey"] = _view.GetSet_ddlAttorney;
						PopulateAttorneyControls((int)this.GlobalCacheData["AttorneyKey"]);
					}
				}
				else
				{
                    this.GlobalCacheData.Add("AttorneyKey", _view.GetSet_ddlAttorney, LifeTimes);
					PopulateAttorneyControls((int)this.GlobalCacheData["AttorneyKey"]);
				}
			}
		}

		protected void _view_OnAddressTypeSelectedIndexChanged(object sender, KeyChangedEventArgs e)
		{
			if (_view.Get_ddlAddressTypeSelectedValue != -1)
			{
				if (this.GlobalCacheData.ContainsKey("AddressTypeKey"))
				{
					if (this.GlobalCacheData["AddressTypeKey"].ToString() != _view.Get_ddlAddressTypeSelectedValue.ToString())
					{
						this.GlobalCacheData["AddressTypeKey"] = _view.Get_ddlAddressTypeSelectedValue;
						if (this.GlobalCacheData.ContainsKey("AddressFormatKey"))
							this.GlobalCacheData.Remove("AddressFormatKey");
						BindAddressFormat((int)this.GlobalCacheData["AddressTypeKey"]);
					}
				}
				else
				{
                    this.GlobalCacheData.Add("AddressTypeKey", _view.Get_ddlAddressTypeSelectedValue, LifeTimes);
				}
			}
		}

		protected void _view_OnAddressFormatSelectedIndexChanged(object sender, KeyChangedEventArgs e)
		{
			if (_view.Get_ddlAddressFormatSelectedValue != -1)
			{
				if (this.GlobalCacheData.ContainsKey("AddressFormatKey"))
				{
					if (this.GlobalCacheData["AddressFormatKey"].ToString() != _view.Get_ddlAddressFormatSelectedValue.ToString())
					{
						this.GlobalCacheData["AddressFormatKey"] = _view.Get_ddlAddressFormatSelectedValue;
					}
				}
				else
				{
                    this.GlobalCacheData.Add("AddressFormatKey", _view.Get_ddlAddressFormatSelectedValue, LifeTimes);
				}
			}
		}

		protected void _view_OnCancelButtonClicked(object sender, EventArgs e)
		{
			Navigator.Navigate("AttorneyDetails");
		}

		protected void _view_SelectAddressButtonClicked(object sender, KeyChangedEventArgs e)
		{
			if (this.GlobalCacheData.ContainsKey("AddressKey"))
				this.GlobalCacheData["AddressKey"] = Convert.ToInt32(e.Key);
			else
                this.GlobalCacheData.Add("AddressKey", Convert.ToInt32(e.Key), LifeTimes);
		}

		#endregion

		#region Methods

		/// <summary>
		/// 
		/// </summary>
		protected void BindDeedsOfficeDropDown()
		{
			_view.BindDeedsOfficeDropDown(lookupRepo.DeedsOffice);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="selectedDeedsOfficeKey"></param>
		protected void BindAttorneyDropDown(int selectedDeedsOfficeKey)
		{
			ILegalEntityRepository legalEntityRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
			_view.BindAttorneyDropDown(legalEntityRepo.GetAttorneysByDeedsOffice(selectedDeedsOfficeKey));
		}

		/// <summary>
		/// 
		/// </summary>
		public void BindAddressType()
		{
			_view.BindAddressTypeDropDown(lookupRepo.AddressTypes);
		}

		/// <summary>
		/// 
		/// </summary>
		public void BindAddressFormat(int selectedAddressTypeKey)
		{
			_view.BindAddressFormatDropDown(lookupRepo.AddressFormatsByAddressType((AddressTypes)selectedAddressTypeKey));
		}

		/// <summary>
		/// 
		/// </summary>
		protected void ClearAttorneyControls()
		{
			_view.ClearAttorneyControls();
		}

		/// <summary>
		/// 
		/// </summary>
		protected void ClearAddress()
		{
			_view.ClearAddress();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="attorneyKey"></param>
		protected void PopulateAttorneyControls(int attorneyKey)
		{
			ILegalEntityRepository legalEntityRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();

			_attorney = legalEntityRepo.GetAttorneyByKey(attorneyKey);

			ILegalEntityAddress leAddress = _attorney.LegalEntity.LegalEntityAddresses[0];
			// Get the latest address that has been added to the Legal Entity
			foreach (ILegalEntityAddress leAdd in _attorney.LegalEntity.LegalEntityAddresses)
			{
				if (leAdd.Key > leAddress.Key)
					leAddress = leAdd;
			}
			// make sure the Address Format has the correct items populated
			BindAddressFormat(leAddress.AddressType.Key);
			_view.PopulateAttorneyControls(_attorney);
		}

		/// <summary>
		/// 
		/// </summary>
		protected void PopulateYesNoDropDowns()
		{
			_view.PopulateWorkflowEnabledDropDown();
			if (!_Adding)
			{
				_view.PopulateRegistrationAttorneyDropDown();
				_view.PopulateLitigationAttorneyDropDown();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected void ShowAttorneyDetails()
		{
			_view.Set_pnlAttorneyDetailsVisibility = true;
			_view.Set_pnlAddressVisibility = true;
            bool litigation = _view.LitigationAttorneyAuthenticated;

			if (_Editing)
			{
				_view.Set_lblAttorneyNameVisibility = false;
				_view.Set_txtAttorneyNameVisibility = true;
				_view.Set_lblWorkflowEnabledVisibility = false;
				_view.Set_ddlWorkflowEnabledVisibility = true;
				_view.Set_lblAttorneyContactVisibility = false;
				_view.Set_txtAttorneyContactVisibility = true;
				_view.Set_lblAttorneyMandateVisibility = false;
				_view.Set_txtAttorneyMandateVisibility = true;
				_view.Set_lblPhoneNumberVisibility = false;
				_view.Set_txtPhoneNumberVisibility = true;
				if (_Adding)
				{
					SetAttorneyVisibility(false);
					_view.Set_tdAttorneyNumberVisibility = false;
					_view.Set_lblAttorneyNumberVisibility = false;
					_view.Set_tdStatusVisibility = false;
					_view.Set_lblStatusVisibility = false;
					_view.Set_ddStatusVisibility = false;
					_view.Set_lblRegistrationAttorneyVisibility = true;
					_view.Set_ddlRegistrationAttorneyVisibility = false;
					_view.Set_tdDeedsOfficeChangeVisibility = false;
					_view.Set_ddlDeedsOfficeChangeVisibility = false;

                    //Default the effective date to todays date
                    _view.Set_EffectiveDateDefault = DateTime.Now;
				}
				else
				{
					SetAttorneyVisibility(true);
					_view.Set_tdAttorneyNumberVisibility = true;
					_view.Set_lblAttorneyNumberVisibility = true;
					_view.Set_tdStatusVisibility = true;
					_view.Set_lblStatusVisibility = false;
					_view.Set_ddStatusVisibility = true;
					_view.Set_lblRegistrationAttorneyVisibility = false;
					_view.Set_ddlRegistrationAttorneyVisibility = true;
                    _view.Set_tdDeedsOfficeChangeVisibility = true;
					_view.Set_ddlDeedsOfficeChangeVisibility = true;
				}
				_view.Set_lblEmailAddressVisibility = false;
				_view.Set_txtEmailAddressVisibility = true;
				_view.Set_lblAddressVisibility = false;
				_view.Set_lblAddressTypeVisibility = false;
				_view.Set_ddlAddressTypeVisibility = true;
				_view.Set_lblAddressFormatVisibility = false;
				_view.Set_ddlAddressFormatVisibility = true;
				_view.Set_lblEffectiveDateVisibility = false;
				_view.Set_dtEffectiveDateVisibility = true;
				_view.Set_addressDetailsVisibility = true;
				_view.Set_CancelButtonVisibility = true;
                _view.Set_SubmitButtonVisibility = true;

                if (!_Adding && litigation)
                {
                    SetFieldsAsReadOnly();
                    _view.Set_lblLitigationAttorneyVisibility = false;
                    _view.Set_ddlLitigationAttorneyVisibility = true;
                    _view.Set_ContactsButtonVisibility = true;
                }
                else
                {
                    _view.Set_lblLitigationAttorneyVisibility = true;
                    _view.Set_ddlLitigationAttorneyVisibility = false;
                    _view.Set_ContactsButtonVisibility = false;

                }
			}
			else 
            {
                SetFieldsAsReadOnly();
                _view.Set_lblLitigationAttorneyVisibility = true;
                _view.Set_ddlLitigationAttorneyVisibility = false;
                _view.Set_CancelButtonVisibility = false;
                _view.Set_SubmitButtonVisibility = false;
                if (litigation)
                    _view.Set_ContactsButtonVisibility = true;
                else
                    _view.Set_ContactsButtonVisibility = false;
			}
		}

        private void SetFieldsAsReadOnly()
        {
            SetAttorneyVisibility(true);
            _view.Set_tdAttorneyNumberVisibility = true;
            _view.Set_lblAttorneyNumberVisibility = true;
            _view.Set_lblStatusVisibility = true;
            _view.Set_ddStatusVisibility = false;
            _view.Set_lblAttorneyNameVisibility = true;
            _view.Set_txtAttorneyNameVisibility = false;
            _view.Set_lblWorkflowEnabledVisibility = true;
            _view.Set_ddlWorkflowEnabledVisibility = false;
            _view.Set_lblAttorneyContactVisibility = true;
            _view.Set_txtAttorneyContactVisibility = false;
            _view.Set_lblAttorneyMandateVisibility = true;
            _view.Set_txtAttorneyMandateVisibility = false;
            _view.Set_lblPhoneNumberVisibility = true;
            _view.Set_txtPhoneNumberVisibility = false;
            _view.Set_lblRegistrationAttorneyVisibility = true;
            _view.Set_ddlRegistrationAttorneyVisibility = false;
            _view.Set_lblEmailAddressVisibility = true;
            _view.Set_txtEmailAddressVisibility = false;
            _view.Set_tdDeedsOfficeChangeVisibility = false;
            _view.Set_ddlDeedsOfficeChangeVisibility = false;
            _view.Set_lblAddressVisibility = true;
            _view.Set_lblAddressTypeVisibility = true;
            _view.Set_ddlAddressTypeVisibility = false;
            _view.Set_lblAddressFormatVisibility = true;
            _view.Set_ddlAddressFormatVisibility = false;
            _view.Set_lblEffectiveDateVisibility = true;
            _view.Set_dtEffectiveDateVisibility = false;
            _view.Set_addressDetailsVisibility = false;
        }

		/// <summary>
		/// 
		/// </summary>
		protected void SetAttorneyVisibility(bool attorneyVisibility)
		{
			_view.Set_tdAttorneyVisibility = attorneyVisibility;
			_view.Set_ddlAttorneyVisibility = attorneyVisibility;
		}

		#endregion


        protected List<ICacheObjectLifeTime> LifeTimes
        {
            get
            {
                if (_lifeTimes == null)
                {
                    List<string> views = new List<string>();
                    views.Add("AttorneyDetails");
                    views.Add("AttorneyDetailsAdd");
                    views.Add("AttorneyDetailsUpdate");
                    views.Add("AttorneyContact");
                    views.Add("AttorneyContactView");
                    _lifeTimes = new List<ICacheObjectLifeTime>();
                    _lifeTimes.Add(new SimplePageCacheObjectLifeTime(views));
                }
                return _lifeTimes;
            }
        }

        protected void ClearCachedValues()
        {
            this.GlobalCacheData.Remove("DeedsOfficeKey");
            this.GlobalCacheData.Remove("AddressTypeKey");
            this.GlobalCacheData.Remove("AttorneyKey");
            this.GlobalCacheData.Remove("AddressFormatKey");
            this.GlobalCacheData.Remove("AddressTypeKey");
            this.GlobalCacheData.Remove("AddressTypeKey");

        }
	}
}
