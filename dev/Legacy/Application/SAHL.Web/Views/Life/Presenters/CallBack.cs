using System;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Service;
using SAHL.Common.DomainMessages;
using SAHL.Common;

namespace SAHL.Web.Views.Life.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class CallBack : SAHLCommonBasePresenter<SAHL.Web.Views.Life.Interfaces.ICallBack>
    {
        private CBOMenuNode _node;
        private IEventList<ICallback> _callbacks;
        private bool _enableDropDown;
        private IApplicationRepository _applicationRepo;
        private ILifeRepository _lifeRepo;
        private IReasonRepository _reasonRepo;
        private IStageDefinitionRepository _stageDefinitionRepo;
        private ILookupRepository _lookupRepo;
        private string _callbackReasonDescription;
        private IReadOnlyEventList<IReasonDefinition> _reasonDefinitions;
        private int _applicationKey;

        /// <summary>
        /// 
        /// </summary>
        public string CallbackReasonDescription
        {
            get { return _callbackReasonDescription; }
            set { _callbackReasonDescription = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ILifeRepository LifeRepo
        {
            get { return _lifeRepo; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ILookupRepository LookupRepo
        {
            get { return _lookupRepo; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CallBack(SAHL.Web.Views.Life.Interfaces.ICallBack view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            // Get the CBO Node    
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            _applicationKey = (int)_node.GenericKey;

            _applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            _lifeRepo = RepositoryFactory.GetRepository<ILifeRepository>();
            _stageDefinitionRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
            _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            _reasonRepo = RepositoryFactory.GetRepository<IReasonRepository>();

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;


            _view.OnCancelButtonClicked += new EventHandler(OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += new EventHandler(OnSubmitButtonClicked);

            // get Callbacks
            _callbacks = _applicationRepo.GetCallBacksByApplicationKey(_applicationKey, false);

            // get Reason definitions
            if (!string.IsNullOrEmpty(_callbackReasonDescription))
            {
                _enableDropDown = false;
                _reasonDefinitions = _reasonRepo.GetReasonDefinitionsByReasonDescription(ReasonTypes.LifeCallback, _callbackReasonDescription);
            }
            else
            {
                _enableDropDown = true;
                _reasonDefinitions = _reasonRepo.GetReasonDefinitionsByReasonType(ReasonTypes.LifeCallback);
            }

            // add reason definitions to dictionary to use in binding
            IDictionary<int, string> reasons = new Dictionary<int, string>();
            foreach (IReasonDefinition def in _reasonDefinitions)
            {
                reasons.Add(def.Key, def.ReasonDescription.Description);             
            }

            // bind the callback grid
            _view.BindCallBackGrid(_callbacks);

            // bind the callback reasons dropdown
            _view.BindCallBackReasons(reasons, _enableDropDown);
       }

        protected void CreateCallBack()
        {
            // validate selection
            if (_view.ReasonDefinitionKey == -1)
                _view.Messages.Add(new Error("A callback reason must be selected before submitting.", "A callback reason must be selected before submitting."));
            if (_view.CallBackDate.HasValue == false)
                _view.Messages.Add(new Error("The callback date must be entered.", "The callback date must be entered."));
            if (_view.CallBackDate.Value < DateTime.Now)
                _view.Messages.Add(new Error("The date and time cannot be in the past", "The date and time cannot be in the past"));

            if (_view.IsValid)
            {
                TransactionScope txn = new TransactionScope();

                try
                {
                    ISecurityRepository secRepo = RepositoryFactory.GetRepository<ISecurityRepository>();
                    IADUser adUser = secRepo.GetADUserByPrincipal(_view.CurrentPrincipal);

                    IReasonDefinition reasonDefinition = _reasonRepo.GetReasonDefinitionByKey(_view.ReasonDefinitionKey);

                    // write the stage transition  record
                    IStageTransition stageTransition = _stageDefinitionRepo.SaveStageTransition(_applicationKey
                        , Convert.ToInt32(SAHL.Common.Globals.StageDefinitionGroups.LifeOrigination)
                        , SAHL.Common.Constants.StageDefinitionConstants.CallBackSet
                        , reasonDefinition.ReasonDescription.Description + (!String.IsNullOrEmpty(_view.CallBackComment) ? " (" + _view.CallBackComment.Trim() + ")" : "")
                        , adUser);

                    // Save the Reason
                    IReason reason = _reasonRepo.CreateEmptyReason();
                    reason.GenericKey = _applicationKey;
                    reason.Comment = _view.CallBackComment;
                    reason.ReasonDefinition = reasonDefinition;
                    reason.StageTransition = stageTransition;
                    _reasonRepo.SaveReason(reason);

                    // Save the Callback
                    string adUserName = _view.CurrentPrincipal.Identity.Name;
                    SAHL.Common.BusinessModel.Interfaces.ICallback callback = _lifeRepo.CreateEmptyCallback();
                    callback.GenericKey = _applicationKey;
                    callback.GenericKeyType = _lookupRepo.GenericKeyType.ObjectDictionary[Convert.ToString((int)SAHL.Common.Globals.GenericKeyTypes.Offer)];
                    callback.CallbackDate = _view.CallBackDate.Value;
                    callback.CallbackUser = adUserName;
                    callback.EntryDate = DateTime.Now;
                    callback.EntryUser = adUserName;
                    callback.Reason = reason;
                    _lifeRepo.SaveCallback(callback);
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
        protected virtual void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            CreateCallBack();

            if (_view.IsValid)
            {
                TransactionScope txn = new TransactionScope();

                try
                {
                    // complete the x2 activity 
                    X2Service svc = ServiceFactory.GetService<IX2Service>() as X2Service;
                    svc.CompleteActivity(_view.CurrentPrincipal, null, false);

                    // remove the node
                    CBOManager.RemoveCBOMenuNode(_view.CurrentPrincipal, _node.NodePath, CBONodeSetType.X2);

                    txn.VoteCommit();

                    _view.Navigator.Navigate("Submit");
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

        protected virtual void OnCancelButtonClicked(object sender, EventArgs e)
        {
            // cancel the x2 activity 
            X2Service svc = ServiceFactory.GetService<IX2Service>() as X2Service;
            svc.CancelActivity(_view.CurrentPrincipal);
            
            if (GlobalCacheData.ContainsKey(ViewConstants.NavigateTo))
            {
                string navigateTo = GlobalCacheData[ViewConstants.NavigateTo].ToString();
                GlobalCacheData.Remove(ViewConstants.NavigateTo);
                _view.Navigator.Navigate(navigateTo);
            }
            else
                _view.Navigator.Navigate(_node.URL);
        }
    }
}
