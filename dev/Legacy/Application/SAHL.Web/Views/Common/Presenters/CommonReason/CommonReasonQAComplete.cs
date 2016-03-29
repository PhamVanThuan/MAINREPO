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
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.BusinessModel;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Service;
using SAHL.Common.Exceptions;
using SAHL.Common.Globals;
using SAHL.Common.CacheData;

namespace SAHL.Web.Views.Common.Presenters.CommonReason
{

    /// <summary>
    /// CommonReasonQAComplete Presenter
    /// </summary>
    public class CommonReasonQAComplete : CommonReasonBase
    {
        private List<SelectedReason> _selectedReasons;

        /// <summary>
        /// Constructpor for CommonReasonQAComplete
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CommonReasonQAComplete(ICommonReason view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }
        /// <summary>
        /// OnViewInitialised event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.SetHiddenIndText = "1";
            _view.CancelButtonVisible = true;
        }

        /// <summary>
        /// Overrides the base OnSubmitButtonClicked event so that specific credit decline actions can be performed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void _view_OnSubmitButtonClicked(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            _selectedReasons = (List<SelectedReason>)e.Key;

            if (_selectedReasons.Count > 0) // Only call Save if Reasons have been selected
            {
                base._view_OnSubmitButtonClicked(sender, e);

                SAHL.Common.BusinessModel.Interfaces.IMemo memo;
                IMemoRepository _memoRepository = RepositoryFactory.GetRepository<IMemoRepository>();
                ILookupRepository lookups = RepositoryFactory.GetRepository<ILookupRepository>();
                IOrganisationStructureRepository OSR = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                IADUser adUser = OSR.GetAdUserForAdUserName(_view.CurrentPrincipal.Identity.Name);

                for (int x = 0; x < _selectedReasons.Count; x++)
                {
                    if (!string.IsNullOrEmpty(_selectedReasons[x].Comment) && _selectedReasons[x].Comment.Length > 0)
                    {
                        memo = _memoRepository.CreateMemo();
                        memo.GenericKey = base.GenericKey; // base.GenericKey;
                        if (_node != null)
                            memo.GenericKeyType = lookups.GenericKeyType.ObjectDictionary[((int)_node.GenericKeyTypeKey).ToString()];
                        else
                            memo.GenericKeyType = lookups.GenericKeyType.ObjectDictionary["2"];
                        memo.Description = _selectedReasons[x].Comment;
                        memo.GeneralStatus = lookups.GeneralStatuses[GeneralStatuses.Active];
                        memo.InsertedDate = DateTime.Now.Date;
                        memo.ADUser = adUser;


                        TransactionScope txn = new TransactionScope();

                        try
                        {
                            ExclusionSets.Add(RuleExclusionSets.MemoMandatoryDateExclude); 
                            _memoRepository.SaveMemo(memo);
                            ExclusionSets.Remove(RuleExclusionSets.MemoMandatoryDateExclude); 
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
                }

                TransactionScope tx = new TransactionScope();
                try
                {
                    CompleteActivityAndNavigate();
                }
                catch (Exception)
                {
                    tx.VoteRollBack();
                    if (_view.IsValid)
                        throw;
                }
                finally
                {
                    tx.Dispose();
                }
            }
            else
            {
                TransactionScope tx = new TransactionScope();
                try
                {
                    SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
                    this.X2Service.CompleteActivity(_view.CurrentPrincipal, null, spc.IgnoreWarnings);
                    base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
                
                }
                catch (Exception)
                {
                    tx.VoteRollBack();
                    if (_view.IsValid)
                        throw;
                }
                finally
                {
                    tx.Dispose();
                }
            }

        }

        public override void CancelActivity()
        {
            base.X2Service.CancelActivity(_view.CurrentPrincipal);
            base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }


        public override void CompleteActivityAndNavigate()
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            X2ServiceResponse rsp = base.X2Service.CompleteActivity(_view.CurrentPrincipal, null, spc.IgnoreWarnings);
            if (base.sdsdgKeys.Count > 0)
             {
                 UpdateReasonsWithStageTransitionKey();
             }
            base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }

    }
}
