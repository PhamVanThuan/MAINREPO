<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Codebehind="RuleSetAdministration.aspx.cs" Inherits="SAHL.Web.Views.RuleSetAdministration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <br />
    <SAHL:SAHLGridView ID="GridAvailableRuleSets" runat="server" Height="1px" Width="799px" Caption="Available Rule Sets" OnSelectedIndexChanged="GridAvailableRuleSets_SelectedIndexChanged" PostBackType="SingleClick">
    </SAHL:SAHLGridView>
    <br />
    <div runat="server" id="divRuleSetView">
        <SAHL:SAHLLabel ID="SAHLLabel1" runat="server" CssClass="LabelText" >Rules:</SAHL:SAHLLabel>
        <br />
        <asp:ListBox ID="lstRules" runat="server" Height="248px" Width="800px"></asp:ListBox>
    </div>
    <table style="width: 800px" id="tblRuleSetUpdate" runat="server">
        <tr>
            <td style="width: 38px">
                <SAHL:SAHLLabel ID="SAHLLabel2" runat="server" CssClass="LabelText">Name:</SAHL:SAHLLabel></td>
            <td style="width: 123px" align="left">
                &nbsp;<SAHL:SAHLTextBox ID="txtRuleSetName" runat="server" Width="464px"></SAHL:SAHLTextBox></td>
            <td style="width: 14px">
                <SAHL:SAHLRequiredFieldValidator ID="SAHLRequiredFieldValidator1" runat="server"
                    ControlToValidate="txtRuleSetName" />
            </td>
        </tr>
        <tr>
            <td style="width: 38px">
                <SAHL:SAHLLabel ID="SAHLLabel3" runat="server" CssClass="LabelText">Description:</SAHL:SAHLLabel></td>
            <td style="width: 123px" align="left">
                &nbsp;<SAHL:SAHLTextBox ID="txtRuleDescription" runat="server" Width="463px"></SAHL:SAHLTextBox></td>
            <td style="width: 14px">
                <SAHL:SAHLRequiredFieldValidator ID="SAHLRequiredFieldValidator2" runat="server"
                    ControlToValidate="txtRuleDescription" />
            </td>
        </tr>
        <tr>
            <td style="width: 38px">
            </td>
            <td style="width: 123px">
            </td>
            <td style="width: 14px">
            </td>
        </tr>
        <tr>
            <td style="width: 38px">
            </td>
            <td style="width: 123px">
            </td>
            <td style="width: 14px">
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center" style="height: 21px">
                <SAHL:SAHLLabel ID="SAHLLabel4" runat="server" CssClass="LabelText" >Rules:</SAHL:SAHLLabel></td>
            <td align="center" colspan="1" style="width: 14px; height: 21px;">
            </td>
        </tr>
        <tr>
            <td colspan="2">
                    <div style="float: right; width: 257px; height: 120px; border-style: ridge; border-width: thin">
                        <SAHL:SAHLCheckboxList ID="lstSelectedRules" runat="server" Height="165px" Width="325px" BorderColor="Black" BorderWidth="1px" RepeatLayout="Flow">
                        </SAHL:SAHLCheckboxList></div>
                    <div style="float: left; width: 256px; height: 123px" id="DIV1">
                        <asp:ListBox ID="lstAvailableRules" runat="server" Height="175px" Width="330px" BackColor="Transparent"></asp:ListBox></div>
                    <br />
                    <br />
                    <div style="margin-left: auto; width: 76px; margin-right: auto; text-align: center">
                        <br />
                        <SAHL:SAHLButton ID="btnAdd" runat="server" Width="65px" Text="Add" OnClick="btnAdd_Click" CausesValidation="False"> </SAHL:SAHLButton>  <br />
                        <br />
                        <SAHL:SAHLButton ID="btnRemove" runat="server" Width="65px" Text="Remove" OnClick="btnRemove_Click" CausesValidation="False"></SAHL:SAHLButton><br />
                    </div>            
            </td>
            <td colspan="1" style="width: 14px">
            </td>
        </tr>
        <tr>
            <td align="right" colspan="2">
                    <SAHL:SAHLButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" /></td>
            <td align="right" colspan="1" style="width: 14px">
            </td>
        </tr>
    </table>
</asp:Content>
