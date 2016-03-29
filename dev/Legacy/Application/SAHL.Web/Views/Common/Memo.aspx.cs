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
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Authentication;
using System.Security.Principal;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;


namespace SAHL.Web.Views.Common
{
    /// <summary>
    /// Memo View
    /// </summary>
  public partial class Memo : SAHLCommonBaseView,SAHL.Web.Views.Common.Interfaces.IMemo
    {

    private const int MemoCharlimit = 7000;
    private const int GridMemoMaxLen = 80;
    private string _memoText;
    private IADUser adUser;
    private IOrganisationStructureRepository OSR;
    private ILookupRepository _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
    private ICommonRepository _comRepo;

    /// <summary>
    /// Set up client side script on Load of Page 
    /// </summary>
    /// <param name="e"></param>
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (!ShouldRunPage) return;
        
        ClientSideSetup();

        OSR = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
        adUser = OSR.GetAdUserForAdUserName(CurrentPrincipal.Identity.Name);
    }
    /// <summary>
    /// Bind Memo Grid
    /// </summary>
    /// <param name="memoLst"></param>
    /// <param name="action"></param>
    public void BindMemoGrid(IEventList<SAHL.Common.BusinessModel.Interfaces.IMemo> memoLst,string action)
    {
        bool showExpiryDateColumn = action != "FollowUp" ? true : false;

        MemoRecordsGrid.Columns.Clear();
        MemoRecordsGrid.AddGridBoundColumn("", "", Unit.Percentage(0), HorizontalAlign.Left, false);
        MemoRecordsGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Insert Date", false, Unit.Percentage(15), HorizontalAlign.Center, true);
        MemoRecordsGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Reminder Date", false, Unit.Percentage(15), HorizontalAlign.Center, true);
        MemoRecordsGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Expiry Date", false, Unit.Percentage(15), HorizontalAlign.Center, showExpiryDateColumn);
        MemoRecordsGrid.AddGridBoundColumn("", "Memo", Unit.Percentage(25), HorizontalAlign.Left, true);
        MemoRecordsGrid.AddGridBoundColumn("", "Captured By", Unit.Percentage(15), HorizontalAlign.Left, true);
        MemoRecordsGrid.AddGridBoundColumn("", "Status", Unit.Percentage(9), HorizontalAlign.Left, true);

        MemoRecordsGrid.DataSource = memoLst;
        MemoRecordsGrid.DataBind();
    }

    /// <summary>
    /// Bind Memo Grid for Display and Update Mode
    /// </summary>
    /// <param name="memoLst"></param>
    /// <param name="selectedIndex"></param>
    public void BindMemoGridForDisplayAndUpdate(IEventList<SAHL.Common.BusinessModel.Interfaces.IMemo> memoLst, int selectedIndex)
    {
        // There are times when the grid only displays memos as per the height - issue with this grid ?
        MemoRecordsGrid.Columns.Clear();
        MemoRecordsGrid.AddGridBoundColumn("", "", Unit.Percentage(0), HorizontalAlign.Left, false);
        MemoRecordsGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Insert Date", false, Unit.Percentage(15), HorizontalAlign.Center, true);
        MemoRecordsGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Reminder Date", false, Unit.Percentage(15), HorizontalAlign.Center, true);
        MemoRecordsGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Expiry Date", false, Unit.Percentage(15), HorizontalAlign.Center, true);
        MemoRecordsGrid.AddGridBoundColumn("", "Memo", Unit.Percentage(25), HorizontalAlign.Left, true);
        MemoRecordsGrid.AddGridBoundColumn("", "Captured By", Unit.Percentage(15), HorizontalAlign.Left, true);
        MemoRecordsGrid.AddGridBoundColumn("", "Status", Unit.Percentage(9), HorizontalAlign.Left, true);

        MemoRecordsGrid.DataSource = memoLst;

        if (memoLst.Count > selectedIndex)
            MemoRecordsGrid.SelectedIndex = selectedIndex;
        else
            MemoRecordsGrid.SelectedIndex = 0;

        MemoRecordsGrid.DataBind();

    }

