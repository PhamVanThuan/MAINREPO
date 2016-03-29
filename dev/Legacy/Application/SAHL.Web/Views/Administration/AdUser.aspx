<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdUser.aspx.cs" Inherits="SAHL.Web.Views.Administration.AdUser"
    MasterPageFile="~/MasterPages/Blank.master" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table id="tblSearch" runat="server" class="tableStandard" width="100%">
        <tr>
            <td style="width: 200px">
                <SAHL:SAHLLabel ID="SAHLLabel1" runat="server" CssClass="LabelText" Text="Active Directory User Name"></SAHL:SAHLLabel>
            </td>
            <td>
                <SAHL:SAHLTextBox ID="txtActiveDirectorySearch" runat="server" MaxLength="20" CssClass="mandatory">
                </SAHL:SAHLTextBox>
                <SAHL:SAHLAutoComplete AutoPostBack="true" runat="server" ID="acActiveDirectorySearch"
                    TargetControlID="txtActiveDirectorySearch" ServiceMethod="SAHL.Web.AJAX.ActiveDirectory.GetActiveDirectoryUsersByName"
                    OnItemSelected="acActiveDirectorySearch_ItemSelected" MinCharacters="1">
                </SAHL:SAHLAutoComplete>
            </td>
        </tr>
    </table>
    <br />
    <br />
    <table runat="server" id="tblMaint" class="tableStandard" width="100%" visible="false">
        <tr>
            <td width="100px">
            </td>
            <td width="100px">
                User Name
            </td>
            <td align="left" valign="top">
                <SAHL:SAHLTextBox ID="txtAdUserName" runat="server" Width="149px" 
                    ReadOnly="True"></SAHL:SAHLTextBox>
            </td>
        </tr>
        <tr>
            <td width="100px">
            </td>
            <td width="100px">
                First Name
            </td>
            <td align="left">
                <SAHL:SAHLTextBox ID="txtFirstName" runat="server" Width="150px"></SAHL:SAHLTextBox>
            </td>
        </tr>
        <tr>
            <td width="100px">
            </td>
            <td width="100px">
                Surname
            </td>
            <td align="left">
                <SAHL:SAHLTextBox ID="txtSurname" runat="server" Width="210px"></SAHL:SAHLTextBox>
            </td>
        </tr>
        <tr>
            <td width="100px">
            </td>
            <td width="100px">
                Cellphone
            </td>
            <td align="left">
                <SAHL:SAHLTextBox ID="txtCellNum" runat="server" Width="320px"></SAHL:SAHLTextBox>
            </td>
        </tr>
        <tr>
            <td width="100px">
            </td>
            <td width="100px">
                EMail
            </td>
            <td align="left">
                <SAHL:SAHLTextBox ID="txtEMail" runat="server" Width="363px"></SAHL:SAHLTextBox>
            </td>
        </tr>
                <tr>
            <td width="100px">
            </td>
            <td width="100px">
                Status
            </td>
            <td align="left">
                <SAHL:SAHLDropDownList ID="ddlStatus" runat="server" PleaseSelectItem="False"></SAHL:SAHLDropDownList>
            </td>
        </tr>
        <tr runat="server" id="trSubmit" visible="true">
            <td align="left">
            </td>
            <td colspan="2">
                <br />
                <br />
                <SAHL:SAHLButton runat="server" ID="btnSubmit" Text="Submit" OnClick="Submit_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
