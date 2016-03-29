using System;
using System.Collections.Generic;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.Globals;
using SAHL.Common.CacheData;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel;
using System.Linq;

namespace SAHL.Web.Views.Administration.Presenters
{
    /// <summary>
    /// Attorney Add
    /// </summary>
    public class AttorneyAdd : AttorneyBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public AttorneyAdd(SAHL.Web.Views.Administration.Interfaces.IAttorney view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _view.OnSubmitButtonClicked += _view_OnSubmitButtonClicked;
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
            base._Adding = true;
            base._Editing = true;

            BindAddressType();
            if (_view.IsPostBack)
            {
                PopulateYesNoDropDowns();
                if (this.GlobalCacheData.ContainsKey("AddressTypeKey"))
                    BindAddressFormat((int)this.GlobalCacheData["AddressTypeKey"]);
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
            SetAttorneyVisibility(false);

            if (_view.IsPostBack)
            {
                if (this.GlobalCacheData.ContainsKey("DeedsOfficeKey"))
                {
                    _view.PopulateRegistrationLitigationControls(true, false);
                    ShowAttorneyDetails();
                }

                if (this.GlobalCacheData.ContainsKey("AddressFormatKey"))
                    _view.GetSet_AddressFormat = (AddressFormats)(int)this.GlobalCacheData["AddressFormatKey"];
                else if (_view.Get_ddlAddressFormatSelectedValue != -1)
                    _view.GetSet_AddressFormat = (AddressFormats)(int)_view.Get_ddlAddressFormatSelectedValue;
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
        }

        void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            ILegalEntityRepository legalEntityRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            ICommonRepository commRepo = RepositoryFactory.GetRepository<ICommonRepository>();
            ILookupRepository lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();

            _attorney = legalEntityRepo.CreateEmptyAttorney();
            IDeedsOffice attorneyDeedsOffice = commRepo.GetByKey<IDeedsOffice>(_view.GetSet_ddlDeedsOffice);
            ILegalEntityCompany attorneyLegalEntityCompany = legalEntityRepo.GetEmptyLegalEntity(LegalEntityTypes.Company) as ILegalEntityCompany;

            _attorney.DeedsOffice = attorneyDeedsOffice;
            _attorney.AttorneyWorkFlowEnabled = _view.Get_ddlWorkflowEnabled;
            _attorney.AttorneyContact = _view.Get_txtAttorneyContact;
            _attorney.AttorneyMandate = _view.Get_txtAttorneyMandate;
            _attorney.AttorneyRegistrationInd = true; // Default to Yes as per spec
            _attorney.AttorneyLitigationInd = false; // Default to No as per spec
            _attorney.GeneralStatus = commRepo.GetByKey<IGeneralStatus>((int)GeneralStatuses.Active); // Default to Active as per spec
            _attorney.OriginationSources.Add(_view.Messages, lookupRepository.OriginationSources.Where(x => x.Key == (int)OriginationSources.SAHomeLoans).Single());

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

            IAddress addressNew = _view.Get_CapturedAddress;
            addressNew.ValidateEntity();

            if (_view.Messages.Count == 0)
            {
                TransactionScope txn = new TransactionScope();
                try
                {
                    this.ExclusionSets.Add(RuleExclusionSets.LegalEntityAddAttorney);

                    legalEntityRepo.SaveLegalEntity(_attorney.LegalEntity, false);

                    if (addressNew != null)
                        legalEntityRepo.SaveAddress(addressType, _attorney.LegalEntity, addressNew, _view.GetSet_dtEffectiveDate);

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
                //    this.GlobalCacheData.Add("DeedsOfficeKey", this.GlobalCacheData["DeedsOfficeKey"], new List<ICacheObjectLifeTime>());
                //    Navigator.Navigate("AttorneyDetails");
                //}
                if (_view.IsValid && _attorney.Key != 0)
                    Navigator.Navigate("AttorneyDetails");
            }
        }

        #endregion
    }
}
