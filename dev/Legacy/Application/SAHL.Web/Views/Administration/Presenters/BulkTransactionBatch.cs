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
    /// Presenter for BulkTransactionBatch
    /// </summary>
    public class BulkTransactionBatch : BulkBatchTransactionBase
    {       
        private int batchStatusKey;

        private int batchNumberPrev;

        private int transactionTypeNumber = -1;

        const string BatchStatusSelectedItem = "BatchStatusSelectedItem";

        private int transactionTypeNumberPrev = -1;

        /// <summary>
        /// Constructor for BultTransactionBatch
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public BulkTransactionBatch(IBulkTransactionBatch view, SAHLCommonBaseController controller)
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

            _view.onNextButtonClicked += new EventHandler(_view_onNextButtonClicked);
            _view.onPrevButtonClicked+=new EventHandler(_view_onPrevButtonClicked);

            _view.BindBatchStatus(lookups.BulkBatchStatuses);

            if (!_view.IsPostBack)
                _view.BindBatchNumber(null);

            if (PrivateCacheData.ContainsKey(BatchStatusSelectedItem))
                bulkBatch = PrivateCacheData[BatchStatusSelectedItem] as IList<IBulkBatch>;

            if (bulkBatch != null)
                _view.BindBatchNumber(bulkBatch);

            _view.BindTransactionStatus(lookups.BatchTransactionStatuses);

            _view.SetControlsForDisplay();
            _view.SetButtonsNextAndPrevious = false;
           
            if (PrivateCacheData.ContainsKey("PageNumber"))
                 pageNumber = Convert.ToInt32(PrivateCacheData["PageNumber"]);
            else
                PrivateCacheData.Add("PageNumber", pageNumber);

            if (PrivateCacheData.ContainsKey("ChangeEventTransactionStatusFired") == false)
                PrivateCacheData.Add("ChangeEventTransactionStatusFired", null);

            if (PrivateCacheData.ContainsKey("ChangeEventBatchNumberFired") == false)
                PrivateCacheData.Add("ChangeEventBatchNumberFired", null);

            _view.OnBatchStatusSelectedIndexChange+=new KeyChangedEventHandler(_view_OnBatchStatusSelectedIndexChange);
            _view.OnBatchNumberSelectedIndexChange+=new KeyChangedEventHandler(_view_OnBatchNumberSelectedIndexChange);
            _view.OnTransactionStatusSelectedIndexChange += new KeyChangedEventHandler(_view_OnTransactionStatusSelectedIndexChange);

        }
        /// <summary>
        /// View PreRender event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;

            if (_view.IsPostBack && batchTransaction != null && batchTransaction.Count > 0)
            {
                if (batchNumber >= 0 && batchNumber != batchNumberPrev)
                    ResetPageNumbers();

                //if (transactionTypeNumber >= 0 && transactionTypeNumber != transactionTypeNumberPrev)
                if (transactionTypeNumber != transactionTypeNumberPrev)
                    ResetPageNumbers();

                FilterTransactionsPerPage();
                _view.BindBulkGridDisplay("Display", pageNumber, totalPages, numberofRecordsFound, batchTransaction);
            }          
        }

        /// <summary>
        /// Event fired when BatchStatus is changed on view.
        /// Using SelectedIndex-1 as due to addition of Default value - "Please Select" , all
        /// items in the drop down have moved one index down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnBatchStatusSelectedIndexChange(object sender, KeyChangedEventArgs e)
        {
            if (Convert.ToInt32(e.Key) >= 0)
                batchStatusKey = Convert.ToInt32(e.Key) - 1;
            else
                batchStatusKey = 0;

            if (batchStatusKey >= 0)
                bulkBatch = bulkBatchRepo.GetBulkBatchByBulkBatchTypeAndStatusKey(1, batchStatusKey);

            if (PrivateCacheData.ContainsKey(BatchStatusSelectedItem))
                PrivateCacheData[BatchStatusSelectedItem] = bulkBatch;
            else
            {
                PrivateCacheData.Remove(BatchStatusSelectedItem);
                PrivateCacheData.Add(BatchStatusSelectedItem, bulkBatch);
            }

            if (bulkBatch.Count > 0)             
                _view.BindBatchNumber(bulkBatch);
        }

        void _view_OnBatchNumberSelectedIndexChange(object sender, KeyChangedEventArgs e)
        {
            if (Convert.ToInt32(e.Key) >= 0)
                batchNumber = Convert.ToInt32(e.Key) -1;
            else
                batchNumber = 0;

            if (batchNumber >= 0)
            {
                batchTransactionDT = bulkBatchRepo.GetBatchTransactionByBulkBatchKey(bulkBatch[batchNumber].Key);
                numberofRecordsFound = batchTransactionDT.Rows.Count;
                batchTransaction = batchTransactionDT.DefaultView;

                if (PrivateCacheData["ChangeEventBatchNumberFired"] != null)
                {
                    batchNumberPrev = Convert.ToInt32(PrivateCacheData["ChangeEventBatchNumberFired"]);
                    PrivateCacheData.Remove("ChangeEventBatchNumberFired");
                    PrivateCacheData.Add("ChangeEventBatchNumberFired", batchNumber);
                }
                else
                {
                    PrivateCacheData.Remove("ChangeEventBatchNumberFired");
                    PrivateCacheData.Add("ChangeEventBatchNumberFired", batchNumber);
                }

                if (bulkBatch.Count > 0)
                {
                    bool found = false;
                    foreach (ITransactionType tt in lookups.TransactionTypes)
                    {
                        if (tt.Key == bulkBatch[batchNumber].BatchTransactions[0].TransactionTypeNumber)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found)
                    {
                        transactionTypeDescription = lookups.TransactionTypes.ObjectDictionary[bulkBatch[batchNumber].BatchTransactions[0].TransactionTypeNumber.ToString()];
                        subsidyProviderDescription = empRepo.GetSubsidyProviderByKey(bulkBatch[batchNumber].IdentifierReferenceKey);
                        _view.BindBatchFields(bulkBatch[batchNumber], subsidyProviderDescription.LegalEntity.DisplayName, transactionTypeDescription.Description);
                        // transactionTypeDescription = bulkBatchRepo.GetTransactionTypeByKey(bulkBatch[batchNumber].BatchTransactions[0].TransactionTypeNumber);
                    }
                }
            }
            else
                batchTransaction = null;
        }
        
        void _view_OnTransactionStatusSelectedIndexChange(object sender, KeyChangedEventArgs e)
        {
            if (Convert.ToInt32(e.Key) >= 0)
                transactionTypeNumber = Convert.ToInt32(e.Key)-1;
            else
                transactionTypeNumber = -1;

            if (bulkBatch != null && transactionTypeNumber >= 0 && batchNumber >= 0)
            {
                batchTransactionDT = bulkBatchRepo.GetBatchTransactionByBulkBatchKey(bulkBatch[batchNumber].Key);
                numberofRecordsFound = batchTransactionDT.Rows.Count;
                batchTransaction = batchTransactionDT.DefaultView;
                batchTransaction.RowFilter = "BatchTransactionStatusKey=" + transactionTypeNumber;
            }
            else
                if (bulkBatch != null && batchNumber >= 0)
                {
                    batchTransactionDT = bulkBatchRepo.GetBatchTransactionByBulkBatchKey(bulkBatch[batchNumber].Key);
                    numberofRecordsFound = batchTransactionDT.Rows.Count;
                    batchTransaction = batchTransactionDT.DefaultView;
                    batchTransaction.RowFilter = "";
                }
                else
                    batchTransaction = null;

            if (PrivateCacheData["ChangeEventTransactionStatusFired"] != null)
            {
                transactionTypeNumberPrev = Convert.ToInt32(PrivateCacheData["ChangeEventTransactionStatusFired"]);
                
                PrivateCacheData.Remove("ChangeEventTransactionStatusFired");
                PrivateCacheData.Add("ChangeEventTransactionStatusFired", transactionTypeNumber);       
            }
            else
            {
                PrivateCacheData.Remove("ChangeEventTransactionStatusFired");
                PrivateCacheData.Add("ChangeEventTransactionStatusFired", transactionTypeNumber);
            }

        }      
   }
}
