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
using SAHL.Common.Authentication;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common;
using SAHL.Common.Web.UI;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using System.Text;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Administration
{
  public partial class AddRule : SAHLCommonBaseView, IRuleDetails
  {
    
    protected System.Web.UI.HtmlControls.HtmlTable tblRules;
    protected HtmlTable tblRuleParams;
    protected override void OnInit(EventArgs e)
    {
      base.OnInit(e);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!ShouldRunPage) return;
      if (!string.IsNullOrEmpty(Request.Form[ddlParamType.UniqueID]))
          ddlParamType.SelectedValue = Request.Form[ddlParamType.UniqueID];
    }

    protected override void OnPreRender(EventArgs e)
    {
      base.OnPreRender(e);
    }

    public event EventHandler OnSearchButtonClicked;
    public event EventHandler OnRuleSelected;
    public event EventHandler OnRuleParamSelected;
    public event EventHandler OnRuleSubmitButtonClicked;
    public event EventHandler OnNewRuleClick;
    public event EventHandler OnRuleParamSubmitClicked;
	public event EventHandler OnRuleSubmitClicked;
	
    public void BindRulesGrid(IEventList<IRuleItem> Rules)
    {
      List<RuleItemBind> BindRules = new List<RuleItemBind>();
      foreach (IRuleItem r in Rules)
      {
        BindRules.Add(new RuleItemBind(r));
      }
      RulesGrid.Columns.Clear();
	  RulesGrid.AddGridBoundColumn("Key", "Key", Unit.Percentage(10), HorizontalAlign.Left, true);
      RulesGrid.AddGridBoundColumn("Name", "Rule Name", Unit.Percentage(30), HorizontalAlign.Left, true);
      RulesGrid.AddGridBoundColumn("Desc", "Description", Unit.Percentage(60), HorizontalAlign.Left, true);
	  //RulesGrid.AddGridBoundColumn("AsmName", "AssemblyName", Unit.Percentage(15), HorizontalAlign.Left, false);
	  //RulesGrid.AddGridBoundColumn("TypeName", "TypeName", Unit.Percentage(15), HorizontalAlign.Left, false);
      RulesGrid.DataSource = BindRules;
      RulesGrid.DataBind();
    }

    public void BindRule(IRuleItem Rule)
    {
	  //txtMaintAsmName.Text = Rule.AssemblyName;
	  //txtMaintRuleDesc.Text = Rule.Description;
	  //txtMaintTypeName.Text = Rule.TypeName;
	  //txtMaintRuleName.Text = Rule.Name;
      txtMaintRuleKey.Value = Rule.Key.ToString();
    }
    
    public void BindRuleParam(IRuleParameter Param)
    {
      txtPMaintVal.Text = Param.Value;
      txtPMaintName.Text = Param.Name;
      txtMaintRuleParamKey.Value = Param.Key.ToString();
      txtMaintRuleKey.Value = Param.RuleItem.Key.ToString();
      string val = Param.RuleParameterType.CSharpDataType;
      ListItem li = ddlParamType.Items.FindByText(val);
      int idx = ddlParamType.Items.IndexOf(li);
      ddlParamType.SelectedIndex = idx;
      txtMaintRuleKey.Value = Param.RuleItem.Key.ToString();
    }

	  public void PopulateParamTypeDropDown(IEventList<IParameterType> ParamTypes)
	  {
		  ddlParamType.DataSource = ParamTypes;
		  ddlParamType.DataTextField = "CSharpDataType";
		  ddlParamType.DataValueField = "Key";
		  ddlParamType.DataBind();
	  }

	  public void PopulateStatusDropDown(IList<IGeneralStatus> GeneralStatus, int GeneralStatusKey)
	  {
		  ddlStatus.DataSource = GeneralStatus;
		  ddlStatus.DataTextField = "Description";
		  ddlStatus.DataValueField = "Key";
		  ddlStatus.SelectedValue = GeneralStatusKey.ToString();
		  ddlStatus.DataBind();
	  }

	  public void PopulateEnforceRuleDropDown(bool EnforceRule)
	  {
		  ddlEnforce.Items.Clear();
		  ddlEnforce.Items.Add("Yes");
		  ddlEnforce.Items.Add("No");
		  ddlEnforce.SelectedValue = EnforceRule == true ? "Yes" : "No";
	  }

    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
      if (null != OnSearchButtonClicked)
      {
        OnSearchButtonClicked(sender, new KeyChangedEventArgs(txtRuleName.Text));
      }
    }


    protected void btnNewRule_OnClick(object sender, EventArgs e)
    {
      if (null != OnNewRuleClick)
      {
        OnNewRuleClick(null, null);
      }
    }

    protected void BtnSubmitRuleClick(object sender, EventArgs e)
    {
      if (null != OnRuleSubmitButtonClicked)
      {
        OnRuleSubmitButtonClicked(null, new KeyChangedEventArgs(RulesGrid.SelectedRow.Cells[0].Text));
      }
    }

    protected void RulesGrid_GridDoubleClick(object sender, SAHL.Common.Web.UI.Controls.GridSelectEventArgs e)
    {
      // get the rule and then get the rule params and bind those.
      int SelectedRowIndex = e.m_RowNum;
      
      int RuleKey = Convert.ToInt32(((GridViewRow)RulesGrid.Rows[e.m_RowNum]).Cells[0].Text);
      if (SelectedRowIndex != -1)
      {
        if (null != OnRuleSelected)
        {
          OnRuleSelected(sender, new GridRowSelectEventArgs(RuleKey));
        }
      }
    }

    public void BindRuleParameters(IEventList<IRuleParameter> RuleParams)
    {
      List<RuleParameterBind> Bind = new List<RuleParameterBind>();
      foreach (IRuleParameter r in RuleParams)
      {
        Bind.Add(new RuleParameterBind(r));
      }
      RulesParamGrid.Columns.Clear();
      RulesParamGrid.AddGridBoundColumn("Key", "", Unit.Percentage(0), HorizontalAlign.Left, false);
      RulesParamGrid.AddGridBoundColumn("RuleItemBindKey", "", Unit.Percentage(0), HorizontalAlign.Left, false);
      RulesParamGrid.AddGridBoundColumn("Name", "Parameter Name", Unit.Percentage(40), HorizontalAlign.Left, true);
      RulesParamGrid.AddGridBoundColumn("RuleParamTypeName", "Data Type", Unit.Percentage(0), HorizontalAlign.Left, true);
      RulesParamGrid.AddGridBoundColumn("RUleParaTypeKey", "", Unit.Percentage(20), HorizontalAlign.Left, false);
      RulesParamGrid.AddGridBoundColumn("Value", "Value", Unit.Percentage(40), HorizontalAlign.Left, true);
      RulesParamGrid.DataSource = Bind;
      RulesParamGrid.DataBind();
    }

    protected void RulesParamGrid_GridDoubleClick(object sender, GridSelectEventArgs e)
    {
      int SelectedRowIndex = e.m_RowNum;
      int Key = Convert.ToInt32(((GridViewRow)RulesParamGrid.Rows[e.m_RowNum]).Cells[0].Text);
      if (SelectedRowIndex != -1)
      {
        if (null != OnRuleParamSelected)
        {
          OnRuleParamSelected(sender, new GridRowSelectEventArgs(Key));
        }
      }
    }

    #region Properties
      public bool VisibleParamSubmitVisible
      {
          set
          {
              btnSubmitParam.Visible = value;
          }
      }
    public bool RulesTableVisible
    {
      set
      {
        tblRules.Visible = value;
      }
    }
    public bool RulesParamTableVisible
    {
      set
      {
        tblRuleParams.Visible = value;
      }
    }

    public string RuleName
    {
      get
      {
        return txtRuleName.Text;
      }
    }

    public bool RulesMaintVisible
    {
      set { tblMaintRule.Visible = value; }
    }

    public bool RulesSearchVisible
    {
      set { tblSearch.Visible = value; }
    }

	public int Get_ddlStatus
	{
		get
		{
			if (!string.IsNullOrEmpty(ddlStatus.SelectedValue))
			{
				return Convert.ToInt32(ddlStatus.SelectedValue);
			}
			else
				return (int)GeneralStatuses.Inactive;
		}
	}

	public bool Get_ddlEnforce
	{
	  get
	  {
		  if (!string.IsNullOrEmpty(ddlEnforce.SelectedValue))
		  {
			  return Convert.ToString(ddlEnforce.SelectedValue) == "Yes" ? true : false;
		  }
		  else
			  return false;
	  }
	}

	public string GetSet_txtRuleStatusReason
	{
		get
		{
			if (!string.IsNullOrEmpty(txtRuleStatusReason.Text))
			{
				return txtRuleStatusReason.Text;
			}
			else
				return "";
		}
		set
		{
			txtRuleStatusReason.Text = value;
		}
	}
	
    public bool ParamMaintVisible { set { tblMaintParam.Visible = value; } }

    public int MaintRuleKey { get { return Convert.ToInt32(txtMaintRuleKey.Value); } }
	//public string MaintRuleName { get { return txtMaintRuleName.Text; } }
	//public string MaintRuleDesc { get { return txtMaintRuleDesc.Text; } }
	//public string MaintAsmName { get { return txtMaintAsmName.Text; } }
	//public string MaintTypeName { get { return txtMaintTypeName.Text; } }
    public string RuleMaintSubmitText { set { btnMaint.Text = value; } }
    public string SearchValue { get { return txtRuleName.Text;} }
    public int PMaintParamKey { get { return Convert.ToInt32(txtMaintRuleParamKey.Value);} }
    public string PMaintName { get { return txtPMaintName.Text; } }
    public string PMaintVal { get { return txtPMaintVal.Text; } }
    public string PMaintType { get { return ddlParamType.SelectedItem.Text; } } 
    #endregion

    protected void btnSubmitParam_Click(object sender, EventArgs e)
    {
      if (null != OnRuleParamSubmitClicked)
      {
        OnRuleParamSubmitClicked(null, null);
      }
    }

	  protected void btnSubmitRule_Click(object sender, EventArgs e)
	  {
		  if (null != OnRuleSubmitClicked)
		  {
			  OnRuleSubmitClicked(null, null);
		  }
	  }

  }
  #region Internal Bind Classes
  public class RuleItemBind
  {
      public int _Key;
      public int Key { get { return _Key; } }
      public string _Name;
      public string Name { get { return _Name; } }
      public string _Desc;
      public string Desc { get { return _Desc; } }
      public string _AsmName;
      public string AsmName { get { return _AsmName; } }
      public string _TypeName;
      public string TypeName { get { return _TypeName; } }
      public List<RuleParameterBind> Params = new List<RuleParameterBind>();
      public RuleItemBind(IRuleItem ri)
      {
          _Key = ri.Key;
          _Name = ri.Name;
          _Desc = ri.Description;
          _AsmName = ri.AssemblyName;
          _TypeName = ri.TypeName;
          foreach (IRuleParameter p in ri.RuleParameters)
          {
              RuleParameterBind rpb = new RuleParameterBind(p);
              Params.Add(rpb);
          }
      }
  }

  public class RuleParameterBind
  {
      public int _Key;
      public int Key { get { return _Key; } }
      public int _RuleItemBindKey;
      public int RuleItemBindKey { get { return _RuleItemBindKey; } }
      public string _Name;
      public string Name { get { return _Name; } }
      public int _RuleParamTypeKey;
      public int RUleParaTypeKey { get { return _RuleParamTypeKey; } }
      public string _RuleParamTypeName;
      public string RuleParamTypeName { get { return _RuleParamTypeName; } }
      public string _Value;
      public string Value { get { return _Value; } }
      public RuleParamTypeBind RPType;
      public RuleParameterBind(IRuleParameter rip)
      {
          _Key = rip.Key;
          _Name = rip.Name;
          _RuleItemBindKey = rip.RuleItem.Key;
          _Value = rip.Value;
          RPType = new RuleParamTypeBind(rip.RuleParameterType);
          _RuleParamTypeKey = RPType.Key;
          _RuleParamTypeName = RPType.Desc;
      }
  }

  public class RuleParamTypeBind
  {
      public int _Key;
      public int Key { get { return _Key; } }
      public string _Desc;
      public string Desc { get { return _Desc; } }
      public RuleParamTypeBind(IParameterType ript)
      {
          _Key = ript.Key;
          _Desc = ript.CSharpDataType;
      }
  }
  #endregion
}
