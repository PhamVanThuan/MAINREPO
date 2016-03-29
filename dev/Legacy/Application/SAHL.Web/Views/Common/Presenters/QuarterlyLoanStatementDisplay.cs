using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.Common.DomainMessages;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.CacheData;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class QuarterlyLoanStatementDisplay : SAHLCommonBasePresenter<IQuarterlyLoanStatement>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public QuarterlyLoanStatementDisplay(IQuarterlyLoanStatement view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);

            //ICommonRepository commonRepo = RepositoryFactory.GetRepository<ICommonRepository>();
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

            IList<IOriginationSource> osList = new List<IOriginationSource>();
            osList.Add(appRepo.GetOriginationSource(OriginationSources.SAHomeLoans));
            osList.Add(appRepo.GetOriginationSource(OriginationSources.RCS));
            _view.BindOriginationSource(osList);

            ICapRepository capRepo = RepositoryFactory.GetRepository<ICapRepository>();
            IList<IResetConfiguration> resetList = capRepo.GetResetConfigurations();
            _view.BindResetConfiguration(resetList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            IBulkBatchRepository batchRepo = RepositoryFactory.GetRepository<IBulkBatchRepository>();
            IBulkBatch bulkBatch = batchRepo.GetEmptyBulkBatch();
            bulkBatch.BulkBatchStatus = lookupRepo.BulkBatchStatuses.ObjectDictionary[Convert.ToInt32(BulkBatchStatuses.ReadyforProcessing).ToString()];
            bulkBatch.Description = lookupRepo.BulkBatchTypes.ObjectDictionary[Convert.ToInt32(BulkBatchTypes.QuarterlyLoanStatements).ToString()].Description;
            bulkBatch.BulkBatchType = lookupRepo.BulkBatchTypes.ObjectDictionary[Convert.ToInt32(BulkBatchTypes.QuarterlyLoanStatements).ToString()];
            bulkBatch.EffectiveDate = DateTime.Today;
            bulkBatch.UserID = _view.CurrentPrincipal.Identity.Name;

            if (_view.SelectedResetConfiguration != -1)
            {
                IBulkBatchParameter bpRow = batchRepo.GetEmptyBulkBatchParameter();
                bpRow.ParameterName = "ResetConfigurationKey";
                bpRow.ParameterValue = _view.SelectedResetConfiguration.ToString();
                bpRow.BulkBatch = bulkBatch;
                bulkBatch.BulkBatchParameters.Add(_view.Messages, bpRow);
            }
            else
            {
                _view.Messages.Add(new Error("Please select a reset configuration", ""));
            }

            if (_view.SelectedOriginationSource != -1)
            {
                IBulkBatchParameter bpRow = batchRepo.GetEmptyBulkBatchParameter();
                bpRow.ParameterName = "OriginationSourceKey";
                bpRow.ParameterValue = _view.SelectedOriginationSource.ToString();
                bpRow.BulkBatch = bulkBatch;
                bulkBatch.BulkBatchParameters.Add(_view.Messages, bpRow);
            }
            else
            {
                _view.Messages.Add(new Error("Please select an Origination Source", ""));
            }

            if (!string.IsNullOrEmpty(_view.SelectedMailingAddresses))
            {
                IBulkBatchParameter bpRow = batchRepo.GetEmptyBulkBatchParameter();
                bpRow.ParameterName = "MailAddress";
                bpRow.ParameterValue = _view.SelectedMailingAddresses;
                bpRow.BulkBatch = bulkBatch;
                bulkBatch.BulkBatchParameters.Add(_view.Messages, bpRow);
            }
            else
            {
                _view.Messages.Add(new Error("Please enter at least one mailing address", ""));
            }

            if (!string.IsNullOrEmpty(_view.SelectedSamples))
            {
                IBulkBatchParameter bpRow = batchRepo.GetEmptyBulkBatchParameter();
                bpRow.ParameterName = "SampleList";
                bpRow.ParameterValue = _view.SelectedSamples;
                bpRow.BulkBatch = bulkBatch;
                bulkBatch.BulkBatchParameters.Add(_view.Messages, bpRow);
            }

            if (_view.SelectedStatementMonths != -1)
            {
                IBulkBatchParameter bpRow = batchRepo.GetEmptyBulkBatchParameter();
                bpRow.ParameterName = "StatementMonths";
                bpRow.ParameterValue = _view.SelectedStatementMonths.ToString();
                bpRow.BulkBatch = bulkBatch;
                bulkBatch.BulkBatchParameters.Add(_view.Messages, bpRow);
            }
            else
            {
                _view.Messages.Add(new Error("Please enter a statement period", ""));
            }

            if (_view.Messages.Count > 0)
                return;
            else
            {
                TransactionScope txn = new TransactionScope();
                try
                {
                    batchRepo.SaveBulkBatch(bulkBatch);
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

                if (_view.Messages.Count == 0)
                {
                    string extractJobName = lookupRepo.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.Batch.LoanStatementJob].ControlText;
                    if (!string.IsNullOrEmpty(extractJobName))
                    {
                        ISqlAgentService sqlService = ServiceFactory.GetService<ISqlAgentService>();
                        sqlService.StartSQLServerAgentJob(extractJobName);
                    }
                    Navigator.Navigate("Extract");
                }
            }
        }

    }
}
