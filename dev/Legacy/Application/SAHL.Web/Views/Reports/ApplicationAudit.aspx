<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true" CodeBehind="ApplicationAudit.aspx.cs" Inherits="SAHL.Web.Views.Reports.ApplicationAudit" Title="Untitled Page" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <div class="row" style="margin:auto;background-color:Transparent;">
        <div class="titleText cellInput" style="padding:2px;">Application Number: </div>
        <div class="cellInput" style="margin-left:30px;">
            <SAHL:SAHLTextBox ID="txtApplicationKey" runat="server" />
            <SAHL:SAHLAutoComplete ID="acApplicationKey" runat="server" TargetControlID="txtApplicationKey" ServiceMethod="SAHL.Web.AJAX.Application.GetApplicationKeys" AutoPostBack="false" MinCharacters="3" />
        </div>
        <div class="cellInput" style="margin-left:10px;">
            <SAHL:SAHLButton ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
        </div>
    </div>
    <div style="width:99%;" class="row">
        <SAHL:SAHLLabel ID="lblNoData" runat="server" Visible="false">No audit data found.</SAHL:SAHLLabel>
        <asp:Repeater ID="repAudit" runat="server" Visible="false">
            <ItemTemplate>
                <div style="width:90%;padding:5px;" class="<%=BackgroundCssClass %> borderAll">
                    <div class="row rowStandard" style="padding-top:4px;">
                        <div class="cell titleText" style="width:150px;">User:</div>
                        <div class="cell"><%# DataBinder.Eval(Container.DataItem, "WindowsLogon") %></div>
                    </div>
                    <div class="row rowStandard" style="width:90%;">
                        <div class="cell titleText" style="width:150px;">Audit Date:</div>
                        <div class="cell"><%# DataBinder.Eval(Container.DataItem, "AuditDate") %></div>
                    </div>
                    <div class="row rowStandard" style="width:90%;">
                        <div class="cell titleText" style="width:150px;">Area:</div>
                        <div class="cell"><%# DataBinder.Eval(Container.DataItem, "TableName") %> (<%# DataBinder.Eval(Container.DataItem, "PrimaryKeyValue") %>)</div>
                    </div>
                    <table class="borderAll tableStandard" style="width:99%;">
                        <tr>
                            <td style="width:50%" class="borderBottom">&nbsp;</td>
                            <td style="width:25%" class="titleText borderLeft borderBottom">Previous</td>
                            <td style="width:25%" class="titleText borderLeft borderBottom">Current</td>
                        </tr>
                        <asp:Repeater ID="repValues" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td w class="borderBottom"><%# DataBinder.Eval(Container.DataItem, "Title") %></td>
                                    <td class="borderLeft borderBottom"><%# DataBinder.Eval(Container.DataItem, "Previous") %></td>
                                    <td class="borderLeft borderBottom"><%# DataBinder.Eval(Container.DataItem, "Current") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
                <br />
            </ItemTemplate>
        </asp:Repeater>
    </div>        
</asp:Content>
