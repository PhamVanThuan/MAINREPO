<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    CodeBehind="DebtCounsellingSummary.aspx.cs" Inherits="SAHL.Web.Views.DebtCounselling.DebtCounsellingSummary"
    Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <table class="tableStandard" width="100%">
        <tr>
            <td align="center">
                <SAHL:SAHLLabel ID="lblEWorkWarningMessage" runat="server" TextAlign="Left" BackColor="Red"
                    ForeColor="White" Font-Size="Larger">
                 A loss control case exists for this account in e-work - please see details below.</SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td>
                <ajaxToolkit:Accordion ID="accDebtCounsellingSummary" runat="server" SelectedIndex="-1"
                    HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                    ContentCssClass="" FadeTransitions="false" FramesPerSecond="40" TransitionDuration="250"
                    AutoSize="None" SuppressHeaderPostbacks="true">
                    <Panes>
                        <ajaxToolkit:AccordionPane ID="apDebtCounsellingDetails" runat="server">
                            <Header>
                                <a href="">Debt Counselling Details</a>
                            </Header>
                            <Content>
                                <br />
                                <table style="width: 99%">
                                    <tr>
                                        <td class="titleText">
                                            Account Number:
                                        </td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblAccountKey" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td style="width: 10px">
                                        </td>
                                        <td class="titleText">
                                            Remaining Term:
                                        </td>
                                        <td>
                                            <SAHL:SAHLLabel ID="lblRemainingTermHighlight" Visible="false" runat="server" BackColor="Red"
                                                ForeColor="White" Font-Size="Larger"></SAHL:SAHLLabel>
                                            <asp:Label runat="server" CssClass="LabelText" ID="lblRemainingTerm" Visible="false"
                                                Text="" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TitleText">
                                            Account Open Date:
                                        </td>
                                        <td class="cellDisplay">
                                            <asp:Label runat="server" CssClass="LabelText" ID="lblAccountOpenDate" Text="TODO" />
                                        </td>
                                        <td>
                                        </td>
                                        <td class="titleText">
                                            SPV:
                                        </td>
                                        <td class="cellDisplay">
                                            <asp:Label runat="server" CssClass="LabelText" ID="lblSPV" Text="TODO" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TitleText">
                                            Current Balance:
                                        </td>
                                        <td class="cellDisplay">
                                            <asp:Label runat="server" CssClass="LabelText" ID="lblCurrentBalance" Text="TODO" />
                                        </td>
                                        <td>
                                        </td>
                                        <td class="TitleText">
                                            Instalment Amount:
                                        </td>
                                        <td class="cellDisplay">
                                            <asp:Label runat="server" CssClass="LabelText" ID="lblInstalmentAmount" Text="TODO" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TitleText">
                                            Arrear Balance:
                                        </td>
                                        <td class="cellDisplay">
                                            <asp:Label runat="server" CssClass="LabelText" ID="lblArrearBalance" Text="TODO" />
                                        </td>
                                        <td>
                                        </td>
                                        <td class="TitleText">
                                            HOC Premium:
                                        </td>
                                        <td class="cellDisplay">
                                            <asp:Label runat="server" CssClass="LabelText" ID="lblHOCPremium" Text="TODO" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TitleText">
                                            Months in Arrear:
                                        </td>
                                        <td class="cellDisplay">
                                            <asp:Label runat="server" CssClass="LabelText" ID="lblMonthsInArrear" Text="TODO" />
                                        </td>
                                        <td>
                                        </td>
                                        <td class="TitleText">
                                            Life Premium (Regent):
                                        </td>
                                        <td class="cellDisplay">
                                            <asp:Label runat="server" CssClass="LabelText" ID="lblLifeRegent" Text="TODO" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TitleText">
                                            Debit Order Day:
                                        </td>
                                        <td class="cellDisplay">
                                            <asp:Label runat="server" CssClass="LabelText" ID="lblDODay" Text="TODO" />
                                        </td>
                                        <td>
                                        </td>
                                        <td class="TitleText">
                                            Life Premium (SAHL):
                                        </td>
                                        <td class="cellDisplay">
                                            <asp:Label runat="server" CssClass="LabelText" ID="lblLifeSAHL" Text="TODO" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TitleText">
                                            Loan To Value:
                                        </td>
                                        <td class="cellDisplay">
                                            <asp:Label runat="server" CssClass="LabelText" ID="lblLTV" Text="TODO" />
                                        </td>
                                        <td>
                                        </td>
                                        <td class="TitleText">
                                            Monthly Service Fee:
                                        </td>
                                        <td class="cellDisplay">
                                            <asp:Label runat="server" CssClass="LabelText" ID="lblAdminFee" Text="TODO" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TitleText">
                                            Latest Valuation Amount:
                                        </td>
                                        <td class="cellDisplay">
                                            <asp:Label runat="server" CssClass="LabelText" ID="lblValuation" Text="TODO" />
                                        </td>
                                        <td>
                                        </td>
                                        <td class="TitleText">
                                            Total Instalment:
                                        </td>
                                        <td class="cellDisplay">
                                            <asp:Label runat="server" CssClass="LabelText" ID="lblTotalInstalment" Text="TODO" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TitleText">
                                            Valuation Date:
                                        </td>
                                        <td class="cellDisplay">
                                            <asp:Label runat="server" CssClass="LabelText" ID="lblValuationDate" Text="TODO" />
                                        </td>
                                        <td>
                                        </td>
                                        <td class="TitleText">
                                            Fixed DO Amount:
                                        </td>
                                        <td class="cellDisplay">
                                            <asp:Label runat="server" CssClass="LabelText" ID="lblFixedDOAmount" Text="TODO" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TitleText">
                                            1st Payment Due Date:
                                        </td>
                                        <td class="cellDisplay">
                                            <asp:Label runat="server" CssClass="LabelText" ID="lblPaymentDueDate" Text="TODO" />
                                        </td>
                                        <td>
                                        </td>
                                        <td class="TitleText">
                                            1st Payment Amount:
                                        </td>
                                        <td class="cellDisplay">
                                            <asp:Label runat="server" CssClass="LabelText" ID="lblPaymentAmount" Text="TODO" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TitleText">
                                            Reference Number:
                                        </td>
                                        <td class="cellDisplay">
                                            <asp:Label runat="server" CssClass="LabelText" ID="lblReferenceNumber" Text="TODO" />
                                        </td>
                                        <td>
                                        </td>
                                        <td class="TitleText">
                                        </td>
                                        <td class="cellDisplay">
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </Content>
                        </ajaxToolkit:AccordionPane>
                        <ajaxToolkit:AccordionPane ID="apDetailypes" runat="server">
                            <Header>
                                <a href="">Detail Types</a>
                            </Header>
                            <Content>
                                <table style="width: 100%">
                                    <tr style="width: 100%">
                                        <td style="width: 100%">
                                            <div style="overflow: scroll; height: 220px; width: 99%">
                                                <SAHL:DXGridView ID="gvDCDetailTypes" runat="server" AutoGenerateColumns="False"
                                                    Width="100%" PostBackType="None" Settings-ShowTitlePanel="true" SettingsText-Title="Detail Types"
                                                    SettingsPager-Mode="ShowAllRecords" Settings-ShowGroupPanel="false" SettingsBehavior-AllowDragDrop="false">
                                                </SAHL:DXGridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                            </Content>
                        </ajaxToolkit:AccordionPane>
                        <ajaxToolkit:AccordionPane ID="apSAHLRoleDetails" runat="server">
                            <Header>
                                <a href="">SAHL Role Details</a>
                            </Header>
                            <Content>
                                <br />
                                <table>
                                    <tr>
                                        <td class="titleText" style="width: 240px">
                                            Debt Counselling Consultant:
                                        </td>
                                        <td class="cellDisplay" style="width: 300px">
                                            <SAHL:SAHLLabel ID="lblConsultant" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titleText">
                                            Debt Counselling Supervisor:
                                        </td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblSupervisor" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titleText">
                                            Debt Counselling Court Consultant:
                                        </td>
                                        <td class="cellDisplay">
                                            <SAHL:SAHLLabel ID="lblCourtConsultant" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </Content>
                        </ajaxToolkit:AccordionPane>
                        <ajaxToolkit:AccordionPane ID="apExternalRoles" runat="server">
                            <Header>
                                <a href="">External Role Details</a>
                            </Header>
                            <Content>
                                <br />
                                <table>
                                    <tr>
                                        <td class="titleText" style="width: 200px">
                                            Debt Counsellor Company:
                                        </td>
                                        <td class="cellDisplay" style="width: 200px">
                                            <SAHL:SAHLLabel ID="lblDCCompany" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td style="width: 20px">
                                        </td>
                                        <td class="titleText" style="width: 200px">
                                            Debt Counsellor Contact:
                                        </td>
                                        <td class="cellDisplay" style="width: 200px">
                                            <SAHL:SAHLLabel ID="lblDCContact" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titleText" style="width: 200px">
                                            NCR Number:
                                        </td>
                                        <td class="cellDisplay" style="width: 200px">
                                            <SAHL:SAHLLabel ID="lblNCRNumber" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td style="width: 20px">
                                        </td>
                                        <td class="titleText" style="width: 200px">
                                            Payment Distribution Agent:
                                        </td>
                                        <td class="cellDisplay" style="width: 200px">
                                            <SAHL:SAHLLabel ID="lblPDA" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titleText" style="width: 200px">
                                            Attorney:
                                        </td>
                                        <td class="cellDisplay" style="width: 200px">
                                            <SAHL:SAHLLabel ID="lblAttorney" runat="server" CssClass="LabelText">TODO</SAHL:SAHLLabel>
                                        </td>
                                        <td style="width: 20px">
                                        </td>
                                        <td class="titleText" style="width: 200px">
                                            Court Date:
                                        </td>
                                        <td class="cellDisplay" style="width: 200px">
                                            <SAHL:SAHLLabel ID="lblCourtDate" runat="server" CssClass="LabelText">TODO</SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </Content>
                        </ajaxToolkit:AccordionPane>
                        <ajaxToolkit:AccordionPane ID="apAcceptedProposal" runat="server">
                            <Header>
                                <a href="">Accepted Proposal Details</a>
                            </Header>
                            <Content>
                                <br />
                                <table style="width: 100%">
                                    <tr>
                                        <td class="titleText" style="width: 150px">
                                            Accepted User:
                                        </td>
                                        <td class="cellDisplay" style="width: 150px">
                                            <SAHL:SAHLLabel ID="lblAcceptedUser" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td style="width: 50px">
                                        </td>
                                        <td class="titleText" style="width: 150px">
                                            Accepted Date:
                                        </td>
                                        <td class="cellDisplay" style="width: 150px">
                                            <SAHL:SAHLLabel ID="lblAcceptedDate" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="titleText" style="width: 150px">
                                            Accepted Rate:
                                        </td>
                                        <td class="cellDisplay" style="width: 150px">
                                            <SAHL:SAHLLabel ID="lblAcceptedProposalRate" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td style="width: 50px">
                                        </td>
                                        <td class="titleText" style="width: 150px">
                                            Accepted Reason:
                                        </td>
                                        <td class="cellDisplay" style="width: 150px">
                                            <SAHL:SAHLLabel ID="lblAcceptedReason" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <%--<tr>
                                    <td class="titleText" style="width: 150px">
                                            Review Date:
                                        </td>
                                        <td class="cellDisplay" style="width: 150px">
                                            <SAHL:SAHLLabel ID="lblReviewDate" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>

                                       
                                        <td style="width: 50px">
                                        </td>
                                        <td class="titleText" style="width: 150px">
                                        </td>
                                        <td class="cellDisplay" style="width: 150px">
                                       </td>
                                         <td>
                                        </td>
                                    </tr>--%>
                                </table>
                                <br />
                            </Content>
                        </ajaxToolkit:AccordionPane>
                        <ajaxToolkit:AccordionPane ID="apEWorkDetails" runat="server" Visible="false">
                            <Header>
                                <a href="">E-Work Loss Control Case Details</a>
                            </Header>
                            <Content>
                                <br />
                                <SAHL:SAHLLabel ID="lblNoEWorkCaseDetails" runat="server" CssClass="LabelText" Visible="False">No e-work case exists in Loss Control.<br /></SAHL:SAHLLabel>
                                <table id="tblEWorkCaseDetails" runat="server" style="width: 100%">
                                    <tr>
                                        <td class="titleText" style="width: 150px">
                                            Stage
                                        </td>
                                        <td class="cellDisplay" style="width: 150px">
                                            <SAHL:SAHLLabel ID="lblEworkStage" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td style="width: 50px">
                                        </td>
                                        <td class="titleText" style="width: 150px">
                                            Assigned User
                                        </td>
                                        <td class="cellDisplay" style="width: 150px">
                                            <SAHL:SAHLLabel ID="lblEworkUser" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </Content>
                        </ajaxToolkit:AccordionPane>
                    </Panes>
                </ajaxToolkit:Accordion>
            </td>
        </tr>
        <tr>
            <td align="left">
                <div class="cell" id="lblTip" runat="server" style="padding: 2px;">
                    <strong>Tip:</strong> Click the blue arrow above to expand/collapse more information.</div>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
