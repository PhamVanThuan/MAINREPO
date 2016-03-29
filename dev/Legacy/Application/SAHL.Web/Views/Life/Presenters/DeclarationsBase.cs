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
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Service;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.CacheData;

using Castle.ActiveRecord;
using SAHL.Common.DomainMessages;
using SAHL.Common;

namespace SAHL.Web.Views.Life.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class DeclarationsBase : SAHLCommonBasePresenter<IDeclarations>
    {
        private InstanceNode _node;
        private string _Activity;
        /// <summary>
        /// 
        /// </summary>
        public string Activity
        {
            set { _Activity = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public DeclarationsBase(IDeclarations view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            // Get the CBO Node 
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            if (GlobalCacheData.ContainsKey(ViewConstants.NavigateTo) == true)
                GlobalCacheData.Remove(ViewConstants.NavigateTo);

            GlobalCacheData.Add(ViewConstants.NavigateTo, _view.ViewName, new List<ICacheObjectLifeTime>());

        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        /// 
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _view.OnNextButtonClicked += new EventHandler(OnNextButtonClicked);

            // Get the LifeOgination Data to check if Declarations screen has been completed
            Dictionary<string, object> DC = _node.X2Data as Dictionary<string, object>;
            _view.ActivityDone = DC[_Activity] == DBNull.Value ? false : Convert.ToBoolean(DC[_Activity]);

            // get the life application
            IApplicationRepository applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            IApplicationLife applicationLife = applicationRepo.GetApplicationLifeByKey(_node.GenericKey);

            // load the relevant text statements depending on lifepolicy type
            int[] statementTypes = null;
            switch (applicationLife.LifePolicyType.Key)
            {
                case (int)SAHL.Common.Globals.LifePolicyTypes.StandardCover:
                    statementTypes = new int[] { (int)TextStatementTypes.Declaration };
                    break;
                case (int)SAHL.Common.Globals.LifePolicyTypes.AccidentOnlyCover:
                    statementTypes = new int[] { (int)TextStatementTypes.AccidentalDeclaration };
                    break;
                default:
                    break;
            }

            // bind the text statements
            ILifeRepository lifeRepo = RepositoryFactory.GetRepository<ILifeRepository>();
            IReadOnlyEventList<ITextStatement> lstStatements = lifeRepo.GetTextStatementsForTypes(statementTypes);
            _view.BindDeclarations(lstStatements);
        }


        /// <summary>
        /// Handles the Event that is fired by the view when the Next button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnNextButtonClicked(object sender, EventArgs e)
        {
            if (_view.ActivityDone == false && _view.AllOptionsChecked == false)
                _view.Messages.Add(new Error("All points must be accepted before you can continue.", "All points must be accepted before you can continue."));

            TransactionScope txn = new TransactionScope();
            try
            {
                // Navigate to the next State
                if (_view.IsValid)
                {
                    X2Service svc = ServiceFactory.GetService<IX2Service>() as X2Service;
                    svc.WorkFlowWizardNext(_view.CurrentPrincipal, _view.ViewName, _view.Navigator);
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
