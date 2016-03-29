<%@ Import Namespace="SAHL.Internet.Components.Other" %>
<%@ Control Language="C#" AutoEventWireup="true" Codebehind="ErrorHandler.ascx.cs"
    Inherits="SAHL.Internet.Components.Other.ErrorHandler" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<script type="text/javascript">
  var GOOG_FIXURL_LANG = 'en';
  var GOOG_FIXURL_SITE = 'http://www.sahomeloans.com/';
</script>

<asp:Panel ID="UpdatePanel" runat="server">
        <asp:Panel ID="pnlErrorHandler" runat="server" Visible="true">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td align="left">
                        <div class="inner-breadcrumb">
                         <h1><a id="A1" class="inner-breadcrumb-title" runat="server" href=""><font style="font-size:16px; font-weight: bold; color:#FFFFFF"><asp:Label runat="server" ID="ErrorHeading" Text="The page you requested could not be found." /></font></a></h1><a class="inner-breadcrumb-next">FOR ASSISTANCE DIAL 0860 103729</a>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="rowspacer">
                    </td>
                </tr>
                <tr>
                    <td class="rowspacer">
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <span class="radio">
                            <br />
                            <asp:Label ID="ResponseLabel" Font-Size="14px" runat="server" />
                            <asp:Label ID="ErrorLabel" Font-Size="14px" runat="server">The page you are looking for might have been removed, had its name changed, or is temporarily unavailable.</asp:Label>
                            </span>
                    </td>
                </tr>
                <tr>
                    <td class="rowspacer">
                        <br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="redline">
                    </td>
                </tr>
            </table>
        </asp:Panel>
</asp:Panel>
