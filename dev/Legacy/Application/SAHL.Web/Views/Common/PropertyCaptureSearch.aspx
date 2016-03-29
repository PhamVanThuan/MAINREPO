<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PropertyCaptureSearch.aspx.cs" Inherits="SAHL.Web.Views.Common.PropertyCaptureSearch" 
    MasterPageFile="~/MasterPages/Blank.Master" Title="Property Details" EnableViewState="False" %>

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
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtIdentity"
                        ErrorMessage="*"></asp:RequiredFieldValidator></td>
                <td>
                    <SAHL:SAHLButton ID="btnSearch" runat="server" Text="Search" UseSubmitBehavior="False"
                        OnClientClick="OnSearchClick()" OnClick="btnSearch_Click" /></td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
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
        <tr>
            <td align="center" valign="bottom">
                <SAHL:SAHLButton ID="btnPrev" runat="server" OnClick="btnPrev_Click" Text="Prev" />
                <SAHL:SAHLButton ID="btnNext" runat="server" OnClick="btnNext_Click" Text="Next" /></td>
        </tr>
    </table>
    <br />
    <input type="hidden" name="addressKey" value="" /><br />
    <input type="hidden" name="searchClicked" value="" /><br />

</asp:Content>