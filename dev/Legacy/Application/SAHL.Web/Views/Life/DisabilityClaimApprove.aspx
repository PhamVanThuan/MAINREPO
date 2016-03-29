<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true" CodeBehind="DisabilityClaimApprove.aspx.cs" Inherits="SAHL.Web.Views.Life.DisabilityClaimApprove" Title="Disability Claim Details" %>

<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <script src="../../Scripts/jquery-1.5.min.js" type="text/javascript"></script>
    <script src="../../Scripts/date.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function setEndDate() {
            var startDateText = $("#" + '<%=lblDisbilityPaymentStartDate.ClientID %>').text();
            var dateMask = "dd-MM-yyyy";
            if (startDateText.indexOf("/") > 0)
            {
                dateMask = "dd/MM/yyyy"
            }

            var startDate = Date.parseExact(startDateText, dateMask);

            var instalments = parseInt($("#" + '<%=txtInstalmentsAuthorised.ClientID %>').val());

            var endDate = startDate.clone().addMonths(instalments - 1);

            if (isNaN(endDate.getFullYear())) {
                $("#" + '<%=lblDisbilityPaymentEndDate.ClientID %>').text("-");
                $("#" + '<%=hidDisbilityPaymentEndDate.ClientID %>').val("-");
            }
            else {
                $("#" + '<%=lblDisbilityPaymentEndDate.ClientID %>').text(endDate.toString(dateMask));
                $("#" + '<%=hidDisbilityPaymentEndDate.ClientID %>').val(endDate.toString(dateMask));
            }
        }
    </script>

    <div style="text-align: center">
        <asp:Panel ID="pnlDisabilityClaimDetails" runat="server" GroupingText="Disability Claim Approve" Width="100%">
            <table style="width: 100%;" class="tableStandard">
                <tr>
                    <td colspan="3">&nbsp</td>
                </tr>
                <tr>
                    <td align="left" class="TitleText" style="width: 25%">Last Date Worked</td>
                    <td align="left" style="width: 25%">
                        <SAHL:SAHLLabel ID="lblDateLastWorked" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                    </td>
                    <td style="width: 50%"></td>
                </tr>
                <tr>
                    <td align="left" class="TitleText" style="width: 25%">Loan Debit Order Day</td>
                    <td align="left" style="width: 25%">
                        <SAHL:SAHLLabel ID="lblDebitOrderDay" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                    </td>
                    <td style="width: 50%"></td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp</td>
                </tr>
                <tr>
                    <td align="left" class="TitleText" style="width: 25%">Disability Payment Start Date</td>
                    <td align="left" style="width: 25%">
                        <SAHL:SAHLLabel ID="lblDisbilityPaymentStartDate" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                    </td>
                    <td style="width: 50%"></td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp</td>
                </tr>
                <tr>
                    <td align="left" class="TitleText">No. of Authorised Instalments</td>
                    <td align="left">
                        <SAHL:SAHLTextBox ID="txtInstalmentsAuthorised" runat="server" Width="15%" TabIndex="1" DisplayInputType="Number" FormatDecimalPlace="0" onkeyup="setEndDate();" MaxLength="3" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp</td>
                </tr>
                <tr>
                    <td align="left" class="TitleText">Disability Payment End Date</td>
                    <td align="left">
                        <SAHL:SAHLLabel ID="lblDisbilityPaymentEndDate" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                        <input type="hidden" id="hidDisbilityPaymentEndDate" runat="server"/>
                    </td>
                    <td></td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlExclusions" runat="server" Width="100%" >
            <table width="100%" class="tableStandard">
                <tr>
                    <td>
                        <SAHL:SAHLGridView ID="gridFurtherLendingExclusions" runat="server" AutoGenerateColumns="false" FixedHeader="false"
                            EnableViewState="false" GridHeight="100px" GridWidth="100%" Width="100%" PostBackType="None" SelectFirstRow="false"
                            HeaderCaption="Further Lending Exclusions" NullDataSetMessage="" EmptyDataSetMessage="There are no further lending exclusions."
                            OnRowDataBound="gridFurtherLendingExclusions_OnRowDataBound">
                            <RowStyle CssClass="TableRowA" />
                        </SAHL:SAHLGridView>
                    </td>
                </tr>
                </table>
        </asp:Panel>
        <asp:Panel ID="pnlButtons" runat="server" Width="100%">
            <table width="100%" class="tableStandard">
                <tr>
                    <td align="right">
                        <SAHL:SAHLButton ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                        <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="False" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>
