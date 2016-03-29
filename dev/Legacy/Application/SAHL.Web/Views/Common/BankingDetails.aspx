<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.Common.BankingDetails"  Title="Banking Details" Codebehind="BankingDetails.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

<script type="text/javascript" language="javascript">


function OnSubmitButtonClicked()
{
    var button = document.getElementById('<%=SubmitButton.ClientID %>');
    if(button.value == 'Delete')
    {
        if(confirm('Are you sure you delete this bank account?'))
            event.returnValue = true;
        else
            event.returnValue = false;
    }
}


</script> 

<table border="0" cellspacing="0" cellpadding="0" class="PageBlock" style="width: 100%" width="100%"><tr><td align="left" style="width: 99%;" valign="top">

    <SAHL:SAHLGridView ID="BankDetailsGrid" runat="server" AutoGenerateColumns="false" FixedHeader="false" 
        EnableViewState="false" GridHeight="250px" GridWidth="100%" Width="100%"
        HeaderCaption="Banking Details"
        NullDataSetMessage=""
        EmptyDataSetMessage="There are no Banking Details." 
        OnSelectedIndexChanged="BankDetailsGrid_SelectedIndexChanged" SelectFirstRow="True" PostBackType="NoneWithClientSelect">
        <RowStyle CssClass="TableRowA" Width="100%" />       
    </SAHL:SAHLGridView>
    <br />
  
    <table id="tblControls" runat="server" border="0" style="height: 61px">
        
        <tr>
            <td class="TitleText" style="width: 134px">
                Bank
            </td>
            <td colspan="1" style="width: 586px">
                <SAHL:SAHLDropDownList ID="ddlBank" runat="server" Width="49.5%" Mandatory="True" >
                </SAHL:SAHLDropDownList>
                <SAHL:SAHLLabel ID="lblBank" runat="server" Visible="false">-</SAHL:SAHLLabel>
                    </td>
            <td align="left">
                &nbsp;</td>
        </tr>
        <tr id="BranchDynamic" visible="true">
            <td style="width:134px;" class="TitleText">
                Branch 
            </td>
            <td style="width:586px;">
    <SAHL:SAHLTextBox ID="txtBranch" runat="server" Width="48.5%" Mandatory="True"></SAHL:SAHLTextBox>&nbsp;<br />
                <SAHL:SAHLAutoComplete ID="ddlBranch" runat="server"  ServiceMethod="SAHL.Web.AJAX.Bank.GetBranches"  TargetControlID ="txtBranch" Width="1514.1pc">
                    <ParentControls>
                        <SAHL:SAHLAutoCompleteParentControl ControlID="ddlBank" />
                    </ParentControls>
                </SAHL:SAHLAutoComplete>
                </td>
            <td style="width:15px;" align="left" valign="top">
                &nbsp;&nbsp;
            </td>
        </tr>
        <tr id="BranchStatic" visible="false">
            <td style="width:134px;" class="TitleText">
                Branch 
            </td>
            <td style="width:586px;">
                <SAHL:SAHLLabel ID="lblBranch" runat="server">-</SAHL:SAHLLabel>
                </td>
            <td style="width:15px;" align="left" valign="top">
                &nbsp;&nbsp;
            </td>
        </tr>        
        <tr>
            <td class="TitleText" style="width: 134px">
                Account Type
            </td>
            <td style="width: 586px">
                <SAHL:SAHLDropDownList ID="ddlAccountType" runat="server" Width="128px" Mandatory="True">
                </SAHL:SAHLDropDownList>
                <SAHL:SAHLLabel ID="lblAccountTypeBondOnly" runat="server" CssClass="LabelText" Visible="false" TextAlign="Left">Bond</SAHL:SAHLLabel>
                <SAHL:SAHLLabel ID="lblAccountType" runat="server" Visible="false">-</SAHL:SAHLLabel>
                </td>
            <td colspan="1" align="left">
                &nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td class="TitleText" style="width: 134px">
                Account Number 
            </td>
            <td style="width: 586px">
                <SAHL:SAHLTextBox ID="txtAccountNumber" runat="server" DisplayInputType="Number" MaxLength="25" Width="123px" Mandatory="True"></SAHL:SAHLTextBox>
                <SAHL:SAHLLabel ID="lblAccountNumber" runat="server" Visible="false">-</SAHL:SAHLLabel>
            </td>
            <td colspan="2">
                &nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td class="TitleText" style="width: 134px">
                Account Name 
            </td>
            <td colspan="1" style="width: 586px">
                <SAHL:SAHLTextBox ID="txtAccountName" runat="server" style="width:50%;" MaxLength="255" Height="16px" Wrap="False" Mandatory="True" Width="48.5%"></SAHL:SAHLTextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
       <tr id="ReferenceRow" runat="server" visible="false">
            <td class="TitleText" style="width: 134px">
                Reference
            </td>
            <td colspan="1" style="width: 586px">
                <SAHL:SAHLTextBox ID="txtReference" runat="server" Style="width: 50%;" MaxLength="255"
                    Height="16px" Wrap="False" Mandatory="True" Width="48.5%"></SAHL:SAHLTextBox>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
         <tr id="StatusRow" runat="server" >
            <td class="TitleText" style="width: 134px">
                <SAHL:SAHLLabel ID="lblStatus" runat="server" CssClass="LabelText"
                    Width="64px" Font-Bold="True">Status</SAHL:SAHLLabel></td>
            <td style="width: 586px">
                <SAHL:SAHLDropDownList ID="ddlStatus" runat="server" Width="176px" PleaseSelectItem="False"></SAHL:SAHLDropDownList>
            </td>
            <td colspan="1" align="left">
                &nbsp;&nbsp;
            </td>
        </tr>
    </table>

</td></tr>
<tr id="ButtonRow" runat="server"><td align="right" style="width: 99%">    <br />
    &nbsp;<SAHL:SAHLButton ID="CancelButton" runat="server" Text="Cancel" AccessKey="C" OnClick="CancelButton_Click" CausesValidation="false" />&nbsp;
    <SAHL:SAHLButton ID="SubmitButton" runat="server" Text="Submit" AccessKey="S" OnClick="SubmitButton_Click" OnClientClick = "OnSubmitButtonClicked()" />
    <SAHL:SAHLButton ID="SearchButton" runat="server" Text="Search" AccessKey="S" OnClick="SearchButton_Click"/>
    
</td>
<td>
    &nbsp;</td>
</tr></table>
</asp:Content>
