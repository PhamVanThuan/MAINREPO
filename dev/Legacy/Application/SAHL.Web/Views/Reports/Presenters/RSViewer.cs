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
using SAHL.Web.Views.Reports.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Factories;
using System.Collections.Generic;
using SAHL.Common.CacheData;
using SAHL.Common.Authentication;
using System.Web.Services.Protocols;
using SAHL.Common;
using System.Data.SqlClient;
using SAHL.Common.Globals;
using SAHL.Common.Collections;
using System.Collections;
using System.IO;

namespace SAHL.Web.Views.Reports.Presenters
{
    public class RSViewer : SAHLCommonBasePresenter<IRSViewer>
    {

        private IList<IReportParameter> _ReportParameters;
        private IEventList<IReportStatement> _ReportStatements;
        private IReportRepository _ReportRepo;
        private SAHL.Common.CacheData.SAHLPrincipalCache _spc;
        private List<ICacheObjectLifeTime> _lifeTimes = new List<ICacheObjectLifeTime>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public RSViewer(IRSViewer view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _ReportRepo = RepositoryFactory.GetRepository<IReportRepository>();
            _spc = SAHL.Common.CacheData.SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            if (_view.IsMenuPostBack)
                GlobalCacheData.Clear();

            bool reportGroupChanged = false;

            if (PrivateCacheData.ContainsKey(ViewConstants.ReportGroupIndex))
            {
                if (PrivateCacheData[ViewConstants.ReportGroupIndex].ToString() != _view.SelectedReportGroupIndex.ToString())
                {
                    reportGroupChanged = true;
                    _view.tblReportsSelectedIndex = -1;
                    _view.ViewButtonEnable = false;
                    _view.pnlParametersShow = false;
                    PrivateCacheData.Remove(ViewConstants.ReportGroupIndex);
                    PrivateCacheData.Add(ViewConstants.ReportGroupIndex, _view.SelectedReportGroupIndex);
                }
            }
            else
            {
                PrivateCacheData.Remove(ViewConstants.ReportGroupIndex);
                PrivateCacheData.Add(ViewConstants.ReportGroupIndex, _view.SelectedReportGroupIndex);
            }
            
    
            int index = _view.tblReportsSelectedIndex;
            if (GlobalCacheData.ContainsKey(ViewConstants.SelReportIndex) && reportGroupChanged == false)
            {
                index = int.Parse(GlobalCacheData[ViewConstants.SelReportIndex].ToString());
                if (GlobalCacheData.ContainsKey(ViewConstants.ReportGroup))
                    _view.tblReportsSelectedIndex = index;
            }

            _view.grdDataReportShow = false;
            _view.ReportsGridShow = true;

            _view.OntblReportsSelectedIndexChanged += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OntblReportsSelectedIndexChanged);
            _view.OnReportsGroupChanged += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnReportGroupsChanged);

            _view.onViewButtonClicked += new EventHandler<ReportParametersEventArgs>(_view_onViewButtonClicked);

            ILookupRepository lookups = RepositoryFactory.GetRepository<ILookupRepository>();

            // only load the report groups that the user has access to
            IEventList<IReportGroup> reportGroups = null;
            if (GlobalCacheData.ContainsKey(ViewConstants.ReportGroupData))
                reportGroups = (EventList<IReportGroup>)GlobalCacheData[ViewConstants.ReportGroupData];
            else
            {
                reportGroups = new EventList<IReportGroup>();
                foreach (IReportGroup reportGroup in lookups.ReportGroups)
                {
                    foreach (int featureKey in _spc.FeatureKeys)
                    {
                        if (reportGroup.Feature.Key == featureKey)
                        {
                            reportGroups.Add(_view.Messages, reportGroup);
                            break;
                        }
                    }
                }
                reportGroups.Sort();
                GlobalCacheData.Add(ViewConstants.ReportGroupData, reportGroups, _lifeTimes);
            }

            _view.BindReportGroup(reportGroups);

            // Got to always rebind on Initialise as Data is lost after post back
            if (GlobalCacheData.ContainsKey(ViewConstants.ReportStatementData))
                _ReportStatements = GlobalCacheData[ViewConstants.ReportStatementData] as IEventList<IReportStatement>;

