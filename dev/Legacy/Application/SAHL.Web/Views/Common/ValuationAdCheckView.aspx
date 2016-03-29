<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="ValuationAdCheckView.aspx.cs" Inherits="SAHL.Web.Views.Common.ValuationAdCheckView"
    Title="Lightstone Automated Valuation" EnableViewState="False" %>

<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
<h3 style="text-align:center"> ADCheck Valuation Details </h3>
    <table width="100%">
        <tr>
            <td style="text-align: left">
                <ajaxToolkit:TabContainer runat="server" ID="Tabs" Height="415px" ActiveTabIndex="0"
                    OnActiveTabChanged="Tabs_ActiveTabChanged">
                    <ajaxToolkit:TabPanel runat="server" ID="Panel1" HeaderText="Signature and Bio">
                        <ContentTemplate>
                            &nbsp;<asp:Panel ID="pnlInspectionContactDetails" runat="server" Width="100%" GroupingText="Assessment Details">
                                <table style="width: 100%">
                                    <tbody>
                                        <tr>
                                            <td style="width: 25%">
                                                <SAHL:SAHLLabel ID="_lblAssessmentNumber" runat="server" Width="100%" Text="Assessment Number"
                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                            </td>
                                            <td style="width: 25%">
                                                <SAHL:SAHLLabel ID="lblAssessmentNumberValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                            </td>
                                            <td style="width: 25%">
                                                <SAHL:SAHLLabel ID="_lblRequestNumber" runat="server" Width="100%" Text="Request Number"
                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                            </td>
                                            <td style="width: 25%">
                                                <SAHL:SAHLLabel ID="lblRequestNumberValue" runat="server" Width="25%" TextAlign="Left"></SAHL:SAHLLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%;">
                                                <SAHL:SAHLLabel ID="_lblAssessmentReason" runat="server" Width="100%" Text="Assessment Reason"
                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                            </td>
                                            <td style="width: 25%;">
                                                <SAHL:SAHLLabel ID="lblAssessmentReasonValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                            </td>
                                            <td style="width: 25%;">
                                                <SAHL:SAHLLabel ID="_lblRequestedby" runat="server" Width="100%" Text="Requested by"
                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                            </td>
                                            <td style="width: 25%;">
                                                <SAHL:SAHLLabel ID="lblRequestedbyValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 25%">
                                                <SAHL:SAHLLabel ID="_lblCorrectProperty" runat="server" Width="100%" Text="Correct Property"
                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                            </td>
                                            <td style="width: 25%">
                                                <SAHL:SAHLLabel ID="lblCorrectPropertyValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                            </td>
                                            <td style="width: 25%">
                                                <SAHL:SAHLLabel ID="_lblCorrectAddress" runat="server" Width="100%" Text="Correct Address"
                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                            </td>
                                            <td style="width: 25%">
                                                <SAHL:SAHLLabel ID="lblCorrectAddressValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </asp:Panel>
                            <table style="width: 100%">
                                <tbody>
                                    <tr>
                                        <td style="width: 50%" valign="top">
                                            <asp:Panel ID="pnlValuation" runat="server" Width="100%" GroupingText="Valuation Amount">
                                                <table style="width: 100%">
                                                    <tbody>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblValueAsIs" runat="server" Width="100%" Text="Value As Is"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblValueAsIsValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%;">
                                                                <SAHL:SAHLLabel ID="_lblCostToComplete" runat="server" Width="100%" Text="Cost to Complete"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%;">
                                                                <SAHL:SAHLLabel ID="lblCostToCompleteValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblValueOnCompletion" runat="server" Width="100%" Text="Value on Completion"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblValueOnCompletionValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel ID="pnlMainBuildingSummary" runat="server" Width="100%" GroupingText="Main Building Summary">
                                                <table style="width: 100%">
                                                    <tbody>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblClassifcation" runat="server" Width="100%" Text="Classification"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblClassificationValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblRoofType" runat="server" Width="100%" Text="Roof Type" Font-Bold="False"
                                                                    TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblRoofTypeValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblMainBuildingExtent" runat="server" Width="100%" Text="Extent (sq meters)"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblMainBuildingExtentValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblMainbuildingRate" runat="server" Width="100%" Text="Rate (R / sq Meter)"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblMainbuildingRateValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblMainBuilding" runat="server" Width="100%" Text="Value" Font-Bold="False"
                                                                    TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblMainBuildingValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel ID="pnlCottageSummary" runat="server" Width="100%" GroupingText="Cottage Summary">
                                                <table style="width: 100%">
                                                    <tbody>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblCottageExtent" runat="server" Width="100%" Text="Extent (sq meters)"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblCottageExtentValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblCottageRate" runat="server" Width="100%" Text="Rate (R / sq meter)"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblCottageRateValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblCottageReplacement" runat="server" Width="100%" Text="Replacement Value"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblCottageReplacementValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                        <td style="width: 50%" valign="top">
                                            <asp:Panel ID="pnlOutbuilding" runat="server" Width="100%" GroupingText="Outbuilding Summary">
                                                <table style="width: 100%">
                                                    <tbody>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblOutbuildingExtent" runat="server" Width="100%" Text="Extent (sq meters)"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblOutbuildingExtentValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblOutbuildingRate" runat="server" Width="100%" Text="Rate (R / sq meter)"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblOutbuildingRateValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblOutbuildingReplacement" runat="server" Width="100%" Text="Replacement Value"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblOutbuildingReplacementValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel ID="pnlthatchSummary" runat="server" Width="100%" GroupingText="Thatch Summary">
                                                <table style="width: 100%">
                                                    <tbody>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_labelthatch" runat="server" Width="100%" Text="Value" Font-Bold="False"
                                                                    TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblThatchValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblThatchValueCheck" runat="server" Width="100%" Text="Thatch Value % Check"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblThatchValueCheckValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblThatchExtentCheck" runat="server" Width="100%" Text="Thatch Extent Check"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblThatchExtentCheckValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblThatchMinimumDistance" runat="server" Width="100%" Text="Minimum Distance (Meters)"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblThatchMinimumDistanceValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel ID="Panel10" runat="server" Width="100%" GroupingText="Improvement Summary">
                                                <table style="width: 100%">
                                                    <tbody>
                                                        <tr>
                                                            <td style="width: 25%;">
                                                                <SAHL:SAHLLabel ID="_lblImprovementSummary" runat="server" Width="100%" Text="Improvement Comment"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%;">
                                                                <SAHL:SAHLLabel ID="lblImprovementSummaryValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblImprovement" runat="server" Width="100%" Text="Value" Font-Bold="False"
                                                                    TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblImprovementValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <br />
                            <asp:Panel ID="Panel4" runat="server" Width="100%" GroupingText="Comment">
                                <SAHL:SAHLTextBox ID="txtComment" runat="server" Width="100%" TextAlign="Left" TextMode="MultiLine"
                                    Rows="4" ReadOnly="True"></SAHL:SAHLTextBox>
                            </asp:Panel>
                            <br />
                        
