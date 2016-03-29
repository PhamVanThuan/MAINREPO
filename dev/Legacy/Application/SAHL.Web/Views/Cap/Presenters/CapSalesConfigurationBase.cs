using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Cap.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

namespace SAHL.Web.Views.Cap.Presenters
{
    /// <summary>
    ///
    /// </summary>
    public class CapSalesConfigurationBase : SAHLCommonBasePresenter<ICapSalesConfiguration>
    {
        protected ICapRepository _capRepository;
        protected DataTable _resetDates;
        protected IList<ICapTypeConfigurationDetail> _capConfigDetailList;
        protected IList<ICapTypeConfiguration> _capConfigList;
        protected ILookupRepository _lookupRepo;

        #region Constructor

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CapSalesConfigurationBase(ICapSalesConfiguration view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        #endregion Constructor

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _capRepository = RepositoryFactory.GetRepository<ICapRepository>();
            _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
        }

        /// <summary>
        ///
        /// </summary>
        protected void BindResetDateDropDown()
        {
            _resetDates = _capRepository.GetCurrentCAPResetConfigDates();
            _view.BindResetDateDropDown(_resetDates);
        }

        protected static DateTime getDateFromDDMMYYYY(string date)
        {
            int d = int.Parse(date.Substring(0, 2));
            int m = int.Parse(date.Substring(3, 2));
            int y = int.Parse(date.Substring(6, 4));
            return new DateTime(y, m, d);
        }

        /// <summary>
        ///
        /// </summary>
        protected void BindGrid()
        {
            DateTime resetDate;
            if (!_view.IsPostBack)
            {
                if (_resetDates.Rows.Count > 0)
                {
                    resetDate = Convert.ToDateTime(_resetDates.Rows[0]["ResetDate"]);
                }
                else
                    return;
            }
            else
            {
                if (!String.IsNullOrEmpty(_view.SelectedCapResetConfigDate))
                    resetDate = DateTime.ParseExact(_view.SelectedCapResetConfigDate, SAHL.Common.Constants.DateFormat, System.Globalization.CultureInfo.InvariantCulture);
                else
                    return;
            }

            _capConfigList = _capRepository.GetCapTypeConfigByResetDate(resetDate);
            _capConfigDetailList = new List<ICapTypeConfigurationDetail>();
            for (int i = 0; i < _capConfigList.Count; i++)
            {
                for (int j = 0; j < _capConfigList[i].CapTypeConfigurationDetails.Count; j++)
                {
                    _capConfigDetailList.Add(_capConfigList[i].CapTypeConfigurationDetails[j]);
                }
            }

            _view.BindCapSalesGrid(_capConfigDetailList);
        }
    }
}