<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="PropertyCapture.aspx.cs" Inherits="SAHL.Web.Views.Common.PropertyCapture"
    Title="Property Details" EnableViewState="False" %>

<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

    <script language="javascript" type="text/javascript">
    
    function selectAddress(key, description)
    {
        if (confirm('Are you sure you want to use the following address?\n\n' + description))
        { 
            document.forms[0].addressKey.value = key;
            var element = document.getElementById('divMsg');
            centerElement(element, true, true);
            element.innerText = 'Please be patient while the LightStone WebService retrieves property data...';
            element.style.display = "inline"
            document.getElementById('<%=btnSelect.ClientID %>').click();
        }
    }

    function OnSearchClick()
    {
        document.forms[0].searchClicked.value = true;
    }
    
    function setPageNo()
    {
//        var p1 = document.getElementById('<%=pnlProperty.ClientID %>');
//        var p2 = document.getElementById('<%=pnlAddress.ClientID %>');
//        var p3 = document.getElementById('<%=pnlPropertyDetails.ClientID %>');
//    
// 
//        if (document.forms[0].pageNo.value == 'Property Search')
//        {   
//            document.forms[0].pageNo.value = 'Address Capture';
//        }
//        else if (document.forms[0].pageNo.value == 'Address Capture')
//        {
//            document.forms[0].pageNo.value = 'Property Details';
//        }
//        else if (document.forms[0].pageNo.value == 'Property Details') 
//        {
//        
//        }      
//        

//        document.getElementById('<%=btnNext.ClientID %>');
    }

//disable a specific validator
//function disableValidator()
//{
//var myval = document.getElememtById('validator.ClientId');
//myval.style.cssText="";
//myval.style.display='none';
//myval.style.accelerator=true;
//}


/*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-**-*-*-*-*-*/
// Changes the status of validation to [status] of all child controls of [control] if
// [control] is not null. If [control] is null, [status] is set for all validators.
/*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-**-*-*-*-*-*/
function traverseTree(status, control)
{
	if(control == null)
	{
		for(var i = 0; i < Page_Validators.length; i++)
		{
			Page_Validators[i].enabled = status;
			Page_Validators[i].style.display = status ? 'inline' : 'none';						
		}	
	}
	else	
	{	
		//this is a way to check that the control is a validation control
		if(control.evaluationfunction != null)
		{
			control.enabled = status;
			control.style.display = status ? 'inline' : 'none';		
		}
		for( var i=0; i < control.childNodes.length; i++)
		{
			traverseTree(status, control.childNodes[i]);
		}
	}
	
}

/*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-**-*-*-*-*-*/
// Looks inside the control having [containerID] as its ID for all validators and enables them.
// If [containerID] is null (not provided) then all validators are enabled.
/*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-**-*-*-*-*-*/
function enableValidators(containerID)
{
	var control;
	if(containerID == null)
	{
		control = null;					
	}
	else
	{
		control = document.getElementById(containerID);		
	}	
	if( containerID == null || control != null )
		traverseTree(true,  control);	
}

