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
using System.Collections.Generic;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Exceptions;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Service;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.CacheData;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.X2.Framework.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.CommonReason
{
    public class CommonReasonLifeProductSwitch : CommonReasonBase
    {
        private int _lifePolicyTypeKey = -1;
        private IApplicationLife _applicationLife;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CommonReasonLifeProductSwitch(ICommonReason view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            // Override the Node - Generic Key so we are certain that we are dealing with the correct key.
            GenericKey = Convert.ToInt32(GlobalCacheData["ApplicationLifeKey"]);
            _lifePolicyTypeKey = Convert.ToInt32(GlobalCacheData["LifePolicyTypeKey"]);
            _applicationLife = _appRepo.GetApplicationLifeByKey(GenericKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void _view_OnSubmitButtonClicked(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            TransactionScope ts = new TransactionScope();
            try
            {
                // We need to do both in the same scope 
                // Save the reasons away
                SaveReasons(e);

                // Then update the Life Application
                _applicationLife.LifePolicyType = _lookUpRepo.LifePolicyTypes.ObjectDictionary[_lifePolicyTypeKey.ToString()];
                _appRepo.SaveApplication(_applicationLife);

                // this commit must happen before the premium recalc below as the recalc calls a stored proc and needs the status to be updated/committed
                ts.VoteCommit();

                // recalculate premiums
                ILifeRepository lifeRepo = RepositoryFactory.GetRepository<ILifeRepository>();
                IAccountLifePolicy accountLifePolicy = _applicationLife.Account as IAccountLifePolicy;
                lifeRepo.RecalculateSALifePremium(accountLifePolicy, true);
            }
            catch (Exception)
            {
                CancelActivity();
                ts.VoteRollBack();

                if (_view.IsValid)
                    throw;
            }
            finally
            {
                ts.Dispose();
            }

            if (_view.Messages.Count > 0)
                return;

            CompleteActivityAndNavigate();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void CancelActivity()
        {
            base.X2Service.CancelActivity(_view.CurrentPrincipal);
            base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }

        /// <summary>
        /// _applicationLife
        /// </summary>
        public override void CompleteActivityAndNavigate()
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            IX2Info x2Info = spc.X2Info as IX2Info;
            TransactionScope txn = new TransactionScope();
            try
            {
                /*  
                 * The flags in "X2DATA.LifeOrigination" needs to be 
                 * reset at this stage depending on the policy type selected 
                 */
                int done = 0;
                Dictionary<string, string> inputs = new Dictionary<string, string>();

                if (_applicationLife.LifePolicyType.Key == (int)LifePolicyTypes.AccidentOnlyCover)
                {
                    inputs.Add("BenefitsDone", done.ToString());
                }
                inputs.Add("ExclusionsDone", done.ToString());
                inputs.Add("RPARDone", done.ToString());
                inputs.Add("DeclarationDone", done.ToString());
                inputs.Add("FAISDone", done.ToString());
                inputs.Add("ConfirmationRequired", false.ToString());
                inputs.Add("ExclusionsConfirmationDone", done.ToString());
                inputs.Add("DeclarationConfirmationDone", done.ToString());
                inputs.Add("FAISConfirmationDone", done.ToString());

                IX2Service svc = ServiceFactory.GetService<IX2Service>() as X2Service;
                X2ServiceResponse rsp = svc.CompleteActivity(_view.CurrentPrincipal, inputs, false);
                if (base.sdsdgKeys.Count > 0)
                {
                    UpdateReasonsWithStageTransitionKey();
                }
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

            GlobalCacheData.Remove(ViewConstants.InstanceID);
            GlobalCacheData.Add(ViewConstants.InstanceID, x2Info.InstanceID, new List<ICacheObjectLifeTime>());
            base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
            Navigator.Navigate("X2InstanceRedirect");
        }
    }
}

