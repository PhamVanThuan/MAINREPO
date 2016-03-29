using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.UI;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SAHL.Common.CacheData;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.DomainMessages;
using Castle.ActiveRecord;
using System.Linq;
using System.IO;
using System.Security.Principal;
using SAHL.Common.Service.Interfaces;
using System.Reflection;
using SAHL.Common.Logging;


namespace SAHL.Web.Views.DebtCounselling.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class ProposalSummaryBase : SAHLCommonBasePresenter<IProposalSummary>
    {
        public CBOMenuNode _node;
        public InstanceNode _instanceNode;
        public List<IProposal> _lstDebtCounsellingProposalSummary = new List<IProposal>();

        public enum ButtonClicked
        {
            Add, Update, View, Copy, SetActive, Delete, Reasons,Create, Print, Schedule
        }

        private int _genericKey;
        public int GenericKey
        {
            get { return _genericKey; }
            set { _genericKey = value; }
        }

        private int _genericKeyTypeKey;
        public int GenericKeyTypeKey
        {
            get { return _genericKeyTypeKey; }
            set { _genericKeyTypeKey = value; }
        }

        private IProposal _selectedProposal;
        public IProposal SelectedProposal
        {
            get { return _selectedProposal; }
            set { _selectedProposal = value; }
        }

        private int _accountKey;
        /// <summary>
        /// 
        /// </summary>
        public int AccountKey
        {
            get { return _accountKey; }
            set { _accountKey = value; }
        }
		private IControlRepository controlRepository;
		public IControlRepository ControlRepository
		{
			get
			{
				if (controlRepository == null)
					controlRepository = RepositoryFactory.GetRepository<IControlRepository>();

				return controlRepository;
			}
		}
        private ILookupRepository _lookupRepo;
        public ILookupRepository LookupRepo
        {
            get
            {
                if (_lookupRepo == null)
                    _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

                return _lookupRepo;
            }
        }

        private IDebtCounsellingRepository _debtCounsellingRepo;
        public IDebtCounsellingRepository DebtCounsellingRepo
        {
            get
            {
                if (_debtCounsellingRepo == null)
                    _debtCounsellingRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

                return _debtCounsellingRepo;
            }
        }

        private IList<ICacheObjectLifeTime> _lifeTimes;

        public IList<ICacheObjectLifeTime> LifeTimes
        {
            get
            {
                if (_lifeTimes == null)
                    _lifeTimes = new List<ICacheObjectLifeTime>();

                return _lifeTimes;
            }
        }

        /// <summary>
        /// Gets the <see cref="IADUser"/> for the current principal.
        /// </summary>
        protected IADUser CurrentADUser
        {
            get
            {
                ISecurityRepository secRepo = RepositoryFactory.GetRepository<ISecurityRepository>();
                return secRepo.GetADUserByPrincipal(_view.CurrentPrincipal); ;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ProposalSummaryBase(IProposalSummary view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            // Get the Instance Node   
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            if (_node is InstanceNode)
            {
                _instanceNode = _node as InstanceNode;
                _genericKey = _instanceNode.GenericKey; // this will be the debtcounsellingkey
                _genericKeyTypeKey = _instanceNode.GenericKeyTypeKey;
            }
            else
            {
                _genericKey = _node.GenericKey;
                _genericKeyTypeKey = _node.GenericKeyTypeKey;
            }

            _view.OnViewButtonClicked += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnViewButtonClicked);
            _view.OnPrintButtonClicked += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnPrintButtonClicked);
			_view.OnViewAmortisationScheduleClicked += OnViewAmmortisationScheduleClicked;

            BindProposals();

        }

        /// <summary>
        /// Set the relevant properties for displaying controls within the view
        /// </summary>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;

            //setup visible buttons
            _view.ShowViewButton = true;			
            _view.ShowCancelButton = false;
            _view.ShowAddButton = false;
            _view.ShowUpdateButton = false;
            _view.ShowSetActiveButton = false;
            _view.ShowCopyToDraftButton = false;
			_view.ShowDeleteButton = false;
            _view.ShowCreateCounterProposalButton = false;
            _view.ShowPrintButton = true;
        }

        void _view_OnViewButtonClicked(object sender, KeyChangedEventArgs e)
        {
            _selectedProposal = GetSelectedProposalByKey(Convert.ToInt32(e.Key));

            ValidateSelection(ButtonClicked.View);

            if (_view.IsValid)
            {
                GlobalCacheData.Remove(ViewConstants.ProposalKey);
                GlobalCacheData.Add(ViewConstants.ProposalKey, _selectedProposal.Key, LifeTimes);

                _view.Navigator.Navigate("View");
            }
        }

        void _view_OnPrintButtonClicked(object sender, KeyChangedEventArgs e)
        {
            _selectedProposal = GetSelectedProposalByKey(Convert.ToInt32(e.Key));

            ValidateSelection(ButtonClicked.Print);

            if (_view.IsValid)
            {
                IReportRepository repRep = RepositoryFactory.GetRepository<IReportRepository>();
                IReportStatement rs = repRep.GetReportStatementByName("Proposal Detail").Where(x => x.ReportType.Key == (int)ReportTypes.ReportingServicesReport).FirstOrDefault();

                if (rs == null)
                    return;

                GlobalCacheData.Remove(ViewConstants.ReportParameters);
                GlobalCacheData.Remove(ViewConstants.ReportPath);

                IReportParameter rp = rs.ReportParameters.Where(x => x.ParameterName == "ProposalKey").FirstOrDefault();

                if (rp == null)
                    return;

                Dictionary<IReportParameter, object> parameters = new Dictionary<IReportParameter, object> { {rp, _selectedProposal.Key} };

                GlobalCacheData.Add(ViewConstants.ReportPath, rs.StatementName, _lifeTimes);
                GlobalCacheData.Add(ViewConstants.ReportParameters, parameters, _lifeTimes);

                _view.Navigator.Navigate("Print");
            }
        }

		private void OnViewAmmortisationScheduleClicked(object sender, KeyChangedEventArgs e)
		{
            _selectedProposal = GetSelectedProposalByKey(Convert.ToInt32(e.Key));

            ValidateSelection(ButtonClicked.Schedule);

            if (_view.IsValid)
            {
                var ammortisationScheduleControl = new SAHL.Web.Controls.AmortisationSchedule();
                ammortisationScheduleControl.Render(Convert.ToInt32(e.Key), false);

                ISecurityService securityService = ServiceFactory.GetService<ISecurityService>();
                WindowsImpersonationContext windowsImpersonationContext = securityService.BeginImpersonation();

                var temporaryFilePath = ammortisationScheduleControl.SaveToPDFAndReturnFilePath();
                _view.SendPDFToClient(temporaryFilePath);

                securityService.EndImpersonation(windowsImpersonationContext);
            }
		}

        public void BindProposals()
        {
            _lstDebtCounsellingProposalSummary.Clear();

            List<IProposal> proplist = DebtCounsellingRepo.GetProposalsByGenericKey(_genericKey, _genericKeyTypeKey);

            // Get the list of proposals
            // only populate the list with the type that has been set in the view.
            foreach (IProposal prop in proplist)
            {
                if ((int)_view.ShowProposals == 0 || prop.ProposalType.Key == (int)_view.ShowProposals)
                    _lstDebtCounsellingProposalSummary.Add(prop);
            }

            // bind the list of proposals
            _view.BindProposalSummaryGrid(_lstDebtCounsellingProposalSummary);
        }

        public IProposal GetSelectedProposalByKey(int proposalKey)
        {
			try
			{
				if (proposalKey > 0)

					return DebtCounsellingRepo.GetProposalByKey(proposalKey);
				else
					return null;
			}
			catch
			{
				return null;

			};
        }

        public void ValidateSelection(ButtonClicked buttonClicked)
        {
            string errorMessage = "";

            _view.Messages.Clear();

            if (_selectedProposal == null && buttonClicked != ButtonClicked.Add)
            {
                errorMessage = "Must select a proposal";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
                return;
            }

            switch (buttonClicked)
            {
                case ButtonClicked.Add:
                    //check if there are any other draft proposals
                    IList<IProposal> draftProposals = DebtCounsellingRepo.GetProposalsByTypeAndStatus(_genericKey, ViewProposalType, ProposalStatuses.Draft);
                    if (draftProposals.Count > 0)
                        errorMessage = "A Draft proposal already exists - a new proposal cannot be added.";
                    break;

                case ButtonClicked.Update:
                    //check that the selected proposal is draft                        
                    if (_selectedProposal.ProposalStatus.Key != (int)SAHL.Common.Globals.ProposalStatuses.Draft)
                        errorMessage = "Can only update a Draft proposal.";
                    break;

                case ButtonClicked.Copy:
                    //check that the selected proposal is not a draft
                    if (_selectedProposal.ProposalStatus.Key == (int)SAHL.Common.Globals.ProposalStatuses.Draft)
                        errorMessage = "Cannot copy a draft proposal - it should be updated instead.";
                    else
                    {
                        //check if there are any other draft proposals
                        draftProposals = DebtCounsellingRepo.GetProposalsByTypeAndStatus(_genericKey, ViewProposalType, ProposalStatuses.Draft);
                        if (draftProposals.Count > 0)
                            errorMessage = "A Draft proposal already exists - cannot copy to a new draft.";
                    }
                    break;

                case ButtonClicked.SetActive:
                    //check that the selected proposal is a draft
                    if (_selectedProposal.ProposalStatus.Key != (int)SAHL.Common.Globals.ProposalStatuses.Draft)
                        errorMessage = "Only draft proposals can be made active.";

                    break;
                case ButtonClicked.Reasons:
                    break;

                case ButtonClicked.Create:
                    break;

                case ButtonClicked.Schedule:
                    break;

                case ButtonClicked.View:
                    break;
                default:
                    break;
            }

            if (!String.IsNullOrEmpty(errorMessage))
                _view.Messages.Add(new Error(errorMessage, errorMessage));
        }

        private ProposalTypes ViewProposalType
        {
            get
            {
                return (ProposalTypes)_view.ShowProposals;
            }
        }
    }
}
