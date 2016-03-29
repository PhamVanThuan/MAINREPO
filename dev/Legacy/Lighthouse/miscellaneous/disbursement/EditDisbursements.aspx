<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditDisbursements.aspx.cs" Inherits="disbursement_EditDisbursements" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Interim Audit Record</title>
    <link href="../style.css" rel="stylesheet" type="text/css" />
    
   
</head>
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0">
    &nbsp;<asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Arial Narrow"
        Font-Size="14pt" Height="27px" Text="Disbursement" Width="274px"></asp:Label><br />
    <form id="form1" runat="server">
    <div>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="GridData" BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="1px" CellPadding="2" ForeColor="Black" GridLines="None" OnRowCommand="GridView1_RowCommand" Width="100%">
            <Columns>
                <asp:CommandField ShowSelectButton="True" />
                <asp:BoundField DataField="Loannumber" HeaderText="Loan Number" />
                <asp:BoundField DataField="SequenceNumber" HeaderText="Sequence Number" />
                <asp:BoundField DataField="Branchcode" HeaderText="Branch Code" />
                <asp:BoundField DataField="accountnumber" HeaderText="Account Number" />
                <asp:BoundField DataField="ClientName" HeaderText="Client Name" />
                <asp:BoundField DataField="ErrorDescription" HeaderText="Error" />
                <asp:BoundField DataField="ErrorCode" HeaderText="Code" />
            </Columns>
            <FooterStyle BackColor="Tan" />
            <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
            <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
            <HeaderStyle BackColor="Tan" Font-Bold="True" />
            <AlternatingRowStyle BackColor="PaleGoldenrod" />
        </asp:GridView>
        <asp:ObjectDataSource ID="GridData" runat="server" SelectMethod="GetDisbursementData"
            TypeName="Disbursement_BO"></asp:ObjectDataSource>
        <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" BackColor="LightGoldenrodYellow"
            BorderColor="Tan" BorderWidth="1px" CellPadding="2" DataSourceID="GridData" ForeColor="Black"
            GridLines="None" Height="50px" Width="552px">
            <FooterStyle BackColor="Tan" />
            <EditRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
            <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
            <Fields>
                <asp:BoundField DataField="Loannumber" HeaderText="Loan Number" />
                <asp:TemplateField HeaderText="Account Type">
                    <EditItemTemplate>
                        &nbsp;<asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="DB_AccountType"
                            DataTextField="ACBAccountType" DataValueField="ACBAccountType">
                        </asp:DropDownList><asp:ObjectDataSource ID="DB_AccountType" runat="server" SelectMethod="GetAccountTypes"
                            TypeName="Disbursement_BO"></asp:ObjectDataSource>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Bank">
                    <EditItemTemplate>
                        <asp:DropDownList ID="DropDownList2" runat="server" DataSourceID="DB_Bank" DataTextField="ACBBankCode"
                            DataValueField="ACBBankCode">
                        </asp:DropDownList><asp:ObjectDataSource ID="DB_Bank" runat="server" SelectMethod="GetBankTypes"
                            TypeName="Disbursement_BO"></asp:ObjectDataSource>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Branch">
                    <EditItemTemplate>
                        <asp:DropDownList ID="DropDownList3" runat="server" DataSourceID="DB_BranchCode"
                            DataTextField="ACBBranchCode" DataValueField="ACBBranchCode">
                        </asp:DropDownList><asp:ObjectDataSource ID="DB_BranchCode" runat="server" SelectMethod="GetBranchCodes"
                            TypeName="Disbursement_BO"></asp:ObjectDataSource>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="accountnumber" HeaderText="Account Number" />
                <asp:ButtonField ButtonType="Button" Text="Submit" />
            </Fields>
            <HeaderStyle BackColor="Tan" Font-Bold="True" />
            <AlternatingRowStyle BackColor="PaleGoldenrod" />
        </asp:DetailsView>
        <br />
        
        <table cellpadding="5" id="TABLE1">
            <tr>
                <td nowrap="nowrap" align="right">
                    Loan Number:</td>
                <td>
                    <asp:Label ID="lblLoanNumber" Runat="server"></asp:Label>
                </td>
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
                <td nowrap="nowrap" align="right" style="height: 24px">
                </td>
                <td style="height: 24px">
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
