using System;
using System.Data;
using SAHL.Common.Web.UI;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Web.Views.Administration.Interfaces;
using System.Collections.Generic;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
namespace SAHL.Web.Views.Administration.Presenters
{
    /// <summary>
    /// Base presenter for all BullkBatchTransaction presenters
    /// </summary>
    public class BulkBatchTransactionBase : SAHLCommonBasePresenter<IBulkTransactionBatch>
    {
        /// <summary>
        /// Look ups Repository
        /// </summary>
        protected ILookupRepository lookups;
        /// <summary>
        /// List of BulkBatch trans to bind to drop down
        /// </summary>
        protected IList<IBulkBatch> bulkBatch;
        /// <summary>
        /// BulkBatch Repository 
        /// </summary>
        protected IBulkBatchRepository bulkBatchRepo;
        /// <summary>
        /// DataView of BatchTransaction which is the default view of the batchTransaction Data Table
        /// </summary>
        protected DataView batchTransaction;
        /// <summary>
        /// BatchTransaction DataTable - contains records that get binded to Batch Transaction Grid
        /// </summary>
        protected DataTable batchTransactionDT = new DataTable();
        /// <summary>
        /// Contains Batch Number selected
        /// </summary>
        protected int batchNumber;
        /// <summary>
        /// Number of records found in Datable - BatchTransactionDT
        /// </summary>
        protected int numberofRecordsFound;
        
        protected IEmploymentRepository empRepo;

        protected ITransactionType transactionTypeDescription;
        /// <summary>
        /// Used to get the Account by Key - Used by Add and Save presenters
        /// </summary>
        protected IAccountRepository accRepo;
        /// <summary>
        /// Used to get the Legal Entity By Key - used by Add and Save presenters
        /// </summary>
        protected ILegalEntityRepository legalentityRepo;
        /// <summary>
        /// Used by BatchTransaction Data Cache
        /// </summary>
        protected const string BatchTransactionData = "BatchTransactionData";
        /// <summary>
        /// Used by BatchTransaction Data Cache
        /// </summary>
        protected const string BatchTransactionDataTable = "BatchTransactionDataTable";
        /// <summary>
        /// Saved Record Indicator - cache
        /// </summary>
        protected const string SavedRecord = "SaveRecordIndicator";

        protected SAHL.Common.BusinessModel.Interfaces.ISubsidyProvider subsidyProviderDescription;
        /// <summary>
        /// MortgageLoan Repository
        /// </summary>
        protected IMortgageLoanRepository mlRepo ;
        /// <summary>
        /// Total Pages calculated
        /// </summary>
        protected int totalPages;
        /// <summary>
        /// Page Size
        /// </summary>
        protected int pageSize = 20;
        /// <summary>
        /// Default Page Number
        /// </summary>
        protected int pageNumber = 1;
        /// <summary>
        /// Used in Private Cache - stores value of Selected Index on Grid
        /// </summary>
        protected const string selectedIndexOnGrid = "SelectedIndexOnGrid";

         /// <summary>
        /// Constructor for BultTransactionBatch
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public BulkBatchTransactionBase(IBulkTransactionBatch view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            empRepo = RepositoryFactory.GetRepository<IEmploymentRepository>();

            lookups = RepositoryFactory.GetRepository<ILookupRepository>();
        
            bulkBatchRepo = RepositoryFactory.GetRepository<IBulkBatchRepository>();
        }

        protected void FilterTransactionsPerPage()
        {
            if (PrivateCacheData.ContainsKey("PageNumber") && PrivateCacheData["PageNumber"] != null)
                pageNumber = Convert.ToInt32(PrivateCacheData["PageNumber"]);

            int startIndex = (pageNumber * pageSize) - (pageSize - 1);
            int endIndex = pageNumber * pageSize;
            if (endIndex > numberofRecordsFound)
                endIndex = numberofRecordsFound;
            if (batchTransaction.Count < endIndex)
            {
                endIndex = batchTransaction.Count;
            }
            System.Text.StringBuilder keys = new System.Text.StringBuilder();
            for (int iC = startIndex - 1; iC < endIndex; iC++)
            {
                keys.Append(batchTransaction[iC]["BatchTransactionKey"] + ",");
            }

            if (keys.Length == 0)
                batchTransaction.RowFilter = "BatchTransactionKey = -1";
            else
                batchTransaction.RowFilter = "BatchTransactionKey in (" + keys.ToString(0, keys.Length - 1) + ")";
            
            int RecalcTotalPages = 1;
            if (batchTransaction.Count > 0)
            {
                double calcval = 0;

                if (endIndex < 20)
                {
                    calcval = endIndex / pageSize;
                }
                else
                {
                    calcval = numberofRecordsFound / pageSize;
                }
                //}

                RecalcTotalPages = (int)Math.Round(calcval + 0.5, MidpointRounding.AwayFromZero);
            }
            totalPages = RecalcTotalPages;
        }

        /// <summary>
        /// This method is a bit of a hack - all it does is change the names of some of the mandatory 
        /// domain messages so they are a little mroe user friendly.
        /// </summary>
        protected void FixDomainMessages()
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);

            foreach (IDomainMessage msg in spc.DomainMessages)
            {
                if (msg.Message.ToLower().IndexOf("identifier reference key") > -1)
                {
                    spc.DomainMessages.Remove(msg);
                    spc.DomainMessages.Add(new Error("Subsidy Provider is a mandatory field", ""));
                    break;
                }
            }
        }           
        protected void _view_onNextButtonClicked(object sender, EventArgs e)
        {
            pageNumber++;

            if (PrivateCacheData.ContainsKey("PageNumber") && PrivateCacheData["PageNumber"] != null)
                PrivateCacheData["PageNumber"] = pageNumber;

            if (PrivateCacheData.ContainsKey(selectedIndexOnGrid) && PrivateCacheData[selectedIndexOnGrid] != null)
                PrivateCacheData[selectedIndexOnGrid] = 0; // reset selected index on page change
        }

        protected void _view_onPrevButtonClicked(object sender, EventArgs e)
        {
            pageNumber--;

            if (PrivateCacheData.ContainsKey("PageNumber") && PrivateCacheData["PageNumber"] != null)
                PrivateCacheData["PageNumber"] = pageNumber;

            if (PrivateCacheData.ContainsKey(selectedIndexOnGrid) && PrivateCacheData[selectedIndexOnGrid] != null)
                PrivateCacheData[selectedIndexOnGrid] = 0; // reset selected index on page change
        }

        protected void ResetPageNumbers()
        {
            pageNumber = 1;

            if (PrivateCacheData.ContainsKey("PageNumber") && PrivateCacheData["PageNumber"] != null)
                PrivateCacheData["PageNumber"] = pageNumber;

            if (PrivateCacheData.ContainsKey(selectedIndexOnGrid) && PrivateCacheData[selectedIndexOnGrid] != null)
                PrivateCacheData[selectedIndexOnGrid] = 0; // reset selected index on page change
        }
    }
}
