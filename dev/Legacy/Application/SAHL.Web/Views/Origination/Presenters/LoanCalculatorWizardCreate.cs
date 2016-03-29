using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.X2.Framework.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Globals;

using SAHL.Common.Factories;
using SAHL.Common.CacheData;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace SAHL.Web.Views.Origination.Presenters
{
    public class LoanCalculatorWizardCreate : LoanCalculatorBase
    {
        ILegalEntityRepository _leRepo;
        ILegalEntityNaturalPerson _lenp;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LoanCalculatorWizardCreate(SAHL.Web.Views.Origination.Interfaces.ILoanCalculator view, SAHLCommonBaseController controller)
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

            // Depends on x2 stuff...
            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnCreateApplicationButtonClicked += new EventHandler(_view_OnCreateApplicationButtonClicked);
            _view.CreateApplicationButtonText = "Next";
            //_view.IsWizardMode = true;
            

            _view.PopulateMarketingSource(LookupRepo.ApplicationSources);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_OnCreateApplicationButtonClicked(object sender, EventArgs e)
        {
            TransactionScope txn = new TransactionScope();

            try
            {
                
                
                _lenp = (ILegalEntityNaturalPerson)LERepo.GetLegalEntityByKey(_view.ExistingLegalEntityKey);
                

                CreateNewApplication(); //populates _app

                if (_view.MarketingSource.Length > 0)
                    _app.ApplicationSource = LookupRepo.ApplicationSources.ObjectDictionary[_view.MarketingSource];				               

                IApplicationRole applicationRole = _app.AddRole((int)OfferRoleTypes.LeadMainApplicant, _lenp);

                // add the 'income contributor' application role attribute
                IApplicationRoleAttribute applicationRoleAttribute = AppRepo.GetEmptyApplicationRoleAttribute();
                applicationRoleAttribute.OfferRole = applicationRole;
                applicationRoleAttribute.OfferRoleAttributeType = AppRepo.GetApplicationRoleAttributeTypeByKey((int)SAHL.Common.Globals.OfferRoleAttributeTypes.IncomeContributor);
                applicationRole.ApplicationRoleAttributes.Add(_view.Messages, applicationRoleAttribute);

                AppRepo.SaveApplication(_app);

                // once we have an application create a workflow case
                Dictionary<string, string> Inputs = new Dictionary<string, string>();
                Inputs.Add("ApplicationKey", _app.Key.ToString());
                IX2Info XI = X2Service.GetX2Info(_view.CurrentPrincipal);
                if (XI == null || String.IsNullOrEmpty(XI.SessionID))
                    X2Service.LogIn(_view.CurrentPrincipal);

                X2Service.CreateWorkFlowInstance(_view.CurrentPrincipal, SAHL.Common.Constants.WorkFlowProcessName.Origination, (-1).ToString(), SAHL.Common.Constants.WorkFlowName.ApplicationCapture, SAHL.Common.Constants.WorkFlowActivityName.ApplicationWizardCreate, Inputs, false);
                if (_view.Messages.Count != 0)
                {
                    X2Service.CancelActivity(_view.CurrentPrincipal);
                    return;
                }

                X2Service.CreateCompleteActivity(_view.CurrentPrincipal, Inputs, false, null);

                // navigate to workflow
                if (GlobalCacheData.ContainsKey(ViewConstants.SelectedLegalEntityKey))
                    GlobalCacheData.Remove(ViewConstants.SelectedLegalEntityKey);

                if (GlobalCacheData.ContainsKey(ViewConstants.NavigateTo))
                    GlobalCacheData.Remove(ViewConstants.NavigateTo);

                IList<ICacheObjectLifeTime> lifeTimes = new List<ICacheObjectLifeTime>();
                GlobalCacheData.Add(ViewConstants.SelectedLegalEntityKey, _lenp.Key, lifeTimes);
                GlobalCacheData.Add(ViewConstants.NavigateTo, "ClientSuperSearch", lifeTimes);

                Navigator.Navigate("LegalEntityAddressAdd");

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
		       

        private ILegalEntityRepository LERepo
        {
            get
            {
                if (_leRepo == null)
                    _leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();

                return _leRepo;
            }
        }
    }
}
