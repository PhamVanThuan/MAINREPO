using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Web.Views.Common.Presenters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Web.Views.PersonalLoan.Presenters
{
    public class UnsecuredLendingMailingAddressUpdate : ApplicationMailingAddressBase
    {
        IDictionary<string, string> leAddressLst;
        IList<ILegalEntity> leEmailAddressLst;
        IApplicationUnsecuredLending applicationUnsecuredLending;
        IApplicationUnsecuredLendingRepository appULRepo;

        /// <summary>
        /// Constructor for Account Mailing Address Update
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public UnsecuredLendingMailingAddressUpdate(IAccountMailingAddress view, SAHLCommonBaseController controller)
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

            //_view.SetControlsForUpdate();

            appULRepo = RepositoryFactory.GetRepository<IApplicationUnsecuredLendingRepository>();
            applicationUnsecuredLending = appULRepo.GetApplicationByKey(_genericKey);

            leAddressLst = new Dictionary<string, string>();
            leEmailAddressLst = new List<ILegalEntity>();
            IDictionary<string, ILegalEntity> leEmailAddressDict = new Dictionary<string, ILegalEntity>();

            foreach (IExternalRole er in applicationUnsecuredLending.ActiveClientRoles)
            {
                // Create Email list based on LegalEntity
                if ((!string.IsNullOrEmpty(er.LegalEntity.EmailAddress) && er.LegalEntity.EmailAddress.Trim().Length > 0)
                        && !(leEmailAddressDict.ContainsKey(er.LegalEntity.EmailAddress)))
                {
                    leEmailAddressDict.Add(er.LegalEntity.EmailAddress, er.LegalEntity);
                }

                foreach (ILegalEntityAddress leAdd in er.LegalEntity.LegalEntityAddresses)
                {
                    if (leAdd.GeneralStatus.Key == (int)GeneralStatuses.Active
                            && leAdd.Address.AddressFormat.Key != (int)AddressFormats.FreeText)
                    {
                        if (!leAddressLst.ContainsKey(leAdd.Address.Key.ToString()))
                        {
                            leAddressLst.Add(leAdd.Address.Key.ToString(),
                                leAdd.Address.GetFormattedDescription(AddressDelimiters.Comma));
                        }
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

            if (leAddressLst == null || leAddressLst.Count == 0)
            {
                _view.ShowUpdateButton = false;
            }

            //applicationUnsecuredLending.ApplicationMailingAddresses

            if (applicationUnsecuredLending.ApplicationMailingAddresses != null
                    && applicationUnsecuredLending.ApplicationMailingAddresses.Count > 0)
            {
                // If the language key changes, the condition sets need to be verified for the chosen language.
                // Cache the key to check for change
                PrivateCacheData.Remove("LanguageKey");
                PrivateCacheData.Add("LanguageKey", applicationUnsecuredLending.ApplicationMailingAddresses[0].Language.Key);

                if (!_view.IsPostBack && applicationUnsecuredLending.ApplicationMailingAddresses[0].CorrespondenceMedium != null)
                {
                    if (PrivateCacheData.ContainsKey("CorrespondenceMediumsValue"))
                    {
                        PrivateCacheData["CorrespondenceMediumsValue"]
                            = applicationUnsecuredLending.ApplicationMailingAddresses[0].CorrespondenceMedium.Key;
                    }
                    else
                    {
                        PrivateCacheData.Add("CorrespondenceMediumsValue",
                            applicationUnsecuredLending.ApplicationMailingAddresses[0].CorrespondenceMedium.Key);
                    }
                }

                _view.BindUpdateableFieldsForApplication(applicationUnsecuredLending.ApplicationMailingAddresses[0]);
            }
            else
            {
                _view.BindLookUpsForUpdate();
            }

            _view.CorrespondenceMediumRowVisible = true;

            if ((PrivateCacheData.ContainsKey("CorrespondenceMediumsValue")
                    && Convert.ToString(PrivateCacheData["CorrespondenceMediumsValue"]) != "-select-")
                        && Convert.ToInt32(PrivateCacheData["CorrespondenceMediumsValue"]) == (int)CorrespondenceMediums.Email)
            {
                _view.CorrespondenceMailAddressRowVisible = true;
            }
            else
            {
                _view.CorrespondenceMailAddressRowVisible = false;
            }

            _view.OnddlMailingAddressSelectedIndexChanged += (_view_OnddlMailingAddressSelectedIndexChanged);
            _view.OnddlCorrespondenceMediumSelectedIndexChanged += new KeyChangedEventHandler(_view_OnddlCorrespondenceMediumSelectedIndexChanged);
            _view.onSubmitButtonClicked += (_view_onSubmitButtonClicked);
            _view.onCancelButtonClicked += (_view_onCancelButtonClicked);
            _view.ReBind = true;
        }

        private void _view_OnddlCorrespondenceMediumSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            if (!(PrivateCacheData.ContainsKey("CorrespondenceMediumsValue")))
            {
                CorrespondenceMediumHelper(e);
            }
            else
            {
                if (PrivateCacheData["CorrespondenceMediumsValue"].ToString() != e.Key.ToString())
                {
                    CorrespondenceMediumHelper(e);
                }
            }
        }

        private void CorrespondenceMediumHelper(KeyChangedEventArgs e)
        {
            if (e.Key.ToString() != "-select-")
            {
                if (Convert.ToInt32(e.Key) == (int)CorrespondenceMediums.Email)
                {
                    _view.CorrespondenceMailAddressRowVisible = true;
                }
                else
                {
                    _view.CorrespondenceMailAddressRowVisible = false;
                }
            }
            else
            {
                _view.CorrespondenceMailAddressRowVisible = false;
            }

            if (PrivateCacheData.ContainsKey("CorrespondenceMediumsValue"))
            {
                PrivateCacheData["CorrespondenceMediumsValue"] = e.Key.ToString();
            }
            else
            {
                PrivateCacheData.Add("CorrespondenceMediumsValue", e.Key.ToString());
            }
        }

        private void _view_OnddlMailingAddressSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            if (e.Key.ToString() == "-select-")
            {
                _view.BindMailingAddressUpdate("");
            }
            else
            {
                _view.BindMailingAddressUpdate(leAddressLst[e.Key.ToString()]);
            }
        }

        private void _view_onSubmitButtonClicked(object sender, EventArgs e)
        {
            IApplicationMailingAddress mailingAdd;
            IAddressRepository addressRepo = RepositoryFactory.GetRepository<IAddressRepository>();

            IAddress address = _view.GetSelectedAddressKey > 0 ? addressRepo.GetAddressByKey(_view.GetSelectedAddressKey) : null;

            IApplicationMailingAddress mailAddressNew = addressRepo.CreateEmptyApplicationMailingAddress();

            if (applicationUnsecuredLending.ApplicationMailingAddresses != null
                    && applicationUnsecuredLending.ApplicationMailingAddresses.Count > 0)
            {
                mailingAdd = _view.GetCapturedApplicationMailingAddress(applicationUnsecuredLending.ApplicationMailingAddresses[0]);
            }
            else
            {
                mailingAdd = View.GetCapturedApplicationMailingAddress(mailAddressNew);
                mailingAdd.Application = applicationUnsecuredLending;
            }

            mailingAdd.Address = address;

            ILookupRepository LR = RepositoryFactory.GetRepository<ILookupRepository>();

            if (_view.OnlineStatementRequired == false)
            {
                mailingAdd.OnlineStatementFormat
                    = LR.OnlineStatementFormats.ObjectDictionary[Convert.ToString((int)OnlineStatementFormats.NotApplicable)];
            }

            if (Convert.ToInt32(_view.CorrespondenceMediumKey)
                    == (int)CorrespondenceMediums.Email && _view.CorrespondenceMailAddressKey != "-select-")
            {
                ILegalEntityRepository _leRep = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                mailingAdd.LegalEntity = _leRep.GetLegalEntityByKey(Convert.ToInt32(_view.CorrespondenceMailAddressKey));
            }
            else
            {
                mailingAdd.LegalEntity = null;
            }

            TransactionScope txn = new TransactionScope();

            try
            {
                addressRepo.SaveApplicationMailingAddress(mailingAdd);
                txn.VoteCommit();
            }
            catch (Exception)
            {
                txn.VoteRollBack();
                _view.ReBind = false;

                if (_view.IsValid)
                {
                    throw;
                }
            }
            finally
            {
                txn.Dispose();
            }

            /* English conditions are created by default - so if it is a change TO english, ignore,
            /  else check if the new language is translatable. if it is, add the new translated items
            */
            if (PrivateCacheData.ContainsKey("LanguageKey"))
            {
                // check if the language has changed - if so, handle the translatable conditions
                if ((int)PrivateCacheData["LanguageKey"] != mailingAdd.Language.Key)
                {
                    CheckConditionsLanguage(mailingAdd);
                }
            }
            else
            {
                // this is the first time a language has been selected because privatecache languagekey is null
                CheckConditionsLanguage(mailingAdd);
            }

            if (_view.IsValid)
            {
                _view.Navigator.Navigate("Display");
            }
        }

        //TODO CHECK THIS WITH NBPUSER! for a set with conditions
        private void CheckConditionsLanguage(IApplicationMailingAddress mailingAddress)
        {
            IConditionsRepository conditionsRepository = RepositoryFactory.GetRepository<IConditionsRepository>();

            // check if a condition set has already been added else ignore this method
            if (conditionsRepository.SavedConditionSetExists(_genericKey))
            {
                if ((mailingAddress.Language.Key != (int)Languages.English) && mailingAddress.Language.Translatable)
                {
                    conditionsRepository.CheckForAndAddTranslatedConditions(_genericKey, _genericKeyTypeKey);
                }
            }
        }

        private void _view_onCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Display");
        }
    }
}