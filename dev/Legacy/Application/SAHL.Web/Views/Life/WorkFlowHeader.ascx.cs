using System;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Factories;
using SAHL.Common;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Life
{
    public partial class WorkFlowHeader : System.Web.UI.UserControl
    {
        private InstanceNode _instanceNode;
        private CBONode _cboNode;
        //private ICBOService CBOService;
        private IViewBase _view;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            _view = Page as IViewBase;

            //CBOManager = ServiceFactory.GetService<ICBOService>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            _cboNode = _view.CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2);

            if (_cboNode!= null && _cboNode is InstanceNode)
            {
                _instanceNode = _cboNode as InstanceNode;

                IApplicationRepository applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                // get the application
                IApplication application = applicationRepo.GetApplicationByKey((int)_instanceNode.GenericKey);
                IApplicationLife applife = application as IApplicationLife;

                if (applife == null)
                    return;

                // Get the Life Account object
                IAccountLifePolicy accountLifePolicy = application.Account as IAccountLifePolicy;

                // Get Loan Account
                IMortgageLoanAccount mortgageloanAccount = accountLifePolicy.ParentMortgageLoan as IMortgageLoanAccount;

                // Set the controls
                lblClientName.Text = mortgageloanAccount.GetLegalName(LegalNameFormat.InitialsOnly);
                lblLoanNumber.Text = mortgageloanAccount.Key.ToString();

                ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

                lblPolicyType.Text = lookupRepo.LifePolicyTypes.ObjectDictionary[((int)LifePolicyTypes.StandardCover).ToString()].Description;
                
                if (applife.LifePolicyType != null)
                {
                    lblPolicyType.Text = applife.LifePolicyType.Description;
                }

                if (mortgageloanAccount.SecuredMortgageLoan != null)
                {
                    lblLoanNumber.Text += " (" + mortgageloanAccount.SecuredMortgageLoan.MortgageLoanPurpose.Description + ")";
                }
            }
        }
    }
}