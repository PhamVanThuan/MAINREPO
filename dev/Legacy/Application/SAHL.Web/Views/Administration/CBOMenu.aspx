<%@ Page Language="C#" AutoEventWireup="true" Codebehind="CBOMenu.aspx.cs" Inherits="SAHL.Web.Views.Administration.CBOMenu"
    MasterPageFile="~/MasterPages/Blank.master" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
<table runat="server" width="100%" id="tblCBO" visible="false">
    <tr>
        <td align="center" style="width: 50%">
            <SAHL:SAHLLabel ID="SAHLLabel6" runat="server" CssClass="LabelText" TextAlign="0">TreeView</SAHL:SAHLLabel></td>
        <td align="center">
            <SAHL:SAHLLabel ID="SAHLLabel3" runat="server">Statement Name</SAHL:SAHLLabel></td>
    </tr>
<tr>
<td style="width: 50%">
<asp:Panel runat="server" ScrollBars="Vertical" ID="pnl" Width="99%" Height="300px" style="overflow-y:scroll">
                <SAHL:SAHLTreeView runat="server" ID="tv" OnNodeSelected="tv_NodeSelected" />
                </asp:Panel>
</td>
    <td valign="top">
        <SAHL:SAHLDropDownList ID="ddlUIStatement" runat="server" Width="100%">
        </SAHL:SAHLDropDownList></td>