</ContentTemplate>
                        <HeaderTemplate>
                            Valuation
                        
</HeaderTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel runat="server" ID="Panel3" HeaderText="Email">
                        <ContentTemplate>
                            &nbsp;<table style="width: 100%">
                                <tbody>
                                    <tr>
                                        <td style="width: 50%" valign="top">
                                            <asp:Panel ID="Panel5" runat="server" Width="100%" GroupingText="Address">
                                                <table style="width: 100%">
                                                    <tbody>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_Building" runat="server" Width="100%" Text="Building" Font-Bold="False"
                                                                    TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%;">
                                                                <SAHL:SAHLLabel ID="_lblStreetAddress" runat="server" Width="100%" Text="Street"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%;">
                                                                <SAHL:SAHLLabel ID="lblStreetAddressValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblSuburb" runat="server" Width="100%" Text="Suburb" Font-Bold="False"
                                                                    TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblSuburbValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblCity" runat="server" Width="100%" Text="City" Font-Bold="False"
                                                                    TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblCityValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblProvince" runat="server" Width="100%" Text="Province" Font-Bold="False"
                                                                    TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblProvinceValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblCountry" runat="server" Width="100%" Text="Country" Font-Bold="False"
                                                                    TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblCountryValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblPostalCode" runat="server" Width="100%" Text="Postal Code"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblPostalCodeValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel ID="pnlValuationAddressCheck" runat="server" Width="100%" GroupingText="Valuation Address Check">
                                                <table style="width: 100%">
                                                    <tbody>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblStandNumber" runat="server" Width="100%" Text="Stand Number"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblStandNumberValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%;">
                                                                <SAHL:SAHLLabel ID="_lblStreetNumber" runat="server" Width="100%" Text="Street Number"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%;">
                                                                <SAHL:SAHLLabel ID="lblStreetNumberValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblStreetName" runat="server" Width="100%" Text="Street Name"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblStreetNameValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel ID="Panel8" runat="server" Width="100%" GroupingText="Area">
                                                <table style="width: 100%">
                                                    <tbody>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblAreaType" runat="server" Width="100%" Text="Area Type" Font-Bold="False"
                                                                    TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblAreaTypeValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblSectorType" runat="server" Width="100%" Text="Sector Type"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblSectorTypeValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblLocalityComment" runat="server" Width="100%" Text="Locality Comment"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblLocalityCommentValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblMarketComment" runat="server" Width="100%" Text="Market Comment"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblMarketCommentValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel ID="Panel11" runat="server" Width="100%" GroupingText="Retention">
                                                <table style="width: 100%">
                                                    <tbody>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblRetentionReason" runat="server" Width="100%" Text="Reason"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblRetentionReasonValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblRetentionAmount" runat="server" Width="100%" Text="Amount"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblRetentionAmountValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                        <td style="width: 50%" valign="top">
                                            <asp:Panel ID="Panel6" runat="server" Width="100%" GroupingText="Property">
                                                <table style="width: 100%">
                                                    <tbody>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblERFType" runat="server" Width="100%" Text="ERF Type" Font-Bold="False"
                                                                    TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblERFTypeValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblERFNumber" runat="server" Width="100%" Text="ERF Number"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblERFNumberValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblPortion" runat="server" Width="100%" Text="Portion" Font-Bold="False"
                                                                    TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblPortionValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblPortionOfPortion" runat="server" Width="100%" Text="Portion of Portion"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblPortionOfPortionValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblSubdivisionPortion" runat="server" Width="100%" Text="Rem. Subdivision Portion"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblSubdivisionPortionValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblLandSize" runat="server" Width="100%" Text="Land Size" Font-Bold="False"
                                                                    TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblLandSizeValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblSectionNumber" runat="server" Width="100%" Text="Section Number"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblSectionNumberValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_txtLegalDescription" runat="server" Width="100%" Text="Legal Description"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%" colspan="2" rowspan="1">
                                                                <SAHL:SAHLTextBox ID="txtLegalDescriptionValue" runat="server" Width="100%" TextAlign="Left"
                                                                    TextMode="MultiLine" Rows="4" ReadOnly="True"></SAHL:SAHLTextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblPropertyUse" runat="server" Width="100%" Text="Property Use"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblPropertyUseValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblPropertyType" runat="server" Width="100%" Text="Property Type"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblPropertyTypeValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblPropertyComment" runat="server" Width="100%" Text="Property Comment"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblPropertyCommentValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel ID="Panel9" runat="server" Width="100%" GroupingText="Complex / Flat">
                                                <table style="width: 100%">
                                                    <tbody>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                &nbsp;<SAHL:SAHLLabel ID="_lblUnitsInComplex" runat="server" Width="100%" Text="No. of Units in Complex"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                                            <td style="width: 25%">
                                                                &nbsp;<SAHL:SAHLLabel ID="lblUnitsInComplexValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblFlatNumber" runat="server" Width="100%" Text="Flat Number"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblFlatNumberValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblUnitSize" runat="server" Width="100%" Text="Unit Size" Font-Bold="False"
                                                                    TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblUnitSizeValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblComplexName" runat="server" Width="100%" Text="Complex Name"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblComplexNameValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="_lblFloorsInComplex" runat="server" Width="100%" Text="Floors in Complex"
                                                                    Font-Bold="False" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                            </td>
                                                            <td style="width: 25%">
                                                                <SAHL:SAHLLabel ID="lblFloorsInComplexValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                &nbsp;</td>
                                                            <td style="width: 25%">
                                                                &nbsp;</td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </asp:Panel>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        
