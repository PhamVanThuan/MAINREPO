<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestHarness.aspx.cs" Inherits="SAHL.Web.Services.Test.TestHarness" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:RadioButton ID="RadioButton1" runat="server" GroupName="Group1" Text="InternetLead" Checked="true" />
        <asp:RadioButton ID="RadioButton2" runat="server" GroupName="Group1" Text="InternetApplication" Checked="false" />
        <asp:RadioButton ID="RadioButton3" runat="server" GroupName="Group1" Text="MobisiteLead" Checked="false" />
        <asp:RadioButton ID="RadioButton4" runat="server" GroupName="Group1" Text="MobisiteApplication" Checked="false" />
        <br />
        <asp:Button ID="Button1" runat="server" Text="Test CreateInternetLead" OnClick="Button1_Click" />
    </div>
    <br />
    <div>
        <asp:CheckBox ID="CheckBox1" runat="server" Text="Is Self Employed" />
        <br />
        <asp:Button ID="Button4" runat="server" Text="Test GetMaximumPurchasePrice" OnClick="Button2_Click" />
        <br />
        <asp:Label ID="lblMaximumPurchasePrice" runat="server" Text="0" />
    </div>
    </form>
</body>
</html>
