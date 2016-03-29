<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.PersonalLoan.AssignUser" Title="Assign User" CodeBehind="AssignUser.aspx.cs" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <div>
        <br />
        <table style="width: 100%; height: 100%">
            <tr>
                <td align="left" style="width: 3%;">
                    <SAHL:SAHLLabel ID="titleExceptionsManagers" runat="server" CssClass="LabelText" TextAlign="left">
                        Users: </SAHL:SAHLLabel>
                </td>
                <td align="left" style="width: 9%;">
                    <SAHL:SAHLDropDownList ID="ddlExceptionsManager" runat="server" OnSelectedIndexChanged="ddlExceptionsManagerSelectedIndexChanged">
                    </SAHL:SAHLDropDownList>
                </td>
                <td>
                    <SAHL:SAHLRequiredFieldValidator ID="valExceptionsManager" runat="server" ControlToValidate="ddlExceptionsManager"
                        ErrorMessage="Please select a Personal Loan Credit Exceptions Manager" InitialValue="-select-" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                </td>
                <td align="left">
                     <SAHL:SAHLButton ID="btnSubmit" runat="server" Text="Submit" AccessKey="S" OnClick="btnSubmit_Click" />
                <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" AccessKey="C" CausesValidation="false"
                    OnClick="btnCancel_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
