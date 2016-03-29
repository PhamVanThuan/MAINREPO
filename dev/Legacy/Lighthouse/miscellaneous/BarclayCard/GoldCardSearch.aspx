<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GoldCardSearch.aspx.cs" Inherits="BarclayCard_GoldCardSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>SAHL Gold Card - Loan Search</title>
    <link href="../style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label1" runat="server" Text="Label" Visible="False"></asp:Label>
            <table>
                <tr>
                    <td style="width: 100px">
                        <asp:Label ID="lblLoanNumber" runat="server" Text="Loan Number"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtLoanNumber" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px">
                        <asp:Label ID="lblSurname" runat="server" Text="Surname"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSurname" runat="server" Width="400px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px">
                        <asp:Label ID="lblFirstName" runat="server" Text="First Names"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFirstNames" runat="server" Width="400px"></asp:TextBox>
                    </td>
                </tr>
                <tr align="right">
                    <td style="height: 26px; text-align: center;" width="100px">
                    </td>
                    <td style="height: 26px; text-align: left;">
                        <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search" />
                        <input id="btnReset" class="BtnNormal3" onclick="ResetScreen()" title="Reset" type="button"
                            value="Reset" />
                        <br />
                        <asp:CustomValidator ID="ValidateSearchCtrl" runat="server" ErrorMessage="No Search Criteria selected"
                            OnServerValidate="ValidateSearch"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="gvSearch" runat="server" AutoGenerateColumns="False" CellPadding="4"
                            BorderWidth="1px" ForeColor="Black" BackColor="White" GridLines="Vertical" BorderColor="#DEDFDE"
                            BorderStyle="None" RowHeaderColumn="clientname" EmptyDataText="The search returned no Loans"
                            Width="100%" Caption="Loan Search Results" CaptionAlign="Top" AllowPaging="True"
                            DataKeyNames="LoanNumber" OnDataBound="gvSearch_DataBound" AutoGenerateSelectButton="True" >
                            <Columns>
                                <asp:BoundField DataField="LoanNumber" HeaderText="Loan Number" SortExpression="LoanNumber" />
                                <asp:BoundField DataField="AccountLegalName" HeaderText="Name" />
                                <asp:BoundField DataField="SPVDescription" HeaderText="SPV" SortExpression="SPVDescription" />
                                <asp:BoundField DataField="LoanRate" HeaderText="Rate" SortExpression="LoanRate"
                                    DataFormatString="{0:F4}%" />
                                <asp:BoundField DataField="LoanCurrentBalance" HeaderText="Current Balance" SortExpression="LoanCurrentBalance"
                                    DataFormatString="R {0:C2}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="LoanArrearBalance" HeaderText="Arrear Balance" SortExpression="LoanArrearBalance"
                                    DataFormatString="R {0:C2}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="LoanInstallmentAmount" HeaderText="Installment" SortExpression="LoanInstallmentAmount"
                                    DataFormatString="R {0:C2}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ClientNumber" HeaderText="Client Number" SortExpression="ClientNumber" />
                            </Columns>
                            <FooterStyle BackColor="#CCCC99" />
                            <RowStyle BackColor="#F7F7DE" />
                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Left" />
                            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle HorizontalAlign="Left" BackColor="BurlyWood" Font-Bold="True" ForeColor="White" BorderStyle="None" />
                            <AlternatingRowStyle BackColor="White" />
                            <PagerTemplate>
                                <table width="100%">
                                    <tr>
                                        <td width="70%">
                                            <asp:Label ID="MessageLabel" ForeColor="Blue" Text="Select a page:" runat="server" />
                                            <asp:DropDownList ID="PageDropDownList" AutoPostBack="true" OnSelectedIndexChanged="PageDropDownList_SelectedIndexChanged"
                                                runat="server" />
                                        </td>
                                        <td width="70%" align="right">
                                            <asp:Label ID="CurrentPageLabel" ForeColor="Blue" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </PagerTemplate>
                        </asp:GridView>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center">
                        <asp:Button ID="btnGoldCardApp" runat="server" OnClick="btnGoldCardApp_Click" Text="SAHL Gold Card" />
                        <br />
                        <asp:CustomValidator ID="ValidateApplicationCtrl" runat="server" ErrorMessage="Gold Card cannot be offered."></asp:CustomValidator>
                    </td>
                </tr>
            </table>
        </div>
        <asp:SqlDataSource ID="osqlds_GetLoanFindDataByNumber" runat="server" SelectCommand="c_GetLoanFindDataByNumber"
            SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtLoanNumber" Name="Nbr" PropertyName="Text" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="osqlds_GetLoanFindDataBySurname" runat="server" SelectCommand="c_GetLoanFindDataBySurname"
            SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtSurname" Name="Surname" PropertyName="Text" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="osqlds_GetLoanFindDataByFirstName" runat="server" SelectCommand="c_GetLoanFindDataByFirstName"
            SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtFirstNames" Name="FirstName" PropertyName="Text" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="osqlds_GetLoanFindDataByNames" runat="server" SelectCommand="c_GetLoanFindDataByNames"
            SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtFirstNames" Name="FirstName" PropertyName="Text" />
                <asp:ControlParameter ControlID="txtSurname" Name="Surname" PropertyName="Text" />
            </SelectParameters>
        </asp:SqlDataSource>
    </form>
</body>
</html>
