using System;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Factories;
using Castle.ActiveRecord;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Exceptions;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.CommonReason
{

    /// <summary>
    /// CommonReasonQAQuery Presenter
    /// </summary>
    public class CommonReasonQAQuery : CommonReasonBase
    {
        IReasonRepository reasonRepo;

        /// <summary>
        /// CommonReasonQAQuery Constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CommonReasonQAQuery(ICommonReason view, SAHLCommonBaseController controller)
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

                // get the instance node
                _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as InstanceNode;

                //set the generic key
                if (_node != null)
                    GenericKey = _node.GenericKey;

            reasonRepo = RepositoryFactory.GetRepository<IReasonRepository>();

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

            ILookupRepository lookups = RepositoryFactory.GetRepository<ILookupRepository>();
            IOrganisationStructureRepository OSR = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            IADUser adUser = OSR.GetAdUserForAdUserName(_view.CurrentPrincipal.Identity.Name);
           
           DeletePreviouslySavedReasons();

            if (_selectedReasons.Count > 0) // Only call Save if Reasons have been selected
            {
                SAHL.Common.BusinessModel.Interfaces.IMemo memo = null;
                IMemoRepository _memoRepository = RepositoryFactory.GetRepository<IMemoRepository>();

                for (int x = 0; x < _selectedReasons.Count; x++)
                {
                    //get an empty reason to populate
                    IReason res = reasonRepo.CreateEmptyReason();
                    res.Comment = _selectedReasons[x].Comment;
                    res.GenericKey = _node.GenericKey;
                    res.ReasonDefinition = reasonRepo.GetReasonDefinitionByKey(_selectedReasons[x].ReasonDefinitionKey);

                    // Create the Memo Object if Comment exists
                    if (!string.IsNullOrEmpty(_selectedReasons[x].Comment) && _selectedReasons[x].Comment.Length > 0)
                    {
                        memo = _memoRepository.CreateMemo();
                        if (_node != null)
                        {
                            memo.GenericKey = _node.GenericKey;
                            if (_node != null)
                                memo.GenericKeyType = lookups.GenericKeyType.ObjectDictionary[(_node.GenericKeyTypeKey).ToString()];
                            else
                                memo.GenericKeyType = lookups.GenericKeyType.ObjectDictionary[((int)GenericKeyTypes.Offer).ToString()];
                        }
                        memo.Description = _selectedReasons[x].Comment;
                        memo.GeneralStatus = lookups.GeneralStatuses[GeneralStatuses.Active];
                        memo.InsertedDate = DateTime.Now.Date;
                        memo.ADUser = adUser;
                    }
                    TransactionScope txn = new TransactionScope();
                    
                    try
                    {

                        if (!string.IsNullOrEmpty(_selectedReasons[x].Comment) && _selectedReasons[x].Comment.Length > 0)
                        {
                            ExclusionSets.Add(RuleExclusionSets.MemoMandatoryDateExclude); 
                            _memoRepository.SaveMemo(memo);
                            ExclusionSets.Remove(RuleExclusionSets.MemoMandatoryDateExclude); 
                        }
                        
                        reasonRepo.SaveReason(res);
                        base.InsertedReasonKeys.Add(res.Key);//this is required for the base presenter to link the stage transition to the reason
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
                {
                    TransactionScope txn = new TransactionScope();
                    try
                    {
                        CompleteActivityAndNavigate();
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

        private void DeletePreviouslySavedReasons()
        {
            string reasonTypeKey = _view.ViewAttributes["reasontypekey"];

            IReadOnlyEventList<IReason> reasons = reasonRepo.GetReasonByGenericKeyAndReasonTypeKey(_node.GenericKey, Convert.ToInt32(reasonTypeKey));

            if (reasons != null && reasons.Count > 0)
            {
                TransactionScope txn = new TransactionScope();

                try
                {
                    for (int i = 0; i < reasons.Count; i++)
                    {
                        reasonRepo.DeleteReason(reasons[i]);
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
            }
        }

        public override void CancelActivity()
        {
            X2Service.CancelActivity(_view.CurrentPrincipal);
            X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }

        public override void CompleteActivityAndNavigate()
        {
            X2ServiceResponse rsp = X2Service.CompleteActivity(_view.CurrentPrincipal, null, false);
            if (base.sdsdgKeys.Count > 0)
            {
                UpdateReasonsWithStageTransitionKey();
            }
            X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }
    }
}
