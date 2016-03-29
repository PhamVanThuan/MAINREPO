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
using SAHL.Web.UI;

using SAHL.Common.Authentication;
using Microsoft.ApplicationBlocks.UIProcess;


using SAHL.Web.UI.Controls;
using SAHL.Common.Web.UI.Controls;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.Web.UI;


namespace SAHL.Web.Views
{
    public partial class RulesAdministration : SAHLCommonBaseView
    {
        private enum ViewMode
        {
            display,
            update,
            add,
            parameters
        }

        private string holdNewParamType = "";
        private string holdNewDesc = "";
        private string holdNewVal = "";
        private string holdSelectedParameter = "";

        ViewMode m_ViewMode;
//        RulesAdminController m_Controller = null;
        protected override void OnInit(EventArgs e)
        {
            //if (IsPostBack)
            //{
            //    holdNewParamType = Request.Form[cbxParameterType.UniqueID];
            //    holdNewDesc = Request.Form[txtParamDesc.UniqueID];
            //    holdNewVal = Request.Form[txtValue.UniqueID];

            //    holdSelectedParameter = Request.Form[lstParameters.UniqueID];
            //}
            
            //base.OnInit(e);
            
            //m_Controller = base.Controller as RulesAdminController;

           

            //if (!ShouldRunPage())
            //    return;
      
            //BindData();


            //ViewSettings viewSettings = UIPConfiguration.Config.GetViewSettingsFromName(base.ViewName);
            //if (viewSettings.CustomAttributes.Count > 0)
            //{
            //    System.Xml.XmlNode PageStateNode = viewSettings.CustomAttributes.GetNamedItem("state");

            //    if (PageStateNode != null)
            //    {
            //        m_ViewMode = (ViewMode)Enum.Parse(typeof(ViewMode), PageStateNode.Value);
            //    }
            //}

          

            //BindControls();
            
        }

