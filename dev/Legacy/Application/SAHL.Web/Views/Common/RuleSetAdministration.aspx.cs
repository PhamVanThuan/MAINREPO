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
    public partial class RuleSetAdministration : SAHLCommonBaseView
    {
        private enum ViewMode
        {
            display,
            update,
            add
        }

    

        ViewMode m_ViewMode;
    //    RulesAdminController m_RulesController = null;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
      //      m_RulesController = base.Controller as RulesAdminController;

      //      if (!ShouldRunPage())
      //          return;

            

      //      BindData();

      //      ViewSettings viewSettings = UIPConfiguration.Config.GetViewSettingsFromName(base.ViewName);
      //      if (viewSettings.CustomAttributes.Count > 0)
      //      {
      //          System.Xml.XmlNode PageStateNode = viewSettings.CustomAttributes.GetNamedItem("state");

      //          if (PageStateNode != null)
      //          {
      //              m_ViewMode = (ViewMode)Enum.Parse(typeof(ViewMode), PageStateNode.Value);
      //          }
      //      }
           
      //  //    if (!IsPostBack)
      //  //    {
      //          BindControls();
      ////      }

          
        }

        private void BindData()
        {
            //if (!IsPostBack)
            //{
            //    if (m_RulesController.RulesDS == null)
            //        m_RulesController.GetRulesAndRuleSets(base.GetClientMetrics());
            //    lstSelectedRules.Items.Clear();
            //    m_RulesController.SelectedRules = new List<ListItem>();
            //}
        }

        private void BindControls()
        {
            // decide which controls to bind

            // always bind the datagrid


            //switch (m_ViewMode)
            //{
            //    case ViewMode.add:
            //        PopulateAddControls();
            //        break;
            //    case ViewMode.display:
            //        PopulateGrid();
            //        PopulateDisplayControls();
            //        break;
            //    case ViewMode.update:
            //        PopulateGrid();
            //        PopulateUpdateControls();
            //        break;
            //}
        }


        private void PopulateUpdateControls()
        {
            //lstAvailableRules.Items.Clear();
            //lstSelectedRules.Items.Clear();

            //txtRuleSetName.Text = GridAvailableRuleSets.SelectedRow.Cells[1].Text;
            //txtRuleDescription.Text = GridAvailableRuleSets.SelectedRow.Cells[2].Text;

            //if (m_RulesController.SelectedRules == null)
            //{
            //    m_RulesController.SelectedRules = new List<ListItem>();
            //}
            //else
            //{
            //    m_RulesController.SelectedRules.Clear();
            //}

            //if (m_RulesController.RulesDS.RuleItem.Rows.Count > 0)
            //{
            //    for (int x = 0; x < m_RulesController.RulesDS.RuleItem.Rows.Count; x++)
            //    {
            //        ListItem itm = new ListItem();
            //        itm.Text = m_RulesController.RulesDS.RuleItem[x].Name;
            //        itm.Value = m_RulesController.RulesDS.RuleItem[x].RuleItemKey.ToString();
            //        if (!m_RulesController.SelectedRules.Contains(itm))
            //        {
            //            lstAvailableRules.Items.Add(itm);
            //        }
            //    }
            //}

           
            //SAHL.Common.Datasets.Rules.RuleSetRuleRow[] rsrRow = m_RulesController.RulesDS.RuleSetRule.Select("RuleSetKey = " + GridAvailableRuleSets.SelectedRow.Cells[0].Text) as SAHL.Common.Datasets.Rules.RuleSetRuleRow[];
            //for (int x = 0; x < rsrRow.Length; x++)
            //{
            //    for (int y = 0; y < lstAvailableRules.Items.Count; y++)
            //    {
            //        if (rsrRow[x].RuleItemKey == int.Parse(lstAvailableRules.Items[y].Value))
            //        {
            //            ListItem selItem = lstAvailableRules.Items[y];
            //            selItem.Selected = rsrRow[x].EnForceRule;
            //            m_RulesController.SelectedRules.Add(selItem);
            //            lstAvailableRules.Items.RemoveAt(y);                     
            //        }
            //    }

            //}

           
            //for (int x = 0; x < m_RulesController.SelectedRules.Count; x++)
            //{
            //    lstSelectedRules.Items.Add(m_RulesController.SelectedRules[x]);
            //}
        }

        private void PopulateAddControls()
        {            
                //if (m_RulesController.RulesDS.RuleItem.Rows.Count > 0)
                //{
                //    for (int x = 0; x < m_RulesController.RulesDS.RuleItem.Rows.Count; x++)
                //    {
                //        ListItem itm = new ListItem();
                //        itm.Text = m_RulesController.RulesDS.RuleItem[x].Name;
                //        itm.Value = m_RulesController.RulesDS.RuleItem[x].RuleItemKey.ToString();
                //        if (!m_RulesController.SelectedRules.Contains(itm))
                //        {
                //            lstAvailableRules.Items.Add(itm);
                //        }
                //    }                   
                //}
                //lstSelectedRules.Items.Clear();
                //for (int x = 0; x < m_RulesController.SelectedRules.Count; x++)
                //{
                //    lstSelectedRules.Items.Add(m_RulesController.SelectedRules[x]);
                //}            
        }


        private void PopulateDisplayControls()
        {
        //    lstRules.Items.Clear();
        //    if (GridAvailableRuleSets.Rows.Count == 0)
        //    {
        //        return;
        //    }

        //    SAHL.Common.Datasets.Rules.RuleSetRuleRow[] ruleRows = m_RulesController.RulesDS.RuleSetRule.Select("RuleSetKey = " + GridAvailableRuleSets.SelectedRow.Cells[0].Text) as SAHL.Common.Datasets.Rules.RuleSetRuleRow[];
        //    for (int x = 0; x < ruleRows.Length; x++)
        //    {
        //        ListItem mItem = new ListItem();
        //        mItem.Text = ruleRows[x].RuleItemRow.Name + " - " + ruleRows[x].RuleItemRow.Description;
        //        lstRules.Items.Add(mItem);
        //    }
        }

        private void PopulateGrid()
        {
            //GridAvailableRuleSets.DataSource = m_RulesController.RulesDS.RuleSet;
            //GridAvailableRuleSets.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    RegisterClientScripts();
            //}
        }

        protected override void OnPreRender(EventArgs e)
        {
            //base.OnPreRender(e);
            //// decide what to show
            //switch (m_ViewMode)
            //{
            //    case ViewMode.add:
            //        divRuleSetView.Visible = false;
            //        tblRuleSetUpdate.Visible = true;
            //        GridAvailableRuleSets.Visible = false;
            //        lstAvailableRules.Visible = true;
            //        lstSelectedRules.Visible = true;
            //        break;
            //    case ViewMode.display:
            //        divRuleSetView.Visible = true;
            //        tblRuleSetUpdate.Visible = false;
            //        GridAvailableRuleSets.Visible = true;

            //        break;
            //    case ViewMode.update:
            //        divRuleSetView.Visible = false;
            //        tblRuleSetUpdate.Visible = true;

            //        break;
            //}
           
        }

        protected void GridAvailableRuleSets_SelectedIndexChanged(object sender, EventArgs e)
        {
            //switch (m_ViewMode)
            //{
            //    case ViewMode.display:
            //        {
            //            PopulateDisplayControls();
            //            break;
            //        }
            //    case ViewMode.update:
            //        {
            //            PopulateUpdateControls();
            //            break;
            //        }
            //}
        }


        private void RegisterClientScripts()
        {
            StringBuilder mBuilder = new StringBuilder();

//            mBuilder.AppendLine("function addItem()");
//            mBuilder.AppendLine("{");           
//            mBuilder.AppendLine("var listAvailableItems = document.getElementById(\"" + lstAvailableRules.ClientID + "\");");
//            mBuilder.AppendLine("var listSelectedItems = document.getElementById(\"" + lstSelectedRules.ClientID + "\");");
//       //     mBuilder.AppendLine("if(listAvailableItems[0].value != \"\")");
//      //      mBuilder.AppendLine(" {	 ");
////            mBuilder.AppendLine("var selopt = listAvailableItems[listAvailableItems.selectedIndex];");
//            mBuilder.AppendLine("listAvailableItems.options.remove(listAvailableItems.selectedIndex);");
//            mBuilder.AppendLine("var selopt = document.createElement('OPTION');");
//            mBuilder.AppendLine("selopt.text = 'Hello';");
//            mBuilder.AppendLine("selopt.value = false;");
//            mBuilder.AppendLine("alert(selopt.value);");
//            mBuilder.AppendLine("listSelectedItems.options.add(selopt);");           
//        //    mBuilder.AppendLine(" }  ");
//            mBuilder.AppendLine(" }  ");

         
          


//            mBuilder.AppendLine("function removeItem()");
//            mBuilder.AppendLine("{");
//            mBuilder.AppendLine("var listSelectedItems = document.getElementById(\"" + lstSelectedRules.ClientID + "\");");
//            mBuilder.AppendLine("if(listSelectedItems[0].value != \"\")");
//            mBuilder.AppendLine("   {	 ");
//            mBuilder.AppendLine("var listAvailableItems = document.getElementById(\"" + lstAvailableRules.ClientID + "\");");

//            mBuilder.AppendLine("var selopt = listSelectedItems[listSelectedItems.selectedIndex];");
//            mBuilder.AppendLine("listSelectedItems.options.remove(listSelectedItems.selectedIndex);");
//            mBuilder.AppendLine("listAvailableItems.options.add(selopt);");                        
//            mBuilder.AppendLine("}");
         
           
//            mBuilder.AppendLine("}");

           

//            mBuilder.AppendLine("function ClearLists()");
//            mBuilder.AppendLine("{");
//            mBuilder.AppendLine("var listAvailableItems = document.getElementById(\"" + lstAvailableRules.ClientID + "\");");
//            mBuilder.AppendLine("var x=0;");
//            mBuilder.AppendLine("for(x=listAvailableItems.length;x>-1;x--)");
//            mBuilder.AppendLine("{");
//            mBuilder.AppendLine("listAvailableItems.options.remove(x);");
//            mBuilder.AppendLine("}");
//            mBuilder.AppendLine("var listSelectedItems = document.getElementById(\"" + lstSelectedRules.ClientID + "\");");
//            mBuilder.AppendLine("for(x=listSelectedItems.length;x>-1;x--)");
//            mBuilder.AppendLine("{");
//            mBuilder.AppendLine("listSelectedItems.options.remove(x);");
//            mBuilder.AppendLine("}");
//            mBuilder.AppendLine("}");
       

            //if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "lstScripts"))
            //{
            //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "lstScripts", mBuilder.ToString(), true);
            //}
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            //if (lstAvailableRules.SelectedItem != null)
            //{
            //    ListItem item = lstAvailableRules.SelectedItem;
            //    ListItem newItem = new ListItem(item.Text, item.Value);

            //    if (!m_RulesController.SelectedRules.Contains(newItem))
            //    {
            //        m_RulesController.SelectedRules.Add(newItem);
            //        lstSelectedRules.Items.Add(newItem);
            //        lstAvailableRules.Items.RemoveAt(lstAvailableRules.SelectedIndex);
            //    }              
            //}
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            //if (lstSelectedRules.SelectedItem != null)
            //{
            //    for (int x = 0; x < m_RulesController.SelectedRules.Count; x++)
            //    {
            //        if (m_RulesController.SelectedRules[x] == lstSelectedRules.SelectedItem)
            //        {
            //            m_RulesController.SelectedRules.RemoveAt(x);
            //            break;
            //        }
            //    }
            //    lstAvailableRules.Items.Add(lstSelectedRules.SelectedItem);
            //    lstSelectedRules.Items.RemoveAt(lstSelectedRules.SelectedIndex);
            //}
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
        //    switch (m_ViewMode)
        //    {
        //        case ViewMode.add:
        //            {
        //                SAHL.Common.Datasets.Rules.RuleSetDataTable tblRuleSet = m_RulesController.RulesDS.RuleSet;
        //                SAHL.Common.Datasets.Rules.RuleSetRow mRuleSetRow = tblRuleSet.AddRuleSetRow(txtRuleSetName.Text, txtRuleDescription.Text);

        //                SAHL.Common.Datasets.Rules.RuleSetRuleDataTable tblRuleSetRule = m_RulesController.RulesDS.RuleSetRule;
        //                for (int x = 0; x < lstSelectedRules.Items.Count; x++)
        //                {
        //                    SAHL.Common.Datasets.Rules.RuleItemRow[] mRuleItemRow = m_RulesController.RulesDS.RuleItem.Select("RuleItemKey = " + lstSelectedRules.Items[x].Value) as SAHL.Common.Datasets.Rules.RuleItemRow[];
        //                    tblRuleSetRule.AddRuleSetRuleRow(mRuleSetRow, mRuleItemRow[0], lstSelectedRules.Items[x].Selected);
        //                }
        //                break;
        //            }
        //        case ViewMode.update:
        //            {
        //                SAHL.Common.Datasets.Rules.RuleSetDataTable tblRuleSet = m_RulesController.RulesDS.RuleSet;
        //                SAHL.Common.Datasets.Rules.RuleSetRow[] mRuleSetRow = tblRuleSet.Select("RuleSetKey = " + GridAvailableRuleSets.SelectedRow.Cells[0].Text) as SAHL.Common.Datasets.Rules.RuleSetRow[];
        //                mRuleSetRow[0].Name = txtRuleSetName.Text;
        //                mRuleSetRow[0].Description = txtRuleDescription.Text;

        //                SAHL.Common.Datasets.Rules.RuleSetRuleDataTable tblRuleSetRule = m_RulesController.RulesDS.RuleSetRule;
        //                tblRuleSetRule.Rows.Clear();
        //                for (int x = 0; x < lstSelectedRules.Items.Count; x++)
        //                {
        //                    SAHL.Common.Datasets.Rules.RuleItemRow[] mRuleItemRow = m_RulesController.RulesDS.RuleItem.Select("RuleItemKey = " + lstSelectedRules.Items[x].Value) as SAHL.Common.Datasets.Rules.RuleItemRow[];
        //                    tblRuleSetRule.AddRuleSetRuleRow(mRuleSetRow[0], mRuleItemRow[0], lstSelectedRules.Items[x].Selected);
        //                }

        //                break;
        //            }
        //    }
        //    m_RulesController.UpdateRulesAndRuleSets(base.GetClientMetrics());
        }
    }

    public class SelectedItem
    {
        private ListItem m_Item;
        private bool m_Enforce;

        public ListItem item
        {
            get
            {
                return m_Item;
            }
            set
            {
                m_Item = value;
            }
        }

        public bool Enforce
        {
            get
            {
                return m_Enforce;
            }
            set
            {
                m_Enforce = value;
            }
        }
    }

}
