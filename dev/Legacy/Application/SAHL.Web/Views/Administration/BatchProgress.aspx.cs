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
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Web.UI.Controls;

namespace SAHL.Web.Views.Administration
{
    /// <summary>
    /// View_BatchProgress
    /// </summary>
    public partial class Views_BatchProgress : SAHLCommonBaseView, IBatchProgress
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!ShouldRunPage) return;
        }
        /// <summary>
        /// DataRowBound Event for BatchGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BatchGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            TableCellCollection cells = e.Row.Cells;

            IBulkBatch bulkBatch = e.Row.DataItem as IBulkBatch;

            if (e.Row.DataItem != null)
            {
                cells[0].Text = bulkBatch.Description;
                cells[1].Text = bulkBatch.EffectiveDate.ToString(); ;
                cells[2].Text = bulkBatch.BulkBatchStatus.Description.ToString();
                cells[3].Text = bulkBatch.UserID.ToString();
                cells[4].Text = bulkBatch.Key.ToString();
            }    
        }
        /// <summary>
        /// Bind Batch Grid 
        /// </summary>
        public void BindBatchGrid(IList<IBulkBatch> bulkTransactions) 
        {    
            BatchGrid.AddGridBoundColumn("", "Description", Unit.Percentage(40), HorizontalAlign.Left, true);
            BatchGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Effective Date", false, Unit.Percentage(15), HorizontalAlign.Center, true);
            BatchGrid.AddGridBoundColumn("", "Batch Status", Unit.Percentage(30), HorizontalAlign.Left, true);
            BatchGrid.AddGridBoundColumn("", "User", Unit.Percentage(15), HorizontalAlign.Center, true);
            BatchGrid.AddGridBoundColumn("", "", Unit.Percentage(0), HorizontalAlign.Left, false);

            BatchGrid.DataSource = bulkTransactions;

            if (Request.Form["__EVENTTARGET"] != null)
            {
                if (Request.Form["__EVENTTARGET"] == BatchTypeList.UniqueID)
                {
                    BatchGrid.SelectedIndex = 0;

                    MessageTypeList.AutoPostBack = false;
                    MessageTypeList.SelectedIndex = 0;
                    MessageTypeList.AutoPostBack = true;
                }
            }

            BatchGrid.DataBind();
        }

      /// <summary>
      /// MessageGrid RowDataBound event
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
        protected void MessageGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            TableCellCollection cells = e.Row.Cells;

            IBulkBatchLog bulkBatchLog = e.Row.DataItem as IBulkBatchLog;

            if (e.Row.DataItem != null)
            {
                cells[0].Text = bulkBatchLog.Description == null ? " " : bulkBatchLog.Description;
                cells[1].Text = bulkBatchLog.MessageReference == null ? " " : bulkBatchLog.MessageReference.ToString();
                cells[2].Text = bulkBatchLog.MessageReferenceKey ==  null ? " " : bulkBatchLog.MessageReferenceKey.ToString();
            }
        }
        /// <summary>
        /// Bind Message Grid according to Message Value and Grid Value Selected 
        /// </summary>
        /// <param name="bulkBatchLog"></param>
        public void BindMessageGrid(IList<IBulkBatchLog> bulkBatchLog)
        {
            MessageGrid.AddGridBoundColumn("", "Description", Unit.Percentage(60), HorizontalAlign.Left, true);
            MessageGrid.AddGridBoundColumn("", "Reference", Unit.Percentage(20), HorizontalAlign.Left, true);
            MessageGrid.AddGridBoundColumn("", "Value", Unit.Percentage(20), HorizontalAlign.Left, true);

            MessageGrid.DataSource = bulkBatchLog;
            MessageGrid.DataBind();
        }
        /// <summary>
        /// Bind LookUp - Drop Downs
        /// </summary>
        /// <param name="bulkBatchTypes"></param>
        /// <param name="messageType"></param>
        public void BindLookUps(IEventList<IBulkBatchType> bulkBatchTypes, IEventList<IMessageType> messageType)
        {
            BatchTypeList.DataSource = bulkBatchTypes;
            BatchTypeList.DataTextField = "Description";
            BatchTypeList.DataValueField = "Key";
            BatchTypeList.DataBind();
            BatchTypeList.Items.Insert(0, new ListItem("- Please Select -", "-select-"));

            MessageTypeList.DataSource = messageType;
            MessageTypeList.DataTextField = "Description";
            MessageTypeList.DataValueField = "Key";
            MessageTypeList.DataBind();
            MessageTypeList.Items.Insert(0, new ListItem("- Please Select -", "-select-"));

        }
        /// <summary>
        /// Bind Batch Grid data fields based on Grid Selected Item
        /// </summary>
        /// <param name="bulkBatch"></param>
        public void BindBatchGridFields(IBulkBatch bulkBatch)
        {
            Description.Text = bulkBatch.Description == null ? " " : bulkBatch.Description;
            EffectiveDate.Text = Convert.ToDateTime(bulkBatch.EffectiveDate).ToString(SAHL.Common.Constants.DateFormat);
            Started.Text = bulkBatch.StartDateTime == null ? " - " : Convert.ToDateTime(bulkBatch.StartDateTime).ToString(SAHL.Common.Constants.DateTimeFormat);
            BatchStatus.Text = bulkBatch.BulkBatchStatus.Description;
            Finished.Text = bulkBatch.CompletedDateTime == null ? "-" : Convert.ToDateTime(bulkBatch.CompletedDateTime).ToString(SAHL.Common.Constants.DateTimeFormat);
            FileName.Text = bulkBatch.FileName == null ? " " : bulkBatch.FileName.ToString();
            
        }
        /// <summary>
        /// SelectedIndexChange for BatchTypeList
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BatchTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OnBatchTypeListSelectedIndexChange != null)
                OnBatchTypeListSelectedIndexChange(this.Page, new KeyChangedEventArgs(BatchTypeList.SelectedIndex));

        }
        /// <summary>
        /// SelectedIndexChange for MessageTypeList
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MessageTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Request.Form["__EVENTTARGET"] != null)
            {
                if (Request.Form["__EVENTTARGET"] == BatchTypeList.UniqueID)
                {
                    MessageTypeList.AutoPostBack = false;
                    MessageTypeList.SelectedIndex = 0;
                    MessageTypeList.AutoPostBack = true;
                }
            }

            if (OnMessageTypeListSelectedIndexChange != null)
                OnMessageTypeListSelectedIndexChange(this.Page, new KeyChangedEventArgs(MessageTypeList.SelectedIndex));
  
        }
        /// <summary>
        /// Selected Index Change even for Batch Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BatchGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageTypeList.AutoPostBack = false;
            MessageTypeList.SelectedIndex = 0;
            MessageTypeList.AutoPostBack = true;

            MessageGrid.DataSource = null;
            MessageGrid.DataBind();
   
            if (BatchGrid.SelectedIndex >= 0)
            {
                OnBatchGridSelectedIndexChanged(sender, new KeyChangedEventArgs(BatchGrid.SelectedIndex));
            }

        }

        public int GetSelectedIndexOnGrid
        {
            get { return BatchGrid.SelectedIndex; }
        }

        public int GetSelectedMessageType
        {
            get { return MessageTypeList.SelectedIndex; }
        }
        /// <summary>
        /// Click Event for Refresh button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RefreshButton_Click(object sender, EventArgs e)
        {

        }

        public int? BulkBatchTypeKey
        {
            get
            {
                if (BatchTypeList.SelectedIndex == 0)
                    return new int?();
                else
                    return new int?(Int32.Parse(BatchTypeList.SelectedValue));
            }
            set
            {
                if (value.HasValue)
                    BatchTypeList.SelectedValue = value.Value.ToString();
                else
                    BatchTypeList.SelectedValue = "";
            }
        }

        //private void CreateRefreshMetaTag()
        //{
        //    HtmlMeta htmlMeta = new HtmlMeta();
        //    HtmlHead htmlHead = (HtmlHead)Page.Header;

        //    htmlMeta.Name = "refresh";
        //    htmlMeta.Content = "60000";
        //    htmlHead.Controls.Add(htmlMeta);
        //}
        #region EventHandlers
        /// <summary>
        /// Change event of selected index on BatchGrid
        /// </summary>
        public event KeyChangedEventHandler OnBatchGridSelectedIndexChanged;
        /// <summary>
        /// Change event of MessageTypeList change
        /// </summary>
        public event KeyChangedEventHandler OnMessageTypeListSelectedIndexChange;
        /// <summary>
        /// Change event of BatchTypeList
        /// </summary>
        public event KeyChangedEventHandler OnBatchTypeListSelectedIndexChange;

        #endregion

    }
}
    //#region Private Variables
    //private enum PageState
    //{
    //    Display = 1,
    //    Extract,
    //    Import,
    //    MailingHouse
    //}

    //CapController m_MyController = null;
    //private PageState m_PageState;

    //#endregion

    //protected override void OnInit(EventArgs e)
    //{
    //    base.OnInit(e);
    //    m_MyController = base.Controller as CapController;
    //    m_MyController.ClientMetrics = base.GetClientMetrics();

    //    if (!ShouldRunPage())
    //        return;

    //    ViewSettings viewSettings = UIPConfiguration.Config.GetViewSettingsFromName(base.ViewName);
    //    if (viewSettings.CustomAttributes.Count > 0)
    //    {
    //        System.Xml.XmlNode PageStateNode = viewSettings.CustomAttributes.GetNamedItem("state");
    //        if (PageStateNode != null)
    //        {
    //            switch (PageStateNode.Value)
    //            {
    //                case "display":
    //                    m_PageState = PageState.Display;
    //                    break;
    //                case "extract":
    //                    m_PageState = PageState.Extract;
    //                    break;
    //                case "import":
    //                    m_PageState = PageState.Import;
    //                    break;
    //                case "mailinghouse":
    //                    m_PageState = PageState.MailingHouse;
    //                    break;
    //                default:
    //                    m_PageState = PageState.Display;
    //                    break;
    //            }
    //        }
    //    }

    //    PopulateLookups();
    //    bindCombos();

    //    int bulkBatchTypeKey = -1;

    //    if (m_PageState == PageState.Display)
    //    {
    //        if (Request.Form[BatchTypeList.UniqueID] != null)
    //            bulkBatchTypeKey = int.Parse(Request.Form[BatchTypeList.UniqueID]);
    //        BatchTypeList.Enabled = true;
    //    }
    //    else
    //    {
    //        switch (m_PageState)
    //        {
    //            case PageState.Extract:
    //                bulkBatchTypeKey = 2;
    //                break;
    //            case PageState.Import:
    //                bulkBatchTypeKey = 3;
    //                break;
    //            case PageState.MailingHouse:
    //                bulkBatchTypeKey = 4;
    //                break;
    //        }

    //        foreach (ListItem li in BatchTypeList.Items)
    //        {
    //            if (li.Value == bulkBatchTypeKey.ToString())
    //            {
    //                li.Selected = true;
    //                BatchTypeList.Enabled = false;
    //                break;
    //            }
    //        }
    //    }

    //    m_MyController.GetBulkBatchByBulkBatchType(bulkBatchTypeKey);

    //    bindGrid();
    //}

    //private void CreateRefreshMetaTag()
    //{
    //    HtmlMeta htmlMeta = new HtmlMeta();
    //    HtmlHead htmlHead = (HtmlHead)Page.Header;

    //    htmlMeta.Name = "refresh";
    //    htmlMeta.Content = "60000";
    //    htmlHead.Controls.Add(htmlMeta);
    //} 

    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    if (!ShouldRunPage())
    //        return;

    //    bindDisplay();
    //}

    //private void bindCombos()
    //{
    //    BatchTypeList.DataSource = m_MyController.Lookups.BulkBatchType;
    //    BatchTypeList.DataTextField = "Description";
    //    BatchTypeList.DataValueField = "BulkBatchTypeKey";
    //    BatchTypeList.DataBind();
    //    BatchTypeList.Items.Insert(0, new ListItem("- Please Select -", "-select-"));

    //    MessageTypeList.DataSource = m_MyController.Lookups.MessageType;
    //    MessageTypeList.DataTextField = "Description";
    //    MessageTypeList.DataValueField = "MessageTypeKey";
    //    MessageTypeList.DataBind();
    //    MessageTypeList.Items.Insert(0, new ListItem("- Please Select -", "-select-"));
    //}

    //private void bindGrid()
    //{
    //    BatchGrid.AddGridBoundColumn("Description", "Description", Unit.Percentage(40), HorizontalAlign.Left, true);
    //    BatchGrid.AddGridBoundColumn("EffectiveDate", Constants.DateFormat, GridFormatType.GridDate, "Effective Date", false, Unit.Percentage(15), HorizontalAlign.Center, true);
    //    BatchGrid.AddGridBoundColumn("BulkBatchStatusKey", "Batch Status", Unit.Percentage(30), HorizontalAlign.Left, true);
    //    BatchGrid.AddGridBoundColumn("UserID", "User", Unit.Percentage(15), HorizontalAlign.Center, true);
    //    BatchGrid.AddGridBoundColumn("BulkBatchKey", "", Unit.Percentage(0), HorizontalAlign.Left, false);

    //    BatchGrid.DataSource = m_MyController.m_BulkBatchDS._BulkBatch;
    //    if (Request.Form["__EVENTTARGET"] != null)
    //    {
    //        if (Request.Form["__EVENTTARGET"] == BatchTypeList.UniqueID)
    //        {
    //            BatchGrid.SelectedIndex = 0;

    //            MessageTypeList.AutoPostBack = false;
    //            MessageTypeList.SelectedIndex = 0;
    //            MessageTypeList.AutoPostBack = true;
    //        }
    //    }
    //    BatchGrid.DataBind();
    //}

    //private void bindDisplay()
    //{
    //    if (m_MyController.m_BulkBatchDS._BulkBatch.Rows.Count == 0) return;

    //    BulkBatch.BulkBatchRow bRow = m_MyController.m_BulkBatchDS._BulkBatch.FindByBulkBatchKey(int.Parse(BatchGrid.Rows[BatchGrid.SelectedIndexInternal].Cells[4].Text));
    //    if (bRow != null)
    //    {
    //        if (bRow.IsDescriptionNull())
    //            Description.Text = "";
    //        else
    //            Description.Text = bRow.Description;
    //        EffectiveDate.Text = bRow.EffectiveDate.ToString(Constants.DateFormat);
    //        if (bRow.IsStartDateTimeNull())
    //            Started.Text = "-";
    //        else
    //            Started.Text = bRow.StartDateTime.ToString(Constants.DateFormat);
    //        BatchStatus.Text = GetStatusText(bRow.BulkBatchStatusKey);
    //        if (bRow.IsCompletedDateTimeNull())
    //            Finished.Text = "-";
    //        else
    //            Finished.Text = bRow.CompletedDateTime.ToString(Constants.DateFormat);
    //        if (bRow.IsFileNameNull())
    //            FileName.Text = "-";
    //        else
    //            FileName.Text = bRow.FileName;
    //    }
    //    else
    //    {
    //        Description.Text = "";
    //        EffectiveDate.Text = "";
    //        Started.Text = "";
    //        BatchStatus.Text = "";
    //        Finished.Text = "";
    //        FileName.Text = "";
    //    }

    //    if (Request.Form[MessageTypeList.UniqueID] != null)
    //        MessageTypeList.SelectedValue = MessageTypeList.Items.FindByValue(Request.Form[MessageTypeList.UniqueID]).Value;
    //}

    //protected void BatchGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        string findValue;

    //        findValue = e.Row.Cells[2].Text;
    //        if (findValue.Equals("&nbsp;"))
    //            findValue = "0";
    //        Lookup.BulkBatchStatusRow sRow = m_MyController.Lookups.BulkBatchStatus.FindByBulkBatchStatusKey(int.Parse(findValue));
    //        if (sRow != null)
    //            e.Row.Cells[2].Text = sRow.Description;
    //        else
    //            e.Row.Cells[2].Text = "-";
    //    }
    //}

    //protected void BatchTypeList_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    //
    //}

    //protected void MessageTypeList_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (Request.Form["__EVENTTARGET"] != null)
    //    {
    //        if (Request.Form["__EVENTTARGET"] == BatchTypeList.UniqueID)
    //        {
    //            MessageTypeList.AutoPostBack = false;
    //            MessageTypeList.SelectedIndex = 0;
    //            MessageTypeList.AutoPostBack = true;
    //        }
    //    }

    //    if (!MessageTypeList.SelectedValue.Equals("-select-"))
    //    {
    //        m_MyController.GetBatchLogByBatchKeyMessageType(int.Parse(BatchGrid.SelectedRow.Cells[4].Text), int.Parse(MessageTypeList.SelectedValue));

    //        MessageGrid.AddGridBoundColumn("Description", "Description", Unit.Percentage(60), HorizontalAlign.Left, true);
    //        MessageGrid.AddGridBoundColumn("MessageReference", "Reference", Unit.Percentage(20), HorizontalAlign.Left, true);
    //        MessageGrid.AddGridBoundColumn("MessageReferenceKey", "Value", Unit.Percentage(20), HorizontalAlign.Left, true);

    //        MessageGrid.DataSource = m_MyController.m_BulkBatchDS.BulkBatchLog;
    //        MessageGrid.DataBind();
    //    }
    //}

    //protected void BatchGrid_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    MessageTypeList.AutoPostBack = false;
    //    MessageTypeList.SelectedIndex = 0;
    //    MessageTypeList.AutoPostBack = true;

    //    MessageGrid.DataSource = null;
    //    MessageGrid.DataBind();
    //}

    //protected void RefreshButton_Click(object sender, EventArgs e)
    //{

    //}

    //private string GetStatusText(int key)
    //{
    //    Lookup.BulkBatchStatusRow row = m_MyController.Lookups.BulkBatchStatus.FindByBulkBatchStatusKey(key);
    //    if (row != null)
    //        return row.Description;
    //    else
    //        return key.ToString();
    //}

