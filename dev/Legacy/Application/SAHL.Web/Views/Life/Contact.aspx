<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Life.Contact" Title="Contact" Codebehind="Contact.aspx.cs" %>


<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>

<%@ Register Src="WorkFlowHeader.ascx" TagName="WorkFlowHeader" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <uc1:WorkFlowHeader ID="WorkFlowHeader1" runat="server" />
    <div style="text-align: center">
        <table width="100%" class="tableStandard">
            <tr>
                <td>
                    <asp:Panel ID="pnlAssuredLivesGrid" runat="server" Width="100%">
                        <table id="tAssuredLivesGrid" width="100%">
                            <tr>
                                <td align="left" style="text-align: left">
                                    <cc1:LegalEntityGrid ID="LegalEntityGrid" runat="server" OnSelectedIndexChanged="LegalEntityGrid_SelectedIndexChanged" ColumnIDPassportVisible="True" GridHeight="">
                                    </cc1:LegalEntityGrid>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <SAHL:SAHLLabel ID="SAHLLabel1" runat="server"></SAHL:SAHLLabel>
                    <asp:Panel ID="pnlAssuredLivesDetails" runat="server" Width="100%" GroupingText="Personal Details">
                        <table id="tAssuredLivesDetail" class="tableStandard" border="0" style="text-align: left" width="100%">
                            <tr>
                                <td class="TitleText" style="width: 126px">
                                    Salutation</td>
                                <td style="width: 200px">
                                    <SAHL:SAHLLabel ID="lblSalutation" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                                <td style="width: 30px">
                                </td>
                                <td class="TitleText" style="width: 144px">
                                    Home Phone</td>
                                <td style="width: 210px">
                                    <SAHL:SAHLLabel ID="lblHomePhone" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td class="TitleText" style="width: 126px">
                                    <SAHL:SAHLLabel ID="lblFirstNamesText" runat="server" CssClass="TitleText" Font-Bold="True" >First Names</SAHL:SAHLLabel></td>
                                <td style="width: 200px">
                                    <SAHL:SAHLLabel ID="lblFirstNames" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                                <td style="width: 30px">
                                </td>
                                <td class="TitleText" style="width: 144px">
                                    Work Phone</td>
                                <td style="width: 150px">
                                    <SAHL:SAHLLabel ID="lblWorkPhone" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td class="TitleText" style="width: 126px">
                                    <SAHL:SAHLLabel ID="lblSurnameText" runat="server" CssClass="TitleText" Font-Bold="True">Surname</SAHL:SAHLLabel></td>
                                <td style="width: 200px">
                                    <SAHL:SAHLLabel ID="lblSurname" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                                <td class="TitleText" style="width: 30px">
                                </td>
                                <td class="TitleText" style="width: 144px">
                                    Fax Number</td>
                                <td style="width: 150px">
                                    <SAHL:SAHLLabel ID="lblFaxNumber" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>                            <tr>
                                <td class="TitleText" style="width: 126px">
                                    Preferred Name
                                </td>
                                <td style="width: 200px">
                                    <SAHL:SAHLLabel ID="lblPreferredName" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                                <td style="width: 30px">
                                </td>
                                <td class="TitleText" style="width: 144px">
                                    Mobile Phone
                                </td>
                                <td style="width: 150px">
                                    <SAHL:SAHLLabel ID="lblCellphone" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                </td>
                            </tr>
                            <tr>
                                <td class="TitleText" style="width: 126px">
                                    Gender
                                </td>
                                <td style="width: 200px">
                                    <SAHL:SAHLLabel ID="lblGender" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                                <td style="width: 30px">
                                </td>
                                <td class="TitleText" style="width: 144px">
                                    Email Address</td>
                                <td style="width: 150px">
                                    <SAHL:SAHLLabel ID="lblEmailAddress" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                            <tr>
                                <td class="TitleText" style="width: 126px">
                                    <SAHL:SAHLLabel ID="lblIDText" runat="server" CssClass="TitleText" Font-Bold="True">ID/Passport No</SAHL:SAHLLabel></td>
                                <td style="width: 200px">
                                    <SAHL:SAHLLabel ID="lblIDNumber" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                                <td style="width: 30px">
                                </td>
                                <td class="TitleText" style="width: 144px">
                                </td>
                                <td style="width: 150px">
                                </td>
                            </tr>
                            <tr>
                                <td class="TitleText" style="width: 126px">
                                    Birth Date</td>
                                <td style="width: 200px">
                                    <SAHL:SAHLLabel ID="lblBirthDate" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                                <td style="width: 30px">
                                </td>
                                <td class="TitleText" style="width: 144px">
                                    Age next Birthday</td>
                                <td style="width: 150px">
                                    <SAHL:SAHLLabel ID="lblAgeNextBirthday" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <br />
                    <asp:Panel ID="pnlPlanButtons" runat="server" Width="100%">
                        <table id="tPlanButtons" width="100%">
                            <tr>
                                <td align="left" colspan="2">
                                    <cc1:AddressGrid ID="AddressGrid" runat="server" PostBackType="NoneWithClientSelect">
                                    </cc1:AddressGrid>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <SAHL:SAHLButton ID="btnUpdateDetails" runat="server" Text="Update Contact Details"
                                        OnClick="btnUpdateDetails_Click" ButtonSize="Size6" SecurityTag="LifeUpdateAccessWorkflow"/>
                                    <SAHL:SAHLButton ID="btnAddAddress" runat="server" Text="Add Address" OnClick="btnAddAddress_Click"
                                        ButtonSize="Size4" SecurityTag="LifeUpdateAccessWorkflow"/>
                                    <SAHL:SAHLButton ID="btnUpdateAddress" runat="server" Text="Update Address" OnClick="btnUpdateAddress_Click"
                                        ButtonSize="Size5" SecurityTag="LifeUpdateAccessWorkflow"/>
                                    <SAHL:SAHLButton ID="btnNext" runat="server" Text="Next" OnClick="btnNext_Click" SecurityTag="LifeUpdateAccessWorkflow"/>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
