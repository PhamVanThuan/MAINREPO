<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Blank.Master"
    CodeBehind="CourtDetails.aspx.cs" Inherits="SAHL.Web.Views.DebtCounselling.CourtDetails" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <script src="../../Scripts/jquery-1.5.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            if ($("#<%= ddlHearingType.ClientID %>").val() == 2) {
                $("#<%= trCourt.ClientID %>").hide();
            }
            else {
                $("#<%= trCourt.ClientID %>").show();
            }
        });

        function HearingTypeChanged() {
            if ($("#<%= ddlHearingType.ClientID %>").val() == 2) {
                $("#<%= trCourt.ClientID %>").hide();
            }
            else {
                $("#<%= trCourt.ClientID %>").show();
            }
        }

        // Comment popup window stuffs
        var keyValue;
        function OnMoreInfoClick(element, key) {
            callbackPanel.SetContentHtml("");
            popup.ShowAtElement(element);
            keyValue = key;
        }

        function popup_Shown(s, e) {
            callbackPanel.PerformCallback(keyValue);
        }
    </script>
    <SAHL:DXPopupControl ID="popup" ClientInstanceName="popup" runat="server" PopupHorizontalAlign="WindowCenter"
        HeaderText="Comment">
        <ContentCollection>
            <dx1:PopupControlContentControl runat="server">
                <SAHL:DXCallbackPanel ID="callbackPanel" ClientInstanceName="callbackPanel" runat="server"
                    Width="700px" Height="400px" OnCallback="imgOnCommentClick" RenderMode="Table">
                    <PanelCollection>
                        <SAHL:DXPanelContent runat="server">
                            <table>
                                <tr>
                                    <td>
                                        <SAHL:SAHLTextBox ID="txtCommentEditor" runat="server" TextMode="MultiLine" Width="680px"
                                            Height="350px">
                                        </SAHL:SAHLTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <SAHL:SAHLLabel ID="lblCommentMessage" runat="server" Text="Note: The comments will appear on a report. Please check formatting and spelling." />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <br />
                                        <SAHL:SAHLButton ID="btnCommentEditorSubmit" runat="server" Text="Update" OnClientClick="popup.Hide();"
                                            OnClick="btnCommentEditorSubmit_Click" />
                                        <input id="btnCommentEditorCancel" type="button" value="Cancel" onclick="popup.Hide();" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:HiddenField ID="hidHearingDetailKey" runat="server" />
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
    <div style="text-align: left">
        <table width="100%" class="tableStandard">
            <tr>
                <td>
                    <SAHL:DXGridView ID="grdHearingDetail" PostBackType="NoneWithClientSelect" runat="server"
                        KeyFieldName="Key" AutoGenerateColumns="false" Width="100%" OnHtmlDataCellPrepared="DXGridView1_HtmlDataCellPrepared"
                        Caption="Hearing Details" SettingsText-EmptyDataRow="No hearing details exist for the debt counselling case.">
                        <Settings ShowTitlePanel="true" />
                        <SettingsPager PageSize="15" />
                        <Columns>
                            <SAHL:DXGridViewFormattedTextColumn FieldName="Key" Caption="Key" VisibleIndex="0"
                                Visible="false">
                            </SAHL:DXGridViewFormattedTextColumn>
                            <SAHL:DXGridViewFormattedTextColumn FieldName="DebtCounselling.Key" Caption="DebtCounsellingKey"
                                VisibleIndex="1" Visible="false">
                            </SAHL:DXGridViewFormattedTextColumn>
                            <SAHL:DXGridViewFormattedTextColumn FieldName="HearingType.Description" Caption="Hearing Type"
                                VisibleIndex="2">
                            </SAHL:DXGridViewFormattedTextColumn>
                            <SAHL:DXGridViewFormattedTextColumn FieldName="HearingAppearanceType.Description"
                                Caption="Appearance Status" VisibleIndex="3">
                            </SAHL:DXGridViewFormattedTextColumn>
                            <SAHL:DXGridViewFormattedTextColumn FieldName="Court.Name" Caption="Court" VisibleIndex="4">
                            </SAHL:DXGridViewFormattedTextColumn>
                            <SAHL:DXGridViewFormattedTextColumn FieldName="CaseNumber" Caption="CaseNumber" VisibleIndex="5">
                            </SAHL:DXGridViewFormattedTextColumn>
                            <SAHL:DXGridViewFormattedTextColumn Name="colHearingDate" FieldName="HearingDate"
                                Caption="Next Hearing Date" VisibleIndex="6">
                            </SAHL:DXGridViewFormattedTextColumn>
                            <SAHL:DXGridViewFormattedTextColumn FieldName="GeneralStatus.Description" Caption="Status"
                                VisibleIndex="7" Visible="false">
                            </SAHL:DXGridViewFormattedTextColumn>
                            <SAHL:DXGridViewFormattedTextColumn CellStyle-HorizontalAlign="Center" Name="colComment"
                                FieldName="Comment" Caption="Comment" VisibleIndex="8">
                                <DataItemTemplate>
                                    <img id="imgCorrespondenceDetails" style="text-align: center; cursor: pointer;" onclick="OnMoreInfoClick(this, '<%# Container.KeyValue %>')"
                                        alt="View Comment" src="<%=Img_src_path%>" />
                                </DataItemTemplate>
                            </SAHL:DXGridViewFormattedTextColumn>
                        </Columns>
                    </SAHL:DXGridView>
                </td>
            </tr>
            <tr>
                <td>
                    <SAHL:SAHLPanel ID="pnlCourtDetailsMaintenance" runat="server">
                        <table width="100%" class="tableStandard">
                            <tr class="rowStandard">
                                <td class="TitleText" style="width: 150px">Hearing Type
                                </td>
                                <td class="TitleText">
                                    <SAHL:SAHLDropDownList ID="ddlHearingType" runat="server" onchange="HearingTypeChanged()"
                                        Mandatory="True">
                                    </SAHL:SAHLDropDownList>
                                </td>
                            </tr>
                            <tr class="rowStandard">
                                <td class="TitleText">Appearance Status
                                </td>
                                <td class="TitleText">
                                    <SAHL:SAHLDropDownList ID="ddlAppearanceType" runat="server" Mandatory="True" OnSelectedIndexChanged="OnHearingAppearanceTypeChanged" AutoPostBack="true">
                                    </SAHL:SAHLDropDownList>
                                    <ajaxToolkit:CascadingDropDown ID="ccdAppearanceType" runat="server" TargetControlID="ddlAppearanceType"
                                        Category="Court" PromptText="- Please select -" PromptValue="-1" ServicePath="~/AJAX/Court.asmx"
                                        ServiceMethod="GetAppearanceTypesByHearingType" ParentControlID="ddlHearingType"
                                        EmptyValue="-1" EmptyText="- Please select -" LoadingText="[Loading...]" />
                                </td>
                            </tr>
                            <tr id="trCourt" runat="server" class="rowStandard">
                                <td class="TitleText">Court
                                </td>
                                <td class="TitleText">
                                    <SAHL:SAHLTextBox ID="txtCourtSearch" runat="server" Mandatory="True">
                                    </SAHL:SAHLTextBox>
                                    <asp:HiddenField ID="hidSelectedCourtKey" runat="server" EnableViewState="False" />
                                    <SAHL:SAHLAutoComplete AutoPostBack="true" runat="server" ID="acCourtSearch" TargetControlID="txtCourtSearch"
                                        OnItemSelected="OnCourtSearchResultItemSelected" MinCharacters="1" ServiceMethod="SAHL.Web.AJAX.Court.GetCourtsByTypeAndPrefix">
                                    </SAHL:SAHLAutoComplete><img alt="Search" src="../../Images/Search.png" />
                                </td>
                            </tr>
                            <tr class="rowStandard">
                                <td class="TitleText">Case Number
                                </td>
                                <td class="TitleText">
                                    <SAHL:SAHLTextBox ID="txtCaseNumber" runat="server" Mandatory="True">
                                    </SAHL:SAHLTextBox>
                                </td>
                            </tr>
                            <tr class="rowStandard">
                                <td class="TitleText">
									<asp:Label runat="server" ID="lblHearingDate" Text="Next Hearing Date"></asp:Label>
                                </td>
                                <td class="TitleText">
                                    <SAHL:SAHLDateBox ID="dteHearingDate" runat="server" Mandatory="True">
                                    </SAHL:SAHLDateBox>
                                </td>
                            </tr>
                            <tr class="rowStandard">
                                <td class="TitleText" valign="top">Comments
                                </td>
                                <td class="TitleText">
                                    <SAHL:SAHLTextBox ID="txtComments" runat="server" Mandatory="False" Rows="5" TextMode="MultiLine"
                                        Width="400px"></SAHL:SAHLTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <br />
                                    <SAHL:SAHLButton ID="btnSubmit" runat="server" Text="Add" AccessKey="S" OnClick="btnSubmit_Click" />
                                    <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" AccessKey="C" OnClick="btnCancel_Click"
                                        CausesValidation="false" />
                                </td>
                            </tr>
                        </table>
                    </SAHL:SAHLPanel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
