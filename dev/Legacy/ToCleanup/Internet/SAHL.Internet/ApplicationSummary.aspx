<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ApplicationSummary.aspx.cs" Inherits="Views_ApplicationSummary" %>
<%@ Register Src="Menu.ascx" TagPrefix="menu" TagName="SideMenu" %>
<%@ Register Src="~/Components/Calculators/ApplicationFormSummary.ascx" TagPrefix="SAHL" TagName="ApplicationSummary" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="~/Components/SessionVariables.ascx"  TagName="SessionVariables" TagPrefix="SAHL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<html>
<head id="Head1" runat="server">
    <title>SAHL Internet Components Development Framework - Application Summary</title>
    <link  rel="stylesheet" href="/CSS/skin.css"  type="text/css" />
</head>
<SAHL:SessionVariables ID="SessionVars" runat="server" />
<body>
    <form id="form1" runat="server">
    <div>
            <table border="0" cellpadding="10px" cellspacing="10px" style="width: 100%">
                <tr>
                <td style="width: 20%">
                    <img src="/images/logo.gif" /></td>
                    <td colspan="2" style="width: 80%" align="center">
                    <h3>SAHL Internet Component Development Framework</h3>
                     </td>
                </tr>
                <tr>
                    <td style="height: 500px" align="right" valign="top">
                     <menu:SideMenu id="SideMenu" runat="server" />
                    </td>
                    <td style="height: 500px">
                    <SAHL:ApplicationSummary ID="ApplicationSummary" 
                     StepImageURLAffordability="http://localhost/internet/images/steps/affordability3.jpg" 
                     StepImageURLNewPurchase="http://localhost/internet/images/steps/affordability3.jpg" 
                     StepImageURLSwitchLoan="http://localhost/internet/images/steps/switch3.jpg"  
                    runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 50px">
                    </td>
                </tr>
            </table>
    </div>
    </form>
</body>
</html>