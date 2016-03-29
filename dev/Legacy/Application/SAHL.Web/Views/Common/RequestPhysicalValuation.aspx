<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Common.RequestPhysicalValuation" Title="Reassign User" CodeBehind="RequestPhysicalValuation.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v10.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <div>
        <SAHL:DXGridView runat="server" ID="requestedValuationsGrid" AutoGenerateColumns="False" Width="100%" Images-CollapsedButton-Height=""
                        PostBackType="None" Settings-ShowTitlePanel="true"
                        SettingsText-Title="Valuations" SettingsText-EmptyDataRow="No Data" Settings-ShowVerticalScrollBar="true" Settings-VerticalScrollableHeight="240">
            <Columns>
                <SAHL:DXGridViewFormattedTextColumn HeaderStyle-HorizontalAlign="Center" FieldName="Valuer"
                    Format="GridString" Caption="Valuer">
                </SAHL:DXGridViewFormattedTextColumn>
                <SAHL:DXGridViewFormattedTextColumn HeaderStyle-HorizontalAlign="Center" FieldName="Valuation Date"
                    Format="GridString" Caption="Valuation Date" >
                </SAHL:DXGridViewFormattedTextColumn>
                <SAHL:DXGridViewFormattedTextColumn HeaderStyle-HorizontalAlign="Center" FieldName="Valuation Status"
                    Format="GridString" Caption="Valuation Status" >
                </SAHL:DXGridViewFormattedTextColumn>
                <SAHL:DXGridViewFormattedTextColumn HeaderStyle-HorizontalAlign="Center" FieldName="Active"
                    Format="GridString" Caption="Active">
                </SAHL:DXGridViewFormattedTextColumn>
                <SAHL:DXGridViewFormattedTextColumn HeaderStyle-HorizontalAlign="Center" FieldName="Valuation Amount"
                    Format="GridCurrency" Caption="Valuation Amount">
                </SAHL:DXGridViewFormattedTextColumn>
                <SAHL:DXGridViewFormattedTextColumn HeaderStyle-HorizontalAlign="Center" FieldName="HOC Valuation"
                    Format="GridCurrency"  Caption="HOC Valuation">
                </SAHL:DXGridViewFormattedTextColumn>
                <SAHL:DXGridViewFormattedTextColumn HeaderStyle-HorizontalAlign="Center" FieldName="Valuation Type"
                    Format="GridString" Caption="Valuation Type" >
                </SAHL:DXGridViewFormattedTextColumn>
                <SAHL:DXGridViewFormattedTextColumn HeaderStyle-HorizontalAlign="Center" FieldName="Requested By"
                    Format="GridString" Caption="Requested By">
                </SAHL:DXGridViewFormattedTextColumn>
                <SAHL:DXGridViewFormattedTextColumn HeaderStyle-HorizontalAlign="Center" FieldName="Reason"
                    Format="GridString" Caption="Reason">
                </SAHL:DXGridViewFormattedTextColumn>
                <SAHL:DXGridViewFormattedTextColumn HeaderStyle-HorizontalAlign="Center" FieldName="Comment"
                    Format="GridString" Caption="Comment">
                </SAHL:DXGridViewFormattedTextColumn>
            </Columns>
        </SAHL:DXGridView>
    </div>

    <div>
        &nbsp;
    </div>

    <table border="0" style="width: 100%">
        <tr>
            <td style="width: 50%; vertical-align: top;" valign="top">
                <asp:Panel ID="pnlInspectionContactDetails" runat="server" GroupingText="Inspection Contact Details"
                    Width="100%">
                    <table border="0" style="width: 100%">
                        <tr>
                            <td style="width: 50%; text-align: left">
                                <SAHL:SAHLLabel ID="Label1" runat="server" Font-Bold="True" Text="Contact 1 " CssClass="LabelText" TextAlign="Left"></SAHL:SAHLLabel></td>
                            <td style="width: 50%">
                                <SAHL:SAHLTextBox ID="txtContact1Name" runat="server" Width="90%" Mandatory="True"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 50%; text-align: left">
                                <SAHL:SAHLLabel ID="Label2" runat="server" Text="Phone " Font-Bold="True" CssClass="LabelText" TextAlign="Left"></SAHL:SAHLLabel></td>
                            <td style="width: 50%">
                                <SAHL:SAHLTextBox ID="txtContact1Phone" runat="server" Width="90%" Mandatory="True"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 50%; text-align: left">
                                <SAHL:SAHLLabel ID="Label6" runat="server" Text="Work Phone " Font-Bold="True" CssClass="LabelText" TextAlign="Left"></SAHL:SAHLLabel></td>
                            <td style="width: 50%">
                                <SAHL:SAHLTextBox ID="txtContact1WorkPhone" runat="server" Width="90%"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 50%; text-align: left">
                                <SAHL:SAHLLabel ID="Label5" runat="server" Text="Mobile Phone " Font-Bold="True" CssClass="LabelText" TextAlign="Left"></SAHL:SAHLLabel></td>
                            <td style="width: 50%">
                                <SAHL:SAHLTextBox ID="txtContact1MobilePhone" runat="server" Width="90%"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 50%; text-align: left">
                                <SAHL:SAHLLabel ID="Label3" runat="server" Text="Contact 2 " Font-Bold="True" CssClass="LabelText" TextAlign="Left"></SAHL:SAHLLabel></td>
                            <td style="width: 50%">
                                <SAHL:SAHLTextBox ID="txtContact2Name" runat="server" Width="90%"></SAHL:SAHLTextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 50%; text-align: left">
                                <SAHL:SAHLLabel ID="Label4" runat="server" Text="Phone " Font-Bold="True" CssClass="LabelText" TextAlign="Left"></SAHL:SAHLLabel></td>
                            <td style="width: 50%;">
                                <SAHL:SAHLTextBox ID="txtContact2Phone" runat="server" Width="90%"></SAHL:SAHLTextBox></td>
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
                                <SAHL:SAHLLabel ID="lblAssesmentByDate" runat="server" Font-Bold="True"
                                    Text="Assessment Date" Width="149px" CssClass="LabelText"></SAHL:SAHLLabel></td>
                            <td style="width: 70%">
                                <SAHL:SAHLDateBox ID="dtAssessmentByDateValue" runat="server" CultureName="en-GB" Width="60%" Mandatory="True"></SAHL:SAHLDateBox></td>
                        </tr>
                        <tr>
                            <td style="width: 30%; text-align: left">
                                <SAHL:SAHLLabel ID="lblAssessmentReason" runat="server" Font-Bold="True" Text="Assessment Reason "
                                    Width="169px" CssClass="LabelText" TextAlign="Left"></SAHL:SAHLLabel>

                            </td>
                            <td style="width: 70%">
                                <SAHL:SAHLDropDownList ID="ddlValuationReasons" runat="server" AutoPostBack="false" PleaseSelectItem="true" Mandatory="true">
                                </SAHL:SAHLDropDownList></td>
                        </tr>
                        <tr>
                            <td style="text-align: left" valign="top">
                                <SAHL:SAHLLabel ID="lblSpecialInstructions" runat="server" Font-Bold="True" Text="Special Instructions "
                                    Width="163px" CssClass="LabelText" TextAlign="Left"></SAHL:SAHLLabel></td>
                            <td style="width: 70%">
                                <SAHL:SAHLTextBox ID="txtSpecialInstructions" runat="server" TextMode="MultiLine"
                                    Width="90%" Height="76px"></SAHL:SAHLTextBox></td>
                        </tr>

                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
    </table>
    <div style="float:right;">
        <SAHL:SAHLButton ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
        <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
    </div>
</asp:Content>
