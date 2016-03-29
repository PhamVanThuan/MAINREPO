<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true" CodeBehind="ValuationSummary.aspx.cs" Inherits="SAHL.Web.Views.Common.ValuationSummary" Title="LightStone Valuation." EnableViewState="False" %>
<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
<h3 style="text-align:center"> LightStone Valuation Details </h3>
<script src="../../Scripts/jquery-1.5.min.js" type="text/javascript"></script>
<script language="javascript">
    $(document).ready(function () {
        //Default Action
        $(".tab_content").hide(); //Hide all content
        $("ul.tabs li:first").addClass("active").show(); //Activate first tab
        $(".tab_content:first").show(); //Show first tab content

        //On Click Event
        $("ul.tabs li").click(function () {
            $("ul.tabs li").removeClass("active"); //Remove any "active" class
            $(this).addClass("active"); //Add "active" class to selected tab
            $(".tab_content").hide(); //Hide all tab content
            var activeTab = $(this).find("a").attr("href"); //Find the rel attribute value to identify the active tab + content
            $(activeTab).fadeIn(); //Fade in the active content
            return false;
        });
    });
</script>
<asp:Xml runat="server" ID="panelXML"></asp:Xml>
</asp:Content>