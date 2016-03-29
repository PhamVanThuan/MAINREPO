<%@ Page Language="C#" AutoEventWireup="true" Inherits="ApplicationError" Codebehind="ApplicationError.aspx.cs" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>HALO :: Error</title>
    <link rel="stylesheet" type="text/css" href="CSS/Common.css">
    <link rel="stylesheet" type="text/css" id="styles" runat="server">
    <style type="text/css">
    body 
    {
        margin: 0px;
    	font-size: 0.75em;
    }
    h2 
    {
        font-size:1.7em;
    }

    #buttonsPanel
    {
        text-align:center;
        margin-top:15px;
        width:100%;
    }
    #content 
    {
        position:relative;
        padding:0px 20px 0px 20px;
    }

    .code
    {
        background-color:#F0F0F0;;
        display:block;
        padding:5px;
        border:solid 1px silver;
        margin-top:5px;
        font-family:Courier New;
    }
    .stackTrace 
    {
        display:block;
        width:100%;
        margin-top:15px;
    }
    </style>
</head>
<body class="Error">
    <form id="Form1" action="Default.aspx" runat="server">
        <div id="masterLogo" style="clear:both;">&nbsp;</div>
        <div id="content">
            
            <h2 style="padding-top:5px;">Error</h2>
            <hr size="1" />
            An error has occurred in the application: <SAHL:SAHLLabel ID="lblError" runat="server" Font-Bold="true" />
            <asp:Label ID="lblStackTrace" runat="server" CssClass="stackTrace" Visible="false">
            </asp:Label>
            <div id="buttonsPanel">
                <SAHL:SAHLButton ID="HomePageButton" runat="server" ButtonSize="Size6" Text="Please Start Again"
                    OnClick="HomePageButton_Click" PostBackUrl="~/Default.aspx" />
            </div>
        </div>
    </form>
</body>
</html>
