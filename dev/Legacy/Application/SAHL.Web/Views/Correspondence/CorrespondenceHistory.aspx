<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    CodeBehind="CorrespondenceHistory.aspx.cs" Inherits="SAHL.Web.Views.Correspondence.CorrespondenceHistory"
    Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Src="../Life/WorkFlowHeader.ascx" TagName="WorkFlowHeader" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx1" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v10.2, Version=10.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <script type="text/javascript">
    // <![CDATA[
        var keyValue;
        function OnMoreInfoClick(element, key) {
            callbackPanel.SetContentHtml("");
            popup.ShowAtElement(element);
            keyValue = key;
        }
        function popup_Shown(s, e) {
            callbackPanel.PerformCallback(keyValue);
        }
    // ]]> 
    </script>
    <SAHL:DXPopupControl ID="popup" ClientInstanceName="popup" runat="server" PopupHorizontalAlign="WindowCenter"
        HeaderText="Details">
        <ContentCollection>
            <dx1:PopupControlContentControl runat="server">
                <SAHL:DXCallbackPanel ID="callbackPanel" ClientInstanceName="callbackPanel" runat="server"
                    Width="700px" Height="400px" OnCallback="callbackPanel_Callback" RenderMode="Table">
                    <PanelCollection>
                        <SAHL:DXPanelContent runat="server">
                            <table>
                                <tr>
                                    <td>
                                        <SAHL:DXHtmlEditor ID="CorrespondenceDetailEditor" runat="server">
                                            <Settings AllowHtmlView="false" AllowPreview="true" AllowDesignView="false" />
                                            <SettingsImageUpload UploadImageFolder="" />
                                        </SAHL:DXHtmlEditor>
                                    </td>
                                </tr>
                            </table>
                        </SAHL:DXPanelContent>
                    </PanelCollection>
                </SAHL:DXCallbackPanel>
            </dx1:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents Shown="popup_Shown" />
    </SAHL:DXPopupControl>
    <uc1:WorkFlowHeader ID="WorkFlowHeader1" runat="server" />
    <div>
        <table width="100%">
            <tr>
                <td>
                    <SAHL:DXGridView ID="gridHistory" PostBackType="None"  runat="server" KeyFieldName="Key" AutoGenerateColumns="false"
                        Width="100%" OnHtmlDataCellPrepared="gridHistory_HtmlDataCellPrepared">
                        <Settings ShowGroupPanel="true" />
                        <SettingsBehavior AllowGroup="true"/>
                        <SettingsPager PageSize="15" />
                        <Columns>
                            <SAHL:DXGridViewFormattedTextColumn FieldName="DateActioned" Caption="Date Actioned" VisibleIndex="0">
                            </SAHL:DXGridViewFormattedTextColumn>
                            <SAHL:DXGridViewFormattedTextColumn FieldName="DateSent" Caption="Date Sent" VisibleIndex="1">
                            </SAHL:DXGridViewFormattedTextColumn>
                            <SAHL:DXGridViewFormattedTextColumn FieldName="Document" Caption="Document" VisibleIndex="2">
                            </SAHL:DXGridViewFormattedTextColumn>
                            <SAHL:DXGridViewFormattedTextColumn FieldName="Recipient" Caption="Recipient" VisibleIndex="3">
                            </SAHL:DXGridViewFormattedTextColumn>
                            <SAHL:DXGridViewFormattedTextColumn FieldName="SentTo" Caption="Sent To" VisibleIndex="4">
                            </SAHL:DXGridViewFormattedTextColumn>
                            <SAHL:DXGridViewFormattedTextColumn FieldName="User" Caption="User" VisibleIndex="5">
                            </SAHL:DXGridViewFormattedTextColumn>
                            <SAHL:DXGridViewFormattedTextColumn CellStyle-HorizontalAlign="Center" FieldName="" Caption="Details" VisibleIndex="6">
                                <DataItemTemplate>
                                    <img id="imgCorrespondenceDetails" style="text-align:center;cursor:pointer;" onclick="OnMoreInfoClick(this, '<%# Container.KeyValue %>')" alt="View Correspondence Details" src="../../Images/find.png" />
                                </DataItemTemplate>
                            </SAHL:DXGridViewFormattedTextColumn>
                        </Columns>
                    </SAHL:DXGridView>
                </td>
            </tr>
            <tr id="ButtonRow" runat="server">
                <td align="center">
                    <SAHL:SAHLButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel"
                        CausesValidation="False" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