        protected override void OnPreRender(EventArgs e)
        {           

            //btnUpdate.Attributes["OnClick"] = btnUpdate.Attributes["OnClick"] + ";UpdateParameter()";
            base.OnPreRender(e);

           
            

            //switch (m_ViewMode)
            //{
            //    case ViewMode.add:
            //        {
            //            gridRules.Visible = false;                      
            //            panelAddUpdate.Visible = true;
            //            btnDelete.Visible = true;
            //            btnUpdate.Visible = true;
            //            btnNew.Visible = true;
            //            btnSave.Visible = true;
            //            txtParamDesc.Enabled = true;
            //            txtValue.Enabled = true;
            //            cbxParameterType.Enabled = true;
            //            PopulateAddUpdateControls();
            //            break;
            //        }
            //    case ViewMode.display:
            //        {
            //            gridRules.PostBackType = GridPostBackType.SingleClick;
            //            gridRules.Visible = true;                
            //            btnDelete.Visible = false;
            //            btnUpdate.Visible = false;
            //            btnNew.Visible = false;
            //            btnSave.Visible = false;
            //            panelAddUpdate.Visible = false;
            //            txtParamDesc.Enabled = false;
            //            txtValue.Enabled = false;
            //            cbxParameterType.Enabled = false;

            //            break;
            //        }
            //    case ViewMode.update:
            //        {                     
            //            gridRules.Visible = true;
            //            gridRules.PostBackType = GridPostBackType.SingleClick;              
            //            panelAddUpdate.Visible = true;
            //            btnDelete.Visible = true;
            //            btnUpdate.Visible = true;
            //            btnSave.Visible = true;
            //            btnNew.Visible = true;
            //            txtParamDesc.Enabled = true;
            //            txtValue.Enabled = true;
            //            cbxParameterType.Enabled = true;
            //            PopulateAddUpdateControls();                        
            //            break;
            //        }
            //    case ViewMode.parameters:
            //        {
            //            gridRules.Visible = true;
            //            gridRules.PostBackType = GridPostBackType.SingleClick;               
            //            panelAddUpdate.Visible = true;
            //            btnDelete.Visible = false;
            //            btnUpdate.Visible = true;
            //            btnSave.Visible = true;
            //            btnNew.Visible = false;
            //            txtName.Enabled = false;
            //            txtDescription.Enabled = false;
            //            txtParamDesc.Enabled = false;
            //            txtValue.Enabled = true;
            //            cbxParameterType.Enabled = false;
            //            PopulateAddUpdateControls();             
            //            break;
            //        }

            //}
            //RegisterClientScripts();

            //lstParameters.SelectedIndex = 0;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (IsPostBack && Request.Form["__EVENTTARGET"] == btnUpdate.UniqueID)
            //{
            //    if (holdSelectedParameter != "")
            //    {
            //        //Store parameters in list

            //        //try and select the row from the rule parameter table in the dataset and update it if it exists.
            //        SAHL.Common.Datasets.Rules.RuleParameterRow[] rpRow = m_Controller.RulesDS.RuleParameter.Select("RuleParameterKey = '" + holdSelectedParameter.ToString() + "'") as SAHL.Common.Datasets.Rules.RuleParameterRow[];
            //        if (rpRow.Length > 0)
            //        {
            //            if (m_ViewMode != ViewMode.parameters)
            //            {
            //                rpRow[0].RuleParameterTypeKey = int.Parse(holdNewParamType);
            //                rpRow[0].Name = holdNewDesc;
            //            }
            //            rpRow[0].Value = holdNewVal;
            //        }
            //        else
            //        {                        
            //            SAHL.Common.Datasets.Rules.RuleItemRow[] ruleItemRow = m_Controller.RulesDS.RuleItem.Select("RuleItemKey = " + gridRules.Rows[gridRules.SelectedIndex].Cells[0].Text) as SAHL.Common.Datasets.Rules.RuleItemRow[];
            //            SAHL.Common.Datasets.Rules.RuleParameterTypeRow[] ruleParameterRow = m_Controller.RulesDS.RuleParameterType.Select("RuleParameterTypeKey = " + cbxParameterType.Items[cbxParameterType.SelectedIndex].Value)as SAHL.Common.Datasets.Rules.RuleParameterTypeRow[];
            //            m_Controller.RulesDS.RuleParameter.AddRuleParameterRow(
            //                ruleItemRow[0],
            //                txtParamDesc.Text,
            //                ruleParameterRow[0],
            //                holdNewVal
            //                );
            //        }
            //    }
            //    else
            //    {
            //        //Add a new record to the parameter table in the dataset  
            //        SAHL.Common.Datasets.Rules.RuleItemRow[] ruleItemRow = m_Controller.RulesDS.RuleItem.Select("RuleItemKey = " + gridRules.Rows[gridRules.SelectedIndex].Cells[0].Text) as SAHL.Common.Datasets.Rules.RuleItemRow[];
            //        SAHL.Common.Datasets.Rules.RuleParameterTypeRow[] ruleParameterRow = m_Controller.RulesDS.RuleParameterType.Select("RuleParameterTypeKey = " + cbxParameterType.Items[cbxParameterType.SelectedIndex].Value) as SAHL.Common.Datasets.Rules.RuleParameterTypeRow[];
            //        m_Controller.RulesDS.RuleParameter.AddRuleParameterRow(
            //            ruleItemRow[0],
            //            txtParamDesc.Text,
            //            ruleParameterRow[0],
            //            holdNewVal
            //            );

            //    }

            //}
            //lstParameters.Items.Clear();
            //txtValue.Text = "";
            //txtParamDesc.Text = "";
            //cbxParameterType.Text = "";
          
 
            //    lstParameters.Attributes["onclick"] = "LstParamChange()";
            //    gridRules.Attributes["onclick"] = "GridChanged()";
               
            //    AjaxPro.Utility.RegisterTypeForAjax(typeof(RulesAdministration));
        }


        private void BindControls()
        {

            //for (int x = 0; x < m_Controller.RulesDS.RuleParameterType.Rows.Count; x++)
            //{
            //    SAHL.Common.Datasets.Rules.RuleParameterTypeRow r = m_Controller.RulesDS.RuleParameterType.Rows[x] as SAHL.Common.Datasets.Rules.RuleParameterTypeRow;
            //    ListItem pItem = new ListItem();
            //    pItem.Text = r.Description;
            //    pItem.Value = r.RuleParameterTypeKey.ToString();
            //    cbxParameterType.Items.Add(pItem);
            //}

            //switch (m_ViewMode)
            //{
            //    case ViewMode.add:
            //        PopulateAddUpdateControls();
            //        break;
            //    case ViewMode.display:
            //        PopulateGrid();
            //        PopulateDisplayControls();
            //        break;
            //    case ViewMode.update:
            //        PopulateGrid();                 
            //        break;
            //    case ViewMode.parameters:
            //        {
            //            PopulateGrid();
            //            PopulateDisplayControls();
            //            break;
            //        }
            //}
        }


