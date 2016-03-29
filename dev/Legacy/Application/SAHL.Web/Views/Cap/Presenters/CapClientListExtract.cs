using System;
using System.Data;
using SAHL.Web.Views.Cap.Interfaces;
using SAHL.Common.Web.UI;
using System.Collections.Generic;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.CacheData;

namespace SAHL.Web.Views.Cap.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class CapClientListExtract : CapClientListBase
    {

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CapClientListExtract(ICAPClientList view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!View.ShouldRunPage) return;

            _view.OnExtractButtonClicked += new EventHandler(_view_OnExtractButtonClicked);

            _capRepo = RepositoryFactory.GetRepository<ICapRepository>();

            _view.ArrearRowVisible = true;
            _view.SPVRowVisible = true;
            _view.FileNameRowVisible = false;
            _view.DateExcludeRowVisible = true;
            _view.ExtractButtonVisible = true;
            _view.ImportButtonVisible = false;

            _view.BindOperatorDropDown();

            BindResetDateDropDown();
            
            _view.DateExcludeValue = DateTime.Now;

            if (!_view.IsPostBack )
            {
                if (_resetDates.Rows.Count > 0)
                {
                    BindOfferDateDropDown(Convert.ToDateTime(_resetDates.Rows[0]["ResetDate"]));
                    BindSPVList(Convert.ToDateTime(_resetDates.Rows[0]["ResetDate"]));
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(_view.SelectedCapResetConfigDate))
                {
                    //DateTime resetDate = Convert.ToDateTime(_view.SelectedCapResetConfigDate);
                    DateTime resetDate = DateTime.ParseExact(_view.SelectedCapResetConfigDate, SAHL.Common.Constants.DateFormat, System.Globalization.CultureInfo.InvariantCulture);
                    BindOfferDateDropDown(resetDate);
                    BindSPVList(resetDate);
                }

                if (_view.SelectedCapType != -1)
                    BindGrid(_view.SelectedCapType);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="resetDate"></param>
        private void BindSPVList(DateTime  resetDate)
        {
            int resetConfigKey = -1;
            if (_resetDates.Rows.Count > 0)
            {
                for (int i = 0; i < _resetDates.Rows.Count; i++)
                {
                    if (Convert.ToDateTime(_resetDates.Rows[i]["ResetDate"]) == resetDate)
                        resetConfigKey = Convert.ToInt32(_resetDates.Rows[i]["ResetConfigurationKey"]);
                }
            }

            ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Key", typeof(string)));
            dt.Columns.Add(new DataColumn("Value", typeof(string)));

            for (int i = 0; i < lookupRepo.SPVList.Count; i++)
            {
                if (!lookupRepo.SPVList[i].ResetConfigurationKey.HasValue )
                {
                    if (lookupRepo.SPVList[i].ResetConfigurationKey != resetConfigKey)
                        continue;
                }

                if (lookupRepo.SPVList[i].Key > (int)SPVs.MBT14TRUST)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Key"] = lookupRepo.SPVList[i].Key;
                    newRow["Value"] = lookupRepo.SPVList[i].Key.ToString() + " " + lookupRepo.SPVList[i].Description;
                    dt.Rows.Add(newRow);
                }
            }

            _view.BindSPVList(dt);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnExtractButtonClicked(object sender, EventArgs e)
        {
            ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            IBulkBatchRepository batchRepo = RepositoryFactory.GetRepository<IBulkBatchRepository>();

            IBulkBatch bulkBatch = batchRepo.GetEmptyBulkBatch();
            bulkBatch.BulkBatchStatus =  lookupRepo.BulkBatchStatuses.ObjectDictionary[Convert.ToInt32(BulkBatchStatuses.ReadyforProcessing).ToString()];
            bulkBatch.Description = "Cap Extract Client List";
            bulkBatch.BulkBatchType = lookupRepo.BulkBatchTypes.ObjectDictionary[Convert.ToInt32(BulkBatchTypes.CapExtractClientList).ToString()];
            if (_view.SelectedCapType != -1)
                bulkBatch.IdentifierReferenceKey = _view.SelectedCapType;
            bulkBatch.EffectiveDate = DateTime.Today;
            bulkBatch.UserID = _view.CurrentPrincipal.Identity.Name;

            IBulkBatchParameter bpRow1 = batchRepo.GetEmptyBulkBatchParameter();
            bpRow1.ParameterName = BulkBatchParameterNames.CapTypeConfigurationKey.ToString(); ;
            bpRow1.ParameterValue = _view.SelectedCapType.ToString();
            bpRow1.BulkBatch = bulkBatch;
            bulkBatch.BulkBatchParameters.Add(_view.Messages, bpRow1);

            if (_view.LoanArrearBalanceValue.HasValue)
            {
                IBulkBatchParameter bpRow2 = batchRepo.GetEmptyBulkBatchParameter();
                bpRow2.ParameterName = BulkBatchParameterNames.ArrearBalance.ToString();
                bpRow2.ParameterValue = _view.LoanArrearBalanceValue.Value.ToString();
                bpRow2.BulkBatch = bulkBatch;
                bulkBatch.BulkBatchParameters.Add(_view.Messages, bpRow2);
            }

            IBulkBatchParameter bpRow3 = batchRepo.GetEmptyBulkBatchParameter();
            bpRow3.ParameterName = BulkBatchParameterNames.MathematicalOperator.ToString();
            bpRow3.ParameterValue = _view.SelectedOperatorValue.ToString();
            bpRow3.BulkBatch = bulkBatch;
            bulkBatch.BulkBatchParameters.Add(_view.Messages, bpRow3);

            if (_view.DateExcludeValue.HasValue)
            {
                IBulkBatchParameter bpRow4 = batchRepo.GetEmptyBulkBatchParameter();
                bpRow4.ParameterName = BulkBatchParameterNames.ExcludeOffersAfterDate.ToString();
                bpRow4.ParameterValue = _view.DateExcludeValue.Value.ToString("yyyy/MM/dd") + " 12:00:00 AM"; ;
                bpRow4.BulkBatch = bulkBatch;
                bulkBatch.BulkBatchParameters.Add(_view.Messages, bpRow4);
            }

            if (_view.SPVList.Count > 0)
            {
                IBulkBatchParameter bpRow5 = batchRepo.GetEmptyBulkBatchParameter();
                bpRow5.ParameterName = BulkBatchParameterNames.SPV.ToString(); ;
                bpRow5.ParameterValue = String.Join(",", _view.SPVList.ToArray());
                bpRow5.BulkBatch = bulkBatch;
                bulkBatch.BulkBatchParameters.Add(_view.Messages, bpRow5);
            }

            TransactionScope txn = new TransactionScope();
            try
            {
                batchRepo.SaveBulkBatch(bulkBatch);

                string extractJobName = lookupRepo.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.Batch.CapExtractJob].ControlText;
                if (!string.IsNullOrEmpty(extractJobName))
                {
                    ISqlAgentService sqlService = ServiceFactory.GetService<ISqlAgentService>();
                    sqlService.StartSQLServerAgentJob(extractJobName);
                }

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

            // set the batch type global parameter and navigate
            this.GlobalCacheData.Add(ViewConstants.BulkBatchTypeKey, BulkBatchTypes.CapExtractClientList, new List<ICacheObjectLifeTime>());
            Navigator.Navigate("BatchProgress");
           
        }       
    }
}
