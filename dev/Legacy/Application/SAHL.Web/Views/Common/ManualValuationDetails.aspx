<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.Common.ValuationDetails" Title="Valuation Details" Codebehind="ManualValuationDetails.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<table width="100%" class="tableStandard">
<tr><td align="left" style="height:99%; width: 100%;" valign="top">

    <SAHL:SAHLGridView ID="gridProperty" runat="server" AutoGenerateColumns="false" FixedHeader="false" 
        EnableViewState="false" GridHeight="100px"  GridWidth="100%"  Width="100%"
        HeaderCaption="Property"
        PostBackType="SingleClick" 
        NullDataSetMessage=""
        EmptyDataSetMessage="There are no Properties.">
        <RowStyle CssClass="TableRowA" />
    </SAHL:SAHLGridView>
    
    <br />
    
    <SAHL:SAHLGridView ID="gridValuations" runat="server" AutoGenerateColumns="false" FixedHeader="false" 
        EnableViewState="false" GridHeight="150px" GridWidth="100%"  Width="100%"
        HeaderCaption="Valuations"
        NullDataSetMessage="There are no Manual Valuations."
        EmptyDataSetMessage="There are no Manual Valuations." OnSelectedIndexChanged="gridValuations_SelectedIndexChanged" PostBackType="SingleClick">
        <RowStyle CssClass="TableRowA" />
    </SAHL:SAHLGridView>
    <br />
    
    <table border="0" id="DisplayData" runat="server">
        <tr>
            <td valign="top">
            
                <table border="0">
                    <tr>
                        <td style="width:131px;" class="TitleText">
                            Valuer
                        </td>
                        <td style="width:211px;">
                            <SAHL:SAHLLabel ID="lblValuer" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel>
                            <SAHL:SAHLDropDownList ID="ddlValuer" runat="server" style="width:98%;" OnSelectedIndexChanged="ddlValuer_SelectedIndexChanged" OnDataBound="ddlValuer_DataBound">
                            </SAHL:SAHLDropDownList>
                        </td>
                        <td style="width:15px;">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TitleText">
                            Valuation Amount
                        </td>
                        <td>
                            <SAHL:SAHLLabel ID="lblValuationAmount" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel>
                            <SAHL:SAHLCurrencyBox ID="txtValuationAmount" runat="server" style="width:80%;"></SAHL:SAHLCurrencyBox>
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="TitleText">
                            Valuation Date
                        </td>
                        <td>
                            <SAHL:SAHLLabel ID="lblValuationDate" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel>
                            <SAHL:SAHLInputDate ID="dateValuationDate" runat="server" />
                        </td>
                        <td>
                            <SAHL:SAHLTextBox ID="ValValuationDateUpdateControl" runat="server" style="display:none;"></SAHL:SAHLTextBox>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="TitleText">
                            Municipal Valuation
                        </td>
                        <td>
                            <SAHL:SAHLLabel ID="lblMunicipalValuation" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel>
                            <SAHL:SAHLCurrencyBox ID="txtMunicipalValuation" runat="server"  style="width:80%;"></SAHL:SAHLCurrencyBox>
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            
            </td>
            <td valign="top">
            
                <asp:Panel ID="HOCDetails" runat="server" GroupingText="HOC Details">
                    <table border="0">
                        <tr>
                            <td style="width:171px;" class="TitleText">
                                Roof Description
                            </td>
                            <td style="width:181px;">
                                <SAHL:SAHLLabel ID="lblHOCRoofDescription" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel>
                                <SAHL:SAHLDropDownList ID="ddlHOCRoofDescription" runat="server" style="width:98%;"  OnSelectedIndexChanged="ddlHOCRoofDescription_SelectedIndexChange" AutoPostBack="True">
                                </SAHL:SAHLDropDownList>
                            </td>
                            <td style="width:15px;">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="TitleText">
                                Conventional Valuation
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="lblHOCConventionalAmount" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel>
                                <SAHL:SAHLCurrencyBox ID="txtHOCConventionalAmount" runat="server" style="width:80%;"></SAHL:SAHLCurrencyBox>
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="TitleText">
                                HOC Thatch Amount
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="lblHOCThatchAmount" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel>
                                <SAHL:SAHLCurrencyBox ID="txtHOCThatchAmount" runat="server" style="width:80%;"></SAHL:SAHLCurrencyBox>
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                       
                        <tr>
                            <td class="TitleText">
                                HOC Valuation Amount
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="lblHOCValuationAmount" runat="server" CssClass="LabelText" TextAlign="Left">-</SAHL:SAHLLabel>
                                <SAHL:SAHLCurrencyBox ID="txtHOCValuationAmount" runat="server" 
                                    Style="width: 80%" Visible="false"></SAHL:SAHLCurrencyBox></td>
                            <td>
                                &nbsp;<SAHL:SAHLTextBox ID="Dummy" runat="server" style="visibility:hidden;" Columns="1"></SAHL:SAHLTextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            
            </td>
        </tr>
    </table>
    <br />

</td></tr>
<tr id="ButtonRow" runat="server"><td align="right">
    <SAHL:SAHLButton ID="BtnValuationDetailsDisplay" runat="server" Text="Valuation Details" AccessKey="V" OnClick="ValuationDetailsDisplay_Click" ButtonSize="Size5" />
    <SAHL:SAHLButton ID="CancelButton" runat="server" Text="Cancel" AccessKey="C" OnClick="CancelButton_Click" CausesValidation="False" />
    <SAHL:SAHLButton ID="BackButton" runat="server" Text="Back" AccessKey="S" OnClick="BackButton_Click" />
    <SAHL:SAHLButton ID="SubmitButton" runat="server" Text="Add" AccessKey="S" OnClick="SubmitButton_Click" />
</td></tr></table>
</asp:Content>
