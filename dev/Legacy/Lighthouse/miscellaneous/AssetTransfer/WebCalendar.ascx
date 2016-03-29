<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WebCalendar.ascx.cs" Inherits="WebCalendar" %>

<table style="width:250" cellpadding=0, cellspacing=0>
    <tr>
        <td><asp:TextBox ID="txtDate" runat="server" ReadOnly="True" Width="112px"></asp:TextBox>        
            <asp:Button ID="cmdPick" runat="server" Text="Pick" OnClick="cmdPick_Click" /></td>
    </tr>
    <tr>
        <td colspan=2 style="height:auto">
            <asp:Calendar ID="calDate" runat="server" Width="250px" Height="150px" BackColor="White" BorderColor="Black" BorderStyle="Solid" CellSpacing="1" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" NextPrevFormat="ShortMonth" OnSelectionChanged="calDate_SelectionChanged">
                <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                <OtherMonthDayStyle ForeColor="#999999" />
                <DayStyle BackColor="#CCCCCC" />
                <TodayDayStyle BackColor="#999999" ForeColor="White" />
                <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="White" />
                <DayHeaderStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" Height="8pt" />
                <TitleStyle BackColor="#333399" BorderStyle="Solid" Font-Bold="True" Font-Size="12pt"
                    ForeColor="White" Height="12pt" />
            </asp:Calendar>
        </td>        
    </tr>
</table>
