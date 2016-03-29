using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Globals;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using Castle.ActiveRecord;

namespace SAHL.Web.Views.Common.Presenters.Employment
{
    public class LegalEntitySubsidyDetailsAdd : LegalEntitySubsidyBase
    {

        private const string SubsidyKey = "SubsidyKey";
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LegalEntitySubsidyDetailsAdd(ISubsidyDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            View.SaveButtonClicked += new EventHandler(View_SaveButtonClicked);
            View.SubsidySelected += new KeyChangedEventHandler(View_SubsidySelected);

            _view.GridVisible = true;
            _view.ReadOnly = false;
            _view.ShowButtons = true;
            _view.ShowStatus = false;

            base.BindSubsidyAccounts();

            // get the current legal entities subsidies and bind them to the view
            ILegalEntity legalEntity = GetLegalEntity(View.CurrentPrincipal);
            _view.BindSubsidies(EmploymentRepository.GetSubsidiesByLegalEntityKey(legalEntity.Key));


        }

        void View_SubsidySelected(object sender, KeyChangedEventArgs e)
        {
            PrivateCacheData.Add(SubsidyKey, e.Key);
        }

        void View_SaveButtonClicked(object sender, EventArgs e)
        {
            // ICommonRepository
            IEmploymentSubsidised employmentPrev = null;
            IEmploymentSubsidised employmentNew = EmploymentRepository.GetEmptyEmploymentByType(CachedEmployment.EmploymentType) as IEmploymentSubsidised;
            CopyCachedValues(_view.CurrentPrincipal, employmentNew);

            // create the new subsidy object
            ISubsidy subsidyPrev = null;
            ISubsidy subsidyNew = View.GetCapturedSubsidy();

            if (PrivateCacheData.ContainsKey(SubsidyKey))
            {
                subsidyPrev = EmploymentRepository.GetSubsidyByKey(Convert.ToInt32(PrivateCacheData[SubsidyKey]));

                // set the employment record attached to the selected subsidy to previous
                employmentPrev = subsidyPrev.Employment;
                employmentPrev.EmploymentStatus = LookupRepository.EmploymentStatuses.ObjectDictionary[((int)EmploymentStatuses.Previous).ToString()];
                employmentPrev.EmploymentEndDate = DateTime.Now;

            }
            
            // set the details of the new subsidy
            subsidyNew.GeneralStatus = LookupRepository.GeneralStatuses[GeneralStatuses.Active];
            subsidyNew.LegalEntity = GetLegalEntity(View.CurrentPrincipal);
            subsidyNew.Employment = employmentNew;
            employmentNew.Subsidy = subsidyNew;

            // Solution 2
            // assume that OfferKey was selected
            if (subsidyNew.Application != null
                && subsidyNew.Application.Account != null
                && subsidyNew.Application.Account.AccountStatus.Key == (int)AccountStatuses.Application)
            {
                subsidyNew.Account = subsidyNew.Application.Account;
            }

            // validate the new subsidy, and if everything is fine we can save the employment record(s)
            subsidyNew.ValidateEntity();
            if (View.IsValid)
            {
                TransactionScope txn = new TransactionScope();
                try
                {
                    if (employmentPrev != null)
                        EmploymentRepository.SaveEmployment(employmentPrev);
                                        
                    //-----------------------
                    // Save the subsidy information here
                    //if (employmentNew.EmploymentType.Key == (int)EmploymentTypes.Subsidised)
                    //{
                    //    IApplication app = null;
                    //    IAccount acc = GetApplicationAccount(View.CurrentPrincipal, app);

                    //    if (acc != null)
                    //    {
                    //        if (acc.AccountStatus.Key == (int)AccountStatuses.Application)
                    //        {
                    //            //app.Subsidies.Add(null, subsidyNew);
                    //            //IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
                    //            //appRepo.SaveApplication(app);
                    //            acc.Subsidies.Add(null, subsidyNew);
                    //            IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                    //            accRepo.SaveAccount(acc);
                    //        }
                    //    }
                    //}

                    UpdateEmploymentVerificationProcess(_view, employmentNew);
                    EmploymentRepository.SaveEmployment(employmentNew);

                    //------------------------
                    ClearCachedData();
                    txn.VoteCommit();

                    View.ShouldRunPage = false;
                    View.Navigator.Navigate("LegalEntityEmploymentDisplay");
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
        }      
    }
}
