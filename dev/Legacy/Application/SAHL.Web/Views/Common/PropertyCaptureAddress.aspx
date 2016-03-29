<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true" CodeBehind="PropertyCaptureAddress.aspx.cs" Inherits="SAHL.Web.Views.Common.PropertyCaptureAddress" Title="Propery Address Details" EnableViewState="False" %>

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
     
    &nbsp;<asp:Panel ID="pnlSelectedProperty" runat="server" GroupingText="Selected Property"
        Width="99%">
        <div style="width: 100px; height: 10px">
        </div>
        <asp:Label ID="lblSelectedProperty" runat="server" Text="-" Width="99%"></asp:Label></asp:Panel>
    <asp:Panel ID="pnlAddress" runat="server" GroupingText="Property Address Details"
        Width="99%">
        <div style="width: 100px; height: 10px">
        </div>
        <cc1:AddressDetails ID="addressDetails" runat="server" ClientAddressSelectFunction="selectAddress"
            HeightSearch="300px" Orientation="Horizontal" WidthForm="375px">
        </cc1:AddressDetails>
        <div style="width: 100px; height: 10px">
        </div>
        <SAHL:SAHLButton ID="btnSelect" runat="server" ButtonSize="Size5" OnClick="btnSelect_Click"
            Text="Save Address" /></asp:Panel>
    <br />
    <table width="99%">
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td align="center" valign="bottom">
                <SAHL:SAHLButton ID="btnPrev" runat="server" OnClick="btnPrev_Click" Text="Prev" />
                <SAHL:SAHLButton ID="btnNext" runat="server" OnClick="btnNext_Click" Text="Next" /></td>
        </tr>
    </table>

</asp:Content>
