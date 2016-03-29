using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.CacheData;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Administration.Presenters
{
    public class AttorneyInvoice : AttorneyInvoiceView
    {
        public AttorneyInvoice(IAttorneyInvoice view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            if (_view.IsMenuPostBack && GlobalCacheData.ContainsKey(ViewConstants.SelectedAccountKey))
                GlobalCacheData.Remove(ViewConstants.SelectedAccountKey);

            if (GlobalCacheData.ContainsKey(ViewConstants.SelectedAccountKey))
                _view.AccountKey = Convert.ToInt32(GlobalCacheData[ViewConstants.SelectedAccountKey]);

            if (_view.AccountKey > 0)
                BindInvoiceGrid(_view.AccountKey);

            _view.OnAccountItemSelect += new EventHandler(_view_OnAccountItemSelect);
            _view.OnCancelClick += new EventHandler(_view_OnCancelClick);
            _view.OnAddClick += new EventHandler(_view_OnAddClick);
            _view.OnDeleteClick += new EventHandler(_view_OnDeleteClick);

            _view.BindAttorneys(RegRepo.GetLitigationAttorneys());
            _view.ReadOnly = false;
        }

        void _view_OnDeleteClick(object sender, EventArgs e)
        {
            TransactionScope txn = new TransactionScope();
            try
            {
                IAccountAttorneyInvoice accAttInv = AccRepo.GetAccountAttorneyInvoiceByKey(_view.SelectedAccountAttorneyInvoiceKey);

                AccRepo.DeleteAccountAttorneyInvoice(accAttInv);

                txn.VoteCommit();
            }
            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }

            txn.Dispose();

            //and show that the item has been removed
            BindInvoiceGrid(_view.AccountKey);
        }

        void _view_OnAddClick(object sender, EventArgs e)
        {
            IAccountAttorneyInvoice accAttInv = AccRepo.CreateEmptyAccountAttorneyInvoice();

            _view.PopulateDetail(accAttInv);

            if (accAttInv.AttorneyKey < 1)
            {
                string msg = "Please select an Attorney.";
                _view.Messages.Add(new Error(msg, msg));
            }

            if (String.IsNullOrEmpty(accAttInv.InvoiceNumber))
            {
                string msg = "Please enter an Invoice Number.";
                _view.Messages.Add(new Error(msg, msg));
            }

            if (accAttInv.InvoiceDate<=DateTime.MinValue)
            {
                string msg = "Please enter an Invoice Date.";
                _view.Messages.Add(new Error(msg, msg));
            }

            if (!(accAttInv.Amount > 0))
            {
                string msg = "An amount, not zero is required.";
                _view.Messages.Add(new Error(msg, msg));
            }

            if (!_view.IsValid)
                return;


            TransactionScope txn = new TransactionScope();
            try
            {
                AccRepo.SaveAccountAttorneyInvoice(accAttInv);
                _view.ResetInputs();

                txn.VoteCommit();
            }
            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }

            txn.Dispose();

            //and show the item that has been added
            BindInvoiceGrid(_view.AccountKey);
        }

        void _view_OnCancelClick(object sender, EventArgs e)
        {
            if (GlobalCacheData.ContainsKey(ViewConstants.SelectedAccountKey))
                GlobalCacheData.Remove(ViewConstants.SelectedAccountKey);

            _view.Navigator.Navigate("AttorneyInvoice");
        }

        protected void _view_OnAccountItemSelect(object sender, EventArgs e)
        {
            if (GlobalCacheData.ContainsKey(ViewConstants.SelectedAccountKey))
                GlobalCacheData.Remove(ViewConstants.SelectedAccountKey);

            GlobalCacheData.Add(ViewConstants.SelectedAccountKey, _view.AccountKey, LifeTimes);

            BindInvoiceGrid(_view.AccountKey);
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
    }
}