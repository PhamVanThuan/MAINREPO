<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Common.LegalEntityEnableUpdate" Title="Untitled Page"
    Codebehind="LegalEntityEnableUpdate.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <div style="height: 100%; width: 100%; vertical-align: middle; text-align: center">
        <SAHL:SAHLLabel runat="server" ID="labelMessage" Text="This action will cause the selected legal entity's details to be updateable."></SAHL:SAHLLabel>
        <br />
        <SAHL:SAHLLabel runat="server" ID="labelQuestion" Text="Are you sure you wish to enable the update of all the legal entity's detail?"></SAHL:SAHLLabel>
        <br />
        <br />
        <SAHL:SAHLButton ID="btnSubmitButton" runat="server" AccessKey="U" Text="Update"
            OnClick="btnSubmitButton_Click" />
        <SAHL:SAHLButton ID="btnCancelButton" runat="server" AccessKey="C" CausesValidation="False"
            Text="Cancel" OnClick="btnCancelButton_Click" />
    </div>
</asp:Content>
