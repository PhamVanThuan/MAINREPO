<%@ Page Language="C#" CodeFile="ManageLoan.aspx.cs" Inherits="ManageLoan_aspx" %>

<%@ Register Src="../Common/UserControls/ManageLoanStatus.ascx" TagName="ManageLoanStatus"
    TagPrefix="uc9" %>

<%@ Register Src="../Common/UserControls/LoanLinks.ascx" TagName="LoanLinks" TagPrefix="uc8" %>



<%@ Register TagPrefix="uc7" TagName="ManageLoanDetail" Src="../common/UserControls/ManageLoanDetail.ascx" %>
<%@ Register TagPrefix="uc5" TagName="ManageDebitOrder" Src="../common/UserControls/ManageDebitOrder.ascx" %>
<%@ Register TagPrefix="uc6" TagName="ManageHOC" Src="../common/UserControls/ManageHOC.ascx" %>
<%@ Register TagPrefix="uc4" TagName="ManageLoanBreakdown" Src="../common/UserControls/ManageLoanBreakdown.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ManageLoan" Src="../common/UserControls/ManageLoan.ascx" %>
<%@ Register TagPrefix="uc2" TagName="ManageBankAccount" Src="../common/UserControls/ManageBankAccount.ascx" %>
<%@ Register TagPrefix="uc3" TagName="ManageLoanRates" Src="../common/UserControls/ManageLoanRates.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="../style.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:Panel ID="pnlMain" runat="server" Height="50px" Width="125px">
    
    
    <table border=0 cellpadding=0 cellspacing=0>
        <tr>
            <td colspan="2">
                <table border=0 cellpadding=0 cellspacing=0 width=900 >
                    <tr>
                    <td class="ManagementPanel">
                        <uc1:ManageLoan id="ucMLManageLoan" runat="server" Visible="true"/>
                    </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign=top>
                    <table border=0 cellpadding=0 cellspacing=1 width=100%  >
                        <tr>
                            <td class="ManagementPanel">
                    <uc3:ManageLoanRates id="ucMLManageLoanRates" runat="server" Visible="true"/>
                    
                            </td>
                        </tr>
                        <tr>
                            <td class="ManagementPanel">
                    <uc4:ManageLoanBreakdown ID="ucMLManageLoanBreakdown" Runat="server" Visible="true" />
                            </td>
                        </tr>
                    </table>
            </td>
            <td valign=top>
                <table  border=0 cellpadding=0 cellspacing=1 width=100%>
                <tr><td  class="ManagementPanel" valign="top">
                    <uc2:ManageBankAccount id="ucMLManageBankAccount" runat="server" Visible="true"/>
                </td></tr>
                        <tr>
                            <td class="ManagementPanel">
                                <uc5:ManageDebitOrder ID="ucMLManageDebitOrder" Runat="server" Visible="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="ManagementPanel">
                                &nbsp;<asp:LinkButton ID="lbLoanCalculator" runat="server">[ Loan Calculator ]</asp:LinkButton>
                                <asp:LinkButton ID="lbLoanDetail" runat="server" OnClick="lbLoanDetail_Click">[ Loan Detail ]</asp:LinkButton>
                                <asp:LinkButton ID="lbHOC" runat="server" OnClick="lbHOC_Click">[ HOC ]</asp:LinkButton><br />
                                &nbsp;</td>
                        </tr>
                    </table>
            </td>
        </tr>
    </table>
        </asp:Panel>
        <asp:Panel ID="pnlLoanDetail" runat="server" Height="50px" Width="125px" Visible="False">
            <table align="center" style="width: 116px">
                <tr>
                    <td class="ManagementPanel">
                        &nbsp;<uc7:ManageLoanDetail ID="ManageLoanDetail1" runat="server" />
                        <asp:LinkButton ID="lbLoanDetailMain" runat="server" OnClick="lbLoanDetailMain_Click">[ Return ] </asp:LinkButton>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlHOC" runat="server" Height="50px" Width="125px" Visible="False">
            <table align="center" style="width: 116px">
                <tr>
                    <td class="ManagementPanel">
                        <uc6:ManageHOC ID="ManageHOC1" runat="server" />
                        <asp:LinkButton ID="lbHOCMain" runat="server" OnClick="lbHOCMain_Click">[ Return ] </asp:LinkButton>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        &nbsp;
        <uc9:ManageLoanStatus ID="ucManageLoanStatus" runat="server" />
    </form>
</body>
</html>
