<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="ProductManagement.aspx.cs" Inherits="SAHL.Web.Views.Administration.ProductManagement"
    Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">


    <table class="tableStandard" width="100%">
        <tr>
            <td style="width: 50px" class="TitleText" align="left">
                <SAHL:SAHLLabel ID="lblOriginationSource" runat="server" Text="Company"></SAHL:SAHLLabel>
            </td>
            <td style="width: 150px" class="TitleText" align="left">
                <SAHL:SAHLDropDownList ID="ddlOriginationSource" runat="server" Width="200px" AutoPostBack="True"
                    CssClass="CboText" OnSelectedIndexChanged="ddlOriginationSource_SelectedIndexChanged"
                    PleaseSelectItem="False">
                </SAHL:SAHLDropDownList>
            </td>
            <td style="width: 50px" class="TitleText" align="left">
                <SAHL:SAHLLabel ID="lblProduct" runat="server" Text="Product"></SAHL:SAHLLabel>
            </td>
            <td style="width: 250px" class="TitleText" align="left">
                <SAHL:SAHLDropDownList ID="ddlProduct" runat="server" Width="200px" AutoPostBack="True"
                    CssClass="CboText" OnSelectedIndexChanged="ddlProduct_SelectedIndexChanged">
                </SAHL:SAHLDropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="4" align="left">
                <ajaxToolkit:Accordion ID="accProductManagement" runat="server" SelectedIndex="-1"
                    HeaderCssClass="accordionHeader" ContentCssClass="" FadeTransitions="False"
                    FramesPerSecond="40" TransitionDuration="250" AutoSize="Fill" SuppressHeaderPostbacks="true" 
                    HeaderSelectedCssClass="accordionHeaderSelected">
                    <Panes>
                        <ajaxToolkit:AccordionPane ID="apCreditCriteria" runat="server">
                            <Header>
                                <a href="">Credit Criteria</a>
                            </Header>
                            <Content>
                                <SAHL:SAHLGridView ID="CreditCriteriaGrid" runat="server" AutoGenerateColumns="false"
                                    EmptyDataSetMessage="No credit criteria exists for this Company/Product." 
                                    EmptyDataText="No credit criteria exists for this Company/Product."
                                    EnableViewState="false" FixedHeader="false" 
                                    GridHeight="400px" GridWidth="100%" Width="100%" 
                                    HeaderCaption="" NullDataSetMessage="" 
                                    PostBackType="NoneWithClientSelect"
                                    OnRowDataBound="CreditCriteriaGrid_RowDataBound"
                                    OnSelectedIndexChanged="CreditCriteriaGrid_SelectedIndexChanged"
                                    OnDataBound="CreditCriteriaGrid_DataBound" 
                                    SelectFirstRow="False">
                                    <HeaderStyle CssClass="TableHeaderB" />
                                    <RowStyle CssClass="TableRowA" />
                                </SAHL:SAHLGridView>
                            </Content>
                        </ajaxToolkit:AccordionPane>
                        <ajaxToolkit:AccordionPane ID="apLinkedProducts" runat="server">
                            <Header>
                                <a href="">Linked Products (Checked Products are linked to this Credit Matrix)</a>
                            </Header>
                            <Content>
                                <SAHL:SAHLCheckboxList ID="chklLinkedOSP" runat="server" RepeatColumns="3" OnDataBound="chklLinkedOSP_DataBound"
                                    Width="100%" Height="350px" CausesValidation="True">
                                </SAHL:SAHLCheckboxList>
                            </Content>
                        </ajaxToolkit:AccordionPane>
                    </Panes>
                    <HeaderTemplate>
                    </HeaderTemplate>
                </ajaxToolkit:Accordion>
            </td>
        </tr>
        <tr>
            <td align="left" colspan="4">
            <br />
                <SAHL:SAHLButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" ButtonSize="Size4" Text="Submit" />&nbsp;
                <SAHL:SAHLButton ID="btnLoad" runat="server" OnClick="btnLoad_Click" ButtonSize="Size4"
                    Text="Load Data" />
            </td>
        </tr>
    </table>
    <ajaxToolkit:Accordion ID="accApplicationSummary" runat="server" SelectedIndex="-1"
        HeaderCssClass="accordianHeader" HeaderSelectedCssClass="accordianHeaderSelected"
        ContentCssClass="" FadeTransitions="false" FramesPerSecond="40" TransitionDuration="250"
        AutoSize="None" SuppressHeaderPostbacks="true">
        <Panes>
        </Panes>
    </ajaxToolkit:Accordion>
</asp:Content>
