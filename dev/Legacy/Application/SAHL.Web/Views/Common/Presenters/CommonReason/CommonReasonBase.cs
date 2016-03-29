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
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using System.Diagnostics.CodeAnalysis;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using System.Web.UI.MobileControls;
using SAHL.Common.CacheData;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Common.Presenters.CommonReason
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class CommonReasonBase : SAHLCommonBasePresenter<ICommonReason>
    {
        protected CBOMenuNode _node;
        protected int _genericKey;
        private int _originalGenericKey; //keep the original generic key, cause some presenters set the _genericKey to another key
        private int _genericKeyTypeKey;
        protected List<SelectedReason> _selectedReasons;
        private IList<int> _insertedReasonKeys = new List<int>();
        protected IReasonRepository _reasonRepo;
        protected IStageDefinitionRepository _stageDefinitionRepo;
        protected ILookupRepository _lookUpRepo;
        protected IApplicationRepository _appRepo;


        public IList<int> InsertedReasonKeys
        {
            get { return _insertedReasonKeys; }
            set { _insertedReasonKeys = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int GenericKey
        {
            get { return _genericKey; }
            set { _genericKey = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<SelectedReason> SelectedReasons
        {
            get { return _selectedReasons; }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<int> sdsdgKeys { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CommonReasonBase(ICommonReason view, SAHLCommonBaseController controller)
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

            if (!_view.ShouldRunPage) return;

            sdsdgKeys = new List<int>();

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            //set the generic key
            if (_node != null)
            {
                _genericKey = int.Parse(_node.GenericKey.ToString());
                //keep the original generic key, cause some presenters set this to another key
                _originalGenericKey = _genericKey;
                _genericKeyTypeKey = int.Parse(_node.GenericKeyTypeKey.ToString());
            }


            _view.OnSubmitButtonClicked += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnSubmitButtonClicked);
            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);

            _reasonRepo = RepositoryFactory.GetRepository<IReasonRepository>();
            _stageDefinitionRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
            _lookUpRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            _appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

            List<int> reasonTypeKeys = new List<int>();

            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);

            if (spc.Principal.IsInRole("Branch Consultant") && String.Compare(_view.ViewName, "WF_Decline", true) == 0)
            {
                reasonTypeKeys.Add((int)SAHL.Common.Globals.ReasonTypes.BranchDecline);
            }
            else
            {
                string l = _view.ViewAttributes["reasontypekey"];

                string[] groups = l.Split(',');
                for (int x = 0; x < groups.Length; x++)
                {
                    try
                    {
                        reasonTypeKeys.Add(int.Parse(groups[x]));
                    }
                    catch
                    {
                        //todo : handle error
                    }
                }
            }

            //get a list of sdsdg's so that we can update the reason with the transition key that was written
            if (_view.ViewAttributes.ContainsKey("stagedefinitionstagedefinitionkey"))
            {
                string[] sdsdgs = _view.ViewAttributes["stagedefinitionstagedefinitionkey"].Split(',');
                for (int x = 0; x < sdsdgs.Length; x++)
                {
                    try
                    {
                        sdsdgKeys.Add(int.Parse(sdsdgs[x]));
                    }
                    catch
                    {
                        //todo : handle error
                    }
                }
            }

            if (reasonTypeKeys.Count > 0)
            {
                List<IReasonType> lstReasonTypes = new List<IReasonType>();
                for (int x = 0; x < reasonTypeKeys.Count; x++)
                {
                    IReasonType rt = _reasonRepo.GetReasonTypeByKey(reasonTypeKeys[x]);
                    lstReasonTypes.Add(rt);
                }
                _view.BindReasonTypes(lstReasonTypes);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            CancelActivity();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {

            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;
            _view.CancelButtonVisible = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void _view_OnSubmitButtonClicked(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            TransactionScope ts = new TransactionScope();
            try
            {
                SaveReasons(e);
                ts.VoteCommit();
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected void SaveReasons(SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            _selectedReasons = (List<SelectedReason>)e.Key;

            for (int x = 0; x < _selectedReasons.Count; x++)
            {
                //populate the reason
                IReason res = _reasonRepo.CreateEmptyReason();
                res.Comment = _selectedReasons[x].Comment;
                res.GenericKey = _genericKey;
                res.ReasonDefinition = _reasonRepo.GetReasonDefinitionByKey(_selectedReasons[x].ReasonDefinitionKey);

                if (res.ReasonDefinition.ReasonType.GenericKeyType.Key == (int)SAHL.Common.Globals.GenericKeyTypes.OfferInformation)
                {
                    IApplicationRepository AR = RepositoryFactory.GetRepository<IApplicationRepository>();
                    IApplication app = AR.GetApplicationByKey(_genericKey);
                    if (app != null)
                    {
                        IApplicationInformation appInfo = app.GetLatestApplicationInformation();
                        res.GenericKey = appInfo.Key;
                    }
                }

                _reasonRepo.SaveReason(res);
                _insertedReasonKeys.Add(res.Key);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateReasonsWithStageTransitionKey()
        {

            if (sdsdgKeys == null || sdsdgKeys.Count < 1)
                return;

            TransactionScope txn = new TransactionScope(TransactionMode.Inherits);

            try
            {
                // only use the first stage transition
                IStageTransition stageTransition = _stageDefinitionRepo.GetLastStageTransitionByGenericKeyAndSDSDGKey(_originalGenericKey, _genericKeyTypeKey, sdsdgKeys[0]);

                // update the reasons with the stage transition key
                if (_insertedReasonKeys != null)
                {
                    foreach (int reasonKey in _insertedReasonKeys)
                    {
                        IReason reason = _reasonRepo.GetReasonByKey(reasonKey);

                        if (reason != null)
                        {
                            reason.StageTransition = stageTransition;
                            _reasonRepo.SaveReason(reason);
                        }
                    }
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

        /// <summary>
        /// 
        /// </summary>
        public abstract void CancelActivity();

        /// <summary>
        /// 
        /// </summary>
        public abstract void CompleteActivityAndNavigate();
    }
}
