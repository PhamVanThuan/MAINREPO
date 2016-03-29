using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI.Controls;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel;
using System.Text;

namespace SAHL.Web.Views.Common
{
    /// <summary>
    /// 
    /// </summary>
    public partial class QuarterlyLoanStatement : SAHLCommonBaseView, IQuarterlyLoanStatement
    {
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnSubmitButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public int SelectedOriginationSource
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.Form[ddlOriginationSource.UniqueID]) && Request.Form[ddlOriginationSource.UniqueID] != "-select-")
                    return Convert.ToInt32( Request.Form[ddlOriginationSource.UniqueID] );
                else
                    return -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int SelectedResetConfiguration
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.Form[ddlResetConfiguration.UniqueID]) && Request.Form[ddlResetConfiguration.UniqueID] != "-select-")
                    return Convert.ToInt32( Request.Form[ddlResetConfiguration.UniqueID] );
                else
                    return -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int SelectedStatementMonths
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.Form[txtStatementMonths.UniqueID]))
                    return Convert.ToInt32(Request.Form[txtStatementMonths.UniqueID]);
                else
                    return -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SelectedSamples
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                if (Request.Form[SampleType1.UniqueID] != null)
                {
                    int count = 5;
                    if (Request.Form[SampleType1Count.UniqueID] != null)
                        count = Convert.ToInt32(Request.Form[SampleType1Count.UniqueID]);
                    sb.AppendLine("1:" + count.ToString());
                }
                if (Request.Form[SampleType2.UniqueID] != null)
                {
                    int count = 5;
                    if (Request.Form[SampleType2Count.UniqueID] != null)
                        count = Convert.ToInt32(Request.Form[SampleType2Count.UniqueID]);
                    sb.AppendLine("2:" + count.ToString());
                }
                if (Request.Form[SampleType3.UniqueID] != null)
                {
                    int count = 5;
                    if (Request.Form[SampleType3Count.UniqueID] != null)
                        count = Convert.ToInt32(Request.Form[SampleType3Count.UniqueID]);
                    sb.AppendLine("3:" + count.ToString());
                }
                if (Request.Form[SampleType4.UniqueID] != null)
                {
                    int count = 5;
                    if (Request.Form[SampleType4Count.UniqueID] != null)
                        count = Convert.ToInt32(Request.Form[SampleType4Count.UniqueID]);
                    sb.AppendLine("4:" + count.ToString());
                }
                if (Request.Form[SampleType5.UniqueID] != null)
                {
                    int count = 5;
                    if (Request.Form[SampleType5Count.UniqueID] != null)
                        count = Convert.ToInt32(Request.Form[SampleType5Count.UniqueID]);
                    sb.AppendLine("5:" + count.ToString());
                }
                if (Request.Form[SampleType6.UniqueID] != null)
                {
                    int count = 5;
                    if (Request.Form[SampleType6Count.UniqueID] != null)
                        count = Convert.ToInt32(Request.Form[SampleType6Count.UniqueID]);
                    sb.AppendLine("6:" + count.ToString());
                }
                if (Request.Form[SampleType7.UniqueID] != null)
                {
                    int count = 5;
                    if (Request.Form[SampleType7Count.UniqueID] != null)
                        count = Convert.ToInt32(Request.Form[SampleType7Count.UniqueID]);
                    sb.AppendLine("7:" + count.ToString());
                }
                if (Request.Form[SampleType8.UniqueID] != null)
                {
                    int count = 5;
                    if (Request.Form[SampleType8Count.UniqueID] != null)
                        count = Convert.ToInt32(Request.Form[SampleType8Count.UniqueID]);
                    sb.AppendLine("8:" + count.ToString());
                }
                if (Request.Form[SampleType9.UniqueID] != null)
                {
                    int count = 5;
                    if (Request.Form[SampleType9Count.UniqueID] != null)
                        count = Convert.ToInt32(Request.Form[SampleType9Count.UniqueID]);
                    sb.AppendLine("9:" + count.ToString());
                }
                if (Request.Form[SampleType10.UniqueID] != null)
                {
                    int count = 5;
                    if (Request.Form[SampleType10Count.UniqueID] != null)
                        count = Convert.ToInt32(Request.Form[SampleType10Count.UniqueID]);
                    sb.AppendLine("10:" + count.ToString());
                }
                string sampleList = sb.ToString().Replace("\r\n", ",");
                if (sampleList.Length > 0)
                    sampleList = sampleList.Substring(0, sampleList.Length - 1);
                return sampleList;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SelectedMailingAddresses
        {
            get
            {
                if (MailAddress.Text.Length != 0)
                {
                    return MailAddress.Text.Replace("\r\n", ",");
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (OnSubmitButtonClicked != null)
            {
                OnSubmitButtonClicked(sender, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="osList"></param>
        public void BindOriginationSource(IList<IOriginationSource> osList)
        {
            ddlOriginationSource.DataSource = osList;
            ddlOriginationSource.DataValueField = "Key";
            ddlOriginationSource.DataTextField = "Description";
            ddlOriginationSource.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resetList"></param>
        public void BindResetConfiguration(IList<IResetConfiguration> resetList)
        {
            ddlResetConfiguration.DataSource = resetList;
            ddlResetConfiguration.DataValueField = "Key";
            ddlResetConfiguration.DataTextField = "Description";
            ddlResetConfiguration.DataBind();
        }


    }
}
