<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="RCSUpload.aspx.cs" Inherits="SAHL.Web.Views.RCS.RCSUpload" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <table class="tableStandard" width="100%">
        <tr>
            <td align="left" style="height: 99%;" valign="top">
                <SAHL:SAHLGridView ID="UploadHistoryGrid" runat="server" AutoGenerateColumns="false"
                    FixedHeader="false" EnableViewState="false" GridHeight="140px" GridWidth="100%"
                    Width="100%" HeaderCaption="RCS Offer Import History" NullDataSetMessage="There is no RCS upload history."
                    EmptyDataSetMessage="No RCS upload History Found" OnRowDataBound="UploadHistory_RowDataBound">
                    <RowStyle CssClass="TableRowA" />
                </SAHL:SAHLGridView>
                <table style="width: 100%">
                    <tr id="FileNameRow" runat="server">
                        <td>
                            <SAHL:SAHLLabel ID="FileNameTitle" runat="server" Text="Upload New File"></SAHL:SAHLLabel>
                        </td>
                        <td colspan="2">
                            <asp:FileUpload ID="UploadFile" runat="server" Style="width: 400px;" />
                            <SAHL:SAHLTextBox ID="FileNameDisplay" runat="server" Style="width: 400px;">0</SAHL:SAHLTextBox>
                            <SAHL:SAHLCustomValidator ID="FileNameVal" runat="server" ControlToValidate="ValControl"
                                ErrorMessage="Please capture a file name." />
                            <SAHL:SAHLTextBox ID="ValControl" runat="server" Style="display: none;">0</SAHL:SAHLTextBox>
                            <SAHL:SAHLButton ID="UploadButton" runat="server" Text="Upload" AccessKey="U" OnClick="Upload_Click" />
                        </td>
                    </tr>
                </table>
                <div style="width: 100%; height: 130px; border: 1px solid #E5E5E5; overflow: auto;">
                    <table runat="server" id="ReplacementTable" style="width: 100%">
                        <tr>
                            <td align="left" style="height: 99%;" valign="top">
                                <asp:Table ID="ReplaceList" runat="server" Width="99%">
                                </asp:Table>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr id="ButtonRow" runat="server">
            <td align="right">
                <SAHL:SAHLButton ID="CancelButton" runat="server" Text="Reset" AccessKey="C" Visible="true"
                    OnClick="Cancel_Click" />
                <SAHL:SAHLButton ID="ResultsButton" runat="server" Text="View Results" AccessKey="S"
                    Visible="true" OnClick="Results_Click" />
                <SAHL:SAHLButton ID="ReplaceButton" runat="server" Text="Replace Data" AccessKey="S"
                    Visible="true" OnClick="Replace_Click" />
                <SAHL:SAHLButton ID="SubmitButton" runat="server" Text="Process" AccessKey="S" Visible="true"
                    OnClick="Submit_Click" />
                <SAHL:SAHLCustomValidator ID="ReplaceValidator" runat="server" ControlToValidate="dummyTextBox"
                    ErrorMessage="Please complete all the replacement values" />
                <SAHL:SAHLTextBox ID="dummyTextBox" runat="server" Style="display: none;">0</SAHL:SAHLTextBox>
            </td>
        </tr>
    </table>
</asp:Content>
