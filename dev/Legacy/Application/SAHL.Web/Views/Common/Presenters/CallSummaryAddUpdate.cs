using System;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Service.Interfaces;
using SAHL.X2.Framework.Interfaces;
using Castle.ActiveRecord;

using SAHL.Common.CacheData;
using SAHL.Common.Exceptions;
using SAHL.Common.DomainMessages;
using SAHL.Common;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class CallSummaryAddUpdate : CallSummary
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
		public CallSummaryAddUpdate(ICallSummary view, SAHLCommonBaseController controller)
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

            lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            IHelpDeskRepository hdRepo = RepositoryFactory.GetRepository<IHelpDeskRepository>();

            _view.OnAddUpdateClicked += new EventHandler(_view_OnAddUpdateClicked);
            _view.OnSubmitClicked += new EventHandler(_view_OnSubmitClicked);
            _view.OnCancelClicked += new EventHandler(_view_OnCancelClicked);
            _view.OnGridSelectedIndexChanged += new KeyChangedEventHandler(_view_OnGridIndexChanged);

            InstanceNode _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            _instanceID = _node.InstanceID;

			if (_node.GenericKeyTypeKey == (int)GenericKeyTypes.LegalEntity)
            {
                _legalEntityKey = _node.GenericKey;
                _view.SetLegalEntityKey = _legalEntityKey;
            }

            if (_view.IsPostBack == false)
            {
                PrivateCacheData[_cachedDataScreenModeAddingKey] = true;
                _view.SetupControlsForAddOrUpdate(true, false);
                _view.CallSummaryGridSelectFirstRow = false;
                _view.SetDefaultDates();
            }
            else
            {
                if (!PrivateCacheData.ContainsKey(_cachedDataScreenModeAddingKey) ||
                        Convert.ToBoolean(PrivateCacheData[_cachedDataScreenModeAddingKey]) == true)
                {
                    PrivateCacheData[_cachedDataScreenModeAddingKey] = true;
                    _view.SetupControlsForAddOrUpdate(true, false);
                }
                if (_view.AddOrUpdateButtonClicked)
                {
                    _gridSelectedIndex = _view.CallSummaryGridSelectedIndex;
                    _view.CallSummaryGridSelectFirstRow = false;
                }
            }

			if (_view.SelectedMemoType == ((int)GenericKeyTypes.LegalEntity).ToString() || string.IsNullOrEmpty(_view.SelectedMemoType))
				_view.AccountDropDownVisible = false;
			else
				_view.AccountDropDownVisible = true;

            _helpDeskQueryList = hdRepo.GetHelpDeskQueryByInstanceID(_instanceID);
            if (_helpDeskQueryList != null)
            {
                _view.BindGrid(_helpDeskQueryList);

                if (_helpDeskQueryList.Count > 0)
                {
                    _view.SubmitButtonEnabled = true;
                    _view.AddOrUpdateButtonEnabled = false;
                }
                else
                {
                    _view.SubmitButtonEnabled = false;
                    _view.AddOrUpdateButtonEnabled = true;
                }
            }
            else
            {
                _view.SubmitButtonEnabled = false;
                _view.AddOrUpdateButtonEnabled = true;
            }

			BindCategoryDropDown(0);
            BindStatusDropDown();
            BindAccountDropDown(_legalEntityKey);
            BindQueryTypeDropDown();
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

            if (_view.Messages.Count > 0)
            {
                if (_helpDeskQueryList != null)
                {
                    _view.BindGrid(_helpDeskQueryList);

                    if (_helpDeskQueryList.Count > 0)
                    {
                        _view.SubmitButtonEnabled = true;
                        _view.AddOrUpdateButtonEnabled = false;
                    }
                    else
                    {
                        _view.SubmitButtonEnabled = false;
                        _view.AddOrUpdateButtonEnabled = true;
                    }
                }
                else
                {
                    _view.SubmitButtonEnabled = false;
                    _view.AddOrUpdateButtonEnabled = true;
                }

                if (PrivateCacheData.ContainsKey("HelpDeskQueryListSelIdx"))
                {
                    _update = true;
                    PrivateCacheData[_cachedDataScreenModeAddingKey] = false;
                    _view.SubmitButtonEnabled = false;
                    _view.AddOrUpdateButtonEnabled = true;
                    _view.SetupControlsForAddOrUpdate(false, false);
                }
                else
                    _view.SetupControlsForAddOrUpdate(true, false);

                if (_view.SelectedMemoType == ((int)GenericKeyTypes.LegalEntity).ToString() || string.IsNullOrEmpty(_view.SelectedMemoType))
                    _view.AccountDropDownVisible = false;
                else
                    _view.AccountDropDownVisible = true;
            }

			if (PrivateCacheData.ContainsKey("HelpDeskQueryListSelIdx"))
			{
				_update = true;
				BindCategoryDropDown(_helpDeskQueryList[Convert.ToInt32(PrivateCacheData["HelpDeskQueryListSelIdx"])].HelpDeskCategory.Key);
				_view.BindUpdateControls(_helpDeskQueryList[Convert.ToInt32(PrivateCacheData["HelpDeskQueryListSelIdx"])]);
			}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		void _view_OnAddUpdateClicked(object sender, EventArgs e)
		{
			IHelpDeskRepository hdRepo = RepositoryFactory.GetRepository<IHelpDeskRepository>();
			IMemoRepository memoRepo = RepositoryFactory.GetRepository<IMemoRepository>();
			IHelpDeskQuery helpDeskQuery;
			int helpDeskQueryKey = 0;

			bool adding = false;
			if (!PrivateCacheData.ContainsKey(_cachedDataScreenModeAddingKey) ||
				Convert.ToBoolean(PrivateCacheData[_cachedDataScreenModeAddingKey]) == true)
			{
				//we are adding a record...create empty HelpDeskQuery
				helpDeskQuery = hdRepo.CreateEmptyHelpDeskQuery();
				adding = true;
			}
			else
			{
				_update = true;
                helpDeskQuery = _helpDeskQueryList[Convert.ToInt32(PrivateCacheData["HelpDeskQueryListSelIdx"])];
			}

            // Run validation
            if (!DoValidate(helpDeskQuery))
                return;

			_view.PopulateHeldeskRecord(helpDeskQuery, adding);

			TransactionScope txn = new TransactionScope();
			using (txn)
			{
				try
				{
					memoRepo.SaveMemo(helpDeskQuery.Memo);
					helpDeskQuery.ResolvedDate = DateTime.Now;
					helpDeskQueryKey = hdRepo.SaveHelpDeskQuery(helpDeskQuery);
					IDictionary<string, object> hdDict = new Dictionary<string, object>();
					hdDict.Add("HelpDeskQueryKey", helpDeskQueryKey);
					hdRepo.UpdateX2HelpDeskData(_instanceID, hdDict);
                    txn.VoteCommit();
                    this.PrivateCacheData.Remove("HelpDeskQueryListSelIdx");
                    _view.ResetControls();
				}
				catch (Exception)
				{
					txn.VoteRollBack();
                    if (_view.IsValid)
                        throw;
                }
			}

			if (helpDeskQueryKey != 0)
			{
				_view.SetupControlsForAddOrUpdate(true, false);
				_view.SubmitButtonEnabled = true;
				_view.ClearUpdateControls();
				_view.AccountDropDownVisible = false;
				_helpDeskQueryList = hdRepo.GetHelpDeskQueryByHelpDeskQueryKey(helpDeskQueryKey);

				if (_helpDeskQueryList != null)
				{
					_view.BindGrid(_helpDeskQueryList);

					if (_helpDeskQueryList.Count > 0)
					{
						_view.SubmitButtonEnabled = true;
						_view.AddOrUpdateButtonEnabled = false;
					}
					else
					{
						_view.SubmitButtonEnabled = false;
						_view.AddOrUpdateButtonEnabled = true;
					}
				}
				else
				{
					_view.SubmitButtonEnabled = false;
					_view.AddOrUpdateButtonEnabled = true;
				}
			}
			else
			{
				_view.SubmitButtonEnabled = false;
				_view.AddOrUpdateButtonEnabled = true;
			}
		}

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnSubmitClicked(object sender, EventArgs e)
        {
			IHelpDeskRepository hdRepo = RepositoryFactory.GetRepository<IHelpDeskRepository>();
			IMemoRepository memoRepo = RepositoryFactory.GetRepository<IMemoRepository>();
			int helpDeskQueryKey = 0;

			// wtf ~ There can only be 1 Re: The Highlander
			for (int i = 0; i < _helpDeskQueryList.Count; i++)
			{
                if (_helpDeskQueryList[i].Memo.GeneralStatus.Key == (int)GeneralStatuses.Inactive)
				{
					_helpDeskQueryList[i].ResolvedDate = DateTime.Now;
					TransactionScope txn = new TransactionScope();
					using (txn)
					{
						try
						{
							memoRepo.SaveMemo(_helpDeskQueryList[i].Memo);
							helpDeskQueryKey = hdRepo.SaveHelpDeskQuery(_helpDeskQueryList[i]);
							txn.VoteCommit();
						}
						catch (Exception)
						{
							txn.VoteRollBack();
                            if (_view.IsValid)
                                throw;
                        }
					}

					if (_view.Messages.ErrorMessages.Count == 0 && helpDeskQueryKey != 0)
					{
						IX2Service x2svc = ServiceFactory.GetService<IX2Service>();
						Dictionary<string, string> Inputs = new Dictionary<string, string>();
						Inputs.Add("LegalEntityKey", _legalEntityKey.ToString());
						Inputs.Add("HelpDeskQueryKey", helpDeskQueryKey.ToString());
						Inputs.Add("CurrentConsultant", _view.CurrentPrincipal.Identity.Name);
						IX2Info XI = x2svc.GetX2Info(_view.CurrentPrincipal);

						if (XI == null || String.IsNullOrEmpty(XI.SessionID))
							x2svc.LogIn(_view.CurrentPrincipal);

						X2ServiceResponse Rsp = null;
						x2svc.StartActivity(_view.CurrentPrincipal, _instanceID, SAHL.Common.Constants.WorkFlowActivityName.HelpDeskComplete, null, false);
						Rsp = x2svc.CompleteActivity(_view.CurrentPrincipal, Inputs, false);

						if (Rsp.IsError)
							x2svc.CancelActivity(_view.CurrentPrincipal);
						else
							x2svc.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
					}
				}
			}
			GlobalCacheData.Remove(ViewConstants.InstanceID);
			Navigator.Navigate("HelpDeskClientSearch");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnCancelClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("CallSummaryDisplay");
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        void _view_OnGridIndexChanged(object sender, KeyChangedEventArgs e)
        {
            if (e != null)
            {
                //Setup view for update 
                PrivateCacheData[_cachedDataScreenModeAddingKey] = false;
				_view.SubmitButtonEnabled = false;
				_view.AddOrUpdateButtonEnabled = true;
                _view.SetupControlsForAddOrUpdate(false, false);
				if (_helpDeskQueryList[Convert.ToInt32(e.Key)].Memo.GenericKeyType.Key == Convert.ToInt32(GenericKeyTypes.Account))
                    _view.AccountDropDownVisible = true;
                else
                    _view.AccountDropDownVisible = false;

                if (this.PrivateCacheData.ContainsKey("HelpDeskQueryListSelIdx"))
                    PrivateCacheData["HelpDeskQueryListSelIdx"] = e.Key;
                else
                    PrivateCacheData.Add("HelpDeskQueryListSelIdx", e.Key);

            }
        }
    }
}
