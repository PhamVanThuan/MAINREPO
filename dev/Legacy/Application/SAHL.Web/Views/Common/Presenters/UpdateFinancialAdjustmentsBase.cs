using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SAHL.Web.Views.Common.Presenters
{
    public class UpdateFinancialAdjustmentsBase : SAHLCommonBasePresenter<IFinancialAdjustments>
    {
        protected ICommonRepository _cRepo;
        protected IAccountRepository _accountRepository;
        protected IFinancialAdjustmentRepository _fadjRepo;
        protected ILookupRepository _lookups;
        protected IMortgageLoanRepository _mlRepo;
        protected IAccount _account;
        protected IList<IFinancialAdjustment> _financialAdjustments;
        protected const string SelectedFinancialAdjustment = "SelectedFinancialAdjustments";
        private List<ICacheObjectLifeTime> _lifeTimes;
        private IADUser _adUser;

        protected List<ICacheObjectLifeTime> LifeTimes
        {
            get
            {
                if (_lifeTimes == null)
                {
                    List<string> views = new List<string>();
                    views.Add("UpdateFinancialAdjustments");
                    _lifeTimes = new List<ICacheObjectLifeTime>();
                    _lifeTimes.Add(new SimplePageCacheObjectLifeTime(views));
                }
                return _lifeTimes;
            }
        }

        /// <summary>
        /// Gets the <see cref="IADUser"/> for the current principal.
        /// </summary>
        protected IADUser CurrentADUser
        {
            get
            {
                if (_adUser == null)
                {
                    ISecurityRepository secRepo = RepositoryFactory.GetRepository<ISecurityRepository>();
                    _adUser = secRepo.GetADUserByPrincipal(_view.CurrentPrincipal);
                }
                return _adUser;
            }
        }

        /// CBO Menu Node
        /// </summary>
        protected CBOMenuNode _node;

        /// <summary>
        /// Constructor for UpdateFinancialAdjustmentsBase
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public UpdateFinancialAdjustmentsBase(IFinancialAdjustments view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            _accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();
            _fadjRepo = RepositoryFactory.GetRepository<IFinancialAdjustmentRepository>();
            _mlRepo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
        }

        /// <summary>
        /// OnView Initialised event - retrieve data for use by presenters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.SetUpFinancialAdjustmentGrid();

            _fadjRepo = RepositoryFactory.GetRepository<IFinancialAdjustmentRepository>();
            _lookups = RepositoryFactory.GetRepository<ILookupRepository>();
            _cRepo = RepositoryFactory.GetRepository<ICommonRepository>();

            GetFinancialAdjustments(false);
        }

        protected void GetFinancialAdjustments(bool bIsPostback)
        {
            // get the account object
            _account = _accountRepository.GetAccountByKey(_node.GenericKey);

            _financialAdjustments = new List<IFinancialAdjustment>();

            //Get the rate override objects
            foreach (IFinancialService fs in _account.FinancialServices)
            {
                if (fs.AccountStatus.Key == (int)GeneralStatuses.Active)
                {
                    foreach (IFinancialAdjustment fa in fs.FinancialAdjustments)
                    {
                        if ((fa.FinancialAdjustmentTypeSource.Key == (int)FinancialAdjustmentTypeSources.DefendingCancellations ||
                            fa.FinancialAdjustmentTypeSource.Key == (int)FinancialAdjustmentTypeSources.DiscountedLinkrate))
                        {
                            _financialAdjustments.Add(fa);
                        }
                    }
                }
            }

            var showLabel = _financialAdjustments.Where(x => x.FinancialAdjustmentStatus.Key == (int)FinancialAdjustmentStatuses.Inactive &&
                                                             x.FromDate >= DateTime.Now.Date).Count() > 0;
            if (showLabel)
            {
                _view.InformationLabel = "Note: These financial adjustments will be set to active on the next day after their effective date.";
            }
            else
            {
                _view.InformationLabel = String.Empty;
            }
            DataTable dt = ConvertEventListToDataTable(_financialAdjustments, _account.Key);
            BindFinancialAdjustmentGrid(dt, bIsPostback);
        }

        private static DataTable ConvertEventListToDataTable(IList<IFinancialAdjustment> financialAdjustments, int accountKey)
        {
            DataTable dt = new DataTable();
            dt.TableName = accountKey.ToString();
            dt.Columns.Add(new DataColumn("FinancialAdjustmentKey", typeof(int)));
            dt.Columns.Add(new DataColumn("FinancialAdjustment", typeof(String)));
            dt.Columns.Add(new DataColumn("FinancialAdjustmentType", typeof(String)));
            dt.Columns.Add(new DataColumn("EffectiveDate", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("Term", typeof(int)));
            dt.Columns.Add(new DataColumn("Value", typeof(String)));
            dt.Columns.Add(new DataColumn("Status", typeof(String)));
            dt.Columns.Add(new DataColumn("CancellationDate", typeof(DateTime)));

            if (financialAdjustments != null)
            {
                foreach (var fadj in financialAdjustments)
                {
                    DataRow dr = dt.NewRow();
                    dr["FinancialAdjustmentKey"] = fadj.Key;
                    dr["FinancialAdjustment"] = fadj.FinancialAdjustmentSource.Description;
                    dr["FinancialAdjustmentType"] = fadj.FinancialAdjustmentType.Description;
                    dr["EffectiveDate"] = fadj.FromDate.Value;
                    dr["Term"] = fadj.Term;
                    dr["Value"] = fadj.ToString();
                    dr["Status"] = fadj.FinancialAdjustmentStatus.Description;
                    if(fadj.CancellationDate != null)
                    {
                        dr["CancellationDate"] = fadj.CancellationDate;
                    }
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        protected void BindFinancialAdjustmentGrid(DataTable dt, bool bIsPostback)
        {
            //I Really don't want to do this.
            //but the Devil made me.
            CheckForUpdatedFinancialAdjustments(dt);

            if (this.GlobalCacheData.ContainsKey("FinancialAdjustmentData"))
            {
                DataTable temp = this.GlobalCacheData["FinancialAdjustmentData"] as DataTable;
                if (temp.TableName == dt.TableName)
                {
                    dt = temp;
                    if (bIsPostback)
                    {
                        _view.BindFinancialAdjustmentGridPost(dt);
                    }
                    else
                    {
                        _view.BindFinancialAdjustmentGrid(dt);
                    }
                    return;
                }
            }

            if (bIsPostback)
            {
                _view.BindFinancialAdjustmentGridPost(dt);
            }
            else
            {
                _view.BindFinancialAdjustmentGrid(dt);
            }

            this.GlobalCacheData.Add("FinancialAdjustmentData", dt, LifeTimes);
        }

        protected void CheckForUpdatedFinancialAdjustments(DataTable dt)
        {
            if (this.GlobalCacheData.ContainsKey("FinancialAdjustmentData"))
            {
                DataTable temp = this.GlobalCacheData["FinancialAdjustmentData"] as DataTable;
                foreach (DataRow row in dt.Rows)
                {
                    if (row["CancellationDate"] != System.DBNull.Value)
                    {
                        foreach (DataRow tempRow in temp.Rows)
                        {
                            if (row["FinancialAdjustmentKey"].ToString() == tempRow["FinancialAdjustmentKey"].ToString())
                            {
                                if (tempRow["CancellationDate"] == System.DBNull.Value)
                                {
                                    this.GlobalCacheData.Remove("FinancialAdjustmentData");
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}