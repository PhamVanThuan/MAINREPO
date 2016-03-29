<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SwitchCalculator.aspx.cs" Inherits="Views_SwitchCalculator" %>

<%@ Register Src="Menu.ascx" TagPrefix="menu" TagName="SideMenu" %>
<%@ Register Src="~/Components/Calculators/GeneralCalculator.ascx" TagPrefix="SAHL" TagName="SwitchCalculator" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="~/Components/SessionVariables.ascx"  TagName="SessionVariables" TagPrefix="SAHL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<html>
<head id="Head1" runat="server">
    <title>SAHL Internet Components Development Framework - Switch Loan Calculator</title>
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
                    <SAHL:SwitchCalculator
                     ID="SwitchCalculator" 
                     CalculatorMode="2" 
                     NavigateTo="http://localhost/internet/ApplicationForm.aspx"
                     CalculatorName="SWITCH YOUR HOME LOAN CALCULATOR"
                     CalculatorDescriptionText="Fill in your Income and Loan details to find out how much you can save by switching your bond to us. This switch loan calculator will allow you to apply for a Home Loan with SA Home Loans."
                     StepImageURLNewPurchase="http://localhost/internet/images/steps/affordability1.jpg" 
                     StepImageURLSwitchLoan="http://localhost/internet/images/steps/switch1.jpg"  
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
