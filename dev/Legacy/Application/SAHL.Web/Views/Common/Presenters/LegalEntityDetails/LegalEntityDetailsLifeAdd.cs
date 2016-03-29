using System;
using SAHL.Web.Views.Common.Interfaces; 
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Globals;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.Common.Presenters.LegalEntityDetails
{
    public class LegalEntityDetailsLifeAdd : LegalEntityDetailsAddBase
    {
        public LegalEntityDetailsLifeAdd(ILegalEntityDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            // Bind additional life-specific stuff
            _view.BindInsurableInterestType(LookupRepository.LifeInsurableInterestTypes.BindableDictionary, String.Empty);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {

            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.InsurableInterestUpdateVisible = true;
            _view.InsurableInterestDisplayVisible = false;
            _view.CancelButtonVisible = true;
            _view.SubmitButtonText = "Add New";
            _view.AddRoleTypeVisible = false;
            _view.LegalEntityTypeEnabled = false;

        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2219:DoNotRaiseExceptionsInExceptionClauses", Justification = "New rule added to FxCop, avoiding refactor")]
        protected override void SubmitButtonClicked(object sender, EventArgs e)
        {
            // Create a blank LE populate it and save it
            base.LegalEntity = LegalEntityRepository.GetEmptyLegalEntity((LegalEntityTypes)View.SelectedLegalEntityType);
            base.LegalEntity.IntroductionDate = DateTime.Now;

            // Get the details from the screen
            _view.PopulateLegalEntityDetailsForAdd(base.LegalEntity);
            // Populate the marketing options ...
            base.PopulateMarketingOptions();

            TransactionScope txn = new TransactionScope();

            try
            {
                // Save the legal entity 
                base.LegalEntityRepository.SaveLegalEntity(base.LegalEntity, false);

                // save the assured life role
                if (GlobalCacheData.ContainsKey(ViewConstants.SelectedLifeAccountKey))
                {
                    int lifeAccountKey = Convert.ToInt32(GlobalCacheData[ViewConstants.SelectedLifeAccountKey]);
                    IAccountRepository accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                    ILifeRepository lifeRepo = RepositoryFactory.GetRepository<ILifeRepository>();
                    IAccountLifePolicy accountLifePolicy = accountRepo.GetAccountByKey(lifeAccountKey) as IAccountLifePolicy;

                    // Populate the LifeInsurableInterest object
                    if (_view.SelectedInsurableInterestTypeAdd > -1)
                    {
                        ILifeInsurableInterest lifeInsurableInterest = lifeRepo.CreateEmptyLifeInsurableInterest();
                        lifeInsurableInterest.Account = accountLifePolicy;
                        lifeInsurableInterest.LegalEntity = base.LegalEntity;
                        lifeInsurableInterest.LifeInsurableInterestType = base.LookupRepository.LifeInsurableInterestTypes.ObjectDictionary[Convert.ToString(_view.SelectedInsurableInterestTypeAdd)];

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

        protected override void ReBindLegalEntity(object sender, KeyChangedEventArgs e)
        {
            base.ReBindLegalEntity(sender, e);

            Navigator.Navigate("Rebind");
        }

        protected override void CancelButtonClicked(object sender, EventArgs e)
        {
            GlobalCacheData.Remove(ViewConstants.LegalEntity);
            GlobalCacheData.Remove(ViewConstants.SelectedLegalEntityKey);

            Navigator.Navigate("Cancel");
        }
    }
}
