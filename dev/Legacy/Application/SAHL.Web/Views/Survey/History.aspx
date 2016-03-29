<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    CodeBehind="History.aspx.cs" Inherits="SAHL.Web.Views.Survey.History" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2, Version=10.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <table class="tableStandard" width="100%">
        <tr>
            <td>
                <SAHL:DXGridView ID="grdClientQuestionnaire" runat="server" AutoGenerateColumns="False"
                    PostBackType="SingleClick" Width="100%" KeyFieldName="GUID"
                    OnSelectionChanged="grdClientQuestionnaire_SelectionChanged">
                    <SettingsText Title="Completed Surveys" EmptyDataRow="No Client Surveys" />
                    <SettingsPager PageSize="5">
                    </SettingsPager>
                    <Settings ShowGroupPanel="True" />
                    <Styles>
                        <AlternatingRow Enabled="True">
                        </AlternatingRow>
                    </Styles>
                    <Border BorderWidth="2px"></Border>
                </SAHL:DXGridView>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <SAHL:SAHLSurvey ID="surveyControl" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
