<%@ Page Language="C#" CodeFile="ManageTransactions.aspx.cs" Inherits="ManageTransactions_aspx" %>

<%@ Register Src="../Common/UserControls/ManageLoanStatus.ascx" TagName="ManageLoanStatus"
    TagPrefix="uc3" %>
<%@ Register TagPrefix="uc2" TagName="ManageLoan" Src="../common/UserControls/ManageLoan.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ManageTransaction" Src="../common/UserControls/ManageTransaction.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="../style.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="frmMain" runat="server">
    <div>
        <table cellspacing="0" cellpadding="0" width="900">
            <tr>
                <td class="ManagementPanel" colspan="2">
                    <uc2:ManageLoan ID="ucMTManageLoan" Runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
        <uc1:ManageTransaction ID="ucMTManageTransaction" Runat="server" />
                </td>
            </tr>
            
        </table>
      <uc3:ManageLoanStatus ID="ucManageLoanStatus" runat="server" />
    </div>
        
    </form>
</body>
</html>
