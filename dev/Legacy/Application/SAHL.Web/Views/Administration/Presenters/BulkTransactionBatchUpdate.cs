using System;
using System.Data;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Web.Views.Administration.Interfaces;
using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.Globals;
using SAHL.Common.DomainMessages;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Web.Views.Administration.Presenters
{
	/// <summary>
	/// BulkTransaction Update Presenter
	/// </summary>
	public class BulkTransactionBatchUpdate : BulkBatchTransactionBase
	{
		int batchStatusKey;
		double amount;
		int batchNumberPrev;
		int selectedGridIndex;
		IBulkBatch bulkBatchRec;
		int bulkBatchKey;
		IBatchTransaction batchTransactionRec;
		IBatchTransaction batchTransactionRecord;
		IEmploymentRepository empRepo;

		const string BatchStatusSelectedItem = "BatchStatusSelectedItem";

		/// <summary>
		/// Constructor for BultTransactionBatch Add
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public BulkTransactionBatchUpdate(IBulkTransactionBatch view, SAHLCommonBaseController controller)
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

			_view.SetControlsForUpdate();
			_view.SetButtonsNextAndPrevious = false;
			_view.SetPostBackType();
			_view.SetUpdateButtons = false;

			empRepo = RepositoryFactory.GetRepository<IEmploymentRepository>();

			GetPagesNumberFromCache();

			_view.BindBatchStatusForUpdate(lookups.BulkBatchStatuses);

			if (!_view.IsPostBack)
				_view.BindBatchNumber(null);

			if (PrivateCacheData.ContainsKey(BatchStatusSelectedItem))
				bulkBatch = PrivateCacheData[BatchStatusSelectedItem] as IList<IBulkBatch>;

			if (bulkBatch != null)
				_view.BindBatchNumber(bulkBatch);

			_view.OnBatchStatusSelectedIndexChange += (_view_OnBatchStatusSelectedIndexChange);
			_view.OnBatchNumberSelectedIndexChange += (_view_OnBatchNumberSelectedIndexChange);
			_view.onNextButtonClicked += (_view_onNextButtonClicked);
			_view.onPrevButtonClicked += (_view_onPrevButtonClicked);
			_view.onFindButtonClicked += (_view_onFindButtonClicked);
			_view.OnUpdateButtonClicked += (_view_OnUpdateButtonClicked);
			_view.OnBulkGridSelectedIndexChange += (_view_OnBulkGridSelectedIndexChange);
			_view.OnSaveButtonClicked += (_view_OnSaveButtonClicked);
			_view.OnRemoveButtonClicked += (_view_OnRemoveButtonClicked);
			_view.OnPostButtonClicked += (_view_OnPostButtonClicked);
			_view.OnCancelButtonClicked += (_view_OnCancelButtonClicked);
		}

		private void GetPagesNumberFromCache()
		{
			if (PrivateCacheData.ContainsKey("PageNumber"))
				pageNumber = Convert.ToInt32(PrivateCacheData["PageNumber"]);
			else
				PrivateCacheData.Add("PageNumber", pageNumber);
		}
		/// <summary>
		/// OnView PreRender event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void OnViewPreRender(object sender, EventArgs e)
		{

			base.OnViewPreRender(sender, e);
			if (!View.ShouldRunPage) return;

			if (_view.IsPostBack && batchTransaction != null && batchTransaction.Count > 0)
			{
				if (PrivateCacheData.ContainsKey(selectedIndexOnGrid))
					selectedGridIndex = Convert.ToInt32(PrivateCacheData[selectedIndexOnGrid]);
				else
					selectedGridIndex = 0;

				if (batchNumber >= 0 && batchNumber != batchNumberPrev)
				{
					ResetPageNumbers();
					selectedGridIndex = 0;
					if (PrivateCacheData.ContainsKey(SavedRecord))
						PrivateCacheData[SavedRecord] = 0;
				}

				FilterTransactionsPerPageForUpdate();
				_view.BindBulkGridAdd("Update", selectedGridIndex, pageNumber, totalPages, numberofRecordsFound, batchTransaction);

				if (PrivateCacheData.ContainsKey(SavedRecord) && Convert.ToInt32(PrivateCacheData[SavedRecord]) == 1)
					_view.SetUpdateButtons = true;
				_view.SetAddUpdateControls = true;
				_view.SetSelectedIndexOnGrid(selectedGridIndex);

			}
		}

		void _view_OnBatchStatusSelectedIndexChange(object sender, KeyChangedEventArgs e)
		{
			if (Convert.ToInt32(e.Key) >= 0)
				batchStatusKey = Convert.ToInt32(e.Key) - 1;
			else
				batchStatusKey = 0;

			RepopulateBatchBulk();
		}

		void _view_OnBatchNumberSelectedIndexChange(object sender, KeyChangedEventArgs e)
		{
			if (Convert.ToInt32(e.Key) >= 0)
				batchNumber = Convert.ToInt32(e.Key) - 1;
			else
				batchNumber = 0;

			if (batchNumber >= 0)
			{
				batchTransactionDT = bulkBatchRepo.GetUpdateBulkTransactionBatchByBulkBatchKey(bulkBatch[batchNumber].Key);
				numberofRecordsFound = batchTransactionDT.Rows.Count;
				batchTransaction = batchTransactionDT.DefaultView;

				if (PrivateCacheData.ContainsKey(BatchTransactionDataTable))
					PrivateCacheData[BatchTransactionDataTable] = batchTransactionDT;
				else
					PrivateCacheData.Add(BatchTransactionDataTable, batchTransactionDT);

				string displayName = String.Empty;
				if (bulkBatch[batchNumber].IdentifierReferenceKey > 0)
				{
					subsidyProviderDescription = empRepo.GetSubsidyProviderByKey(bulkBatch[batchNumber].IdentifierReferenceKey);
					displayName = subsidyProviderDescription.LegalEntity.DisplayName;
				}


				string loanDescription = String.Empty;
				if (bulkBatch[batchNumber].BatchTransactions.Count > 0 && bulkBatch[batchNumber].BatchTransactions[0].TransactionTypeNumber > 0)
				{
					transactionTypeDescription = lookups.TransactionTypes.ObjectDictionary[bulkBatch[batchNumber].BatchTransactions[0].TransactionTypeNumber.ToString()];
                    loanDescription = transactionTypeDescription.Description;
				}

				_view.BindBatchFields(bulkBatch[batchNumber], displayName, loanDescription);
			}
			else
			{
				batchTransaction = null;
				if (PrivateCacheData.ContainsKey(BatchTransactionDataTable))
					PrivateCacheData[BatchTransactionDataTable] = null;
				numberofRecordsFound = 0;
			}
			if (PrivateCacheData.ContainsKey("ChangeEventOfBatchNumberFired") && PrivateCacheData["ChangeEventOfBatchNumberFired"] != null)
			{
				batchNumberPrev = Convert.ToInt32(PrivateCacheData["ChangeEventOfBatchNumberFired"]);
				PrivateCacheData["ChangeEventOfBatchNumberFired"] = batchNumber;
			}
			else
				PrivateCacheData.Add("ChangeEventOfBatchNumberFired", batchNumber);
		}

		void GetDataFromPrivateCache()
		{
			if (PrivateCacheData.ContainsKey(BatchTransactionData) && PrivateCacheData[BatchTransactionData] != null)
				batchTransaction = PrivateCacheData[BatchTransactionData] as DataView;

			if (PrivateCacheData.ContainsKey(BatchTransactionDataTable))
				batchTransactionDT = PrivateCacheData[BatchTransactionDataTable] as DataTable;

			if (PrivateCacheData.ContainsKey(selectedIndexOnGrid))
				selectedGridIndex = Convert.ToInt32(PrivateCacheData[selectedIndexOnGrid]);
		}

		void _view_OnUpdateButtonClicked(object sender, EventArgs e)
		{
			bulkBatchKey = bulkBatch[batchNumber].Key;
		   
			GetDataFromPrivateCache();

			int selectedBatchTransactionKey = Convert.ToInt32(batchTransaction[selectedGridIndex]["BatchTransactionKey"]);
			int subsidyKey = Convert.ToInt32(batchTransaction[selectedGridIndex]["SubsidyKey"]);

			if (bulkBatchKey > 0)
			{
				ISubsidy subsidy = empRepo.GetSubsidyByKey(subsidyKey);
				bulkBatchRec = bulkBatch[batchNumber];
				batchTransactionRec = bulkBatchRepo.GetBatchTransactionByKey(selectedBatchTransactionKey);
				// I need to use the DT here bcos the rec brought back in here do not = recs on batchtransaction table for bulkbatch
				for (int i = 0; i < batchTransactionDT.Rows.Count; i++)
				{
					if (Convert.ToInt32(batchTransactionDT.Rows[i]["batchTransactionStatusKey"]) != 3 && Convert.ToInt32(batchTransactionDT.Rows[i]["batchTransactionStatusKey"]) != 4)
						amount += Convert.ToDouble(batchTransactionDT.Rows[i]["Amount"]); // need sum of all in DT that are not deleted or posted 
				}

				double AmountToAdjust = Convert.ToDouble(batchTransactionRec.Amount);
				if (_view.TransactionAmount.HasValue)
				{
					amount = (amount - AmountToAdjust) + _view.TransactionAmount.Value; // subtract the amt that has changed and then add the new changed amount
					batchTransactionRec.Amount = _view.TransactionAmount.Value;
				}
				batchTransactionRec.BatchTransactionStatus = lookups.BatchTransactionStatuses[1]; // modified


				if (_view.EffectiveDate.HasValue)
				{
					batchTransactionRec.EffectiveDate = _view.EffectiveDate.Value;
					bulkBatchRec.EffectiveDate = _view.EffectiveDate.Value;
				}

				batchTransactionRec.UserID = _view.CurrentPrincipal.Identity.Name;
				bulkBatchRec.Description = bulkBatchRec.Key + " - " + bulkBatchRec.EffectiveDate.ToString(SAHL.Common.Constants.DateFormat) + " - " + bulkBatchRec.UserID + " - " + amount;
				subsidy.StopOrderAmount = batchTransactionRec.Amount;
				TransactionScope txn = new TransactionScope();
				try
				{
                    this.ExclusionSets.Add(RuleExclusionSets.SubsidyDetailsMandatorySalaryNumber);
                    empRepo.SaveSubsidy(subsidy);
					bulkBatchRepo.SaveBulkBatch(bulkBatchRec);
					if (batchTransactionRec != null)
						bulkBatchRepo.SaveBatchTransaction(batchTransactionRec);
                    this.ExclusionSets.Remove(RuleExclusionSets.SubsidyDetailsMandatorySalaryNumber);
					txn.VoteCommit();
                    
				}

				catch (Exception)
				{
					txn.VoteRollBack();
					if (View.IsValid)
						throw;
				}

				finally
				{
					txn.Dispose();
				}

				if (_view.IsValid)
				{
					batchTransaction[selectedGridIndex].Row["BatchTransactionStatusKey"] = lookups.BatchTransactionStatuses[1].Key;
					batchTransaction[selectedGridIndex].Row["Amount"] = _view.TransactionAmount.Value;
					batchTransaction[selectedGridIndex].Row["StopOrderAmount"] = _view.TransactionAmount.Value;
					BatchTransactionAddToPrivateCache();

					RepopulateBatchBulk();
				}
			}
		}

		void _view_OnSaveButtonClicked(object sender, EventArgs e)
		{
			if (numberofRecordsFound > 0)
			{
				IBulkBatch bulkBatchRecord;

				IList<IBatchTransaction> batchTransactionRecords;
				int _bulkBatchKey = bulkBatch[batchNumber].Key;

				if (PrivateCacheData.ContainsKey(BatchTransactionData) && PrivateCacheData[BatchTransactionData] != null)
					batchTransaction = PrivateCacheData[BatchTransactionData] as DataView;

				if (_bulkBatchKey > 0)
				{
					bulkBatchRecord = bulkBatchRepo.GetBulkBatchByKey(_bulkBatchKey);
					batchTransactionRecords = bulkBatchRepo.GetBatchTransactionsByBulkBatchKey(_bulkBatchKey);

					if (_view.EffectiveDate.HasValue)
						bulkBatchRecord.EffectiveDate = _view.EffectiveDate.Value;

					TransactionScope txn = new TransactionScope();
					try
					{
						bulkBatchRepo.SaveBulkBatch(bulkBatchRecord);
						if (batchTransactionRecords != null)
						{
							for (int a = 0; a < batchTransactionRecords.Count; a++)
							{
								if (_view.EffectiveDate.HasValue)
									batchTransactionRecords[a].EffectiveDate = _view.EffectiveDate.Value;

								bulkBatchRepo.SaveBatchTransaction(batchTransactionRecords[a]);
							}
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
					if (_view.IsValid)
					{
						BatchTransactionAddToPrivateCache();

						if (PrivateCacheData.ContainsKey(SavedRecord))
							PrivateCacheData[SavedRecord] = 1;
						else
							PrivateCacheData.Add(SavedRecord, 1);
					}
				}
			}
			else
			{
				string errorMessage = "There are no bulk transactions to be saved.";
				_view.Messages.Add(new Error(errorMessage, errorMessage));
			}
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
					double _calcval = rowNumberOfAccount/pageSize;
					pageNumber = (int)Math.Round(_calcval + 0.5, MidpointRounding.AwayFromZero);

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

		void _view_OnBulkGridSelectedIndexChange(object sender, KeyChangedEventArgs e)
		{
			selectedGridIndex = Convert.ToInt32(e.Key);

			if (PrivateCacheData.ContainsKey(selectedIndexOnGrid))
				PrivateCacheData[selectedIndexOnGrid] = selectedGridIndex;
			else
				PrivateCacheData.Add(selectedIndexOnGrid, selectedGridIndex);
		}

		void FilterTransactionsPerPageForUpdate()
		{
			GetPagesNumberFromCache();

			int startIndex = (pageNumber * pageSize) - (pageSize - 1);
			int endIndex = pageNumber * pageSize;
			if (endIndex > numberofRecordsFound)
				endIndex = numberofRecordsFound;

			System.Text.StringBuilder keys = new System.Text.StringBuilder();

			if (batchTransaction.Count == 1 && startIndex == endIndex)
				keys.Append(batchTransactionDT.Rows[0]["BatchTransactionKey"]+ ",");
			else
			{
				for (int iC = startIndex - 1; iC < endIndex; iC++)
				{
					keys.Append(batchTransactionDT.Rows[iC]["BatchTransactionKey"] + ",");
				}
			}

			if (keys.Length == 0)
				batchTransaction.RowFilter = "BatchTransactionKey = -1";
			else
				batchTransaction.RowFilter = "BatchTransactionKey in (" + keys.ToString(0, keys.Length - 1) + ")";

			BatchTransactionAddToPrivateCache();

			int RecalcTotalPages = 1;

			if (batchTransaction.Count > 0)
			{
				RecalcTotalPages = (int)Math.Round((numberofRecordsFound / pageSize) + 0.5, MidpointRounding.AwayFromZero);
			}
			totalPages = RecalcTotalPages;
		}

		private void BatchTransactionAddToPrivateCache()
		{
			// Add these filtered trans to the Private Cache    
			if (PrivateCacheData.ContainsKey(BatchTransactionData))
				PrivateCacheData[BatchTransactionData] = batchTransaction;
			else
				PrivateCacheData.Add(BatchTransactionData, batchTransaction);
		}

		void _view_OnRemoveButtonClicked(object sender, EventArgs e)
		{
			bulkBatchKey = bulkBatch[batchNumber].Key;

			GetDataFromPrivateCache();
			int batchTransactionStatusKey = 0;

			// This the BatchTransactionKey of the selected Index on the Grid
			int selectedBatchTransactionKey = Convert.ToInt32(batchTransaction[selectedGridIndex]["BatchTransactionKey"]);

			for (int i = 0; i < batchTransactionDT.Rows.Count; i++)
			{
				if (Convert.ToInt32(batchTransactionDT.Rows[i]["batchTransactionStatusKey"]) != (int)BatchTransactionStatuses.Posted && Convert.ToInt32(batchTransactionDT.Rows[i]["batchTransactionStatusKey"]) != (int)BatchTransactionStatuses.Deleted)
					amount += Convert.ToDouble(batchTransactionDT.Rows[i]["Amount"]); // need sum of all in DT that are not deleted or posted
			}

			if (bulkBatchKey > 0)
			{
				bulkBatchRec = bulkBatch[batchNumber];
				batchTransactionRecord = bulkBatchRepo.GetBatchTransactionByKey(selectedBatchTransactionKey);

				if (Convert.ToInt32(batchTransactionRecord.BatchTransactionStatus.Key) == (int)BatchTransactionStatuses.Deleted)
				{
					batchTransactionRecord.BatchTransactionStatus = lookups.BatchTransactionStatuses.ObjectDictionary[((int)BatchTransactionStatuses.Modified).ToString()];
					amount += Convert.ToDouble(batchTransactionRecord.Amount);
					batchTransactionStatusKey = (int)BatchTransactionStatuses.Modified;
				}
				else
				{
					batchTransactionRecord.BatchTransactionStatus = lookups.BatchTransactionStatuses.ObjectDictionary[((int)BatchTransactionStatuses.Deleted).ToString()]; 
					amount -= Convert.ToDouble(batchTransactionRecord.Amount);
					batchTransactionStatusKey = (int)BatchTransactionStatuses.Deleted;
				}

				if (_view.EffectiveDate.HasValue)
				{
					batchTransactionRecord.EffectiveDate = _view.EffectiveDate.Value;
					bulkBatchRec.EffectiveDate = _view.EffectiveDate.Value;
				}

				batchTransactionRecord.UserID = _view.CurrentPrincipal.Identity.Name;
				bulkBatchRec.Description = bulkBatchRec.Key + " - " + bulkBatchRec.EffectiveDate.ToString(SAHL.Common.Constants.DateFormat) + " - " + _view.CurrentPrincipal.Identity.Name + " - " + amount;

			}
			TransactionScope txn = new TransactionScope();
			try
			{
				bulkBatchRepo.SaveBulkBatch(bulkBatchRec);
				if (batchTransactionRecord != null)
					bulkBatchRepo.SaveBatchTransaction(batchTransactionRecord);
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
				batchTransaction[selectedGridIndex].Row["BatchTransactionStatusKey"] = lookups.BatchTransactionStatuses[batchTransactionStatusKey].Key;

				BatchTransactionAddToPrivateCache();

				RepopulateBatchBulk();
			}

		}

		void _view_OnPostButtonClicked(object sender, EventArgs e)
		{

			IAccount acc;
			// DataTable batchLoanTransactionsDT = null;
			accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
			double amountPosted = 0;
			bool successfulPost = false;
			IBatchTransaction btRec ;

			batchTransactionDT = bulkBatchRepo.GetUpdateBulkTransactionBatchByBulkBatchKey(bulkBatch[batchNumber].Key);
			int transactionTypeNumber = Convert.ToInt32(bulkBatch[batchNumber].BatchTransactions[0].TransactionTypeNumber);

			string errMsg = "Batch Number " + bulkBatch[batchNumber].Key + " has completed with errors";
			bool errorOccurred = false;

			TransactionScope txn = new TransactionScope();
			try
			{
				IBulkBatch bB = bulkBatchRepo.GetBulkBatchByKey(bulkBatch[batchNumber].Key);

				IRuleService svc = ServiceFactory.GetService<IRuleService>();
				int batchExists = svc.ExecuteRule(_view.Messages, "BulkBatchAlreadyPosted", bB);
				if (batchExists == 0)
					return;

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
							double ReadvanceComparison = mlRepo.GetReadvanceComparisonAmount(Convert.ToInt32(batchTransactionDT.Rows[i]["AccountKey"]));
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
								if (hocAcc != null)
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
										bulkBatchRepo.PostTransaction(Convert.ToInt32(batchTransactionDT.Rows[i]["BatchTransactionKey"]), Convert.ToInt32(batchTransactionDT.Rows[i]["AccountKey"]), transactionTypeNumber, Convert.ToDateTime(btRec.EffectiveDate), float.Parse((batchTransactionDT.Rows[i]["Amount"].ToString())), btRec.Reference, _view.CurrentPrincipal.Identity.Name);
									}
						}// End of Readvances
						else // Not a Readvance
						{ // Try posting valid  Transaction
							btRec = bulkBatchRepo.GetBatchTransactionByKey(Convert.ToInt32(batchTransactionDT.Rows[i]["BatchTransactionKey"]));
							bulkBatchRepo.PostTransaction(Convert.ToInt32(batchTransactionDT.Rows[i]["BatchTransactionKey"]), Convert.ToInt32(batchTransactionDT.Rows[i]["AccountKey"]), transactionTypeNumber, Convert.ToDateTime(btRec.EffectiveDate), float.Parse((batchTransactionDT.Rows[i]["Amount"].ToString())), btRec.Reference, _view.CurrentPrincipal.Identity.Name);
							if (!_view.IsValid)
							{
								errorOccurred = true;
							}
							else // Get the successfully posted tran and sum the amounts
							{
								IReadOnlyEventList<IFinancialTransaction> list = bulkBatchRepo.GetBatchLoanTransactions(Convert.ToInt32(batchTransactionDT.Rows[i]["AccountKey"]), transactionTypeNumber, Convert.ToDateTime(btRec.EffectiveDate), bulkBatch[batchNumber].BatchTransactions[0].Reference, _view.CurrentPrincipal.Identity.Name);

								for (int k = 0; k < list.Count; k++)
								{
									amountPosted += list[k].Amount;
									successfulPost = true;
								}

								//batchLoanTransactionsDT = bulkBatchRepo.GetBatchLoanTransactions(Convert.ToInt32(batchTransactionDT.Rows[i]["AccountKey"]), transactionTypeNumber, Convert.ToDateTime(_view.GetEffectiveDateForUpdate), bulkBatch[batchNumber].BatchTransactions[0].Reference.ToString(), _view.GetLoggedOnUser.Identity.Name.ToString());
								//if (batchLoanTransactionsDT != null)
								//{
								//    amountPosted += Convert.ToDouble(batchTransactionDT.Rows[i]["Amount"]);
								//    successfulPost = true;
								//}
							}
						}
					}
				}
				// Update the amount with successfully posted trans and update the status
				if (successfulPost)
				{
					bulkBatch[batchNumber].BulkBatchStatus = lookups.BulkBatchStatuses[2]; // successful
					bulkBatch[batchNumber].Description = bulkBatch[batchNumber].Key + " - " + bulkBatch[batchNumber].EffectiveDate.ToString(SAHL.Common.Constants.DateFormat) + " - " + _view.CurrentPrincipal.Identity.Name + " - " + amountPosted;
					bulkBatchRepo.SaveBulkBatch(bulkBatch[batchNumber]);

				}
				else // else mark the batch as failed
				{
					bulkBatch[batchNumber].BulkBatchStatus = lookups.BulkBatchStatuses[1]; // Failed
				}
				bulkBatchRepo.SaveBulkBatch(bulkBatch[batchNumber]);
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
				_view.SetBatchPostErrorMessage(errMsg);
			else
				Navigator.Navigate("BulkTransactionBatch");

		}

		void _view_OnCancelButtonClicked(object sender, EventArgs e)
		{
			Navigator.Navigate("BulkTransactionBatch");
		}

		private void RepopulateBatchBulk()
		{
			if (batchStatusKey >= 0)
			{
				bulkBatch = bulkBatchRepo.GetBulkBatchByBulkBatchTypeAndStatusKey(1, batchStatusKey);

				if (PrivateCacheData.ContainsKey(BatchStatusSelectedItem))
					PrivateCacheData[BatchStatusSelectedItem] = bulkBatch;
				else
					PrivateCacheData.Add(BatchStatusSelectedItem, bulkBatch);
			}
			else
				if (PrivateCacheData.ContainsKey(BatchStatusSelectedItem))
					PrivateCacheData[BatchStatusSelectedItem] = null;

			if (bulkBatch.Count > 0)
				_view.BindBatchNumber(bulkBatch);
		}
	}
}
