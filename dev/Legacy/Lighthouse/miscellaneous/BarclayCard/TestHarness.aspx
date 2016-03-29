<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestHarness.aspx.cs" Inherits="BarclayCard_TestHarness" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body style="background-color: #cccc99">
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td style="width: 160px">
                        <asp:Label ID="lblLoanNumber" runat="server" Text="Loan / Prospect Number"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="txtLoanNumber" runat="server"></asp:TextBox>
                        <asp:CheckBox ID="chkProspect" runat="server" Text="Prospect" />
                        </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnRefresh" runat="server" OnClick="LoadData" Text="Refresh" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            CellPadding="4" DataSourceID="SqlDataSource1" ForeColor="#333333" GridLines="None"
                            RowHeaderColumn="clientname" AutoGenerateSelectButton="True" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowDataBound="GridView1_RowDataBound" EmptyDataText="No Data Found">
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <Columns>
                                <asp:BoundField DataField="loannumber" HeaderText="loannumber" SortExpression="loannumber" />
                                <asp:BoundField DataField="clientnumber" HeaderText="clientnumber" SortExpression="clientnumber" />
                                <asp:BoundField DataField="clientname" HeaderText="clientname" SortExpression="clientname">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="clientsurname" HeaderText="clientsurname" SortExpression="clientsurname" />
                                <asp:BoundField DataField="purposenumber" HeaderText="purposenumber" />
                            </Columns>
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <EditRowStyle BackColor="#999999" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Left" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnGoldCardApp" runat="server" OnClick="btnGoldCardApp_Click" Text="Gold Card Application" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" >
        </asp:SqlDataSource>
        
    </form>
</body>
</html>
