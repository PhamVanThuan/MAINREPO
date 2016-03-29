using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using Castle.ActiveRecord;
using System.Collections.Generic;
using SAHL.Common.Globals;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.BusinessModel;

namespace SAHL.Web.Views.Administration.Presenters
{
    public class Admin_AdUserMaint : Admin_AdUserBase
    {

        public Admin_AdUserMaint(IAduser view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            _view.OnSubmitClick += new EventHandler(_view_OnSubmitClick);

        }

        /// <summary>
        /// Set the relevant properties for displaying controls within the view
        /// </summary>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _view.VisibleSubmit = true;

            if (PrivateCacheData.ContainsKey("ADUSERNAME"))
                _view.AdUserName = PrivateCacheData["ADUSERNAME"].ToString();
        }

        void _view_OnSubmitClick(object sender, EventArgs e)
        {
            // add fields to cache
            PrivateCacheData.Remove("ADUSERNAME");
            PrivateCacheData.Add("ADUSERNAME", _view.AdUserName);


            //  get the ADUser
            IADUser user = RepositoryFactory.GetRepository<IOrganisationStructureRepository>().GetAdUserForAdUserName(_view.AdUserName);
            if (user != null && String.Compare(user.ADUserName, _view.AdUserName, true) == 0)
                _view.UserExistsInDatabase = true;
            else
                _view.UserExistsInDatabase = false;

            if (ValidateInput() == false)
                return;

            TransactionScope txn = new TransactionScope();
            try
            {
                if (_view.IsValid)
                {

                    // add exclusion set
                    this.ExclusionSets.Add(RuleExclusionSets.LegalEntityOperators);

                    // if ADUser doesnt exist then create a new one
                    if (user == null)
                    {
                        // add
                        user = base.OrganisationStructureRepo.CreateEmptyAdUser();
                        user.ADUserName = _view.AdUserName;
                        ILegalEntityNaturalPerson legalEntity = base.LegalEntityRepo.GetEmptyLegalEntity(LegalEntityTypes.NaturalPerson) as ILegalEntityNaturalPerson;
                        legalEntity.FirstNames = _view.FirstName;
                        legalEntity.Surname = _view.Surname;
                        legalEntity.EmailAddress = _view.EMail;
                        legalEntity.CellPhoneNumber = _view.CellPhoneNumber;

                        legalEntity.LegalEntityStatus = base.CommonRepo.GetByKey<ILegalEntityStatus>((int)LegalEntityStatuses.Alive);
                        legalEntity.CitizenType = base.CommonRepo.GetByKey<ICitizenType>((int)CitizenTypes.SACitizen);
                        legalEntity.DocumentLanguage = base.CommonRepo.GetLanguageByKey((int)Languages.English);
                        legalEntity.HomeLanguage = base.CommonRepo.GetLanguageByKey((int)Languages.English);
                        legalEntity.Education = base.CommonRepo.GetByKey<IEducation>((int)Educations.Unknown);
                        legalEntity.IntroductionDate = DateTime.Now;

                        base.LegalEntityRepo.SaveLegalEntity(legalEntity, false);

                        user.LegalEntity = legalEntity;
                    }
                    else
                    {
                        // update 
                        user.LegalEntity.FirstNames = _view.FirstName;
                        user.LegalEntity.Surname = _view.Surname;
                        user.LegalEntity.EmailAddress = _view.EMail;
                        user.LegalEntity.CellPhoneNumber = _view.CellPhoneNumber;                     
                    }

                    user.GeneralStatusKey = base.CommonRepo.GetByKey<IGeneralStatus>(_view.GeneralStatusKey);

                    // save 
                    base.OrganisationStructureRepo.SaveAdUser(user);

                    // remove exclusion set
                    this.ExclusionSets.Remove(RuleExclusionSets.LegalEntityOperators);

                    txn.VoteCommit();

                    _view.SelectedUserName = "";
                    PrivateCacheData.Remove("SELECTEDADUSERNAME");
                }
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

        bool ValidateInput()
        {
            bool valid = true;
            string errorMessage = "";

            if (String.IsNullOrEmpty(_view.AdUserName))
            {
                errorMessage = "Must enter User Name";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
                valid = false;
            }
            else if (_view.AdUserName.StartsWith(@"SAHL\") == false)
            {
                errorMessage = "User Name must be in format 'SAHL\\xxxxxxx'";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
                valid = false;
            }

            if (String.IsNullOrEmpty(_view.FirstName))
            {
                errorMessage = "Must enter First Name";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
                valid = false;
            }
            if (String.IsNullOrEmpty(_view.Surname))
            {
                errorMessage = "Must enter Surname";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
                valid = false;
            }
            if (String.IsNullOrEmpty(_view.EMail))
            {
                errorMessage = "Must enter Email Address";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
                valid = false;
            }
            if (String.IsNullOrEmpty(_view.CellPhoneNumber))
            {
                errorMessage = "Must enter Cellphone Number";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
                valid = false;
            }

            return valid;
        }

    }
}
