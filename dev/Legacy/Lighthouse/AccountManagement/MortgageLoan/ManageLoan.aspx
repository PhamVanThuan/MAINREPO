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
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript" src="../Common/Scripts/popup.js" ></script>
    <title>Manage Loan</title>
    <link href="../style.css" type="text/css" rel="stylesheet" />
</head>

<body onload="InitializePopup();">
    <form id="form1" runat="server">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td colspan="2">
                    <table border="0" cellpadding="0" cellspacing="0" width="900">
                        <tr>
                            <td class="ManagementPanel">
                                <uc1:ManageLoan ID="ucMLManageLoan" runat="server" Visible="true" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <table border="0" cellpadding="0" cellspacing="1" width="100%">
                        <tr>
                            <td class="ManagementPanel">
                                <uc3:ManageLoanRates ID="ucMLManageLoanRates" runat="server" Visible="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="ManagementPanel">
                                <uc4:ManageLoanBreakdown ID="ucMLManageLoanBreakdown" runat="server" Visible="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="ManagementPanel" valign="top">
                                <uc7:ManageLoanDetail ID="ucMLManageLoanDetail" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <table border="0" cellpadding="0" cellspacing="1" width="100%">
                        <tr>
                            <td class="ManagementPanel" valign="top">
                                <uc2:ManageBankAccount ID="ucMLManageBankAccount" runat="server" Visible="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="ManagementPanel">
                                <uc5:ManageDebitOrder ID="ucMLManageDebitOrder" runat="server" Visible="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="ManagementPanel">
                                <uc6:ManageHOC ID="ucMLManageHOC" runat="server" Visible="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="ManagementPanel">
                                <uc8:LoanLinks ID="LoanLinks1" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <uc9:ManageLoanStatus ID="ucManageLoanStatus" runat="server" />
    </form>
</body>
</html>
