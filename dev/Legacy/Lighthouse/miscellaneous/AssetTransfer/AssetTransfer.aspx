<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AssetTransfer.aspx.cs" Inherits="AssetTransfer" %>

<%@ Register TagPrefix="asp" TagName="WebCalc" Src="~/AssetTransfer/WebCalendar.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="sahl-mlss.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">

<!--

        function window_onload() {

            window.document.all.item("txtError").style.visibility = "hidden";
            window.document.all.item("txtConfirm").style.visibility = "hidden";
            window.document.all.item("txtLoanToRemove").style.visibility = "hidden";
            window.document.all.item("cmdRemoveLoanAuto").style.visibility = "hidden";

            if (window.document.all.item("txtError").value != "") {
                alert(window.document.all.item("txtError").value);
            }
            window.document.all.item("txtError").value = "";

            if (window.document.all.item("txtConfirm").value != "") {
                var add = confirm(window.document.all.item("txtConfirm").value.replace(/lnBreak/g, "\n"));
                window.document.all.item("txtConfirm").value = "";
                if (add == false) {
                    window.document.all.item("txtLoanNumber").value = window.document.all.item("txtLoanToRemove").value;
                    window.document.all.item("cmdRemoveLoanAuto").click();
                }
            }

        }

-->
    </script>
</head>
<body language="javascript" onload="return window_onload()">
    <form id="form1" runat="server">
    <div>
        <table style="width: 860px">
            <tr>
                <td style="height: 21px" class="ManagementPanel">
                    <table>
                        <tr>
                            <td colspan="2" align="center" style="height: 18px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 13%; height: 24px;">
                                <label>
                                    SPV</label>
                            </td>
                            <td style="height: 24px; width: 38%">
                                <asp:DropDownList ID="ddlSPV" runat="server" DataTextField="SPVDescription" DataValueField="SPVNumber"
                                    Width="346px">
                                </asp:DropDownList>
                                <asp:Button ID="cmdTransferSPV" runat="server" Text="Transfer SPV" Width="112px"
                                    OnClick="cmdTransferSPV_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 13%; height: 27px;">
                                Loan Number
                            </td>
                            <td style="height: 27px">
                                <asp:TextBox ID="txtLoanNumber" Width="112px" runat="server" ToolTip="Loan number to be added or deleted from the list"></asp:TextBox>
                                <asp:Button ID="cmdAddLoan" runat="server" Text="Add Loan" Width="112px" OnClick="cmdAddLoan_Click" />
                                <asp:Button ID="cmdRemoveLoan" runat="server" Text="Remove Loan" Width="112px" OnClick="cmdRemoveLoan_Click" />
                                <asp:Button ID="cmdRemoveLoanAuto" runat="server" Text="Remove Loan" Width="112px"
                                    OnClick="cmdRemoveLoan_Click" />
                                <asp:Button ID="cmdClearAll" runat="server" OnClick="cmdClearAll_Click" Text="Clear Table" />
                            </td>
                        </tr>
                        <tr valign="top">
                            <td style="width: 13%; height: 22px;">
                                Accrual Date
                            </td>
                            <td style="height: 22px;" align="left">
                                <asp:TextBox ID="txtAccrualDate" runat="server" Width="112px" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 22px">
                            </td>
                            <td style="width: 38%; height: 22px;">
                                <asp:CheckBox ID="chkCancellation" runat="server" Text="Include loans under cancellation" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table width="100%" border="0">
                                    <tr>
                                        <td style="height: 21px; width: 25%">
                                            Current Balance Total
                                        </td>
                                        <td style="height: 21px">
                                            <asp:TextBox ID="txtCurrentBalance" runat="server" ReadOnly="True"></asp:TextBox>
                                        </td>
                                        <td style="height: 21px; width: 25%;">
                                            Bond Total
                                        </td>
                                        <td style="height: 21px">
                                            <asp:TextBox ID="txtBond" runat="server" ReadOnly="True"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 25%; height: 21px">
                                            Accrued Interest Total
                                        </td>
                                        <td style="height: 21px;">
                                            <asp:TextBox ID="txtAccruedInterest" runat="server" ReadOnly="True"></asp:TextBox>
                                        </td>
                                        <td style="height: 21px">
                                            Loan Count
                                        </td>
                                        <td style="height: 21px">
                                            <asp:TextBox ID="txtLoanCount" runat="server" ReadOnly="True"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 25%; height: 26px;">
                                            Debtor Balance Total
                                        </td>
                                        <td style="height: 26px">
                                            <asp:TextBox ID="txtDebtorBalance" runat="server" ReadOnly="True"></asp:TextBox>
                                        </td>
                                        <td style="height: 26px">
                                            Export to CSV
                                        </td>
                                        <td style="height: 26px">
                                            <asp:Button ID="cmdExport" runat="server" OnClick="cmdExport_Click" Text="Export"
                                                Width="112px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:GridView ID="grdLoans" runat="server" Width="100%" AutoGenerateColumns="False"
                                                Font-Size="10pt" AutoGenerateSelectButton="True" OnSelectedIndexChanged="grdLoans_SelectedIndexChanged">
                                                <Columns>
                                                    <asp:BoundField DataField="AccountKey" HeaderText="Loan Number" />
                                                    <asp:BoundField DataField="ClientSurname" HeaderText="Client" />
                                                    <asp:BoundField DataField="SPVDescription" HeaderText="SPV Description" />
                                                    <asp:BoundField DataField="LoanTotalBondAmount" HeaderText="Bond" ApplyFormatInEditMode="True"
                                                        DataFormatString="{0:f}">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="LoanCurrentBalance" HeaderText="Current Balance">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AccruedInterest" HeaderText="Accrued Interest">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DebtorBalance" HeaderText="Debtor Balance">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TransferedYN" HeaderText="Transferred">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <HeaderStyle Font-Size="10pt" />
                                            </asp:GridView>
                                            <br />
                                            <br />
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                                <asp:TextBox ID="txtMessage" runat="server" Width="834px" Visible="False"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <asp:TextBox ID="txtError" runat="server" Width="828px"></asp:TextBox>
    <asp:TextBox ID="txtLoanToRemove" runat="server" Width="828px"></asp:TextBox>
    <asp:TextBox ID="txtConfirm" runat="server" Width="828px"></asp:TextBox>
    </form>
</body>
</html>