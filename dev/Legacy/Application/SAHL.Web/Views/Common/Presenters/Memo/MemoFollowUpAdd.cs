using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using SAHL.Common.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI.Events;
using Castle.ActiveRecord;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using System.Collections.Generic;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.Exceptions;

namespace SAHL.Web.Views.Common.Presenters.Memo
{
    /// <summary>
    /// 
    /// </summary>
    public class MemoFollowUpAdd : MemoBase
    {
        /// <summary>
        /// Constructor for FollowUp Memo Add
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public MemoFollowUpAdd(IMemo view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            {
            }
        }

        /// <summary>
        /// OnViewInitialised for FollowUp Memo Add
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            //_genericKeyType = (int)SAHL.Common.Globals.GenericKeyTypes.Offer;
            //_genericKey = 270488;

            _memo = _memoRepository.GetMemoByGenericKey(_genericKey, _genericKeyType, (int)GeneralStatuses.Active);
       
            _view.AddButtonClicked += new EventHandler(_view_AddButtonClicked);
            _view.CancelButtonClicked += new EventHandler(_view_CancelButtonClicked);
            _view.OnMemoStatusChanged += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnMemoStatusChanged);
        }

        /// <summary>
        /// PreRender event for OnViewInitialised
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            _view.ShowControlsDisplay = false;
            _view.ShowControlsUpdate = false;
            _view.ShowControlsAdd = true;
            _view.ShowButtons = true;
            _view.SetDefaultDateForAdd();
            _view.HideExpiryDateControlForFollowUp();
            _view.ShowHours();
            _view.SetLabelData("FollowUp", "Add");
            _view.BindMemoGrid(_memo,"FollowUp");

        }
        /// <summary>
        /// MemoStatus Changed event - repopulate Memo grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_OnMemoStatusChanged(object sender, KeyChangedEventArgs e)
        {
            if (Convert.ToInt32(e.Key) == 0)
                _memo = _memoRepository.GetMemoByGenericKey(_genericKey, _genericKeyType);
            else
                _memo = _memoRepository.GetMemoByGenericKey(_genericKey, _genericKeyType, Convert.ToInt32(e.Key));
          
        }

        void _view_CancelButtonClicked(object sender, EventArgs e)
        {
            base.X2Service.CancelActivity(_view.CurrentPrincipal);
            base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }

        void _view_AddButtonClicked(object sender, EventArgs e)
        {
            // Add exclusion set
            this.ExclusionSets.Add(RuleExclusionSets.MemoExpiryDateExclude);

            TransactionScope txn = new TransactionScope();
            SAHL.Common.BusinessModel.Interfaces.IMemo memo = null;
            try
            {
                memo = _memoRepository.CreateMemo();

                _view.GetCapturedMemoForFollowupAdd(memo);
                memo.GenericKey = _genericKey;
                //memo.GenericKeyType = lookups.GenericKeyType.ObjectDictionary[((int)SAHL.Common.Globals.GenericKeyTypes.Offer).ToString()];
                memo.GenericKeyType = lookups.GenericKeyType.ObjectDictionary[_genericKeyType.ToString()];
                _memoRepository.SaveMemo(memo);
                this.ExclusionSets.Remove(RuleExclusionSets.MemoExpiryDateExclude);
                txn.VoteCommit();
            }
            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                {
                    base.X2Service.CancelActivity(_view.CurrentPrincipal);
                    throw;
                }
            }

            finally
            {
                txn.Dispose();
            }

            Dictionary<string, string> FieldInputs = new Dictionary<string, string>();
        
            FieldInputs.Add("GenericKey", memo.Key.ToString());

            if (_view.IsValid)
            {
                base.X2Service.CompleteActivity(_view.CurrentPrincipal, FieldInputs, false);
                base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
            }


        }


    }

}