</ContentTemplate>
                        <HeaderTemplate>
                            Property
                        
</HeaderTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel runat="server" ID="Panel2" HeaderText="Controls">
                        <ContentTemplate>
<div><asp:Panel ID="pnlGridConditions" runat="server" Width="100%" GroupingText="Conditions"><SAHL:SAHLGridView ID="grdConditions" runat="server" Width="100%" GridWidth="100%"
                                        TotalsFooter="True" Invisible="False" AutoGenerateColumns="False" EnableViewState="False"
                                        GridHeight="25%" EmptyDataSetMessage="There are no Conditions for this valuation." SelectFirstRow="True"
                                        FixedHeader="False" EmptyDataText="There are no Conditions for this valuation." NullDataSetMessage="There are no Conditions for this valuation.">
<RowStyle CssClass="TableRowA"></RowStyle>
</SAHL:SAHLGridView> </asp:Panel> &nbsp;&nbsp;<asp:Panel ID="Panel12" runat="server" Width="100%" GroupingText="Condition Comments"><SAHL:SAHLGridView ID="grdConditionComments" runat="server" Width="100%" GridWidth="100%"
                                        TotalsFooter="True" Invisible="False" AutoGenerateColumns="False" EnableViewState="False"
                                        GridHeight="25%" EmptyDataSetMessage="Not available yet." SelectFirstRow="True"
                                        FixedHeader="False" EmptyDataText="Not available yet." NullDataSetMessage="Not available yet.">
