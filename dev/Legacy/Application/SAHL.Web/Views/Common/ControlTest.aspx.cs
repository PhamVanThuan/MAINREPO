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
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using DevExpress.Web.ASPxTreeList;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Common
{
    public partial class ControlTest : SAHLCommonBaseView, IControlTest
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!ShouldRunPage) return;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ShouldRunPage) 
                return;
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (!ShouldRunPage)
                return;

            base.OnPreRender(e);
        }

        #region Interface Members

        public event EventHandler RemoveButtonClicked;

        public event TreeListNodeDragEventHandler TreeNodeDragged;

        public void BindOrganisationStructure(DataSet orgStructLst)
        {
            tlOrgStructure.KeyFieldName = "PrimaryKey";
            tlOrgStructure.ParentFieldName = "ParentKey";
            tlOrgStructure.DataSource = orgStructLst;
            tlOrgStructure.DataBind();

            ASPxTreeList1.KeyFieldName = "PrimaryKey";
            ASPxTreeList1.ParentFieldName = "ParentKey";
            ASPxTreeList1.DataSource = orgStructLst;
            ASPxTreeList1.DataBind();

            #region SAHL Grid

            DXGridViewFormattedTextColumn col = new DXGridViewFormattedTextColumn();
            col.FieldName = "PrimaryKey";
            col.Caption = col.FieldName;
            col.Width = Unit.Pixel(100);
            col.Visible = false;
            gvSAHL.Columns.Add(col);

            col = new DXGridViewFormattedTextColumn();
            col.FieldName = "OSDescription";
            col.Caption = col.FieldName;
            col.Width = Unit.Pixel(100);
            col.Visible = true;
            gvSAHL.Columns.Add(col);

            col = new DXGridViewFormattedTextColumn();
            col.FieldName = "OSTypeDescription";
            col.Caption = col.FieldName;
            col.Width = Unit.Pixel(100);
            col.Visible = true;
            gvSAHL.Columns.Add(col);

            col = new DXGridViewFormattedTextColumn();
            col.FieldName = "DisplayName";
            col.Caption = col.FieldName;
            col.Width = Unit.Pixel(100);
            col.Visible = true;
            gvSAHL.Columns.Add(col);

            gvSAHL.KeyFieldName = "PrimaryKey";
            gvSAHL.DataSource = orgStructLst;
            gvSAHL.DataBind();

            #endregion

            #region Dev Express Grid


            gvDevExpress.KeyFieldName = "PrimaryKey";
            gvDevExpress.DataSource = orgStructLst;
            gvDevExpress.DataBind();


            #endregion
        }

        public DataRow GetFocusedNode
        {
            get
            {
                if (tlOrgStructure.FocusedNode != null)
                {
                    DataRowView drv = tlOrgStructure.FocusedNode.DataItem as DataRowView;
                    if (drv != null)
                        return drv.Row;
                }
                return null;
            }
        }



        #endregion

        protected void ProcessDragNode(object sender, TreeListNodeDragEventArgs e)
        {
            //if (TreeNodeDragged != null)
            //    TreeNodeDragged(sender, e);

            try
            {
                if (chkThowError.Checked)
                {
                    this.Messages.Add(new DomainMessage("Drag Error", "Drag Error"));
                    throw new Exception();
                }

                ASPxTreeList1.RefreshVirtualTree();
            }
            catch (Exception)
            {
                if (this.IsValid)
                    throw;

                //e.Cancel = true;
                //this.OnPreRender(null);

                //this.ASPxTreeList1.CancelEdit();
            }
            finally
            {
            }

            if (chkDevExpressDragHandled.Checked)
                e.Handled = true;

        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            //if (RemoveButtonClicked != null)
            //    RemoveButtonClicked(sender, e);

            try
            {
                this.Messages.Add(new DomainMessage("Delete Error", "Delete Error"));
                throw new Exception();
            }
            catch (Exception)
            {
                if (this.IsValid)
                    throw;
            }
            finally
            {
            }
        }

        protected void ASPxTreeList1_NodeUpdated(object sender, DevExpress.Web.Data.ASPxDataUpdatedEventArgs e)
        {
            //System.Collections.Specialized.OrderedDictionary dicOld = e.OldValues;
            //System.Collections.Specialized.OrderedDictionary dicNew = e.NewValues;
        }

        protected void ASPxTreeList1_NodeInserted(object sender, DevExpress.Web.Data.ASPxDataInsertedEventArgs e)
        {
            //System.Collections.Specialized.OrderedDictionary dic = e.NewValues;

        }

        protected void ASPxTreeList1_NodeDeleted(object sender, DevExpress.Web.Data.ASPxDataDeletedEventArgs e)
        {
            //System.Collections.Specialized.OrderedDictionary dic = e.Values;

        }

        protected void ASPxTreeList1_NodeValidating(object sender, TreeListNodeValidationEventArgs e)
        {
            //System.Collections.Specialized.OrderedDictionary dicOld = e.OldValues;
            //System.Collections.Specialized.OrderedDictionary dicNew = e.NewValues;

        }

    }
}
