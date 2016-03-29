<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Codebehind="X2InstanceRedirect.aspx.cs" Inherits="SAHL.Web.Views.Common.X2InstanceRedirect" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <div style="height: 20px">
    </div>
    <table style="width: 100%">
        <tr>
            <td align="center">
                <SAHL:SAHLLabel ID="Label1" runat="server" Text=""></SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td align="center">
                <SAHL:SAHLButton ID="btnRefresh" runat="server" Text="Refresh" />
            </td>
        </tr>
        <tr>
            <td align="center">
                Page will auto refresh in<input id="txtSecsRemaining" readonly="readonly" style="width: 18px; text-align: center;"
                    type="text" />seconds.</td>
        </tr>
    </table>

    <script language="javascript" type="text/javascript">
    function ReloadPage()
    {
        window.location.reload()
    }
    setInterval("ReloadPage()",7000)
    
    /* script to show time remaining until next refresh */
    var seconds=6
    document.getElementById("txtSecsRemaining").value='7'
    function ShowTimeRemaining()
    {
        document.getElementById("txtSecsRemaining").value=seconds
        seconds-=1
    }

    setInterval("ShowTimeRemaining()",1000)
   
    </script>

</asp:Content>
