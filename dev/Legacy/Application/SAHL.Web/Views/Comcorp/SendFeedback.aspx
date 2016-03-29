<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    CodeBehind="SendFeedback.aspx.cs" Inherits="SAHL.Web.Views.Comcorp.SendFeedback" Title="Send Feedback" ValidateRequest="false" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <SAHL:SAHLLabel ID="CommentsLabel" runat="server" Font-Bold="true">Comments</SAHL:SAHLLabel>
    <table class="tableStandard" width="100%">
        <tr>
            <td>
                <SAHL:SAHLTextBox ID="txtComment" runat="server"
                    TextMode="MultiLine" Width="100%"  EnableViewState="False" CssClass="mandatory"
                    Font-Names="Verdana" Font-Size="8pt" Height="129px"></SAHL:SAHLTextBox>
            </td>
        </tr>
        <tr>
            <td style="text-align: right">
                <SAHL:SAHLButton ID="SubmitButton" runat="server" Text="Submit" AccessKey="S" OnClick="SubmitButton_Click" />
                <SAHL:SAHLButton ID="CancelButton" runat="server" Text="Cancel" AccessKey="C" OnClick="CancelButton_Click" CausesValidation="False" />
            </td>
        </tr>
    </table>
</asp:Content>