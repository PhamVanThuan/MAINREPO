<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Life.RPAR" Title="RPAR" Codebehind="RPAR.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Src="WorkFlowHeader.ascx" TagName="WorkFlowHeader" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <uc1:WorkFlowHeader ID="WorkFlowHeader1" runat="server" />
    <div style="text-align: center">
        <SAHL:SAHLLabel ID="Label1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Medium"
            Font-Underline="True" Text="RPAR" CssClass="LabelText"></SAHL:SAHLLabel>
        <br />
        <br />
        <table width="100%"  class="tableStandard">
            <tr>
                <td align="left">
                    <table align="left">
                        <tr>
                            <td valign="middle" width="336px">
                                <asp:Label ID="lblReplace" runat="server" Text="Is this policy to replace another policy with any insurer?"></asp:Label>
                            </td>
                            <td width="336px">
                                <asp:RadioButtonList ID="rblYesNo" runat="server" RepeatDirection="Horizontal" AutoPostBack="True"
                                    OnSelectedIndexChanged="rblYesNo_SelectedIndexChanged" RepeatLayout="Flow">
                                    <asp:ListItem>Yes</asp:ListItem>
                                    <asp:ListItem Selected="True">No</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle">
                                <asp:Label ID="lblAssurer" runat="server" Text="Name of Assurer whose policy is being replaced"
                                    Visible="False"></asp:Label></td>
                            <td>
                                <SAHL:SAHLDropDownList ID="ddlAssurer" runat="server" Visible="False" Width="200px"
                                    AutoPostBack="True" CssClass="RequiredInput" OnSelectedIndexChanged="ddlAssurer_SelectedIndexChanged" PleaseSelectItem="False">
                                </SAHL:SAHLDropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle">
                                <asp:Label ID="lblOther" runat="server" Text="Please enter assurer name here" Visible="False"></asp:Label></td>
                            <td>
                                <SAHL:SAHLTextBox ID="txtOther" runat="server" CssClass="RequiredInput" Visible="False"
                                    Width="200px"></SAHL:SAHLTextBox>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle">
                                <asp:Label ID="lblPolicyNumber" runat="server" Text="Policy number being replaced"
                                    Visible="False"></asp:Label></td>
                            <td>
                                <SAHL:SAHLTextBox ID="txtPolicyNumber" runat="server" Visible="False" Width="200px"
                                    CssClass="RequiredInput"></SAHL:SAHLTextBox>&nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="left">
                <br />
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="lblAware" runat="server" Text="Are you aware that:" Visible="False"></asp:Label></td>
            </tr>
            <tr>
                <td align="center" valign="top">
                    <asp:Table ID="tblText" runat="server" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px"
                        Visible="False">
                    </asp:Table>
                </td>
            </tr>
            <tr>
                <td align="left" style="height: 21px">
                    <asp:Label ID="lblContinue" runat="server" Text="Now that you are aware of these factors, would you like to continue?"
                        Visible="False"></asp:Label></td>
            </tr>
            <tr>
                <td align="center">
                    </td>
            </tr>
            <tr>
                <td align="center">
                    <SAHL:SAHLButton ID="btnNTU" runat="server" Text="NTU" Visible="False" OnClick="btnNTU_Click"
                        SecurityTag="LifeUpdateAccessWorkflow" />
                    <SAHL:SAHLButton ID="btnConsider" runat="server" Text="Considering" Visible="False"
                        OnClick="btnConsider_Click" SecurityTag="LifeUpdateAccessWorkflow" />
                    <SAHL:SAHLButton ID="btnNext" runat="server" Text="Next" OnClick="btnNext_Click"
                        SecurityTag="LifeUpdateAccessWorkflow" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="HiddenField1" runat="server" Value="false" />
    </div>
</asp:Content>
