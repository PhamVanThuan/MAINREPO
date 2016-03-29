<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Common.LegalEntityDetails" Title="Untitled Page" CodeBehind="LegalEntityDetails.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <table class="tableStandard" width="100%">
        <tr>
            <td>
                <asp:Panel ID="pnlNaturalPersonDisplay" runat="server" Width="100%">
                    <table class="tableStandard" width="100%">
                        <tr>
                            <td class="TitleText" style="width: 15%">Type</td>
                            <td style="width: 20%" class="cellDisplay">
                                <SAHL:SAHLLabel ID="lblType" runat="server">-</SAHL:SAHLLabel>
                            </td>
                            <td style="width: 5%"></td>
                            <td style="width: 15%"></td>
                            <td style="width: 20%"></td>
                            <td style="width: 15%"></td>
                        </tr>
                        <tr>
                            <td id="tdNatRoleType" runat="server" class="TitleText">Role
                            </td>
                            <td class="cellDisplay">
                                <SAHL:SAHLLabel ID="lblRoleTypeDisplay" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel><SAHL:SAHLDropDownList
                                    ID="ddlRoleTypeUpdate" runat="server" Width="186px" PleaseSelectItem="False"
                                    TabIndex="5" SecurityTag="LegalEntityRoleTypeChange" SecurityDisplayType="Disable">
                                </SAHL:SAHLDropDownList>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr id="trNatUpdIncomeContributor" runat="server">
                            <td class="TitleText">Income Contributor</td>
                            <td class="cellDisplay">
                                <SAHL:SAHLLabel ID="lblNatIncomeContributor" runat="server">-</SAHL:SAHLLabel><asp:CheckBox
                                    ID="chkNatUpdIncomeContributor" runat="server" TabIndex="10" />
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td class="TitleText">ID Number</td>
                            <td class="cellDisplay">
                                <SAHL:SAHLLabel ID="lblIDNumber" runat="server">-</SAHL:SAHLLabel>
                                <SAHL:SAHLTextBox ID="txtIDNumberUpdate" runat="server" MaxLength="20" Width="182px"
                                    DisplayInputType="Number" TabIndex="12" CssClass="mandatory">
                                </SAHL:SAHLTextBox>
                                <SAHL:SAHLAutoComplete AutoPostBack="true" runat="server" ID="acIDNumberUpdate" TargetControlID="txtIDNumberUpdate"
                                    ServiceMethod="SAHL.Web.AJAX.LegalEntity.GetLegalEntitiesByIDNumber" MinCharacters="6">
                                </SAHL:SAHLAutoComplete>
                            </td>
                            <td></td>
                            <td class="TitleText">Introduction Date&nbsp;</td>
                            <td>
                                <SAHL:SAHLLabel ID="lblIntroductionDate" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td class="TitleText">Salutation</td>
                            <td class="cellDisplay">
                                <SAHL:SAHLLabel ID="lblSalutation" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel><SAHL:SAHLDropDownList
                                    ID="ddlSalutationUpdate" runat="server" Width="186px" PleaseSelectItem="False"
                                    TabIndex="15">
                                </SAHL:SAHLDropDownList>
                            </td>
                            <td></td>
                            <td id="tdCitizenshipType" runat="server" class="TitleText">Citizenship Type</td>
                            <td>
                                <SAHL:SAHLLabel ID="lblCitizenshipType" runat="server">-</SAHL:SAHLLabel><SAHL:SAHLDropDownList
                                    ID="ddlCitizenshipTypeUpdate" runat="server" Width="230px" PleaseSelectItem="False"
                                    TabIndex="60">
                                </SAHL:SAHLDropDownList></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td class="TitleText">Initials</td>
                            <td class="cellDisplay">
                                <SAHL:SAHLLabel ID="lblInitials" runat="server" CssClass="LabelText" TextAlign="left">-</SAHL:SAHLLabel><SAHL:SAHLTextBox
                                    ID="txtInitialsUpdate" runat="server" MaxLength="5" Width="182px" TabIndex="20"></SAHL:SAHLTextBox>
                            </td>
                            <td></td>
                            <td id="tdDateOfBirth" runat="server" class="TitleText">Date of Birth</td>
                            <td>
                                <SAHL:SAHLLabel ID="lblDateofBirth" runat="server">-</SAHL:SAHLLabel><SAHL:SAHLDateBox
                                    ID="txtUpdDateOfBirth" runat="server" Width="225px" TabIndex="70" /></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td class="TitleText">First Names</td>
                            <td class="cellDisplay">
                                <SAHL:SAHLLabel ID="lblFirstNames" runat="server" CssClass="LabelText" TextAlign="left">-</SAHL:SAHLLabel><SAHL:SAHLTextBox
                                    ID="txtFirstNamesUpdate" runat="server" MaxLength="50" Width="182px" TabIndex="25"
                                    CssClass="mandatory"></SAHL:SAHLTextBox>
                            </td>
                            <td></td>
                            <td id="tdPassportNumber" runat="server" class="TitleText">Passport Number
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="lblPassportNumber" runat="server">-</SAHL:SAHLLabel>
                                <SAHL:SAHLTextBox ID="txtUpdPassportNumber" runat="server" MaxLength="20" Width="225px"
                                    TabIndex="75">
                                </SAHL:SAHLTextBox>
                                <SAHL:SAHLAutoComplete AutoPostBack="true" runat="server" ID="acPassportNumberUpdate"
                                    TargetControlID="txtUpdPassportNumber" ServiceMethod="SAHL.Web.AJAX.LegalEntity.GetLegalEntitiesByPassportNumber"
                                    MinCharacters="3">
                                </SAHL:SAHLAutoComplete>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td class="TitleText">Surname</td>
                            <td class="cellDisplay">
                                <SAHL:SAHLLabel ID="lblSurname" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel><SAHL:SAHLTextBox
                                    ID="txtSurnameUpdate" runat="server" MaxLength="50" Width="182px" TabIndex="30"
                                    CssClass="mandatory"></SAHL:SAHLTextBox>
                            </td>
                            <td></td>
                            <td id="tdTaxNumber" runat="server" class="TitleText">Tax Number</td>
                            <td>
                                <SAHL:SAHLLabel ID="lblTaxNumber" runat="server">-</SAHL:SAHLLabel><SAHL:SAHLTextBox
                                    ID="txtUpdTaxNumber" runat="server" MaxLength="20" Width="225px" TabIndex="80"></SAHL:SAHLTextBox></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td class="TitleText">Preferred Name</td>
                            <td class="cellDisplay">
                                <SAHL:SAHLLabel ID="lblPreferredName" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel><SAHL:SAHLTextBox
                                    ID="txtUpdPreferredName" runat="server" MaxLength="50" Width="182px" TabIndex="35"></SAHL:SAHLTextBox>
                            </td>
                            <td></td>
                            <td class="TitleText">Home Language</td>
                            <td>
                                <SAHL:SAHLLabel ID="lblHomeLanguage" runat="server">-</SAHL:SAHLLabel><SAHL:SAHLDropDownList
                                    ID="ddlUpdHomeLanguage" runat="server" Width="230px" PleaseSelectItem="False"
                                    TabIndex="85">
                                </SAHL:SAHLDropDownList></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td id="tdGender" runat="server" class="TitleText">Gender</td>
                            <td class="cellDisplay">
                                <SAHL:SAHLLabel ID="lblGender" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel><SAHL:SAHLDropDownList
                                    ID="ddlGenderUpdate" runat="server" Width="186px" PleaseSelectItem="False" TabIndex="40">
                                </SAHL:SAHLDropDownList>
                            </td>
                            <td></td>
                            <td class="TitleText">Document Language</td>
                            <td>
                                <SAHL:SAHLLabel ID="lblDocumentLanguage" runat="server">-</SAHL:SAHLLabel><SAHL:SAHLDropDownList
                                    ID="ddlUpdDocumentLanguage" runat="server" Width="230px" PleaseSelectItem="False"
                                    TabIndex="90">
                                </SAHL:SAHLDropDownList></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td id="tdMaritalStatus" runat="server" class="TitleText">Marital Status</td>
                            <td class="cellDisplay">
                                <SAHL:SAHLLabel ID="lblMaritalStatus" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel><SAHL:SAHLDropDownList
                                    ID="ddlMaritalStatusUpdate" runat="server" Width="186px" PleaseSelectItem="False"
                                    TabIndex="45">
                                </SAHL:SAHLDropDownList>
                            </td>
                            <td></td>
                            <td id="tdStatus" runat="server" class="TitleText">Status
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="lblStatus" runat="server">-</SAHL:SAHLLabel><SAHL:SAHLDropDownList
                                    ID="ddlUpdStatus" runat="server" Width="230px" PleaseSelectItem="False" TabIndex="95">
                                </SAHL:SAHLDropDownList></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td id="tdPopulationGroup" runat="server" class="TitleText">Population Group</td>
                            <td class="cellDisplay">
                                <SAHL:SAHLLabel ID="lblPopulationGroup" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel><SAHL:SAHLDropDownList
                                    ID="ddlUpdPopulationGroup" runat="server" Width="186px" PleaseSelectItem="False"
                                    TabIndex="50">
                                </SAHL:SAHLDropDownList>
                            </td>
                            <td></td>
                            <td id="UpdateInsurableInterestLabel" runat="server" class="TitleText">Insurable Interest
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="lblInsurableInterest" runat="server">-</SAHL:SAHLLabel><SAHL:SAHLDropDownList
                                    ID="ddlNatUpdInsurableInterest" runat="server" Width="186px" PleaseSelectItem="False"
                                    TabIndex="100">
                                </SAHL:SAHLDropDownList>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td class="TitleText">Education</td>
                            <td class="cellDisplay">
                                <SAHL:SAHLLabel ID="lblEducation" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel><SAHL:SAHLDropDownList
                                    ID="ddlEducation" runat="server" Width="186px" PleaseSelectItem="False" TabIndex="55">
                                </SAHL:SAHLDropDownList>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlCompanyDisplay" runat="server" Width="100%">
                    <table class="tableStandard" width="100%">
                        <tr>
                            <td class="TitleText" style="width: 15%">Type</td>
                            <td style="width: 20%" class="cellDisplay">
                                <SAHL:SAHLLabel ID="lblCOLegalEntityType" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel><SAHL:SAHLDropDownList
                                    ID="COLegalEntityTypeUpdate" runat="server" Width="186px" PleaseSelectItem="true"
                                    AutoPostBack="True" Visible="false" TabIndex="5">
                                </SAHL:SAHLDropDownList>&nbsp;
                            </td>
                            <td style="width: 5%"></td>
                            <td style="width: 15%"></td>
                            <td style="width: 20%"></td>
                            <td style="width: 15%"></td>
                        </tr>
                        <tr>
                            <td id="tdCoRoleType" runat="server" class="TitleText">Role
                            </td>
                            <td class="cellDisplay">
                                <SAHL:SAHLLabel ID="lblCoRoleTypeDisplay" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel><SAHL:SAHLDropDownList
                                    ID="ddlCoRoleTypeUpdate" runat="server" PleaseSelectItem="False" Width="186px"
                                    TabIndex="10" SecurityTag="LegalEntityRoleTypeChange" SecurityDisplayType="Disable">
                                </SAHL:SAHLDropDownList>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr id="trCoUpdIncomeContributor" runat="server">
                            <td class="TitleText">Income Contributor</td>
                            <td class="cellDisplay">
                                <SAHL:SAHLLabel ID="lblCoIncomeContributor" runat="server">-</SAHL:SAHLLabel><asp:CheckBox
                                    ID="chkCoUpdIncomeContributor" runat="server" TabIndex="15" />
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td class="TitleText">Introduction Date</td>
                            <td class="cellDisplay">
                                <SAHL:SAHLLabel ID="lblCOIntroductionDate" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel><SAHL:SAHLDateBox
                                    ID="COIntroductionDateUpdate" runat="server" Visible="false" Width="182px" TabIndex="20" /></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td class="TitleText">Status</td>
                            <td class="cellDisplay">
                                <SAHL:SAHLLabel ID="lblCOStatus" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel><SAHL:SAHLDropDownList
                                    ID="ddlCOUpdStatus" runat="server" Width="186px" PleaseSelectItem="False" TabIndex="25">
                                </SAHL:SAHLDropDownList>&nbsp;
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td class="TitleText">Registered Name</td>
                            <td class="cellDisplay">
                                <SAHL:SAHLLabel ID="lblCORegisteredName" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel><SAHL:SAHLTextBox
                                    ID="CORegisteredNameUpdate" runat="server" MaxLength="100" Width="99%" Visible="false"
                                    TabIndex="30" CssClass="mandatory"></SAHL:SAHLTextBox>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td class="TitleText">Trading Name</td>
                            <td class="cellDisplay">
                                <SAHL:SAHLLabel ID="lblCOTradingName" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel><SAHL:SAHLTextBox
                                    ID="txtCOUpdTradingName" runat="server" MaxLength="50" Width="99%" TabIndex="35"></SAHL:SAHLTextBox>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td id="RegNumberLabelUpdate" runat="server" class="TitleText">Registration Number</td>
                            <td class="cellDisplay">
                                <SAHL:SAHLLabel ID="lblCORegistrationNumber" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel><SAHL:SAHLTextBox
                                    ID="CORegistrationNumberUpdate" runat="server" MaxLength="20" Width="99%" TabIndex="40"
                                    CssClass="mandatory"></SAHL:SAHLTextBox>
                                <SAHL:SAHLAutoComplete AutoPostBack="true" runat="server" ID="acCORegistrationNumberUpdate"
                                    TargetControlID="CORegistrationNumberUpdate" ServiceMethod="SAHL.Web.AJAX.LegalEntity.GetLegalEntitiesByRegistrationNumber">
                                    <ParentControls>
                                        <SAHL:SAHLAutoCompleteParentControl ControlID="COLegalEntityTypeUpdate" />
                                    </ParentControls>
                                </SAHL:SAHLAutoComplete>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td class="TitleText">Tax Number</td>
                            <td class="cellDisplay">
                                <SAHL:SAHLLabel ID="lblCOTaxNumber" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel><SAHL:SAHLTextBox
                                    ID="txtCOUpdTaxNumber" runat="server" MaxLength="20" Width="99%" TabIndex="45"></SAHL:SAHLTextBox>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td class="TitleText">Document Language</td>
                            <td class="cellDisplay">
                                <SAHL:SAHLLabel ID="lblCODocumentLanguage" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel><SAHL:SAHLDropDownList
                                    ID="ddlCOUpdDocumentLanguage" runat="server" Width="99%" PleaseSelectItem="False"
                                    TabIndex="50">
                                </SAHL:SAHLDropDownList>&nbsp;
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlAdd" runat="server" Width="100%">
                    <table id="tblLegalEntityType" class="tableStandard" width="100%">
                        <tr>
                            <td class="TitleText" style="width: 15%">Type
                            </td>
                            <td style="width: 20%">
                                <SAHL:SAHLDropDownList ID="ddlLegalEntityTypes" runat="server" Width="186px" PleaseSelectItem="False"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlLegalEntityTypes_SelectedIndexChanged"
                                    TabIndex="5">
                                </SAHL:SAHLDropDownList>
                            </td>
                            <td style="width: 5%"></td>
                            <td style="width: 15%"></td>
                            <td style="width: 20%"></td>
                            <td style="width: 15%"></td>
                        </tr>
                        <tr>
                            <td class="TitleText" visible="false" id="tdRole" runat="server">Role
                            </td>
                            <td>
                                <SAHL:SAHLDropDownList ID="ddlRoleTypeAdd" runat="server" Width="186px" PleaseSelectItem="False"
                                    TabIndex="10">
                                </SAHL:SAHLDropDownList>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr id="trAddIncomeContributor" runat="server">
                            <td class="TitleText">Income Contributor</td>
                            <td>
                                <asp:CheckBox ID="chkAddIncomeContributor" runat="server" TabIndex="15" />
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                    <asp:Panel ID="pnlNaturalPersonAdd" runat="server" Width="100%">
                        <table class="tableStandard" width="100%">
                            <tr>
                                <td class="TitleText" style="width: 15%">ID Number</td>
                                <td style="width: 20%">
                                    <SAHL:SAHLTextBox ID="txtNatAddIDNumber" runat="server" MaxLength="20" Width="182px"
                                        DisplayInputType="Number" TabIndex="17" CssClass="mandatory">
                                    </SAHL:SAHLTextBox>
                                    <SAHL:SAHLAutoComplete AutoPostBack="true" runat="server" ID="acNatAddIDNumber" TargetControlID="txtNatAddIDNumber"
                                        ServiceMethod="SAHL.Web.AJAX.LegalEntity.GetLegalEntitiesByIDNumber" MinCharacters="6">
                                    </SAHL:SAHLAutoComplete>
                                </td>
                                <td style="width: 5%"></td>
                                <td class="TitleText" style="width: 15%">Introduction Date
                                </td>
                                <td style="width: 20%">
                                    <SAHL:SAHLLabel ID="lblNatIntroductionDate" runat="server"></SAHL:SAHLLabel></td>
                                <td style="width: 15%"></td>
                            </tr>
                            <tr>
                                <td class="TitleText">Salutation</td>
                                <td>
                                    <SAHL:SAHLDropDownList ID="ddlNatAddSalutation" runat="server" Width="186px" PleaseSelectItem="False"
                                        TabIndex="20">
                                    </SAHL:SAHLDropDownList>
                                </td>
                                <td></td>
                                <td class="TitleText">Citizenship Type</td>
                                <td>
                                    <SAHL:SAHLDropDownList ID="ddlNatAddCitizenshipType" runat="server" Width="230px"
                                        PleaseSelectItem="False" TabIndex="65">
                                    </SAHL:SAHLDropDownList></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="TitleText">Initials</td>
                                <td>
                                    <SAHL:SAHLTextBox ID="txtNatAddInitials" runat="server" MaxLength="5" Width="182px"
                                        TabIndex="25"></SAHL:SAHLTextBox>
                                </td>
                                <td></td>
                                <td class="TitleText">Date of Birth</td>
                                <td>
                                    <SAHL:SAHLDateBox ID="txtNatAddDateOfBirth" runat="server" Width="225px" TabIndex="75" /><SAHL:SAHLTextBox
                                        ID="valNatAddDateOfBirthControl" runat="server" Style="display: none;"></SAHL:SAHLTextBox></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="TitleText">First Names</td>
                                <td>
                                    <SAHL:SAHLTextBox ID="txtNatAddFirstNames" runat="server" MaxLength="50" Width="182px"
                                        TabIndex="30" CssClass="mandatory"></SAHL:SAHLTextBox>
                                </td>
                                <td></td>
                                <td class="TitleText">Passport Number</td>
                                <td>
                                    <SAHL:SAHLTextBox ID="txtNatAddPassportNumber" runat="server" MaxLength="20" Width="225px"
                                        TabIndex="80">
                                    </SAHL:SAHLTextBox>
                                    <SAHL:SAHLAutoComplete AutoPostBack="true" runat="server" ID="acNatAddPassportNumber"
                                        TargetControlID="txtNatAddPassportNumber" ServiceMethod="SAHL.Web.AJAX.LegalEntity.GetLegalEntitiesByPassportNumber"
                                        MinCharacters="3">
                                    </SAHL:SAHLAutoComplete>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="TitleText">Surname</td>
                                <td>
                                    <SAHL:SAHLTextBox ID="txtNatAddSurname" runat="server" MaxLength="50" Width="182px"
                                        TabIndex="35" CssClass="mandatory"></SAHL:SAHLTextBox>
                                </td>
                                <td></td>
                                <td class="TitleText">Tax Number
                                </td>
                                <td>
                                    <SAHL:SAHLTextBox ID="txtNatAddTaxNumber" runat="server" MaxLength="20" Width="225px"
                                        TabIndex="85"></SAHL:SAHLTextBox></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="TitleText">Preferred Name</td>
                                <td>
                                    <SAHL:SAHLTextBox ID="txtNatAddPreferredName" runat="server" MaxLength="50" Width="182px"
                                        TabIndex="40"></SAHL:SAHLTextBox>
                                </td>
                                <td></td>
                                <td class="TitleText">Home Language</td>
                                <td>
                                    <SAHL:SAHLDropDownList ID="ddlNatAddHomeLanguage" runat="server" Width="230px" PleaseSelectItem="False"
                                        TabIndex="90">
                                    </SAHL:SAHLDropDownList></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="TitleText">Gender</td>
                                <td>
                                    <SAHL:SAHLDropDownList ID="ddlNatAddGender" runat="server" Width="186px" PleaseSelectItem="False"
                                        TabIndex="45">
                                    </SAHL:SAHLDropDownList>
                                </td>
                                <td></td>
                                <td class="TitleText">Document Language</td>
                                <td>
                                    <SAHL:SAHLDropDownList ID="ddlNatAddDocumentLanguage" runat="server" Width="230px"
                                        PleaseSelectItem="False" TabIndex="95">
                                    </SAHL:SAHLDropDownList></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="TitleText">Marital Status</td>
                                <td>
                                    <SAHL:SAHLDropDownList ID="ddlNatAddMaritalStatus" runat="server" Width="186px" PleaseSelectItem="False"
                                        TabIndex="50">
                                    </SAHL:SAHLDropDownList></td>
                                <td></td>
                                <td class="TitleText">Status</td>
                                <td>
                                    <SAHL:SAHLDropDownList ID="ddlNatAddStatus" runat="server" Width="230px" PleaseSelectItem="False"
                                        TabIndex="100">
                                    </SAHL:SAHLDropDownList></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="TitleText">Population Group</td>
                                <td>
                                    <SAHL:SAHLDropDownList ID="ddlNatAddpopulationGroup" runat="server" Width="186px"
                                        PleaseSelectItem="False" TabIndex="55">
                                    </SAHL:SAHLDropDownList></td>
                                <td></td>
                                <td id="InsurableInterestLabel" runat="server" class="TitleText">Insurable Interest
                                </td>
                                <td>
                                    <SAHL:SAHLDropDownList ID="ddlNatAddInsurableInterest" runat="server" Width="186px"
                                        PleaseSelectItem="False" TabIndex="105">
                                    </SAHL:SAHLDropDownList></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="TitleText">Education</td>
                                <td>
                                    <SAHL:SAHLDropDownList ID="ddlNatAddEducation" runat="server" Width="186px" PleaseSelectItem="False"
                                        TabIndex="60">
                                    </SAHL:SAHLDropDownList></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlCompanyAdd" runat="server" Width="100%">
                        <table width="100%" class="tableStandard">
                            <tr>
                                <td class="TitleText" style="width: 15%">Introduction Date</td>
                                <td style="width: 20%;" class="cellDisplay">
                                    <SAHL:SAHLLabel ID="lblCOAddIntroductionDate" runat="server" CssClass="LabelText"></SAHL:SAHLLabel>
                                </td>
                                <td style="width: 5%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 20%"></td>
                                <td style="width: 15%"></td>
                            </tr>
                            <tr>
                                <td class="TitleText">Company Name</td>
                                <td>
                                    <SAHL:SAHLTextBox ID="txtCOAddCompanyName" runat="server" MaxLength="100" Width="99%"
                                        TabIndex="20" CssClass="mandatory"></SAHL:SAHLTextBox>
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="TitleText">Trading Name</td>
                                <td>
                                    <SAHL:SAHLTextBox ID="txtCOAddTradingName" runat="server" MaxLength="50" Width="99%"
                                        TabIndex="25"></SAHL:SAHLTextBox>
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td id="RegNumberLabel" runat="server" class="TitleText">Registration Number</td>
                                <td>
                                    <SAHL:SAHLTextBox ID="txtCOAddRegistrationNumber" runat="server" MaxLength="25" Width="99%"
                                        TabIndex="30" CssClass="mandatory"></SAHL:SAHLTextBox>
                                    <SAHL:SAHLAutoComplete AutoPostBack="true" runat="server" ID="acCOAddRegistrationNumber"
                                        TargetControlID="txtCOAddRegistrationNumber" ServiceMethod="SAHL.Web.AJAX.LegalEntity.GetLegalEntitiesByRegistrationNumber">
                                        <ParentControls>
                                            <SAHL:SAHLAutoCompleteParentControl ControlID="ddlLegalEntityTypes" />
                                        </ParentControls>
                                    </SAHL:SAHLAutoComplete>
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="TitleText">Tax Number</td>
                                <td>
                                    <SAHL:SAHLTextBox ID="txtCOAddTaxNumber" runat="server" MaxLength="20" Width="99%"
                                        TabIndex="35"></SAHL:SAHLTextBox>
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="TitleText">Document Language</td>
                                <td>
                                    <SAHL:SAHLDropDownList ID="ddlCOAddDocumentlanguage" runat="server" Width="99%" PleaseSelectItem="False"
                                        TabIndex="40">
                                    </SAHL:SAHLDropDownList>
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="TitleText">Status</td>
                                <td>
                                    <SAHL:SAHLDropDownList ID="ddlCOAddStatus" runat="server" Width="99%" PleaseSelectItem="False"
                                        TabIndex="45">
                                    </SAHL:SAHLDropDownList>
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <table class="tableStandard" width="100%">
        <tr id="ContactDetails" runat="server" align="left">
            <td style="width: 35%">
                <asp:Panel ID="Panel5" runat="server" CssClass="TitleText" GroupingText="Contact Details"
                    Width="100%">
                    <table class="tableStandard" width="100%">
                        <tr id="trHomePhone" runat="server">
                            <td class="TitleText" style="width: 15%">Home Phone</td>
                            <td style="width: 20%">
                                <SAHL:SAHLLabel ID="lblHomePhone" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel><SAHL:SAHLTextBox
                                    ID="txtHomePhoneCode" runat="server" DisplayInputType="Number" MaxLength="10"
                                    TabIndex="110" Width="42px"></SAHL:SAHLTextBox><SAHL:SAHLTextBox ID="txtHomePhoneNumber"
                                        runat="server" DisplayInputType="Number" MaxLength="15" TabIndex="111" Width="136px"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr>
                            <td class="TitleText">Work Phone</td>
                            <td>
                                <SAHL:SAHLLabel ID="lblWorkPhone" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel><SAHL:SAHLTextBox
                                    ID="txtWorkPhoneCode" runat="server" DisplayInputType="Number" MaxLength="10"
                                    TabIndex="112" Width="42px"></SAHL:SAHLTextBox><SAHL:SAHLTextBox ID="txtWorkPhoneNumber"
                                        runat="server" DisplayInputType="Number" MaxLength="15" TabIndex="113" Width="136px"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr>
                            <td class="TitleText">Fax Number</td>
                            <td>
                                <SAHL:SAHLLabel ID="lblFaxNumber" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel><SAHL:SAHLTextBox
                                    ID="txtFaxCode" runat="server" DisplayInputType="Number" MaxLength="10" TabIndex="114"
                                    Width="42px"></SAHL:SAHLTextBox><SAHL:SAHLTextBox ID="txtFaxNumber" runat="server"
                                        DisplayInputType="Number" MaxLength="15" TabIndex="115" Width="136px"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr>
                            <td class="TitleText">Cellphone No</td>
                            <td>
                                <SAHL:SAHLLabel ID="lblCellphoneNumber" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel><SAHL:SAHLTextBox
                                    ID="txtCellPhoneNumber" runat="server" DisplayInputType="Number" MaxLength="15"
                                    TabIndex="116" Width="182px"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr>
                            <td class="TitleText">Email Address</td>
                            <td>
                                <SAHL:SAHLLabel ID="lblEmailAddress" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel><SAHL:SAHLTextBox
                                    ID="txtEmailAddress" runat="server" DisplayInputType="AlphaNumNoSpace" MaxLength="50"
                                    TabIndex="117" Width="182px"></SAHL:SAHLTextBox></td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
            <td style="width: 5%"></td>
            <td style="width: 35%">
                <asp:Panel ID="MarketingOptionPanel" runat="server" CssClass="TitleText" GroupingText="Marketing Options Excluded"
                    Width="100%">
                    <SAHL:SAHLCheckboxList ID="MarketingOptionExcludedListBox" runat="server" Style="width: 100%;"
                        CellPadding="1" CellSpacing="1" RepeatColumns="1" RepeatDirection="Vertical"
                        RepeatLayout="Flow">
                    </SAHL:SAHLCheckboxList>
                </asp:Panel>
            </td>
            <td style="width: 15%"></td>
        </tr>
    </table>
    <table class="tableStandard" width="100%">
        <tr id="ButtonRow" runat="server" align="right">
            <td style="width: 15%"></td>
            <td style="width: 20%"></td>
            <td style="width: 5%"></td>
            <td style="width: 15%">&nbsp;
            </td>
            <td style="width: 20%" align="right">
                <SAHL:SAHLButton ID="btnSubmitButton" runat="server" AccessKey="U" Text="Update"
                    OnClick="btnSubmitButton_Click" />
                <SAHL:SAHLButton ID="btnCancelButton" runat="server" AccessKey="C" CausesValidation="False"
                    Text="Cancel" OnClick="btnCancelButton_Click" />
            </td>
            <td align="right" style="width: 15%"></td>
        </tr>
    </table>
</asp:Content>