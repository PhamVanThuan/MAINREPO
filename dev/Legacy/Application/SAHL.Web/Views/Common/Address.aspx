<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true" CodeBehind="Address.aspx.cs" Inherits="SAHL.Web.Views.Common.Address" Title="Untitled Page" EnableViewState="False" %>

<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <script language="javascript" type="text/javascript">
    
    function selectAddress(key, description)
    {
        if (confirm('Are you sure you want to select the following address?\n\n' + description))
        { 
            document.forms[0].addressKey.value = key;
            
            // try and find a button to click
            var btnClick = document.getElementById('<%=btnAdd.ClientID %>');
            if (btnClick == null)
                btnClick = document.getElementById('<%=btnUpdate.ClientID %>');
            if (btnClick != null)
                btnClick.click();
        }
    }
 
    </script>
    <br />
    <asp:Panel ID="pnlGrid" runat="server" Visible="true" Width="99%">
        <cc1:AddressGrid ID="grdAddress" runat="server" GridHeight="130px">
        </cc1:AddressGrid>
    </asp:Panel>
    <br />
    <SAHL:SAHLLabel ID="lblTitleText" runat="server" Visible="false" style="padding-top:5px;padding-bottom:5px;"></SAHL:SAHLLabel>
    <asp:Panel ID="pnlDisplay" runat="server" Width="99%" Visible="False" GroupingText="Address Details">
        <br />
        <asp:Label ID="lblDisplayAddress" runat="server" Text="No address selected" />
        <br />
    </asp:Panel>
    <div class="row">
        <asp:Panel ID="pnlInput" runat="server" Width="98%" Visible="False" style="margin-left:10px;">
            <div id="divLegalEntityInfo" runat="server" class="borderBottom row">
                <div class="row">
                    <div class="cellInput TitleText" style="width:150px;">Address Type</div>
                    <div class="cellInput" style="width:200px;">
                        <SAHL:SAHLDropDownList ID="ddlAddressType" runat="server" AutoPostBack="true" PleaseSelectItem="false"></SAHL:SAHLDropDownList>
                        <SAHL:SAHLLabel ID="lblAddressType" runat="server" Visible="false" />
                        <SAHL:SAHLRequiredFieldValidator ID="valAddressType" runat="server" ControlToValidate="ddlAddressType"
                            ErrorMessage="Please select an Address Type" InitialValue="-select-" />
                    </div>
                    <div class="cellInput TitleText" style="width:150px;">Address Format</div>
                    <div class="cellInput" style="width:200px;">
                        <SAHL:SAHLDropDownList ID="ddlAddressFormat" runat="server" PleaseSelectItem="false" AutoPostBack="True"></SAHL:SAHLDropDownList>
                        <SAHL:SAHLLabel ID="lblAddressFormat" runat="server" Visible="false" />
                        <SAHL:SAHLRequiredFieldValidator ID="valAddressFormat" runat="server" ControlToValidate="ddlAddressFormat"
                            ErrorMessage="Please select an Address Format" InitialValue="-select-" />
                    </div>
                </div>
                <div class="row">
                    <div class="cellInput TitleText" style="width:150px;">Effective Date</div>
                    <div class="cellInput" style="width:200px;">
                        <SAHL:SAHLDateBox ID="dtEffectiveDate" runat="server" />
                    </div>
                    <div id="divAddressStatus" runat="server" class="cellInput" visible="false">
                        <div class="cellInput TitleText colText" style="width:150px;">Address Status</div>
                        <div class="cellInput" style="width:200px;">
                            <SAHL:SAHLDropDownList ID="ddlAddressStatus" runat="server" PleaseSelectItem="false"></SAHL:SAHLDropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <br />
            <% // input form fields %>
            <br />
            <cc1:AddressDetails ID="addressDetails" runat="server" ClientAddressSelectFunction="selectAddress">
            </cc1:AddressDetails>
        </asp:Panel>
    </div>
    <div class="buttonBar" style="width:99%">
        &nbsp; &nbsp; &nbsp;
        <table style="width:99%">
            <tr id="ButtonRow" runat="server">
                <td style="width:20%" align="left">
        <SAHL:SAHLButton ID="btnBack" runat="server" ButtonSize="Size4" CausesValidation="false"
            CssClass="BtnNormal4" Text="Back" Visible="False" OnClick="OnCalculate_Click" /></td>
                <td style="width:40%">
                </td >
                <td  style="width:40%" align="right">
        <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" Visible="False" CssClass="buttonSpacer" CausesValidation="false" />
        <SAHL:SAHLButton ID="btnUpdate" runat="server" Text="Update" Visible="False" CssClass="buttonSpacer" />
        <SAHL:SAHLButton ID="btnDelete" runat="server" Text="Delete" Visible="False" CssClass="buttonSpacer" OnClientClick="return confirm('Are you sure you want to delete the selected address?');" />
        <SAHL:SAHLButton ID="btnAdd" runat="server" Text="Add" Visible="False" CssClass="buttonSpacer" />
        <SAHL:SAHLButton ID="btnAuditTrail" runat="server" Text="Audit Trail" Visible="False" CssClass="buttonSpacer" /></td>
            </tr>            
           
        </table>
    </div>
    <input type="hidden" name="addressKey" value="" />
</asp:Content>
