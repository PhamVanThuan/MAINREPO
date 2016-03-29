<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="EstateAgentLegalEntity.aspx.cs" Inherits="SAHL.Web.Views.Administration.EstateAgentLegalEntity"
    Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

    <script language="javascript" type="text/javascript">

function selectAddress(key, description)
{
    if (confirm('Are you sure you want to select the following address?\n\n' + description))
    { 
        document.forms[0].addressKey.value = key;
        document.getElementById('<%=SelectAddress.ClientID %>').click();
    }
}
    </script>

    <input type="hidden" name="addressKey" value="" />
    <asp:Button ID="SelectAddress" runat="server" Text="" Visible="True" Width="0px"
        Height="0px" />
    <br />
    <table class="tableStandard" width="100%">
        <tr style="width: 100%">
            <td id="tdMain" runat="server">
                <asp:Panel ID="pnlLEViewHeader" runat="server" Width="99%" Height="100%" CssClass="headerPanel"
                    Visible="false">
                    <table>
                        <tr>
                            <td valign="middle">
                                <asp:Label ID="label1" runat="server" Text="Legal Entity Details" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <ajaxToolkit:Accordion ID="accEstateAgentLE" runat="server" SelectedIndex="-1" HeaderCssClass="accordionHeader"
                    HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="" FadeTransitions="false"
                    FramesPerSecond="40" TransitionDuration="250" AutoSize="None" SuppressHeaderPostbacks="true">
                    <Panes>
                        <ajaxToolkit:AccordionPane ID="apLegalEntityDetails" runat="server" Enabled="true">
                            <Header>
                                <a href="">Legal Entity Details</a>
                            </Header>
                            <Content>
                                <asp:Panel ID="pnlLegalEntityDetail" runat="server" Width="100%">
                                    <table class="tableStandard" width="100%">
                                        <tr style="width: 100%;">
                                            <td style="width: 50%;">
                                                <table class="tableStandard" width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="pnlAdd" runat="server" Width="100%">
                                                                <table id="tblLegalEntityType" class="tableStandard" width="100%">
                                                                    <tr style="width: 100%">
                                                                        <td runat="server" style="width: 30%; height: 24px;" class="TitleText">
                                                                            Type
                                                                        </td>
                                                                        <td runat="server" style="width: 70%; height: 24px;">
                                                                            <SAHL:SAHLLabel ID="lblLEType" runat="server">-</SAHL:SAHLLabel>
                                                                            <SAHL:SAHLDropDownList ID="ddlLegalEntityTypes" runat="server" Width="80%" PleaseSelectItem="False"
                                                                                AutoPostBack="True" OnSelectedIndexChanged="ddlLegalEntityTypes_SelectedIndexChanged"
                                                                                TabIndex="5">
                                                                            </SAHL:SAHLDropDownList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <asp:Panel ID="pnlNaturalPersonAdd" runat="server" Width="100%" Visible="False">
                                                                    <table class="tableStandard" width="100%">
                                                                        <tr>
                                                                            <td class="TitleText" id="tdRole" runat="server" style="width: 30%;">
                                                                                Role
                                                                            </td>
                                                                            <td style="width: 70%;">
                                                                                <SAHL:SAHLLabel ID="lblRole" runat="server">-</SAHL:SAHLLabel>
                                                                                <SAHL:SAHLDropDownList ID="ddlOSDescriptionTypeAdd" runat="server" Width="80%" PleaseSelectItem="true"
                                                                                    TabIndex="10">
                                                                                </SAHL:SAHLDropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="TitleText">
                                                                                Introduction Date</td>
                                                                            <td>
                                                                                <SAHL:SAHLLabel ID="lblNatIntroductionDate" runat="server"></SAHL:SAHLLabel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="TitleText">
                                                                                ID Number</td>
                                                                            <td>
                                                                                <SAHL:SAHLLabel ID="lblIDNumber" runat="server">-</SAHL:SAHLLabel>
                                                                                <SAHL:SAHLTextBox ID="txtNatAddIDNumber" runat="server" MaxLength="20" Width="80%"
                                                                                    DisplayInputType="Number" TabIndex="17" CssClass="mandatory">
                                                                                </SAHL:SAHLTextBox>
                                                                                <SAHL:SAHLAutoComplete AutoPostBack="true" runat="server" ID="acNatAddIDNumber" TargetControlID="txtNatAddIDNumber"
                                                                                    ServiceMethod="SAHL.Web.AJAX.LegalEntity.GetLegalEntitiesByIDNumber" MinCharacters="6">
                                                                                </SAHL:SAHLAutoComplete>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="TitleText">
                                                                                Salutation</td>
                                                                            <td>
                                                                                <SAHL:SAHLLabel ID="lblSalutation" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel>
                                                                                <SAHL:SAHLDropDownList ID="ddlNatAddSalutation" runat="server" Width="80%" PleaseSelectItem="False"
                                                                                    TabIndex="20">
                                                                                </SAHL:SAHLDropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="TitleText">
                                                                                Initials</td>
                                                                            <td>
                                                                                <SAHL:SAHLLabel ID="lblInitials" runat="server" CssClass="LabelText" TextAlign="left">-</SAHL:SAHLLabel>
                                                                                <SAHL:SAHLTextBox ID="txtNatAddInitials" runat="server" MaxLength="5" Width="80%"
                                                                                    TabIndex="25"></SAHL:SAHLTextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="TitleText">
                                                                                First Names</td>
                                                                            <td>
                                                                                <SAHL:SAHLLabel ID="lblFirstNames" runat="server" CssClass="LabelText" TextAlign="left">-</SAHL:SAHLLabel>
                                                                                <SAHL:SAHLTextBox ID="txtNatAddFirstNames" runat="server" MaxLength="50" Width="80%"
                                                                                    TabIndex="30" CssClass="mandatory"></SAHL:SAHLTextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="TitleText">
                                                                                Surname</td>
                                                                            <td>
                                                                                <SAHL:SAHLLabel ID="lblSurname" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel>
                                                                                <SAHL:SAHLTextBox ID="txtNatAddSurname" runat="server" MaxLength="50" Width="80%"
                                                                                    TabIndex="35" CssClass="mandatory"></SAHL:SAHLTextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="TitleText">
                                                                                Preferred Name</td>
                                                                            <td>
                                                                                <SAHL:SAHLLabel ID="lblPreferredName" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                                                                <SAHL:SAHLTextBox ID="txtNatAddPreferredName" runat="server" MaxLength="50" Width="80%"
                                                                                    TabIndex="40"></SAHL:SAHLTextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="TitleText">
                                                                                Gender</td>
                                                                            <td>
                                                                                <SAHL:SAHLLabel ID="lblGender" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel>
                                                                                <SAHL:SAHLDropDownList ID="ddlNatAddGender" runat="server" Width="80%" PleaseSelectItem="False"
                                                                                    TabIndex="45">
                                                                                </SAHL:SAHLDropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="TitleText">
                                                                                Status</td>
                                                                            <td>
                                                                                <SAHL:SAHLLabel ID="lblStatus" runat="server">-</SAHL:SAHLLabel>
                                                                                <SAHL:SAHLDropDownList ID="ddlNatAddStatus" runat="server" Width="80%" PleaseSelectItem="False"
                                                                                    TabIndex="100">
                                                                                </SAHL:SAHLDropDownList></td>
                                                                            <td>
                                                                        </tr>
                                                                        <tr>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                                <asp:Panel ID="pnlCompanyAdd" runat="server" Width="100%" Visible="false">
                                                                    <table width="100%" class="tableStandard">
                                                                        <tr>
                                                                            <td id="tdCoRoleType" runat="server" class="TitleText" style="width: 30%;">
                                                                                Organisation Type
                                                                            </td>
                                                                            <td class="cellDisplay" style="width: 70%;">
                                                                                <SAHL:SAHLLabel ID="lblOrganisationTypeDisplay" runat="server" CssClass="LabelText"
                                                                                    TextAlign="Left">-</SAHL:SAHLLabel>
                                                                                <SAHL:SAHLDropDownList ID="ddlOrganisationType" runat="server" PleaseSelectItem="False"
                                                                                    Width="80%" TabIndex="10" SecurityTag="LegalEntityRoleTypeChange" SecurityDisplayType="Disable">
                                                                                </SAHL:SAHLDropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="TitleText">
                                                                                Introduction Date</td>
                                                                            <td class="cellDisplay">
                                                                                <SAHL:SAHLLabel ID="lblCOIntroductionDate" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                                                                <SAHL:SAHLDateBox ID="COIntroductionDateUpdate" runat="server" Visible="false" Width="80%"
                                                                                    TabIndex="20" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="TitleText">
                                                                                Company Name</td>
                                                                            <td>
                                                                                <SAHL:SAHLLabel ID="lblCORegisteredName" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                                                                <SAHL:SAHLTextBox ID="CORegisteredNameUpdate" runat="server" MaxLength="100" Width="80%"
                                                                                    TabIndex="30" CssClass="mandatory"></SAHL:SAHLTextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="TitleText">
                                                                                Trading Name</td>
                                                                            <td>
                                                                                <SAHL:SAHLLabel ID="lblCOTradingName" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                                                                <SAHL:SAHLTextBox ID="txtCOUpdTradingName" runat="server" MaxLength="50" Width="80%"
                                                                                    TabIndex="35"></SAHL:SAHLTextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td id="RegNumberLabel" runat="server" class="TitleText">
                                                                                Registration Number</td>
                                                                            <td>
                                                                                <SAHL:SAHLLabel ID="lblCORegistrationNumber" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                                                                <SAHL:SAHLTextBox ID="CORegistrationNumberUpdate" runat="server" MaxLength="20" Width="80%"
                                                                                    TabIndex="40" CssClass="mandatory"></SAHL:SAHLTextBox>
                                                                                <SAHL:SAHLAutoComplete AutoPostBack="true" runat="server" ID="acCORegistrationNumberUpdate"
                                                                                    TargetControlID="CORegistrationNumberUpdate" ServiceMethod="SAHL.Web.AJAX.LegalEntity.GetLegalEntitiesByRegistrationNumber">
                                                                                    <ParentControls>
                                                                                        <SAHL:SAHLAutoCompleteParentControl ControlID="ddlLegalEntityTypes" />
                                                                                    </ParentControls>
                                                                                </SAHL:SAHLAutoComplete>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="TitleText">
                                                                                Status</td>
                                                                            <td>
                                                                                <SAHL:SAHLLabel ID="lblCOStatus" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                                                                <SAHL:SAHLDropDownList ID="ddlCOStatus" runat="server" Width="80%" PleaseSelectItem="False"
                                                                                    TabIndex="25">
                                                                                </SAHL:SAHLDropDownList>
                                                                                &nbsp;
                                                                            </td>
                                                                    </table>
                                                                </asp:Panel>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td style="width: 50%;">
                                                <asp:Panel ID="pnlContactDetails" runat="server" Width="100%">
                                                    <table class="tableStandard" width="100%">
                                                        <tr id="ContactDetails" runat="server" align="left" style="width: 100%">
                                                            <td style="width: 100%">
                                                                <asp:Panel ID="Panel5" runat="server" CssClass="TitleText" GroupingText="Contact Details"
                                                                    Width="100%">
                                                                    <table id="tblContactDetails" runat="server" class="tableStandard" width="100%">
                                                                        <tr id="trHomePhone" runat="server" style="width: 100%">
                                                                            <td class="TitleText" style="width: 20%">
                                                                                Home Phone</td>
                                                                            <td style="width: 75%">
                                                                                <SAHL:SAHLLabel ID="lblHomePhone" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel><SAHL:SAHLTextBox
                                                                                    ID="txtHomePhoneCode" runat="server" DisplayInputType="Number" MaxLength="10"
                                                                                    TabIndex="110" Width="42px"></SAHL:SAHLTextBox><SAHL:SAHLTextBox ID="txtHomePhoneNumber"
                                                                                        runat="server" DisplayInputType="Number" MaxLength="15" TabIndex="111" Width="136px"></SAHL:SAHLTextBox>
                                                                            </td>
                                                                            <td style="width: 5%">
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="TitleText">
                                                                                Work Phone</td>
                                                                            <td>
                                                                                <SAHL:SAHLLabel ID="lblWorkPhone" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel><SAHL:SAHLTextBox
                                                                                    ID="txtWorkPhoneCode" runat="server" DisplayInputType="Number" MaxLength="10"
                                                                                    TabIndex="112" Width="42px"></SAHL:SAHLTextBox><SAHL:SAHLTextBox ID="txtWorkPhoneNumber"
                                                                                        runat="server" DisplayInputType="Number" MaxLength="15" TabIndex="113" Width="136px"></SAHL:SAHLTextBox></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="TitleText">
                                                                                Fax Number</td>
                                                                            <td>
                                                                                <SAHL:SAHLLabel ID="lblFaxNumber" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel><SAHL:SAHLTextBox
                                                                                    ID="txtFaxCode" runat="server" DisplayInputType="Number" MaxLength="10" TabIndex="114"
                                                                                    Width="42px"></SAHL:SAHLTextBox><SAHL:SAHLTextBox ID="txtFaxNumber" runat="server"
                                                                                        DisplayInputType="Number" MaxLength="15" TabIndex="115" Width="136px"></SAHL:SAHLTextBox></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="TitleText">
                                                                                Cellphone No</td>
                                                                            <td>
                                                                                <SAHL:SAHLLabel ID="lblCellphoneNumber" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel><SAHL:SAHLTextBox
                                                                                    ID="txtCellPhoneNumber" runat="server" DisplayInputType="Number" MaxLength="15"
                                                                                    TabIndex="116" Width="182px" CssClass="mandatory"></SAHL:SAHLTextBox></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="TitleText">
                                                                                Email Address</td>
                                                                            <td>
                                                                                <SAHL:SAHLLabel ID="lblEmailAddress" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel><SAHL:SAHLTextBox
                                                                                    ID="txtEmailAddress" runat="server" DisplayInputType="AlphaNumNoSpace" MaxLength="50"
                                                                                    TabIndex="117" Width="182px" CssClass="mandatory"></SAHL:SAHLTextBox></td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <br />
                                </asp:Panel>
                                <table id="leSummaryDetails" runat="server" class="tableStandard" width="100%" visible="false">
                                    <tr>
                                        <td id="tdAddressDetails" runat="server" style="width: 50%;">
                                            <asp:Panel ID="pnlContDet" runat="server" GroupingText="Legal Entity Contact Details"
                                                CssClass="TitleText" Width="100%">
                                            </asp:Panel>
                                        </td>
                                        <td id="tdContactDetails" runat="server" style="width: 50%;">
                                            <asp:Panel ID="pnlAddDet" runat="server" GroupingText="Legal Entity Address - " CssClass="TitleText"
                                                Width="100%">
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </Content>
                        </ajaxToolkit:AccordionPane>
                        <ajaxToolkit:AccordionPane ID="apLegalEntityAddDetails" runat="server" Enabled="true">
                            <Header>
                                <a href="">Legal Entity Address Details</a>
                            </Header>
                            <Content>
                                <asp:Panel ID="pnlAddressDetails" runat="server" Width="100%">
                                    <table id="tblAddressDetails" runat="server" class="tableStandard" width="100%">
                                        <tr>
                                            <td>
                                                <table class="tableStandard" width="100%">
                                                    <tr>
                                                        <td align="left" valign="top">
                                                            <table id="Table1" runat="server" class="tableStandard" width="100%">
                                                                <tr>
                                                                    <td style="width: 15%;" class="TitleText">
                                                                        Address Type</td>
                                                                    <td style="width: 35%;">
                                                                        <SAHL:SAHLLabel ID="lblAddressType" runat="server">-</SAHL:SAHLLabel>
                                                                        <SAHL:SAHLDropDownList ID="ddlAddressType" Width="85%" runat="server" AutoPostBack="True"
                                                                            Mandatory="true" CssClass="mandatory" PleaseSelectItem="false" OnSelectedIndexChanged="ddlAddressType_SelectedIndexChanged">
                                                                        </SAHL:SAHLDropDownList></td>
                                                                    <td style="width: 15%;" class="TitleText">
                                                                        Address Format</td>
                                                                    <td style="width: 30%;">
                                                                        <SAHL:SAHLLabel ID="lblAddressFormat" runat="server">-</SAHL:SAHLLabel>
                                                                        <SAHL:SAHLDropDownList ID="ddlAddressFormat" runat="server" AutoPostBack="True" Mandatory="true"
                                                                            CssClass="mandatory" PleaseSelectItem="false" OnSelectedIndexChanged="ddlAddressFormat_SelectedIndexChanged"
                                                                            Width="85%">
                                                                        </SAHL:SAHLDropDownList></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="TitleText">
                                                                        Effective Date</td>
                                                                    <td>
                                                                        <SAHL:SAHLLabel ID="lblEffectiveDate" runat="server"></SAHL:SAHLLabel>
                                                                        <SAHL:SAHLDateBox ID="dtEffectiveDate" runat="server" Mandatory="true" CssClass="mandatory"></SAHL:SAHLDateBox></td>
                                                                    <td class="TitleText">
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <asp:Panel ID="pnlAddressLabel" runat="server" CssClass="TitleText" GroupingText="Address"
                                                    Width="50%">
                                                    <SAHL:SAHLLabel ID="lblAddress" runat="server" CssClass="LabelText" Width="50%">-</SAHL:SAHLLabel>
                                                </asp:Panel>
                                                <cc1:AddressDetails ID="addressDetails" runat="server" ClientAddressSelectFunction="selectAddress">
                                                </cc1:AddressDetails>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </Content>
                        </ajaxToolkit:AccordionPane>
                    </Panes>
                </ajaxToolkit:Accordion>
                <asp:Panel ID="pnlInfo" runat="server">
                    <hr />
                    &nbsp&nbsp<asp:Image ID="Image1" runat="server" ImageUrl="../../Images/help.gif" />
                    Click the blue arrow above to expand/collapse the panes for more information.
                    <hr />
                </asp:Panel>
                <asp:Panel ID="pnlAddressGrid" runat="server" Visible="false" Width="99%">
                    <cc1:AddressGrid ID="grdAddress" runat="server" GridHeight="130px">
                    </cc1:AddressGrid>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <table class="tableStandard" width="100%">
        <tr id="ButtonRow" runat="server" align="right">
            <td align="right">
                <SAHL:SAHLButton ID="btnSubmitButton" runat="server" Visible="false" AccessKey="U"
                    Text="Update" OnClick="btnSubmitButton_Click" />
                <SAHL:SAHLButton ID="btnCancelButton" runat="server" AccessKey="C" CausesValidation="False"
                    Text="Cancel" OnClick="btnCancelButton_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