   /// <summary>
    /// Bind Memo Grid for Follow Up
   /// </summary>
   /// <param name="memoLst"></param>
   /// <param name="selectedIndex"></param>
    public void BindMemoGridForFollowUp(IEventList<SAHL.Common.BusinessModel.Interfaces.IMemo> memoLst, int selectedIndex)
    {
        MemoRecordsGrid.Columns.Clear();
        MemoRecordsGrid.AddGridBoundColumn("", "", Unit.Percentage(0), HorizontalAlign.Left, false);
        MemoRecordsGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Insert Date", false, Unit.Percentage(15), HorizontalAlign.Center, true);
        MemoRecordsGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Reminder Date", false, Unit.Percentage(15), HorizontalAlign.Center, true);
        MemoRecordsGrid.AddGridBoundColumn("", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Expiry Date", false, Unit.Percentage(15), HorizontalAlign.Center, false);
        MemoRecordsGrid.AddGridBoundColumn("", "Memo", Unit.Percentage(25), HorizontalAlign.Left, true);
        MemoRecordsGrid.AddGridBoundColumn("", "Captured By", Unit.Percentage(15), HorizontalAlign.Left, true);
        MemoRecordsGrid.AddGridBoundColumn("", "Status", Unit.Percentage(9), HorizontalAlign.Left, true);

        MemoRecordsGrid.DataSource = memoLst;

        if (memoLst != null)
        {
            if (memoLst.Count > selectedIndex)
                MemoRecordsGrid.SelectedIndex = selectedIndex;
            else
                MemoRecordsGrid.SelectedIndex = 0;
        }
        MemoRecordsGrid.DataBind();

    }

    public void ShowHours()
    {
        trHours.Visible = true;
    }

    /// <summary>
    /// Manually populate Status Drop Down : Corresponds to General Status Key Column
    /// </summary>
    public void PopulateStatusDropDown()
    {
       ddlMemoStatus.Items.Add(new ListItem (MemoStatus.All.ToString()));
       ddlMemoStatus.Items.Add(new ListItem(MemoStatus.UnResolved.ToString()));
       ddlMemoStatus.Items.Add(new ListItem(MemoStatus.Resolved.ToString()));
       ddlMemoStatus.SelectedIndex = (int)MemoStatus.UnResolved;
    }

    /// <summary>
    /// Manually populate Status Update Drop Down : Corresponds to General Status Key Column
    /// </summary>
    public void PopulateStatusUpdateDropDown()
    {
        MemoStatusUpdate.Items.Add(new ListItem(MemoStatus.Resolved.ToString()));
        MemoStatusUpdate.Items.Add(new ListItem(MemoStatus.UnResolved.ToString()));
    }

      
    /// <summary>
    /// Bind Memo Text Fields - corresponding to selected index on grid
    /// </summary>
    /// <param name="memo"></param>
    public void BindMemoFields(SAHL.Common.BusinessModel.Interfaces.IMemo memo)
    {
        InsertDate.Text = Convert.ToDateTime(memo.InsertedDate).ToString(SAHL.Common.Constants.DateFormat);
        CapturedBy.Text = memo.ADUser.ADUserName;

        if (memo.ExpiryDate != null)
        {
            ExpiryDateUpdate.Date = Convert.ToDateTime(memo.ExpiryDate);
            ExpiryDate.Text = Convert.ToDateTime(memo.ExpiryDate).ToString(SAHL.Common.Constants.DateFormat);
        }
        else
        {
            ExpiryDateUpdate.Date = DateTime.Now.Date;
            ExpiryDate.Text = memo.ExpiryDate == null ? "-" : Convert.ToDateTime(DateTime.Now).ToString(SAHL.Common.Constants.DateFormat);
        }
        if (memo.GeneralStatus.Key == (int)GeneralStatuses.Active)
            lblMemoStatus.Text = MemoStatus.UnResolved.ToString(); 
        else
            lblMemoStatus.Text = MemoStatus.Resolved.ToString();
        ReminderDate.Text = memo.ReminderDate == null ? "-" : Convert.ToDateTime(memo.ReminderDate).ToString(SAHL.Common.Constants.DateFormat);
        ReminderDateUpdate.Date = Convert.ToDateTime(memo.ReminderDate);
        if (memo.GeneralStatus.Key == (int)GeneralStatuses.Active)
            MemoStatusUpdate.SelectedValue = MemoStatus.UnResolved.ToString(); //"UnResolved"; //(int)MemoStatus.UnResolved;
        else
            MemoStatusUpdate.SelectedValue = MemoStatus.Resolved.ToString(); // "Resolved"; // (int)MemoStatus.Resolved;
        lblMemo.Text = HttpUtility.HtmlEncode(memo.Description);
        MemoUpdate.Text = memo.Description;
    }

    /// <summary>
    /// Bind Memo Text Fields - corresponding to selected index on grid
    /// </summary>
    /// <param name="memo"></param>
    public void BindMemoFieldsForFollowUpUpdate(SAHL.Common.BusinessModel.Interfaces.IMemo memo)
    {
        InsertDate.Text = Convert.ToDateTime(memo.InsertedDate).ToString(SAHL.Common.Constants.DateFormat);
        CapturedBy.Text = memo.ADUser.ADUserName;
        ExpiryDate.Text = Convert.ToDateTime(memo.ExpiryDate).ToString(SAHL.Common.Constants.DateFormat);
        ExpiryDateUpdate.Date = Convert.ToDateTime(memo.ExpiryDate);
        if (memo.GeneralStatus.Key == (int)GeneralStatuses.Active)
            lblMemoStatus.Text = MemoStatus.UnResolved.ToString();
        else
            lblMemoStatus.Text = MemoStatus.Resolved.ToString();
        ReminderDate.Text = Convert.ToDateTime(memo.ReminderDate).ToString(SAHL.Common.Constants.DateFormat);
        ReminderDateUpdate.Date = Convert.ToDateTime(memo.ReminderDate);

        string hour = "00";
        string min = "00";

        if (memo.ReminderDate.HasValue)
        {
            DateTime dteReminder = Convert.ToDateTime(memo.ReminderDate);

            if (dteReminder.Hour.ToString().Length == 1)
                hour = "0" + dteReminder.Hour.ToString();
            else
                hour = dteReminder.Hour.ToString();

            if (dteReminder.Minute.ToString().Length == 1)
                min = "0" + dteReminder.Minute.ToString();
            else
                min = dteReminder.Minute.ToString();
        }
 
        ddlHour.SelectedValue = hour;
        ddlMin.SelectedValue = min;

        if (memo.GeneralStatus.Key == (int)GeneralStatuses.Active)
            MemoStatusUpdate.SelectedValue = MemoStatus.UnResolved.ToString(); //"UnResolved"; //(int)MemoStatus.UnResolved;
        else
            MemoStatusUpdate.SelectedValue = MemoStatus.Resolved.ToString(); // "Resolved"; // (int)MemoStatus.Resolved;
        lblMemo.Text = HttpUtility.HtmlEncode(memo.Description);
        MemoUpdate.Text = memo.Description; // Dont encode here because this is a textbox value

        if (memo.GeneralStatus.Key == (int)GeneralStatuses.Inactive)
            SubmitButton.Enabled = false;
    }

  /// <summary>
  /// Hide ExpiryDate Controls for Follow Up
  /// </summary>
    
  public void HideExpiryDateControlForFollowUp()
  {
      ExpiryDate.Visible = false;
      ExpiryDateUpdate.Visible = false;
      ExpiryDateTitle.Visible = false;
      ExpiryDate.Text = "";
  }

  public void HideReminderDate()
      {
          ReminderDate.Visible = false;
          ReminderDateTitle.Visible = false;
          ReminderDateUpdate.Visible = false;
      }     

     /// <summary>
    /// RowDataBound event on Memo Grid
    /// </summary>
    /// <param name="s"></param>
    /// <param name="e"></param>
    protected void MemoRecordsGrid_RowDataBound(Object s, GridViewRowEventArgs e)
    {
        TableCellCollection cells = e.Row.Cells;

        SAHL.Common.BusinessModel.Interfaces.IMemo memo = e.Row.DataItem as SAHL.Common.BusinessModel.Interfaces.IMemo;

        if (e.Row.DataItem != null)
        {
            ComRepo.AttachUnModifiedToCurrentNHibernateSession(memo);

            cells[0].Text = memo.Key.ToString();
            cells[1].Text = memo.InsertedDate.ToString(); 
            cells[2].Text = memo.ReminderDate.ToString();
            cells[3].Text = memo.ExpiryDate.ToString();
            cells[4].Text = HttpUtility.HtmlEncode(memo.Description);
            if (cells[4].Text.Length > GridMemoMaxLen)
                cells[4].Text = cells[4].Text.Substring(0, GridMemoMaxLen);
            cells[5].Text = memo.ADUser.ADUserName;
            if (memo.GeneralStatus.Key ==(int)GeneralStatuses.Active)
                cells[6].Text = "UnResolved";
            else
                cells[6].Text = "Resolved";
        }
    }

    /// <summary>
    /// Change of Memo Status - Filters on Memo Grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlMemoStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (OnMemoStatusChanged != null)
            OnMemoStatusChanged(this.Page, new KeyChangedEventArgs(ddlMemoStatus.SelectedIndex));
    }