        private void BindData()
        {
            //if (!IsPostBack)
            //{
            //    if (m_Controller.RulesDS == null)
            //        m_Controller.GetRulesAndRuleSets(base.GetClientMetrics());               
            //}
        }


        private void PopulateAddUpdateControls()
        {

            //if (gridRules.Rows.Count > 0)
            //{
            //    txtName.Text = gridRules.Rows[gridRules.SelectedIndex].Cells[1].Text;
            //    txtDescription.Text = gridRules.Rows[gridRules.SelectedIndex].Cells[2].Text;
            //}           
        }

        private void PopulateDisplayControls()
        {
            //if (gridRules.Rows.Count > 0)
            //{
            //    txtName.Text = gridRules.Rows[gridRules.SelectedIndex].Cells[1].Text;
            //    txtDescription.Text = gridRules.Rows[gridRules.SelectedIndex].Cells[2].Text;
            //}

            //cbxParameterType.Items.Clear();
            //for (int x = 0; x < m_Controller.RulesDS.RuleParameterType.Rows.Count; x++)
            //{
            //    SAHL.Common.Datasets.Rules.RuleParameterTypeRow r = m_Controller.RulesDS.RuleParameterType.Rows[x] as SAHL.Common.Datasets.Rules.RuleParameterTypeRow;
            //    ListItem pItem = new ListItem();
            //    pItem.Text = r.Description;
            //    pItem.Value = r.RuleParameterTypeKey.ToString();
            //    cbxParameterType.Items.Add(pItem);
            //}
            //cbxParameterType.SelectedIndex = -1;
        }

        private void PopulateGrid()
        {
            //gridRules.DataSource = m_Controller.RulesDS.RuleItem;
            //gridRules.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //switch (m_ViewMode)
            //{
            //    case ViewMode.add:
            //        {
            //            SAHL.Common.Datasets.Rules.RuleItemDataTable tblRuleItem = m_Controller.RulesDS.RuleItem;
            //            SAHL.Common.Datasets.Rules.RuleItemRow mRuleSetRow = tblRuleItem.AddRuleItemRow(txtName.Text, txtDescription.Text);
            //            break;
            //        }
            //    case ViewMode.update:
            //        {
            //            SAHL.Common.Datasets.Rules.RuleItemDataTable tblRuleItem = m_Controller.RulesDS.RuleItem;
            //            SAHL.Common.Datasets.Rules.RuleItemRow[] mRuleItemRow = tblRuleItem.Select("RuleItemKey = " + gridRules.SelectedRow.Cells[0].Text) as SAHL.Common.Datasets.Rules.RuleItemRow[];
            //            mRuleItemRow[0].Name = txtName.Text;
            //            mRuleItemRow[0].Description = txtDescription.Text;
            //            break;
            //        }
            //    case ViewMode.parameters:
            //        {
            //            SAHL.Common.Datasets.Rules.RuleItemDataTable tblRuleItem = m_Controller.RulesDS.RuleItem;
            //            SAHL.Common.Datasets.Rules.RuleItemRow[] mRuleItemRow = tblRuleItem.Select("RuleItemKey = " + gridRules.SelectedRow.Cells[0].Text) as SAHL.Common.Datasets.Rules.RuleItemRow[];
            //            mRuleItemRow[0].Name = txtName.Text;
            //            mRuleItemRow[0].Description = txtDescription.Text;
            //            break;
            //        }
            //}
            //m_Controller.UpdateRulesAndRuleSets(base.GetClientMetrics());  

        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            //txtParamDesc.Text = "";
            //txtValue.Text = "";
            //cbxParameterType.SelectedIndex = -1;            
        }                     

       



