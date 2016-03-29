using Castle.ActiveRecord;
using SAHL.Common.Web.UI;
using SAHL.V3.Framework;
using SAHL.V3.Framework.Repositories;
using SAHL.Web.Views.Common.Interfaces;
using System;

namespace SAHL.Web.Views.Common.Presenters
{
    public class ApplicationSummaryForResubCheck : ApplicationSummaryBase
    {
        private IV3ServiceManager v3ServiceManager;
        private IDecisionTreeRepository decisionTreeRepository;

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ApplicationSummaryForResubCheck(IApplicationSummary view, SAHLCommonBaseController controller)
            : base(view, controller) { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            if (!_view.ShouldRunPage)
                return;

            v3ServiceManager = V3ServiceManager.Instance;
            decisionTreeRepository = v3ServiceManager.Get<IDecisionTreeRepository>();

            // call the base presenter
            base.OnViewInitialised(sender, e);

            ConditionalConvertTo30YearLoan();
        }

        private void ConditionalConvertTo30YearLoan()
        {
            using (TransactionScope txn = new TransactionScope())
            {
                try
                {
                    if (AppRepo.DoesApplicationRequire30YearLoanTermConversion(Application))
                    {
                        var decisionTreeResult = decisionTreeRepository.QualifyApplication(Application.Key);
                        if (decisionTreeResult.QualifiesForThirtyYearTerm)
                        {
                            AppRepo.ConvertAcceptedApplicationToExtendedTerm(Application, decisionTreeResult.LoanDetailFor30YearTerm.Term.Value, decisionTreeResult.QualifiesForThirtyYearTerm, decisionTreeResult.PricingAdjustmentThirtyYear);
                            txn.VoteCommit();
                        }
                        else
                        {
                            v3ServiceManager.HandleSystemMessages(decisionTreeResult.Messages);
                        }
                    }
                }
                catch (Exception)
                {
                    txn.VoteRollBack();
                    if (_view.IsValid)
                        throw;
                }
            }
        }
    }
}