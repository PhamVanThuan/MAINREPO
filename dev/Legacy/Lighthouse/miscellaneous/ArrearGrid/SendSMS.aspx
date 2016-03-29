<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SendSMS.aspx.cs" Inherits="ArrearGrid_SendSMS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Send SMS</title>
</head>
<body style="width: 850px; height: 350px" bgcolor="#e0e0e0">
    <form id="frmSendSMS" runat="server">
    <div>
       <table id="tMain" width="100%">
          <tr>
             <td width="50%">
                SMS loading date [dd/mm/yyyy]</td>
             <td width="50%">
                <asp:TextBox ID="txtSMSDate" runat="server"></asp:TextBox>
                &nbsp;<asp:Button ID="btnSearchSMS" runat="server" Text="Search" style="cursor: hand" OnClick="btnSearchSMS_Click" /></td>
          </tr>
          <tr>
             <td width="50%">
                No. of cases loaded</td>
             <td width="50%">
                <asp:TextBox ID="txtTotalCases" runat="server" ReadOnly="True">0</asp:TextBox></td>
          </tr>
          <tr>
             <td width="50%">
                No. of messages to be sent</td>
             <td width="50%">
                <asp:TextBox ID="txtTotalSms" runat="server" ReadOnly="True"></asp:TextBox></td>
          </tr>
          <tr>
             <td colspan="2">
                <asp:Button ID="btnProcessSMS" runat="server" Text="Send SMS" style="cursor: hand" OnClick="btnProcessSMS_Click" /></td>
          </tr>
          <tr>
             <td colspan="2">
                Message Details</td>
          </tr>
          <tr>
             <td colspan="2" valign="top">
                <asp:Label ID="lblErr" runat="server" ForeColor="Red" Text="Error" Visible="False"></asp:Label></td>
          </tr>
          <tr>
             <td colspan="2" valign="top">
                <asp:GridView ID="gvSMS" runat="server" CellPadding="4" Font-Size="Small" ForeColor="#333333" GridLines="None">
                   <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                   <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                   <EditRowStyle BackColor="#999999" />
                   <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                   <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                   <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                   <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                </asp:GridView>
             </td>
          </tr>
       </table>
    
    </div>
    </form>
</body>
</html>
