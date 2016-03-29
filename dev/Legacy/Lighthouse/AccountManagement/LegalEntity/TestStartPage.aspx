<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestStartPage.aspx.cs" Inherits="Main" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="../style.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            <asp:Label ID="lblAccountKey" runat="server" CssClass="style.css" Text="Enter an Account Key:"></asp:Label>
            <asp:TextBox ID="txtAccountKey" runat="server" CssClass="style.css"></asp:TextBox>
            <asp:Button ID="cmdFind" runat="server" CssClass="style.css" OnClick="cmdFind_Click"
                Text="Find details" />
        </div>
    
    </div>
    </form>
</body>
</html>
