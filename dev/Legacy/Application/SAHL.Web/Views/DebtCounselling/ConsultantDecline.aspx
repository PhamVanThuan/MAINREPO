<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Blank.master"
    CodeBehind="ConsultantDecline.aspx.cs" Inherits="SAHL.Web.Views.DebtCounselling.ConsultantDecline" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <script type="text/javascript">
 
    </script>
    <div style="text-align: left">
        <table width="100%" class="tableStandard">
            <tr>
                <td>
                    <SAHL:DXGridView ID="gvProposals" runat="server" AutoGenerateColumns="False" PostBackType="NoneWithClientSelect"
                        Width="100%" KeyFieldName="Key" ClientInstanceName="grid" Settings-ShowTitlePanel="true"
                        OnHtmlDataCellPrepared="gvProposals_HtmlDataCellPrepared" Settings-ShowGroupPanel="false">
                        <SettingsText Title="Select Proposal to Decline" EmptyDataRow="No Proposals Exist" />
                    </SAHL:DXGridView>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False"
                        EnableViewState="False" OnClick="btnCancel_Click" />
                    <SAHL:SAHLButton ID="btnSubmit" runat="server" Text="Submit" CausesValidation="False"
                        EnableViewState="False" OnClick="btnSubmit_Click" />
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hiddenSelection" runat="server" />
</asp:Content>
