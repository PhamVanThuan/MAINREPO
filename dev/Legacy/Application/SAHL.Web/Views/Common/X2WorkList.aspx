<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.Common.X2WorkList" Title="CBO Page" Codebehind="X2WorkList.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
  
    <SAHL:DXGridView ID="gridInstance" runat="server" AutoGenerateColumns="False" Width="100%" EnableViewState="false"
        KeyFieldName="InstanceID" PostBackType="DoubleClick" OnSelectionChanged="gridInstance_SelectionChanged" >
        <SettingsText Title="Workflow Instances" />
    </SAHL:DXGridView>   
    
    <div style="text-align:right;padding-top:7px;padding-right:10px;">
        <SAHL:SAHLButton ID="btnSelect" runat="server" Text="Select" AccessKey="C" OnClick="btnSelect_Click" />
    </div>
</asp:Content>