<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    CodeBehind="Capture.aspx.cs" Inherits="SAHL.Web.Views.Survey.Capture" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>

	
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">

<script type="text/javascript" language="javascript">


	function OnSubmitButtonClicked() {			
		if (confirm('Are you sure you want to send the survey?'))
			event.returnValue = true;
		else
			event.returnValue = false;
		
	}


</script> 
	<table class="tableStandard" width="100%">
        <tr>
            <td>
                <SAHL:DXGridView ID="grdSurvey" runat="server" AutoGenerateColumns="False" Width="100%"
                    EnableViewState="false" KeyFieldName="GUID">
                    <SettingsText Title="Available Questionnaires" />
                </SAHL:DXGridView>
            </td>
        </tr>
        <tr>
            <td>
                Please select the legal entity that is completing the questionnaire:
                <SAHL:SAHLDropDownList runat="server" ID="ddlLegalEntity" PleaseSelectItem="true" />
            </td>
        </tr>
        <tr>
            <td>
                <SAHL:SAHLButton ID="btnDoSurvey" runat="server" OnClick="btnDoSurvey_Click" Text="Do Survey" />
                <SAHL:SAHLButton ID="btnSendSurvey" runat="server" OnClick="btnSendSurvey_Click" Text="Send Survey"  OnClientClick = "OnSubmitButtonClicked()" />
            </td>
        </tr>
        <tr>
            <td>
                <SAHL:SAHLSurvey ID="surveyControl" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <SAHL:SAHLButton ID="btnSaveSurvey" runat="server" OnClick="btnSaveSurvey_Click"
                    Text="Survey Completed" />
                <SAHL:SAHLButton ID="btnCancelSurvey" runat="server" OnClick="btnCancelSurvey_Click"
                    Text="Cancel" />
            </td>
        </tr>
    </table>
</asp:Content>