<RowStyle CssClass="TableRowA"></RowStyle>
</SAHL:SAHLGridView> </asp:Panel> </div>
</ContentTemplate>
                        <HeaderTemplate>
Conditions&nbsp; 
</HeaderTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel runat="server" ID="FeaturesTabPanel" HeaderText="Features">
                        <ContentTemplate>
                            <table border="0" style="width: 100%">
                                <tr>
                                    <td style="width: 50%" valign="top">
                                        <asp:Panel ID="Panel7" runat="server" GroupingText="Main Building" Width="100%">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblBathRoomCount" runat="server" Font-Bold="False" Text="Bathroom Count"
                                                            Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                    </td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblBathRoomCountValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%;">
                                                        <SAHL:SAHLLabel ID="_lblStudyCount" runat="server" Font-Bold="False" Text="Study Count"
                                                            Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                    </td>
                                                    <td style="width: 25%;">
                                                        <SAHL:SAHLLabel ID="lblStudyCountValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblFamilyRoomCount" runat="server" Font-Bold="False" Text="Family Room Count"
                                                            Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                    </td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblFamilyRoomCountValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblEntranceHallCount" runat="server" Font-Bold="False" Text="Entrance  Hall Count"
                                                            Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblEntranceHallCountValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblLaundryRoomCount" runat="server" Font-Bold="False" Text="Laundry Room count"
                                                            Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblLaundryRoomCountValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblDiningCount" runat="server" Font-Bold="False" Text="Dining Count"
                                                            Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblDiningCountValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblLoungeCount" runat="server" Font-Bold="False" Text="Lounge Count"
                                                            Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblLoungeCountValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblMainBuildingBedroomCount" runat="server" Font-Bold="False"
                                                            Text="Bedroom Count" Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblMainBuildingBedroomCountValue" runat="server" Width="100%"
                                                            TextAlign="Left"></SAHL:SAHLLabel></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblMainBuildingKitchenCount" runat="server" Font-Bold="False"
                                                            Text="Kitchen Count" Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblMainBuildingKitchenCountValue" runat="server" Width="100%"
                                                            TextAlign="Left"></SAHL:SAHLLabel></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblPantryCount" runat="server" Font-Bold="False" Text="Pantry Count"
                                                            Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblPantryCountValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblMainBuildingWCCount" runat="server" Font-Bold="False" Text="WC Count"
                                                            Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblMainBuildingWCCountValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblOtherRoomCount" runat="server" Font-Bold="False" Text="Other Room Count"
                                                            Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblOtherRoomCountValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblMainBuildingThatchRate" runat="server" Font-Bold="False" Text="Thatch Rate"
                                                            Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblMainBuildingThatchRateValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblMainBuildingThatchExtent" runat="server" Font-Bold="False"
                                                            Text="Thatch Extent" Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblMainBuildingThatchExtentValue" runat="server" Width="100%"
                                                            TextAlign="Left"></SAHL:SAHLLabel></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblMainBuildingConventionalRate" runat="server" Font-Bold="False"
                                                            Text="Conventional Rate" Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblMainBuildingConventionalRateValue" runat="server" Width="100%"
                                                            TextAlign="Left"></SAHL:SAHLLabel></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblMainBuildingConventionalExtent" runat="server" Font-Bold="False"
                                                            Text="Conventional Extent" Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblMainBuildingConventionalExtentValue" runat="server" Width="100%"
                                                            TextAlign="Left"></SAHL:SAHLLabel></td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlSwimmingPool" runat="server" GroupingText="Swimming Pool" Width="100%">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblSwimmingPool" runat="server" Font-Bold="False" Text="Value"
                                                            Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                    </td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblSwimmingPoolValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%;">
                                                        <SAHL:SAHLLabel ID="_lblSwimmingPoolType" runat="server" Font-Bold="False" Text="Type"
                                                            Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                    </td>
                                                    <td style="width: 25%;">
                                                        <SAHL:SAHLLabel ID="lblSwimmingPoolTypeValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel ID="Panel17" runat="server" GroupingText="Tennis Court" Width="100%">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblTennisCourt" runat="server" Font-Bold="False" Text="Value"
                                                            Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                    </td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblTennisCourtValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%;">
                                                        <SAHL:SAHLLabel ID="_lblTennisCourtType" runat="server" Font-Bold="False" Text="Type"
                                                            Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                    </td>
                                                    <td style="width: 25%;">
                                                        <SAHL:SAHLLabel ID="lblTennisCourtTypeValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                    <td style="width: 50%" valign="top">
                                        <asp:Panel ID="Panel13" runat="server" GroupingText="Outbuilding" Width="100%">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblOutbuildingBathroomCount" runat="server" Font-Bold="False"
                                                            Text="Bathroom Count" Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                    </td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblOutbuildingBathroomCountValue" runat="server" Width="100%"
                                                            TextAlign="Left"></SAHL:SAHLLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%;">
                                                        <SAHL:SAHLLabel ID="_lblOutbuildingWorkshopCount" runat="server" Font-Bold="False"
                                                            Text="Workshop Count" Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                    </td>
                                                    <td style="width: 25%;">
                                                        <SAHL:SAHLLabel ID="lblOutbuildingWorkshopCountValue" runat="server" Width="100%"
                                                            TextAlign="Left"></SAHL:SAHLLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblOutbuildingLaundryCount" runat="server" Font-Bold="False"
                                                            Text="Laundry Count" Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                    </td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblOutbuildingLaundryCountValue" runat="server" Width="100%"
                                                            TextAlign="Left"></SAHL:SAHLLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblOutbuildingGarageCount" runat="server" Font-Bold="False" Text="Garage Count"
                                                            Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblOutbuildingGarageCountValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblOutbuildingBedroomCount" runat="server" Font-Bold="False"
                                                            Text="Bedroom Count" Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblOutbuildingBedroomCountValue" runat="server" Width="100%"
                                                            TextAlign="Left"></SAHL:SAHLLabel></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblOutbuildingStoreRoomCount" runat="server" Font-Bold="False"
                                                            Text="Store Room Count" Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblOutbuildingStoreRoomcountValue" runat="server" Width="100%"
                                                            TextAlign="Left"></SAHL:SAHLLabel></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblOutbuildingKitchenCount" runat="server" Font-Bold="False"
                                                            Text="Kitchen Count" Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblOutbuildingKitchenCountValue" runat="server" Width="100%"
                                                            TextAlign="Left"></SAHL:SAHLLabel></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblOutbuildingWCCount" runat="server" Font-Bold="False" Text="WC Count"
                                                            Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblOutbuildingWCCountValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblOutbuildingThatchRate" runat="server" Font-Bold="False" Text="Thatch Rate"
                                                            Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblOutbuildingThatchRateValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblOutbuildingThatchExtent" runat="server" Font-Bold="False"
                                                            Text="Thatch Extent" Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblOutbuildingThatchExtentValue" runat="server" Width="100%"
                                                            TextAlign="Left"></SAHL:SAHLLabel></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblOutbuildingConventionalRate" runat="server" Font-Bold="False"
                                                            Text="Conventional Rate" Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblOutbuildingConventionalRateValue" runat="server" Width="100%"
                                                            TextAlign="Left"></SAHL:SAHLLabel></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblOutbuildingConventionalExtent" runat="server" Font-Bold="False"
                                                            Text="Conventional Extent" Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblOutbuildingConventionalExtentValue" runat="server" Width="100%"
                                                            TextAlign="Left"></SAHL:SAHLLabel></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        &nbsp;</td>
                                                    <td style="width: 25%">
                                                        &nbsp;</td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel ID="Panel14" runat="server" GroupingText="Cottage" Width="100%">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblCottageCottageStandNumber" runat="server" Font-Bold="False"
                                                            Text="Bathroom Count" Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                    </td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblSCottageCottageStandNumberValue" runat="server" Width="100%"
                                                            TextAlign="Left"></SAHL:SAHLLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%;">
                                                        <SAHL:SAHLLabel ID="_lblCottageBedroomCount" runat="server" Font-Bold="False" Text="Bedroom Count"
                                                            Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                    </td>
                                                    <td style="width: 25%;">
                                                        <SAHL:SAHLLabel ID="lblBedroomCountValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblCottageKitchenCount" runat="server" Font-Bold="False" Text="Kitchen Count"
                                                            Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                                    </td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblCottageKitchenCountValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblCottageLivingRoomCount" runat="server" Font-Bold="False" Text="Living Room Count"
                                                            Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblCottageLivingRoomCountValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblCottageOtherRoomCount" runat="server" Font-Bold="False" Text="Other Room Count"
                                                            Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblCottageOtherRoomCountValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblCottageThatchRate" runat="server" Font-Bold="False" Text="Thatch Rate"
                                                            Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblCottageThatchRateValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblCottageThatchExtent" runat="server" Font-Bold="False" Text="Thatch Extent"
                                                            Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblCottageThatchExtentValue" runat="server" Width="100%" TextAlign="Left"></SAHL:SAHLLabel></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblCottageConventionalRate" runat="server" Font-Bold="False"
                                                            Text="Conventional Rate" Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblCottageConventionalRateValue" runat="server" Width="100%"
                                                            TextAlign="Left"></SAHL:SAHLLabel></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="_lblCottageConventionalExtent" runat="server" Font-Bold="False"
                                                            Text="Conventional Extent" Width="100%" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                                    <td style="width: 25%">
                                                        <SAHL:SAHLLabel ID="lblCottageConventionalExtentValue" runat="server" Width="100%"
                                                            TextAlign="Left"></SAHL:SAHLLabel></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%">
                                                        &nbsp;</td>
                                                    <td style="width: 25%">
                                                        &nbsp;</td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                    <td style="width: 100%" valign="top">
                                        &nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="width: 100%">
                                        &nbsp;</td>
                                </tr>
                            </table>
                        
