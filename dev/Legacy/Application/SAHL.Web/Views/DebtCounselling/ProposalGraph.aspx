<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProposalGraph.aspx.cs" MasterPageFile="~/MasterPages/Blank.master" Inherits="SAHL.Web.Views.DebtCounselling.ProposalGraph" %>

<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="sahlControls" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2, Version=10.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.XtraCharts.v10.2, Version=10.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="charts" %>
<%@ Register Assembly="DevExpress.XtraCharts.v10.2.Web, Version=10.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxCharts" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
	<script src="../../Scripts/jquery-1.5.min.js" type="text/javascript"></script>
	<script src="../../Scripts/Plugins/Lightbox/jquery.fancybox-1.3.4.pack.js" type="text/javascript"></script>
	<link href="../../Scripts/Plugins/Lightbox/jquery.fancybox-1.3.4.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript">
		$(document).ready(function () {
			$("#<%= proposalGraph.ClientID %>").width("180");
			$("#<%= proposalGraph.ClientID %>").height("120");
			$("#data").attr("src", $("#<%= proposalGraph.ClientID %>").attr("src"));
			$("#inline").fancybox({
			'hideOnContentClick' : true
		});
	});
	</script>
	<div style="display: none">
		<img id="data" /></div>
	<a id="inline" href="#data">
		<sahlControls:ProposalGraph ID="proposalGraph" runat="server" Width="800" Height="600">
		</sahlControls:ProposalGraph>
	</a>
</asp:Content>
