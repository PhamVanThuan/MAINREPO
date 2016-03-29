using System;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.CacheData;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Common.Presenters.LegalEntityDetails
{
    /// <summary>
    /// 
    /// </summary>
    public class LegalEntityDetailsLifeAddExisting : LegalEntityDetailsUpdateBase
    {
        /// <summary>
        /// Presenter constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LegalEntityDetailsLifeAddExisting(ILegalEntityDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            // Bind additional life-specific stuff
            _view.BindInsurableInterestType(LookupRepository.LifeInsurableInterestTypes.BindableDictionary, "");

            base.LoadLegalEntityFromGlobalCache();

            base.BindLegalEntity();

            _view.OnReBindLegalEntity += new KeyChangedEventHandler(OnReBindLegalEntity);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {

            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.InsurableInterestUpdateVisible = true;
            _view.NonContactDetailsDisabled = true;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2219:DoNotRaiseExceptionsInExceptionClauses", Justification = "New rule added to FxCop, avoiding refactor")]
        protected override void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            // Get the details from the screen
            _view.PopulateLegalEntityDetailsForUpdate(base.LegalEntity);
            // Populate the marketing options ...
            PopulateMarketingOptions();

            TransactionScope txn = new TransactionScope();

            try
            {
                // save the legal entity 
                base.LegalEntityRepository.SaveLegalEntity(base.LegalEntity, false);

                // save the assured life role
                if (GlobalCacheData.ContainsKey(ViewConstants.SelectedLifeAccountKey))
                {
                    int lifeAccountKey = Convert.ToInt32(GlobalCacheData[ViewConstants.SelectedLifeAccountKey]);
                    IAccountRepository accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                    ILifeRepository lifeRepo = RepositoryFactory.GetRepository<ILifeRepository>();
                    IAccountLifePolicy accountLifePolicy = accountRepo.GetAccountByKey(lifeAccountKey) as IAccountLifePolicy;

                    // Populate the LifeInsurableInterest object
                    if (_view.SelectedInsurableInterestTypeUpdate > -1)
                    {
                        ILifeInsurableInterest lifeInsurableInterest = lifeRepo.CreateEmptyLifeInsurableInterest();
                        lifeInsurableInterest.Account = accountLifePolicy;
                        lifeInsurableInterest.LegalEntity = base.LegalEntity;
                        lifeInsurableInterest.LifeInsurableInterestType = base.LookupRepository.LifeInsurableInterestTypes.ObjectDictionary[Convert.ToString(_view.SelectedInsurableInterestTypeUpdate)];

                        // Add the LifeInsurableInterest to the AccountLifePolicy object
                        accountLifePolicy.LifeInsurableInterests.Add(_view.Messages, lifeInsurableInterest);
                    }

                    // populate the Role object
                    IRole role = accountRepo.CreateEmptyRole();
                    role.Account = accountLifePolicy;
                    role.LegalEntity = base.LegalEntity;
                    role.RoleType = base.LookupRepository.RoleTypes.ObjectDictionary[Convert.ToString((int)SAHL.Common.Globals.RoleTypes.AssuredLife)];
                    role.GeneralStatus = base.LookupRepository.GeneralStatuses[GeneralStatuses.Active];
                    role.StatusChangeDate = System.DateTime.Now;

                    // Add the role to the AccountLifePolicy object
                    accountLifePolicy.Roles.Add(_view.Messages, role);

                    // Save the AccountLifePolicy (Role and Lifeinsurableinterest)
                    accountRepo.SaveAccount(accountLifePolicy);

                    // write the stage transition  record
                    int genericKey = -1, stageDefinitionGroupKey = -1;
                    string comments = base.LegalEntity.GetLegalName(LegalNameFormat.Full);
                    if (accountLifePolicy.LifePolicy != null)
                    {
                        genericKey = accountLifePolicy.Key;
                        comments += " added to policy";
                        stageDefinitionGroupKey = Convert.ToInt32(SAHL.Common.Globals.StageDefinitionGroups.LifeAdmin);
                    }
                    else
                    {
                        genericKey = accountLifePolicy.CurrentLifeApplication.Key;
                        comments += " added to application";
                        stageDefinitionGroupKey = Convert.ToInt32(SAHL.Common.Globals.StageDefinitionGroups.LifeOrigination);
                    }

                    IStageDefinitionRepository stageDefinitionRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
                    ISecurityRepository secRepo = RepositoryFactory.GetRepository<ISecurityRepository>();
                    stageDefinitionRepo.SaveStageTransition(genericKey, stageDefinitionGroupKey, SAHL.Common.Constants.StageDefinitionConstants.AddAssuredLife, comments, secRepo.GetADUserByPrincipal(_view.CurrentPrincipal));

                    // Recalculate Premiums 
                    lifeRepo.RecalculateSALifePremium(accountLifePolicy, true);
                }

                txn.VoteCommit();

                Navigator.Navigate("Submit");
            }
            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }
            finally
            {
                //db can rollback txn when rules fail, need to not throw ex 
                //if view is valid
                try
                {
                    txn.Dispose();
                }
                catch (Exception)
                {
                    if (_view.IsValid)
                        throw;
                }
            }
        }

        void OnReBindLegalEntity(object sender, KeyChangedEventArgs e)
        {
            // Persist the LegalEntityKey in the Global cache (and call the next presenter)
            GlobalCacheData.Remove(ViewConstants.SelectedLegalEntityKey);
            GlobalCacheData.Add(ViewConstants.SelectedLegalEntityKey, e.Key, new List<ICacheObjectLifeTime>());

            Navigator.Navigate("Rebind");
        }

        protected override void OnCancelButtonClicked(object sender, EventArgs e)
        {
            GlobalCacheData.Remove(ViewConstants.LegalEntity);
            GlobalCacheData.Remove(ViewConstants.SelectedLegalEntityKey);

            Navigator.Navigate("Cancel");
        }
    }
}