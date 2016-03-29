<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Cap.CapCreateSearch" Title="CAP Create Search" Codebehind="CapCreateSearch.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <script language="javascript" type="text/javascript">
    
        function doSearch(btnSearchId)
        {
            if (window.event.keyCode == 13)
            {
                window.event.returnValue = false;
                document.getElementById(btnSearchId).click();
            }
        }
        
    </script>
    <table id="CapPage" runat="server" width="100%" class="tableStandard">
        <tr class="rowStandard">
            <td style="width: 20%" class="TitleText">
                Account Type
            </td>
            <td style="width: 30%">
                <SAHL:SAHLDropDownList ID="ddlAccountType" runat="server">
                </SAHL:SAHLDropDownList>
            </td>
            <td style="width: 50%">
            </td>
        </tr>
        <tr class="rowStandard">
            <td style="width: 20%" class="TitleText">
                Account Number
            </td>
            <td style="width: 30%">
                <SAHL:SAHLTextBox ID="txtAccountNumber" runat="server" Width="165px"></SAHL:SAHLTextBox>
            </td>
            <td style="width: 50%">
            </td>
        </tr>
        <tr>
        </tr>
        <tr>
        </tr>
        <tr>
            <td>
            </td>
            <td>
            </td>
            <td align="left">
                <SAHL:SAHLButton ID="SearchButton" runat="server" Text="Search" AccessKey="C" CausesValidation="false" OnClick="SearchButton_Click" />
            </td>
        </tr>
        <tr>
        </tr>
        <tr>
            <td colspan="5">
                <SAHL:SAHLGridView ID="CapSearchGrid" runat="server" AutoGenerateColumns="false"
                    FixedHeader="false" EnableViewState="false" GridHeight="200px" GridWidth="100%"
                    Width="100%" HeaderCaption="Search Results"
                    NullDataSetMessage="" EmptyDataSetMessage="Account Number does not exist" OnGridDoubleClick="SearchGridDoubleClick">
                    <HeaderStyle CssClass="TableHeaderB" />
                    <RowStyle CssClass="TableRowA" />
                </SAHL:SAHLGridView>
            </td>
        </tr>
        <tr id="ButtonRow" runat="server">
            <td>
            </td>
            <td>
            </td>
            <td align="right">
                <SAHL:SAHLButton ID="CancelButton" runat="server" Text="Cancel" AccessKey="C" CausesValidation="false"
                    OnClick="CancelButton_Click" />
                <SAHL:SAHLButton ID="SubmitButton" runat="server" Text="Create Cap2 Offer" ButtonSize="Size6"
                    AccessKey="S" OnClick="SubmitButton_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
