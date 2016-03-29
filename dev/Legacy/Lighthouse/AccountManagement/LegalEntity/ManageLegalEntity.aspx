<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageLegalEntity.aspx.cs" Inherits="MultiViewStart" %>
<%@ Register TagPrefix="uc6" TagName="ExistingLegalEntity" Src="../Common/UserControls/LegalEntityDetail.ascx" %>
<%@ Register TagPrefix="uc5" TagName="LegalEntitySearch" Src="../Common/UserControls/LegalEntitySearch.ascx" %>
<%@ Register TagPrefix="uc4" TagName="ddRoleTypes" Src="../Common/UserControls/RoleTypes.ascx" %>
<%@ Register TagPrefix="uc3" TagName="LegalEntityObj" Src="../Common/UserControls/LegalEntityObj.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Account" Src="../Common/UserControls/AccountDetails.ascx" %>
<%@ Register TagPrefix="uc2" TagName="LegalEntityList" Src="../Common/UserControls/LegalEntityList.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="../style.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
           <asp:MultiView ID="MultiView1" runat="server" OnActiveViewChanged="MultiView1_ActiveViewChanged" >
            <asp:View ID="vwAcct" Runat="server">
                <table>
                    <tr>
                        <td class="ManagementPanel" >
                            <uc1:Account ID="ucLEAccount" Runat="server" EnableViewState="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="ManagementPanel">
                            <uc2:LegalEntityList ID="ucLEList" Runat="server" EnableViewState="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="ManagementPanel">
                            <table width=100%>
                                <tr>
                                    <td >
                                    <asp:Button ID="cmdAdd"  Runat="server" Text="Add Legal Entity" CssClass="CommandButton" ToolTip="Link a legal entity to this account." OnClick="cmdAdd_Click" />
                                    <asp:Button ID="cmdEdit"  Runat="server" Text="Edit Legal Entity" CssClass="CommandButton" ToolTip="Edit a legal entity playing a role in this account." OnClick="cmdEdit_Click" Enabled="False" />
                                    <asp:Button ID="cmdRemove"  Runat="server" Text="Remove Legal Entity" CssClass="CommandButton" ToolTip="Remove a legal entity from this account." OnClick="cmdRemove_Click" Enabled="False" />
                                    </td>
                                    <td>
                            <asp:Button ID="cmdAcct_Back"  Runat="server" Text="Back" CssClass="CommandButton" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                &nbsp;</asp:View>
            <asp:View ID="vwLESearchExisting" Runat="server">
                <table>
                    <tr>
                        <td class="ManagementPanel">
                            &nbsp;
                            <uc5:LegalEntitySearch ID="ucLEListSearch" Runat="server" EnableViewState="true" />
                        </td>
                    </tr>
                    <tr align=left>
                        <td class="ManagementPanel">
                            <uc4:ddRoleTypes ID="ucRoleTypes" Runat="server" EnableViewState="true" Visible="false" />
                            <uc6:ExistingLegalEntity ID="ucLEtoAdd" Runat="server" EnableViewState="true" Visible="false" />
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vwRemove" Runat="server">
                <table>
                    <tr>
                        <td class="Head">
                            <table>
                                <tr>
                                    <td>
                                        Remove Legal Entity from Account Number:</td>
                                    <td >
                                        <asp:Label ID="lblAccountKey" Runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr align=left>
                        <td class="ManagementPanel">
                            <uc6:ExistingLegalEntity ID="ucLEtoDelete" Runat="server" EnableViewState="true" />
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vwEdit" Runat="server">
                &nbsp;
                <table>
                    <tr>
                        <td class="Head">
                            <table>
                                <tr>
                                    <td>
                                        Edit Legal Entity</td>
                                </tr>
                            </table>
                        
                        </td>
                    </tr>
                    <tr>
                        <td class="ManagementPanel">
                            &nbsp;
                            <uc3:LegalEntityObj ID="ucLEtoEdit" Runat="server" EnableViewState="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="ManagementPanel">
                            <table width=100%>
                                <tr>
                                    <td align=left><asp:Button ID="cmdConfirm_Edit"  Runat="server" Text="Confirm" CssClass="CommandButton" />
                                    </td>
                                    <td align=right>
                                        <asp:Button ID="cmdBack_edit"  Runat="server" Text="Cancel" CssClass="CommandButton" OnClick="cmdBack_edit_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:View>&nbsp;</asp:MultiView></div>
    
    </div>
    </form>
</body>
</html>
