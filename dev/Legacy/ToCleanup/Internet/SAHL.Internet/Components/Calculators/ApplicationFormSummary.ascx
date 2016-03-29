<%@ Import Namespace="SAHL.Internet.Components.Calculators"%>
<%@ Control Language="C#" AutoEventWireup="true" Codebehind="ApplicationFormSummary.ascx.cs" Inherits="SAHL.Internet.Components.Calculators.ApplicationFormSummary" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlWebApplicationFormSummary" runat="server" Visible="true">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td align="left">
                        <div class="inner-breadcrumb">
						<h4 class="inner-breadcrumb-heading" ><asp:Label ID="lblCalculatorName" runat="server" Text="THANK YOU" /></h4>
						<a id="A1" runat="server" class="inner-breadcrumb-next">FOR ASSISTANCE DIAL 0860 103729</a>
						</div>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                       <asp:Image BorderColor="White" BorderWidth="5px" ID="stepimage" runat="server" 
                       AlternateText="Summary - step three"></asp:Image> 
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        Your application has been successfully submitted, thank you for choosing SA Home Loans! One of our helpful consultants will contact you shortly. Please print a copy of this page for your records.
                    </td>
                </tr>
                <tr>
                    <td class="rowspacer">
                    </td>
                </tr>
                <tr>
                    <td class="rowspacer">
                        <table border="0" cellpadding="0" cellspacing="5" width="50%">
                            <tr>
                                <td colspan="2">
                                    <h2>
                                        Details of the Loan</h2>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Label ID="Label2" runat="server" Text="Your Reference Number"></asp:Label></td>
                                <td valign="top" align="right">
                                    <asp:Label ID="lblSummaryReferenceNumber" runat="server" CssClass="LabelText"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label6" runat="server" CssClass="LabelText">Loan Period (months)</asp:Label>
                                </td>
                                <td align="right">
                                    &nbsp;<asp:Label ID="lblLoanPeriod" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label8" runat="server" Text="Total Loan Amount"></asp:Label>
                                </td>
                                <td align="right">
                                    &nbsp;<asp:Label ID="lblTotalLoan" runat="server" CssClass="LabelText"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label11" runat="server" Text="My Monthly Income"></asp:Label>
                                </td>
                                <td align="right">
                                    &nbsp;<asp:Label ID="lblMonthlyIncome" runat="server" CssClass="LabelText"></asp:Label></td>
                            </tr>
                            <tr runat="server" id="rowCapitaliseFees" visible="false">
                                <td>
                                    <asp:Label ID="Label9" runat="server" Text="Capitalise Fees"></asp:Label>
                                </td>
                                <td align="right">
                                    <asp:Label ID="lblCapitaliseFees" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="rowInterestOnly" visible="false">
                                <td>
                                    <asp:Label ID="Label12" runat="server" Text="Interest Only"></asp:Label>
                                </td>
                                <td align="right">
                                    <asp:Label ID="lblInterestOnly" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="rowFixedPortionElected" visible="false">
                                <td>
                                    <asp:Label ID="Label10" runat="server" Text="Fixed Portion Elected"></asp:Label>
                                </td>
                                <td align="right">
                                    <asp:Label ID="lblIsFixedPortion" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="redline">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label7" runat="server" Text="Variable Interest Rate"></asp:Label>
                                </td>
                                <td align="right">
                                    &nbsp;<asp:Label ID="lblInterestRate" runat="server" CssClass="LabelText"></asp:Label></td>
                            </tr>
                            <tr runat="server" id="rowFixedPortion" visible="false">
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="Fixed Portion Elected"></asp:Label>
                                </td>
                                <td align="right">
                                    &nbsp;<asp:Label ID="lblFixedPortionElected" runat="server" CssClass="LabelText"></asp:Label></td>
                            </tr>
                            <tr runat="server" id="rowFixedPortionRate" visible="false">
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Fixed Interest Rate"></asp:Label>
                                </td>
                                <td align="right">
                                    &nbsp;<asp:Label ID="lblFixedInterestRate" runat="server" CssClass="LabelText"></asp:Label></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
