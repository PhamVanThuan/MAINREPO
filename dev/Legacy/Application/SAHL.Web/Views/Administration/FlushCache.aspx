<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FlushCache.aspx.cs" Inherits="SAHL.Web.Views.Administration.FlushCache" MasterPageFile="~/MasterPages/Blank.master" %>

<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <style type="text/css">
    .block
    {
        float:left;
        margin-left:10px;
        margin-bottom:10px;
        width:250px;
        height:140px;
        margin-right:10px;
    }
    .blockRow
    {
        padding:4px;
    }
    .blockRow div
    {
        padding-bottom: 2px;
    }
    .message
    {
        float:left;
        display:block;
        width:95%;
        padding:4px;
        margin:10px;
    }
    </style>
    <script language="javascript">
    function confirmLookup()
    {
        var ddlLookup = document.getElementById('<%=ddlLookup.ClientID %>');
        var selectedValue = ddlLookup.options[ddlLookup.selectedIndex].value;
        
        if (selectedValue.toUpperCase() == 'ALL')
            return confirm('Are you sure you want to clear ALL lookups?');
        else    
            return confirm('Are you sure you want to clear the lookups \'' + selectedValue + '\'?');
    }
    
    function confirmUserAccess()
    {
        var txtUserAccess = document.getElementById('<%=txtUserAccess.ClientID %>');
        if (txtUserAccess.value.trim().length == 0)
            return false;
        else
            return confirm('Are you sure you want to reload the user\'s access?')
    }

    // toggles the enabled status of the lookups button
    function toggleLookupButton(ddl)    
    {
        document.getElementById('<%=btnLookup.ClientID %>').disabled = (ddl.selectedIndex == 0);
    }
    </script>
    <asp:Panel ID="pnlMain" runat="server" Width="99%">
        <div ID="lblMsg" runat="server" class="fontBigger message">&nbsp;</div>
        <!--
        <div class="borderAll block">
            <div class="borderBottom backgroundLight blockRow">
                <strong>Clear All</strong>
            </div>
            <div class="blockRow">Clear all items from the cache. </div>
            <div class="blockRow" style="text-align:right;">
                <SAHL:SAHLButton ID="btnClearAll" runat="server" Text="Clear All" OnClientClick="return confirm('Are you sure you want to clear everything from the cache?\n\nThis may cause errors for users currently on the system.');" OnClick="btnClearAll_Click"></SAHL:SAHLButton>
            </div>
        </div>
        -->
        <div class="borderAll block">
            <div class="borderBottom backgroundLight blockRow">
                <strong>Clear User Access</strong>
            </div>
            <div class="blockRow">Clear a user's access rights to force the rebuilding of feature keys and CBO items available for selection. </div>
            <div class="blockRow" style="padding-top:20px;">
                <div class="cell">User login: </div>
                <div class="cell" style="float:right;">
                    <SAHL:SAHLTextBox ID="txtUserAccess" runat="server" MaxLength="100" Width="150"></SAHL:SAHLTextBox>
                    <SAHL:SAHLAutoComplete ID="acUserAccess" runat="server" TargetControlID="txtUserAccess" ServiceMethod="SAHL.Web.AJAX.AdUser.GetAdUsers" MinCharacters="3" AutoPostBack="false" />
                </div>
            </div>
            <div class="blockRow" style="text-align:right;">
                <SAHL:SAHLButton ID="btnUserAccess" runat="server" Text="Clear" OnClientClick="return confirmUserAccess();" OnClick="btnUserAccess_Click"></SAHL:SAHLButton>
            </div>
        </div>

        <div class="borderAll block">
            <div class="borderBottom backgroundLight blockRow">
                <strong>Clear Lookups</strong>
            </div>
            <div class="blockRow">Clear lookups from the cache: they will be reloaded the next time they're accessed.  Only lookup items that have been accessed will be displayed here.</div>
            <div class="blockRow" style="padding-top:20px;">
                <div class="cell">Select item: </div>
                <div class="cell" style="float:right;">
                    <SAHL:SAHLDropDownList ID="ddlLookup" runat="server" Width="150" onchange="toggleLookupButton(this)"></SAHL:SAHLDropDownList>
                </div>
            </div>
            <div class="blockRow" style="text-align:right;">
                <SAHL:SAHLButton ID="btnLookupAll" runat="server" Text="Clear All" OnClientClick="return confirm('Are you sure you want to clear all lookups from the cache?');" OnClick="btnLookupAll_Click"></SAHL:SAHLButton>
                <SAHL:SAHLButton ID="btnLookup" runat="server" Text="Clear" OnClientClick="return confirmLookup();" OnClick="btnLookup_Click"></SAHL:SAHLButton>
            </div>
        </div>
        
        <div class="borderAll block">
            <div class="borderBottom backgroundLight blockRow">
                <strong>Clear UIStatements</strong>
            </div>
            <div class="blockRow">Clear all UIStatements from the cache. </div>
            <div class="blockRow" style="text-align:right;">
                <SAHL:SAHLButton ID="btnUIStatement" runat="server" Text="Clear All" OnClientClick="return confirm('Are you sure you want to clear all UIStatements from the cache?.');" OnClick="btnUIStatement_Click"></SAHL:SAHLButton>
            </div>
        </div>
        
        <div class="borderAll block">
            <div class="borderBottom backgroundLight blockRow">
                <strong>Clear X2 UserOrganisation Cache</strong>
            </div>
            <div class="blockRow">Clear all cached items in X2.</div>
            <div class="blockRow" style="text-align:right;">
                <SAHL:SAHLButton ID="btnOrgStructure" runat="server" Text="Clear All" OnClientClick="return confirm('Are you sure you want to clear the X2 Cache?');" OnClick="btnOrgStructure_Click"></SAHL:SAHLButton>
            </div>
        </div>

        <div class="borderAll block">
            <div class="borderBottom backgroundLight blockRow">
                <strong>Clear RuleItem Cache</strong>
            </div>
            <div class="blockRow">Clear RuleItem Cache.</div>
            <div class="blockRow" style="text-align:right;">
                <SAHL:SAHLButton ID="btnRuleItem" runat="server" Text="Clear All" OnClientClick="return confirm('Are you sure you want to clear the X2 Cache?');" OnClick="btnRuleItem_Click"></SAHL:SAHLButton>
            </div>
        </div>
        
    </asp:Panel>
</asp:Content>
