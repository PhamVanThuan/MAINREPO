using Castle.ActiveRecord;
using SAHL.Common;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Origination.Interfaces;
using System;

namespace SAHL.Web.Views.Origination.Presenters
{
    public class ApplicationLoanDetailsUpdate : ApplicationLoanDetailsBase
    {
        public ApplicationLoanDetailsUpdate(IApplicationLoanDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _view.OnUpdateClicked += new EventHandler(OnSaveClicked);
        }

        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            base.OnViewLoaded(sender, e);
            if (!_view.ShouldRunPage)
                return;

            // Hide the readonly controls and show the editable ones
            _view.IsReadOnly = false;
            _view.IsSPVReadOnly = true;
            _view.IsDiscountReadonly = false;
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            GetApplicationFromCBO();

            if (!_view.ShouldRunPage)
                return;

            BindLookups();

            // Bind the Application
            _view.BindApplicationDetails(Application);
            _view.BindProduct(base.LookupRepository.Products.BindableDictionary[Convert.ToString((int)Application.CurrentProduct.ProductType)]);
            _view.SetSubmitCaption("Save");
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            if (!_view.ShouldRunPage)
                return;
        }

        protected virtual void OnSaveClicked(object sender, EventArgs e)
        {
            _view.GetApplicationDetails(Application);

            //errors could have been loaded in the previous calls
            if (!_view.IsValid || !PassValidation(Application))
                return;
            Application.CalculateApplicationDetail(_view.IsBondToRegisterExceptionAction, false);

            RuleServ.ExecuteRule(_view.Messages, "ProductSuperLoNewCat1", Application);
            RuleServ.ExecuteRule(_view.Messages, "ProductVarifixApplicationMinLoanAmount", Application);
            RuleServ.ExecuteRule(_view.Messages, "ProductVarifixApplicationFixedMinimum", Application);
            RuleServ.ExecuteRule(_view.Messages, "AlphaHousingLoanMustBeNewVariableLoan", Application);
            RuleServ.ExecuteRule(_view.Messages, "AlphaHousingLoanMustNotBeInterestOnlyLoan", Application);
            RuleServ.ExecuteRule(_view.Messages, Rules.ApplicationProductEdgeLTVWarning, _application);
            if (!_view.IsValid)
                return;

            TransactionScope trnScope = new TransactionScope();

            try
            {
                ExclusionSets.Add(RuleExclusionSets.LoanDetailsUpdate);
                ApplicationRepository.SaveApplication(Application);
                ExclusionSets.Remove(RuleExclusionSets.LoanDetailsUpdate);

                X2Service.CompleteActivity(_view.CurrentPrincipal, null, false);
                if (_view.IsValid)
                {
                    Navigator.Navigate("Update");
                }
                trnScope.VoteCommit();
            }
            catch (Exception)
            {
                trnScope.VoteRollBack();

                if (_view.IsValid)
                    throw;
            }
            finally
            {
                trnScope.Dispose();
            }
        }
    }
}