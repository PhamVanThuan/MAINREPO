<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    CodeBehind="ContactClient.aspx.cs" Inherits="SAHL.Web.Views.Capitec.ContactClient" Title="Contact Client" ValidateRequest="false" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <div style="text-align: center">
        <table border="0" cellspacing="0" cellpadding="0" class="tableStandard" width="100%">
            <tr>
                <td align="left" style="height: 99%; width: 100%;" valign="top">
                    <br />
                    <br />
                    <table class="tableStandard" width="100%">
                        <tr>
                            <td style="width: 170px;">
                                <SAHL:SAHLLabel ID="lblContactDate" runat="server" Font-Bold="true">Date</SAHL:SAHLLabel>
                            </td>
                            <td class="cellDisplay" style="width: 170px;">
                                <SAHL:SAHLDateBox ID="dteContactDate" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td class="cellDisplay">&nbsp;</td>

                            <td class="TitleText" style="width: 679px">&nbsp;</td>
                            <td class="cellDisplay">&nbsp;</td>
                            <td style="width: 3px"></td>
                        </tr>

                        <tr>
                            <td valign="top">
                                <SAHL:SAHLLabel ID="CommentsLabel" runat="server" Font-Bold="true">Comments</SAHL:SAHLLabel>
                            </td>
                            <td colspan="4">
                                <SAHL:SAHLTextBox ID="txtComments" runat="server"
                                    TextMode="MultiLine" Width="100%" EnableViewState="False" CssClass="mandatory"
                                    Font-Names="Verdana" Font-Size="8pt" Height="129px"></SAHL:SAHLTextBox>
                                <br />
                            </td>
                            <td valign="top" style="width: 3px"></td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td colspan="4" style="text-align:right">
                                <SAHL:SAHLButton ID="SubmitButton" runat="server" Text="Submit" AccessKey="S" OnClick="SubmitButton_Click" />
                                <SAHL:SAHLButton ID="CancelButton" runat="server" Text="Cancel" AccessKey="C" OnClick="CancelButton_Click"
                                    CausesValidation="False" />
                            </td>
                            <td valign="top" style="width: 3px"></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
