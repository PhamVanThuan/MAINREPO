<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" CodeBehind="QuickCashDetails.aspx.cs" Inherits="SAHL.Web.Views.QuickCash.QuickCashDetails" Title="Quick Cash" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language="javascript" type="text/javascript">
    function calculateApproximateInterest()
    {
        var interestRate;
        var numberOfdays;
        var amount;
        interestRate = document.getElementById('<%=txtInterestRate.ClientID%>').value ; 
        numberOfdays =   document.getElementById('<%=txtNumDaysInterest.ClientID%>').value ;  
        amount = SAHLCurrencyBox_getValue('<%=txtRequiredAmt.ClientID%>').replaceAll(',', '');
        if (isNaN(parseFloat(interestRate)) || isNaN(parseFloat(numberOfdays)) || isNaN(parseFloat(amount)))
        {
            document.getElementById('<%=txtApproxInterestAmt.ClientID%>').value = "R 0.00";
        }
        else
        {
            var rate = (parseFloat(interestRate)/100)*parseFloat(amount);
            document.getElementById('<%=txtApproxInterestAmt.ClientID%>').value  = "R " + formatNumber(((rate*numberOfdays)/365), 2, ',', '.').toString();
        }        
    }
</script>    
    <div id="divMain">
    <asp:Panel ID="panelApproved" runat="server" GroupingText="Quick Cash Approved" Width="100%" Font-Bold="true">
            <br />
            <table id="tblQCApproved" style="width: 100%; height: 20px;">
                <tr>
                    <td>
                    <SAHL:SAHLLabel ID="lblTotCashOutReq" runat="server" TextAlign="Left" Font-Bold="true">Total Cash Out Required</SAHL:SAHLLabel></td>
                    <td><SAHL:SAHLCurrencyBox id="txtCashOutRequired" runat="server" ReadOnly="true"/></td>
                    <td>&nbsp;&nbsp</td>
                    <td><SAHL:SAHLLabel ID="lblCashReqdLOA" runat="server" TextAlign="Left" Font-Bold="true">QuickCash Required (LOA)</SAHL:SAHLLabel></td>
                    <td><SAHL:SAHLCurrencyBox id="txtCashRequiredLOA" runat="server" ReadOnly="true"/></td>
                    <td>&nbsp;&nbsp</td>
                    <td><SAHL:SAHLLabel ID="lblUpfrontAmt" runat="server" CssClass="LabelText" Font-Bold="true">Upfront Approved Amount</SAHL:SAHLLabel></td>
                    <td><SAHL:SAHLCurrencyBox id="txtUpfrontApprovedAmt" runat="server"  AllowNegative="false" ReadOnly="true"/></td>
                    <td>&nbsp;&nbsp</td>
                    <td><SAHL:SAHLLabel ID="lblTotQCApprovedAmt" runat="server" CssClass="LabelText" Font-Bold="true">Total QC Approved Amount</SAHL:SAHLLabel></td>
                    <td><SAHL:SAHLCurrencyBox id="txtQCApprovedAmount" runat="server" AllowNegative="false" ReadOnly="true"/></td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <SAHL:SAHLGridView ID="gridQuickCashPaymentDetails" runat="server" AutoGenerateColumns="false" FixedHeader="false" 
        EnableViewState="false" width="100%" GridWidth="100%"   HeaderCaption="Quick Cash Payments"
        NullDataSetMessage=""
        EmptyDataSetMessage="There are no Quick Cash Payments."
        OnRowDataBound="gridQuickCashPaymentDetails_RowDataBound" ShowFooter="true" OnSelectedIndexChanged="gridQuickCashPaymentDetails_SelectedIndexChanged">
        <RowStyle CssClass="TableRowA" />
        </SAHL:SAHLGridView>
        <br />
        <SAHL:SAHLGridView ID="gridThirdPartyPayments" runat="server" AutoGenerateColumns="false" FixedHeader="false" 
        EnableViewState="false" width="100%" GridWidth="100%"   HeaderCaption="Payments"
        PostBackType="None" 
        NullDataSetMessage="" 
        Visible = "false" ShowFooter="true"
        EmptyDataSetMessage="There are no Payments."
        OnRowDataBound="gridThirdPartyPayments_RowDataBound" OnSelectedIndexChanged="gridThirdPartyPayments_SelectedIndexChanged"> 
        <RowStyle CssClass="TableRowA" />
        </SAHL:SAHLGridView>
        
        <asp:Panel ID="panelPayments" runat="server" GroupingText="Quick Cash Payments" Width="100%" Font-Bold="true">
        <br />
            <table style="width: 100%">
                <tr>
                    <td style="width: 10%">
                        <SAHL:SAHLLabel ID="lblBankAccount" runat="server" Font-Bold="true" TextAlign="Left" Width="134px">Bank Account</SAHL:SAHLLabel></td>
                    <td style="width: 503px">
                        <SAHL:SAHLDropDownList ID="ddlBankAccount" runat="server" Width="700px" PleaseSelectItem="false"></SAHL:SAHLDropDownList></td>
                </tr>
            </table>
            <br />
            <table id="tblPayments" style="width: 100%">
                <tr>
                    <td style="width:10%">
                        <SAHL:SAHLLabel ID="lblPaymentType" runat="server" Font-Bold="true" TextAlign="Left">Payment Type</SAHL:SAHLLabel>
                    </td>
                    <td style="width:10%">
                        <SAHL:SAHLDropDownList ID="ddlPaymentType" runat="server" Width="138px" OnSelectedIndexChanged="ddlPaymentType_SelectedIndexChanged" AutoPostBack="true">
                        </SAHL:SAHLDropDownList></td>
                    <td style="width:10%">
                        <SAHL:SAHLLabel ID="lblAvailableAmt" runat="server" Font-Bold="true" TextAlign="Left">Available Amount</SAHL:SAHLLabel></td>
                    <td style="width:10%">
                        <SAHL:SAHLCurrencyBox ID="txtAvailableAmt" runat="server" ReadOnly="true" AllowNegative="false" MaxLength="15"></SAHL:SAHLCurrencyBox></td>
                </tr>
                <tr>
                    <td style="width:10%">
                        <SAHL:SAHLLabel ID="lblPrime" runat="server" Font-Bold="true" TextAlign="Left">Prime</SAHL:SAHLLabel></td>
                    <td style="width:10%">
                        <SAHL:SAHLTextBox ID="txtPrime" runat="server" ReadOnly="true"></SAHL:SAHLTextBox></td>
                    <td style="width:10%">
                        <SAHL:SAHLLabel ID="lblRequiredAmt" runat="server" Font-Bold="true" TextAlign="Left">Required Amount</SAHL:SAHLLabel></td>
                    <td style="width:10%">
                        <SAHL:SAHLCurrencyBox ID="txtRequiredAmt" runat="server" AllowNegative="false" MaxLength="15"></SAHL:SAHLCurrencyBox></td>
                </tr>
                <tr>
                    <td style="width:10%">
                        <SAHL:SAHLLabel ID="lblLinkRate" runat="server" Font-Bold="true" TextAlign="Left"
                            Width="100px" >Link Rate</SAHL:SAHLLabel></td>
                    <td style="width:10%">
                        <SAHL:SAHLDropDownList ID="ddlLinkRate" runat="server" Width="80px" PleaseSelectItem="false">
                        </SAHL:SAHLDropDownList></td>
                    <td style="width:10%">
                        <SAHL:SAHLLabel ID="lblNumDaysInterest" runat="server" Font-Bold="true" TextAlign="Left">Number of days interest</SAHL:SAHLLabel></td>
                    <td style="width:10%">
                        <SAHL:SAHLTextBox ID="txtNumDaysInterest" runat="server">0</SAHL:SAHLTextBox></td>
                </tr>
                <tr>
                    <td style="width:10%">
                        <SAHL:SAHLLabel ID="lblInterestRate" runat="server" Font-Bold="true" TextAlign="Left" 
                            >Interest Rate</SAHL:SAHLLabel></td>
                    <td style="width:10%">
                        <SAHL:SAHLTextBox ID="txtInterestRate" runat="server" ReadOnly="true"></SAHL:SAHLTextBox></td>
                    <td style="width:10%">
                        <SAHL:SAHLLabel ID="lblApproxInterestAmt" runat="server" Font-Bold="true" TextAlign="Left">Approximate Interest Amount</SAHL:SAHLLabel></td>
                    <td style="width:10%">
                        <SAHL:SAHLTextBox ID="txtApproxInterestAmt" runat="server" ReadOnly="true" AllowNegative="false" DisplayInputType="Normal" MaxLength="6"></SAHL:SAHLTextBox></td>
                </tr>
            </table>
            <br />
        </asp:Panel>
        <br />
         <table  style="width:99%">
            <tr id="ButtonRow" runat="server" align="right">
            <td align="right">
                <SAHL:SAHLButton ID="btnCancel" runat="server" AccessKey="C" CausesValidation="false" OnClick="CancelButton_Click" Text="Cancel"/>
                <SAHL:SAHLButton ID="btnSubmit" runat="server" AccessKey="S" OnClick="SubmitButton_Click" Text="Submit" />
            </td></tr>
        </table>   
 </div>        
</asp:Content>
