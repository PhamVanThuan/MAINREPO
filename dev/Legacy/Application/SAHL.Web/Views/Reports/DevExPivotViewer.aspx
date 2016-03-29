<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="True"CodeBehind="DevExPivotViewer.aspx.cs" Inherits="SAHL.Web.Views.Reports.DevExPivotViewer" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>

<%@ Register assembly="DevExpress.Web.ASPxPivotGrid.v10.2, Version=10.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPivotGrid" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    &nbsp;&nbsp;
    <asp:Panel runat="server" Width="99%" ID="pnlMain" > 
    <div id="divReportView" runat="server">
    	<SAHL:DXPivotGrid ID="aspxPivotGrid" runat="server" 
			OLAPConnectionString="provider=MSOLAP;data source=sahls11;initial catalog=Origination;cube name=Origination;">
		</SAHL:DXPivotGrid>
    </div>
    </asp:Panel>
    <br />
    <div class="buttonBar" style="width:99%;margin-top:5px;">
            <SAHL:SAHLButton ID="btnBack" runat="server" OnClick="btnCancel_Click" Text="Back" />
    </div>
</asp:Content>