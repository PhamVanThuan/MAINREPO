<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CBOSummary.aspx.cs" Inherits="SAHL.Web.Views.Administration.CBOSummary" 
    MasterPageFile="~/MasterPages/Blank.master" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
<table id="tblMaint" runat="server" width="790">
<tr>
<td colspan="2"><SAHL:SAHLLabel runat="server" ID="lblHeadnig">CBO Menu Node Summary.</SAHL:SAHLLabel></td>
</tr>
        <tr>
            <td width="20%">
                <SAHL:SAHLLabel ID="lbl1" runat="server">Description</SAHL:SAHLLabel>
            </td>
            <td>
                <SAHL:SAHLTextBox ID="txtDesc" runat="server" ReadOnly="True" Width="557px"/>
            </td>
        </tr>
        <tr>
            <td width="20%">
                <SAHL:SAHLLabel ID="SAHLLabel1" runat="server">Node Type</SAHL:SAHLLabel></td>
            <td>
                <SAHL:SAHLTextBox ID="txtNodeType" runat="server" ReadOnly="True" Width="557px"></SAHL:SAHLTextBox></td>
        </tr>
        <tr>
            <td width="20%">
                <SAHL:SAHLLabel ID="SAHLLabel2" runat="server">URL</SAHL:SAHLLabel></td>
            <td>
                <SAHL:SAHLTextBox ID="txtURL" runat="server" ReadOnly="True" Width="557px"></SAHL:SAHLTextBox>
            </td>
        </tr>
        
        <tr>
            <td width="20%">
                <SAHL:SAHLLabel ID="SAHLLabel4" runat="server">Sequence</SAHL:SAHLLabel></td>
            <td>
            <SAHL:SAHLTextBox ID="txtSequence" runat="server" ReadOnly="True" Width="557px"></SAHL:SAHLTextBox>
            </td>
        </tr>
        <tr>
            <td width="20%">
                <SAHL:SAHLLabel ID="SAHLLabel5" runat="server">MenuIcon</SAHL:SAHLLabel></td>
            <td>
                <SAHL:SAHLTextBox ID="txtMenuIcon" runat="server" ReadOnly="True" Width="557px"></SAHL:SAHLTextBox>
            </td>
        </tr>
        <tr>
            <td width="20%">
                <SAHL:SAHLLabel ID="SAHLLabel7" runat="server">GenericTypeKey</SAHL:SAHLLabel></td>
            <td>
                <SAHL:SAHLTextBox ID="txtGenericKeyType" runat="server" ReadOnly="True" Width="557px"></SAHL:SAHLTextBox></td>
        </tr>
        <tr>
            <td width="20%">
                <SAHL:SAHLLabel ID="SAHLLabel8" runat="server">HasOriginationSource</SAHL:SAHLLabel></td>
            <td>
                <SAHL:SAHLTextBox ID="txtHasOrogSource" runat="server" ReadOnly="True" Width="557px"></SAHL:SAHLTextBox></td>
        </tr>
        <tr>
            <td width="20%">
                <SAHL:SAHLLabel ID="SAHLLabel9" runat="server">IsRemovable</SAHL:SAHLLabel></td>
            <td>
                <SAHL:SAHLTextBox ID="txtIsremovable" runat="server" ReadOnly="True" Width="557px"></SAHL:SAHLTextBox></td>
        </tr>
        <tr>
            <td width="20%">
                <SAHL:SAHLLabel ID="SAHLLabel10" runat="server">ExpandLevel</SAHL:SAHLLabel></td>
            <td>
                <SAHL:SAHLTextBox ID="txtExpandLevel" runat="server" ReadOnly="True" Width="557px"></SAHL:SAHLTextBox>
            </td>
        </tr>
        <tr>
            <td width="20%">
                <SAHL:SAHLLabel ID="SAHLLabel11" runat="server">IncludeParentHeaderIcons</SAHL:SAHLLabel></td>
            <td>
                <SAHL:SAHLTextBox ID="txtIncludeParent" runat="server" ReadOnly="True" Width="557px"></SAHL:SAHLTextBox></td>
        </tr>
                <tr>
            <td width="20%">
                <SAHL:SAHLLabel ID="SAHLLabel3" runat="server">CBO Parent</SAHL:SAHLLabel></td>
            <td>
                <SAHL:SAHLTextBox ID="txtCBOParent" runat="server" ReadOnly="True" Width="557px"></SAHL:SAHLTextBox></td>
        </tr>
                <tr>
            <td width="20%">
                <SAHL:SAHLLabel ID="SAHLLabel6" runat="server">UI Statement</SAHL:SAHLLabel></td>
            <td>
                <SAHL:SAHLTextBox ID="txtUIStatement" runat="server" ReadOnly="True" Width="557px"></SAHL:SAHLTextBox></td>
        </tr>
                <tr>
            <td width="20%">
                <SAHL:SAHLLabel ID="SAHLLabel12" runat="server">Feature</SAHL:SAHLLabel></td>
            <td>
                <SAHL:SAHLTextBox ID="txtFeature" runat="server" ReadOnly="True" Width="557px"></SAHL:SAHLTextBox></td>
        </tr>
        <tr>
        <td colspan="2"><SAHL:SAHLButton runat="server" ID="btnFinish" Text="Finish" OnClick="btnNext_Click" /></td>
        </tr>
    </table>
</asp:Content>