            if (_ReportStatements != null)
            {
                _ReportStatements.Sort();

                _view.BindReportStatement(_ReportStatements);

                if (GlobalCacheData.ContainsKey(ViewConstants.ReportParameterData))
                    _ReportParameters = GlobalCacheData[ViewConstants.ReportParameterData] as IList<IReportParameter>;

                if (_ReportParameters != null && _ReportParameters.Count > 0) // only do this if post back was not caused by grid selection click
                    _view.BindReportParameterList(_ReportParameters, _ReportStatements);
            }

            if (GlobalCacheData.ContainsKey(ViewConstants.ReportGroup))
            {
                _view.SelectedReportGroup = GlobalCacheData[ViewConstants.ReportGroup].ToString();

                PrivateCacheData.Remove(ViewConstants.ReportGroupIndex);
                PrivateCacheData.Add(ViewConstants.ReportGroupIndex, int.Parse(GlobalCacheData[ViewConstants.ReportGroup].ToString()));

                ReportGroupsChanged(null, null);

                IReportStatement rs = GlobalCacheData[ViewConstants.ReportStatement] as IReportStatement;
                _view.SelectedReportName = rs.Key.ToString();

                IList<string> values = null;

                values = GlobalCacheData[ViewConstants.OriginalReportParameterValues] as IList<string>;

                _view.OriginalReportParameters = values;

                RemoveRequiredGlobalCacheItems();
            }

