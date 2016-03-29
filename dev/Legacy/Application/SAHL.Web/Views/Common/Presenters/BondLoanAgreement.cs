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


namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class BondLoanAgreement : BondLoanAgreementBase
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public BondLoanAgreement(IBondLoanAgreement view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            _view.BondGridPostBack = true; //Only enable postback when doing updates, needs to be set before the base initialises

            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.OnBondGrid_SelectedIndexChanged += new EventHandler(_view_BondGrid_SelectedIndexChanged);
        }

        /// <summary>
        /// Set the relevant properties for displaying controls within the view
        /// </summary>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return; 

            _view.ShowCancel = false;
            _view.ShowSubmit = false;
            _view.UpdateBond = false;
            _view.AddLoanAgreement = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void _view_BondGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowLoanAgreements_BondSelected();
            //IEventList<ILoanAgreement> listLA = null;

            //foreach (IBond bond in _view.Bonds)
            //{
            //    if (bond.Key == _view.BondGridSelectedValue)
            //    {
            //        listLA = bond.LoanAgreements;
            //        break;
            //    }
            //}

            //_view.LoanAgreements = listLA;
            //_view.BindLoanAgreement();
        }
    }
}
