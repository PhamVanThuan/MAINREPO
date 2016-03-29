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
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class LoanDetailUpdate : SAHLCommonBasePresenter<ILoanDetail>
    {
        private IReadOnlyEventList<IDetail> _lstDetail;
        private ILookupRepository _lookupRepository;

        private string _cachedDataDetailClassKey = "SelectedDetailClass";
        private string _cachedDataDetailTypeKey = "SelectedDetailType";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LoanDetailUpdate(ILoanDetail view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelClicked);
            _view.OnSubmitButtonClicked += new KeyChangedEventHandler(_view_OnSubmitClicked);
            _view.OnGridSelectedIndexChanged += new KeyChangedEventHandler(_view_OnGridIndexChanged);

            _view.UpdateMode = true;

            CBOMenuNode cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (cboNode != null)
            {
                SetupDisplay(Convert.ToInt32(cboNode.GenericKey));
            }
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

            int detailtypeKey = -1;
            if (PrivateCacheData.ContainsKey(_cachedDataDetailTypeKey) )
                 detailtypeKey = Convert.ToInt32(PrivateCacheData[_cachedDataDetailTypeKey]);

             if (detailtypeKey == (int)SAHL.Common.Globals.DetailTypes.UnderCancellation) 
                _view.CancellationTypeEnabled = true;
            else
                _view.CancellationTypeEnabled = false;

            if (detailtypeKey != -1)
            {
                IDetailType detailRec = _lookupRepository.DetailTypes.ObjectDictionary[detailtypeKey.ToString()];
                if (detailRec.AllowUpdate == true)
                    _view.SubmitButtonEnabled = true;
                else
                    _view.SubmitButtonEnabled = false;
            }
        }

        #region Private Methods

        private void BindDetailTypeByClass(int detailClassKey)
        {
            List<IDetailType> detailTypeList = new List<IDetailType>();
            for (int detailTypeIndex = 0; detailTypeIndex < _lookupRepository.DetailTypes.Count; detailTypeIndex++)
            {
                if (_lookupRepository.DetailTypes[detailTypeIndex].DetailClass.Key == detailClassKey &&
                    _lookupRepository.DetailTypes[detailTypeIndex].GeneralStatus.Key == Convert.ToInt32(GeneralStatuses.Active) &&
                    _lookupRepository.DetailTypes[detailTypeIndex].AllowScreen == true)
                {
                    detailTypeList.Add(_lookupRepository.DetailTypes[detailTypeIndex]);
                }
            }

            _view.BindDetailTypeDropDown(detailTypeList);
        }

        private void SetupDisplay(int accountKey)
        {
            IAccountRepository _accRepository;
            _accRepository = RepositoryFactory.GetRepository<IAccountRepository>();
            _lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();

            _lstDetail = _accRepository.GetDetailByAccountKey(accountKey);

            _view.ShowButtons = true;
            _view.SubmitButtonText = "Update";
            _view.DetailGridPostBackType = GridPostBackType.SingleClick;

            _view.BindDetailGrid(_lstDetail);
            _view.BindDetailClassDropDown(_lookupRepository.DetailClasses);
            _view.BindDetailCancellationTypeDropDown(_lookupRepository.CancellationTypes);

            if (_lstDetail != null && _lstDetail.Count > 0 && !_view.IsPostBack)
            {
                BindDetailTypeByClass(_lstDetail[0].DetailType.DetailClass.Key);
                if (_lstDetail[0].DetailType.AllowUpdate == true && _lstDetail[0].DetailType.AllowScreen == true)
                {
                    _view.ShowLabels = false;
                    _view.SubmitButtonEnabled = true;
                    _view.BindData(_lstDetail[0]);

                    PrivateCacheData.Add(_cachedDataDetailClassKey, _lstDetail[0].DetailType.DetailClass.Key);
                    PrivateCacheData.Add(_cachedDataDetailTypeKey, _lstDetail[0].DetailType.Key);
                }
                else
                {
                    _view.ShowLabels = true;
                    _view.SubmitButtonEnabled = false;
                    _view.BindData(_lstDetail[0]);
                }
            }
            else
            {
                if (_view.UpdatedDetailClass != -1)
                {
                    BindDetailTypeByClass(_view.UpdatedDetailClass);
                    if (PrivateCacheData.ContainsKey(_cachedDataDetailClassKey) &&
                        Convert.ToInt32(PrivateCacheData[_cachedDataDetailClassKey]) != _view.UpdatedDetailClass)
                    {
                        PrivateCacheData[_cachedDataDetailTypeKey] = -1;
                    }
                    else
                    {
                        if (_view.UpdatedDetailType != -1 &&
                            PrivateCacheData.ContainsKey(_cachedDataDetailTypeKey) &&
                            Convert.ToInt32(PrivateCacheData[_cachedDataDetailTypeKey]) != _view.UpdatedDetailType)
                        {
                            PrivateCacheData[_cachedDataDetailTypeKey] = _view.UpdatedDetailType;
                        }
                    }
                    PrivateCacheData[_cachedDataDetailClassKey] = _view.UpdatedDetailClass;
                }
            }
        }

        #endregion


        #region Events Handlers

        void _view_OnGridIndexChanged(object sender, KeyChangedEventArgs e)
        {
            if (e != null)
            {
                if (_lstDetail != null && _lstDetail.Count > Convert.ToInt32(e.Key))
                {
                    int selectedIndex = Convert.ToInt32(e.Key);
                    BindDetailTypeByClass(_lstDetail[selectedIndex].DetailType.DetailClass.Key);

                    if (_lstDetail[selectedIndex].DetailType.AllowUpdate == true && _lstDetail[selectedIndex].DetailType.AllowScreen == true)
                    {
                        _view.ShowLabels = false;
                        _view.SubmitButtonEnabled = true;
                        _view.BindData(_lstDetail[selectedIndex]);

                        PrivateCacheData.Add(_cachedDataDetailClassKey, _lstDetail[selectedIndex].DetailType.DetailClass.Key);
                        PrivateCacheData.Add(_cachedDataDetailTypeKey, _lstDetail[selectedIndex].DetailType.Key);

      
                    }
                    else
                    {
                        _view.ShowLabels = true;
                        _view.SubmitButtonEnabled = false;
                        _view.BindData(_lstDetail[selectedIndex]);
                    }
                }
            }
        }

       
        void _view_OnCancelClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("LoanDetailDisplay");
        }

        void _view_OnSubmitClicked(object sender, KeyChangedEventArgs e)
        {
            if (e != null )
            {
                if (_view.UpdatedDetailAmount > 999999999.99)
                    _view.Messages.Add(new Error("Amount cannot be greater than R999,999,999.99", "Amount cannot be greater than R999,999,999.99"));

                if (_view.UpdatedDetailType == (int)SAHL.Common.Globals.DetailTypes.UnderCancellation && _view.UpdatedCancellationType == -1)
                    _view.Messages.Add(new Error("Cancellation Type must be selected.", "Cancellation Type must be selected."));

                if (_view.Messages.ErrorMessages.Count == 0)
                {
                    int selectGridIndex = Convert.ToInt32(e.Key);
                    IDetail updateRec = _lstDetail[selectGridIndex];
                    if (_view.UpdatedDetailDate.HasValue)
                        updateRec.DetailDate = _view.UpdatedDetailDate.Value ;
                    updateRec.Description = _view.UpdatedDetailDescription;
                    updateRec.Amount = _view.UpdatedDetailAmount;
                    if (_view.UpdatedDetailType != -1)
                        updateRec.DetailType = _lookupRepository.DetailTypes.ObjectDictionary[_view.UpdatedDetailType.ToString()];
                    if (_view.UpdatedCancellationType != -1 && updateRec.DetailType.Key == (int)SAHL.Common.Globals.DetailTypes.UnderCancellation)
                        updateRec.LinkID = _view.UpdatedCancellationType;

                    IAccountRepository _accRepository;
                    _accRepository = RepositoryFactory.GetRepository<IAccountRepository>();
                    
					IRuleService svc = ServiceFactory.GetService<IRuleService>();

					if (updateRec.DetailType.Key == (int)SAHL.Common.Globals.DetailTypes.LoanClosed)
					{
						svc.ExecuteRule(_view.Messages, "DetailCannotBeClosedWithCurrentBalanceNotZero", updateRec);
					}

					if (_view.IsValid)
					{
						TransactionScope txn = new TransactionScope();

						try
						{
							_accRepository.SaveDetail(updateRec);
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

					if (_view.Messages.Count == 0)
						_view.Navigator.Navigate("LoanDetailDisplay");
				}
            }
        }

        #endregion


    }
}
