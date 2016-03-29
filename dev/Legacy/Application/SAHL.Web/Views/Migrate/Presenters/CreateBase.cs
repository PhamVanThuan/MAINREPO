using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.CacheData;
using SAHL.Web.Views.Migrate.Interfaces;
using SAHL.Common.Web.UI;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;


namespace SAHL.Web.Views.Migrate.Presenters
{
    public class CreateBase : SAHLCommonBasePresenter<ICreateCase>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CreateBase(ICreateCase view, SAHLCommonBaseController controller)
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

            if (GlobalCacheData.ContainsKey(ViewConstants.WizardPage))
                _view.WizardPage = Convert.ToInt32(GlobalCacheData[ViewConstants.WizardPage].ToString());

            if (GlobalCacheData.ContainsKey(ViewConstants.SelectedAccountKey))
                _view.AccountKey = Convert.ToInt32(GlobalCacheData[ViewConstants.SelectedAccountKey].ToString());

            //if we have a case, and the DC != the cache item
            //then we have returned from the select presenter and need to save the new key
            if (_view.SelectedCase != null
                && GlobalCacheData.ContainsKey(ViewConstants.DebtCounsellorLegalEntityKey))
            {
                _view.SelectedCase.DebtCounsellorKey = Convert.ToInt32(GlobalCacheData[ViewConstants.DebtCounsellorLegalEntityKey].ToString());

                using (TransactionScope txn = new TransactionScope())
                {
                    try
                    {
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
            }

            //reset the globalcache for the DC, could be more clever, but then become complicated logic
            //keep it simple, and this is temporary
            if (GlobalCacheData.ContainsKey(ViewConstants.SelectedTreeNodeKey))
                GlobalCacheData.Remove(ViewConstants.SelectedTreeNodeKey);

            if (_view.SelectedCase != null && _view.SelectedCase.DebtCounsellorKey.HasValue)
            {
                IList<ILegalEntityOrganisationStructure> leosList = OSRepo.GetLegalEntityOrganisationStructuresForLegalEntityKey(_view.SelectedCase.DebtCounsellorKey.Value);
                if (leosList != null && leosList.Count > 0)
                    GlobalCacheData.Add(ViewConstants.SelectedTreeNodeKey, leosList[0].Key, LifeTimes);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            if (_view.IsValid)
                Navigate();
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Navigate()
        {
            if (GlobalCacheData.ContainsKey(ViewConstants.SelectedAccountKey))
                GlobalCacheData.Remove(ViewConstants.SelectedAccountKey);

            GlobalCacheData.Add(ViewConstants.SelectedAccountKey, _view.AccountKey.ToString(), LifeTimes);

            if (GlobalCacheData.ContainsKey(ViewConstants.WizardPage))
                GlobalCacheData.Remove(ViewConstants.WizardPage);

            GlobalCacheData.Add(ViewConstants.WizardPage, _view.WizardPage.ToString(), LifeTimes);


            //we have set the int to the page we want to navigate to, 
            //so go there
            switch (_view.WizardPage)
            {
                case 0:
                    Navigator.Navigate("CreateMigrateDCCase");
                    break;
                case 1:
                    Navigator.Navigate("CaseLegalEntities");
                    break;
                case 2:
                    Navigator.Navigate("DebtCounsellorMigrateSelect");
                    break;
                case 3:
                    Navigator.Navigate("CaseDetail");
                    break;
                case 4:
                    Navigator.Navigate("CreateMigrateDCProposal");
                    break;
                default:
                    Navigator.Navigate(_view.ViewName);
                    break;
            }
        }

        private IMigrationDebtCounsellingRepository _mdcRepo;
        protected IMigrationDebtCounsellingRepository MDCRepo
        {
            get
            {
                if (_mdcRepo == null)
                    _mdcRepo = RepositoryFactory.GetRepository<IMigrationDebtCounsellingRepository>();

                return _mdcRepo;
            }
        }

        private IOrganisationStructureRepository _osRepo;
        protected IOrganisationStructureRepository OSRepo
        {
            get
            {
                if (_osRepo == null)
                    _osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();

                return _osRepo;
            }
        }

        private ILookupRepository _lkRepo;
        protected ILookupRepository LKRepo
        {
            get
            {
                if (_lkRepo == null)
                    _lkRepo = RepositoryFactory.GetRepository<ILookupRepository>();

                return _lkRepo;
            }
        }
        
        private ICommonRepository _cRepo;
        protected ICommonRepository CRepo
        {
            get
            {
                if (_cRepo == null)
                    _cRepo = RepositoryFactory.GetRepository<ICommonRepository>();

                return _cRepo;
            }
        }

        private IList<ICacheObjectLifeTime> _lifeTimes;
        public IList<ICacheObjectLifeTime> LifeTimes
        {
            get
            {
                if (_lifeTimes == null)
                {
                    List<string> views = new List<string>();
                    views.Add("CreateMigrateDCCase");
                    views.Add("CaseLegalEntities");
                    views.Add("DebtCounsellorMigrateSelect");
                    views.Add("CaseDetail");
                    views.Add("CreateMigrateDCProposal");

                    _lifeTimes = new List<ICacheObjectLifeTime>();
                    _lifeTimes.Add(new SimplePageCacheObjectLifeTime(views));
                }

                return _lifeTimes;
            }
        }
    }
}