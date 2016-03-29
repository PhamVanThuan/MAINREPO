<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true" CodeBehind="SubsidyProvider.aspx.cs" Inherits="SAHL.Web.Views.Administration.SubsidyProvider" Title="Untitled Page" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
 <script language="javascript" type="text/javascript">
    
     function selectAddress(key, description)
    {
        if (confirm('Are you sure you want to add the following address?\n\n' + description))
        { 
            document.forms[0].addressKey.value = key;
            document.getElementById('<%=btnAdd.ClientID %>').click();
        }
    }
    </script>
    <style type="text/css">
    .col1 { width: 150px; }
    .col2 { width: 200px; }
    .col3 { width: 200px; }
    </style>
<asp:Panel ID="pnlSubsidyProviderDetails" runat="server" Width="98%" GroupingText="Subsidy Provider Details" Visible="true">     
<br />
<div class="row">
    <div class="cellInput TitleText col1">Subsidy Provider&nbsp;</div>
    <div class="cellInput col2">
        <SAHL:SAHLTextBox ID="txtSubsidyProvider" runat="server" Width="180px" Mandatory="true" ></SAHL:SAHLTextBox> 
        <SAHL:SAHLAutoComplete ID="acSubsidyProvider" runat="server" ServiceMethod="SAHL.Web.AJAX.Employment.GetSubsidyProviders" TargetControlID="txtSubsidyProvider" AutoPostBack="true"></SAHL:SAHLAutoComplete> 
    </div> 
    <div class="cellInput TitleText col3">Subsidy Type&nbsp;</div> 
    <div class="cellInput">
         <SAHL:SAHLLabel ID="lblSubsidyType" runat="server">-</SAHL:SAHLLabel>  
        <SAHL:SAHLDropDownList ID="ddlSubsidyType" runat="server" Mandatory="true" PleaseSelectItem="true"></SAHL:SAHLDropDownList>
    </div>
</div>
<div class="row">
    <div class="cellInput TitleText col1">Contact Person&nbsp;</div>
    <div class="cellInput col2">
        <SAHL:SAHLLabel ID="lblContactPerson" runat="server">-</SAHL:SAHLLabel>
        <SAHL:SAHLTextBox ID="txtContactPersonEdit" runat="server" Width="180px" Mandatory="true"></SAHL:SAHLTextBox>
    </div>
    <div class="cellInput TitleText col3">Phone Code & Number&nbsp;</div>
    <div class="cellInput">
        <SAHL:SAHLLabel ID="lblPhoneNumber" runat="server">-</SAHL:SAHLLabel>
        <SAHL:SAHLPhone ID="txtPhone" runat="server" CssClass="mandatory" />
    </div>
</div>
<div class="row">
     <div class="cellInput TitleText col1">Email Address&nbsp;</div>
     <div class="cellInput col2">
        <SAHL:SAHLLabel ID="lblEmailAddress" runat="server">-</SAHL:SAHLLabel>
        <SAHL:SAHLTextBox ID="txtEmailAddressEdit" runat="server" MaxLength="50" Width="180px"></SAHL:SAHLTextBox>
    </div>
    <div class="cellInput TitleText col3">Persal Organisation Code&nbsp;</div>
    <div class="cellInput">
        <SAHL:SAHLLabel ID="lblPersalOrganisationCode" runat="server" Enabled="False">-</SAHL:SAHLLabel>
    </div>
</div>
</asp:Panel>
<br />
<asp:Panel ID="pnlAddress" runat="server" Width="98%" GroupingText="Subsidy Provider Address" Visible="true">     
<br />
<div class="row">
<div class="row">
    <div class="cellInput TitleText col1">Address Type&nbsp;</div>
     <div class="cellInput col2">
        <SAHL:SAHLLabel ID="lblAddressType" runat="server">-</SAHL:SAHLLabel>
        <SAHL:SAHLDropDownList ID="ddlAddressType" runat="server" AutoPostBack="True"></SAHL:SAHLDropDownList>                
    </div>    
    <div class="cellInput TitleText col3">Address Format&nbsp;</div>
     <div class="cellInput">
        <SAHL:SAHLLabel ID="lblAddressFormat" runat="server">-</SAHL:SAHLLabel>
        <SAHL:SAHLDropDownList ID="ddlAddressFormat" runat="server" AutoPostBack="True"></SAHL:SAHLDropDownList>                
    </div>
</div>
<div class="borderBottom row">
    <div class="cellInput TitleText col1"> Effective Date&nbsp;</div>
    <div class="cellInput col2">
        <SAHL:SAHLLabel ID="lblEffectiveDate" runat="server" >-</SAHL:SAHLLabel>
        <SAHL:SAHLDateBox ID="dtEffectiveDate" runat="server" ></SAHL:SAHLDateBox> 
    </div>
</div>
<div>
<br />
</div>
<SAHL:SAHLLabel ID="lblAddress" runat="server" CssClass="LabelText" Height="46px" Width="192px" >-</SAHL:SAHLLabel>
 <% // input form fields %>
            <br />
            <cc1:AddressDetails ID="addressDetails" runat="server" ClientAddressSelectFunction="selectAddress">
            </cc1:AddressDetails>
</div>
</asp:Panel>
<div class="buttonBar" style="width:99%">
<SAHL:SAHLButton ID="CancelButton" runat="server" Text="Cancel" AccessKey="C" OnClick="CancelButton_Click" CausesValidation="false" />
<SAHL:SAHLButton ID="SubmitButton" runat="server" Text="Submit" AccessKey="S" OnClick="SubmitButton_Click"/>
</div>
    <input type="hidden" name="addressKey" value=""/>
    <asp:Button ID="btnAdd" runat="server" Text="" Visible="True" Width="0px" Height="0px"/>
</asp:Content>
