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
using SAHL.Common.BusinessModel;
using SAHL.Common.Security;
using SAHL.Common.Globals;
using SAHL.Common;
using SAHL.Common.CacheData;
using SAHL.Common.DomainMessages;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Web.Views.Administration.Presenters
{
    /// <summary>
    /// BulkBatchAdd Presenter
    /// </summary>
    public class BulkTransactionBatchAdd : BulkBatchTransactionBase
    {
        IList<SAHL.Common.BusinessModel.Interfaces.ISubsidyProvider> subsidyProviderParent;
        const string SubsidyProviderParent = "ParentSubsidyProviders";
        const string SubsidyProviderKey = "SubsidyProviderKeySelected";
        const string transactionTypeKey = "transactionTypeKeySelected";
        const string BulkBatchKey = "BulkBatchKeyGenerated";
        IBulkBatch bulkBatchRec;
        IList<IBatchTransaction> batchTransactionRecs;
        int selectedBatchTransactionKey;
        IBatchTransaction batchTransactionRecord;
        

        private enum UpdateAction
        {
            Update = 1,
            Remove = 2
        }
       
        IReadOnlyEventList<ITransactionType> transactionTypes;
        SAHLPrincipal sahlCurrentPrincipal;
        int selectedInd;
        int susbidyProviderKeySelected;
        int transactionTypeSelected;
        int savedRecordInd;
        int susbidyProviderInd;
        int BulkBatchKeySaved;
        int subsidyTypeNumber;
        int selectedGridIndex;
        int susbidyProviderKeyPrev = -1;
        int subsidyProviderTypeKey;
        DateTime effectiveDate;
        IBulkBatch bulkBatchRecord;
        double amount;
        double amountAdjustment;

         /// <summary>
        /// Constructor for BulkTransactionBatch Add
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public BulkTransactionBatchAdd(IBulkTransactionBatch view, SAHLCommonBaseController controller)
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

            if (!View.ShouldRunPage) return;

            _view.SetControlsForAdd();
            _view.SetButtonsNextAndPrevious = false;
            _view.SetUpdateButtons = false;
            _view.SetPostBackType();

            _view.BindSubsidyTypes(lookups.SubsidyProviderTypes);

            sahlCurrentPrincipal = _view.CurrentPrincipal;
            transactionTypes = bulkBatchRepo.GetLoanTransactionTypesByKeys(sahlCurrentPrincipal, ((int)TransactionTypes.InstalmentPaymentSubsidy).ToString());
            _view.BindTransactionTypes(transactionTypes);

            if (PrivateCacheData.ContainsKey(SubsidyProviderParent) && PrivateCacheData[SubsidyProviderParent] != null)
                subsidyProviderParent = PrivateCacheData[SubsidyProviderParent] as IList<SAHL.Common.BusinessModel.Interfaces.ISubsidyProvider>;

            if (subsidyProviderParent != null)
                _view.BindParentSubsidyProvider(subsidyProviderParent);
            else
                _view.BindParentSubsidyProvider(null);

            GetPagesNumberFromCache();

            if (PrivateCacheData.ContainsKey("SubsidyProviderPrev") == false)
                PrivateCacheData.Add("SubsidyProviderPrev", susbidyProviderKeyPrev);

            _view.OnSubsidyTypeSelectedIndexChange+=new KeyChangedEventHandler(_view_OnSubsidyTypeSelectedIndexChange);
            _view.OnParentSubsidySelectedIndexChange+=new KeyChangedEventHandler(_view_OnParentSubsidySelectedIndexChange);
            _view.OnTransactionTypeSelectedIndexChange+=new KeyChangedEventHandler(_view_OnTransactionTypeSelectedIndexChange);
            _view.onNextButtonClicked+=new EventHandler(_view_onNextButtonClicked);
            _view.onPrevButtonClicked+=new EventHandler(_view_onPrevButtonClicked);
            _view.OnBulkGridSelectedIndexChange+=new KeyChangedEventHandler(_view_OnBulkGridSelectedIndexChange);
            _view.onFindButtonClicked+=new KeyChangedEventHandler(_view_onFindButtonClicked);
            _view.OnUpdateButtonClicked+=new EventHandler(_view_OnUpdateButtonClicked);
            _view.OnSaveButtonClicked+=new EventHandler(_view_OnSaveButtonClicked);
            _view.OnPostButtonClicked+=new EventHandler(_view_OnPostButtonClicked);
            _view.OnCancelButtonClicked+=new EventHandler(_view_OnCancelButtonClicked);
            _view.OnRemoveButtonClicked += new EventHandler(_view_OnRemoveButtonClicked);
            //_view.OnPostButtonClicked+=new EventHandler(_view_OnPostButtonClicked);
        }

        private void GetPagesNumberFromCache()
        {
            if (PrivateCacheData.ContainsKey("PageNumber"))
                pageNumber = Convert.ToInt32(PrivateCacheData["PageNumber"]);
            else
                PrivateCacheData.Add("PageNumber", pageNumber);
        }
        /// <summary>
        /// OnView PreRender Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            if (batchTransaction == null)
                GetDataSetsFromCache(); // if batchtransaction is null, check the cache

            if (batchTransaction != null && batchTransaction.Count > 0)
            {
                GetValuesFromPrivateCache();

                if (susbidyProviderKeySelected != susbidyProviderKeyPrev)
                    ResetPageNumbers();

                FilterDataTableTransactionsPerPage();
                _view.BindBulkGridAdd("Add", selectedGridIndex, pageNumber, totalPages, numberofRecordsFound, batchTransaction);

                _view.SetAddUpdateControls = true;
                _view.SetSelectedIndexOnGrid(selectedGridIndex);

                if (PrivateCacheData.ContainsKey(SavedRecord) && PrivateCacheData[SavedRecord] != null)
                    savedRecordInd = Convert.ToInt32(PrivateCacheData[SavedRecord]);

                if (savedRecordInd == 1)
                {
                    GetCachedData();
                    _view.DisableAllDropDownsForAdd = false;
                    _view.RestoreDropDownValues(susbidyProviderKeySelected, transactionTypeSelected, subsidyProviderTypeKey, effectiveDate);
                    _view.SetUpdateButtons = true;
                }
            }       
        }

        private void GetDataSetsFromCache()
        {
            if (PrivateCacheData.ContainsKey(BatchTransactionData) && PrivateCacheData[BatchTransactionData] != null)
                batchTransaction = PrivateCacheData[BatchTransactionData] as DataView;

            if (PrivateCacheData.ContainsKey(BatchTransactionDataTable) && PrivateCacheData[BatchTransactionDataTable] != null)
                batchTransactionDT = PrivateCacheData[BatchTransactionDataTable] as DataTable;
        }

        private void GetValuesFromPrivateCache()
        {
            if (PrivateCacheData.ContainsKey(selectedIndexOnGrid))
                selectedGridIndex = Convert.ToInt32(PrivateCacheData[selectedIndexOnGrid]);
            else
                selectedGridIndex = 0;

            if (PrivateCacheData.ContainsKey(SubsidyProviderKey))
                susbidyProviderKeySelected = Convert.ToInt32(PrivateCacheData[SubsidyProviderKey]);

            if (PrivateCacheData.ContainsKey("SubsidyProviderPrev"))
                susbidyProviderKeyPrev = Convert.ToInt32(PrivateCacheData["SubsidyProviderPrev"]);
        }

        void _view_OnSubsidyTypeSelectedIndexChange(object sender, KeyChangedEventArgs e)
        {
            if (Convert.ToInt32(e.Key) >= 0)
                subsidyTypeNumber = Convert.ToInt32(e.Key) - 1;
            else
                subsidyTypeNumber = 0;

            if (subsidyTypeNumber >= 0)
            {
                subsidyProviderParent = bulkBatchRepo.GetSubsidyProviderBySubsidyProviderTypeKey(lookups.SubsidyProviderTypes[subsidyTypeNumber].Key);

                if (PrivateCacheData.ContainsKey("SubsidyProviderTypeKey"))
                    PrivateCacheData["SubsidyProviderTypeKey"] = lookups.SubsidyProviderTypes[subsidyTypeNumber].Key;
                else
                    PrivateCacheData.Add("SubsidyProviderTypeKey", lookups.SubsidyProviderTypes[subsidyTypeNumber].Key);

                if (PrivateCacheData.ContainsKey(SubsidyProviderParent))
                    PrivateCacheData[SubsidyProviderParent] = subsidyProviderParent;
                else
                    PrivateCacheData.Add(SubsidyProviderParent, subsidyProviderParent);

                if (subsidyProviderParent != null && subsidyProviderParent.Count > 0)
                    _view.BindParentSubsidyProvider(subsidyProviderParent);
            }
            else
                   _view.BindParentSubsidyProvider(null);
        }

        void _view_OnParentSubsidySelectedIndexChange(object sender, KeyChangedEventArgs e)
        {
            if (PrivateCacheData.ContainsKey(SavedRecord) && PrivateCacheData[SavedRecord] != null)
                savedRecordInd = Convert.ToInt32(PrivateCacheData[SavedRecord]);
            else
                savedRecordInd = 0;

            if (Convert.ToInt32(e.Key) >= 0)
                susbidyProviderInd = Convert.ToInt32(e.Key) - 1;
            else
                susbidyProviderInd = 0;

            if (savedRecordInd == 0) // You only want to refresh this data when the batch has not been saved 
            {
                if (susbidyProviderInd >= 0)
                {
                    int[] UserAccess = GetUsersOSP();
                    batchTransactionDT = bulkBatchRepo.GetAddBulkTransactionBatchBySubsidyProviderKey(subsidyProviderParent[susbidyProviderInd].Key, UserAccess);
                    numberofRecordsFound = batchTransactionDT.Rows.Count;
                    batchTransaction = batchTransactionDT.DefaultView;

                    // Add the original number of trans to the cache - needed to ADD
                    if (PrivateCacheData.ContainsKey(BatchTransactionDataTable))
                        PrivateCacheData[BatchTransactionDataTable] = batchTransactionDT;
                    else
                        PrivateCacheData.Add(BatchTransactionDataTable, batchTransactionDT);

                    if (PrivateCacheData.ContainsKey(SubsidyProviderKey))
                    {
                        PrivateCacheData["SubsidyProviderPrev"] = Convert.ToInt32(PrivateCacheData[SubsidyProviderKey]);
                        PrivateCacheData[SubsidyProviderKey] = subsidyProviderParent[susbidyProviderInd].Key;
                    }
                    else
                        PrivateCacheData.Add(SubsidyProviderKey, subsidyProviderParent[susbidyProviderInd].Key);
                }
                else
                {
                    batchTransaction = null;
                    batchTransactionDT = null;
                    numberofRecordsFound = 0;
                }
            }
            else
            {
                if (PrivateCacheData.ContainsKey(BatchTransactionData) && PrivateCacheData[BatchTransactionData] != null)
                    batchTransaction = PrivateCacheData[BatchTransactionData] as DataView;

                if (PrivateCacheData.ContainsKey(BatchTransactionDataTable) && PrivateCacheData[BatchTransactionDataTable] != null)
                    batchTransactionDT = PrivateCacheData[BatchTransactionDataTable] as DataTable;

                numberofRecordsFound = batchTransactionDT.Rows.Count;
            }
        }

        void _view_OnTransactionTypeSelectedIndexChange(object sender, KeyChangedEventArgs e)
        {
            int tt = 0;

            if (Convert.ToInt32(e.Key) >= 0)
                tt = Convert.ToInt32(e.Key) - 1;

            if (PrivateCacheData.ContainsKey(transactionTypeKey))
                PrivateCacheData[transactionTypeKey] = transactionTypes[tt].Key;
            else
                PrivateCacheData.Add(transactionTypeKey, transactionTypes[tt].Key);                
        }

        void _view_OnBulkGridSelectedIndexChange(object sender, KeyChangedEventArgs e)
        {
            selectedGridIndex = Convert.ToInt32(e.Key);

            if (PrivateCacheData.ContainsKey(selectedIndexOnGrid))
                PrivateCacheData[selectedIndexOnGrid] = selectedGridIndex;
            else
                PrivateCacheData.Add(selectedIndexOnGrid, selectedGridIndex);

               if (PrivateCacheData.ContainsKey(SavedRecord) && PrivateCacheData[SavedRecord] != null)
                    savedRecordInd = Convert.ToInt32(PrivateCacheData[SavedRecord]);

                if (savedRecordInd == 1) // If the record has been saved, used the same datasets
                {
                    if (PrivateCacheData.ContainsKey(BatchTransactionData) && PrivateCacheData[BatchTransactionData] != null)
                        batchTransaction = PrivateCacheData[BatchTransactionData] as DataView;

                    if (PrivateCacheData.ContainsKey(BatchTransactionDataTable) && PrivateCacheData[BatchTransactionDataTable] != null)
                        batchTransactionDT = PrivateCacheData[BatchTransactionDataTable] as DataTable;

                    numberofRecordsFound = batchTransactionDT.Rows.Count;
                }
        }

        int [] GetUsersOSP()
        {
            // this is pretty dumb - get the string then return as int - then the repository method
            // converts back to string - after go-live rather change or overload the repository method 
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            //string[] osps = spc.OriginationSourceKeysStringForQuery.Split(',');
            //int[] result = new int[osps.Length];
            //for (int i = 0; i < osps.Length; i++)
                //result[i] = Convert.ToInt32(osps[i]);

            return spc.UserOriginationSourceKeys.ToArray(); // result;
        }

        void FilterDataTableTransactionsPerPage()
        {
            if (batchTransaction.Table.Columns.Contains("BatchTransactionKey") == false)
            {
                batchTransaction.Table.Columns.Add("BatchTransactionKey");

                for (int iX = 0; iX < batchTransaction.Count; iX++)
                    batchTransaction[iX]["BatchTransactionKey"] = iX;
            }

            batchTransaction.RowFilter = "";

            if (PrivateCacheData.ContainsKey("PageNumber") && PrivateCacheData["PageNumber"] != null)
                pageNumber = Convert.ToInt32(PrivateCacheData["PageNumber"]);

            if (PrivateCacheData.ContainsKey(BatchTransactionDataTable) && PrivateCacheData[BatchTransactionDataTable] != null)
            {
                batchTransactionDT = PrivateCacheData[BatchTransactionDataTable] as DataTable;
                numberofRecordsFound = batchTransactionDT.Rows.Count;
            }

            int startIndex = (pageNumber * pageSize) - (pageSize - 1);
            int endIndex = pageNumber * pageSize;
            if (endIndex > numberofRecordsFound)
                endIndex = numberofRecordsFound;

            System.Text.StringBuilder keys = new System.Text.StringBuilder();
            for (int iC = startIndex - 1; iC < endIndex; iC++)
            {
                keys.Append(batchTransaction[iC]["BatchTransactionKey"].ToString() + ",");
            }

            if (keys.Length == 0)
                batchTransaction.RowFilter = "BatchTransactionKey = -1";
            else
                batchTransaction.RowFilter = "BatchTransactionKey in (" + keys.ToString(0, keys.Length - 1) + ")";
           
            // Add these filtered trans to the Private Cache    
            if (PrivateCacheData.ContainsKey(BatchTransactionData))
                PrivateCacheData[BatchTransactionData] = batchTransaction;
            else
                PrivateCacheData.Add(BatchTransactionData, batchTransaction);

            int RecalcTotalPages = 1;
            if (batchTransaction.Count > 0)
                RecalcTotalPages = (int)Math.Round((numberofRecordsFound / pageSize) + 0.5, MidpointRounding.AwayFromZero);

            totalPages = RecalcTotalPages;
        }

        void _view_OnSaveButtonClicked(object sender, EventArgs e)
        {
			if (numberofRecordsFound > 0)
			{
				if (PrivateCacheData.ContainsKey(BulkBatchKey) && PrivateCacheData[BulkBatchKey] != null)
					BulkBatchKeySaved = Convert.ToInt32(PrivateCacheData[BulkBatchKey]);

				GetCachedData();

				if (BulkBatchKeySaved > 0)
					_view.SetMesssageBoxForAdd(BulkBatchKeySaved.ToString());
				else
				{ // save what is on the screen - without any amendments
					if (savedRecordInd == 0)
					{
						bulkBatchRecord = bulkBatchRepo.GetEmptyBulkBatch();
						accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
						legalentityRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                        empRepo = RepositoryFactory.GetRepository<IEmploymentRepository>();

						bulkBatchRecord.BulkBatchStatus = lookups.BulkBatchStatuses.ObjectDictionary[((int)BulkBatchStatuses.Unposted).ToString()];
						bulkBatchRecord.BulkBatchType = lookups.BulkBatchTypes.ObjectDictionary[((int)BulkBatchTypes.BulkTransaction).ToString()];
						bulkBatchRecord.IdentifierReferenceKey = susbidyProviderKeySelected;
                        //#11144 add sp name to LT Reference
                        SAHL.Common.BusinessModel.Interfaces.ISubsidyProvider sp = empRepo.GetSubsidyProviderByKey(susbidyProviderKeySelected);
                        string leName = "";
                        if (sp != null && sp.LegalEntity != null)
                        {
                            leName = sp.LegalEntity.GetLegalName(LegalNameFormat.Full);
                        }

						if (_view.EffectiveDate.HasValue)
						{
							bulkBatchRecord.EffectiveDate = _view.EffectiveDate.Value;
							if (PrivateCacheData.ContainsKey("EffectiveDate"))
								PrivateCacheData["EffectiveDate"] = _view.EffectiveDate.Value;
							else
								PrivateCacheData.Add("EffectiveDate", _view.EffectiveDate.Value);
						}
						bulkBatchRecord.UserID = _view.CurrentPrincipal.Identity.Name;

						TransactionScope txn = new TransactionScope();
						try
						{
							// call save first - we need to do this so we get a key as a reference for the transaction records
							bulkBatchRepo.SaveBulkBatch(bulkBatchRecord);

							for (int k = 0; k < batchTransactionDT.Rows.Count; k++)
							{
								IBatchTransaction batchTransactionRecord = bulkBatchRepo.GetEmptyBatchTransaction();

								batchTransactionRecord.Account = accRepo.GetAccountByKey(Convert.ToInt32(batchTransactionDT.Rows[k]["AccountKey"]));
								batchTransactionRecord.LegalEntity = legalentityRepo.GetLegalEntityByKey(Convert.ToInt32(batchTransactionDT.Rows[k]["LegalEntityKey"]));
								batchTransactionRecord.TransactionTypeNumber = transactionTypeSelected;

								batchTransactionRecord.EffectiveDate = _view.EffectiveDate.Value;
								batchTransactionRecord.UserID = _view.CurrentPrincipal.Identity.Name;
                                batchTransactionRecord.Reference = BatchReferenceText("Bulk Txn :", bulkBatchRecord.Key.ToString(), leName);
								batchTransactionRecord.BatchTransactionStatus = lookups.BatchTransactionStatuses[0];
								batchTransactionRecord.Amount = Convert.ToDouble(batchTransactionDT.Rows[k]["Amount"]);
								batchTransactionRecord.BulkBatch = bulkBatchRecord;

								amount += batchTransactionRecord.Amount;
								bulkBatchRecord.BatchTransactions.Add(_view.Messages, batchTransactionRecord);
							}


							// update the description and save again along with the transaction records
							bulkBatchRecord.Description = bulkBatchRecord.Key.ToString() + " - " + bulkBatchRecord.EffectiveDate.ToString(SAHL.Common.Constants.DateFormat) + " - " + bulkBatchRecord.UserID + " - " + amount.ToString();
							bulkBatchRepo.SaveBulkBatch(bulkBatchRecord);
							txn.VoteCommit();
						}
						catch (Exception)
						{
							txn.VoteRollBack();
							if (_view.IsValid)
								throw;
							else
							{
								FixDomainMessages();
								return;
							}
						}
						finally
						{
							txn.Dispose();
						}
						if (_view.IsValid)
						{
							_view.SetMesssageBoxForAdd(bulkBatchRecord.Key.ToString());
							_view.SetUpdateButtons = true;

							// Save the BulkBatchKey to be used by subsequent updates
							if (PrivateCacheData.ContainsKey(BulkBatchKey))
								PrivateCacheData[BulkBatchKey] = bulkBatchRecord.Key;
							else
								PrivateCacheData.Add(BulkBatchKey, bulkBatchRecord.Key);

							// The save to BulkBatch and BatchTransactions must happen only once, thereafter, it will be an update
							if (PrivateCacheData.ContainsKey(SavedRecord))
								PrivateCacheData[SavedRecord] = 1;
							else
								PrivateCacheData.Add(SavedRecord, 1);
						}
					}
				}
			}
			else
			{
				string errorMessage = "There are no bulk transactions to be saved.";
				_view.Messages.Add(new Error(errorMessage, errorMessage));
			}
        }

        private static string BatchReferenceText(string preFix, string RefNo, string sufFix)
        {
            string retval = String.Format(@"{0} {1} {2}", preFix, RefNo, sufFix);
            //The Reference field is varchar(50), make sure the reference does not exceed this
            //this is currently limited by [SAHLDB].dbo.ArrearTransaction, pLoanProcessTran, pLoanPostFinTran, pLoanPostMemoTran
            if (retval.Length > 50)
                retval = retval.Remove(50);

            return retval;
        }

        void _view_onFindButtonClicked(object sender, KeyChangedEventArgs e)
        {
            int accountNumber = Convert.ToInt32(e.Key);
            int rowNumberOfAccount = 0;
            bool accountFound = false;

            if (PrivateCacheData.ContainsKey(BatchTransactionDataTable) && PrivateCacheData[BatchTransactionDataTable] != null)
                batchTransactionDT = PrivateCacheData[BatchTransactionDataTable] as DataTable;

            if (batchTransactionDT != null && batchTransactionDT.Rows.Count > 0)
            {
                for (int i = 0; i < batchTransactionDT.Rows.Count; i++)
                {
                    if (Convert.ToInt32(batchTransactionDT.Rows[i]["AccountKey"]) == accountNumber) //ItemArray zero will be the account key
                    {
                        accountFound = true;
                        rowNumberOfAccount = i;
                        i = batchTransactionDT.Rows.Count; // can exit this once account is found
                    }
                }
                if (accountFound && (rowNumberOfAccount >= 0 && rowNumberOfAccount <= batchTransactionDT.Rows.Count))
                {
                    pageNumber = (int)Math.Round((rowNumberOfAccount / pageSize) + 0.5, MidpointRounding.AwayFromZero);
                   
                    if (PrivateCacheData.ContainsKey("PageNumber") && PrivateCacheData["PageNumber"] != null)
                        PrivateCacheData["PageNumber"] = pageNumber;

                    selectedGridIndex = rowNumberOfAccount - (pageSize * (pageNumber - 1));

                    if (PrivateCacheData.ContainsKey(selectedIndexOnGrid))
                        PrivateCacheData[selectedIndexOnGrid] = selectedGridIndex;
                    else
                        PrivateCacheData.Add(selectedIndexOnGrid, selectedGridIndex);
                }
                else
                    _view.SetErrorMessage("Account not found");
            }
            else
                _view.SetErrorMessage("There are no account to find");
        }
   
        void _view_OnUpdateButtonClicked(object sender, EventArgs e)
        {
           GetCachedData();

            // This the BatchTransactionKey of the selected Index on the Grid
            selectedBatchTransactionKey = Convert.ToInt32(batchTransaction[selectedInd]["BatchTransactionKey"]);
                        
            if (savedRecordInd == 0) // Add Option
                SaveBatchPriorToUpdate(selectedBatchTransactionKey, (int)UpdateAction.Update); // End of Add Option
            else
                SaveBatchAfterUpdate(selectedBatchTransactionKey, (int)UpdateAction.Update);
       }

        private void SaveBatchAfterUpdate(int selectedBatchTransactionKey, int UpdateAction)
        { // Update the Record that has been Saved

            int batchTransactionStatusKey = 0;
            if (PrivateCacheData.ContainsKey(BulkBatchKey) && PrivateCacheData[BulkBatchKey] != null)
                BulkBatchKeySaved = Convert.ToInt32(PrivateCacheData[BulkBatchKey]);
            
            if (BulkBatchKeySaved > 0)
            {
                bulkBatchRec = bulkBatchRepo.GetBulkBatchByKey(BulkBatchKeySaved);
                batchTransactionRecs = bulkBatchRepo.GetBatchTransactionsByBulkBatchKey(BulkBatchKeySaved);

                if (UpdateAction == 1) // Update
                {
                    if (_view.TransactionAmount.HasValue)
                        batchTransactionRecs[selectedBatchTransactionKey].Amount = _view.TransactionAmount.Value;
                    batchTransactionRecs[selectedBatchTransactionKey].BatchTransactionStatus = lookups.BatchTransactionStatuses[1]; // modified
                    batchTransactionStatusKey = 1;
                }
                else // Remove button clicked
                {   // If Record was marked as Deleted, then update to Modified, else mark as Deleted
                    if (batchTransactionRecs[selectedBatchTransactionKey].BatchTransactionStatus.Key == 4)
                    {
                        batchTransactionRecs[selectedBatchTransactionKey].BatchTransactionStatus = lookups.BatchTransactionStatuses[1]; // Modified
                        batchTransactionStatusKey = 1;
                    }
                    else
                    {
                        batchTransactionRecs[selectedBatchTransactionKey].BatchTransactionStatus = lookups.BatchTransactionStatuses[4]; // Deleted
                        batchTransactionStatusKey = 4;
                    }
                }
                for (int x = 0; x < batchTransactionRecs.Count; x++)
                {
                    if (batchTransactionRecs[x].BatchTransactionStatus.Key != 4 && batchTransactionRecs[x].BatchTransactionStatus.Key != 3)
                       amount += Convert.ToDouble(batchTransactionRecs[x].Amount);
                }

                bulkBatchRec.Description = bulkBatchRec.Key.ToString() + " - " + bulkBatchRec.EffectiveDate.ToString(SAHL.Common.Constants.DateFormat) + " - " + bulkBatchRec.UserID + " - " + amount.ToString();

                TransactionScope txn = new TransactionScope();
                try
                {
                    bulkBatchRepo.SaveBulkBatch(bulkBatchRec);
                    bulkBatchRepo.SaveBatchTransaction(batchTransactionRecs[selectedBatchTransactionKey]);
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
                {
                    batchTransaction[selectedInd].Row["BatchTransactionStatusKey"] = lookups.BatchTransactionStatuses[batchTransactionStatusKey].Key;
                    if (UpdateAction == 1)
                        batchTransaction[selectedInd].Row["Amount"] = _view.TransactionAmount.Value;

                    // Add Batch Transaction to the Cache as from here onword, this is where we wud get it from
                    if (PrivateCacheData.ContainsKey(BatchTransactionData))
                        PrivateCacheData[BatchTransactionData] = batchTransaction;
                    else
                        PrivateCacheData.Add(BatchTransactionData, batchTransaction);
                }
            }
        }

        private void SaveBatchPriorToUpdate(int selectedBatchTransactionKey, int UpdateAction)
        {
            bulkBatchRecord = bulkBatchRepo.GetEmptyBulkBatch();
            accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            legalentityRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            empRepo = RepositoryFactory.GetRepository<IEmploymentRepository>();

            bulkBatchRecord.BulkBatchStatus = lookups.BulkBatchStatuses[0]; // Unposted
            bulkBatchRecord.BulkBatchType = lookups.BulkBatchTypes[0]; // Bulk Transaction
            bulkBatchRecord.IdentifierReferenceKey = susbidyProviderKeySelected;
            //#11144 add sp name to LT Reference
            SAHL.Common.BusinessModel.Interfaces.ISubsidyProvider sp = empRepo.GetSubsidyProviderByKey(susbidyProviderKeySelected);
            string leName = "";
            if (sp != null && sp.LegalEntity != null)
            {
                leName = sp.LegalEntity.GetLegalName(LegalNameFormat.Full);
            }

			if (_view.EffectiveDate.HasValue)
			{
				bulkBatchRecord.EffectiveDate = _view.EffectiveDate.Value;
                if (PrivateCacheData.ContainsKey("EffectiveDate"))
                    PrivateCacheData["EffectiveDate"] = _view.EffectiveDate.Value;
                else
                    PrivateCacheData.Add("EffectiveDate", _view.EffectiveDate.Value);
            }
            bulkBatchRecord.UserID = _view.CurrentPrincipal.Identity.Name;

            TransactionScope txn = new TransactionScope();
            try
            {
                bulkBatchRepo.SaveBulkBatch(bulkBatchRecord);

                for (int k = 0; k < batchTransactionDT.Rows.Count; k++)
                {
                    IBatchTransaction batchTransactionRecord = bulkBatchRepo.GetEmptyBatchTransaction();

                    batchTransactionRecord.Account = accRepo.GetAccountByKey(Convert.ToInt32(batchTransactionDT.Rows[k]["AccountKey"]));
                    batchTransactionRecord.LegalEntity = legalentityRepo.GetLegalEntityByKey(Convert.ToInt32(batchTransactionDT.Rows[k]["LegalEntityKey"]));
                    batchTransactionRecord.TransactionTypeNumber = transactionTypeSelected;
                    batchTransactionRecord.EffectiveDate = _view.EffectiveDate.Value;
                    batchTransactionRecord.UserID = _view.CurrentPrincipal.Identity.Name;
                    batchTransactionRecord.Reference = BatchReferenceText("Bulk Txn :", bulkBatchRecord.Key.ToString(), leName);
                    batchTransactionRecord.BulkBatch = bulkBatchRecord;

                    if (k == selectedBatchTransactionKey) // This is the selected Index on the Grid in relation to the DataTable
                    {
                        if (UpdateAction == 1)
                        {
                            batchTransactionRecord.BatchTransactionStatus = lookups.BatchTransactionStatuses.ObjectDictionary[((int)BatchTransactionStatuses.Modified).ToString()];
                            if (_view.TransactionAmount.HasValue)
                            {
                                batchTransactionRecord.Amount = _view.TransactionAmount.Value;
                                amountAdjustment = batchTransactionRecord.Amount;
                            }
                        }
                        else
                        {
                            batchTransactionRecord.BatchTransactionStatus = lookups.BatchTransactionStatuses.ObjectDictionary[((int)BatchTransactionStatuses.Deleted).ToString()];
                            if (_view.TransactionAmount.HasValue)
                            {
                                batchTransactionRecord.Amount = _view.TransactionAmount.Value;
                                amountAdjustment = (batchTransactionRecord.Amount * -1); // Amount must be subtracted from total sum
                            }
                        }
                    }
                    else
                    {
                        batchTransactionRecord.BatchTransactionStatus = lookups.BatchTransactionStatuses.ObjectDictionary[((int)BatchTransactionStatuses.Original).ToString()];
                        batchTransactionRecord.Amount = Convert.ToDouble(batchTransactionDT.Rows[k]["Amount"]);
                        amountAdjustment = Convert.ToDouble(batchTransactionDT.Rows[k]["Amount"]);
                    }

                    amount += amountAdjustment;
                    bulkBatchRecord.BatchTransactions.Add(_view.Messages, batchTransactionRecord);
                }
                if (bulkBatchRecord.BatchTransactions != null)
                {
                    for (int n = 0; n < bulkBatchRecord.BatchTransactions.Count; n++)
                        bulkBatchRepo.SaveBatchTransaction(bulkBatchRecord.BatchTransactions[n]);
                }

                bulkBatchRecord.Description = bulkBatchRecord.Key.ToString() + " - " + bulkBatchRecord.EffectiveDate.ToString(SAHL.Common.Constants.DateFormat) + " - " + bulkBatchRecord.UserID + " - " + amount.ToString();
                bulkBatchRepo.SaveBulkBatch(bulkBatchRecord);
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
            {
                _view.SetMesssageBoxForAdd(bulkBatchRecord.Key.ToString());
                _view.SetUpdateButtons = true;

                // Change the status and amount fields on the Original Data used to Populate Grid
                batchTransaction[selectedInd].Row["BatchTransactionStatusKey"] = (int)BatchTransactionStatuses.Modified;
                batchTransaction[selectedInd].Row["Amount"] = _view.TransactionAmount.Value;

                // Save the BulkBatchKey to be used by subsequent updates
                if (PrivateCacheData.ContainsKey(BulkBatchKey))
                    PrivateCacheData[BulkBatchKey] = bulkBatchRecord.Key;
                else
                    PrivateCacheData.Add(BulkBatchKey, bulkBatchRecord.Key);

                // The save to BulkBatch and BatchTransactions must happen only once, thereafter, it will be an update
                if (PrivateCacheData.ContainsKey(SavedRecord))
                    PrivateCacheData[SavedRecord] = 1;
                else
                    PrivateCacheData.Add(SavedRecord, 1);

                // Add Batch Transaction to the Cache as from here onword, this is where we wud get it from
                if (PrivateCacheData.ContainsKey(BatchTransactionData))
                    PrivateCacheData[BatchTransactionData] = batchTransaction;
                else
                    PrivateCacheData.Add(BatchTransactionData, batchTransaction);
            }
        }

        void _view_OnRemoveButtonClicked(object sender, EventArgs e)
        {
            GetCachedData();

            // This the BatchTransactionKey of the selected Index on the Grid
            int selectedBatchTransactionKey = Convert.ToInt32(batchTransaction[selectedInd]["BatchTransactionKey"]);

            if (savedRecordInd == 0) // Add Option
                SaveBatchPriorToUpdate(selectedBatchTransactionKey, (int)UpdateAction.Remove); // End of Add Option
            else
                SaveBatchAfterUpdate(selectedBatchTransactionKey, (int)UpdateAction.Remove);
        }

        void _view_OnPostButtonClicked(object sender, EventArgs e)
        {
            double ReadvanceComparison = 0;
            IAccount acc;
            int transactionTypeNumber;
            // DataTable batchLoanTransactionsDT = null;
            accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            double amountPosted = 0;
            bool successfulPost = false;
            IBatchTransaction btRec = null;
            int bulkBatchKey = 0;
            IBulkBatch bulkBatch = null;

            if (PrivateCacheData.ContainsKey(SavedRecord) && PrivateCacheData[SavedRecord] != null)
                savedRecordInd = Convert.ToInt32(PrivateCacheData[SavedRecord]);
            else
                savedRecordInd = 0;

            // Save the BulkBatchKey to be used by subsequent updates
            if (PrivateCacheData.ContainsKey(BulkBatchKey))
                bulkBatchKey = Convert.ToInt32(PrivateCacheData[BulkBatchKey]);

            bool errorOccurred = false;

            if (savedRecordInd == 1 && bulkBatchKey > 0)
            {
                TransactionScope txn = new TransactionScope();
                try
                {
                    bulkBatch = bulkBatchRepo.GetBulkBatchByKey(bulkBatchKey);

                    IRuleService svc = ServiceFactory.GetService<IRuleService>();
                    int batchExists = svc.ExecuteRule(_view.Messages, "BulkBatchAlreadyPosted", bulkBatch);
                    if (batchExists == 0)
                        return;

                    batchTransactionDT = bulkBatchRepo.GetUpdateBulkTransactionBatchByBulkBatchKey(bulkBatchKey);
                    transactionTypeNumber = Convert.ToInt32(bulkBatch.BatchTransactions[0].TransactionTypeNumber);
                    // If there is a Readvance Restriction or Paid Up with HOC problem, do not call posting of tran, instead mark account as failed.
                    for (int i = 0; i < batchTransactionDT.Rows.Count; i++)
                    {
                        if ((Convert.ToInt32(batchTransactionDT.Rows[i]["BatchTransactionStatusKey"]) != 3) && (Convert.ToInt32(batchTransactionDT.Rows[i]["BatchTransactionStatusKey"]) != 4))
                        {
                            if (transactionTypeNumber == 140) // Readvance
                            {
                                acc = accRepo.GetAccountByKey(Convert.ToInt32(batchTransactionDT.Rows[i]["AccountKey"]));
                                IAccountHOC hocAcc = acc.GetRelatedAccountByType(AccountTypes.HOC, acc.RelatedChildAccounts) as IAccountHOC;
                                mlRepo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
                                ReadvanceComparison = mlRepo.GetReadvanceComparisonAmount(Convert.ToInt32(batchTransactionDT.Rows[i]["AccountKey"]));
                                // If Amount exceeds Readvance Tolerance, do not post transaction and mark as errir
                                if ((ReadvanceComparison + Convert.ToDouble(batchTransactionDT.Rows[i]["Amount"])) > 0)
                                {
                                    errorOccurred = true;
                                    batchTransactionRecord = bulkBatchRepo.GetBatchTransactionByKey(Convert.ToInt32(batchTransactionDT.Rows[i]["BatchTransactionKey"]));
                                    batchTransactionRecord.BatchTransactionStatus = lookups.BatchTransactionStatuses[2]; // Error
                                    batchTransactionRecord.UserID = _view.CurrentPrincipal.Identity.Name;
                                    bulkBatchRepo.SaveBatchTransaction(batchTransactionRecord);
                                }
                                else // Does not exceed Tolerance but check for Paid Up with HOC
                                    if (hocAcc.HOC.HOCStatus.Key == 3) // Paid Up with NO HOC
                                    {
                                        errorOccurred = true;
                                        batchTransactionRecord = bulkBatchRepo.GetBatchTransactionByKey(Convert.ToInt32(batchTransactionDT.Rows[i]["BatchTransactionKey"]));
                                        batchTransactionRecord.BatchTransactionStatus = lookups.BatchTransactionStatuses[2]; // Error
                                        batchTransactionRecord.UserID = _view.CurrentPrincipal.Identity.Name;
                                        bulkBatchRepo.SaveBatchTransaction(batchTransactionRecord);
                                    }
                                    else
                                    { // Try posting valid Readvance Transaction if passes Tolerance and Paid Up with HOC Checks
                                        btRec = bulkBatchRepo.GetBatchTransactionByKey(Convert.ToInt32(batchTransactionDT.Rows[i]["BatchTransactionKey"]));
                                        bulkBatchRepo.PostTransaction(Convert.ToInt32(batchTransactionDT.Rows[i]["BatchTransactionKey"]), Convert.ToInt32(batchTransactionDT.Rows[i]["AccountKey"]), transactionTypeNumber, Convert.ToDateTime(btRec.EffectiveDate), float.Parse((batchTransactionDT.Rows[i]["Amount"].ToString())), btRec.Reference.ToString(), _view.CurrentPrincipal.Identity.Name.ToString());
                                    }
                            }// End of Readvances
                            else // Not a Readvance
                            { // Try posting valid  Transaction
                                // Need to go back and fetch this tran cos the datatable does not have effectiveDate
                                btRec = bulkBatchRepo.GetBatchTransactionByKey(Convert.ToInt32(batchTransactionDT.Rows[i]["BatchTransactionKey"]));
                                bulkBatchRepo.PostTransaction(Convert.ToInt32(batchTransactionDT.Rows[i]["BatchTransactionKey"]), Convert.ToInt32(batchTransactionDT.Rows[i]["AccountKey"]), transactionTypeNumber, Convert.ToDateTime(btRec.EffectiveDate), float.Parse((batchTransactionDT.Rows[i]["Amount"].ToString())), btRec.Reference.ToString(), _view.CurrentPrincipal.Identity.Name.ToString());
                                if (!_view.IsValid)
                                {
                                    errorOccurred = true;
                                }
                                else // Get the successfully posted tran and sum the amounts
                                {
                                    IReadOnlyEventList<IFinancialTransaction> list = bulkBatchRepo.GetBatchLoanTransactions(Convert.ToInt32(batchTransactionDT.Rows[i]["AccountKey"]), transactionTypeNumber, Convert.ToDateTime(btRec.EffectiveDate), bulkBatch.BatchTransactions[0].Reference.ToString(), _view.CurrentPrincipal.Identity.Name);

                                    for (int k = 0; k < list.Count; k++)
                                    {
                                        amountPosted += list[k].Amount;
                                        successfulPost = true;
                                    }
                                }
                            }
                        }
                    }
                    // Update the amount with successfully posted trans and update the status
                    if (successfulPost)
                    {
                        bulkBatch.BulkBatchStatus = lookups.BulkBatchStatuses[2]; // successful
                        bulkBatch.Description = bulkBatch.Key.ToString() + " - " + bulkBatch.EffectiveDate.ToString(SAHL.Common.Constants.DateFormat) + " - " + _view.CurrentPrincipal.Identity.Name + " - " + amountPosted.ToString();
                        bulkBatchRepo.SaveBulkBatch(bulkBatch);
                    }
                    else // else mark the batch as failed
                        bulkBatch.BulkBatchStatus = lookups.BulkBatchStatuses[1]; // Failed

                    bulkBatchRepo.SaveBulkBatch(bulkBatch);
                }
                catch (Exception)
                {
                    txn.VoteRollBack();
                    errorOccurred = true;
                    if (_view.IsValid)
                        throw;
                }
                finally
                {
                    txn.Dispose();
                }

                if (errorOccurred)
                    _view.SetBatchPostErrorMessage("Batch Number " + bulkBatchKey.ToString() + " has completed with errors");
                else
                    Navigator.Navigate("BulkTransactionBatch");
            }
        }

        void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("BulkTransactionBatch");
        }

        private void GetCachedData()
        {
            // Read the selected Grid Index off the private cache
            if (PrivateCacheData.ContainsKey(selectedIndexOnGrid))
                selectedInd = Convert.ToInt32(PrivateCacheData[selectedIndexOnGrid]);

            if (PrivateCacheData.ContainsKey(SubsidyProviderKey) && PrivateCacheData[SubsidyProviderKey] != null)
                susbidyProviderKeySelected = Convert.ToInt32(PrivateCacheData[SubsidyProviderKey]);

            if (PrivateCacheData.ContainsKey(transactionTypeKey) && PrivateCacheData[transactionTypeKey] != null)
                transactionTypeSelected = Convert.ToInt32(PrivateCacheData[transactionTypeKey]);

            if (PrivateCacheData.ContainsKey(SavedRecord) && PrivateCacheData[SavedRecord] != null)
                savedRecordInd = Convert.ToInt32(PrivateCacheData[SavedRecord]);
            else
                savedRecordInd = 0;

            if (PrivateCacheData.ContainsKey(BatchTransactionData) && PrivateCacheData[BatchTransactionData] != null)
                batchTransaction = PrivateCacheData[BatchTransactionData] as DataView;

            if (PrivateCacheData.ContainsKey(BatchTransactionDataTable) && PrivateCacheData[BatchTransactionDataTable] != null)
                batchTransactionDT = PrivateCacheData[BatchTransactionDataTable] as DataTable;

            if (PrivateCacheData.ContainsKey("SubsidyProviderTypeKey") && PrivateCacheData["SubsidyProviderTypeKey"] != null)
                subsidyProviderTypeKey = Convert.ToInt32(PrivateCacheData["SubsidyProviderTypeKey"]);

            if (PrivateCacheData.ContainsKey("EffectiveDate") && PrivateCacheData["EffectiveDate"] != null)
                effectiveDate = Convert.ToDateTime(PrivateCacheData["EffectiveDate"]);
        }
    }
}
