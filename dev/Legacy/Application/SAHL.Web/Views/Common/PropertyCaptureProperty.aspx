<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true" CodeBehind="PropertyCaptureProperty.aspx.cs" Inherits="SAHL.Web.Views.Common.PropertyCaptureProperty" Title="Property Details" EnableViewState="False" %>

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
     
    <asp:Panel ID="pnlPropertyDetails" runat="server" GroupingText="Property Details"
        Visible="False" Width="99%">
        <table id="tblPropertyDetails">
            <tr>
                <td style="width: 180px">
                </td>
                <td colspan="2" style="width: 402px">
                </td>
                <td colspan="1" style="width: 20px">
                </td>
            </tr>
            <tr>
                <td>
                    Property Description 1</td>
                <td colspan="2">
                    <SAHL:SAHLTextBox ID="txtPropertyDescription1" runat="server" Width="98%"></SAHL:SAHLTextBox></td>
                <td colspan="1">
                    <SAHL:SAHLRequiredFieldValidator ID="valPropertyDesc1" runat="server" ControlToValidate="txtPropertyDescription1"
                        Enabled="False" ErrorMessage="All Property Descriptions must be entered " />
                </td>
            </tr>
            <tr>
                <td>
                    Property Description 2</td>
                <td colspan="2">
                    <SAHL:SAHLTextBox ID="txtPropertyDescription2" runat="server" Width="98%"></SAHL:SAHLTextBox></td>
                <td colspan="1">
                    <SAHL:SAHLRequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPropertyDescription2"
                        Enabled="False" ErrorMessage="All Property Descriptions must be entered " />
                </td>
            </tr>
            <tr>
                <td>
                    Property Description 3</td>
                <td colspan="2">
                    <SAHL:SAHLTextBox ID="txtPropertyDescription3" runat="server" Width="98%"></SAHL:SAHLTextBox></td>
                <td colspan="1">
                    <SAHL:SAHLRequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtPropertyDescription3"
                        Enabled="False" ErrorMessage="All Property Descriptions must be entered " />
                </td>
            </tr>
            <tr>
                <td>
                    Property Type</td>
                <td colspan="2">
                    <SAHL:SAHLDropDownList ID="ddlPropertyType" runat="server" Style="width: 100%" Width="98%">
                    </SAHL:SAHLDropDownList></td>
                <td colspan="1">
                    <SAHL:SAHLRequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlPropertyType"
                        Enabled="False" ErrorMessage="Property Type must be selected" InitialValue="-select-" />
                </td>
            </tr>
            <tr>
                <td>
                    Title Type</td>
                <td colspan="2">
                    <SAHL:SAHLDropDownList ID="ddlTitleType" runat="server" Style="width: 100%" Width="98%">
                    </SAHL:SAHLDropDownList></td>
                <td colspan="1">
                    <SAHL:SAHLRequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlTitleType"
                        Enabled="False" ErrorMessage="Title Type must be selected" InitialValue="-select-" />
                </td>
            </tr>
            <tr>
                <td>
                    Occupancy Type</td>
                <td colspan="2">
                    <SAHL:SAHLDropDownList ID="ddlOccupancyType" runat="server" Style="width: 100%" Width="98%">
                    </SAHL:SAHLDropDownList></td>
                <td colspan="1">
                    <SAHL:SAHLRequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlOccupancyType"
                        Enabled="False" ErrorMessage="Occupancy Type must be selected" InitialValue="-select-" />
                </td>
            </tr>
            <tr>
                <td>
                    Inspection Contact 1</td>
                <td colspan="2">
                    <SAHL:SAHLTextBox ID="txtInspectionContact1" runat="server" MaxLength="50" Width="98%"></SAHL:SAHLTextBox></td>
                <td colspan="1">
                    <SAHL:SAHLRequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtInspectionContact1"
                        Enabled="False" ErrorMessage="Inspection Contact must be entered " />
                </td>
            </tr>
            <tr>
                <td>
                    Inspection Contact 1 Tel.</td>
                <td colspan="2">
                    <SAHL:SAHLTextBox ID="txtInspectionTel1" runat="server" MaxLength="50" Width="98%"></SAHL:SAHLTextBox></td>
                <td colspan="1">
                    <SAHL:SAHLRequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtInspectionTel1"
                        Enabled="False" ErrorMessage="Inspection Telephone must be entered " />
                </td>
            </tr>
            <tr>
                <td>
                    Inspection Contact 2</td>
                <td colspan="2">
                    <SAHL:SAHLTextBox ID="txtInspectionContact2" runat="server" MaxLength="50" Width="98%"></SAHL:SAHLTextBox></td>
                <td colspan="1">
                </td>
            </tr>
            <tr>
                <td>
                    Inspection Contact 2 Tel.</td>
                <td colspan="2">
                    <SAHL:SAHLTextBox ID="txtInspectionTel2" runat="server" MaxLength="50" Width="98%"></SAHL:SAHLTextBox></td>
                <td colspan="1">
                </td>
            </tr>
        </table>
    </asp:Panel>
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
