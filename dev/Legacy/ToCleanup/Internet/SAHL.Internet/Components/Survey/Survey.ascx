<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Survey.ascx.cs" Inherits="SAHL.Internet.Components.Survey.Survey" %>
<%@ Register Assembly="SAHL.Internet" Namespace="SAHL.Internet.Components.Survey" TagPrefix="sahl" %>
<div class="survey">
    <p><asp:Label ID="lblMessage" runat="server" CssClass='survey-message' /></p>
    <sahl:SahlSurvey ID="surveyControl" runat="server" />
    <asp:Button ID="bttnSubmit" OnClick="SubmitSurvey" runat="server" Text="Submit Survey" />
</div>