        private void RegisterClientScripts()
        {
            //StringBuilder mBuilder = new StringBuilder();         
            //mBuilder.AppendLine("var items = new Array();");
            //mBuilder.AppendLine("var newItem = false;");

            //mBuilder.AppendLine("function item(desc,type,val)");
            //mBuilder.AppendLine("{");
            //mBuilder.AppendLine("this.desc = desc;");
            //mBuilder.AppendLine("this.type = type;");
            //mBuilder.AppendLine("this.val = val;");
            //mBuilder.AppendLine("}");


            //if (gridRules.Rows.Count > 0)
            //    if (m_ViewMode == ViewMode.update || m_ViewMode == ViewMode.display || m_ViewMode == ViewMode.parameters)
            //    {
            //        lstParameters.Items.Clear();
            //        bool firstRec = true;
            //        int addedCount = 0;
            //        for (int x = 0; x < m_Controller.RulesDS.RuleParameter.Rows.Count; x++)
            //        {
            //            SAHL.Common.Datasets.Rules.RuleParameterRow r = m_Controller.RulesDS.RuleParameter.Rows[x] as SAHL.Common.Datasets.Rules.RuleParameterRow;

            //            if (r.RuleItemKey.ToString() == gridRules.Rows[gridRules.SelectedIndex].Cells[0].Text)
            //            {
            //                SAHL.Common.Datasets.Rules.RuleParameterTypeRow[] rpRow = m_Controller.RulesDS.RuleParameterType.Select("RuleParameterTypeKey = " + r.RuleParameterTypeKey.ToString()) as SAHL.Common.Datasets.Rules.RuleParameterTypeRow[];
            //                string paramType = rpRow[0].RuleParameterTypeKey.ToString();
            //                ListItem mItem = new ListItem();
            //                mItem.Text = r.Name;
            //                mItem.Value = r.RuleParameterKey.ToString();
            //                lstParameters.Items.Add(mItem);
            //                mBuilder.AppendLine("var itm = new item('" + r.Name + "','" + r.RuleParameterTypeKey + "','" + r.Value + "');");
            //                mBuilder.AppendLine("items[" + addedCount.ToString() + "] = itm;");
            //                addedCount++;
            //                if (firstRec)
            //                {
            //                    firstRec = false;
            //                    txtParamDesc.Text = r.Name;
            //                    for (int y = 0; y < cbxParameterType.Items.Count; y++)
            //                    {
            //                        if (cbxParameterType.Items[y].Value == paramType)
            //                        {
            //                            cbxParameterType.SelectedIndex = y;
            //                            break;
            //                        }
            //                    }
            //                    txtValue.Text = r.Value;

            //                    OriginalName.Value = r.Name;
            //                    OriginalType.Value = paramType;
            //                    OriginalValue.Value = r.Value;
            //                }
            //            }
            //        }
            //    }
        
            //mBuilder.AppendLine("function LstParamChange()");
            //mBuilder.AppendLine("{");
            //mBuilder.AppendLine("var p = document.getElementById('" + lstParameters.ClientID + "');");
            //mBuilder.AppendLine("var desc = document.getElementById('" + txtParamDesc.ClientID + "');");
            //mBuilder.AppendLine("var type = document.getElementById('" + cbxParameterType.ClientID + "');");
            //mBuilder.AppendLine("var paramVal = document.getElementById('" + txtValue.ClientID + "');");
            //mBuilder.AppendLine("var origName = document.getElementById('" + OriginalName.ClientID + "');");     
            //mBuilder.AppendLine("var origType = document.getElementById('" + OriginalType.ClientID + "');");
            //mBuilder.AppendLine("var origVal = document.getElementById('" + OriginalValue.ClientID + "');");                                   
            //mBuilder.AppendLine("var idx = p.selectedIndex;");           
            //mBuilder.AppendLine("desc.value = items[idx].desc;");
            //mBuilder.AppendLine("type.value = items[idx].type;");
            //mBuilder.AppendLine("paramVal.value = items[idx].val;");

            //mBuilder.AppendLine("origName.value = items[idx].desc;");
            //mBuilder.AppendLine("origType.value = items[idx].type;");
    
            //mBuilder.AppendLine("origVal.value = items[idx].val;");
       
            //mBuilder.AppendLine("newItem = false;");
            //mBuilder.AppendLine("}");


            //mBuilder.AppendLine("function NewParameter()");
            //mBuilder.AppendLine("{");
            //mBuilder.AppendLine("var p = document.getElementById('" + lstParameters.ClientID + "');");
            //mBuilder.AppendLine("var desc = document.getElementById('" + txtParamDesc.ClientID + "');");
            //mBuilder.AppendLine("var type = document.getElementById('" + cbxParameterType.ClientID + "');");
            //mBuilder.AppendLine("var paramVal = document.getElementById('" + txtValue.ClientID + "');");          
            //mBuilder.AppendLine("desc.value = '';");
            //mBuilder.AppendLine("type.value = '';");
            //mBuilder.AppendLine("paramVal.value = '';");
            //mBuilder.AppendLine("newItem = true;");
            //mBuilder.AppendLine("}");


            //mBuilder.AppendLine("function UpdateParameter()");
            //mBuilder.AppendLine("{");
         
            //mBuilder.AppendLine("var origName = document.getElementById('" + OriginalName.ClientID + "');");
          

          
            //mBuilder.AppendLine("var p = document.getElementById('" + lstParameters.ClientID + "');");
            //mBuilder.AppendLine("var desc = document.getElementById('" + txtParamDesc.ClientID + "');");
            //mBuilder.AppendLine("var type = document.getElementById('" + cbxParameterType.ClientID + "');");
            //mBuilder.AppendLine("var paramVal = document.getElementById('" + txtValue.ClientID + "');");
            //mBuilder.AppendLine("var cnt = p.length;");
            //mBuilder.AppendLine("if(newItem == true || items.length == 0 || p.selectedIndex == -1)");
            //mBuilder.AppendLine("{");           
            //mBuilder.AppendLine("var itm = new item(desc.value,type.value,paramVal.value);");
            //mBuilder.AppendLine("items[cnt] = itm;");           
            //mBuilder.AppendLine("var o = document.createElement('option');");
            //mBuilder.AppendLine("o.text = items[cnt].desc;");
            //mBuilder.AppendLine("o.value = -1;"); 
            //mBuilder.AppendLine("p.options.add(o);");
            //mBuilder.AppendLine("p.selectedIndex = p.length-1;");
            //mBuilder.AppendLine("newItem = false;");
            //mBuilder.AppendLine("}");
            //mBuilder.AppendLine("else");
            //mBuilder.AppendLine("{");
            //mBuilder.AppendLine("var idx = p.selectedIndex;");
            //mBuilder.AppendLine("items[idx].desc = desc.value;");
            //mBuilder.AppendLine("items[idx].type = type.value;");
            //mBuilder.AppendLine("items[idx].val = paramVal.value;");
            //mBuilder.AppendLine("var mItem = p[idx];");
            //mBuilder.AppendLine("mItem.text = desc.value;");
            //mBuilder.AppendLine("}");  
            //mBuilder.AppendLine("}");

            //mBuilder.AppendLine("function DeleteParameter()");
            //mBuilder.AppendLine("{");
            //mBuilder.AppendLine("var p = document.getElementById('" + lstParameters.ClientID + "');");
            //mBuilder.AppendLine("var desc = document.getElementById('" + txtParamDesc.ClientID + "');");
            //mBuilder.AppendLine("var type = document.getElementById('" + cbxParameterType.ClientID + "');");
            //mBuilder.AppendLine("var paramVal = document.getElementById('" + txtValue.ClientID + "');");
            //mBuilder.AppendLine("if(p.selectedIndex > -1)");
            //mBuilder.AppendLine("{");
            //mBuilder.AppendLine("desc.value = '';");
            //mBuilder.AppendLine("type.selectedIndex = -1;");
            //mBuilder.AppendLine("paramVal.value = '';");
            //mBuilder.AppendLine("p.options.remove(p.selectedIndex);");
            //mBuilder.AppendLine("if (p.length > 0)");
            //mBuilder.AppendLine("{");
            //mBuilder.AppendLine("p.selectedIndex = (p.length - 1);");
            //mBuilder.AppendLine("LstParamChange();");
            //mBuilder.AppendLine("}");
            //mBuilder.AppendLine("else");
            //mBuilder.AppendLine("{");
            //mBuilder.AppendLine("newItem = true;");
            //mBuilder.AppendLine("}");
            //mBuilder.AppendLine("}");
            //mBuilder.AppendLine("}");

            //if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "lstScripts"))
            //{
            //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "lstScripts", mBuilder.ToString(), true);
            //}
        }

        protected void gridRules_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnUpdate_Click1(object sender, EventArgs e)
        {

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
           //SAHL.Common.Datasets.Rules.RuleParameterRow[] r = m_Controller.RulesDS.RuleParameter.Select("RuleParameterKey = " + holdSelectedParameter) as SAHL.Common.Datasets.Rules.RuleParameterRow[];
           //m_Controller.RulesDS.RuleParameter.Rows.Remove(r[0]);
        }

       
    }

    
}
