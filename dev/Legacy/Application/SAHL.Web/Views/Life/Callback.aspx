<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" Inherits="SAHL.Web.Views.Life.Callback"
    Title="Callback" Codebehind="Callback.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Src="WorkFlowHeader.ascx" TagName="WorkFlowHeader" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <uc1:WorkFlowHeader ID="WorkFlowHeader1" runat="server" />
    <div style="text-align: center">
        <SAHL:SAHLLabel ID="SAHLLabel2" runat="server" Font-Bold="True" Font-Names="Arial"
            Font-Size="Medium" Font-Underline="True" Text="CREATE CALLBACK" CssClass="LabelText"></SAHL:SAHLLabel>
        <br />
        <br />
        <table width="100%" class="tableStandard">
            <tr>
                <td align="left">
                    <SAHL:SAHLGridView ID="gridCallBack" runat="server" AutoGenerateColumns="false" EmptyDataSetMessage="No callbacks have been set on this policy."
                        EnableViewState="false" FixedHeader="false" GridHeight="150px" GridWidth="100%"
                        HeaderCaption="Callback History" NullDataSetMessage="No callbacks have been set"
                        Width="100%" OnRowDataBound="gridCallBack_RowDataBound">
                        <HeaderStyle CssClass="TableHeaderB" />
                        <RowStyle CssClass="TableRowA" />
                    </SAHL:SAHLGridView>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <SAHL:SAHLLabel ID="SAHLLabel1" runat="server" CssClass="LabelText" Width="94px"></SAHL:SAHLLabel></td>
            </tr>
            <tr>
                <td align="center">
                    <table border="0">
                        <tr>
                            <td>
                                <SAHL:SAHLLabel ID="Label3" runat="server" Text="Call Back Reason" Width="111px"></SAHL:SAHLLabel></td>
                            <td align="left" colspan="4">
                                <SAHL:SAHLDropDownList ID="ddlReason" runat="server" Width="199px" CausesValidation="True">
                                </SAHL:SAHLDropDownList>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="middle">
                            </td>
                            <td align="left">
                            </td>
                            <td align="left" style="width: 45px">
                            </td>
                            <td align="left" style="width: 45px">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="top">
                                <SAHL:SAHLLabel ID="Label2" runat="server" Text="Call Back Date" Width="94px"></SAHL:SAHLLabel></td>
                            <td align="left" valign="top">
                                <SAHL:SAHLDateBox ID="dteCallbackDate" runat="server" EnableTheming="False" EnableViewState="False"/>
                                </td>
                            <td align="left" valign="top" style="width: 45px">
                            </td>
                            <td align="left" valign="top" style="width: 45px">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="middle">
                                Time</td>
                            <td align="left" colspan="1">
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
                                </SAHL:SAHLDropDownList></td>
                            <td align="left" colspan="1">
                            </td>
                            <td align="left" colspan="1">
                            </td>
                            <td align="left" colspan="1">
                            </td>
                        </tr>
                        <tr>
                            <td align="right" valign="middle">
                            </td>
                            <td align="left" colspan="4">
                            </td>
                        </tr>
                        <tr>
                            <td  valign="top" align="right">
                                Notes</td>
                            <td  valign="top" colspan="4" align="left">
                                <SAHL:SAHLTextBox ID="txtNotes" runat="server" Height="84px" TextMode="MultiLine"
                                    Width="500px"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr>
                            <td colspan="3">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <SAHL:SAHLButton ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" SecurityTag="LifeUpdateAccessWorkflow"/>
                    <SAHL:SAHLButton ID="btnCancel" runat="server" CausesValidation="False" OnClick="btnCancel_Click" Text="Cancel" />
                 </td>
            </tr>
        </table>
        <br />
    </div>
</asp:Content>
