<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true" CodeBehind="QuickCashFurtherLoan.aspx.cs" Inherits="SAHL.Web.Views.FurtherLending.QuickCashFurtherLoan" Title="Untitled Page" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
<script language="javascript" type="text/javascript">
    //do not uncomment this function
    function enableUpdateButton(enabled)
    {
        // Leave this function in, it is called by the common QC control
    }
</script>
<cc1:QuickCashPanel ID="QuickCashDetails" runat="server" Width="99%"></cc1:QuickCashPanel>
<br /><br />
<center>
    <SAHL:SAHLButton ID="btnSave" runat="server" ButtonSize="Size5" OnClick="btnSave_Click" CausesValidation="false" Text="Save" />
    <SAHL:SAHLButton ID="btnCancel" runat="server" ButtonSize="Size5" OnClick="btnCancel_Click" CausesValidation="false" Text="Cancel" />
</center>
</asp:Content>
