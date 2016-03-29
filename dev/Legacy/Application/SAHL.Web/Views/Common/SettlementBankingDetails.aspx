<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Common.SettlementBankingDetails" Title="Settlement Banking Details"
    Codebehind="SettlementBankingDetails.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table border="0" cellspacing="0" cellpadding="0" class="PageBlock" style="height: 372px">
        <tr>
            <td align="left" style="height: 99%; width: 783px;" valign="top">
                <br />
                <SAHL:SAHLGridView ID="DetailsGrid" runat="server" AutoGenerateColumns="false" EmptyDataSetMessage="There are no Property Details."
                    EnableViewState="false" FixedHeader="false" GridHeight="100px" GridWidth="770px"
                    HeaderCaption="Property Details" Height="65px" NullDataSetMessage="" OnSelectedIndexChanged="DetailsGrid_SelectedIndexChanged"
                    PostBackType="SingleClick" Width="770px">
                    <RowStyle CssClass="TableRowA" />
                </SAHL:SAHLGridView>
                &nbsp; &nbsp;&nbsp;&nbsp;<br />
                <table id="ListControls" runat="server" border="0">
                    <tr>
                        <td class="TitleText">
                            Bank
                        </td>
                        <td colspan="4">
                            <SAHL:SAHLDropDownList ID="Bank" runat="server" AutoPostBack="True" Style="width: 100%;">
                            </SAHL:SAHLDropDownList>
                        </td>
                        <td>
                            <SAHL:SAHLRequiredFieldValidator ID="BankValidator" runat="server" ControlToValidate="Bank"
                                InitialValue="-select-" ErrorMessage="Please select a Bank" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 126px;" class="TitleText">
                            Branch Code
                        </td>
                        <td style="width: 136px;">
                            <SAHL:SAHLDropDownList ID="BranchCode" runat="server" Style="width: 100%;">
                            </SAHL:SAHLDropDownList>
                        </td>
                        <td style="width: 15px;">
                            <SAHL:SAHLRequiredFieldValidator ID="BranchCodeValidator" runat="server" ControlToValidate="BranchCode"
                                InitialValue="-select-" ErrorMessage="Please select a Branch code" />
                        </td>
                        <td style="width: 106px;" class="TitleText" align="center">
                            Branch Name
                        </td>
                        <td style="width: 354px;">
                            <SAHL:SAHLDropDownList ID="BranchName" runat="server" Style="width: 100%;">
                            </SAHL:SAHLDropDownList>
                        </td>
                        <td style="width: 15px;">
                            <SAHL:SAHLRequiredFieldValidator ID="BranchNameValidator" runat="server" ControlToValidate="BranchName"
                                InitialValue="-select-" ErrorMessage="Please select a Branch Name" />
                        </td>
                    </tr>
                    <tr>
                        <td class="TitleText">
                            Account Type
                        </td>
                        <td>
                            <SAHL:SAHLDropDownList ID="AccountType" runat="server" Style="width: 100%;">
                            </SAHL:SAHLDropDownList>
                        </td>
                        <td>
                            <SAHL:SAHLRequiredFieldValidator ID="AccountTypeValidator" runat="server" ControlToValidate="AccountType"
                                InitialValue="-select-" ErrorMessage="Please select an Account type" />
                        </td>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="TitleText">
                            Account Number
                        </td>
                        <td>
                            <SAHL:SAHLTextBox ID="AccountNumber" runat="server" Style="width: 98%;" DisplayInputType="Number"
                                MaxLength="25"></SAHL:SAHLTextBox>
                        </td>
                        <td>
                            <SAHL:SAHLCustomValidator ID="ValUniqueBankAccount" EnableViewState="true" ControlToValidate="AccountNumber"
                                OnServerValidate="ServerValidate" runat="server" ErrorMessage="A bank account record already exists for the details specified" />
                        </td>
                        <td>
                            <SAHL:SAHLRequiredFieldValidator ID="ValExistingBankAccount" runat="server" ControlToValidate="AccountNumber"
                                InitialValue="" ErrorMessage="Please enter an Account Number" />
                        </td>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="TitleText" style="height: 29px">
                            Account Name
                        </td>
                        <td colspan="4" style="height: 29px">
                            <SAHL:SAHLTextBox ID="AccountName" runat="server" Style="width: 99%;" MaxLength="255"></SAHL:SAHLTextBox>
                        </td>
                        <td style="height: 29px">
                            <SAHL:SAHLRequiredFieldValidator ID="AccountNameValidator" runat="server" ControlToValidate="AccountName"
                                InitialValue="" ErrorMessage="Please enter an Account Name" />
                        </td>
                    </tr>
                    <tr id="StatusRow" runat="server">
                        <td class="TitleText">
                            Status
                        </td>
                        <td>
                            <SAHL:SAHLDropDownList ID="BankAccountStatus" runat="server" Style="width: 100%;">
                            </SAHL:SAHLDropDownList>
                        </td>
                        <td>
                            <SAHL:SAHLRequiredFieldValidator ID="BankAccountStatusValidator" runat="server" ControlToValidate="BankAccountStatus"
                                InitialValue="-select-" ErrorMessage="Please select a Bank Account Status" />
                        </td>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Red" Height="35px"
                    Width="780px"></asp:Label></td>
        </tr>
        <tr id="ButtonRow" runat="server">
            <td align="right" style="width: 783px">
                <SAHL:SAHLButton ID="SubmitButton" runat="server" Text="Submit" AccessKey="S" OnClick="SubmitButton_Click" />
                <SAHL:SAHLButton ID="CancelButton" runat="server" Text="Cancel" AccessKey="C" OnClick="CancelButton_Click"
                    CausesValidation="false" />
            </td>
            <td style="width: 17px">
            </td>
        </tr>
    </table>
</asp:Content>
