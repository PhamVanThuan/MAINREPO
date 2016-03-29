<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true" CodeBehind="Attorney.aspx.cs" Inherits="SAHL.Web.Views.Administration.Attorney" Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <script language="javascript" type="text/javascript">

        function selectAddress(key, description) {
            if (confirm('Are you sure you want to select the following address?\n\n' + description)) {
                document.forms[0].addressKey.value = key;
                document.getElementById('<%=SelectAddress.ClientID %>').click();
    }
}

    </script>
    <input type="hidden" name="addressKey" value="" />
    <asp:Button ID="SelectAddress" runat="server" Text="" Visible="True" Width="0px" Height="0px" OnClick="SelectAddress_Click" />
    <asp:Panel ID="pnlAttorneySelect" runat="server" Width="98%" GroupingText="Select Deeds Office & Attorney" Visible="true">
        <table class="tableStandard" width="100%">
            <tr>
                <td align="left" valign="top">
                    <table id="InfoTable1" runat="server" class="tableStandard" width="100%">
                        <tr>
                            <td style="width: 15%;" class="TitleText">Deeds Office</td>
                            <td style="width: 35%">
                                <SAHL:SAHLDropDownList ID="ddlDeedsOffice" runat="server" Width="100%" PleaseSelectItem="true"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlDeedsOffice_SelectedIndexChanged">
                                </SAHL:SAHLDropDownList>
                            </td>
                            <td style="width: 15%">&nbsp;</td>
                            <td style="width: 35%">&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="TitleText">
                                <SAHL:SAHLLabel ID="tdAttorney" runat="server" Style="width: 330px;" Font-Bold="true" Visible="false">Attorney</SAHL:SAHLLabel>&nbsp;
                            </td>
                            <td>
                                <SAHL:SAHLDropDownList ID="ddlAttorney" runat="server" Width="100%" PleaseSelectItem="true"
                                    EnableViewState="False" AutoPostBack="True" OnSelectedIndexChanged="ddlAttorney_SelectedIndexChanged" Visible="false">
                                </SAHL:SAHLDropDownList>
                                &nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlAttorneyDetails" runat="server" Width="98%" GroupingText="Attorney Details" Visible="false">
        <table class="tableStandard" width="100%">
            <tr>
                <td align="left" valign="top">
                    <table id="InfoTable2" runat="server" class="tableStandard" width="100%">

                        <tr id="tdAttorneyNumber">
                            <td style="width: 15%;" class="TitleText">Attorney Number
                            </td>
                            <td style="width: 35%;">
                                <SAHL:SAHLLabel ID="lblAttorneyNumber" runat="server" />
                            </td>
                            <td id="tdStatus" style="width: 15%;" class="TitleText">Status
                            </td>
                            <td style="width: 35%;">
                                <SAHL:SAHLLabel ID="lblStatus" runat="server" />
                                <SAHL:SAHLDropDownList ID="ddlStatus" Width="30%" runat="Server" Mandatory="true" CssClass="mandatory" PleaseSelectItem="false"></SAHL:SAHLDropDownList></td>
                        </tr>
                        <tr>
                            <td style="width: 15%;" class="TitleText">Attorney Name</td>
                            <td style="width: 35%;">
                                <SAHL:SAHLLabel ID="lblAttorneyName" Width="85%" runat="server" />
                                <SAHL:SAHLTextBox ID="txtAttorneyName" Width="85%" runat="server" MaxLength="100" Mandatory="true" CssClass="mandatory"></SAHL:SAHLTextBox></td>
                            <td style="width: 15%;" class="TitleText">Workflow Enabled</td>
                            <td style="width: 35%;">
                                <SAHL:SAHLLabel ID="lblWorkflowEnabled" Width="30%" runat="server" />
                                <SAHL:SAHLDropDownList ID="ddlWorkflowEnabled" Width="30%" runat="Server" Mandatory="true" CssClass="mandatory"></SAHL:SAHLDropDownList></td>
                        </tr>
                        <tr>
                            <td class="TitleText">Attorney Contact</td>
                            <td>
                                <SAHL:SAHLLabel ID="lblAttorneyContact" runat="server" />
                                <SAHL:SAHLTextBox ID="txtAttorneyContact" Width="85%" runat="server" MaxLength="50" Mandatory="true" CssClass="mandatory"></SAHL:SAHLTextBox></td>
                            <td class="TitleText">Attorney Mandate</td>
                            <td>
                                <SAHL:SAHLLabel ID="lblAttorneyMandate" Width="30%" runat="server" />
                                <SAHL:SAHLCurrencyBox ID="txtAttorneyMandate" runat="server" Width="20%" MaxLength="12" Mandatory="true" CssClass="mandatory"></SAHL:SAHLCurrencyBox></td>
                        </tr>
                        <tr>
                            <td class="TitleText">Phone Number</td>
                            <td>
                                <SAHL:SAHLLabel ID="lblPhoneNumberCode" runat="server" />
                                <SAHL:SAHLLabel ID="lblPhoneNumber" runat="server" />
                                <SAHL:SAHLTextBox ID="txtPhoneNumberCode" runat="server" Width="18%" DisplayInputType="Number" MaxLength="10" Mandatory="true" CssClass="mandatory"></SAHL:SAHLTextBox>
                                <SAHL:SAHLTextBox ID="txtPhoneNumber" runat="server" Width="65%" DisplayInputType="Number" MaxLength="15" Mandatory="true" CssClass="mandatory"></SAHL:SAHLTextBox></td>
                            <td class="TitleText">Registration Attorney</td>
                            <td>
                                <SAHL:SAHLLabel Width="30%" ID="lblRegistrationAttorney" runat="server" />
                                <SAHL:SAHLDropDownList ID="ddlRegistrationAttorney" Width="30%" runat="Server" Mandatory="true" CssClass="mandatory"></SAHL:SAHLDropDownList></td>
                        </tr>
                        <tr>
                            <td class="TitleText">Email Address</td>
                            <td>
                                <SAHL:SAHLLabel ID="lblEmailAddress" runat="server" />
                                <SAHL:SAHLTextBox ID="txtEmailAddress" runat="server" Width="85%" MaxLength="50" Mandatory="true" CssClass="mandatory"></SAHL:SAHLTextBox></td>
                            <td class="TitleText">Litigation Attorney</td>
                            <td>
                                <SAHL:SAHLLabel ID="lblLitigationAttorney" Width="30%" runat="server" />
                                <SAHL:SAHLDropDownList ID="ddlLitigationAttorney" Width="30%" runat="Server" Mandatory="true" CssClass="mandatory" AutoPostBack="True"></SAHL:SAHLDropDownList></td>
                        </tr>
                        <tr>
                            <td id="tdDeedsOfficeChange" class="TitleText">Deeds Office</td>
                            <td>
                                <SAHL:SAHLDropDownList ID="ddlDeedsOfficeChange" Width="85%" runat="server" Mandatory="true" CssClass="mandatory"></SAHL:SAHLDropDownList></td>
                            <td class="TitleText">&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlAddress" runat="server" Width="98%" GroupingText="Attorney Address" Visible="false">
        <table class="tableStandard" width="100%">
            <tr>
                <td align="left" valign="top">
                    <table id="Table1" runat="server" class="tableStandard" width="100%">
                        <tr>
                            <td style="width: 15%;" class="TitleText">Address Type</td>
                            <td style="width: 35%;">
                                <SAHL:SAHLLabel ID="lblAddressType" runat="server">-</SAHL:SAHLLabel>
                                <SAHL:SAHLDropDownList ID="ddlAddressType" Width="85%" runat="server" AutoPostBack="True" Mandatory="true" CssClass="mandatory"
                                    PleaseSelectItem="false" OnSelectedIndexChanged="ddlAddressType_SelectedIndexChanged">
                                </SAHL:SAHLDropDownList></td>
                            <td style="width: 15%;" class="TitleText">Address Format</td>
                            <td style="width: 35%;">
                                <SAHL:SAHLLabel ID="lblAddressFormat" runat="server">-</SAHL:SAHLLabel>
                                <SAHL:SAHLDropDownList ID="ddlAddressFormat" runat="server" AutoPostBack="True" Mandatory="true" CssClass="mandatory"
                                    PleaseSelectItem="false" OnSelectedIndexChanged="ddlAddressFormat_SelectedIndexChanged" Width="85%">
                                </SAHL:SAHLDropDownList></td>
                        </tr>
                        <tr>
                            <td class="TitleText">Effective Date</td>
                            <td>
                                <SAHL:SAHLLabel ID="lblEffectiveDate" runat="server"></SAHL:SAHLLabel>
                                <SAHL:SAHLDateBox ID="dtEffectiveDate" runat="server" Mandatory="true" CssClass="mandatory"></SAHL:SAHLDateBox></td>
                            <td class="TitleText">&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
        <SAHL:SAHLLabel ID="lblAddress" runat="server" CssClass="LabelText" Height="60px" Width="200px">-</SAHL:SAHLLabel>
        <cc1:AddressDetails ID="addressDetails" runat="server" ClientAddressSelectFunction="selectAddress">
        </cc1:AddressDetails>
    </asp:Panel>
    <br />
    <div id="buttons" class="buttonBar" style="width: 99%">
        <SAHL:SAHLButton ID="btnContacts" runat="server" Text="Contacts" OnClick="ContactsButton_Click" Visible="false" SecurityTag="UpdateLitigationAttorneyContacts" SecurityHandler="Custom" SecurityDisplayType="Disable" />
        <SAHL:SAHLButton ID="CancelButton" runat="server" Text="Cancel" AccessKey="C" OnClick="CancelButton_Click" CausesValidation="false" Visible="false" />
        <SAHL:SAHLButton ID="SubmitButton" runat="server" Text="Submit" AccessKey="S" OnClick="SubmitButton_Click" Visible="false" />
    </div>
</asp:Content>
