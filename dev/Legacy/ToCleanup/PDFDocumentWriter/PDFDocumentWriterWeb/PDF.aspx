<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PDF.aspx.cs" Inherits="PDFDocumentWriterWeb.PDF" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table width=800>
    <tr><td width="20%">
        <asp:Label ID="Label1" runat="server" Text="AccountKey / LoanNumber"></asp:Label>
        </td><td>
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        </td></tr>
    <tr><td>
        <asp:Label ID="Label2" runat="server" Text="Select generation type"></asp:Label>
        </td><td>
        <asp:DropDownList ID="ddl" runat="server">
            <asp:ListItem Value="0">Select a legal Doc</asp:ListItem>
            <asp:ListItem Value="1">Opt In Varifix</asp:ListItem>
            <asp:ListItem Value="2">Opt In Interest Only</asp:ListItem>
            <asp:ListItem Value="3">Edge</asp:ListItem>
            <asp:ListItem Value="4">New Variable FL</asp:ListItem>
            <asp:ListItem Value="5">Varifix FL</asp:ListItem>
            <asp:ListItem Value="6">InterestOnly FL</asp:ListItem>
            <asp:ListItem Value="7">PL Disburse</asp:ListItem>
            <asp:ListItem Value="8">PL Legal Agreement</asp:ListItem>
         </asp:DropDownList>
        </td></tr>
    <tr><td colspan=2>
        <asp:Button ID="Button1" runat="server" Text="Go" onclick="Button1_Click" 
            style="width: 31px" />
    
        </td></tr>
        <tr><td colspan=2>
        <asp:Label runat=server ID=lblErr Text=""></asp:Label>
        <asp:Label runat=server ID=lblErr0 Text=""></asp:Label>
        </td></tr>
    </table>
    
    </div>
    </form>
</body>
</html>
