<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="HOCFSSummary.aspx.cs" Inherits="SAHL.Web.Views.Common.HOCFSSummary"
    Title="HOCFSSummary" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table width="100%" class="tableStandard">
        <tr class="rowStandard">
            <td valign="top">
                <cc1:AddressGrid ID="PropertyGrid" runat="server" PostBackType="SingleClick" OnSelectedIndexChanged="PropertiesGrid_SelectedIndexChanged">
                </cc1:AddressGrid>
                <SAHL:SAHLLabel ID="NoRecsLbl" runat="server" Visible="false" Height="10px"></SAHL:SAHLLabel>
                <table id="tblHOC" runat="server" class="tableStandard">
                    <tr class="rowStandard">
                        <td>
                            <table border="0" style="width: 390px;">
                                <tr id="trCommencementDate" runat="server"  class="rowStandard">
                                    <td style="width: 160px;" class="TitleText">
                                        Commencement Date
                                    </td>
                                    <td style="width: 160px;">
                                        <SAHL:SAHLLabel ID="lblCommencementDate" runat="server">-</SAHL:SAHLLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TitleText" style="width: 160px;"  class="rowStandard">
                                        Status</td>
                                    <td>
                                        <SAHL:SAHLLabel ID="lblStatus" runat="server">-</SAHL:SAHLLabel>
                                        <SAHL:SAHLDropDownList ID="ddlStatus" runat="server" Width="180px" PleaseSelectItem="False"
                                            AutoPostBack="true">
                                        </SAHL:SAHLDropDownList>
                                    </td>
                                </tr>
                                <tr id="trAnniversaryDate" runat="server"  class="rowStandard">
                                    <td class="TitleText">
                                        Anniversary Date</td>
                                    <td>
                                        <SAHL:SAHLLabel ID="lblAnniversaryDate" runat="server">-</SAHL:SAHLLabel>
                                    </td>
                                </tr>
                                <tr id="trCloseDate" runat="server"  class="rowStandard">
                                    <td class="TitleText">
                                        Close Date
                                    </td>
                                    <td>
                                        <SAHL:SAHLLabel ID="lblCloseDate" runat="server">-</SAHL:SAHLLabel>
                                    </td>
                                </tr>
                                <tr  class="rowStandard">
                                    <td class="TitleText" style="width: 160px;">
                                        HOC Insurer
                                    </td>
                                    <td id="tdHOCInsurer" runat="server">
                                        <SAHL:SAHLLabel ID="lblHOCInsurer" runat="server">-</SAHL:SAHLLabel>
                                        <SAHL:SAHLDropDownList ID="ddlHOCInsurer" runat="server" Style="width: 180px" PleaseSelectItem="true"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddlHOCInsurer_SelectedIndexChanged">
                                        </SAHL:SAHLDropDownList>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr  class="rowStandard">
                                    <td class="TitleText">
                                        Subsidence Description
                                    </td>
                                    <td>
                                        <SAHL:SAHLLabel ID="lblSubsidenceDescription" runat="server">-</SAHL:SAHLLabel>
                                        <SAHL:SAHLDropDownList ID="ddlSubsidenceDescription" runat="server" Style="width: 180px"
                                            PleaseSelectItem="False" AutoPostBack="True">
                                        </SAHL:SAHLDropDownList>
                                    </td>
                                </tr>
                                <tr  class="rowStandard">
                                    <td class="TitleText">
                                        Construction Description
                                    </td>
                                    <td>
                                        <SAHL:SAHLLabel ID="lblConstructionDescription" runat="server">-</SAHL:SAHLLabel>
                                        <SAHL:SAHLDropDownList ID="ddlConstructionDescription" runat="server" Style="width: 180px"
                                            PleaseSelectItem="False">
                                        </SAHL:SAHLDropDownList>
                                    </td>
                                </tr>
                                <tr  class="rowStandard">
                                    <td class="TitleText">
                                        Roof Description
                                    </td>
                                    <td>
                                        <SAHL:SAHLLabel ID="lblRoofDescription" runat="server">-</SAHL:SAHLLabel>
                                    </td>
                                </tr>
                                <tr  class="rowStandard">
                                    <td class="TitleText">
                                        Thatch Valuation
                                    </td>
                                    <td>
                                        <SAHL:SAHLLabel ID="lblThatchValuation" runat="server">-</SAHL:SAHLLabel>
                                    </td>
                                </tr>
                                <tr  class="rowStandard">
                                    <td class="TitleText">
                                        Conventional Valuation
                                    </td>
                                    <td>
                                        <SAHL:SAHLLabel ID="lblConventionalValuation" runat="server">-</SAHL:SAHLLabel>
                                    </td>
                                </tr>
                                <tr  class="rowStandard">
                                    <td class="TitleText">
                                        Shingle Valuation
                                    </td>
                                    <td>
                                        <SAHL:SAHLLabel ID="lblShingleValuation" runat="server">-</SAHL:SAHLLabel>
                                    </td>
                                </tr>
                                <tr  class="rowStandard">
                                    <td class="TitleText">
                                        Total HOC Sum Insured
                                    </td>
                                    <td>
                                        <SAHL:SAHLLabel ID="lblTotalHOCSumInsured" runat="server">-</SAHL:SAHLLabel>
                                        <SAHL:SAHLTextBox ID="txtTotalHOCSumInsured" runat="server" DisplayInputType="Number" Width="165px"></SAHL:SAHLTextBox>
                                    </td>
                                </tr>
                                <tr  class="rowStandard">
                                    <td class="TitleText">
                                        HOC Policy Number
                                    </td>
                                    <td>
                                        <SAHL:SAHLLabel ID="lblHOCPolicyNumber" runat="server">-</SAHL:SAHLLabel>
                                        <SAHL:SAHLTextBox ID="txtHOCPolicyNumber" runat="server" MaxLength="25" Width="165px"></SAHL:SAHLTextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="bottom" style="width: 320px;">
                            <asp:CheckBox ID="chkCeded" runat="server" Text="Policy ceded" />
                            <asp:HiddenField ID="CededPop" runat="server" />
                            <asp:Panel ID="pnlPremiums" runat="server" GroupingText="Premiums">
                                <table border="0">
                                    <tr  class="rowStandard">
                                        <td class="TitleText" style="width: 150px;">
                                            Pro Rata Premium
                                        </td>
                                        <td style="width: 150px;">
                                            <SAHL:SAHLLabel ID="lblProRataPremium" runat="server">-</SAHL:SAHLLabel>
                                        </td>
                                    </tr>
                                    <tr  class="rowStandard">
                                        <td class="TitleText">
                                            Thatch Premium
                                        </td>
                                        <td>
                                            <SAHL:SAHLLabel ID="lblThatchPremium" runat="server">-</SAHL:SAHLLabel>
                                        </td>
                                    </tr>
                                    <tr  class="rowStandard">
                                        <td class="TitleText">
                                            Conventional Premium
                                        </td>
                                        <td>
                                            <SAHL:SAHLLabel ID="lblConventionalPremium" runat="server">-</SAHL:SAHLLabel>
                                        </td>
                                    </tr>
                                    <tr  class="rowStandard">
                                        <td class="TitleText">
                                            Shingle Premium
                                        </td>
                                        <td>
                                            <SAHL:SAHLLabel ID="lblShinglePremium" runat="server">-</SAHL:SAHLLabel>
                                        </td>
                                    </tr>
                                    <tr class="rowStandard">
                                        <td class="TitleText">
                                            Total HOC Premium
                                        </td>
                                        <td>
                                            <SAHL:SAHLLabel ID="lblTotalHOCPremium" runat="server">-</SAHL:SAHLLabel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="ButtonRow" runat="server">
            <td align="right">
                <SAHL:SAHLButton ID="btnCancelButton" runat="server" AccessKey="C" CausesValidation="False"
                    Text="Cancel" OnClick="btnCancelButton_Click" />
                <SAHL:SAHLButton ID="btnSubmitButton" runat="server" AccessKey="U" OnClick="SubmitButton_Click"
                    Text="Update" />
            </td>
        </tr>
    </table>
</asp:Content>
