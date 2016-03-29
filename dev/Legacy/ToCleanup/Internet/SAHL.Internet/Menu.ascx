<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Menu.ascx.cs" Inherits="Menu" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%--<asp:ScriptManager  EnablePageMethods="false"  ID="scriptManager" runat="server" /> --%>

<ajaxToolkit:ToolkitScriptManager EnablePageMethods="false" CombineScripts="true" ScriptMode="Auto" ID="scriptManager" runat="server" />

<asp:LinkButton ID="LinkButton4" PostBackUrl="Default.aspx" runat="server" CausesValidation="False">Home</asp:LinkButton><br />
<asp:LinkButton ID="LinkButton1" PostBackUrl="SwitchCalculator.aspx" runat="server" CausesValidation="False">Switch Loan Calculator</asp:LinkButton><br />
<asp:LinkButton ID="LinkButton3" PostBackUrl="NewPurchaseCalculator.aspx" runat="server" CausesValidation="False">New Purchase Calculator</asp:LinkButton><br />
<asp:LinkButton ID="LinkButton2" PostBackUrl="AffordabilityCalculator.aspx" runat="server" CausesValidation="False">Affordability Calculator</asp:LinkButton>
<hr />
<asp:LinkButton ID="LinkButton5" PostBackUrl="Spectacular.aspx" runat="server" CausesValidation="False">Spectacular Ad Campaign</asp:LinkButton><br />
<asp:LinkButton ID="LinkButton6" PostBackUrl="ErrorHandler.aspx" runat="server" CausesValidation="False">403 & 404 Error Handler</asp:LinkButton><br />
<hr />
<asp:LinkButton ID="LinkButton7" PostBackUrl="Survey.aspx" runat="server" CausesValidation="False">Survey Module</asp:LinkButton><br />
<asp:LinkButton ID="LinkButton8" PostBackUrl="SendQ1MarketingMail.aspx" runat="server" CausesValidation="False">JSON Mailer -Q1 Marketing Campaign</asp:LinkButton><br />