<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ArrearGrid.aspx.cs" Inherits="ArrearGrid" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body topmargin="0" leftmargin="0" style="width: 850px; height: 350px;" bgcolor="#e0e0e0">
    <form id="form1" runat="server">
    <table>
        <tr>
            <td colspan="3" width="100%" align="center">
                <asp:RadioButtonList ID="optHistory" runat="server" RepeatDirection="Horizontal" Font-Names="Microsoft Sans Serif" Font-Size="10pt" Width="450px" AutoPostBack="True">
                    <asp:ListItem Value="4">Last 4 months</asp:ListItem>
                    <asp:ListItem Value="6">Last 6 months</asp:ListItem>
                    <asp:ListItem Value="0">All transactions</asp:ListItem>
                </asp:RadioButtonList></td>
        </tr>
        <tr>
            <td colspan="3">
                <div id="pnlGrid" style="width: 847px; height: 465px; overflow: auto;">
                            <asp:GridView ID="gvArrearGrid" runat="server" AutoGenerateColumns="False" UseAccessibleHeader="False" Width="830px" BorderStyle="Solid" BorderWidth="1px">
                                <RowStyle BackColor="#FFFFC0" Font-Names="Microsoft Sans Serif" Font-Size="10pt" />
                                <Columns>
                                    <asp:BoundField ApplyFormatInEditMode="True" DataField="Number" HeaderText="Number" Visible="False">
                                        <HeaderStyle Font-Names="Microsoft Sans Serif" Font-Size="10pt" HorizontalAlign="Left"
                                            Width="60px" />
                                        <ItemStyle HorizontalAlign="Left" Width="60px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Type" DataFormatString="{0:s}" HeaderText="Type">
                                        <HeaderStyle HorizontalAlign="Left" Width="230px" />
                                        <ItemStyle HorizontalAlign="Left" Width="210px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Reference" HeaderText="Reference">
                                        <HeaderStyle Width="120px" />
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Changed By" HeaderText="Changed By">
                                        <HeaderStyle Width="80px" />
                                        <ItemStyle Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Insert Date" HeaderText="Insert date">
                                        <HeaderStyle Width="70px" />
                                        <ItemStyle HorizontalAlign="Right" Width="70px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Effective Date" HeaderText="Effective date">
                                        <HeaderStyle Width="90px" />
                                        <ItemStyle HorizontalAlign="Right" Width="90px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Rate" HeaderText="Rate">
                                        <HeaderStyle Width="50px" />
                                        <ItemStyle HorizontalAlign="Right" Width="50px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Amount" HeaderText="Amount">
                                        <HeaderStyle Width="70px" />
                                        <ItemStyle HorizontalAlign="Right" Width="70px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Balance" HeaderText="Balance">
                                        <HeaderStyle Width="70px" />
                                        <ItemStyle HorizontalAlign="Right" Width="70px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ServiceType" HeaderText="Service type">
                                        <HeaderStyle Width="90px" />
                                        <ItemStyle Width="90px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="HTMLColour" HeaderText="Background Colour" />
                                </Columns>
                                <HeaderStyle BackColor="#1414FF" Font-Names="Microsoft Sans Serif" Font-Size="10pt"
                                    ForeColor="White" />
                                <AlternatingRowStyle BackColor="Silver" Font-Names="Microsoft Sans Serif" Font-Size="10pt" />
                            </asp:GridView>
                </div>
            </td>
        </tr>
       <tr>
          <td colspan="3">
             <asp:Label ID="lblErr" runat="server" ForeColor="Red" Text="Error" Visible="False"></asp:Label></td>
       </tr>
    </table>
    </form>
</body>
</html>
