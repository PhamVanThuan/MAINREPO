using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Utils;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.DomainMessages;
using System.Diagnostics.CodeAnalysis;

namespace SAHL.Web.Views.Common
{
	public partial class PropertyCaptureSearch : SAHLCommonBaseView
	{
		public event KeyChangedEventHandler OnSearchButtonClicked;
		public event KeyChangedEventHandler OnPropertiesGridSelectedIndexChanged;
		public event KeyChangedEventHandler OnPropertiesGridDoubleClick;

		private string _sellerID;

		public string SellerID
		{
			set
			{
				_sellerID = value;
			}
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			if (!ShouldRunPage)
				return;

		}

		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			txtIdentity.Text = _sellerID;

			if (PropertiesGrid.Rows.Count > 0 && PropertiesGrid.SelectedIndex > -1)
			{
				lblSelectedProperty.Text = PropertiesGrid.Rows[PropertiesGrid.SelectedIndex].Cells[2].Text;
			}
			else
			{
				lblSelectedProperty.Text = "";
			}

		}

		public void BindPropertiesGrid(DataTable DT)
		{
			//PropertiesGrid.AddGridBoundColumn("", "PropertyID", Unit.Percentage(10), HorizontalAlign.Left, true);
			PropertiesGrid.AddGridBoundColumn("", "PropID", Unit.Percentage(10), HorizontalAlign.Left, true);
			PropertiesGrid.AddGridBoundColumn("", "Erf", Unit.Percentage(10), HorizontalAlign.Left, true);
			PropertiesGrid.AddGridBoundColumn("", "Address", Unit.Percentage(80), HorizontalAlign.Left, true);

			PropertiesGrid.DataSource = DT;
			PropertiesGrid.DataBind();

			//if (DT.Rows.Count > 0)
			//    btnNext.Enabled = true;
		}

		protected void PropertiesGrid_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			//DataTable DT = ((SAHLGridView)sender).DataSource as DataTable;
			//object x = e.Row.DataItem;

			//<xs:sequence>
			//0 <xs:element name="PROP_ID" type="xs:decimal" minOccurs="0" /> 
			//1 <xs:element name="BUYER_IDCK" type="xs:string" minOccurs="0" /> 
			//2 <xs:element name="BUYER_NAME" type="xs:string" minOccurs="0" /> 
			//3 <xs:element name="PROVINCE" type="xs:string" minOccurs="0" /> 
			//4 <xs:element name="MUNICIPALITY" type="xs:string" minOccurs="0" /> 
			//5 <xs:element name="DEEDTOWN" type="xs:string" minOccurs="0" /> 
			//6 <xs:element name="ERF" type="xs:decimal" minOccurs="0" /> 
			//7 <xs:element name="PORTION" type="xs:decimal" minOccurs="0" /> 
			//8 <xs:element name="SECTIONAL_TITLE" type="xs:string" minOccurs="0" /> 
			//9 <xs:element name="UNIT" type="xs:int" minOccurs="0" /> 
			//10 <xs:element name="SUBURB" type="xs:string" minOccurs="0" /> 
			//11 <xs:element name="STREET_NAME" type="xs:string" minOccurs="0" /> 
			//12 <xs:element name="STREET_TYPE" type="xs:string" minOccurs="0" /> 
			//13 <xs:element name="STREET_NUMBER" type="xs:string" minOccurs="0" /> 
			//14 <xs:element name="PO_CODE" type="xs:string" minOccurs="0" /> 
			//15 <xs:element name="SIZE" type="xs:int" minOccurs="0" /> 
			//16 <xs:element name="SG_CODE" type="xs:string" minOccurs="0" /> 
			//17 <xs:element name="ERF_KEY" type="xs:string" minOccurs="0" /> 
			//18 <xs:element name="SECT_SCHEME_NO" type="xs:string" minOccurs="0" /> 
			//</xs:sequence>

