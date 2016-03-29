<%@ Page Language="C#" CodeFile="ModifyInterimAuditRecord.aspx.cs" Inherits="ModifyInterimAuditRecord_aspx" %>
<%@ Register TagPrefix="uc1" TagName="title" Src="../common/title.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Interim Audit Record</title>
    
   
</head>
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0">
<uc1:title ID="Title1" Runat="server" />
        <br />
    <form id="form1" runat="server">
    <div>
        
        <table cellpadding="5">
            <tr>
                <td nowrap="nowrap" align="right">
                    Loan Number:</td>
                <td>
                    <asp:Label ID="lblLoanNumber" Runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td nowrap="nowrap" align="right">
                    Sequence Number:</td>
                <td>
                    <asp:Label ID="lblSeqNum" Runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td nowrap="nowrap" align="right">
                    Client Names:</td>
                <td>
                    <asp:Label ID="lblClientNames" Runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="height: 24px" nowrap="nowrap" align="right">
                    Account Type:</td>
                <td style="height: 24px">
                    <asp:DropDownList ID="ddlAccountType" Runat="server" DataMember="ACBType" DataValueField="ACBTypeNumber" DataTextField="ACBTypeDescription">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td nowrap="nowrap" align="right">
                    Bank:</td>
                <td>
                    <asp:DropDownList ID="ddACBBank" Runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddACBBank_SelectedIndexChanged" DataMember="ACBBank" DataValueField="ACBBankCode" DataTextField="ACBBankDescription">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td nowrap="nowrap" align="right">
                    Branch:</td>
                <td>
                    <asp:DropDownList ID="ddACBBranch" Runat="server" DataMember="ACBBranch" DataValueField="ACBBranchCode" DataTextField="ACBBranchDescription">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td nowrap="nowrap" align="right">
                    Account Number:</td>
                <td>
                    <asp:TextBox ID="tbAccountNumber" Runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td nowrap="nowrap" align="right">
                    Action To Take:</td>
                <td>
                    &nbsp;&nbsp;
                    <asp:RadioButtonList ID="rblAction" Runat="server">
                        <asp:ListItem Value="U" Selected="True">Update Record</asp:ListItem>
                        <asp:ListItem Value="S">Suspend Debit Order</asp:ListItem>
                        <asp:ListItem Value="I">Incorrect Bank Details</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td nowrap="nowrap" align="right">
                </td>
                <td>
                    &nbsp;
                    <asp:Label ID="Label1" Runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    &nbsp;&nbsp;
                    <asp:Button ID="btnSubmit" Runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                </td>
            </tr>
        </table>
    
    </div>
    </form>
    &nbsp;
</body>
</html>
