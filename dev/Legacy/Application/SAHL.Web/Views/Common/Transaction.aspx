<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="Transaction.aspx.cs" Inherits="SAHL.Web.Views.Common.Transaction"
    Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <div style="width: 100%; float: left">
        <table id="DisplayTable" runat="server" class="tableStandard" style="display:none">
            <tr>
                <td style="width: 130px;" class="TitleText" id="DisplayCell1" runat="server">
                    Account
                </td>
                <td style="width: 300px;" id="DisplayCell2" runat="server">
                    <SAHL:SAHLDropDownList ID="FinancialServices" runat="server" Style="width: 100%;"
                        PleaseSelectItem="false" AutoPostBack="True">
                    </SAHL:SAHLDropDownList>
                </td>
                <td style="width: 50px;">
                </td>
                <td style="width: 120px;" class="TitleText">
                    Transaction Type
                </td>
                <td style="width: 200px;">
                    <SAHL:SAHLDropDownList ID="TransactionType" runat="server" PleaseSelectItem="false"
                        AutoPostBack="True" Style="width: 100%;">
                    </SAHL:SAHLDropDownList>
                    <SAHL:SAHLTextBox ID="tbTransactionType" runat="server" Style="display: none" />
                </td>
            </tr>
            <tr>
                <td class="TitleText">
                    Transaction Type Description
                </td>
                <td>
                    <SAHL:SAHLDropDownList ID="TransactionTypeDesc" runat="server" Style="width: 100%;"
                        PleaseSelectItem="false" AutoPostBack="True">
                    </SAHL:SAHLDropDownList>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
        </table>
        <table id="ConfirmTable" runat="server" style="width: 100%; display: none;">
            <tr>
                <td id="ConfirmMessage" runat="server" style="color: #FF0000; width: 100%" class="TitleText"
                    align="center">
                </td>
            </tr>
        </table>
        <div id="gridDiv" runat="server" style="width: 100%; float: left; ">
            <SAHL:DXGridView ID="grdTransaction" runat="server" AutoGenerateColumns="False" Width="99%"  
                EnableViewState="false" KeyFieldName="Number" PostBackType="DoubleClick" OnHtmlRowPrepared="LoanTranGrid_RowDataBound" 
                OnCustomColumnSort="LoanTranGrid_CustomColumnSort" OnHtmlDataCellPrepared="LoanTranGrid_HtmlDataCellPrepared">
                <SettingsText Title="Transaction View" />
                <Settings ShowTitlePanel="True" ShowHorizontalScrollBar="True"/>
                <SettingsBehavior AllowGroup="true" AllowSort="True" />
            </SAHL:DXGridView>

        </div>
        <div style="width: 100%; float: left">
            <SAHL:SAHLGridView ID="grdRollbackConfirm" runat="server" AutoGenerateColumns="false"
                FixedHeader="false" EnableViewState="false" GridHeight="380px" GridWidth="100%"
                Width="100%" HeaderCaption="Transaction(s) to be Rolled Back" ScrollX="true"
                NullDataSetMessage="" PostBackType="None" EmptyDataSetMessage="There are no Loan Transactions.">
            </SAHL:SAHLGridView>
        </div>
        <div id="divButtons" style="float: right;">
            <SAHL:SAHLButton ID="btnRollback" runat="server" Text="Rollback" AccessKey="R" OnClick="btnRollback_Click" />
            <SAHL:SAHLButton ID="btnRollbackConfirm" runat="server" Text="Rollback" AccessKey="R"
                OnClick="btnRollbackConfirm_Click" />
            <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" AccessKey="C" OnClick="btnCancel_Click" />
        </div>
    </div>
</asp:Content>