</ContentTemplate>
                        <HeaderTemplate>
                            Features&nbsp;
                        
</HeaderTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel runat="server" ID="ComparativeTabPanel" HeaderText="Comparative">
                        <HeaderTemplate>
                            Comparative
                        
</HeaderTemplate>
                        <ContentTemplate>
<div style="text-align: left"></div><asp:Panel ID="pnlMaps" runat="server" Width="100%" GroupingText="Comparable Properties"><SAHL:SAHLGridView ID="grdComparativeProperties" runat="server" Width="100%" GridWidth="100%"
                                        TotalsFooter="True" Invisible="False" AutoGenerateColumns="False" EnableViewState="False"
                                        GridHeight="25%" EmptyDataSetMessage="There are no Comparative Properties to display." SelectFirstRow="True"
                                        FixedHeader="False" EmptyDataText="There are no Comparative Properties to display." NullDataSetMessage="There are no Comparative Properties to display.">
<RowStyle CssClass="TableRowA" />
</SAHL:SAHLGridView> </asp:Panel> 
</ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="TabPanel1">
                        <ContentTemplate>
                            <asp:Panel ID="Panel15" runat="server" GroupingText="Improvement Results" Width="100%">
                                <SAHL:SAHLGridView ID="grdImprovementResults" runat="server" AutoGenerateColumns="False"
                                    EmptyDataSetMessage="There are no improvements to this property." EnableViewState="False"
                                    FixedHeader="False" GridHeight="25%" GridWidth="100%" Invisible="False" SelectFirstRow="True"
                                    TotalsFooter="True" Width="100%"  EmptyDataText="There are no improvements to this property." NullDataSetMessage="There are no improvements to this property.">
                                    <RowStyle CssClass="TableRowA" />
                                </SAHL:SAHLGridView>
                            </asp:Panel>
                        
</ContentTemplate>
                        <HeaderTemplate>
                            Improvements
                        
</HeaderTemplate>
                    </ajaxToolkit:TabPanel>
                </ajaxToolkit:TabContainer>
                <SAHL:SAHLButton ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back"
                    Width="153px" /></td>
        </tr>
    </table>
</asp:Content>
