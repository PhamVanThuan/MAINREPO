<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="Employment.aspx.cs" Inherits="SAHL.Web.Views.Common.Employment" Title="Untitled Page"
    EnableViewState="False" %>

<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <SAHL:SAHLPanel ID="pnlGrid" runat="server" Visible="true" Width="99%" SecurityTag="EmploymentConfirmIncomeOnly"
        SecurityHandler="Custom" SecurityDisplayType="Disable">
        <cc1:EmploymentGrid ID="grdEmployment" runat="server" GridHeight="135px" />
        <div style="width: 100%; margin-top: 10px;">
            <div style="float: left; width: 49%; display: inline;">
                <cc1:EmployerDetails ID="pnlEmployerDetails" runat="server" Height="290" />
            </div>
            <div style="float: right; width: 50%; display: inline;">
                <cc1:EmploymentDetails ID="pnlEmploymentDetails" runat="server" Height="290" />
            </div>
        </div>
    </SAHL:SAHLPanel>
    <div class="buttonBar" style="width: 99%; margin-top: 5px;">
        <SAHL:SAHLButton ID="btnExtended" runat="server" Text="Extended Details" Visible="False"
            CssClass="buttonSpacer" CausesValidation="false" ButtonSize="Size5" />
        <SAHL:SAHLButton ID="btnSubsidyDetails" runat="server" Text="Subsidy Details" Visible="False"
            CssClass="buttonSpacer" CausesValidation="false" ButtonSize="Size5" />
        <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" Visible="False" CssClass="buttonSpacer"
            CausesValidation="false" />
        <SAHL:SAHLButton ID="btnSave" runat="server" Text="Save" Visible="False" CssClass="buttonSpacer" />
    </div>
</asp:Content>
