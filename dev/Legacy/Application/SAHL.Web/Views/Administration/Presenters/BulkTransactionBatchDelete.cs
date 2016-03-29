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
using Castle.ActiveRecord;


namespace SAHL.Web.Views.Administration.Presenters
{
    /// <summary>
    /// Presenter to Delete BulkBatch
    /// </summary>
    public class BulkTransactionBatchDelete : BulkBatchTransactionBase
    {
        private int batchNumberPrev;

        /// <summary>
        /// Constructor for BultTransactionBatch Delete
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public BulkTransactionBatchDelete(IBulkTransactionBatch view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
           
        }
        /// <summary>
        /// OnView Initialised event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!View.ShouldRunPage) return;

            _view.SetControlsForDelete();
            _view.SetButtonsNextAndPrevious = false;

            bulkBatch = bulkBatchRepo.GetBulkBatchByBulkBatchTypeAndStatusKey(1, 0);
            _view.BindBatchNumber(bulkBatch);
            GetPagesNumberFromCache();

            if (PrivateCacheData.ContainsKey("ChangeEventOnBatchNumberFired") == false)
                PrivateCacheData.Add("ChangeEventOnBatchNumberFired", null);

            _view.OnBatchNumberSelectedIndexChange+=new KeyChangedEventHandler(_view_OnBatchNumberSelectedIndexChange);
            _view.onNextButtonClicked += new EventHandler(_view_onNextButtonClicked);
            _view.onPrevButtonClicked += new EventHandler(_view_onPrevButtonClicked);
            _view.onDeleteButtonClicked += new KeyChangedEventHandler(_view_onDeleteButtonClicked);
            _view.OnCancelButtonClicked+=new EventHandler(_view_OnCancelButtonClicked);

        }

        private void GetPagesNumberFromCache()
        {

            if (PrivateCacheData.ContainsKey("PageNumber") && PrivateCacheData["PageNumber"] != null)
                pageNumber = Convert.ToInt32(PrivateCacheData["PageNumber"]);
            else
                PrivateCacheData.Add("PageNumber", pageNumber);
        }
        /// <summary>
        /// PreRender Event : Reset Page Numbers if values on drop Down Changes
        /// And Rebind Grid with new Dataset
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {

            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            if (batchNumber >= 0 && batchNumber != batchNumberPrev)
                ResetPageNumbers();

            if (batchTransaction != null && batchTransaction.Count > 0)
            {
                FilterTransactionsPerPage();
                _view.BindBulkGridDisplay("Display", pageNumber, totalPages, numberofRecordsFound, batchTransaction);
            }

        }
        /// <summary>
        /// Change event of Batch Number
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_OnBatchNumberSelectedIndexChange(object sender, KeyChangedEventArgs e)
        {
            if (Convert.ToInt32(e.Key) >= 0)
                batchNumber = Convert.ToInt32(e.Key) - 1;
            else
                batchNumber = 0;

            if (batchNumber >= 0)
            {
                IBulkBatch bb = bulkBatch[batchNumber];

                batchTransactionDT = bulkBatchRepo.GetBatchTransactionByBulkBatchKey(bb.Key);
                numberofRecordsFound = batchTransactionDT.Rows.Count;
                batchTransaction = batchTransactionDT.DefaultView;

                string displayName = String.Empty;
                if (bb.IdentifierReferenceKey > 0)
                {
                    subsidyProviderDescription = empRepo.GetSubsidyProviderByKey(bb.IdentifierReferenceKey);
                    displayName = subsidyProviderDescription.LegalEntity.DisplayName;
                }


                string loanDescription = String.Empty;
                if (bb.BatchTransactions.Count > 0 && bb.BatchTransactions[0].TransactionTypeNumber > 0)
                {
                    transactionTypeDescription = lookups.TransactionTypes.ObjectDictionary[bb.BatchTransactions[0].TransactionTypeNumber.ToString()];
                    loanDescription = transactionTypeDescription.Description;
                }

                _view.BindBatchFields(bb, displayName, loanDescription);

                if (PrivateCacheData["ChangeEventOnBatchNumberFired"] != null)
                {
                    batchNumberPrev = Convert.ToInt32(PrivateCacheData["ChangeEventOnBatchNumberFired"]);
                    PrivateCacheData["ChangeEventOnBatchNumberFired"] = batchNumber;
                }
                else
                    PrivateCacheData.Add("ChangeEventOnBatchNumberFired", batchNumber);
            }
            else
                    batchTransaction = null;
        }

        void _view_onDeleteButtonClicked(object sender, KeyChangedEventArgs e)
        {
           // int batchBulkKeyToDelete;

           if (Convert.ToInt32(e.Key) >= 0)
           {
              batchNumber = Convert.ToInt32(e.Key) - 1;
              if (batchNumber >= 0)
              {
                  TransactionScope txn = new TransactionScope();
                  try
                  {
                      bulkBatchRepo.DeleteBulkBatch(bulkBatch[batchNumber]);
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
                  if (_view.IsValid)
                    Navigator.Navigate("BulkTransactionBatch");

              } 
           }
        }

        void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("BulkTransactionBatch");
        }
    

    }
}
