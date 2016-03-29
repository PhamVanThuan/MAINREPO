<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocVerManagement.aspx.cs" Inherits="DocVerManagement" %>
<%@ Register TagPrefix="Util" TagName="title" Src="../common/title.ascx" %>
<%@ Register Src="../AssetTransfer/WebCalendar.ascx" TagName="WebCalendar" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Document Version Management</title>
    <link href="../style.css" rel="stylesheet" type="text/css" />
</head>
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0">
<Util:title ID="Title1" Runat="server" />
    <form id="form1" runat="server">
    <table align="center" width="550">
        <tr>
            <td colspan="2" align="center">
                Please make sure documents are signed and returned before capturing and accepting
                the data below.
            </td>
        </tr>
        <tr>
            <td width="150" height="25">Loan number</td><td width="400">
                <asp:TextBox ID="txtLoanNumber" runat="server" ReadOnly="True"></asp:TextBox></td>
        </tr>
        <tr>
            <td height="25">Client name</td><td>
                <asp:TextBox ID="txtClientName" runat="server" Width="399px" ReadOnly="True"></asp:TextBox></td>
        </tr>
        <tr>
            <td height="25">
                Date printed</td><td style="height: 21px">
                    <asp:TextBox ID="txtDatePrinted" runat="server" ReadOnly="True"></asp:TextBox></td>
        </tr>
        <tr style="display: none;">
            <td height="25">
                Date signed</td><td width="250">
                <uc1:WebCalendar ID="wcDateReceived" runat="server" />
            </td>
        </tr>
        <tr>
            <td height="25">Latest version</td><td><%=sUpdatedYN%></td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <table style="border: solid 1px black">
                    <tr>
                        <td>&nbsp;</td><td>Version</td><td>Received</td><td>Date</td>
                    </tr>
                    <tr>
                        <td height="25">Loan agreement</td><td>
                            <asp:TextBox ID="txtLAVersion" runat="server" Width="80px" Enabled="False"></asp:TextBox></td><td>
                            <asp:CheckBox ID="chkLAReceived" runat="server" /></td><td>
                            <uc1:WebCalendar ID="wcLADate" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td height="25">Loan schedule</td><td>
                            <asp:TextBox ID="txtLSVersion" runat="server" Width="80px" Enabled="False"></asp:TextBox></td><td>
                            <asp:CheckBox ID="chkLSReceived" runat="server" /></td><td>
                            <uc1:WebCalendar ID="wcLSDate" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td height="25"></td><td>
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" /></td>
        </tr>
        <tr>
            <td colspan="2" align="center">
            </td>
        </tr>
        <tr>
            <td colspan="2" height="25"><%=txtError.Text%></td>
        </tr>
    </table>
        <asp:TextBox ID="txtError" runat="server" Visible="False"></asp:TextBox>
    </form>
</body>
</html>
