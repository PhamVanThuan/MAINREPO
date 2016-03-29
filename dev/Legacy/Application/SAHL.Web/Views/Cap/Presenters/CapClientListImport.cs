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
using System.IO;
using System.Security.Principal;

namespace SAHL.Web.Views.Cap.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class CapClientListImport : CapClientListBase
    {

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CapClientListImport(ICAPClientList view, SAHLCommonBaseController controller)
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

            _view.OnImportButtonClicked += new EventHandler(_view_OnImportButtonClicked);

            _capRepo = RepositoryFactory.GetRepository<ICapRepository>();

            _view.ArrearRowVisible = false ;
            _view.SPVRowVisible = false;
            _view.FileNameRowVisible = true;
            _view.DateExcludeRowVisible = false;
            _view.ExtractButtonVisible = false;
            _view.ImportButtonVisible = true;

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
                    BindOfferDateDropDown(DateTime.ParseExact(_view.SelectedCapResetConfigDate, SAHL.Common.Constants.DateFormat, System.Globalization.CultureInfo.InvariantCulture));
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
        void _view_OnImportButtonClicked(object sender, EventArgs e)
        {
            string targetPath = null;
            ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            IBulkBatchRepository batchRepo = RepositoryFactory.GetRepository<IBulkBatchRepository>();
            IBulkBatch bulkBatch = batchRepo.GetEmptyBulkBatch();
            bulkBatch.BulkBatchStatus = lookupRepo.BulkBatchStatuses.ObjectDictionary[Convert.ToInt32(BulkBatchStatuses.ReadyforProcessing).ToString()];
            bulkBatch.Description = "Cap Import Client List";
            bulkBatch.BulkBatchType = lookupRepo.BulkBatchTypes.ObjectDictionary[Convert.ToInt32(BulkBatchTypes.CapImportClientList).ToString()];
            if (_view.SelectedCapType != -1)
                bulkBatch.IdentifierReferenceKey = _view.SelectedCapType;
            bulkBatch.EffectiveDate = DateTime.Today;
            bulkBatch.FileName = _view.FileNameValue;
            bulkBatch.UserID = _view.CurrentPrincipal.Identity.Name;

            if (_view.SelectedCapType > 0)
            {
                IBulkBatchParameter bpRow1 = batchRepo.GetEmptyBulkBatchParameter();
                bpRow1.ParameterName = BulkBatchParameterNames.CapTypeConfigurationKey.ToString(); ;
                bpRow1.ParameterValue = _view.SelectedCapType.ToString();
                bpRow1.BulkBatch = bulkBatch;
                bulkBatch.BulkBatchParameters.Add(_view.Messages, bpRow1);
            }

            if (!String.IsNullOrEmpty(_view.FileNameValue))
            {
                IBulkBatchParameter bpRow2 = batchRepo.GetEmptyBulkBatchParameter();
                bpRow2.ParameterName = BulkBatchParameterNames.FileName.ToString(); ;
                bpRow2.ParameterValue = _view.FileNameValue;
                bpRow2.BulkBatch = bulkBatch;
                bulkBatch.BulkBatchParameters.Add(_view.Messages, bpRow2);
            }

            TransactionScope txn = new TransactionScope();
            try
            {
                // try and copy the file up
                if (_view.File != null)
                {
                    targetPath = Path.Combine(bulkBatch.BulkBatchType.FilePath, _view.FileNameValue);

                    // copy the file - we need to use impersonation otherwise we get trust issues with Kerberos
                    WindowsImpersonationContext wic = null;
                    ISecurityService securityService = ServiceFactory.GetService<ISecurityService>();
                    wic = securityService.BeginImpersonation();

                    try
                    {
                        _view.File.SaveAs(targetPath);
                    }
                    finally
                    {
                        securityService.EndImpersonation(wic);
                    }
                }
                batchRepo.SaveBulkBatch(bulkBatch);

                string importJobName = lookupRepo.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.Batch.CapImportJob].ControlText;
                if (!string.IsNullOrEmpty(importJobName))
                {
                    ISqlAgentService sqlService = ServiceFactory.GetService<ISqlAgentService>();
                    sqlService.StartSQLServerAgentJob(importJobName);
                }

            }
            catch (Exception)
            {
                txn.VoteRollBack();

                if (!String.IsNullOrEmpty(targetPath) && File.Exists(targetPath))
                    File.Delete(targetPath);

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
            this.GlobalCacheData.Add(ViewConstants.BulkBatchTypeKey, BulkBatchTypes.CapImportClientList, new List<ICacheObjectLifeTime>());
            Navigator.Navigate("BatchProgress");
            
        }

    }
}

