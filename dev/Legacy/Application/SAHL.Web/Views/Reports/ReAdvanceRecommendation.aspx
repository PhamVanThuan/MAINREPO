<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true" CodeBehind="ReAdvanceRecommendation.aspx.cs" Inherits="SAHL.Web.Views.Reports.ReAdvanceRecommendation" Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">    
    <table border="0" cellpadding="0" cellspacing="0" class="PageBlock">
        <tr>
            <td align="left" style="height: 99%" valign="top">
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Panel ID="pnlParameters" runat="server" GroupingText="Report Parameters" Width="99%">
                                <table id="Table1" runat="server" border="0" width="99%">
                                    <tr>
                                        <td class="TitleText" style="width: 100px">
                                            Account Number</td>
                                        <td style="width: 38px">
                                            <sahl:sahltextbox id="txtAccountNumber" runat="server" displayinputtype="Number"
                                                validationgroup="AccountNumber"></sahl:sahltextbox>
                                            &nbsp;
                                        </td>
                                        <td style="width: 15px">
                                            <sahl:sahlbutton id="NextButton" runat="server" accesskey="N" onclick="NextButton_Click"
                                                text="Next" validationgroup="AccountNumber"></sahl:sahlbutton>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="TitleText" style="width: 100px">
                                            Link Rate</td>
                                        <td style="width: 38px">
                                            <sahl:sahldropdownlist id="ddlLinkRate" runat="server" validationgroup="Params"></sahl:sahldropdownlist>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                        <td style="width: 15px">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="TitleText" style="width: 100px">
                                            Readvance Amount</td>
                                        <td style="width: 38px">
                                            <sahl:sahltextbox id="txtReadvance" runat="server" displayinputtype="Number" maxlength="15"
                                                validationgroup="Params"></sahl:sahltextbox>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                        <td style="width: 15px">
                                            &nbsp;</td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table id="Report" runat="server" style="width: 100%" visible="false">
                                <tr>
                                    <td>
                                        <iframe id="ReportViewerFrame" runat="server" frameborder="1" height="470" marginheight="0"
                                            marginwidth="0" scrolling="auto" src="" width="100%"></iframe>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="ButtonRow" runat="server">
            <td align="right" style="width: 783px">
                <sahl:sahlbutton id="CancelButton" runat="server" accesskey="C" causesvalidation="false"
                    onclick="CancelButton_Click" text="Cancel"></sahl:sahlbutton>
                &nbsp;
                <sahl:sahlbutton id="SAHLButton" runat="server" accesskey="S" causesvalidation="true"
                    onclick="SubmitButton_Click" text="Submit" validationgroup="Params"></sahl:sahlbutton>&nbsp;
                <sahl:sahltextbox id="SubmitValuation" runat="server" style="display: none"></sahl:sahltextbox>
            </td>
        </tr>
    </table>
</asp:Content>
