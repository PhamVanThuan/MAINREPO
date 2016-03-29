using System;
using System.Collections.Generic;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.Globals;
using SAHL.Common.CacheData;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Web.Views.Administration.Presenters
{
	/// <summary>
	/// Attorney Edit
	/// </summary>
	public class AttorneyEdit : AttorneyBase
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public AttorneyEdit(SAHL.Web.Views.Administration.Interfaces.IAttorney view, SAHLCommonBaseController controller)
			: base(view, controller)
		{
			_view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);
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

            _view.OnContactsButton_Clicked += new EventHandler(_view_OnContactsButtonClicked);

			base._Adding = false;
			base._Editing = true;

			BindAddressType();
			if (_view.IsPostBack)
			{
				PopulateYesNoDropDowns();
				PopulateStatusDropDown();
				if (this.GlobalCacheData.ContainsKey("DeedsOfficeKey"))
					BindAttorneyDropDown((int)this.GlobalCacheData["DeedsOfficeKey"]);

				if (this.GlobalCacheData.ContainsKey("AddressTypeKey"))
					BindAddressFormat((int)this.GlobalCacheData["AddressTypeKey"]);

                if (_view.LitigationAttorneyAuthenticated && this.GlobalCacheData.ContainsKey("AttorneyKey"))
                {
                    PopulateAttorneyControls((int)this.GlobalCacheData["AttorneyKey"]);
                }
			}
			else
				ClearCachedValues();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void OnViewPreRender(object sender, EventArgs e)
		{
			base.OnViewPreRender(sender, e);

			if (_view.IsPostBack)
			{
				if (this.GlobalCacheData.ContainsKey("AttorneyKey"))
				{
					ShowAttorneyDetails();
				}
				else
				{
					_view.Set_pnlAttorneyDetailsVisibility = false;
					_view.Set_pnlAddressVisibility = false;
					if (this.GlobalCacheData.ContainsKey("DeedsOfficeKey"))
					{
						SetAttorneyVisibility(true);
						BindAttorneyDropDown((int)this.GlobalCacheData["DeedsOfficeKey"]);
					}
					else
						SetAttorneyVisibility(false);
				}

				if (_view.Get_ddlAddressFormatSelectedValue != -1)
					_view.GetSet_AddressFormat = (AddressFormats)(int)_view.Get_ddlAddressFormatSelectedValue;
				else if (this.GlobalCacheData.ContainsKey("AddressFormatKey"))
					_view.GetSet_AddressFormat = (AddressFormats)(int)this.GlobalCacheData["AddressFormatKey"];
			}
			else
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
						}
					}
					else
						this.GlobalCacheData.Add("AddressTypeKey", _view.Get_ddlAddressTypeSelectedValue, LifeTimes);
					BindAddressFormat((int)this.GlobalCacheData["AddressTypeKey"]);
				}
				if (_view.Get_ddlAddressFormatSelectedValue != -1)
				{
					if (this.GlobalCacheData.ContainsKey("AddressFormatKey"))
					{
						this.GlobalCacheData["AddressFormatKey"] = _view.Get_ddlAddressFormatSelectedValue;
					}
					else
					{
						this.GlobalCacheData.Add("AddressFormatKey", _view.Get_ddlAddressFormatSelectedValue, LifeTimes);
					}
					_view.GetSet_AddressFormat = (AddressFormats)(int)this.GlobalCacheData["AddressFormatKey"];
				}
            }
            _view.Set_ContactsButtonVisibility = _view.Get_ddlLitigationAttorney;
        }

        /// <summary>
        /// On Submit Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		void _view_OnSubmitButtonClicked(object sender, EventArgs e)
		{
            SaveAttorney();
		}

        #region Helper Save Methods

        private void SaveLitigationAttorney()
        {
            ILegalEntityRepository legalEntityRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            _attorney = legalEntityRepo.GetAttorneyByKey(_view.GetSet_ddlAttorney);

            _attorney.AttorneyLitigationInd = _view.Get_ddlLitigationAttorney;

            if (_view.Messages.Count == 0)
            {
                TransactionScope txn = new TransactionScope();
                try
                {
                    this.ExclusionSets.Add(RuleExclusionSets.LegalEntityAddAttorney);
                    legalEntityRepo.SaveAttorney(_attorney);
                    this.ExclusionSets.Remove(RuleExclusionSets.LegalEntityAddAttorney);
                    txn.VoteCommit();
                }
                catch (Exception)
                {
                    txn.VoteRollBack();
                    if (View.IsValid)
                        throw;
                }
                finally
                {
                    txn.Dispose();
                }
            }
            if (_view.IsValid && _attorney.Key != 0)
                Navigator.Navigate("AttorneyDetails");

        }

        private void SaveAttorneyComplete()
        {
            ILegalEntityRepository legalEntityRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            ICommonRepository commRepo = RepositoryFactory.GetRepository<ICommonRepository>();

            _attorney = legalEntityRepo.GetAttorneyByKey(_view.GetSet_ddlAttorney);
            IDeedsOffice attorneyDeedsOffice = commRepo.GetByKey<IDeedsOffice>(_view.Get_ddlDeedsOfficeChange);
            ILegalEntityCompany attorneyLegalEntityCompany = legalEntityRepo.GetLegalEntityByKey(_attorney.LegalEntity.Key) as ILegalEntityCompany;

            _attorney.DeedsOffice = attorneyDeedsOffice;
            _attorney.AttorneyWorkFlowEnabled = _view.Get_ddlWorkflowEnabled;
            _attorney.AttorneyContact = _view.Get_txtAttorneyContact;
            _attorney.AttorneyMandate = _view.Get_txtAttorneyMandate;
            _attorney.AttorneyRegistrationInd = _view.Get_ddlRegistrationAttorney;
            _attorney.GeneralStatus = commRepo.GetByKey<IGeneralStatus>(_view.Get_ddlStatus);

            attorneyLegalEntityCompany.RegisteredName = _view.Get_txtAttorneyName;
            attorneyLegalEntityCompany.TradingName = _view.Get_txtAttorneyName;
            attorneyLegalEntityCompany.WorkPhoneCode = _view.Get_txtPhoneNumberCode;
            attorneyLegalEntityCompany.WorkPhoneNumber = _view.Get_txtPhoneNumber;
            attorneyLegalEntityCompany.EmailAddress = _view.Get_txtEmailAddress;

            attorneyLegalEntityCompany.IntroductionDate = DateTime.Now;
            attorneyLegalEntityCompany.DocumentLanguage = commRepo.GetLanguageByKey((int)Languages.English); // Default to English as per spec
            attorneyLegalEntityCompany.LegalEntityStatus = commRepo.GetByKey<ILegalEntityStatus>((int)SAHL.Common.Globals.LegalEntityStatuses.Alive); // Default to Alive as per spec

            _attorney.LegalEntity = attorneyLegalEntityCompany;

            IRuleService svcRule = ServiceFactory.GetService<IRuleService>();
            svcRule.ExecuteRule(_view.Messages, "AttorneyMandatoryFields", _attorney);

            IAddressType addressType = null;
            if (_view.Get_ddlAddressTypeSelectedValue != -1 && _view.Get_ddlAddressFormatSelectedValue != -1)
                addressType = addressRepo.GetAddressTypeByKey(_view.Get_ddlAddressTypeSelectedValue);

            IAddress addressEdit = _view.Get_CapturedAddress;
            addressEdit.ValidateEntity();

            if (_view.Messages.Count == 0)
            {
                TransactionScope txn = new TransactionScope();
                try
                {
                    this.ExclusionSets.Add(RuleExclusionSets.LegalEntityAddAttorney);

                    legalEntityRepo.SaveLegalEntity(_attorney.LegalEntity, false);

                    if (addressEdit != null)
                    {
                        if (!base._Editing)
                            legalEntityRepo.SaveAddress(addressType, _attorney.LegalEntity, addressEdit, _view.GetSet_dtEffectiveDate);
                        else
                        {

                            ILegalEntityAddress leAddress = _attorney.LegalEntity.LegalEntityAddresses[0];

                            foreach (ILegalEntityAddress leAdd in _attorney.LegalEntity.LegalEntityAddresses)
                            {
                                if (leAdd.Key > leAddress.Key)
                                    leAddress = leAdd;
                            }
                            leAddress.AddressType = addressType;
                            leAddress.EffectiveDate = _view.GetSet_dtEffectiveDate;
                            legalEntityRepo.SaveLegalEntityAddress(leAddress, addressEdit);

                        }


                    }
                    legalEntityRepo.SaveAttorney(_attorney);

                    this.ExclusionSets.Remove(RuleExclusionSets.LegalEntityAddAttorney);
                    txn.VoteCommit();
                }
                catch (Exception)
                {
                    txn.VoteRollBack();
                    if (View.IsValid)
                        throw;
                }
                finally
                {
                    txn.Dispose();
                }

                //if (_attorney.Key != 0 && this.GlobalCacheData.ContainsKey("DeedsOfficeKey") && View.IsValid)
                //{
                //    this.GlobalCacheData.Add("AttorneyKey", _attorney.Key, new List<ICacheObjectLifeTime>());
                //    this.GlobalCacheData.Add("DeedsOfficeKey", _view.Get_ddlDeedsOfficeChange, new List<ICacheObjectLifeTime>());
                //    Navigator.Navigate("AttorneyDetails");
                //}
                if (_view.IsValid && _attorney.Key != 0)
                    Navigator.Navigate("AttorneyDetails");
            }
        }

        /// <summary>
        /// Save The Attorney Details
        /// </summary>
        private void SaveAttorney()
        {
            if (_view.LitigationAttorneyAuthenticated)
                SaveLitigationAttorney();
            else
                SaveAttorneyComplete();
        }
        #endregion

        protected void _view_OnContactsButtonClicked(object sender, EventArgs e)
        {
            SaveAttorney();
            Navigator.Navigate("AttorneyContact");
        }

		#endregion

		#region Methods

		/// <summary>
		/// 
		/// </summary>
		protected void PopulateStatusDropDown()
		{
			_view.PopulateStatusDropDown(new List<IGeneralStatus>(lookupRepo.GeneralStatuses.Values));
		}

		#endregion
	}
}
