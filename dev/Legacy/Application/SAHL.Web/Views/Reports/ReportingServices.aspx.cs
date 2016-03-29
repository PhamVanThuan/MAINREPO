using Microsoft.Reporting.WebForms;
using SAHL.Common.BusinessModel.Authentication;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Logging;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Reports.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.Web.UI.WebControls;

namespace SAHL.Web.Views.Reports
{
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "Dispose method will never be called on a web page")]
    public partial class ReportingServices : SAHLCommonBaseView, IReportingServicesReport
    {
        private ReportViewer _rV;
        private string _reportPath;
        private IDictionary<IReportParameter, object> _parameters;
        private Dictionary<string, string> _lstParameters = new Dictionary<string, string>();

        private void SAHLReport_ReportError(object sender, ReportErrorEventArgs e)
        {
            LogPlugin.Logger.LogErrorMessageWithException(System.Reflection.MethodBase.GetCurrentMethod().Name, string.Empty, e.Exception);
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            EnableViewState = true;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!ShouldRunPage) return;
            _rV = new ReportViewer();
            _rV.ID = "SAHLReport";
            _rV.Width = new Unit(100, UnitType.Percentage);
            _rV.BorderStyle = BorderStyle.Solid;
            _rV.BorderColor = Color.Red;
            _rV.BorderWidth = new Unit(2, UnitType.Pixel);
            _rV.ProcessingMode = ProcessingMode.Remote;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ShouldRunPage) return;
            _rV.ReportError += new ReportErrorEventHandler(SAHLReport_ReportError);

            ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

            //set the timeout for the report from the value in the control table.
            int timeout = lookupRepo.Controls.ObjectDictionary["ReportingServicesTimeout"].ControlNumeric.HasValue ? int.Parse(lookupRepo.Controls.ObjectDictionary["ReportingServicesTimeout"].ControlNumeric.Value.ToString()) : 120;
            if (timeout > -1)
            {
                // Multiply by 1000 to convert to milliseconds
                _rV.ServerReport.Timeout = timeout * 1000;
            }
            var scriptManager = System.Web.UI.ScriptManager.GetCurrent(Page);
            if (scriptManager != null)
            {
                scriptManager.AsyncPostBackTimeout = timeout;
            }

            Panel1.Controls.Add(_rV);

            try
            {
                if (IsPostBack) 
                {
                    return;
                }
                //Get the location of the SQL 2005 Reporting Server out of the appsettings in the web config
                //_rV.ServerReport.ReportServerUrl = new Uri(lookupRepo.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.Reporting.SAHLReportServerURL].ControlText);
                _rV.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["ReportServerURL"]);
                _rV.ServerReport.ReportServerCredentials = new ReportServerCredentials(Context);
                _rV.ShowParameterPrompts = false;
                _rV.PromptAreaCollapsed = true;
                _rV.ZoomMode = ZoomMode.Percent;
                _rV.ZoomPercent = 75;
                _rV.ShowZoomControl = true;
                _rV.ShowWaitControlCancelLink = false;

                foreach (ISqlReportParameter p in _parameters.Keys)
                {
                    object val = null;

                    //if(p != null && p.ValidValues.Count == 0)
                    //{
                    //    if (p.Required.HasValue && p.Required.Value == false)
                    //    {
                    //        continue;
                    //    }
                    //}

                    _parameters.TryGetValue(p, out val);
                    if (val != null)
                    {
                        _lstParameters.Add(p.ParameterName, val.ToString());
                    }
                    else
                    {
                        _lstParameters.Add(p.ParameterName, null);
                    }
                }

                if (_reportPath != null)
                {
                    if (!IsPostBack) {
                        _rV.ServerReport.ReportPath = _reportPath;
                    }

                    // get the paramemters

                    ReportParameterInfoCollection OutputParameterCollection = _rV.ServerReport.GetParameters();

                    ReportParameter[] InputParameterCollection = new ReportParameter[OutputParameterCollection.Count];

                    for (int iCnt = 0; iCnt < OutputParameterCollection.Count; ++iCnt)
                    {
                        //if (!OutputParameterCollection[iCnt].PromptUser)
                        //    continue;

                        InputParameterCollection[iCnt] = new ReportParameter();
                        foreach (string key in _lstParameters.Keys)
                        {
                            if (key == OutputParameterCollection[iCnt].Name)
                            {
                                InputParameterCollection[iCnt].Name = OutputParameterCollection[iCnt].Name;

                                string val = null;
                                _lstParameters.TryGetValue(key, out val);
                                string RequestValue = val;

                                if (val.Length < 1)
                                {
                                    //         if (OutputParameterCollection[iCnt].AllowBlank
                                    //|| OutputParameterCollection[iCnt].Nullable)
                                    //         {
                                    //             continue;
                                    //         }
                                }

                                if ((RequestValue != null) && (RequestValue != "#null#"))
                                {
                                    string[] RequestValues = RequestValue.Split(new char[] { '|', '|', '|' });

                                    for (int iValueCnt = 0; iValueCnt < RequestValues.Length; ++iValueCnt)
                                    {
                                        if (OutputParameterCollection[iCnt].DataType == ParameterDataType.DateTime)
                                        {
                                            DateTime dt;
                                            if (DateTime.TryParse(RequestValues[iValueCnt], CultureInfo.GetCultureInfo(SAHL.Common.Web.UI.Controls.Constants.CultureGb), DateTimeStyles.None, out dt))
                                            {
                                                // Reporting Services has a bug where al ldates must be passed as MM-dd-yyyy
                                                InputParameterCollection[iCnt].Values.Add(dt.ToString("yyyy-MM-dd"));
                                            }
                                        }
                                        else if (OutputParameterCollection[iCnt].DataType == ParameterDataType.String)
                                        {
                                            if (OutputParameterCollection[iCnt].MultiValue)
                                            {
                                                string[] szValues = RequestValues[iValueCnt].Split('\r');
                                                foreach (string szValue in szValues)
                                                {
                                                    InputParameterCollection[iCnt].Values.Add(szValue);
                                                }
                                            }
                                            else
                                                InputParameterCollection[iCnt].Values.Add(RequestValues[iValueCnt]);
                                        }
                                        else
                                            InputParameterCollection[iCnt].Values.Add(RequestValues[iValueCnt]);
                                    }
                                }
                            }
                        }
                    }

                    List<ReportParameter> coll = new List<ReportParameter>();

                    coll.AddRange(InputParameterCollection);

                    for (int x = coll.Count - 1; x >= 0; x--)
                    {
                        if (InputParameterCollection[x] != null)
                        {
                            if (InputParameterCollection[x].Name.Length < 1)
                            {
                                coll.RemoveAt(x);
                            }
                        }
                        else
                        {
                            coll.RemoveAt(x);
                        }
                    }

                    if (!IsPostBack)
                    {
                        _rV.ServerReport.SetParameters(coll.ToArray());
                    }

                    //foreach (ISqlReportParameter p in _parameters.Keys)
                    //{
                    //    object val = null;

                    //    if(p != null && p.ValidValues.Count == 0)
                    //    {
                    //        if (p.Required.HasValue && p.Required.Value == false)
                    //        {
                    //            continue;
                    //        }
                    //    }

                    //    _parameters.TryGetValue(p, out val);
                    //    if (val != null)
                    //    {
                    //        _lstParameters.Add(p.ParameterName, val.ToString());
                    //    }
                    //    else
                    //    {
                    //        _lstParameters.Add(p.ParameterName, null);

                    //    }
                    //}

                    //int outputParamCnt = 0;

                    //for (int i = 0; i < OutputParameterCollection.Count; i++)
                    //{
                    //    IReportParameter p = OutputParameterCollection[i] as IReportParameter;

                    //    if (OutputParameterCollection[i].PromptUser)
                    //    {
                    //            outputParamCnt++;
                    //    }
                    //}

                    //ReportParameter[] InputParameterCollection = new ReportParameter[outputParamCnt];

                    //for (int iCnt = 0; iCnt < OutputParameterCollection.Count; ++iCnt)
                    //{
                    //    if (!OutputParameterCollection[iCnt].PromptUser)
                    //        continue;

                    //    IReportParameter p = OutputParameterCollection[iCnt] as IReportParameter;
                    //    if (!p.Required && OutputParameterCollection[iCnt].Values.Count == 0)
                    //    {
                    //        continue;
                    //    }

                    //    InputParameterCollection[iCnt] = new ReportParameter();
                    //    bool found = false;
                    //    foreach (string key in _lstParameters.Keys)
                    //    {
                    //        if (key == OutputParameterCollection[iCnt].Name)
                    //        {
                    //            InputParameterCollection[iCnt].Name = OutputParameterCollection[iCnt].Name;

                    //            found = true;

                    //            string val = null;
                    //            _lstParameters.TryGetValue(key, out val);
                    //            string RequestValue = val;

                    //            if ((RequestValue != null) && (RequestValue != "#null#"))
                    //            {
                    //                string[] RequestValues = RequestValue.Split(',');

                    //                for (int iValueCnt = 0; iValueCnt < RequestValues.Length; ++iValueCnt)
                    //                {
                    //                    if (OutputParameterCollection[iCnt].DataType == ParameterDataType.DateTime)
                    //                    {
                    //                        DateTime dt;
                    //                        if (DateTime.TryParse(RequestValues[iValueCnt], DateTimeFormatInfo.CurrentInfo, System.Globalization.DateTimeStyles.None, out dt))
                    //                        {
                    //                            // Reporting Services has a bug where al ldates must be passed as MM-dd-yyyy
                    //                            InputParameterCollection[iCnt].Values.Add(dt.ToString("yyyy-MM-dd"));

                    //                        }
                    //                    }
                    //                    else if (OutputParameterCollection[iCnt].DataType == ParameterDataType.String)
                    //                    {
                    //                        if (OutputParameterCollection[iCnt].MultiValue)
                    //                        {
                    //                            string[] szValues = RequestValues[iValueCnt].Split('\r');
                    //                            foreach (string szValue in szValues)
                    //                            {
                    //                                InputParameterCollection[iCnt].Values.Add(szValue);
                    //                            }
                    //                        }
                    //                        else
                    //                            InputParameterCollection[iCnt].Values.Add(RequestValues[iValueCnt]);
                    //                    }
                    //                    else
                    //                        InputParameterCollection[iCnt].Values.Add(RequestValues[iValueCnt]);
                    //                }
                    //            }
                    //        }
                    //    }

                    //    if (!found)
                    //    {
                    //        if (!OutputParameterCollection[iCnt].PromptUser)
                    //        {
                    //            InputParameterCollection[iCnt].Name = OutputParameterCollection[iCnt].Name;
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception err)
            {
                this.Messages.Add(new Error(err.Message, err.Message));
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (OnCancelButtonClicked != null)
            {
                OnCancelButtonClicked(sender, e);
            }
        }

        #region IReportingServicesReport Members

        /// <summary>
        ///
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        public string ReportPath
        {
            set { _reportPath = value; }
        }

        public IDictionary<IReportParameter, object> parameters
        {
            set { _parameters = value; }
        }

        #endregion IReportingServicesReport Members
    }
}