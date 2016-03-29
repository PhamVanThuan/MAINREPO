<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" Inherits="SAHL.Web.Views.ReleaseAndVariations.ReleaseAndVariationsSummary"
    Title="Account" Codebehind="ReleaseAndVariationsSummary.aspx.cs" %>
<%@ Register Src="WorkFlowHeader.ascx" TagName="WorkFlowHeader" TagPrefix="uc1" %>
<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table width="100%">
        <tr>
            <td style="width: 49%" valign="top">
                <asp:Panel ID="pnlContact" runat="server" GroupingText="Release & Variation Request Summary" Width="100%" Height="250px">
                    <table width="100%">
                        <tr>
                            <td align="left" style="width: 35%;height: 30px;">
                                Account Name</td>
                            <td align="left">
                                <SAHL:SAHLLabel ID="lblAccountName" runat="server" CssClass="LabelText"
                                    Width="95%"></SAHL:SAHLLabel></td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 35%;height: 30px;">
                                Account Number</td>
                            <td align="left">
                                <SAHL:SAHLLabel ID="lblAccountNumber" runat="server" CssClass="LabelText"
                                    Width="95%"></SAHL:SAHLLabel></td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 35%;height: 30px;">
                                Linked to Offer</td>
                            <td align="left">
                                <SAHL:SAHLLabel ID="lblLinkedToOffer" runat="server" CssClass="LabelText"
                                    Width="95%"></SAHL:SAHLLabel></td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 35%;height: 30px;">
                                Request Type</td>
                            <td align="left">
                                <SAHL:SAHLDropDownList ID="ddlRequestType" runat="server" PleaseSelectItem="False"
                                    Width="250px" Visible="False">
                                </SAHL:SAHLDropDownList>
                                <SAHL:SAHLLabel ID="lblRequestType" runat="server" CssClass="LabelText"
                                    Width="95%" TextAlign="Left" Visible="False"></SAHL:SAHLLabel></td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 35%;height: 30px;">
                                Apply Change To</td>
                            <td align="left">
                                <SAHL:SAHLDropDownList ID="ddlApplyChangeTo" runat="server" Width="250px" Visible="False">
                                </SAHL:SAHLDropDownList>
                                <SAHL:SAHLLabel ID="lblApplyChangeTo" runat="server" CssClass="LabelText"
                                    Width="95%" TextAlign="Left" Visible="False"></SAHL:SAHLLabel></td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 35%;height: 30px;">
                                Loan Balance</td>
                            <td align="left">
                                <SAHL:SAHLLabel ID="lblLoanBalance" runat="server" CssClass="LabelText"
                                    Width="95%"></SAHL:SAHLLabel></td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 35%;height: 30px;">
                                Arrears</td>
                            <td align="left">
                                <SAHL:SAHLLabel ID="lblArrears" runat="server" CssClass="LabelText"
                                    Width="95%"></SAHL:SAHLLabel></td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 35%;height: 30px;">
                                SPV</td>
                            <td align="left">
                                <SAHL:SAHLLabel ID="lblSPV" runat="server" CssClass="LabelText" Width="95%" TextAlign="Left"></SAHL:SAHLLabel></td>
                        </tr>
                    </table>
                </asp:Panel><asp:Panel ID="pnlMemo" runat="server" GroupingText="Release & Variation Request Memo" Width="100%" Height="250px">
                    <SAHL:SAHLTextBox ID="txtNotes" BackColor="#FFFFC0"  runat="server" Height="250px" TextMode="MultiLine" Width="95%"></SAHL:SAHLTextBox></asp:Panel>
            </td>
            <td style="width: 47%" valign="top">
                <asp:Panel ID="pnlLoanDetails" runat="server" GroupingText="Loan Details" Width="100%" Visible="False" Height="100px">
                    <table width="100%">
                        <tr>
                            <td align="left" colspan="1" style="width: 135px; height: 30px;">
                                </td>
                            <td align="left" style="width: 120px; height: 30px;">
                                &nbsp;LTV%</td>
                            <td align="left" style="width: 120px; height: 30px;">
                                PTI%</td>
                            <td align="left" style="width: 120px; height: 30px;">
                                Loan %</td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 135px; height: 30px;">
                                Current :</td>
                            <td align="left" style="width: 120px; height: 30px;">
                                <SAHL:SAHLLabel ID="lblCurrentLTV" runat="server" CssClass="LabelText" TextAlign="Left"></SAHL:SAHLLabel></td>
                            <td align="left" style="width: 120px; height: 30px;">
                                <SAHL:SAHLLabel ID="lblCurrentPTI" runat="server" CssClass="LabelText" TextAlign="Left"></SAHL:SAHLLabel></td>
                            <td align="left" style="width: 120px; height: 30px;">
                                <SAHL:SAHLLabel ID="lblCurrentLoan" runat="server" CssClass="LabelText" TextAlign="Left"></SAHL:SAHLLabel></td>
                        </tr>
                        <tr >
                            <td align="left" style="width: 135px; height: 30px;">
                                Products :</td>
                            <td align="left" style="width: 135px; height: 30px;" colspan="3">
                                <SAHL:SAHLLabel ID="lblProducts" runat="server" CssClass="LabelText"></SAHL:SAHLLabel></td>
                       
                        </tr>
                    </table>
                </asp:Panel><asp:Panel   ID="pnlBondDetails" runat="server" GroupingText="Bond Details" Width="99%" Visible="False" Height="200px">
                    <SAHL:SAHLGridView ID="gridBondDetails" runat="server" GridHeight="200px" GridWidth="100%"
            Width="99%"  AutoGenerateColumns="False" BackColor="#FFFFC0" >
                    </SAHL:SAHLGridView>
                </asp:Panel>
                <asp:Panel ID="pnlConditions" runat="server" GroupingText="Release & Variation Conditions" Width="99%" Visible="False" Height="200px">
                    <SAHL:SAHLGridView ID="gridConditions" BackColor="#FFFFC0"  runat="server" GridHeight="200px" GridWidth="100%"
            Width="99%" AutoGenerateColumns="False" >
                    </SAHL:SAHLGridView>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td align="right" colspan="2" style="height: 31px; text-align: center;">
                <SAHL:SAHLLabel ID="lblCaption" runat="server" CssClass="LabelText" Font-Bold="True" Visible="False" Width="80%" Font-Names="Verdana" Font-Size="Small">This action will create a Release & Variation request. Are you sure you want to proceed? </SAHL:SAHLLabel>&nbsp;&nbsp;
                &nbsp; &nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td align="right" colspan="2" style="height: 31px">
                <SAHL:SAHLButton ID="btnConfirm" runat="server" CssClass="BtnNormal3"
                    Text="Yes" Width="154px" ButtonSize="Size6" Visible="False" OnClick="btnConfirm_Click" />
                <SAHL:SAHLButton ID="btnCancel" runat="server" CssClass="BtnNormal3"
                    Text="Cancel" Width="154px" ButtonSize="Size6" Visible="False" OnClick="btnCancel_Click" />
                <SAHL:SAHLButton ID="btnSubmit" runat="server" CssClass="BtnNormal3"
                    Text="Submit" Width="154px" ButtonSize="Size6" Visible="False" OnClick="btnSubmit_Click" />
                <SAHL:SAHLButton ID="btnPrintRequest" runat="server" CssClass="BtnNormal3"
                    Text="Print Request" Width="154px" ButtonSize="Size6" Visible="False" OnClick="btnPrintRequest_Click" />
                <SAHL:SAHLButton ID="btnUpdateSummary" runat="server"
                    Text="Save" Width="147px" ButtonSize="Size6" Visible="False" OnClick="btnUpdateSummary_Click" />
                <SAHL:SAHLButton ID="btnUpdateConditions" runat="server" CssClass="BtnNormal3"
                    Text="R&V Conditions" Width="154px" ButtonSize="Size6" Visible="False" OnClick="btnUpdateConditions_Click" />
                <SAHL:SAHLButton ID="btnERFInformation" runat="server" ButtonSize="Size6"
                    Text="ERF Information" Width="150px" Visible="False" OnClick="btnERFInformation_Click" />
                </td>
        </tr>
    </table>
</asp:Content>
