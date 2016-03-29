<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" Inherits="SAHL.Web.Views.ReleaseAndVariations.ERFDetails"
    Title="Account" Codebehind="ReleaseAndVariationsERFDetails.aspx.cs" %>
<%@ Register Src="WorkFlowHeader.ascx" TagName="WorkFlowHeader" TagPrefix="uc1" %>
<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
<table width="100%">
    <tr><td>
    <asp:Panel ID="Panel3" runat="server" GroupingText="Release & Variation ERF - Existing Security" Height="50px"
        Width="100%">
        <table width="100%">
            <tr>
                <td align="left" style="width: 10%" valign="top">
                    ERF</td>
                <td align="left" style="width: 30%" valign="top">
                    <SAHL:SAHLTextBox ID="txtExistingSecurity" runat="server" Rows="5" TextMode="MultiLine" Width="95%" BackColor="#FFFFC0"></SAHL:SAHLTextBox></td>
                <td align="left" style="width: 4%" valign="top">
                </td>
                <td align="left" valign="top">
                    &nbsp;<table width="100%">
                        <tr>
                            <td align="left" style="width: 25%">
                                EXT</td>
                            <td align="left">
                                &nbsp;<SAHL:SAHLTextBox ID="txtExistingSecurityEXT" runat="server" Width="250px" BackColor="#FFFFC0"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 25%; height: 29px;">
                                Valuation</td>
                            <td align="left">
                                &nbsp;<SAHL:SAHLTextBox ID="txtExistingSecurityValuation" runat="server" Width="250px" DisplayInputType="Currency" BackColor="#FFFFC0"></SAHL:SAHLTextBox></td>
                        </tr>
                    </table>
                    &nbsp;
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="Panel1" runat="server" GroupingText="Release & Variation ERF - To Be Released" Height="50px"
        Width="100%">
        <table width="100%">
            <tr>
                <td align="left" style="width: 10%" valign="top">
                    ERF</td>
                <td align="left" style="width: 30%" valign="top">
                    <SAHL:SAHLTextBox ID="txtToBeReleased" runat="server" Rows="5" TextMode="MultiLine"
                        Width="95%" BackColor="#FFFFC0"></SAHL:SAHLTextBox></td>
                <td align="left" style="width: 4%" valign="top">
                </td>
                <td align="left" valign="top">
                    &nbsp;<table width="100%">
                        <tr>
                            <td align="left" style="width: 25%">
                                EXT</td>
                            <td align="left">
                                &nbsp;<SAHL:SAHLTextBox ID="txtToBeReleasedExt" runat="server" Width="250px" BackColor="#FFFFC0"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 25%; height: 29px;">
                                Valuation</td>
                            <td align="left">
                                &nbsp;<SAHL:SAHLTextBox ID="txtToBeReleasedValuation" runat="server" Width="250px" DisplayInputType="Currency" BackColor="#FFFFC0"></SAHL:SAHLTextBox></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server" GroupingText="Release & Variation ERF - Remaining Security" Height="50px"
        Width="100%">
        <table width="100%">
            <tr>
                <td align="left" style="width: 10%" valign="top">
                    ERF</td>
                <td align="left" style="width: 30%" valign="top">
                    <SAHL:SAHLTextBox ID="txtRemainingSecurity" runat="server" Rows="5" TextMode="MultiLine"
                        Width="95%" BackColor="#FFFFC0"></SAHL:SAHLTextBox></td>
                <td align="left" style="width: 4%" valign="top">
                </td>
                <td align="left" valign="top">
                    &nbsp;<table width="100%">
                        <tr>
                            <td align="left" style="width: 25%">
                                EXT</td>
                            <td align="left">
                                &nbsp;<SAHL:SAHLTextBox ID="txtRemainingSecurityExt" runat="server" Width="250px" BackColor="#FFFFC0"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 25%; height: 29px;">
                                Valuation</td>
                            <td align="left">
                                &nbsp;<SAHL:SAHLTextBox ID="txtRemainingSecurityValuation" runat="server" Width="250px" DisplayInputType="Currency" BackColor="#FFFFC0"></SAHL:SAHLTextBox></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    </td></tr>
    <tr>
    <td align="right">
    <SAHL:SAHLButton ID="btnUpdate" runat="server" ButtonSize="Size4" CssClass="BtnNormal4 "
        Text="Update" Width="150px" Visible="False" OnClick="btnUpdate_Click" />
    <SAHL:SAHLButton ID="btnCancel" runat="server" ButtonSize="Size4" CssClass="BtnNormal4"
        Text="Done!" Width="147px" Visible="False" OnClick="btnCancel_Click" />&nbsp;
    </td>
    </tr>
    </table>
</asp:Content>
