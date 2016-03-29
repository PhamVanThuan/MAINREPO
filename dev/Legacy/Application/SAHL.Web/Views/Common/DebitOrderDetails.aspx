<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.Common.DebitOrderDetails" Title="Debit Order Details" Codebehind="DebitOrderDetails.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<table border="0" cellspacing="0" cellpadding="0" class="PageBlock" style="width: 100%" width="100%"><tr><td align="left" style="height:99%; width: 100%;" valign="top">
    
    <SAHL:SAHLGridView ID="gridOrder" runat="server" AutoGenerateColumns="false" FixedHeader="false" 
        EnableViewState="false" GridHeight="200px" GridWidth="100%" Width="100%"
        HeaderCaption="Debit Orders" SelectFirstRow="true"
        NullDataSetMessage="There are no debit orders captured against this application"
        EmptyDataSetMessage="There are no debit orders captured against this application" 
        OnSelectedIndexChanged="OrderGrid_SelectedIndexChanged" OnRowDataBound="gridOrder_RowDataBound" PostBackType="SingleClick" EmptyDataText="There are no debit orders captured against this application" >
        <RowStyle CssClass="TableRowA" />
    </SAHL:SAHLGridView>
    
    <br />   
    <table border="0" id="InfoTable" runat="server" width="99%" class="tableStandard">
        <tr>
            <td class="TitleText" style="width: 132px;">
                Payment Type
            </td>
            <td style="width:419px;">
                <SAHL:SAHLLabel ID="DOPaymentType" runat="server">-</SAHL:SAHLLabel>
                <SAHL:SAHLDropDownList ID="DOPaymentTypeUpdate" runat="server" CssClass="CboText" style="width:100%;" PleaseSelectItem="true" OnSelectedIndexChanged="DOPaymentTypeUpdate_SelectedIndexChanged" AutoPostBack="True" Mandatory="true" >
                </SAHL:SAHLDropDownList>
            </td>
            <td style="width:199px;">
                &nbsp;</td>
            <td style="width:425px;">&nbsp;</td>
            <td style="width:15px;">
        </tr>
        <tr id="BnkAccRow" runat="server">
            <td class="TitleText">
                <SAHL:SAHLLabel ID="lblBankAccount" runat="server" CssClass="LabelText"
                    Width="105px" Font-Bold="True">Bank Account</SAHL:SAHLLabel></td>
            <td colspan="3">
                <SAHL:SAHLLabel ID="BankAccount" runat="server">-</SAHL:SAHLLabel>
                <SAHL:SAHLDropDownList ID="BankAccountUpdate" runat="server" CssClass="CboText" Mandatory="true" />
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr id="SalaryPaymentDayRow" runat="server">
            <td >
                <SAHL:SAHLLabel ID="lblSalaryPaymentDays" runat="server" Font-Bold="true">Salary Payment Day</SAHL:SAHLLabel>
            </td>
            <td colspan="3">
                <asp:Table ID="tblSalaryPaymentDays" runat="server" BackColor="#dcdcdc" BorderColor="#a0a0a0" BorderWidth="1" BorderStyle="Solid">


                </asp:Table>
            </td>
            <td>
             </td>
        </tr>
        <tr>
            <td class="TitleText">
                Debit Order Day
            </td>
            <td>
                <SAHL:SAHLLabel ID="DebitOrderDay" runat="server">-</SAHL:SAHLLabel><SAHL:SAHLDropDownList ID="DebitOrderDayUpdate" runat="server" CssClass="CboText" Mandatory="true" />
            </td>
            <td>
                &nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td class="TitleText">
                <SAHL:SAHLLabel ID="lblEffectiveDate" runat="server" TextAlign="Left"><span class="TitleText">Effective Date</span></SAHL:SAHLLabel></td>
            <td>
                <SAHL:SAHLLabel ID="EffectiveDate" runat="server">-</SAHL:SAHLLabel>
                <SAHL:SAHLDateBox ID="EffectiveDateUpdate" runat="server" Mandatory="true" />
            </td>
            <td>
                <SAHL:SAHLTextBox ID="ValEffectiveDateUpdateControl" runat="server" style="display:none;"></SAHL:SAHLTextBox>&nbsp;
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>

</td></tr>
</table>
    <div class="buttonBar" style="width:99%;margin-top:5px;" id="ButtonRow" runat="server">
        <SAHL:SAHLButton ID="btnDelete" runat="server" Text="Delete" AccessKey="D" OnClick="btnDelete_Click" Visible="false" OnClientClick="return confirm('Are you sure you want to delete the selected item?');" />
        <SAHL:SAHLButton ID="btnAdd" runat="server" Text=" Add " AccessKey="A" OnClick="btnAdd_Click" Visible="false" />
        <SAHL:SAHLButton ID="btnUpdate" runat="server" Text="Update" AccessKey="U" OnClick="btnUpdate_Click" Visible="false" />
        <SAHL:SAHLButton ID="CancelButton" runat="server" Text="Cancel" AccessKey="C" OnClick="CancelButton_Click" CausesValidation="false" />
    </div>    
</asp:Content>