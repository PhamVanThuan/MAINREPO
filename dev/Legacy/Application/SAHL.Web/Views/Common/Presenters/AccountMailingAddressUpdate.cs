using System;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Collections.Generic;
using SAHL.Common.Exceptions;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// Presenter for Mailing Address Update
    /// </summary>
    public class AccountMailingAddressUpdate : AccountMailingAddressBase
    {
        IDictionary<string, string> leAddressLst;
        IList<ILegalEntity> leEmailAddressLst;
        /// <summary>
        /// Constructor for Account Mailing Address Update
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public AccountMailingAddressUpdate(IAccountMailingAddress view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }
        /// <summary>
        /// OnViewInitialised event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.SetControlsForUpdate();

            int[] roleTypes = new int[] { (int)RoleTypes.MainApplicant, (int)RoleTypes.Suretor };
            IReadOnlyEventList<ILegalEntity> le = account.GetLegalEntitiesByRoleType(_view.Messages, roleTypes, GeneralStatusKey.Active);
            leAddressLst = new Dictionary<string, string>();
            leEmailAddressLst = new List<ILegalEntity>();
            IDictionary<string, ILegalEntity> leEmailAddressDict = new Dictionary<string, ILegalEntity>();

            foreach (ILegalEntity legalEntity in le)
            {
                // Create Email list based on LegalEntity
                if ((!string.IsNullOrEmpty(legalEntity.EmailAddress) && legalEntity.EmailAddress.Trim().Length > 0) &&
                    !(leEmailAddressDict.ContainsKey(legalEntity.EmailAddress)))
                    leEmailAddressDict.Add(legalEntity.EmailAddress, legalEntity);

                for (int a = 0; a < legalEntity.LegalEntityAddresses.Count; a++)
                {
                    if (legalEntity.LegalEntityAddresses[a].GeneralStatus.Key == (int)GeneralStatuses.Active && legalEntity.LegalEntityAddresses[a].Address.AddressFormat.Key != (int)AddressFormats.FreeText)
                    {
                        if (!leAddressLst.ContainsKey(legalEntity.LegalEntityAddresses[a].Address.Key.ToString()))
                            leAddressLst.Add(legalEntity.LegalEntityAddresses[a].Address.Key.ToString(), legalEntity.LegalEntityAddresses[a].Address.GetFormattedDescription(AddressDelimiters.Comma));
                    }
                }
            }

            foreach (KeyValuePair<string, ILegalEntity> kv in leEmailAddressDict)
            {
                leEmailAddressLst.Add(kv.Value);
            }

            _view.PopulateMailingAddressDropDown(leAddressLst);
            _view.BindEmailAddressDropDown(leEmailAddressLst);
            //_view.BindMailingAddressLstDisplay(mailingAddressLst);

            if (accMailingAddress != null && accMailingAddress.Count > 0)
            {
                if (!_view.IsPostBack && accMailingAddress[0].CorrespondenceMedium != null)
                {
                    if (PrivateCacheData.ContainsKey("CorrespondenceMediumsValue"))
                        PrivateCacheData["CorrespondenceMediumsValue"] = accMailingAddress[0].CorrespondenceMedium.Key;
                    else
                        PrivateCacheData.Add("CorrespondenceMediumsValue", accMailingAddress[0].CorrespondenceMedium.Key);
                }
                _view.BindUpdateableFields(accMailingAddress[0]);
            }
            else
                _view.BindLookUpsForUpdate();

            _view.CorrespondenceMediumRowVisible = true;

            if ((PrivateCacheData.ContainsKey("CorrespondenceMediumsValue") && Convert.ToString(PrivateCacheData["CorrespondenceMediumsValue"]) != "-select-") &&
                Convert.ToInt32(PrivateCacheData["CorrespondenceMediumsValue"]) == (int)CorrespondenceMediums.Email)
            {
                _view.CorrespondenceMailAddressRowVisible = true;
            }
            else
                _view.CorrespondenceMailAddressRowVisible = false;

            _view.OnddlMailingAddressSelectedIndexChanged += (_view_OnddlMailingAddressSelectedIndexChanged);
            _view.OnddlCorrespondenceMediumSelectedIndexChanged += (_view_OnddlCorrespondenceMediumSelectedIndexChanged);
            _view.onSubmitButtonClicked += (_view_onSubmitButtonClicked);
            _view.onCancelButtonClicked += (_view_onCancelButtonClicked);

        }

        void _view_OnddlCorrespondenceMediumSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            if (!(PrivateCacheData.ContainsKey("CorrespondenceMediumsValue")))
            {
                CorrespondenceMediumHelper(e);
            }
            else
            {
                if (PrivateCacheData["CorrespondenceMediumsValue"].ToString() != e.Key.ToString())
                    CorrespondenceMediumHelper(e);
            }
        }

        private void CorrespondenceMediumHelper(KeyChangedEventArgs e)
        {
            if (e.Key.ToString() != "-select-")
            {
                if (Convert.ToInt32(e.Key) == (int)CorrespondenceMediums.Email)
                    _view.CorrespondenceMailAddressRowVisible = true;
                else
                    _view.CorrespondenceMailAddressRowVisible = false;
            }
            else
                _view.CorrespondenceMailAddressRowVisible = false;

            if (PrivateCacheData.ContainsKey("CorrespondenceMediumsValue"))
                PrivateCacheData["CorrespondenceMediumsValue"] = e.Key.ToString();
            else
                PrivateCacheData.Add("CorrespondenceMediumsValue", e.Key.ToString());
        }

        void _view_OnddlMailingAddressSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            if (e.Key.ToString() == "-select-")
                _view.BindMailingAddressUpdate("");
            else
                _view.BindMailingAddressUpdate(leAddressLst[e.Key.ToString()]);
        }

        void _view_onSubmitButtonClicked(object sender, EventArgs e)
        {
            IMailingAddress mailingAdd;
            IAddressRepository addressRepo = RepositoryFactory.GetRepository<IAddressRepository>();
            IAddress address = addressRepo.GetAddressByKey(_view.GetSelectedAddressKey);
            IMailingAddress mailAddress = addressRepo.CreateEmptyMailingAddress();
            
            if (accMailingAddress != null && accMailingAddress.Count > 0)
                mailingAdd = _view.GetCapturedMailingAddress(accMailingAddress[0]);
            else
            {
                mailingAdd = View.GetCapturedMailingAddress(mailAddress);
                mailingAdd.Account = account;
            }

            if (Convert.ToInt32(_view.CorrespondenceMediumKey) == (int)CorrespondenceMediums.Email && _view.CorrespondenceMailAddressKey != "-select-")
            {
                ILegalEntityRepository _leRep = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                mailingAdd.LegalEntity = _leRep.GetLegalEntityByKey(Convert.ToInt32(_view.CorrespondenceMailAddressKey));
            }
            else
                mailingAdd.LegalEntity = null;

            mailingAdd.Address = address;

            TransactionScope txn = new TransactionScope();

            try
            {
                ILookupRepository LR = RepositoryFactory.GetRepository<ILookupRepository>();

                if (_view.OnlineStatementRequired == false)
                {
                    mailingAdd.OnlineStatementFormat = LR.OnlineStatementFormats.ObjectDictionary[Convert.ToString((int)OnlineStatementFormats.NotApplicable)];
                }
                addressRepo.SaveMailingAddress(mailingAdd);
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

            if (_view.IsValid)
                _view.Navigator.Navigate("Display");
        }

        void _view_onCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Display");
        }

    }
}
