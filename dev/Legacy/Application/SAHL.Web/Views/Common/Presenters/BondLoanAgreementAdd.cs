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
using SAHL.Common.UI;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.Common.Presenters
{
    public class BondLoanAgreementAdd : BondLoanAgreementBase
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public BondLoanAgreementAdd(IBondLoanAgreement view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            _view.BondGridPostBack = true; //needs to be set before the base initialises

            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);
            _view.OnBondGrid_SelectedIndexChanged += new EventHandler(_view_BondGrid_SelectedIndexChanged);

            _view.LoanAgreementDate = DateTime.Now;
        }

        /// <summary>
        /// Set the relevant properties for displaying controls within the view
        /// </summary>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
           
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return; 

            _view.UpdateBond = false;
            _view.ShowBondGrid = true;
            _view.SubmitButtonText = "Add";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            ILoanAgreementRepository laRepo = RepositoryFactory.GetRepository<ILoanAgreementRepository>();
            //ILoanAgreement la = laRepo.GetEmptyLoanAgreement(_view.Messages);

            // TODO: this is weird - why not just check the null value?  MattS
            DateTime AgreementDate;
            if (!DateTime.TryParse(_view.LoanAgreementDate.ToString(), out AgreementDate))
                _view.Messages.Add(new SAHL.Common.DomainMessages.Error("Please enter a valid Loan Agreement date.", "Please enter a valid Loan Agreement date."));

            double Amount = Convert.ToDouble(_view.LoanAgreementAmount.Length > 0 ? _view.LoanAgreementAmount : "0");
            string UserName = _view.CurrentPrincipal.Identity.Name;
            IBond Bond = SelectedBond();
            //populate the selected bonds loan agreements in case of domain errors
            //after the view is set to InValid the session dies so lazy load errors if you try there
            ShowLoanAgreements_BondSelected();

            if (_view.IsValid)
            {
                TransactionScope txn = new TransactionScope();
                try
                {
                    laRepo.CreateLoanAgreement(AgreementDate, Amount, DateTime.Now, Bond, UserName);
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
            }

            if (_view.IsValid)
                Navigator.Navigate("BondLoanAgreement");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void _view_BondGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowLoanAgreements_BondSelected();
        }
    }
}
