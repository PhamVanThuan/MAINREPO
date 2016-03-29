<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="PropertyDetails.aspx.cs" Inherits="SAHL.Web.Views.Common.PropertyDetails"
    Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <div style="text-align: center">
        <table width="100%" class="tableStandard">
            <tr>
                <td align="left">
                    <cc1:AddressGrid ID="PropertyAddressGrid" runat="server" PostBackType="SingleClick"
                        OnSelectedIndexChanged="PropertyAddressGrid_SelectedIndexChanged">
                    </cc1:AddressGrid>
                    <br />
                    <table border="0">
                        <tr>
                            <td style="width: 167px;" class="TitleText">
                                Property Type
                            </td>
                            <td style="width: 211px;">
                                <SAHL:SAHLLabel ID="PropertyType" runat="server">-</SAHL:SAHLLabel><SAHL:SAHLDropDownList
                                    ID="ddlPropertyType" runat="server" PleaseSelectItem="true" Style="width: 90%;"
                                    Mandatory="True">
                                </SAHL:SAHLDropDownList>&nbsp;
                            </td>
                            <td style="width: 176px;" class="TitleText">
                                Erf Number
                            </td>
                            <td style="width: 201px;">
                                <SAHL:SAHLLabel ID="ErfNumber" runat="server">-</SAHL:SAHLLabel>
                                <SAHL:SAHLTextBox ID="txtErfNumber" runat="server" Style="width: 90%;" MaxLength="25"></SAHL:SAHLTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="TitleText" style="width: 167px">
                                Title Type
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="TitleType" runat="server">-</SAHL:SAHLLabel><SAHL:SAHLDropDownList
                                    ID="ddlTitleType" runat="server" PleaseSelectItem="true" Style="width: 90%;"
                                    Mandatory="True">
                                </SAHL:SAHLDropDownList>&nbsp;
                            </td>
                            <td class="TitleText">
                                Portion Number
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="PortionNumber" runat="server">-</SAHL:SAHLLabel>
                                <SAHL:SAHLTextBox ID="txtPortionNumber" runat="server" Style="width: 90%;" MaxLength="25"></SAHL:SAHLTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="TitleText" style="width: 167px">
                                Occupancy Type
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="OccupancyType" runat="server">-</SAHL:SAHLLabel><SAHL:SAHLDropDownList
                                    ID="ddlOccupancyType" runat="server" PleaseSelectItem="true" Style="width: 90%;"
                                    Mandatory="True">
                                </SAHL:SAHLDropDownList>&nbsp;
                            </td>
                            <td class="TitleText">
                                Erf Suburb
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="ErfSuburb" runat="server">-</SAHL:SAHLLabel>
                                <SAHL:SAHLTextBox ID="txtErfSuburb" runat="server" Style="width: 90%;" MaxLength="50"></SAHL:SAHLTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="TitleText" style="width: 167px">
                                Area Classification
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="AreaClassification" runat="server">-</SAHL:SAHLLabel><SAHL:SAHLDropDownList
                                    ID="ddlAreaClassification" runat="server" PleaseSelectItem="true" Style="width: 90%;"
                                    Mandatory="True">
                                </SAHL:SAHLDropDownList>&nbsp;
                            </td>
                            <td class="TitleText">
                                Erf Metro Description
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="ErfMetroDescription" runat="server">-</SAHL:SAHLLabel>
                                <SAHL:SAHLTextBox ID="txtErfMetroDescription" runat="server" Style="width: 90%;"
                                    MaxLength="50"></SAHL:SAHLTextBox>
                            </td>
                        </tr>
                        <tr id="trDisplayPropertyDesc1" runat="server">
                            <td class="TitleText" style="width: 167px">
                                Property Description
                            </td>
                            <td rowspan="2">
                                <SAHL:SAHLTextBox ID="PropertyDescription" runat="server" Style="width: 185px;" TextMode="MultiLine"
                                    Rows="3" Font-Names="Verdana" Font-Size="8pt" ReadOnly="True"></SAHL:SAHLTextBox>
                                &nbsp;</td>
                            <td class="TitleText" style="vertical-align:middle;">
                                Sectional Scheme Name</td>
                            <td style="vertical-align:middle;">
                                <SAHL:SAHLLabel ID="SectionalSchemeName" runat="server" >-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr id="trDisplayPropertyDesc2" runat="server">
                            <td class="TitleText" style="width: 167px">
                            </td>
                            <td class="TitleText">
                                Sectional Unit Number
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="SectionalUnitNumber" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr id="trUpdatePropertyDesc1" runat="server">
                            <td class="TitleText" style="width: 167px">
                                Property Description
                            </td>
                            <td>
                                <SAHL:SAHLTextBox ID="txtPropertyDescription1" runat="server" Style="width: 185px;"
                                    Mandatory="True" MaxLength="250" />
                            </td>
                            <td class="TitleText">
                                Sectional Scheme Name
                            </td>
                            <td>
                                <SAHL:SAHLTextBox ID="txtSectionalSchemeName" runat="server" Style="width: 90%;"
                                    MaxLength="50" />
                            </td>
                        </tr>
                        <tr id="trUpdatePropertyDesc2" runat="server">
                            <td class="TitleText" style="width: 167px">
                            </td>
                            <td>
                                <SAHL:SAHLTextBox ID="txtPropertyDescription2" runat="server" Style="width: 185px;"
                                    MaxLength="250" Mandatory="True" />
                            </td>
                            <td class="TitleText">
                                Sectional Unit Number
                            </td>
                            <td>
                                <SAHL:SAHLTextBox ID="txtSectionalUnitNumber" runat="server" Style="width: 90%;"
                                    MaxLength="25" />
                            </td>
                        </tr>
                        <tr id="trUpdatePropertyDesc3" runat="server">
                            <td class="TitleText" style="width: 167px">
                            </td>
                            <td>
                                <SAHL:SAHLTextBox ID="txtPropertyDescription3" runat="server" Style="width: 185px;"
                                    MaxLength="250" Mandatory="True" />
                            </td>
                            <td class="TitleText">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td class="TitleText" rowspan="4" style="width: 167px">
                                <br />
                                Title Deed Number/s
                            </td>
                            <td rowspan="4">
                                <asp:Panel ID="TitleDeedNumberPanel" runat="server" Style="width: 90%; height: 50px;
                                    border: 1px solid #E5E5E5;" ScrollBars="vertical">
                                    <SAHL:SAHLLabel ID="TitleDeedNumber" runat="server">-</SAHL:SAHLLabel>
                                </asp:Panel>
                                <SAHL:SAHLTextBox ID="txtTitleDeedNumber" runat="server" Style="width: 90%;" TextMode="MultiLine"
                                    Rows="5" Mandatory="True" Font-Names="Verdana" Font-Size="8pt"></SAHL:SAHLTextBox>
                             </td>
                            <td class="TitleText">
                                Inspection Contact</td>
                            <td>
                                <SAHL:SAHLLabel ID="InspectionContact" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel><SAHL:SAHLTextBox
                                    ID="txtInspectionContact" runat="server" MaxLength="25" Style="width: 90%" Mandatory="True"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr>
                            <td class="TitleText">
                                Inspection Telephone</td>
                            <td>
                                <SAHL:SAHLLabel ID="InspectionNumber" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                <SAHL:SAHLTextBox ID="txtInspectionNumber" runat="server" MaxLength="25" Style="width: 90%"
                                    Mandatory="True"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr>
                            <td class="TitleText">
                                Inspection Contact 2</td>
                            <td>
                                <SAHL:SAHLLabel ID="InspectionContact2" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                <SAHL:SAHLTextBox ID="txtInspectionContact2" runat="server" MaxLength="25" Style="width: 90%"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr>
                            <td class="TitleText">
                                Inspection Telephone 2</td>
                            <td>
                                <SAHL:SAHLLabel ID="InspectionNumber2" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                <SAHL:SAHLTextBox ID="txtInspectionNumber2" runat="server" MaxLength="25" Style="width: 90%"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr>
                            <td class="TitleText" style="width: 167px">
                                Deeds Property Type
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="DeedsPropertyType" runat="server">-</SAHL:SAHLLabel>
                                <SAHL:SAHLDropDownList ID="ddlDeedsPropertyType" runat="server" PleaseSelectItem="true"
                                    Style="width: 90%;" Mandatory="True">
                                </SAHL:SAHLDropDownList></td>
                            <td class="TitleText">Current Data Provider
                            </td>
                            <td><SAHL:SAHLLabel ID="lbCurrentDataProvider" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr>
                            <td class="TitleText" style="width: 167px">
                                Bond Account Number</td>
                            <td>
                                <SAHL:SAHLLabel ID="BondAccountNumber" runat="server">-</SAHL:SAHLLabel>
                                <SAHL:SAHLTextBox ID="txtBondAccountNumber" runat="server" MaxLength="50" Style="width: 90%"></SAHL:SAHLTextBox></td>
                            <td class="TitleText">
                                LightStone Property ID</td>
                            <td>
                                <SAHL:SAHLLabel ID="LightStonePropertyID" runat="server">-</SAHL:SAHLLabel></td>
                        </tr>
                        <tr>
                            <td class="TitleText" style="width: 167px">
                                Deeds Office</td>
                            <td>
                                <SAHL:SAHLLabel ID="DeedsOffice" runat="server">-</SAHL:SAHLLabel>
                                <SAHL:SAHLDropDownList ID="ddlDeedsOffice" runat="server" PleaseSelectItem="true"
                                    Style="width: 90%;">
                                </SAHL:SAHLDropDownList></td>
                            <td class="TitleText">
                                AdCheck Property ID</td>
                            <td>
                                <SAHL:SAHLLabel ID="AdCheckPropertyID" runat="server">-</SAHL:SAHLLabel></td>
                        </tr>
                    </table>
                    <br />
                    <ajaxToolkit:Accordion ID="accTransfers" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
                        HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="" FadeTransitions="false"
                        FramesPerSecond="40" TransitionDuration="250" AutoSize="None" SuppressHeaderPostbacks="true">
                        <Panes>
                            <ajaxToolkit:AccordionPane ID="apTransfers" runat="server">
                                <Header>
                                    <a href="">Deeds Transfers</a>
                                </Header>
                                <Content>
                                    <asp:Panel ID="pnlDeedsTransfersGrid" runat="server">
                                    </asp:Panel>
                                </Content>
                            </ajaxToolkit:AccordionPane>
                            <ajaxToolkit:AccordionPane ID="apRegistrations" runat="server">
                                <Header>
                                    <a href="">Bond Registrations</a>
                                </Header>
                                <Content>
                                    <asp:Panel ID="pnlBondRegistrationGrid" runat="server">
                                    </asp:Panel>
                                </Content>
                            </ajaxToolkit:AccordionPane>
                        </Panes>
                    </ajaxToolkit:Accordion>
                    <SAHL:SAHLGridView ID="PropertyOwnersGrid" runat="server" AutoGenerateColumns="false"
                        FixedHeader="False" EnableViewState="false" GridHeight="110px" GridWidth="99.8%"
                        Width="150%" HeaderCaption="" PostBackType="None" NullDataSetMessage="" EmptyDataSetMessage="There are no Property Owner Details."
                        OnRowDataBound="PropertyOwnersGrid_RowDataBound" ScrollX="True" SelectFirstRow="False">
                        <RowStyle CssClass="TableRowA" />
                    </SAHL:SAHLGridView>
                    <SAHL:SAHLGridView ID="BondRegistrationsGrid" runat="server" AutoGenerateColumns="false"
                        FixedHeader="False" EnableViewState="false" GridHeight="110px" GridWidth="99.8%"
                        Width="100%" HeaderCaption="" PostBackType="None" NullDataSetMessage="" EmptyDataSetMessage="There are no Bond Details."
                        OnRowDataBound="BondRegistrationsGrid_RowDataBound" SelectFirstRow="False">
                        <RowStyle CssClass="TableRowA" />
                    </SAHL:SAHLGridView>
                </td>
            </tr>
            <tr id="ButtonRow" runat="server">
                <td align="right">
                    <SAHL:SAHLButton ID="btnUpdate" runat="server" AccessKey="A" Text="Update" OnClick="btnUpdate_Click"
                        ButtonSize="Size4" />
                    <SAHL:SAHLButton ID="btnCancel" runat="server" AccessKey="A" Text="Cancel" OnClick="btnCancel_Click"
                        ButtonSize="Size4" CausesValidation="false" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
