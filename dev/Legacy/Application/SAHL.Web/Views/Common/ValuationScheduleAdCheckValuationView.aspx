<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true" CodeBehind="ValuationScheduleAdCheckValuationView.aspx.cs" Inherits="SAHL.Web.Views.Common.ValuationScheduleAdCheckValuationView" Title="Schedule an AdCheck Valuation." EnableViewState="False" %>
<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

    &nbsp;
                <asp:Panel ID="pnlValuationAssignmentDetails" runat="server" GroupingText="Valuation Assignment Details"
                    Width="100%">
                    &nbsp;<table border="0" style="width: 100%">
                        <tr>
                            <td style="width: 25%">
                                <SAHL:SAHLLabel ID="lblAssessmentNumber" runat="server" Font-Bold="True" Text="Assessment Number"
                                    Width="139%" TextAlign="Left"></SAHL:SAHLLabel></td>
                            <td style="width: 25%">
                                <SAHL:SAHLLabel ID="lblAssessmentNumberValue" runat="server" Width="100%"></SAHL:SAHLLabel></td>
                            <td style="width: 25%">
                                <SAHL:SAHLLabel ID="lblRequestNumber" runat="server" Font-Bold="True" Text="Request Number"
                                    Width="133%"></SAHL:SAHLLabel></td>
                            <td style="width: 25%">
                                <SAHL:SAHLLabel ID="lblRequestNumberValue" runat="server" Width="100%"></SAHL:SAHLLabel></td>
                        </tr>
                        <tr>
                            <td style="width: 25%">
                                <SAHL:SAHLLabel ID="lblApplicationName" runat="server" Font-Bold="True" Text="Application Name" Width="93%" TextAlign="Left"></SAHL:SAHLLabel></td>
                            <td style="width: 25%">
                                <SAHL:SAHLLabel ID="lblApplicationNameValue" runat="server" Width="100%"></SAHL:SAHLLabel></td>
                            <td style="width: 25%">
                                <SAHL:SAHLLabel ID="lblRequestedBy" runat="server" Font-Bold="True" Text="Requested By" Width="93%"></SAHL:SAHLLabel></td>
                            <td style="width: 25%">
                                <SAHL:SAHLLabel ID="lblRequestedByValue" runat="server" Width="100%"></SAHL:SAHLLabel></td>
                        </tr>
                    </table>
                </asp:Panel>
    <asp:Panel ID="pnlPropertyDetails" runat="server" GroupingText="Property Details"
                    Width="100%">
        &nbsp;<table border="0" style="width: 100%">
            <tr>
                <td style="width: 25%">
                    <SAHL:SAHLLabel ID="lblBuilding" runat="server" Font-Bold="True" Text="Building"></SAHL:SAHLLabel></td>
                <td style="width: 25%">
                    <SAHL:SAHLLabel ID="lblBuildingValue" runat="server"></SAHL:SAHLLabel></td>
                <td style="width: 25%">
                    <SAHL:SAHLLabel ID="lblTitleType" runat="server" Font-Bold="True" Text="Title Type"></SAHL:SAHLLabel></td>
                <td style="width: 25%">
                    <SAHL:SAHLLabel ID="lblTitleTypeValue" runat="server"></SAHL:SAHLLabel></td>
            </tr>
            <tr>
                <td style="width: 25%; ">
                    <SAHL:SAHLLabel ID="lblStreet" runat="server" Font-Bold="True" Text="Street"></SAHL:SAHLLabel></td>
                <td style="width: 25%; ">
                    <SAHL:SAHLLabel ID="lblStreetValue" runat="server"></SAHL:SAHLLabel></td>
                <td style="width: 25%; ">
                    <SAHL:SAHLLabel ID="lblOccupancyType" runat="server" Font-Bold="True" Text="Occupancy Type"
                        Width="140px"></SAHL:SAHLLabel></td>
                <td style="width: 25%; ">
                    <SAHL:SAHLLabel ID="lblOccupancyTypeValue" runat="server"></SAHL:SAHLLabel></td>
            </tr>
            <tr>
                <td style="width: 25%">
                    <SAHL:SAHLLabel ID="lblSuburb" runat="server" Font-Bold="True" Text="Suburb"></SAHL:SAHLLabel></td>
                <td style="width: 25%">
                    <SAHL:SAHLLabel ID="lblSuburbValue" runat="server"></SAHL:SAHLLabel></td>
                <td style="width: 25%">
                    <SAHL:SAHLLabel ID="lblAreaClassification" runat="server" Font-Bold="True" Text="Area Classification" Width="185px"></SAHL:SAHLLabel></td>
                <td style="width: 25%">
                    <SAHL:SAHLLabel ID="lblAreaClassificationValue" runat="server"></SAHL:SAHLLabel></td>
            </tr>
            <tr>
                <td style="width: 25%">
                    <SAHL:SAHLLabel ID="lblCity" runat="server" Font-Bold="True" Text="City"></SAHL:SAHLLabel></td>
                <td style="width: 25%">
                    <SAHL:SAHLLabel ID="lblCityValue" runat="server"></SAHL:SAHLLabel></td>
                <td style="width: 25%">
                    </td>
                <td style="width: 100px; text-align: left;">
                    </td>
            </tr>
            <tr>
                <td style="width: 25%">
                    <SAHL:SAHLLabel ID="lblProvince" runat="server" Font-Bold="True" Text="Province"></SAHL:SAHLLabel></td>
                <td style="width: 25%">
                    <SAHL:SAHLLabel ID="lblProvinceValue" runat="server"></SAHL:SAHLLabel></td>
                <td style="width: 25%">
                    <SAHL:SAHLLabel ID="lblERFNumber" runat="server" Font-Bold="True" Text="ERF Number" Width="121px"></SAHL:SAHLLabel></td>
                <td style="width: 25%">
                    <SAHL:SAHLLabel ID="lblERFNumberValue" runat="server"></SAHL:SAHLLabel></td>
            </tr>
            <tr>
                <td style="width: 25%; ">
                    <SAHL:SAHLLabel ID="lblCountry" runat="server" Font-Bold="True" Text="Country"></SAHL:SAHLLabel></td>
                <td style="width: 25%; ">
                    <SAHL:SAHLLabel ID="lblCountryValue" runat="server"></SAHL:SAHLLabel></td>
                <td style="width: 25%; ">
                    <SAHL:SAHLLabel ID="lblPortionNumber" runat="server" Font-Bold="True" Text="Portion Number" Width="156px"></SAHL:SAHLLabel></td>
                <td style="width: 25%; ">
                    <SAHL:SAHLLabel ID="lblPortionNumberValue" runat="server"></SAHL:SAHLLabel></td>
            </tr>
            <tr>
                <td style="width: 25%; ">
                    <SAHL:SAHLLabel ID="lblPostalCode" runat="server" Font-Bold="True" Text="Postal Code"></SAHL:SAHLLabel></td>
                <td style="width: 25%; ">
                    <SAHL:SAHLLabel ID="lblPostalCodeValue" runat="server"></SAHL:SAHLLabel></td>
                <td style="width: 25%; ">
                    <SAHL:SAHLLabel ID="lblERFSuburb" runat="server" Font-Bold="True" Text="ERF Suburb"></SAHL:SAHLLabel></td>
                <td style="width: 25%; ">
                    <SAHL:SAHLLabel ID="lblERFSuburbValue" runat="server"></SAHL:SAHLLabel></td>
            </tr>
            <tr>
                <td style="width: 25%; ">
                    <SAHL:SAHLLabel ID="lblTitleDeedNumber" runat="server" Font-Bold="True" Text="Title Deed Number" Width="192px"></SAHL:SAHLLabel></td>
                <td style="width: 25%; ">
                    <SAHL:SAHLLabel ID="txtTitleDeedNumber" runat="server"></SAHL:SAHLLabel></td>
                <td style="width: 25%; ">
                    <SAHL:SAHLLabel ID="lblERFMetroDescription" runat="server" Font-Bold="True" Text="ERF Metro Description" Width="190px"></SAHL:SAHLLabel></td>
                <td style="width: 25%; ">
                    <SAHL:SAHLLabel ID="lblERFMetroDescriptionValue" runat="server"></SAHL:SAHLLabel></td>
            </tr>
            <tr>
                <td style="width: 25%; ">
                    <SAHL:SAHLLabel ID="lblDeedsPropertyType" runat="server" Font-Bold="True" Text="Deeds Property Type"
                        Width="179px"></SAHL:SAHLLabel></td>
                <td style="width: 25%; ">
                    <SAHL:SAHLLabel ID="lblDeedsPropertyTypeValue" runat="server"></SAHL:SAHLLabel></td>
                <td style="width: 25%; ">
                    <SAHL:SAHLLabel ID="lblSectionalSchemeName" runat="server" Font-Bold="True" Text="Sectional Scheme Name"
                        Width="198px"></SAHL:SAHLLabel></td>
                <td style="width: 25%; ">
                    <SAHL:SAHLLabel ID="lblSectionalSchemeNameValue" runat="server"></SAHL:SAHLLabel></td>
            </tr>
            <tr>
                <td style="width: 25%; ">
                    <SAHL:SAHLLabel ID="lblSAPTGPropertyNumber" runat="server" Font-Bold="True" Text="SAPTG Property Number"
                        Width="215px"></SAHL:SAHLLabel></td>
                <td style="width: 25%; ">
                    <SAHL:SAHLLabel ID="lblSAPTGPropertyNumberValue" runat="server"></SAHL:SAHLLabel></td>
                <td style="width: 25%; ">
                    <SAHL:SAHLLabel ID="lblSectionalUnitNumber" runat="server" Font-Bold="True" Text="Sectional Unit Number" Width="209px"></SAHL:SAHLLabel></td>
                <td style="width: 25%; ">
                    <SAHL:SAHLLabel ID="lblSectionalUnitNumberValue" runat="server"></SAHL:SAHLLabel></td>
            </tr>
            <tr>
                <td style="width: 25%">
                    <SAHL:SAHLLabel ID="lblPropertyDescription" runat="server" Font-Bold="True" Text="Property Description"
                        Width="178px"></SAHL:SAHLLabel></td>
                <td colspan="3" style="width: 25%">
                    <SAHL:SAHLTextBox ID="txtPropertyDescription" runat="server" ReadOnly="True" TextMode="MultiLine"
                        Width="80%"></SAHL:SAHLTextBox></td>
            </tr>
        </table>
    </asp:Panel>
    <table border="0" style="width: 100%">
    <tr>
    <td style="width: 50%; vertical-align: top;" valign="top">
    <asp:Panel ID="pnlInspectionContactDetails" runat="server" GroupingText="Inspection Contact Details"
        Width="100%">
        <table border="0" style="width: 100%">
            <tr>
                <td style="width: 50%; text-align: left">
                    <SAHL:SAHLLabel ID="Label1" runat="server"  Font-Bold="True" Text="Contact 1 " CssClass="LabelText" TextAlign="Left"></SAHL:SAHLLabel></td>
                <td style="width: 50%">
                    <SAHL:SAHLTextBox ID="txtContact1Name" runat="server" Width="90%" Mandatory="True"></SAHL:SAHLTextBox>
                    <SAHL:SAHLLabel ID="lblContact1Value" runat="server" Visible="False"></SAHL:SAHLLabel></td>
            </tr>
            <tr>
                <td style="width: 50%; text-align: left">
                    <SAHL:SAHLLabel ID="Label2" runat="server" Text="Phone " Font-Bold="True" CssClass="LabelText" TextAlign="Left"></SAHL:SAHLLabel></td>
                <td style="width: 50%">
                    <SAHL:SAHLTextBox ID="txtContact1Phone" runat="server" Width="90%" Mandatory="True"></SAHL:SAHLTextBox>
                    <SAHL:SAHLLabel ID="lblPhone1Value" runat="server" Visible="False"></SAHL:SAHLLabel></td>
            </tr>
            <tr>
                <td style="width: 50%; text-align: left">
                    <SAHL:SAHLLabel ID="Label6" runat="server" Text="Work Phone " Font-Bold="True" CssClass="LabelText" TextAlign="Left"></SAHL:SAHLLabel></td>
                <td style="width: 50%">
                    <SAHL:SAHLTextBox ID="txtContact1WorkPhone" runat="server" Width="90%"></SAHL:SAHLTextBox>
                    <SAHL:SAHLLabel ID="lblWorkPhone1Value" runat="server" Visible="False"></SAHL:SAHLLabel></td>
            </tr>
            <tr>
                <td style="width: 50%; text-align: left">
                    <SAHL:SAHLLabel ID="Label5" runat="server" Text="Mobile Phone " Font-Bold="True" CssClass="LabelText" TextAlign="Left"></SAHL:SAHLLabel></td>
                <td style="width: 50%">
                    <SAHL:SAHLTextBox ID="txtContact1MobilePhone" runat="server" Width="90%"></SAHL:SAHLTextBox>
                    <SAHL:SAHLLabel ID="lblCellPhone1Value" runat="server" Visible="False"></SAHL:SAHLLabel></td>
            </tr>
            <tr>
                <td style="width: 50%; text-align: left">
                    <SAHL:SAHLLabel ID="Label3" runat="server" Text="Contact 2 " Font-Bold="True"  CssClass="LabelText" TextAlign="Left"></SAHL:SAHLLabel></td>
                <td style="width: 50%">
                    <SAHL:SAHLTextBox ID="txtContact2Name" runat="server" Width="90%"></SAHL:SAHLTextBox>
                    <SAHL:SAHLLabel ID="lblContact2Value" runat="server" Visible="False"></SAHL:SAHLLabel></td>
            </tr>
            <tr>
                <td style="width: 50%; text-align: left">
                    <SAHL:SAHLLabel ID="Label4" runat="server" Text="Phone " Font-Bold="True" CssClass="LabelText" TextAlign="Left"></SAHL:SAHLLabel></td>
                <td style="width: 50%;">
                    <SAHL:SAHLTextBox ID="txtContact2Phone" runat="server" Width="90%"></SAHL:SAHLTextBox>
                    <SAHL:SAHLLabel ID="lblPhone2Value" runat="server" Visible="False"></SAHL:SAHLLabel></td>
            </tr>
        </table>
    </asp:Panel>
            </td>
            <td style="width: 50%" valign="top">
    <asp:Panel ID="pnlValuationRequestDetails" runat="server" GroupingText="Valuation Request Details"
                    Width="100%">
        <table border="0" style="width: 100%">
            <tr>
                <td style="width: 30%; text-align: left">
                    <SAHL:SAHLLabel ID="lblValuer" runat="server" Font-Bold="True" Text="Valuer " CssClass="LabelText" TextAlign="Left"></SAHL:SAHLLabel></td>
                <td style="width: 70%">
                    <SAHL:SAHLDropDownList ID="ddlValuer" runat="server" Visible="False" Width="90%" Mandatory="True">
                    </SAHL:SAHLDropDownList>
                    <SAHL:SAHLLabel ID="lblValuerValue" runat="server" Visible="False"></SAHL:SAHLLabel></td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: left">
                    <SAHL:SAHLLabel ID="lblAssesmentByDate" runat="server" Font-Bold="True" Text="Assessment by " Width="149px" CssClass="LabelText" TextAlign="Left"></SAHL:SAHLLabel></td>
                <td style="width: 70%">
                    <SAHL:SAHLDateBox ID="dtAssessmentByDateValue" runat="server" Visible="False" CultureName="en-GB" Width="60%" Mandatory="True"></SAHL:SAHLDateBox>
                    <SAHL:SAHLLabel ID="lblAssessmentByDateValue" runat="server" Visible="False"></SAHL:SAHLLabel></td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: left">
                    <SAHL:SAHLLabel ID="lblAssessmentReason" runat="server" Font-Bold="True" Text="Assessment Reason "
                        Width="169px" CssClass="LabelText" TextAlign="Left"></SAHL:SAHLLabel></td>
                <td style="width: 70%">
                    <SAHL:SAHLDropDownList ID="ddlAssessmentReasonValue" runat="server" Visible="False" Width="90%" Mandatory="True">
                    </SAHL:SAHLDropDownList>
                    <SAHL:SAHLLabel ID="lblAssessmentReasonValue" runat="server" Visible="False"></SAHL:SAHLLabel></td>
            </tr>
            <tr>
                <td style="width: 30%; text-align: left">
                    <SAHL:SAHLLabel ID="lblAssessmentPriority" runat="server" Font-Bold="True" Text="Assessment Priority "
                        Width="170px" CssClass="LabelText" TextAlign="Left"></SAHL:SAHLLabel></td>
                <td style="width: 70%">
                    
                    <SAHL:SAHLDropDownList ID="ddlAssessmentPriorityValue" runat="server" Visible="False"
                        Width="90%" Mandatory="True">
                        <asp:ListItem Value="0">-select-</asp:ListItem>
                        <asp:ListItem Value="1">Low</asp:ListItem>
                        <asp:ListItem Value="2">Medium</asp:ListItem>
                        <asp:ListItem Value="3">High</asp:ListItem>
                    </SAHL:SAHLDropDownList><SAHL:SAHLLabel ID="lblAssessmentPriorityValue" runat="server" Visible="False"></SAHL:SAHLLabel></td>
            </tr>
            <tr>
                <td style="text-align: left" valign="top">
                    <SAHL:SAHLLabel ID="lblSpecialInstructions" runat="server" Font-Bold="True" Text="Special Instructions "
                        Width="163px" CssClass="LabelText" TextAlign="Left"></SAHL:SAHLLabel></td>
                <td style="width: 70%">
                    <SAHL:SAHLTextBox ID="txtSpecialInstructions" runat="server" TextMode="MultiLine"
                        Width="90%" Visible="False" Height="50px"></SAHL:SAHLTextBox></td>
            </tr>
        </table>
    </asp:Panel>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Panel ID="pnlSelectProperty" runat="server" GroupingText="AdCheck  found the following properties. Please select the correct one from the list below."
                    Visible="False" Width="99%">
                    <SAHL:SAHLGridView ID="grdProperties" runat="server" AutoGenerateColumns="False"
                        EmptyDataSetMessage="There are Properties." EnableViewState="false" FixedHeader="false"
                        GridHeight="100px" GridWidth="100%" NullDataSetMessage="" OnSelectedIndexChanged="grdProperties_SelectedIndexChanged"
                        PostBackType="SingleAndDoubleClick" Width="99%">
                        <RowStyle CssClass="TableRowA" />
                    </SAHL:SAHLGridView>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <br />
    <table width="99%">
        <tr>
            <td style="text-align: left;">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="text-align: right"><SAHL:SAHLButton
                                    ID="btnValidate" runat="server" Text="Validate Property" Visible="False" Width="153px" OnClick="btnValidateProperty_Click" ButtonSize="Size5" />
                &nbsp;<SAHL:SAHLButton
                                    ID="btnInstruct" runat="server" Text="Instruct Valuer" Visible="False" Width="153px" OnClick="btnInstruct_Click" ButtonSize="Size5" /></td>
        </tr>
    </table>
    <br />
        <br />

</asp:Content>