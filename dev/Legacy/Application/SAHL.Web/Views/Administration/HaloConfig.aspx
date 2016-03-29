<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HaloConfig.aspx.cs" Inherits="SAHL.Web.Views.Administration.HaloConfig" MasterPageFile="~/MasterPages/Blank.master" %>

<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <asp:Panel ID="pnlMain" runat="server" Width="99%">
        <SAHL:SAHLGridView ID="grdControl" runat="server" AutoGenerateColumns="false"
            EmptyDataSetMessage="The search returned no configuration data." EnableViewState="false"
            FixedHeader="false" GridHeight="200px" GridWidth="100%" HeaderCaption="Control Table"
            NullDataSetMessage="" PostBackType="None" Width="100%" SelectFirstRow="false" OnRowDataBound="grdControl_RowDataBound">
            <RowStyle CssClass="TableRowA" />
            <Columns>
                <SAHL:GridBoundField HeaderText="Description" DataField="ControlDescription" />
                <SAHL:GridBoundField HeaderText="Numeric Value" DataField="ControlNumeric" />
                <SAHL:GridBoundField HeaderText="Text Value" DataField="ControlText" />
            </Columns>
        </SAHL:SAHLGridView>
        <div>&nbsp;</div>
        <SAHL:SAHLGridView ID="grdConfig" runat="server" AutoGenerateColumns="false"
            EmptyDataSetMessage="No configuration data found." EnableViewState="false"
            FixedHeader="false" GridHeight="200px" GridWidth="100%" HeaderCaption="Config Values"
            NullDataSetMessage="" PostBackType="None" Width="100%" SelectFirstRow="false" OnRowDataBound="grdConfig_RowDataBound">
            <RowStyle CssClass="TableRowA" />
            <Columns>
                <SAHL:GridBoundField HeaderText="Name" DataField="ItemName" />
                <SAHL:GridBoundField HeaderText="Value" DataField="ItemValue" />
                <SAHL:GridBoundField HeaderText="Source" DataField="Source" />
            </Columns>
        </SAHL:SAHLGridView>
    </asp:Panel>
</asp:Content>
