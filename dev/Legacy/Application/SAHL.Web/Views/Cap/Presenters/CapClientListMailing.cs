using System;
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
using System.Globalization;

namespace SAHL.Web.Views.Cap.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class CapClientListMailing : CapClientListBase
    {

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CapClientListMailing(ICAPClientList view, SAHLCommonBaseController controller)
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

            _view.ArrearRowVisible = false ;
            _view.SPVRowVisible = false;
            _view.FileNameRowVisible = false;
            _view.DateExcludeRowVisible = false;
            _view.ExtractButtonVisible = true;
            _view.ImportButtonVisible = false;

            BindResetDateDropDown();
            if (!_view.IsPostBack )
            {
                if (_resetDates.Rows.Count > 0)
                {
                    BindOfferDateDropDown(Convert.ToDateTime(_resetDates.Rows[0]["ResetDate"]));
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(_view.SelectedCapResetConfigDate))
                {
                    BindOfferDateDropDown(DateTime.Parse(_view.SelectedCapResetConfigDate, CultureInfo.GetCultureInfo(SAHL.Common.Web.UI.Controls.Constants.CultureGb)));
                }

                if (_view.SelectedCapType != -1)
                    BindGrid(_view.SelectedCapType);
            }
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
            bulkBatch.BulkBatchStatus = lookupRepo.BulkBatchStatuses.ObjectDictionary[Convert.ToInt32(BulkBatchStatuses.ReadyforProcessing).ToString()];
            bulkBatch.Description = "Cap Mailing House Extract";
            bulkBatch.BulkBatchType = lookupRepo.BulkBatchTypes.ObjectDictionary[Convert.ToInt32(BulkBatchTypes.CapMailingHouseExtract).ToString()];
            if (_view.SelectedCapType != -1)
                bulkBatch.IdentifierReferenceKey = _view.SelectedCapType;
            bulkBatch.EffectiveDate = DateTime.Today;
            bulkBatch.UserID = _view.CurrentPrincipal.Identity.Name;

            IBulkBatchParameter bpRow1 = batchRepo.GetEmptyBulkBatchParameter();
            bpRow1.ParameterName = "CapTypeConfigurationKey";
            bpRow1.ParameterValue = _view.SelectedCapType.ToString();
            bpRow1.BulkBatch = bulkBatch;
            bulkBatch.BulkBatchParameters.Add(_view.Messages, bpRow1);

            TransactionScope txn = new TransactionScope();
            try
            {
                batchRepo.SaveBulkBatch(bulkBatch);

                string extractJobName = lookupRepo.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.Batch.CapMailingExportJob].ControlText;
                if (!string.IsNullOrEmpty(extractJobName))
                {
                    ISqlAgentService sqlService = ServiceFactory.GetService<ISqlAgentService>();
                    sqlService.StartSQLServerAgentJob(extractJobName);
                }

                // set the batch type global parameter and navigate
                this.GlobalCacheData.Add(ViewConstants.BulkBatchTypeKey, BulkBatchTypes.CapMailingHouseExtract, new List<ICacheObjectLifeTime>());
                Navigator.Navigate("BatchProgress");
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

            
        }
    }
}

