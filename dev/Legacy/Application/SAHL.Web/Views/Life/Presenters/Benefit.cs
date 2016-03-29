using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Service;
using SAHL.Common.Service.Interfaces;
using System.Collections.Generic;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;

using Castle.ActiveRecord;
using SAHL.Common.DomainMessages;
using SAHL.Common;

namespace SAHL.Web.Views.Life.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class Benefit : SAHLCommonBasePresenter<IBenefit>
    {
        private InstanceNode _node;
        private const string _activityDoneString = "BenefitsDone";
        private const string _benefitsConfirmProceedString = "BenefitsConfirmProceed";
        private const string _benefitsConfirmRefusedString = "BenefitsConfirmRefused";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public Benefit(IBenefit view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            // Get the CBO Node     
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            if (GlobalCacheData.ContainsKey(ViewConstants.NavigateTo) == true)
                GlobalCacheData.Remove(ViewConstants.NavigateTo);

            GlobalCacheData.Add(ViewConstants.NavigateTo, _view.ViewName, new List<ICacheObjectLifeTime>());

            // Get the LifeOgination Data to check if Benefits screen has been completed
            Dictionary<string, object> DC = _node.X2Data as Dictionary<string, object>;
            _view.ActivityDone = DC[_activityDoneString] == System.DBNull.Value ? false : Convert.ToBoolean(DC[_activityDoneString]);
            _view.ProceedWithPolicy = DC[_benefitsConfirmProceedString] == System.DBNull.Value ? false : Convert.ToBoolean(DC[_benefitsConfirmProceedString]);
            _view.RefusedPolicy = DC[_benefitsConfirmRefusedString] == System.DBNull.Value ? false : Convert.ToBoolean(DC[_benefitsConfirmRefusedString]);

        }
        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;


            _view.OnNextButtonClicked += new EventHandler(OnNextButtonClicked);

            // get the life application
            IApplicationRepository applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            IApplicationLife applicationLife = applicationRepo.GetApplicationLifeByKey(_node.GenericKey);

            // load the relevant text statements depending on lifepolicy type
            int[] statementTypes = null;
            switch (applicationLife.LifePolicyType.Key)
            {
                case (int)SAHL.Common.Globals.LifePolicyTypes.StandardCover:
                    statementTypes = new int[] { (int)TextStatementTypes.Benefits1, (int)TextStatementTypes.Benefits2 };
                    break;
                case (int)SAHL.Common.Globals.LifePolicyTypes.AccidentOnlyCover:
                    statementTypes = new int[] { (int)TextStatementTypes.AccidentalBenefits1, (int)TextStatementTypes.AccidentalBenefits2 };
                    break;
                default :
                    break;
            }

            // bind the text statements
            ILifeRepository lifeRepo = RepositoryFactory.GetRepository<ILifeRepository>();
            IReadOnlyEventList<ITextStatement> lstTxtStatement = lifeRepo.GetTextStatementsForTypes(statementTypes);
            _view.LifePolicyType = (LifePolicyTypes) applicationLife.LifePolicyType.Key;
            _view.BindBenefit(lstTxtStatement);
        }

        /// <summary>
        /// Handles the event fired by the view when the Next button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnNextButtonClicked(object sender, EventArgs e)
        {
            if (_view.ActivityDone == false)
            {
                if ((_view.ProceedWithPolicy == true && _view.RefusedPolicy == true) || (_view.ProceedWithPolicy == false && _view.RefusedPolicy == false))
                {
                    _view.Messages.Add(new Error("Please tick either checkbox: Client is proceeding with policy OR Client has refused cover.", "Please tick either checkbox: Client is proceeding with policy OR Client has refused cover."));
                }
            }

            TransactionScope txn = new TransactionScope();
            try
            {
                // Navigate to the next State
                if (_view.IsValid)
                {
                    Dictionary<string, string> x2Data = new Dictionary<string, string>();
                    x2Data.Add("BenefitsConfirmProceed", _view.ProceedWithPolicy.ToString());
                    x2Data.Add("BenefitsConfirmRefused", _view.RefusedPolicy.ToString());

                    X2Service svc = ServiceFactory.GetService<IX2Service>() as X2Service;
                    svc.WorkFlowWizardNext(_view.CurrentPrincipal, _view.ViewName, _view.Navigator, x2Data);
                }
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
    }
}