/*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-**-*-*-*-*-*/
// Looks inside the control having [containerID] as its ID for all validators and disables them.
// If [containerID] is null (not provided) then all validators are disabled.
/*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-**-*-*-*-*-*/
function disableValidators(containerID)
{
	var control;
	if(containerID == null)
	{
		control = null;					
	}
	else
	{
		control = document.getElementById(containerID);		
	}	
	if( containerID == null || control != null )
		traverseTree(false,  control);	
}
 
    </script>

    &nbsp;
    <asp:Panel ID="pnlProperty" runat="server" GroupingText="Property Search" Width="99%"
        Visible="False">
        <table style="width: 760px">
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td style="width: 200px">
                    Seller/Owner ID Number</td>
                <td style="width: 280px">
                    <SAHL:SAHLTextBox ID="txtIdentity" runat="server" Width="240px"></SAHL:SAHLTextBox>
                    </td>
                <td>
                    <SAHL:SAHLButton ID="btnSearch" runat="server" Text="Search" UseSubmitBehavior="False"
                        OnClientClick="OnSearchClick()" OnClick="btnSearch_Click" /></td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="2">
    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label></td>
            </tr>
        </table>
        <SAHL:SAHLGridView ID="PropertiesGrid" runat="server" AutoGenerateColumns="False"
            EmptyDataSetMessage="There are no Property Details." EnableViewState="false"
            FixedHeader="false" GridHeight="200px" GridWidth="100%" NullDataSetMessage=""
            Width="99%" OnRowDataBound="PropertiesGrid_RowDataBound" OnSelectedIndexChanged="PropertiesGrid_SelectedIndexChanged"
            PostBackType="SingleClick" SelectFirstRow="True">
            <RowStyle CssClass="TableRowA" />
        </SAHL:SAHLGridView>
    </asp:Panel>
    <asp:Panel ID="pnlSelectedProperty" runat="server" GroupingText="Selected Property"
        Width="99%">
        <div style="width: 100px; height: 10px">
        </div>
        <asp:Label ID="lblSelectedProperty" runat="server" Text="-" Width="99%"></asp:Label></asp:Panel>
    <asp:Panel ID="pnlComcorpOfferPropertyDetails" runat="server" GroupingText="Comcorp Property Details" Width="99%">
        <table class="tableStandard" width="100%">
            <tr>
                <td class="TitleText" style="width: 15%">Seller ID No</td>
                <td style="width: 30%">
                    <SAHL:SAHLLabel ID="lblSellerIDNo" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel> <SAHL:SAHLImageButton ID="btnCopySellerIDNo" runat="server" OnClick="btnCopySellerIDNo_Click" ImageUrl="~/Images/Paste.png" BorderStyle="Outset" BorderWidth="1px" Width="14px" ToolTip="Copy to Seller/Owner ID Number" />
                </td>
                <td class="TitleText" style="width: 15%"></td>
                <td style="width: 30%">                    
                </td>
            </tr>
            <tr>
                <td class="TitleText" style="width: 15%">Address</td>
                <td style="width: 30%" colspan="3">
                    <SAHL:SAHLLabel ID="lblComcorpPropertyAddress" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                </td>
                <td class="TitleText" style="width: 15%"></td>
                <td style="width: 30%"></td>
            </tr>
            <tr>
                <td class="TitleText" style="width: 15%">Stand Erf No</td>
                <td style="width: 30%">
                    <SAHL:SAHLLabel ID="lblStandErfNo" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                </td>
                <td class="TitleText" style="width: 15%">Contact Name</td>
                <td style="width: 30%">
                    <SAHL:SAHLLabel ID="lblContactName" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                </td>
            </tr>
            <tr>
                <td class="TitleText" style="width: 15%">Portion No</td>
                <td style="width: 30%">
                    <SAHL:SAHLLabel ID="lblPortionNo" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                </td>
                <td class="TitleText" style="width: 15%">Contact Cellphone</td>
                <td style="width: 30%">
                    <SAHL:SAHLLabel ID="lblContactCellphone" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                </td>
            </tr>
            <tr>
                <td class="TitleText" style="width: 15%">Name Property Registered</td>
                <td style="width: 30%">
                    <SAHL:SAHLLabel ID="lblNamePropertyRegistered" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                </td>
                <td class="TitleText" style="width: 15%">Property Type</td>
                <td style="width: 30%">
                    <SAHL:SAHLLabel ID="lblPropertyType" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                </td>
            </tr>
            <tr>
                <td class="TitleText" style="width: 15%">Title Type</td>
                <td style="width: 30%">
                    <SAHL:SAHLLabel ID="lblTitleType" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                </td>
                <td class="TitleText" style="width: 15%">Occupancy Type</td>
                <td style="width: 30%">
                    <SAHL:SAHLLabel ID="lblOccupancyType" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlAddress" runat="server" Width="99%" GroupingText="Property Address Details">
        <div style="width: 100px; height: 10px">
        </div>
        <cc1:AddressDetails ID="addressDetails" runat="server" ClientAddressSelectFunction="selectAddress"
            Orientation="Horizontal" HeightSearch="300px" WidthForm="475px">
        </cc1:AddressDetails>
        <div style="width: 100px; height: 10px">
        </div>
        <SAHL:SAHLButton ID="btnSelect" runat="server" OnClick="btnSelect_Click" Text="Save Address"
            ButtonSize="Size5" /></asp:Panel>
    <asp:Panel ID="pnlPropertyDetails" runat="server" GroupingText="Property Details"
        Visible="False" Width="99%">
        <table id="tblPropertyDetails">
            <tr>
                <td style="width: 180px">
                </td>
                <td colspan="2" style="width: 402px">
                </td>
            </tr>
            <tr>
                <td>
                    Property Description 1</td>
                <td colspan="2">
                    <SAHL:SAHLTextBox ID="txtPropertyDescription1" runat="server" Style="width:395px" Mandatory="True" MaxLength="250"></SAHL:SAHLTextBox></td>
            </tr>
            <tr>
                <td>
                    Property Description 2</td>
                <td colspan="2">
                    <SAHL:SAHLTextBox ID="txtPropertyDescription2" runat="server" Style="width:395px" Mandatory="True" MaxLength="250"></SAHL:SAHLTextBox></td>
            </tr>
            <tr>
                <td>
                    Property Description 3</td>
                <td colspan="2">
                    <SAHL:SAHLTextBox ID="txtPropertyDescription3" runat="server" Style="width:395px" Mandatory="True" MaxLength="250"></SAHL:SAHLTextBox></td>
            </tr>
            <tr>
                <td>
                    Property Type</td>
                <td colspan="2">
                    <SAHL:SAHLDropDownList ID="ddlPropertyType" runat="server" Style="width: 100%" Width="98%" Mandatory="True" PleaseSelectItem="False">
                    </SAHL:SAHLDropDownList></td>
            </tr>
            <tr>
                <td>
                    Title Type</td>
                <td colspan="2">
                    <SAHL:SAHLDropDownList ID="ddlTitleType" runat="server" Style="width: 100%" Width="98%" Mandatory="True" PleaseSelectItem="False">
                    </SAHL:SAHLDropDownList></td>
            </tr>
            <tr>
                <td>
                    Occupancy Type</td>
                <td colspan="2">
                    <SAHL:SAHLDropDownList ID="ddlOccupancyType" runat="server" Style="width: 100%" Width="98%" Mandatory="True" PleaseSelectItem="true">
                    </SAHL:SAHLDropDownList></td>
            </tr>
            <tr>
                <td>
                    Inspection Contact 1</td>
                <td colspan="2">
                    <SAHL:SAHLTextBox ID="txtInspectionContact1" runat="server" Width="98%" MaxLength="50" Mandatory="True"></SAHL:SAHLTextBox></td>
            </tr>
            <tr>
                <td>
                    Inspection Contact 1 Tel.</td>
                <td colspan="2">
                    <SAHL:SAHLTextBox ID="txtInspectionTel1" runat="server" Width="98%" MaxLength="50" Mandatory="True"></SAHL:SAHLTextBox></td>
            </tr>
            <tr>
                <td>
                    Inspection Contact 2</td>
                <td colspan="2">
                    <SAHL:SAHLTextBox ID="txtInspectionContact2" runat="server" MaxLength="50" Width="98%"></SAHL:SAHLTextBox></td>
            </tr>
            <tr>
                <td>
                    Inspection Contact 2 Tel.</td>
                <td colspan="2">
                    <SAHL:SAHLTextBox ID="txtInspectionTel2" runat="server" MaxLength="50" Width="98%"></SAHL:SAHLTextBox></td>
            </tr>
        </table>
    </asp:Panel>
    &nbsp;<br />
    &nbsp;
    <div id="divMsg" style="vertical-align: middle; position: absolute; background-color: white;
        text-align: center; height: 23px; display: none; border-top-width: thin; border-left-width: thin;
        border-left-color: gray; border-bottom-width: thin; border-bottom-color: gray;
        overflow: visible; color: red; border-top-color: gray; border-right-width: thin;
        border-right-color: gray;">
    </div>
    <br />
    <table width="99%">
        <tr>
            <td>
            </td>
        </tr>
        <tr id="ButtonRow" runat="server">
            <td align="center" valign="bottom">
                <SAHL:SAHLButton ID="btnPrev" runat="server" OnClick="btnPrev_Click" Text="Prev" Visible="False" />
                <SAHL:SAHLButton ID="btnNext" runat="server" OnClick="btnNext_Click" Text="Next" /></td>
        </tr>
    </table>
    <br />
    <input type="hidden" name="addressKey" value="" /><br />
    <input type="hidden" name="searchClicked" value="" /><br />

</asp:Content>
