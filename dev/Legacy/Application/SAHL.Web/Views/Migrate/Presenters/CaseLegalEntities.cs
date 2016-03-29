using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.CacheData;
using SAHL.Web.Views.Migrate.Interfaces;
using SAHL.Common.Web.UI;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Migrate.Presenters
{
    public class CaseLegalEntities : CreateBase
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CaseLegalEntities(ICreateCase view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// OnView Initialised event - retrieve data for use by presenters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSaveLegalEntitiesClicked);
            _view.WizardPage = 1;

            if (GlobalCacheData.ContainsKey(ViewConstants.CancelView))
                GlobalCacheData.Remove(ViewConstants.CancelView);

            if (GlobalCacheData.ContainsKey(ViewConstants.SelectView))
                GlobalCacheData.Remove(ViewConstants.SelectView);


            GlobalCacheData.Add(ViewConstants.CancelView, "CaseLegalEntities", LifeTimes);
            GlobalCacheData.Add(ViewConstants.SelectView, "CaseDetail", LifeTimes);
        }

        protected IMigrationDebtCounsellingExternalRole GetExternalRole(int LEKey)
        {
            foreach (IMigrationDebtCounsellingExternalRole er in _view.SelectedCase.DebtCounsellingExternalRoles)
            {
                if (er.LegalEntityKey == LEKey)
                    return er;
            }

            return null;
        }

        protected void _view_OnSaveLegalEntitiesClicked(object sender, EventArgs e)
        {
            
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);

            using (TransactionScope txn = new TransactionScope())
            {
                try
                {
                    //Ensure we have a case first
                    if (_view.SelectedCase == null)
                    {
                        _view.SelectedCase = MDCRepo.CreateEmptyDebtCounselling();
                        _view.SelectedCase.AccountKey = _view.AccountKey;
                        _view.SelectedCase.ProposalTypeKey = (int)ProposalTypes.Proposal;

                        MDCRepo.SaveDebtCounselling(_view.SelectedCase);
                    }

                    foreach (KeyValuePair<int, bool> kv in _view.ListDCLegalEntities)
                    {
                        IMigrationDebtCounsellingExternalRole er = GetExternalRole(kv.Key);
                        if (kv.Value)
                        {
                            //if not exists, add
                            if (er == null)
                            {
                                er = MDCRepo.CreateEmptyExternalRole();

                                er.DebtCounselling = _view.SelectedCase;
                                er.ExternalRoleTypeKey = (int)ExternalRoleTypes.Client;
                                er.LegalEntityKey = kv.Key;

                                _view.SelectedCase.DebtCounsellingExternalRoles.Add(spc.DomainMessages, er);
                                MDCRepo.SaveMigrationDebtCounsellingExternalRole(er);
                            }
                        }
                        else
                        {
                            //if exists, remove
                            if (er != null)
                            {
                                MDCRepo.DeleteMigrationDebtCounsellingExternalRole(er);
                            }
                        }
                    }

                    MDCRepo.SaveDebtCounselling(_view.SelectedCase);

                    txn.VoteCommit();
                }
                catch (Exception)
                {
                    txn.VoteRollBack();
                    if (_view.IsValid)
                        throw;
                }
            }

            if (_view.IsValid)
                Navigate();
        }
    }
}