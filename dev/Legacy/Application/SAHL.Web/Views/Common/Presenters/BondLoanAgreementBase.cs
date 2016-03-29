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
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;

using System.Collections.Generic;
using SAHL.Common.Collections;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class BondLoanAgreementBase : SAHLCommonBasePresenter<IBondLoanAgreement>
    {
        private CBOMenuNode _cboNode;
        private IFinancialService _varFS;
        private IMortgageLoan _varML;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public BondLoanAgreementBase(IBondLoanAgreement view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_cboNode == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            switch (_cboNode.GenericKeyTypeKey)
            {
                case (int)SAHL.Common.Globals.GenericKeyTypes.FinancialService:
                    // Get the Financial Service 
                    IFinancialServiceRepository varFSRepo = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
                    _varFS = varFSRepo.GetFinancialServiceByKey(_cboNode.GenericKey);
                    break;
                case (int)SAHL.Common.Globals.GenericKeyTypes.Offer:
                    IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                    IAccount account = accRepo.GetAccountByApplicationKey(_cboNode.GenericKey);
                    if (account != null)
                        _varFS = account.GetFinancialServiceByType(FinancialServiceTypes.VariableLoan);
                    break;
                default:
                    break;
            }
            if (_varFS == null)
            {
                _view.AddLoanAgreement = false;
                _view.UpdateBond = false;
                _view.ShowSubmit = false;
            }
            else
            {
                // Get the ML
                _varML = _varFS as IMortgageLoan;

                // Sorting not implemented on DAOEventlists
                List<IBond> listBond = new List<IBond>(_varML.Bonds);
                listBond.Sort(delegate(IBond b1, IBond b2) { return b2.BondRegistrationDate.CompareTo(b1.BondRegistrationDate); });
                _view.Bonds = new EventList<IBond>(listBond);

                //sort loan agreements
                if (listBond.Count > 0)
                    _view.LoanAgreements = LoanAgreementSortedList(listBond[0].LoanAgreements);
                else
                {
                    _view.UpdateBond = false;
                    _view.AddLoanAgreement = false;
                    _view.ShowSubmit = false;
                }

                _view.BindBonds();
                _view.BindLoanAgreement();
            }
            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
        }

        private static IEventList<ILoanAgreement> LoanAgreementSortedList(IEventList<ILoanAgreement> bondlistLA)
        {
            List<ILoanAgreement> listLA = new List<ILoanAgreement>(bondlistLA);
            listLA.Sort(delegate(ILoanAgreement la1, ILoanAgreement la2) { return la2.AgreementDate.CompareTo(la1.AgreementDate); });
            return new EventList<ILoanAgreement>(listLA);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("BondLoanAgreement");
        }

        protected IBond SelectedBond()
        {
            foreach (IBond bond in VariableMortgageLoan.Bonds)
            {
                if (bond.Key == _view.BondGridSelectedKey)
                    return bond;
            }

            return null;
        }

        protected void ShowLoanAgreements_BondSelected()
        {
            IEventList<ILoanAgreement> listLA = null;

            foreach (IBond bond in VariableMortgageLoan.Bonds)
            {
                if (bond.Key == _view.BondGridSelectedKey)
                {
                    listLA = LoanAgreementSortedList(bond.LoanAgreements);
                    break;
                }
            }

            _view.LoanAgreements = new EventList<ILoanAgreement>(listLA); ;
            _view.BindLoanAgreement();
        }

        /// <summary>
        /// The Variable MortgageLoan object. All Bonds and Loan Agreements are saved against the variable Financial Service Key
        /// </summary>
        public IMortgageLoan VariableMortgageLoan
        {
            get { return _varML; }
            set { _varML = value; }
        }
    }
}
