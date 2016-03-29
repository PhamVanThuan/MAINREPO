<%@ Control Language="C#" AutoEventWireup="true" Inherits="SAHL.Web.Views.ReleaseAndVariations.WorkFlowHeader" Codebehind="WorkFlowHeader.ascx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
    <div>
        <div class="backgroundLight" style="width:65%;float:left">
                <SAHL:SAHLLabel ID="lblClient" runat="server" Text="Client Name : " CssClass="LabelText"
                    Font-Bold="true"></SAHL:SAHLLabel>
                <SAHL:SAHLLabel ID="lblClientName" runat="server" Text="-" CssClass="LabelText"></SAHL:SAHLLabel>
        </div>
    
        <div class="backgroundLight" style="width:35%;float:right;text-align:right">
                <SAHL:SAHLLabel ID="lblLoan" runat="server" Text="Loan Number : " CssClass="LabelText"
                    Font-Bold="true"></SAHL:SAHLLabel>
                <SAHL:SAHLLabel ID="lblLoanNumber" runat="server" Text="-" CssClass="LabelText"></SAHL:SAHLLabel>
        </div> 
    </div>
    <br />