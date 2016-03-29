<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Common.LoanDetail" Title="Loan Detail" Codebehind="LoanDetail.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table class="tableStandard" width="100%">
        <tr>
            <td>
                <SAHL:SAHLGridView ID="grdLoanDetail" runat="server" AutoGenerateColumns="false"
                    FixedHeader="false" EnableViewState="false" GridHeight="200px" GridWidth="100%"
                    Width="100%" HeaderCaption="Loan Detail" NullDataSetMessage="" EmptyDataSetMessage="There are no items"
                    OnRowDataBound="grdLoanDetail_RowDataBound" SelectFirstRow="True" OnSelectedIndexChanged="grdLoanDetail_SelectedIndexChanged">
                    <RowStyle CssClass="TableRowA" />
                </SAHL:SAHLGridView>
                <br />
                <table id="InfoTable" runat="server">
                    <tr>
                        <td class="TitleText">
                            Detail Class</td>
                        <td>
                            <SAHL:SAHLLabel ID="lblViewDetailClass" runat="server">-</SAHL:SAHLLabel>
                            <SAHL:SAHLDropDownList ID="ddlDetailClass" PleaseSelectItem="true" runat="server"
                                AutoPostBack="True" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="TitleText">
                            Detail Type</td>
                        <td>
                            <SAHL:SAHLLabel ID="lblViewDetailType" runat="server">-</SAHL:SAHLLabel>
                            <SAHL:SAHLDropDownList ID="ddlDetailType" PleaseSelectItem="true" runat="server"
                                AutoPostBack="true" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="TitleText">
                            Detail Date</td>
                        <td>
                            <SAHL:SAHLLabel ID="lblViewDetailDate" runat="server">-</SAHL:SAHLLabel>
                            <SAHL:SAHLDateBox ID="dateLoanDetailDate" runat="server" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="TitleText">
                            Amount</td>
                        <td>
                            <SAHL:SAHLLabel ID="lblViewAmount" runat="server">-</SAHL:SAHLLabel>
                            <SAHL:SAHLTextBox ID="txtDetailAmount" runat="server" DisplayInputType="Currency" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="TitleText">
                            Description</td>
                        <td>
                            <SAHL:SAHLLabel ID="lblDescription" runat="server">-</SAHL:SAHLLabel>
                            <SAHL:SAHLTextBox ID="txtDescription" runat="server" Height="56px" MaxLength="150"
                                TextMode="MultiLine" Width="496px" />
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TitleText">
                            Cancellation Type</td>
                        <td>
                            <SAHL:SAHLLabel ID="lblCancellationType" runat="server">-</SAHL:SAHLLabel>
                            <SAHL:SAHLDropDownList ID="ddlCancellationType" PleaseSelectItem="true" runat="server" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="ButtonRow" runat="server">
            <td align="right">
                <SAHL:SAHLButton ID="SubmitButton" runat="server" Text="Submit" AccessKey="S" OnClick="SubmitButton_Click" />
                <SAHL:SAHLButton ID="CancelButton" runat="server" Text="Cancel" AccessKey="C" OnClick="CancelButton_Click"
                    CausesValidation="false" />
            </td>
        </tr>
    </table>
</asp:Content>
