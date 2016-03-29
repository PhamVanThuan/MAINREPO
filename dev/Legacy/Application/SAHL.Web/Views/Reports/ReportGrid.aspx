<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Reports.ReportGrid" Title="Untitled Page" CodeBehind="ReportGrid.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <script type="text/javascript">

        function HideBusy() {
            dontShowBusy = true;
        }
    </script>
   <div style="max-height: 240px; overflow-y: auto">
        <SAHL:DXGridView ID="ReportDataGrid" runat="server" AutoGenerateColumns="true" Width="100%"
            EnableViewState="false" PostBackType="NoneWithClientSelect">
            <SettingsText Title="Report Data" />
            <Settings ShowTitlePanel="True" ShowGroupPanel="true" ShowFilterRow="true" ShowHeaderFilterButton="true"
                ShowVerticalScrollBar="false" ShowHorizontalScrollBar="false" VerticalScrollableHeight="300" />
            <SettingsPager Mode="ShowAllRecords" />
        </SAHL:DXGridView>
    </div>
    <br />
    <asp:Panel ID="pnlBatchReport" runat="server" GroupingText="Batch E-Mail Addresses"
        Width="99%" Visible="False">
        <br />
        <SAHL:SAHLLabel ID="SAHLTitle1" runat="server" Font-Size="smaller" Text="The selected report needs to run as a batch report.">
        </SAHL:SAHLLabel>
        <SAHL:SAHLLabel ID="SAHLTitle2" runat="server" CssClass="LabelText" Font-Size="Smaller"
            Text="Please provide the email addresses of the people that should receive this e-mail."
            TextAlign="Left"></SAHL:SAHLLabel>
        <br />
        <br />
        <SAHL:SAHLLabel ID="SAHLLabel1" runat="server" CssClass="LabelText" Font-Size="Smaller"
            Text="Please note: Only internal SAHL email addresses are valid"></SAHL:SAHLLabel><br />
        <br />
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="width: 35%">
                    <table width="100%">
                        <tr>
                            <td style="width: 52px">
                            </td>
                            <td style="width: 369px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 52px">
                            </td>
                            <td style="width: 369px">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 40%">
                                        </td>
                                        <td style="width: 60%">
                                        </td>
                                        <td align="right" style="width: 535px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 40%">
                                        </td>
                                        <td style="width: 60%">
                                        </td>
                                        <td align="right" style="width: 535px; height: 21px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 40%">
                                        </td>
                                        <td style="width: 60%">
                                        </td>
                                        <td align="right" style="width: 535px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 40%">
                                            <asp:Label ID="Label1" runat="server" Text="Email Address:" Width="108px"></asp:Label>
                                        </td>
                                        <td style="width: 60%">
                                            <asp:TextBox ID="txtEmailAddressAdd" runat="server" Width="182px"></asp:TextBox>
                                        </td>
                                        <td align="right" style="width: 535px; height: 26px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 40%">
                                        </td>
                                        <td align="center" style="width: 60%">
                                            <SAHL:SAHLButton ID="btnAddAddress" runat="server" OnClick="btnCancel_Click" OnClientClick="addItem();return false"
                                                Text="Add" UseSubmitBehavior="False" />
                                        </td>
                                        <td align="right" style="width: 535px">
                                        </td>
                                    </tr>
                                </table>
                                &nbsp; &nbsp; &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 52px">
                            </td>
                            <td style="width: 369px">
                            </td>
                        </tr>
                    </table>
                    &nbsp;&nbsp;
                    <asp:HiddenField ID="hiddenSelection" runat="server" />
                    <asp:HiddenField ID="HiddenInd" runat="server" Value="0" />
                </td>
                <td valign="top">
                    &nbsp; &nbsp;
                    <table style="width: 70%">
                        <tr>
                            <td style="width: 1%">
                            </td>
                            <td style="width: 99%">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 1%">
                            </td>
                            <td style="width: 99%">
                                <asp:ListBox ID="lstEmailAddresses" runat="server" Height="192px" Width="100%"></asp:ListBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 1%">
                            </td>
                            <td style="width: 99%">
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="width: 1%">
                            </td>
                            <td align="center" style="width: 99%">
                                <SAHL:SAHLButton ID="btnRemoveAddress" runat="server" OnClick="btnCancel_Click" OnClientClick="removeItem();return false"
                                    Text="Remove" UseSubmitBehavior="False" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <table id="tblTooManyRecords" width="100%" runat="server" visible="false">
        <tr>
            <td align="center">
                <SAHL:SAHLLabel ID="lblTooManyRecords" runat="server" CssClass="LabelText">Too many records to export to Excel! Please refine your query to return less than 65536 records.</SAHL:SAHLLabel>
            </td>
            <td style="width: 3px">
            </td>
        </tr>
    </table>
    <br />
    <table width="100%">
        <tr>
            <td align="right">
                <SAHL:SAHLButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" />
                <SAHL:SAHLButton ID="btnGo" runat="server" OnClick="btnGo_Click" OnClientClick="btnConfirmClick()"
                    Text="Go" Visible="False" />
                <SAHL:SAHLButton ID="btnExport" runat="server" OnClick="ExportReport_Click" Text="Export"
                    OnClientClick="masterCancelBeforeUnload()" />
            </td>
        </tr>
    </table>
</asp:Content>
