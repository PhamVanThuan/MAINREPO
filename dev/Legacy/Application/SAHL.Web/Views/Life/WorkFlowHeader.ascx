<%@ Control Language="C#" AutoEventWireup="true" Inherits="SAHL.Web.Views.Life.WorkFlowHeader"
    Codebehind="WorkFlowHeader.ascx.cs" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<div class="backgroundLight" style="width: 100%">
    <div class="backgroundLight" style="float: left;width: 35%">
        <SAHL:SAHLLabel ID="lblClient" runat="server" Text="Client Name : " CssClass="LabelText"
            Font-Bold="true"></SAHL:SAHLLabel>
        <SAHL:SAHLLabel ID="lblClientName" runat="server" Text="-" CssClass="LabelText"></SAHL:SAHLLabel>
    </div>
    <div class="backgroundLight" style="width: 30%;float:left;text-align:center">
        <SAHL:SAHLLabel ID="lblPolicyTypeDesc" runat="server" CssClass="LabelText" Font-Bold="True"
            TextAlign="Right" Text="Policy Type : "></SAHL:SAHLLabel>
        <SAHL:SAHLLabel ID="lblPolicyType" runat="server" CssClass="LabelText" Text="-" TextAlign="Left"></SAHL:SAHLLabel>
    </div>
    <div class="backgroundLight" style="width: 35%; float: right; text-align: right">
        <SAHL:SAHLLabel ID="lblLoan" runat="server" Text="Loan Number : " CssClass="LabelText"
            Font-Bold="true"></SAHL:SAHLLabel>
        <SAHL:SAHLLabel ID="lblLoanNumber" runat="server" Text="-" CssClass="LabelText"></SAHL:SAHLLabel>
    </div>
</div>
&nbsp
<br />
