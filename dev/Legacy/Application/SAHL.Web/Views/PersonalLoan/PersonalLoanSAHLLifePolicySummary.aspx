<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" 
    Inherits="SAHL.Web.Views.PersonalLoan.PersonalLoanSAHLLifePolicySummary" Codebehind="PersonalLoanSAHLLifePolicySummary.aspx.cs" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
	TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
     <table class="tableStandard">
        <tr>
            <td class="titleText" style="width:300px;">Insurer</td>
            <td class="cellDisplay"><SAHL:SAHLLabel ID="lblInsurer" runat="server"  CssClass="LabelText">-</SAHL:SAHLLabel></td>
        </tr>
        <tr>
            <td class="titleText" style="width:300px;">Policy Number</td>
            <td class="cellDisplay"><SAHL:SAHLLabel ID="lblPolicyNumber" runat="server"  CssClass="LabelText">-</SAHL:SAHLLabel></td>
        </tr>
        <tr>
            <td class="titleText" style="width:300px;">Commencement Date</td>
            <td class="cellDisplay"><SAHL:SAHLLabel ID="lblCommencementDate" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
        </tr>
        <tr>
            <td class="titleText">Status</td>
            <td class="cellDisplay"><SAHL:SAHLLabel ID="lblStatus" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
        </tr>
        <tr>
            <td class="titleText">Close Date</td>
            <td class="cellDisplay"><SAHL:SAHLLabel ID="lblClosedate" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel></td>
        </tr>
         <tr>
			<td class="titleText">Life Policy Premium</td>
			<td class="cellDisplay"><SAHL:SAHLLabel runat="server" ID="lblLifePolicyPremium"  CssClass="LabelText">-</SAHL:SAHLLabel></td>
		</tr>
        <tr>
            <td class="titleText">Sum Insured</td>
            <td class="cellDisplay"><SAHL:SAHLLabel ID="lblSumInsured" runat="server"  CssClass="LabelText">-</SAHL:SAHLLabel></td>
        </tr>      
    </table>
</asp:Content>