<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="Memo.aspx.cs" Inherits="SAHL.Web.Views.Common.Memo" Title="Generic Memo" ValidateRequest="false" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <div style="text-align: center">
        <table border="0" cellspacing="0" cellpadding="0" class="tableStandard" width="100%">
            <tr>
                <td align="left" style="height: 99%; width: 100%;" valign="top">
                    <table border="0">
                        <tr>
                            <td style="width: 176px;">
                                <SAHL:SAHLLabel ID="AccountMemoStatusTitle" runat="server" CssClass="LabelText"></SAHL:SAHLLabel>
                            </td>
                            <td style="width: 186px;">
                                <SAHL:SAHLDropDownList ID="ddlMemoStatus" runat="server" AutoPostBack="True" PleaseSelectItem="false"
                                    OnSelectedIndexChanged="ddlMemoStatus_SelectedIndexChanged">
                                </SAHL:SAHLDropDownList>
                            </td>
                            <td style="width: 15px;">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    <br />
                    <SAHL:SAHLGridView ID="MemoRecordsGrid" runat="server" AutoGenerateColumns="false"
                        FixedHeader="false" EnableViewState="false" GridHeight="200px" GridWidth="100%"
                        Width="100%" NullDataSetMessage="No memo records exist." OnRowDataBound="MemoRecordsGrid_RowDataBound"
                        OnSelectedIndexChanged="MemoRecordsGrid_SelectedIndexChanged" EmptyDataSetMessage="No memo records exist."
                        PageSize="20">
                                                <HeaderStyle CssClass="TableHeaderB" />
                        <RowStyle CssClass="TableRowA" />
                    </SAHL:SAHLGridView>
                    <br />
                    <table class="tableStandard" width="100%" >
                        <tr id="CaptureRow" runat="server">
                            <td style="width: 170px;">
                                <SAHL:SAHLLabel ID="InsertDateTitle" runat="server" Font-Bold="true">Insert Date</SAHL:SAHLLabel>
                            </td>
                            <td class="cellDisplay" style="width: 170px;">
                                <SAHL:SAHLLabel ID="InsertDate" runat="server">-</SAHL:SAHLLabel>
                            </td>
                            <td style="width: 30px;">
                            </td>
                            <td style="width: 170px;">
                                <SAHL:SAHLLabel ID="CapturedByTitle" runat="server" Font-Bold="true">Captured By</SAHL:SAHLLabel>
                            </td>
                            <td  class="cellDisplay" style="width: 170px;">
                                <SAHL:SAHLLabel ID="CapturedBy" runat="server">-</SAHL:SAHLLabel>
                            </td>
                            <td style="width: 3px;">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 170px;">
                                <SAHL:SAHLLabel ID="ExpiryDateTitle" runat="server" Font-Bold="true">Expiry Date</SAHL:SAHLLabel>
                            </td>
                            <td  class="cellDisplay" style="width: 170px;">
                                <SAHL:SAHLLabel ID="ExpiryDate" runat="server">-</SAHL:SAHLLabel>
                                <SAHL:SAHLDateBox ID="ExpiryDateUpdate" runat="server" />
                            </td>
                            <td style="width: 30px;">
                                &nbsp;</td>
                            <td>
                                <SAHL:SAHLLabel ID="MemoStatusTitle" runat="server" Font-Bold="true">Memo Status</SAHL:SAHLLabel>
                            </td>
                            <td  class="cellDisplay" style="width: 170px;">
                                <SAHL:SAHLLabel ID="lblMemoStatus" runat="server">-</SAHL:SAHLLabel>
                                <SAHL:SAHLDropDownList ID="MemoStatusUpdate" runat="server" PleaseSelectItem="false"
                                    Width="100%">
                                </SAHL:SAHLDropDownList>
                            </td>
                            <td style="width: 3px;">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <SAHL:SAHLLabel ID="ReminderDateTitle" runat="server" Font-Bold="true">Reminder Date</SAHL:SAHLLabel>
                            </td>
                            <td  class="cellDisplay">
                                <SAHL:SAHLLabel ID="ReminderDate" runat="server">-</SAHL:SAHLLabel>
                                <SAHL:SAHLDateBox ID="ReminderDateUpdate" runat="server" />
                            </td>
                            
                            <td class="TitleText" style="width: 121px">
                                <SAHL:SAHLLabel ID="tdDateChanged" runat="server" CssClass="LabelText" Font-Bold="True" Visible="False">Date Changed</SAHL:SAHLLabel></td>
                            <td  class="cellDisplay">
                                <SAHL:SAHLLabel ID="lblDateChanged" runat="server" CssClass="LabelText" Visible="False">-</SAHL:SAHLLabel>
                            </td>
                            <td style="width: 3px">
                            </td>
                        </tr>
                        <tr runat="server" visible="false" id="trHours">
                            <td>
                                 <SAHL:SAHLLabel ID="lblHours" runat="server" CssClass="TitleText" Font-Bold="True" >Time</SAHL:SAHLLabel>
                            </td>

                            <td>
                                  <SAHL:SAHLDropDownList ID="ddlHour" runat="server" Width="40px" PleaseSelectItem="False">
                                    <asp:ListItem>00</asp:ListItem>
                                    <asp:ListItem>01</asp:ListItem>
                                    <asp:ListItem>02</asp:ListItem>
                                    <asp:ListItem>03</asp:ListItem>
                                    <asp:ListItem>04</asp:ListItem>
                                    <asp:ListItem>05</asp:ListItem>
                                    <asp:ListItem>06</asp:ListItem>
                                    <asp:ListItem>07</asp:ListItem>
                                    <asp:ListItem>08</asp:ListItem>
                                    <asp:ListItem Selected="True">09</asp:ListItem>
                                    <asp:ListItem>10</asp:ListItem>
                                    <asp:ListItem>11</asp:ListItem>
                                    <asp:ListItem>12</asp:ListItem>
                                    <asp:ListItem>13</asp:ListItem>
                                    <asp:ListItem>14</asp:ListItem>
                                    <asp:ListItem>15</asp:ListItem>
                                    <asp:ListItem>16</asp:ListItem>
                                    <asp:ListItem>17</asp:ListItem>
                                    <asp:ListItem>18</asp:ListItem>
                                    <asp:ListItem>19</asp:ListItem>
                                    <asp:ListItem>20</asp:ListItem>
                                    <asp:ListItem>21</asp:ListItem>
                                    <asp:ListItem>22</asp:ListItem>
                                    <asp:ListItem>23</asp:ListItem>
                                </SAHL:SAHLDropDownList>:<SAHL:SAHLDropDownList ID="ddlMin" runat="server" Width="40px"
                                    PleaseSelectItem="False">
                                    <asp:ListItem Selected="True">00</asp:ListItem>
                                    <asp:ListItem>05</asp:ListItem>
                                    <asp:ListItem>10</asp:ListItem>
                                    <asp:ListItem>15</asp:ListItem>
                                    <asp:ListItem>20</asp:ListItem>
                                    <asp:ListItem>25</asp:ListItem>
                                    <asp:ListItem>30</asp:ListItem>
                                    <asp:ListItem>35</asp:ListItem>
                                    <asp:ListItem>40</asp:ListItem>
                                    <asp:ListItem>45</asp:ListItem>
                                    <asp:ListItem>50</asp:ListItem>
                                    <asp:ListItem>55</asp:ListItem>
                                </SAHL:SAHLDropDownList>
                            </td>
                        </tr>    
                        <tr>
                            <td valign="top">
                                <SAHL:SAHLLabel ID="MemoPanelTitle" runat="server" Font-Bold="true">Memo</SAHL:SAHLLabel>
                            </td>
                            <td colspan="4">
                                <asp:Panel ID="MemoPanel" runat="server" Style="width: 100%; height: 60px; border: 1px solid"
                                    ScrollBars="Vertical" BorderColor="gray">
                                    <SAHL:SAHLLabel ID="lblMemo" runat="server" Style="width: 100%; height: 100%;" CssClass="mandatory"></SAHL:SAHLLabel>
                                </asp:Panel>
                                <SAHL:SAHLTextBox ID="MemoUpdate" Style="width: 100%; height: 60px;" runat="server"
                                    TextMode="MultiLine" Width="100%" EnableViewState="False" CssClass="mandatory"
                                    Font-Names="Verdana" Font-Size="8pt"></SAHL:SAHLTextBox>
                                <br />
                                <SAHL:SAHLLabel ID="lblMessageField" runat="server"></SAHL:SAHLLabel>
                            </td>
                            <td valign="top" style="width: 3px">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="ButtonRow" runat="server">
                <td align="right" style="width: 100%">
                    <SAHL:SAHLButton ID="SubmitButton" runat="server" Text="Submit" AccessKey="S" OnClick="SubmitButton_Click" />
                    <SAHL:SAHLButton ID="CancelButton" runat="server" Text="Cancel" AccessKey="C" OnClick="CancelButton_Click"
                        CausesValidation="False" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
