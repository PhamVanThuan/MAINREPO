<%@ Page Language="C#" MasterPageFile="~/MasterPages/SAHL.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.Common.SettlementDetails" Title="Untitled Page" Codebehind="SettlementDetails.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<script language="javascript" type="text/javascript">
// <!CDATA[

function TABLE1_onclick() {

}

// ]]>
</script>   
   <SAHL:SAHLGridView ID="DetailsGrid" runat="server" AutoGenerateColumns="false" FixedHeader="false" 
        EnableViewState="false" GridHeight="100px" GridWidth="770px" Width="770px"
        HeaderCaption="Property Details"
        PostBackType="SingleClick" 
        NullDataSetMessage=""
        EmptyDataSetMessage="There are no Property Details." 
        OnRowDataBound="DetailsGrid_RowDataBound" 
        OnSelectedIndexChanged="DetailsGrid_SelectedIndexChanged" Height="65px">
        <RowStyle CssClass="TableRowA" />
    </SAHL:SAHLGridView>
    
    <asp:Panel ID="panelSettlementDetails" runat="server" Height="187px" Width="300px">
        <table style="width: 90%; margin-left: auto; margin-right: auto; height: 172px;" id="TABLE1" onclick="return TABLE1_onclick()">
            <tr>
                <td style="width: 512px; height: 175px">
                    <asp:Panel ID="panelSettlement"
        runat="server" Height="101px" Width="290px" GroupingText="Settlement Information">
        <table style="width: 288px; height: 96px;">
            <tr>
                <td style="width: 502px; height: 16px;" align="right">
                    <SAHL:SAHLLabel ID="lblDebtType" runat="server" CssClass="LabelText" Width="136px">Debt Type</SAHL:SAHLLabel></td>
                <td style="height: 16px;">
                </td>
                <td style="height: 16px; width: 1731px;" align="left">
                    <SAHL:SAHLTextBox ID="txtDebtType" runat="server" TabIndex="1" Width="164px" ReadOnly="True" onchanged="SetModifiedTrue()"></SAHL:SAHLTextBox></td>
                <td align="left" style="height: 16px; width: 18px;">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 502px; height: 2px;" align="right">
                    <SAHL:SAHLLabel ID="lblTotalOutstanding" runat="server" CssClass="LabelText" Width="136px">Total Outstanding</SAHL:SAHLLabel></td>
                <td style="height: 2px;">
                </td>
                <td style="height: 2px; width: 1731px;" align="left">
                    <SAHL:SAHLTextBox ID="txtTotalOutstanding" runat="server" TabIndex="2" Width="151px"  DisplayInputType="Currency" ReadOnly="True"></SAHL:SAHLTextBox></td>
                <td align="left" style="height: 2px; width: 18px;">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 502px; height: 1px" align="right">
                    <SAHL:SAHLLabel ID="lblMonthlyInstallment" runat="server" CssClass="LabelText" Width="150px" TextAlign="right">Monthly Instalment</SAHL:SAHLLabel></td>
                <td style="height: 1px">
                </td>
                <td style="height: 1px; width: 1731px;" align="left">
                    <SAHL:SAHLTextBox ID="txtMonthlyInstallment" runat="server" TabIndex="3" Width="150px" DisplayInputType="Currency"></SAHL:SAHLTextBox></td>
                <td align="left" style="height: 1px; width: 18px;" valign="middle">
                    <SAHL:SAHLRequiredFieldValidator ID="SAHLRequiredFieldValidator4" runat="server"
                        ControlToValidate="txtMonthlyInstallment" />
                </td>
            </tr>
            <tr>
                <td style="width: 502px; height: 1px" align="right">
                    <SAHL:SAHLLabel ID="lblReference" runat="server" CssClass="LabelText" Width="131px">Reference</SAHL:SAHLLabel></td>
                <td style="height: 1px">
                </td>
                <td style="height: 1px; width: 1731px;" align="left">
                    <SAHL:SAHLTextBox ID="txtReference" runat="server" TabIndex="6"></SAHL:SAHLTextBox></td>
                <td align="left" style="height: 1px; width: 18px;">
                </td>
            </tr>
        </table>
    </asp:Panel>
                </td>
                <td style="width: 4px; height: 175px">
                </td>
                <td style="width: 4880px; height: 175px">
                    <asp:Panel ID="panelBankDetails" runat="server" GroupingText="Settlement Bank Details"
            Height="119px" Width="412px">
            <table style="width: 100%;height:102px">
                <tr>
                    <td align="right" style="width: 502px; height: 1px;">
                        <SAHL:SAHLLabel ID="lblBank" runat="server" CssClass="LabelText">Bank</SAHL:SAHLLabel></td>
                    <td style="width:301px; height: 1px;">
                    </td>
                    <td align="left" style="width: 2050px; height: 1px;">
                        <SAHL:SAHLTextBox ID="txtBank" runat="server" Width="248px" TabIndex="1" ReadOnly="True"></SAHL:SAHLTextBox></td>                  
                </tr>
                <tr>
                    <td align="right" style="width: 502px;">
                        <SAHL:SAHLLabel ID="lblBranch" runat="server" CssClass="LabelText" Width="77px">Branch</SAHL:SAHLLabel></td>
                    <td style="width:301px;">
                    </td>
                    <td align="left" style="width: 2050px;">
                        <SAHL:SAHLTextBox ID="txtBranch" runat="server" Width="250px" TabIndex="2" ReadOnly="True"></SAHL:SAHLTextBox></td>                  
                </tr>
                <tr>
                    <td align="right" style="width: 502px; height: 9px">
                        <SAHL:SAHLLabel ID="lblAccountType" runat="server" CssClass="LabelText" Width="110px" TextAlign="Right">Account Type</SAHL:SAHLLabel></td>
                    <td style="width:301px; height: 9px;">
                    </td>
                    <td align="left" style="height: 9px; width: 2050px;">
                        <SAHL:SAHLTextBox ID="txtAccountType" runat="server" TabIndex="3" ReadOnly="True"></SAHL:SAHLTextBox></td>                   
                </tr>
                  <tr>
                    <td align="right" style="width: 502px;">
                        <SAHL:SAHLLabel ID="lblAccountNumber" runat="server" CssClass="LabelText" Width="119px" Height="18px" TextAlign="Right">Account Number</SAHL:SAHLLabel></td>
                    <td style="width:301px;">
                    </td>
                    <td align="left" style="width: 2050px;">
                        <SAHL:SAHLTextBox ID="txtAccountNumber" runat="server" Width="230px" TabIndex="4" ReadOnly="True"></SAHL:SAHLTextBox></td>                   
                </tr>
                <tr>
                    <td align="right" style="width: 502px;">
                        <SAHL:SAHLLabel ID="lblAccountName" runat="server" CssClass="LabelText" Width="106px" Height="19px" TextAlign="Right">Account Name</SAHL:SAHLLabel></td>
                    <td style="width:301px;">
                    </td>
                    <td align="left" style="width: 2050px;">
                        <SAHL:SAHLTextBox ID="txtAccountName" runat="server" Width="228px" TabIndex="5" ReadOnly="True"></SAHL:SAHLTextBox></td>                   
                </tr>
            </table>
            </asp:Panel>
                </td>
            </tr>
        </table>
        &nbsp;</asp:Panel>
    &nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
    &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;<br />
    <asp:Label ID="lblError" runat="server" ForeColor="Red" Width="791px"></asp:Label><br />
    <table style="width: 784px">
        <tr>
            <td style="width: 4614px">
            </td>
            <td style="width: 60px">
                <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" Width="80px" TabIndex="8" /></td>
            <td style="width: 60px">
    <SAHL:SAHLButton ID="btnUpdate" runat="server" Text="Update" OnClientClick="SetModifiedFalse()" Height="24px" Width="95px" TabIndex="7" OnClick="btnUpdate_Click" /></td>
        </tr>
    </table>
    <br />
    &nbsp; &nbsp; &nbsp;
    <br />
</asp:Content>