            if (_ReportStatements != null && _ReportStatements.Count > 0)
            {
                int sri = index;

                if (GlobalCacheData.ContainsKey(ViewConstants.SelReportIndex))
                    BuildReportParams(sri);
                else
                {
                    if (reportGroupChanged)
                    {
                        _view.tblReportsSelectedIndex = -1;
                        reportGroupChanged = false;
                    }
                }
            }
        }

        private void RemoveRequiredGlobalCacheItems()
        {
            GlobalCacheData.Remove(ViewConstants.ReportGroup);
            GlobalCacheData.Remove(ViewConstants.ReportParameters);
            GlobalCacheData.Remove(ViewConstants.ReportStatement);
            GlobalCacheData.Remove(ViewConstants.OriginalReportParameterValues);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_OnReportGroupsChanged(object sender, KeyChangedEventArgs e)
        {
            ReportGroupsChanged(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        private void ReportGroupsChanged(object sender, KeyChangedEventArgs e)
        {
            IReadOnlyEventList<IReportStatement> reportStatements = null;

            if (e != null)
            {
                int key = -1;
                if (int.TryParse(e.Key.ToString(), out key))
                    reportStatements = _ReportRepo.GetReportStatementByReportGroupKey(key);
            }
            else
            {
                reportStatements = _ReportRepo.GetReportStatementByReportGroupKey(Convert.ToInt32(GlobalCacheData[ViewConstants.ReportGroup].ToString()));
            }

            // only load the reports that the use has access to
            _ReportStatements = new EventList<IReportStatement>();
            if (reportStatements != null)
            {
                foreach (IReportStatement reportStatement in reportStatements)
                {
                    if (reportStatement.Feature != null)
                    {
                        foreach (int featureKey in _spc.FeatureKeys)
                        {
                            if (reportStatement.Feature.Key == featureKey)
                            {
                                _ReportStatements.Add(_view.Messages, reportStatement);
                                break;
                            }
                        }
                    }
                }
            }
            _ReportStatements.Sort();

            GlobalCacheData.Remove(ViewConstants.ReportStatementData);
            GlobalCacheData.Add(ViewConstants.ReportStatementData, _ReportStatements, _lifeTimes);

            if (_ReportStatements.Count > 0)
                _view.BindReportStatement(_ReportStatements);
        }

        /// <summary>
        /// Handles the event fired by the view when the Report Grid Selected Index is changed
        /// Populate the Report Paramaters controls based on the selected row in the Report Grid
        /// </summary>

        void _view_OntblReportsSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            GlobalCacheData.Remove(ViewConstants.SelReportIndex);
            GlobalCacheData.Add(ViewConstants.SelReportIndex, e.Key, _lifeTimes);

            if (_ReportParameters != null)
                _ReportParameters.Clear();

            BuildReportParams((int)e.Key);
        }

        /// <summary>
        /// 
        /// </summary>
        void BuildReportParams(int gridIndex)
        {
            if (gridIndex == -1)
            {
                _view.tblReportsSelectedIndex = -1;
                return;
            }

            if (_ReportStatements != null && _ReportStatements.Count > 0 
                && _view.SelectedReportType != (int)SAHL.Common.Globals.ReportTypes.CubeReport
                && gridIndex <_ReportStatements.Count)
            {

                IReportStatement rs = _ReportRepo.GetReportStatementByKey(_ReportStatements[gridIndex].Key);
                List<IReportParameter> reportParameters = rs.ReportParameters;               

                _ReportParameters = new List<IReportParameter>();
                if (reportParameters != null)
                {
                    for (int i = 0; i < reportParameters.Count; i++)
                    {                      
                        _ReportParameters.Add(reportParameters[i]);
                    }
                }

                GlobalCacheData.Remove(ViewConstants.ReportParameterData);
                GlobalCacheData.Add(ViewConstants.ReportParameterData, _ReportParameters, _lifeTimes);

                if (_ReportParameters != null && _ReportParameters.Count > 0)
                    _view.BindReportParameterList(_ReportParameters, _ReportStatements);

                _view.BuildParameterControls(_ReportParameters, _ReportStatements);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_onViewButtonClicked(object sender, ReportParametersEventArgs e)
        {
            GlobalCacheData.Add(ViewConstants.ReportGroup, _view.SelectedReportGroup, _lifeTimes);
            GlobalCacheData.Add(ViewConstants.ReportParameters, e.Parameters, _lifeTimes);
            GlobalCacheData.Add(ViewConstants.ReportStatement, _view.SelectedReportStatement, _lifeTimes);

            GlobalCacheData.Add(ViewConstants.SelReportIndex, _view.tblReportsSelectedIndex, _lifeTimes);

            IList<string> originalParamValues = new List<string>();
            foreach (IReportParameter p in e.Parameters.Keys)
            {
                object param = e.Parameters[p];
                if (param != null)
                    originalParamValues.Add(param.ToString());
            }

            GlobalCacheData.Add(ViewConstants.OriginalReportParameterValues, originalParamValues, _lifeTimes);

            switch (_view.SelectedReportType)
            {
                case (int)SAHL.Common.Globals.ReportTypes.DataReport:
                    _view.Navigator.Navigate("ReportGrid");
                    break;
                case (int)SAHL.Common.Globals.ReportTypes.ReportingServicesReport:
                    _view.Navigator.Navigate("ReportingServices");
                    break;
                case (int)SAHL.Common.Globals.ReportTypes.CustomViewReport:
                    if (_view.SelectedReportName.Contains("InterestRateReviewReport"))
                        _view.Navigator.Navigate("InterestRateReview");
                    else if (_view.SelectedReportName.Contains("ReAdvanceRecommendation"))
                        _view.Navigator.Navigate("ReAdvanceRecommendation");
                    else
                        _view.Navigator.Navigate(_view.SelectedReportName);
                    break;
                case (int)SAHL.Common.Globals.ReportTypes.CubeReport:
                    if (_view.SelectedReportStatement != null)
                    {
                        GlobalCacheData.Remove(ViewConstants.ReportStatement);
                        GlobalCacheData.Add(ViewConstants.ReportStatement, _view.SelectedReportStatement, _lifeTimes);

                        _view.Navigator.Navigate("OlapViewer");
                    }
                    break;
                case (int)SAHL.Common.Globals.ReportTypes.StaticPDF:
                case (int)SAHL.Common.Globals.ReportTypes.PDFReport:
                    _view.Navigator.Navigate("PDFReportViewer");
                    break;
				case (int)SAHL.Common.Globals.ReportTypes.DevExPivot:
					_view.Navigator.Navigate("DevExPivotViewer");
					break;

                default: break;
            }
        }
    }
}