			if (e.Row.DataItem != null)
			{
				string streetNo = "";
				string streetName = "";
				string sectional = "";
				string suburb = "";
				string postcode = "";
				string town = "";
				string province = "";

				DataRowView drv = e.Row.DataItem as DataRowView;

				if (!String.IsNullOrEmpty(drv.Row.ItemArray[0].ToString()))
					e.Row.Cells[0].Text = drv.Row.ItemArray[0].ToString().Trim();

				if (!String.IsNullOrEmpty(drv.Row.ItemArray[6].ToString()))
					e.Row.Cells[1].Text = drv.Row.ItemArray[6].ToString().Trim();

				//if (drv.Row.ItemArray[0] != null)
				//    e.Row.Cells[0].Text = drv.Row.ItemArray[0].ToString().Trim();

				//unit no
				if (drv.Row.ItemArray[9] != null && drv.Row.ItemArray[8] != null)
				{
					string title = drv.Row.ItemArray[8].ToString().Trim();
					string unit = drv.Row.ItemArray[9].ToString().Trim();

					//sectional title
					if (!String.IsNullOrEmpty(title))
						sectional = String.Format("Sectional Unit {0} {1}", unit, title);
					else if (!String.IsNullOrEmpty(unit))
						sectional = String.Format("Unit {0},", unit);
				}

				//street number
				if (!String.IsNullOrEmpty(drv.Row.ItemArray[13].ToString()))
					streetNo = drv.Row.ItemArray[13].ToString().Trim();

				//street name
				if (!String.IsNullOrEmpty(drv.Row.ItemArray[11].ToString()))
				{
					streetName = drv.Row.ItemArray[11].ToString().Trim();

					//street type
					if (!String.IsNullOrEmpty(drv.Row.ItemArray[12].ToString()))
						streetName += " " + drv.Row.ItemArray[12].ToString().Trim();
				}

				//suburb
				if (!String.IsNullOrEmpty(drv.Row.ItemArray[10].ToString()))
					suburb = drv.Row.ItemArray[10].ToString().Trim();

				//post code
				if (!String.IsNullOrEmpty(drv.Row.ItemArray[14].ToString()))
					postcode = drv.Row.ItemArray[14].ToString().Trim();

				//deedtown
				if (!String.IsNullOrEmpty(drv.Row.ItemArray[5].ToString()))
					town += drv.Row.ItemArray[5].ToString().Trim();

				//province
				if (!String.IsNullOrEmpty(drv.Row.ItemArray[3].ToString()))
					province += drv.Row.ItemArray[3].ToString().Trim();

				e.Row.Cells[2].Text = String.Format("{0} {1} {2}, {3} {4}, {5}, {6}", sectional, streetNo, streetName, suburb, postcode, town, province);

			}

			//e.Row.Attributes.Add("OnDblClick", "setPageNo()");
			//e.Row.Attributes.Add("OnClick", "propertiesGridClick()");
		}

		protected void PropertiesGrid_SelectedIndexChanged(object sender, EventArgs e)
		{
			lblSelectedProperty.Text = PropertiesGrid.Rows[PropertiesGrid.SelectedIndex].Cells[2].Text;

			if (OnPropertiesGridSelectedIndexChanged != null)
				OnPropertiesGridSelectedIndexChanged(sender, new KeyChangedEventArgs(PropertiesGrid.SelectedIndex));
		}

		protected void PropertiesGrid_GridDoubleClick(object sender, GridSelectEventArgs e)
		{
			btnNext_Click(sender, null);
		}

		protected void btnSearch_Click(object sender, EventArgs e)
		{
			lblSelectedProperty.Text = "";
			PropertiesGrid.SelectedIndex = -1;

			if (OnSearchButtonClicked != null)
				OnSearchButtonClicked(sender, new KeyChangedEventArgs(Page.Request.Form[txtIdentity.UniqueID]));

			if (PropertiesGrid.Rows.Count > 0 && PropertiesGrid.SelectedIndex > -1)
			{
				lblSelectedProperty.Text = PropertiesGrid.Rows[0].Cells[2].Text;
			}

		}

		protected void btnSelect_Click(object sender, EventArgs e)
		{
			string addressKey = Request.Form["addressKey"];

			if (String.IsNullOrEmpty(addressKey)) //use the manually entered data in the address control.
			{
			}
			else //use the existing address
			{
			}

			btnNext_Click(sender, e);
		}

		protected void btnNext_Click(object sender, EventArgs e)
		{

		}

		protected void btnPrev_Click(object sender, EventArgs e)
		{
		}


	}
}
