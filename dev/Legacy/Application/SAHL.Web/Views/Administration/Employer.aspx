<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Employer.aspx.cs" Inherits="SAHL.Web.Views.Administration.Employer"  MasterPageFile="~/MasterPages/Blank.master" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <cc1:EmployerDetails ID="employerDetails" runat="server">
    </cc1:EmployerDetails>
    <div class="buttonBar" style="width:99%">
        <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" Visible="False" CssClass="buttonSpacer" CausesValidation="false" />
        <SAHL:SAHLButton ID="btnClearForm" runat="server" Text="Clear Form" Visible="False" CssClass="buttonSpacer" />
        <SAHL:SAHLButton ID="btnUpdate" runat="server" Text="Update" Visible="False" CssClass="buttonSpacer" />
        <SAHL:SAHLButton ID="btnAdd" runat="server" Text="Add" Visible="False" CssClass="buttonSpacer" />
    </div>
</asp:Content>
