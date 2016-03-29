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
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Web.Views.Administration.Interfaces;
using System.Collections.Generic;

namespace SAHL.Web.Views.Administration.Presenters
{
    /// <summary>
    /// Presenter BatchProgress
    /// </summary>
    public class BatchProgress : SAHLCommonBasePresenter<IBatchProgress>
    {
        private ILookupRepository lookups;

        private IBulkBatchRepository bulkBatchRepo;

        private IList<IBulkBatch> bulkTransactions;

        private IList<IBulkBatchLog> bulkBatchLog;

        private int batchGridIndexSelected;

        private int genericMessageTypeKey;


        /// <summary>
        /// Used by Test
        /// </summary>
        public IList<IBulkBatch> bulkBatch
        {
            set
            {
                bulkTransactions = value;
            }
        }

        /// <summary>
        /// Used by Test
        /// </summary>
        public IList<IBulkBatchLog> bulkBatchLogTrans
        {
            set
            {
                bulkBatchLog = value;
            }
        }
        /// <summary>
        /// Constructor for BatchProgress Presenter
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public BatchProgress(IBatchProgress view, SAHLCommonBaseController controller)
          : base(view, controller)
        {
        }
        /// <summary>
        /// OnViewInitialised Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            lookups = RepositoryFactory.GetRepository<ILookupRepository>();
            bulkBatchRepo = RepositoryFactory.GetRepository<IBulkBatchRepository>();

            _view.BindLookUps(lookups.BulkBatchTypes, lookups.MessageTypes);

            // check to see if a bulk batch type has been passed through via the global cache
            if (GlobalCacheData.ContainsKey(ViewConstants.BulkBatchTypeKey))
            {
                int bulkBatchTypeKey = Convert.ToInt32(GlobalCacheData[ViewConstants.BulkBatchTypeKey]);
                bulkTransactions = bulkBatchRepo.GetBulkBatchTransactionsByBulkBatchTypeKey(bulkBatchTypeKey);
                GlobalCacheData.Remove(ViewConstants.BulkBatchTypeKey);
                _view.BulkBatchTypeKey = bulkBatchTypeKey;

            }

            _view.OnBatchGridSelectedIndexChanged += new KeyChangedEventHandler(_view_OnBatchGridSelectedIndexChanged);
            _view.OnMessageTypeListSelectedIndexChange+=new KeyChangedEventHandler(_view_OnMessageTypeListSelectedIndexChange);
            _view.OnBatchTypeListSelectedIndexChange+=new KeyChangedEventHandler(_view_OnBatchTypeListSelectedIndexChange);
        }
        /// <summary>
        /// View PreRender Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;
            _view.BindBatchGrid(bulkTransactions);


            if (bulkTransactions !=null && bulkTransactions.Count > 0)
                _view.BindBatchGridFields(bulkTransactions[batchGridIndexSelected]);

            if (bulkTransactions != null && bulkTransactions.Count > 0 && genericMessageTypeKey > 0)
            {
               bulkBatchLog = bulkBatchRepo.GetBatchLogByBatchKeyMessageType(bulkTransactions[batchGridIndexSelected].Key, genericMessageTypeKey);
               if (bulkBatchLog != null && bulkBatchLog.Count > 0)
                _view.BindMessageGrid(bulkBatchLog);
            }
            else
                _view.BindMessageGrid(null);
        }
        void _view_OnBatchGridSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
          genericMessageTypeKey = _view.GetSelectedMessageType;
          batchGridIndexSelected = (Convert.ToInt32(e.Key));
        }

        void _view_OnMessageTypeListSelectedIndexChange(object sender, KeyChangedEventArgs e)
        {
            batchGridIndexSelected = _view.GetSelectedIndexOnGrid;

            if (Convert.ToInt32(e.Key) == 0)
                genericMessageTypeKey = 0;
            else
                genericMessageTypeKey = Convert.ToInt32(e.Key);
        }

        void _view_OnBatchTypeListSelectedIndexChange(object sender, KeyChangedEventArgs e)
        {
            if (Convert.ToInt32(e.Key) == 0)
                bulkTransactions = bulkBatchRepo.GetBulkBatchTransactionsByBulkBatchTypeKey(0);
            else
                bulkTransactions = bulkBatchRepo.GetBulkBatchTransactionsByBulkBatchTypeKey(Convert.ToInt32(e.Key));   

        }

    }
}
