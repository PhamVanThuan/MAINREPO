<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    CodeBehind="AttorneyContact.aspx.cs" Inherits="SAHL.Web.Views.Administration.AttorneyContact" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <script src="../../Scripts/jquery-1.5.min.js" type="text/javascript"></script>
    <script type="text/javascript">
    </script>
    <table class="tableStandard">
        <tr class="rowStandard">
            <td>
                <SAHL:SAHLLabel runat="server" ID="lblAttorneyName" Font-Bold="true" Text="Attorney Name : "></SAHL:SAHLLabel>   
            </td>
            <td>
                <SAHL:SAHLLabel runat="server" ID="lblAttorneyNameValue"></SAHL:SAHLLabel>
            </td>
        </tr>
    </table>
    <div id="gridSection" runat="server">
        <table id="tbllitigationAttorney" runat="server" visible="true" style="width: 100%;
            height: 100%">
            <tr style="width: 100%; height: 100%">
                <td>
                    <SAHL:DXGridView ID="litigationAttorneyContactsGrid" runat="server" AutoGenerateColumns="False"
                        Width="100%" PostBackType="None" ClientInstanceName="grid" Settings-ShowTitlePanel="true"
                        SettingsText-Title="Litigation Attorney Contacts">
                        <SettingsEditing Mode="Inline" />
                    </SAHL:DXGridView>
                </td>
            </tr>
        </table>
        <br />
    </div>
    <br />
    <div id="legalEntitySection" runat="server">
        <fieldset>
            <legend style="font-weight: bold;">Add Legal Entity</legend>
            <table class="tableStandard">
                <tr class=>
                    <td>
                        <SAHL:SAHLLabel runat="server" ID="lblFirstName">First Name</SAHL:SAHLLabel>
                    </td>
                    <td>
                        <SAHL:SAHLTextBox runat="server" ID="txtFirstName" CssClass="mandatory"></SAHL:SAHLTextBox>
                    </td>
                    <td>
                        <SAHL:SAHLLabel runat="server" ID="lblSurname">Surname</SAHL:SAHLLabel>
                    </td>
                    <td>
                        <SAHL:SAHLTextBox runat="server" ID="txtSurname" CssClass="mandatory"></SAHL:SAHLTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <SAHL:SAHLLabel runat="server" ID="lblTelephone">Telephone Number</SAHL:SAHLLabel>
                    </td>
                    <td>
                        <SAHL:SAHLPhone runat="server" ID="txtTelephoneNumber" CssClass="mandatory" />
                    </td>
                    <td>
                        <SAHL:SAHLLabel runat="server" ID="lblFaxNumber">Fax Number</SAHL:SAHLLabel>
                    </td>
                    <td>
                        <SAHL:SAHLPhone runat="server" ID="txtFaxNumber" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <SAHL:SAHLLabel runat="server" ID="lblEmailAddress">Email Address</SAHL:SAHLLabel>
                    </td>
                    <td>
                        <SAHL:SAHLTextBox runat="server" ID="txtEmailAddress" CssClass="mandatory"></SAHL:SAHLTextBox>
                    </td>
                    <td>
                        <SAHL:SAHLLabel runat="server" ID="lblRoleType">Role Type</SAHL:SAHLLabel>
                    </td>
                    <td>
                        <SAHL:SAHLDropDownList runat="server" ID="cmbRoleType" CssClass="mandatory">
                        </SAHL:SAHLDropDownList>
                    </td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr style="width: 100%" align="right">
                    <td style="width: 100%" align="right">
                        <SAHL:SAHLButton runat="server" ID="btnAdd" Text="Add Legal Entity" OnClick="OnAddLegalEntityClick" />
                        <SAHL:SAHLButton runat="server" ID="btnDone" Text="Done" OnClick="OnDoneClick" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>
