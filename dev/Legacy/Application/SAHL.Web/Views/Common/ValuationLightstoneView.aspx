<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="ValuationLightstoneView.aspx.cs" Inherits="SAHL.Web.Views.Common.ValuationLightstoneView"
    Title="Lightstone Automated Valuation" EnableViewState="False" %>

<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <table width="99%">
        <tr>
            <td>
                <ajaxToolkit:TabContainer runat="server" ID="Tabs" Height="400px" ActiveTabIndex="0"
                    OnActiveTabChanged="Tabs_ActiveTabChanged">
                    <ajaxToolkit:TabPanel runat="server" ID="Panel1" HeaderText="Signature and Bio">
                        <ContentTemplate>
                            <asp:Panel ID="pnlInspectionContactDetails" runat="server" GroupingText="Automated Valuation"
                                Width="99%">
                                <table style="width: 760px">
                                    <tr>
                                        <td style="width: 200px">
                                            <SAHL:SAHLLabel ID="_lblReferenceNumber" runat="server" Font-Bold="False" Text="Reference Number"
                                                Width="227px" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                        </td>
                                        <td style="width: 280px">
                                            <SAHL:SAHLLabel ID="lblReferenceNumberValue" runat="server" Width="130px" TextAlign="Left"></SAHL:SAHLLabel>
                                        </td>
                                        <td style="width: 280px">
                                            &nbsp;<SAHL:SAHLLabel ID="Label2" runat="server" Font-Bold="False" Text="ERF Size"
                                                Width="227px" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                        <td style="width: 280px">
                                            &nbsp;<SAHL:SAHLLabel ID="lblERFSize" runat="server" Width="130px" TextAlign="Left"></SAHL:SAHLLabel></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px; height: 21px">
                                            <SAHL:SAHLLabel ID="_lblAutomatedValueDate" runat="server" Font-Bold="False" Text="Automated Valuation Date"
                                                Width="227px" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                        </td>
                                        <td style="width: 280px; height: 21px">
                                            <SAHL:SAHLLabel ID="lblAutomatedValueDateValue" runat="server" Width="130px" TextAlign="Left"></SAHL:SAHLLabel>
                                        </td>
                                        <td style="width: 280px; height: 21px">
                                            &nbsp;<SAHL:SAHLLabel ID="lblLastSaleDate" runat="server" Font-Bold="False" Text="Last Sale Date"
                                                Width="227px" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                        <td style="width: 280px; height: 21px">
                                            &nbsp;<SAHL:SAHLLabel ID="lblLastSaleDateValue" runat="server" Width="130px" TextAlign="Left"></SAHL:SAHLLabel></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px">
                                            <SAHL:SAHLLabel ID="_lblEstimatedMarketValue" runat="server" Font-Bold="False" Text="Estimated Market Value"
                                                Width="227px" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                        </td>
                                        <td style="width: 280px">
                                            <SAHL:SAHLLabel ID="lblEstimatedMarketValue" runat="server" Width="130px" TextAlign="Left"></SAHL:SAHLLabel>
                                        </td>
                                        <td style="width: 280px">
                                            &nbsp;<SAHL:SAHLLabel ID="_lblLastSalePrice" runat="server" Font-Bold="False" Text="Last Sale Price"
                                                Width="227px" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                        <td style="width: 280px">
                                            &nbsp;<SAHL:SAHLLabel ID="lblLastSalePriceValue" runat="server" Width="130px" TextAlign="Left"></SAHL:SAHLLabel></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px; height: 21px">
                                            <SAHL:SAHLLabel ID="_lblConfidenceScore" runat="server" Font-Bold="False" Text="Confidence Score"
                                                Width="227px" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                        </td>
                                        <td style="width: 280px; height: 21px">
                                            <SAHL:SAHLLabel ID="lblConfidenceScoreValue" runat="server" Width="130px" TextAlign="Left"></SAHL:SAHLLabel>
                                        </td>
                                        <td style="width: 280px; height: 21px">
                                            &nbsp;<SAHL:SAHLLabel ID="_lblAreaQualityGrade" runat="server" Font-Bold="False" Text="Area Quality Grade (AQG)"
                                                Width="227px" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                        <td style="width: 280px; height: 21px">
                                            &nbsp;<SAHL:SAHLLabel ID="lblAreaQualityGradeValue" runat="server" Width="130px" TextAlign="Left"></SAHL:SAHLLabel></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px">
                                            <SAHL:SAHLLabel ID="_lblExpectedHighValue" runat="server" Font-Bold="False" Text="Expected High Value"
                                                Width="227px" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                        </td>
                                        <td style="width: 280px">
                                            <SAHL:SAHLLabel ID="lblExpectedHighValue" runat="server" Width="130px" TextAlign="Left"></SAHL:SAHLLabel>
                                        </td>
                                        <td style="width: 280px">
                                            &nbsp;<SAHL:SAHLLabel ID="lblAreaExposure" runat="server" Font-Bold="False" Text="Area Exposure"
                                                Width="227px" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                        <td style="width: 280px">
                                            &nbsp;<SAHL:SAHLLabel ID="lblAreaExposureValue" runat="server" Width="130px" TextAlign="Left"></SAHL:SAHLLabel></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px">
                                            <SAHL:SAHLLabel ID="_lblExpectedLowValue" runat="server" Font-Bold="False" Text="Expected Low Value"
                                                Width="227px" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel>
                                        </td>
                                        <td style="width: 280px">
                                            <SAHL:SAHLLabel ID="lblExpectedLowValue" runat="server" Width="130px" TextAlign="Left"></SAHL:SAHLLabel>
                                        </td>
                                        <td style="width: 280px">
                                            <SAHL:SAHLLabel ID="_lblAutomatedValuationDecision" runat="server" Font-Bold="False" Text="Automated Valuation Decision"
                                                Width="255px" TextAlign="Left" CssClass="LabelText"></SAHL:SAHLLabel></td>
                                        <td style="width: 280px">
                                            <SAHL:SAHLLabel ID="lblAutomatedValuationDecisionValue" runat="server" Width="130px" TextAlign="Left"></SAHL:SAHLLabel></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlOwners" runat="server" GroupingText="Owner Details" Visible="False"
                                Width="100%">
                                <SAHL:SAHLGridView ID="grdOwners" runat="server" AutoGenerateColumns="False" EmptyDataSetMessage="There are no Owner Details"
                                    EnableViewState="False" FixedHeader="False" GridHeight="200px" GridWidth="100%"
                                    Width="99%" Invisible="False" SelectFirstRow="True" TotalsFooter="True">
                                    <RowStyle CssClass="TableRowA" />
                                </SAHL:SAHLGridView>
                            </asp:Panel>
                        </ContentTemplate>
                        <HeaderTemplate>
                            Valuation
                        </HeaderTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel runat="server" ID="Panel3" HeaderText="Email">
                        <ContentTemplate>
                            <SAHL:SAHLGridView ID="grdComparativeSales" runat="server" AutoGenerateColumns="False"
                                EmptyDataSetMessage="There are no Comparative Sales" EnableViewState="False"
                                FixedHeader="False" GridHeight="100%" GridWidth="100%" Invisible="False" SelectFirstRow="True"
                                TotalsFooter="True" Width="99%">
                                <RowStyle CssClass="TableRowA" />
                            </SAHL:SAHLGridView>
                        </ContentTemplate>
                        <HeaderTemplate>
                            Comparative Sales
                        </HeaderTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel runat="server" ID="Panel2" HeaderText="Controls">
                        <ContentTemplate>
                            <div>
                                <asp:Panel ID="pnlMaps" runat="server" GroupingText="Aerial Maps" Width="100%" Height="100%" Visible="False">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <SAHL:SAHLLabel ID="Label1" runat="server" Text="Image Source" Width="119px"></SAHL:SAHLLabel>
                                            <asp:DropDownList ID="ddlMapProvider" runat="server" Width="148px" OnSelectedIndexChanged="ddlMapProvider_SelectedIndexChanged"
                                                AutoPostBack="True">
                                                <asp:ListItem Value="0">Select Provider ....</asp:ListItem>
                                                <asp:ListItem Value="1">Google Maps</asp:ListItem>
                                                <asp:ListItem Value="2">MapX</asp:ListItem>
                                            </asp:DropDownList><br />
                                            <asp:Panel ID="Panel4" runat="server" Height="500px" Width="700px" Direction="LeftToRight">
                                                <asp:Literal ID="MapLiteral" runat="server"></asp:Literal>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <br />
                                    <br />
                                </asp:Panel>
                            </div>
                        </ContentTemplate>
                        <HeaderTemplate>
                            Aerial Images
                        </HeaderTemplate>
                    </ajaxToolkit:TabPanel>
                </ajaxToolkit:TabContainer>
            </td>
        </tr>
        <tr>
            <td align="center" valign="bottom" style="text-align: right; height: 26px;">
                <SAHL:SAHLButton ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back"
                    Width="153px" /></td>
        </tr>
        <tr>
            <td align="center" style="height: 26px; text-align: left" valign="bottom">
            </td>
        </tr>
        <tr>
            <td align="center" style="height: 26px; text-align: right" valign="bottom">
                &nbsp;
            </td>
        </tr>
    </table>
    <br />
    <br />
</asp:Content>
