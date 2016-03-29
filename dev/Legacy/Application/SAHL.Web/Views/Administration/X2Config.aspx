<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="X2Config.aspx.cs" Inherits="SAHL.Web.Views.Administration.X2Config" MasterPageFile="~/MasterPages/Blank.master" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <asp:Panel ID="pnlMain" runat="server" Width="99%">

        <SAHL:DXGridView ID="grdMaps" runat="server" AutoGenerateColumns="False" Width="600" EnableViewState="false"
            KeyFieldName="Name" PostBackType="None" SettingsPager-Mode="ShowAllRecords" >
            <Columns>
                <SAHL:DXGridViewFormattedTextColumn FieldName="Name" Caption="Name"  />
                <SAHL:DXGridViewFormattedTextColumn FieldName="MapVersion" Caption="Map Version" />
            </Columns>
        </SAHL:DXGridView>                     

    </asp:Panel>
</asp:Content>
