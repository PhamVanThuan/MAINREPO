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
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Globals;
using SAHL.Common.Service;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.CacheData;

using SAHL.Common.DomainMessages;
using SAHL.Common;

namespace SAHL.Web.Views.Life.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class FAISBase : SAHLCommonBasePresenter<IFAIS>
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

        public InstanceNode Node
        {
            get { return _node; }
            set { _node = value; }
        }
	
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public FAISBase(IFAIS view, SAHLCommonBaseController controller)
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
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            // Get the LifeOgination Data to check if FAIS screen has been completed
            Dictionary<string, object> x2Data = _node.X2Data as Dictionary<string, object>;
            _view.ActivityDone = x2Data[_Activity] == DBNull.Value ? false : Convert.ToBoolean(x2Data[_Activity]);

            _view.ConfirmationRequired = Convert.ToBoolean(x2Data["ConfirmationRequired"]);
            _view.ContactNumber = Convert.ToString(x2Data["ContactNumber"]);

            // get the life application
            IApplicationRepository applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            IApplicationLife applicationLife = applicationRepo.GetApplicationLifeByKey(_node.GenericKey);

            // load the relevant text statements depending on lifepolicy type
            _view.LifePolicyTypeKey = applicationLife.LifePolicyType.Key;

            int[] statementTypes = null;
            switch (applicationLife.LifePolicyType.Key)
            {
                case (int)SAHL.Common.Globals.LifePolicyTypes.StandardCover:
                    statementTypes = new int[] { (int)TextStatementTypes.FAIS };
                    break;
                case (int)SAHL.Common.Globals.LifePolicyTypes.AccidentOnlyCover:
                    statementTypes = new int[] { (int)TextStatementTypes.AccidentalFAIS };
                    break;
                default:
                    break;
            }

            // bind the text statements
            ILifeRepository lifeRepo = RepositoryFactory.GetRepository<ILifeRepository>();
            IReadOnlyEventList<ITextStatement> lstStatements = lifeRepo.GetTextStatementsForTypes(statementTypes);
            _view.BindFAIS(lstStatements);
        }

        protected void ValidateInput()
        {
            if (_view.ActivityDone == false)
            {
                if (_view.AllOptionsChecked == false)
                    _view.Messages.Add(new Error("All points must be accepted before you can continue.", "All points must be accepted before you can continue."));
                if (_view.ContactOptionError == true)
                    _view.Messages.Add(new Error("Must select whether there is a second Life Insured.", "Must select whether there is a second Life Insured."));
                if (_view.ContactNumberError == true)
                    _view.Messages.Add(new Error("Contact Number must be entered.", "Contact Number must be entered."));
            }
        }
    }
}
