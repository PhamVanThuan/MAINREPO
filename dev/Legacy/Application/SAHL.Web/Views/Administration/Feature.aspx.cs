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
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Administration.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.Administration
{
  public partial class Feature : SAHLCommonBaseView, SAHL.Web.Views.Administration.Interfaces.IViewFeature
  {
  //  protected void Page_Load(object sender, EventArgs e)
  //  {
  //    if (!ShouldRunPage) return;
  //}

  #region Treebind
      public void BindFeatureTree(List<IBindableTreeItem> Features)
      {
          // loop through all the features and add them as we go along. Once we add them, add their kids if they have

          foreach (IBindableTreeItem f in Features)
          {
              // add the top level node if no parent else add child
              if (f.ParentKey < 0)
              {
                  AddTopLevelNode(f);
              }
              else
              {
                  bool Found = false;
                  foreach (SAHLTreeNode node in tv.Nodes)
                  {
                      AddChildNode(f, node, ref Found);
                      if (Found)
                          break;
                  }
              }

              // now we have added the parent we can add their kids by recursing back in here.
              if (f.Children.Count > 0)
              {
                  BindFeatureTree(f.Children);
              }
          }
      }

      private void AddTopLevelNode(IBindableTreeItem o)
      {
          SAHLTreeNode tn = new SAHLTreeNode(o.Desc, o.Key.ToString());
          tv.Nodes.Add(tn);
      }

      private void AddChildNode(IBindableTreeItem o, SAHLTreeNode node, ref bool Found)
      {
          // if we found it inside the recurse then get outta here.
          if (Found) return;
          // loop through all the nodes that have been added so far and find the one that is 
          // the parent of the node we are trying to add. Once we find it add it to the newly
          // found nodes children.

          // recurse the children
          foreach (SAHLTreeNode Childnode in node.Nodes)
          {
              if (Childnode.Nodes.Count > 0)
              {
                  AddChildNode(o, Childnode, ref Found);
              }
              else
              {
                  if (Childnode.Value == o.ParentKey.ToString())
                  {
                      SAHLTreeNode newnode = new SAHLTreeNode(o.Desc, o.Key.ToString());
                      Childnode.Nodes.Add(newnode);
                      Found = true;
                      return;
                  }
              }
              //node.Nodes ??? somehow we GetDataItem this rught
          }
          if (node.Value == o.ParentKey.ToString())
          {
              node.Nodes.Add(new SAHLTreeNode(o.Desc, o.Key.ToString()));
              Found = true;
              return;
          }

      }

  #endregion

      protected void btn_Click(object sender, EventArgs e)
      {
          if (null != OnSubmitClick)
          {
              OnSubmitClick(null, null);
          }
      }

      protected void btnClear_Click(object sender, EventArgs e)
      {
          txtParent.Text = "";
          hdParent.Value = "-1";
      }

      protected void tv_NodeSelected(object source, SAHL.Common.Web.UI.Events.SAHLTreeNodeEventArgs e)
      {
          if (null != OnTreeSelected)
          {
              int SelectedKey = Convert.ToInt32(e.TreeNode.Value);
              OnTreeSelected(source, new KeyChangedEventArgs(SelectedKey));
          }
      }
      #region IView
      /// <summary>
      /// 
      /// </summary>
      public bool VisibleMaint { set { tblMaint.Visible = value; } }
      /// <summary>
      /// 
      /// </summary>
      public bool VisibleButtons { set { btnClear.Visible = value; btn.Visible = value; } }
      /// <summary>
      /// 
      /// </summary>
      public bool VisibleFeatureList { set { tblFeatureList.Visible = value; } }
      /// <summary>
      /// 
      /// </summary>
      public string LongName { get { return txtLong.Text; } }
      /// <summary>
      /// 
      /// </summary>
      public string ShortName { get { return txtFeature.Text; } }
      /// <summary>
      /// 
      /// </summary>
      public int Key { get { return Convert.ToInt32(hdKey.Value); } }

      /// <summary>
      /// 
      /// </summary>
      public int ParentKey { get { return Convert.ToInt32(hdParent.Value); } }
      /// <summary>
      /// 
      /// </summary>
      public int Sequence { get { return Convert.ToInt32(txtSeq.Text); } }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="FeatureList"></param>
      public void BindFeatureList(IEventList<IFeature> FeatureList)
      {
          List<BindableFeature> Bind = new List<BindableFeature>();
          foreach (IFeature f in FeatureList)
          {
              Bind.Add(new BindableFeature(f, false));
          }
          Bind.Sort(new BindableFeature());
          ddlFeature.DataSource = Bind;
          ddlFeature.DataTextField = "ShortName";
          ddlFeature.DataValueField = "Key";
          ddlFeature.DataBind();
      }
      /// <summary>
      /// 
      /// </summary>
      public event EventHandler OnSubmitClick;
      /// <summary>
      /// 
      /// </summary>
      public event EventHandler OnTreeSelected;
      /// <summary>
      /// 
      /// </summary>
      public event EventHandler OnFeatureSelectedItemChanged;
      public void BindParent(int Key, string Desc)
      {
          txtParent.Text = Desc;
          hdParent.Value = Key.ToString();
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="feature"></param>
      public void BindFeature(BindableFeature feature)
      {
          // go find the selected parent in the tree
          txtFeature.Text = feature.ShortName;
          hdKey.Value = feature.Key.ToString();
          txtParent.Text = feature.ParentShort;
          hdParent.Value = feature.ParentKey.ToString();
          txtSeq.Text = feature.Sequence.ToString();
          txtLong.Text = feature.LongName;
      }

      #endregion

      protected void ddlFeature_SelectedIndexChanged(object sender, EventArgs e)
      {
          if (null != OnFeatureSelectedItemChanged)
          {
              OnFeatureSelectedItemChanged(null, new KeyChangedEventArgs(ddlFeature.SelectedValue));
          }
      }
  }
}