</tr>
</table>
    <table id="tblMaint" runat="server" width="100%" visible="false">
        <tr>
            <td style="width: 120px">
            </td>
            <td style="width: 220px">
            </td>
            <td>
            </td>
            <td style="width: 90px">
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td style="width: 120px">
                <SAHL:SAHLLabel ID="SAHLLabel1" runat="server">Node Type</SAHL:SAHLLabel>&nbsp;
            </td>
            <td style="width: 220px">
                &nbsp;<SAHL:SAHLDropDownList PleaseSelectItem="false" runat="server" ID="ddlNodeType" Width="120px">
                    <asp:ListItem Value="D">Dynamic</asp:ListItem>
                    <asp:ListItem Value="S">Static</asp:ListItem>
                </SAHL:SAHLDropDownList></td>
            <td style="width: 10px">
            </td>
            <td style="width: 90px">
                <SAHL:SAHLLabel ID="lbl1" runat="server">Description</SAHL:SAHLLabel></td>
            <td>
                <SAHL:SAHLTextBox ID="txtDesc" runat="server" Width="330px"></SAHL:SAHLTextBox>
                <SAHL:SAHLRequiredFieldValidator ID="rfvDesc" ControlToValidate="txtDesc" ErrorMessage="*"
                    runat="server" />
            </td>
        </tr>
        <tr>
            <td style="width: 120px">
                </td>
            <td style="width: 220px">
                &nbsp;</td>
            <td>
            </td>
            <td style="width: 90px">
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td style="width: 120px">
                <SAHL:SAHLLabel ID="SAHLLabel7" runat="server">GenericTypeKey</SAHL:SAHLLabel></td>
            <td style="width: 220px">
                &nbsp;<SAHL:SAHLDropDownList PleaseSelectItem="false" runat="server" ID="ddlGenKeyTYpe" Width="120px">
                </SAHL:SAHLDropDownList></td>
            <td>
            </td>
            <td style="width: 90px">
                <SAHL:SAHLLabel ID="SAHLLabel2" runat="server">URL</SAHL:SAHLLabel></td>
            <td>
                <SAHL:SAHLTextBox ID="txtURL" runat="server" Width="330px"></SAHL:SAHLTextBox>
                <SAHL:SAHLRequiredFieldValidator runat="server" ID="rfvURL" ControlToValidate="txtURL"
                    ErrorMessage="*" />
            </td>
        </tr>
        
        <tr>
            <td style="width: 120px">
                </td>
            <td style="width: 220px">
                &nbsp;
            </td>
            <td>
            </td>
            <td style="width: 90px">
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td style="width: 120px">
                <SAHL:SAHLLabel ID="SAHLLabel8" runat="server">HasOriginationSource</SAHL:SAHLLabel></td>
            <td style="width: 220px">
                &nbsp;<SAHL:SAHLDropDownList PleaseSelectItem="false" runat="server" ID="ddlHasOriginationSource" Width="120px">
                    <asp:ListItem Value="1">true</asp:ListItem>
                    <asp:ListItem Value="0">false</asp:ListItem>
                </SAHL:SAHLDropDownList></td>
            <td>
            </td>
            <td style="width: 90px">
                <SAHL:SAHLLabel ID="SAHLLabel4" runat="server">Sequence</SAHL:SAHLLabel></td>
            <td>
            <SAHL:SAHLTextBox ID="txtSequence" runat="server" Width="330px" DisplayInputType="Number"></SAHL:SAHLTextBox>
            <SAHL:SAHLRequiredFieldValidator runat="server" ID="rfvSequence" ControlToValidate="txtSequence"
                ErrorMessage="*" />
            </td>
        </tr>
        <tr>
            <td style="width: 120px">
                </td>
            <td style="width: 220px">
                &nbsp;</td>
            <td>
            </td>
            <td style="width: 90px">
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td style="width: 120px">
                <SAHL:SAHLLabel ID="SAHLLabel9" runat="server">IsRemovable</SAHL:SAHLLabel></td>
            <td style="width: 220px">
                &nbsp;<SAHL:SAHLDropDownList PleaseSelectItem="false" runat="server" ID="ddlIsRemovable" Width="120px">
                    <asp:ListItem Value="1">true</asp:ListItem>
                    <asp:ListItem Value="0">False</asp:ListItem>
                </SAHL:SAHLDropDownList></td>
            <td>
            </td>
            <td style="width: 90px">
                <SAHL:SAHLLabel ID="SAHLLabel5" runat="server">MenuIcon</SAHL:SAHLLabel></td>
            <td>
                <SAHL:SAHLTextBox ID="txtMenuIcon" runat="server" Width="330px"></SAHL:SAHLTextBox>
                <SAHL:SAHLRequiredFieldValidator runat="server" ID="rfvMenu" ControlToValidate="txtMenuIcon"
                    ErrorMessage="*" />
            </td>
        </tr>
        <tr>
            <td style="width: 120px">
                </td>
            <td style="width: 220px">
                &nbsp;</td>
            <td>
            </td>
            <td style="width: 90px">
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td style="width: 120px">
                <SAHL:SAHLLabel ID="SAHLLabel11" runat="server">IncludeParentHeaderIcons</SAHL:SAHLLabel></td>
            <td style="width: 220px">
                &nbsp;<SAHL:SAHLDropDownList PleaseSelectItem="false" runat="server" ID="ddlIncludeParent" Width="120px">
                    <asp:ListItem Value="1">true</asp:ListItem>
                    <asp:ListItem Value="0">False</asp:ListItem>
                </SAHL:SAHLDropDownList></td>
            <td>
            </td>
            <td style="width: 90px">
                <SAHL:SAHLLabel ID="SAHLLabel10" runat="server">ExpandLevel</SAHL:SAHLLabel></td>
            <td>
                <SAHL:SAHLTextBox ID="txtExpandLevel" runat="server" Width="330px" DisplayInputType="Number"></SAHL:SAHLTextBox>
                <SAHL:SAHLRequiredFieldValidator runat="server" ID="rfvExpand" ControlToValidate="txtExpandLevel"
                    ErrorMessage="*" />
            </td>
        </tr>
        <tr>
            <td style="width: 120px">
                </td>
            <td style="width: 220px">
                &nbsp;</td>
            <td>
            </td>
            <td style="width: 90px">
            </td>
            <td>
            </td>
        </tr>
        <tr>
        <td colspan="5" align="right"><SAHL:SAHLButton runat="server" ID="btnNext" Text="Next" OnClick="btnNext_Click" /></td>
        </tr>
    </table>
                <input type="hidden" runat="server" id="hdKey" />
</asp:Content>
