<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Common.Views_LegalEntityAssetLiability" Title="Legal Entity Assets Liabilities"
    EnableViewState="False" Codebehind="LegalEntityAssetLiability.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table width="100%" class="tableStandard">
        <tr>
            <td align="left" valign="top" colspan="2">
                <table width="100%" class="tableStandard">
                    <tr>
                        <td>
                            <SAHL:SAHLGridView ID="grdAssetLiability" runat="server" AutoGenerateColumns="false"
                                ShowHeader="True" OnRowDataBound="grdAssetLiability_RowDataBound" GridHeight="180px"
                                GridWidth="100%" Width="100%" HeaderCaption="Assets & Liabilities: " NullDataSetMessage=""
                                PostBackType="None" OnSelectedIndexChanged="grdAssetLiability_SelectedIndexChanged"
                                EmptyDataSetMessage="No assets or liabilities found" ShowFooter="True">
                                <RowStyle CssClass="TableRowA" />
                            </SAHL:SAHLGridView>
                            <div style="width: 100%; background-color: #E0E0E0">
                                <div id="Div1" style="float: left">
                                    <SAHL:SAHLLabel ID="lblFooter1" runat="server" Width="516px" CssClass="LabelText" Font-Bold="True"></SAHL:SAHLLabel>
                                </div>
                            <div id="right" style="float: right">
                                <SAHL:SAHLLabel ID="lblFooter3" runat="server" TextAlign="Right" Width="245px" CssClass="LabelText" Font-Bold="True"></SAHL:SAHLLabel>
                            </div>
                            </div>
                            <br />
                            <asp:Panel ID="pnlAssociate" runat="Server" Visible="false">
                                <table>
                                    <tr>
                                        <td style="width: 150px">
                                            <strong>Asset/Liability</strong></td>
                                        <td id="cellAssociate" runat="server" style="width: 361px">
                                            <SAHL:SAHLDropDownList ID="ddlAssociate" runat="server" AutoPostBack="True" Width="500px"
                                                Visible="false" OnSelectedIndexChanged="ddlAssociate_SelectedIndexChanged" PleaseSelectItem="False" Mandatory="True">
                                            </SAHL:SAHLDropDownList>
                                            <SAHL:SAHLLabel ID="lblAssociate" runat="server" Visible="false" Text="There are no assets/liabilities available for association" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlFixedProperty" runat="Server" Width="550px" GroupingText="Assets & Liabilities"
                                BorderStyle="Outset" Visible="false">
                                <table cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td>
                                            <strong>Type</strong></td>
                                        <td runat="server" style="width: 364px">
                                            <SAHL:SAHLLabel ID="fpType" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <strong>Date Acquired</strong></td>
                                        <td>
                                            <SAHL:SAHLLabel ID="fpDateAcquired" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <strong runat="server" id="tdAddress">Address</strong></td>
                                        <td id="Td2" runat="server" style="width: 364px">
                                            <SAHL:SAHLLabel ID="fpAddress" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <strong>Asset Value</strong></td>
                                        <td id="Td4" runat="server" style="width: 364px">
                                            <SAHL:SAHLLabel ID="fpAssetValue" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <strong>Liability Value</strong></td>
                                        <td id="Td5" runat="server" style="width: 364px">
                                            <SAHL:SAHLLabel ID="fpLiabilityValue" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlInvestmentProperty" runat="Server" Width="550px" GroupingText="Assets & Liablities"
                                BorderStyle="Outset" Visible="false">
                                <table cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td>
                                            <strong>Type</strong></td>
                                        <td id="Td1" runat="server" style="width: 364px">
                                            <SAHL:SAHLLabel ID="InvestmentPropertyType" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <strong>Asset Value</strong></td>
                                        <td id="Td7" runat="server" style="width: 364px">
                                            <SAHL:SAHLLabel ID="InvestmentpropertyAssetValue" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <strong>Company Name</strong></td>
                                        <td id="Td8" runat="server" style="width: 364px">
                                            <SAHL:SAHLLabel ID="InvestmentpropertyCompanyName" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlLiabilityLoan" runat="Server" Width="550px" GroupingText="Assets & Liablities"
                                BorderStyle="Outset" Visible="false">
                                <table cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td>
                                            <strong>Type</strong></td>
                                        <td id="Td3" runat="server" style="width: 364px">
                                            <SAHL:SAHLLabel ID="llType" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <strong>Date Repayable</strong></td>
                                        <td id="Td9" runat="server" style="width: 364px">
                                            <SAHL:SAHLLabel ID="llDateRepayable" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <strong>Financial Institution</strong></td>
                                        <td id="Td10" runat="server" style="width: 364px">
                                            <SAHL:SAHLLabel ID="llFinInstitution" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <strong>Instalment Value</strong></td>
                                        <td id="Td6" runat="server" style="width: 364px">
                                            <SAHL:SAHLLabel ID="llInstallmentValue" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <strong>Liability Value</strong></td>
                                        <td id="Td11" runat="server" style="width: 364px">
                                            <SAHL:SAHLLabel ID="llLiabilityValue" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <strong>Loan Type</strong></td>
                                        <td id="Td12" runat="server" style="width: 364px">
                                            <SAHL:SAHLLabel ID="llLoanType" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlLiabilitySurety" runat="Server" Width="550px" GroupingText="Assets & Liablities"
                                BorderStyle="Outset" Visible="false">
                                <table cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td>
                                            <strong>Type</strong></td>
                                        <td id="Td13" runat="server" style="width: 364px">
                                            <SAHL:SAHLLabel ID="lsType" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <strong>Asset Value</strong></td>
                                        <td id="Td14" runat="server" style="width: 364px">
                                            <SAHL:SAHLLabel ID="lsAssetValue" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <strong>Description</strong></td>
                                        <td id="Td15" runat="server" style="width: 364px">
                                            <SAHL:SAHLLabel ID="lsDescription" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <strong>Liability Value</strong></td>
                                        <td id="Td16" runat="server" style="width: 364px">
                                            <SAHL:SAHLLabel ID="lsLiabilityValue" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlLifeAssurance" runat="Server" Width="550px" GroupingText="Assets & Liablities"
                                BorderStyle="Outset" Visible="false">
                                <table cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td>
                                            <strong>Type</strong></td>
                                        <td id="Td17" runat="server" style="width: 364px">
                                            <SAHL:SAHLLabel ID="laType" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <strong>Company Name</strong></td>
                                        <td id="Td19" runat="server" style="width: 364px">
                                            <SAHL:SAHLLabel ID="laCompanyName" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <strong>Surrender Value</strong></td>
                                        <td id="Td20" runat="server" style="width: 364px">
                                            <SAHL:SAHLLabel ID="laSurrenderValue" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlOther" runat="Server" Width="550px" GroupingText="Assets & Liablities"
                                BorderStyle="Outset" Visible="false">
                                <table cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td>
                                            <strong>Type</strong></td>
                                        <td id="Td18" runat="server" style="width: 364px">
                                            <SAHL:SAHLLabel ID="otherType" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <strong>Asset Value</strong></td>
                                        <td id="Td21" runat="server" style="width: 364px">
                                            <SAHL:SAHLLabel ID="otherAssetValue" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <strong>Description</strong></td>
                                        <td id="Td22" runat="server" style="width: 364px">
                                            <SAHL:SAHLLabel ID="otherDescription" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <strong>Liability Value</strong></td>
                                        <td id="Td23" runat="server" style="width: 364px">
                                            <SAHL:SAHLLabel ID="otherLiabilityValue" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlFixedLongTermInvestment" runat="Server" Width="550px" GroupingText="Assets & Liablities"
                                BorderStyle="Outset" Visible="false">
                                <table cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td>
                                            <strong>Type</strong></td>
                                        <td id="Td24" runat="server" style="width: 364px">
                                            <SAHL:SAHLLabel ID="lblFixedLongTermInvestment" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <strong>Company Name</strong></td>
                                        <td id="Td25" runat="server" style="width: 364px">
                                            <SAHL:SAHLLabel ID="lblCompanyFixedLongTermInvestment" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <strong>Liability Value</strong></td>
                                        <td id="Td26" runat="server" style="width: 364px">
                                            <SAHL:SAHLLabel ID="lblLiabilityFixedLongTermInvestment" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <p />
                            <table runat="server" id="tblInput" cellspacing="1" cellpadding="1" width="100%">
                                <tr runat="server" id="rowType" visible="false">
                                    <td style="width: 178px">
                                        <SAHL:SAHLLabel ID="tType" runat="server" Font-Bold="true">Type</SAHL:SAHLLabel></td>
                                    <td style="width: 122px">
                                        <SAHL:SAHLLabel ID="lblAssetType" runat="server" Visible="false">-</SAHL:SAHLLabel>
                                        <SAHL:SAHLDropDownList ID="ddlType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"
                                            Visible="false" Mandatory="True" /></td>
                                    <td style="width: 460px">
                                    </td>
                                </tr>
                                <tr runat="server" id="rowSubType" visible="false">
                                    <td style="width: 178px">
                                        <SAHL:SAHLLabel ID="tSubType" runat="server" Font-Bold="true">Loan Type</SAHL:SAHLLabel></td>
                                    <td style="width: 460px">
                                        <SAHL:SAHLDropDownList ID="ddlSubType" runat="server" Mandatory="True">
                                        </SAHL:SAHLDropDownList></td>
                                </tr>
                                <tr runat="server" id="rowDateAcquired" visible="false">
                                    <td style="width: 178px">
                                        <SAHL:SAHLLabel ID="tDateAcquired" runat="server" Font-Bold="true">Date Acquired</SAHL:SAHLLabel></td>
                                    <td>
                                        <SAHL:SAHLDateBox ID="dtDateAcquired" Style="width: 100px" runat="server" Columns="15" Mandatory="True" /></td>
                                </tr>
                                <tr runat="server" id="rowAddress" visible="false">
                                    <td style="width: 178px">
                                        <SAHL:SAHLLabel ID="tAddress" runat="server" Font-Bold="true">Address</SAHL:SAHLLabel></td>
                                    <td style="width: 550px">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <SAHL:SAHLLabel ID="lblAddress" runat="server" Visible="false">-</SAHL:SAHLLabel>
                                                    <SAHL:SAHLDropDownList ID="ddlAddress" runat="server" OnSelectedIndexChanged="ddlAddressSelectedIndexChanged"
                                                        AutoPostBack="true" Width="450px" Mandatory="True">
                                                    </SAHL:SAHLDropDownList>
                                                </td>
                                                <td>
                                                    <SAHL:SAHLButton ID="btnAssociateAddress" runat="server" CausesValidation="False"
                                                        Text="Capture Address" OnClick="btnAssociateAddress_Click" ButtonSize="Size5" />
                                                </td>
                                            </tr>   
                                        </table>
                                    </td>
                                </tr>
                                <tr runat="server" id="rowCompanyName" visible="false">
                                    <td style="width: 178px">
                                        <SAHL:SAHLLabel ID="tCompanyName" runat="server" Font-Bold="true">Company Name</SAHL:SAHLLabel></td>
                                    <td style="width: 460px">
                                        <SAHL:SAHLTextBox ID="txtCompanyName" runat="server" MaxLength="255" Width="200px" Mandatory="True"></SAHL:SAHLTextBox>
                                    </td>
                                </tr>
                                <tr runat="server" id="rowFinancialInstitution" visible="false">
                                    <td style="width: 178px">
                                        <SAHL:SAHLLabel ID="tFinancialInstitution" runat="server" Font-Bold="true">Financial Institution</SAHL:SAHLLabel></td>
                                    <td style="width: 460px">
                                        <SAHL:SAHLTextBox ID="txtFinancialInstitution" runat="server" Width="200px" Mandatory="True"></SAHL:SAHLTextBox>
                                    </td>
                                </tr>
                                <tr runat="server" id="rowDateRepayable" visible="false">
                                    <td style="width: 178px">
                                        <SAHL:SAHLLabel ID="tDateRepayable" runat="server" Font-Bold="true">Date Repayable</SAHL:SAHLLabel></td>
                                    <td style="width: 460px">
                                        <SAHL:SAHLDateBox ID="dtDateRepayable" runat="server" Mandatory="True" /></td>
                                </tr>
                                <tr runat="server" id="rowInstalmentValue" visible="false">
                                    <td style="width: 178px">
                                        <SAHL:SAHLLabel ID="tInstalmentValue" runat="server" Font-Bold="true">Instalment Value</SAHL:SAHLLabel></td>
                                    <td style="width: 460px">
                                        <SAHL:SAHLCurrencyBox ID="txtInstalmentValue" runat="server" MaxLength="15" Mandatory="True">0.00</SAHL:SAHLCurrencyBox>
                                    </td>
                                </tr>
                                <tr runat="server" id="rowAssetValue" visible="false">
                                    <td style="width: 178px">
                                        <SAHL:SAHLLabel ID="tAssetValue" runat="server" Font-Bold="true">Asset Value</SAHL:SAHLLabel></td>
                                    <td style="width: 460px">
                                        <SAHL:SAHLCurrencyBox ID="txtAssetValue" runat="server" MaxLength="15" Mandatory="True">0.00</SAHL:SAHLCurrencyBox>
                                    </td>
                                </tr>
                                <tr runat="server" id="rowLiabilityValue" visible="false">
                                    <td style="width: 178px">
                                        <SAHL:SAHLLabel ID="tLiabilityValue" runat="server" Font-Bold="true">Liability Value</SAHL:SAHLLabel></td>
                                    <td style="width: 460px">
                                        <SAHL:SAHLCurrencyBox ID="txtLiabilityValue" runat="server" MaxLength="15" AllowNegative="True" Mandatory="True">0.00</SAHL:SAHLCurrencyBox>
                                    </td>
                                </tr>
                                <tr runat="server" id="rowSurrenderValue" visible="false">
                                    <td style="width: 178px">
                                        <SAHL:SAHLLabel ID="tSurrenderValue" runat="server" Font-Bold="true">Surrender Value</SAHL:SAHLLabel></td>
                                    <td style="width: 460px">
                                        <SAHL:SAHLCurrencyBox ID="txtSurrenderValue" runat="server" MaxLength="15" Mandatory="True">0.00</SAHL:SAHLCurrencyBox>
                                    </td>
                                </tr>
                                <tr runat="server" id="rowDescription" visible="false">
                                    <td valign="top" style="width: 178px">
                                        <SAHL:SAHLLabel ID="tDescription" runat="server" Font-Bold="true">Description</SAHL:SAHLLabel></td>
                                    <td style="width: 460px">
                                        <SAHL:SAHLTextBox ID="txtDescription" TextMode="MultiLine" Rows="5" runat="server"
                                            Width="250" Mandatory="True"></SAHL:SAHLTextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="ButtonRow" runat="server">
            <td align="left">
                &nbsp;</td>
            <td align="right" style="height: 25px">
                <SAHL:SAHLButton ID="btnAddUpdate" runat="server" AccessKey="A" CausesValidation="True"
                    Text="Update" OnClick="btnAdd_Click" Visible="false" />
                <SAHL:SAHLButton ID="btnDelete" runat="server" AccessKey="D" CausesValidation="False"
                    Text="Delete" OnClick="btnDelete_Click" Visible="false" />
                <SAHL:SAHLButton ID="btnCancel" runat="server" AccessKey="C" CausesValidation="False"
                    Text="Cancel" OnClick="btnCancel_Click" Visible="false" />
            </td>
        </tr>
    </table>
</asp:Content>