    /// <summary>
    /// Change of Selected Index on Memo Grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void MemoRecordsGrid_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (MemoRecordsGrid.SelectedIndex >= 0)
        {
            OnMemoGridsSelectedIndexChanged(sender, new KeyChangedEventArgs(MemoRecordsGrid.SelectedIndex));
        }
    }
     /// <summary>
    /// Set Memo Grid PostBackType depending on Presenter (action - add, Update and Display)
    /// </summary>
    public void SetGridPostBackType()
    {
        MemoRecordsGrid.PostBackType = GridPostBackType.SingleClick;
    }

    /// <summary>
    /// Click event (Cancel) of Cancel button 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void CancelButton_Click(object sender, EventArgs e)
    {
        if (CancelButtonClicked != null)
            CancelButtonClicked(sender, e);
    }

    /// <summary>
    /// Click event (Add and Update) of Submit button 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void SubmitButton_Click(object sender, EventArgs e)
    {
        if (AddButtonClicked != null)
            AddButtonClicked(sender, e);

        if (UpdateButtonClicked != null)
            UpdateButtonClicked(sender, new KeyChangedEventArgs(MemoRecordsGrid.SelectedIndex));
    }

   /// <summary>
   /// Shows Controls for Display Mode
   /// </summary>
    public bool ShowControlsDisplay
    {
        set
        {
            InsertDateTitle.Visible = value;
            InsertDate.Visible = value;
            ExpiryDate.Visible = value;
            ReminderDate.Visible = value;
            MemoPanel.Visible = value;
            CapturedBy.Visible = value;
            CapturedByTitle.Visible = value;
            lblMemoStatus.Visible = value;
       }    
    }
    /// <summary>
    /// Shows Controls for Update Mode
    /// </summary>
    public bool ShowControlsUpdate
    {
        set
        {
            ExpiryDateUpdate.Visible = value;
            ReminderDateUpdate.Visible = value;
            MemoStatusUpdate.Visible = value;
            MemoUpdate.Visible = value;
            MemoStatusUpdate.SelectedIndex = (int)MemoStatus.UnResolved;
        }
    }
    /// <summary>
    /// Shows Controls for Add Mode
    /// </summary>
    public bool ShowControlsAdd
    {
        set
        {
            ExpiryDateUpdate.Visible = value;
            ReminderDateUpdate.Visible = value;
            MemoStatusUpdate.Visible = value;
            MemoUpdate.Visible = value;
            MemoStatusUpdate.SelectedIndex = (int)MemoStatus.UnResolved;
        }
    }
    /// <summary>
    /// Sets visibility of Buttons Row depending on Presnter
    /// </summary>
    public bool ShowButtons
    {
        set
        {
            ButtonRow.Visible = value;
        }    
    }

      public void HideUpdateButton()
      {
          SubmitButton.Visible = false;
      }


    /// <summary>
    /// Set up screen text depending on presenter and action
    /// </summary>
    /// <param name="presenter"></param>
    /// <param name="action"></param>
    public void SetLabelData(string presenter,string action)
    {
        string headerCaption = presenter.Contains("Memo") ? presenter : presenter + " Memo";
        AccountMemoStatusTitle.Text = presenter + " Status";
        MemoRecordsGrid.HeaderCaption = headerCaption;
        if (!String.IsNullOrEmpty(action))
        {
              SubmitButton.Text = action;
              SubmitButton.AccessKey = action.Substring(0,1);  
        }
    }

    /// <summary>
    /// Set up default values for Reminder Date and Expiry Date in the case of Add
    /// Also set Postbacktype to None
    /// </summary>
    public void SetDefaultDateForAdd()
    {
        if (!ReminderDateUpdate.Date.HasValue)
            ReminderDateUpdate.Date = DateTime.Now;
        if (!ExpiryDateUpdate.Date.HasValue)
            ExpiryDateUpdate.Date = DateTime.Now.AddDays(1.0);
      
        MemoRecordsGrid.PostBackType = GridPostBackType.None;
    }


    private string MemoText
    {
        get
        {
            _memoText = MemoUpdate.Text;
            if (_memoText.Length > MemoCharlimit)
                _memoText = _memoText.Substring(0, MemoCharlimit);
            return _memoText;
        }
    }

    /// <summary>
    /// Capture information entered on screen for Add
    /// </summary>
    /// <param name="memo"></param>
    /// <returns></returns>
      public SAHL.Common.BusinessModel.Interfaces.IMemo GetCapturedMemo(SAHL.Common.BusinessModel.Interfaces.IMemo memo)
      {
          if (ExpiryDateValue.HasValue)
          {
              memo.ExpiryDate = Convert.ToDateTime(ExpiryDateUpdate.Date.Value);
          }
          if (MemoStatusUpdate.SelectedValue == "Resolved")
              memo.GeneralStatus = _lookupRepo.GeneralStatuses[GeneralStatuses.Inactive];
          else
              memo.GeneralStatus = _lookupRepo.GeneralStatuses[GeneralStatuses.Active];

          //memo.ReminderDate = Convert.ToDateTime(ReminderDateUpdate.Date);
          if (ReminderDateValue.HasValue)
          {
              memo.ReminderDate = ReminderDateValue;
          }

          memo.ADUser = adUser;
          memo.Description = MemoText;
          memo.InsertedDate = DateTime.Now;

          return memo;
      }

      /// <summary>
      /// Capture memo for without followup (used for lead update)
      /// </summary>
      /// <param name="memo"></param>
      /// <returns></returns>
      public SAHL.Common.BusinessModel.Interfaces.IMemo GetUpdatedMemoWithoutFollowup(SAHL.Common.BusinessModel.Interfaces.IMemo memo)
      {
          if (MemoStatusUpdate.SelectedValue == "Resolved")
              memo.GeneralStatus = _lookupRepo.GeneralStatuses[GeneralStatuses.Inactive];
          else
              memo.GeneralStatus = _lookupRepo.GeneralStatuses[GeneralStatuses.Active];

          //memo.ReminderDate = Convert.ToDateTime(ReminderDateUpdate.Date);
          if (ReminderDateValue.HasValue)
          {
              memo.ReminderDate = ReminderDateValue;
          }

          memo.ADUser = adUser;
          memo.Description = MemoText;
          memo.InsertedDate = DateTime.Now;

          return memo;
      }

    /// <summary>
    /// Capture information entered on screen for Add - Follow Up
    /// </summary>
    /// <param name="memo"></param>
    /// <returns></returns>
    public SAHL.Common.BusinessModel.Interfaces.IMemo GetCapturedMemoForFollowupAdd(SAHL.Common.BusinessModel.Interfaces.IMemo memo)
    {
        if (MemoStatusUpdate.SelectedValue == "Resolved")
            memo.GeneralStatus = _lookupRepo.GeneralStatuses[GeneralStatuses.Inactive];
        else
            memo.GeneralStatus = _lookupRepo.GeneralStatuses[GeneralStatuses.Active];
        
        if (ReminderDateValue.HasValue)
        {
            DateTime reminderDate = new DateTime(ReminderDateUpdate.Date.Value.Year, ReminderDateUpdate.Date.Value.Month, ReminderDateUpdate.Date.Value.Day, int.Parse(ddlHour.SelectedValue), int.Parse(ddlMin.SelectedValue), 0);
            memo.ReminderDate = Convert.ToDateTime(reminderDate);
        }
        memo.ADUser = adUser;
        memo.Description = MemoText;
        memo.InsertedDate = DateTime.Now;
        return memo;
    }

    /// <summary>
    /// Capture information entered on screen for Add - Without Follow Up
    /// </summary>
    /// <param name="memo"></param>
    /// <returns></returns>
    public SAHL.Common.BusinessModel.Interfaces.IMemo GetCapturedMemoAddWithoutFollowup(SAHL.Common.BusinessModel.Interfaces.IMemo memo)
    {
        if (MemoStatusUpdate.SelectedValue == "Resolved")
            memo.GeneralStatus = _lookupRepo.GeneralStatuses[GeneralStatuses.Inactive];
        else
            memo.GeneralStatus = _lookupRepo.GeneralStatuses[GeneralStatuses.Active];
        
        memo.ADUser = adUser;
        memo.Description = MemoText;
        memo.InsertedDate = DateTime.Now;

        return memo;
    }
    /// <summary>
    /// Capture information entered on screen for Update
    /// </summary>
    /// <param name="memo"></param>
    /// <returns></returns>
    public SAHL.Common.BusinessModel.Interfaces.IMemo GetUpdatedMemo(SAHL.Common.BusinessModel.Interfaces.IMemo memo)
    {
        if (ExpiryDateValue.HasValue)
        {
            memo.ExpiryDate = Convert.ToDateTime(ExpiryDateUpdate.Date.Value);
        }
        if (MemoStatusUpdate.SelectedValue == "Resolved")
            memo.GeneralStatus = _lookupRepo.GeneralStatuses[GeneralStatuses.Inactive];
        else
            memo.GeneralStatus = _lookupRepo.GeneralStatuses[GeneralStatuses.Active];
        if (ReminderDateValue.HasValue)
        {
            memo.ReminderDate = Convert.ToDateTime(ReminderDateUpdate.Date.Value);
        }
        memo.ADUser = adUser;
        memo.Description = MemoText;
        return memo;
    }

    /// <summary>
    /// Capture information entered on screen for Update - Follow Up Presenter
    /// </summary>
    /// <param name="memo"></param>
    /// <returns></returns>
    public SAHL.Common.BusinessModel.Interfaces.IMemo GetUpdatedMemoForFollowUp(SAHL.Common.BusinessModel.Interfaces.IMemo memo)
    {
        if (MemoStatusUpdate.SelectedValue == "Resolved")
            memo.GeneralStatus = _lookupRepo.GeneralStatuses[GeneralStatuses.Inactive];
        else
            memo.GeneralStatus = _lookupRepo.GeneralStatuses[GeneralStatuses.Active];
        if (ReminderDateValue.HasValue)
        {
            DateTime reminderDateExpiryDateUpdate = new DateTime(ReminderDateUpdate.Date.Value.Year, ReminderDateUpdate.Date.Value.Month, ReminderDateUpdate.Date.Value.Day, int.Parse(ddlHour.SelectedValue), int.Parse(ddlMin.SelectedValue), 0);
            memo.ReminderDate = Convert.ToDateTime(reminderDateExpiryDateUpdate);
        }
        memo.ADUser = adUser;
        memo.Description = MemoText;
        return memo;
    }
       
       
     
    private void ClientSideSetup()
    {
        ClientScript.RegisterClientScriptInclude("SAHLScripts", Page.ResolveClientUrl("~/Scripts/SAHLScripts.js"));

        string m_sKeyPressEventCall = "LimitField('{0}', '{1}', {2});";

        m_sKeyPressEventCall = String.Format(m_sKeyPressEventCall, MemoUpdate.ClientID, lblMessageField.ClientID, MemoCharlimit.ToString());
        MemoUpdate.Attributes.Add("onKeyUp", m_sKeyPressEventCall);
    }

    #region event handlers

    /// <summary>
    /// Event handler for change of Memo Status - Filters on Memo Grid
    /// </summary>
    public event KeyChangedEventHandler OnMemoStatusChanged;

    /// <summary>
    /// Event handler for change of Memo Grid Selected Index Change
    /// </summary>
    public event KeyChangedEventHandler OnMemoGridsSelectedIndexChanged;

    /// <summary>
    /// Event handler for Add Button Clicked
    /// </summary>
    public event EventHandler AddButtonClicked;
    /// <summary>
    /// Event handler for Update Button Clicked
    /// </summary>
    public event KeyChangedEventHandler UpdateButtonClicked;
    /// <summary>
    /// Event handler for Cancel Button Clicked
    /// </summary>
    public event EventHandler CancelButtonClicked;

    #endregion

    #region Properties

      public DateTime? ReminderDateValue
      {
          get
          {
              return ReminderDateUpdate.Date;
          }
      }

      public DateTime? ExpiryDateValue
      {
          get
          {
              return ExpiryDateUpdate.Date;
          }
      }

      public int MemoStatusSelectedValue
      {
          get
          {
                  return Convert.ToInt32((MemoStatus)(Enum.Parse(typeof(MemoStatus),(ddlMemoStatus.SelectedValue))));
          }
          set
          {
              ddlMemoStatus.SelectedValue = ((MemoStatus)value).ToString();
          }
      }

      

      private ICommonRepository ComRepo
      {
          get 
          {
              if (_comRepo == null)
                  _comRepo = RepositoryFactory.GetRepository<ICommonRepository>();

              return _comRepo; 
          }
      }
	
    #endregion
  }
}


