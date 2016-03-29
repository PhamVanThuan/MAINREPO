<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    CodeBehind="NoteMaintenance.aspx.cs" Inherits="SAHL.Web.Views.Common.NoteMaintenance" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <script src="../../Scripts/jquery-1.5.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        function GetCasesWithSameDiaryDate() {
            var genericKey = $("#" + '<%=hidGenericKey.ClientID %>').val();
            var diaryDate = $("#" + '<%=dtDiaryDate.ClientID %>').val();
            var adUserKey = $("#" + '<%=HidADUserKey.ClientID %>').val();
            var workflowName = $("#" + '<%=HidWorkflowName.ClientID %>').val(); 
            $.ajax({
                type: "POST",
                url: "../../Ajax/Diary.asmx/GetCasesWithSameDiaryDate",
                data: "{'genericKey': '" + genericKey + "', 'diaryDate': '" + diaryDate + "', 'aduserKey':'" + adUserKey + "', 'workflowName': '" + workflowName + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: onSuccess
            });
        }

        function onSuccess(jsonData) {
            $('#<%= lblRelatedEntries.ClientID %>').html(jsonData.d.toString().toLocaleString());
        }
    </script>
    <table>
        <tr>
            <td class="TitleText">
                Diary Date:&nbsp;
            </td>
            <td>
                <SAHL:SAHLDateBox ID="dtDiaryDate" Style="width: 100px" runat="server" CssClass="mandatory"></SAHL:SAHLDateBox>
            </td>
            <td style="width: 20px">
            </td>
            <td>
                <SAHL:SAHLButton ID="btnSubmit" runat="server" Visible="true" Text="Save Diary Date"
                    OnClick="SubmitButton_Click" />
                <input type="button" id="Button1" value="Check Diary" onclick="GetCasesWithSameDiaryDate()" />
                <asp:Label ID="lblRelatedEntries" runat="server" Text="" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
    <div style="height: 430px; overflow-x: hidden; overflow-y: auto; border-style: solid;
        border-width: thin">
        <SAHL:DXGridView ID="gvNotes" runat="server" EnableCallBacks="True" AutoGenerateColumns="False"
            Width="100%" KeyFieldName="Key" PostBackType="None" OnRowUpdating="gvNotes_RowUpdating"
            OnHtmlEditFormCreated="gvNotes_HtmlEditFormCreated" OnStartRowEditing="gvNotes_StartRowEditing"
            OnRowValidating="gvNotes_RowValidating" OnRowInserting="gvNotes_RowInserting"
            OnCustomColumnDisplayText="gvNotes_CustomColumnDisplayText">
            <SettingsEditing Mode="PopupEditForm" PopupEditFormWidth="700" PopupEditFormModal="true"
                PopupEditFormShowHeader="false" PopupEditFormHorizontalAlign="WindowCenter" PopupEditFormVerticalAlign="WindowCenter" />
            <SettingsPager Mode="ShowAllRecords" />
            <Settings ShowTitlePanel="true" ShowVerticalScrollBar="false" />
            <SettingsText Title="Note Details" EmptyDataRow=" " />
            <SettingsBehavior AllowDragDrop="false" ColumnResizeMode="Control" />
            <Columns>
                <SAHL:DXGridViewCommandColumn VisibleIndex="0" Width="70px" Caption=" " ButtonType="Button">
                    <EditButton Visible="True">
                        <Image ToolTip="Edit Note" Url="../../Images/application_edit.png" />
                    </EditButton>
                    <NewButton Visible="True" Text="Add">
                        <Image ToolTip="Add New Note" Url="../../Images/add_blue4.png" />
                    </NewButton>
                </SAHL:DXGridViewCommandColumn>
                <SAHL:DXGridViewFormattedTextColumn FieldName="InsertedDate" Caption="Date" VisibleIndex="0"
                    Width="115px">
                </SAHL:DXGridViewFormattedTextColumn>
                <SAHL:DXGridViewFormattedTextColumn FieldName="NoteText" Caption="Note" VisibleIndex="1">
                    <EditFormSettings VisibleIndex="0" />
                </SAHL:DXGridViewFormattedTextColumn>
                <SAHL:DXGridViewFormattedTextColumn FieldName="Tag" Caption="Tag" VisibleIndex="2"
                    Width="80px">
                    <EditFormSettings VisibleIndex="1" />
                </SAHL:DXGridViewFormattedTextColumn>
            </Columns>
            <Templates>
                <EditForm>
                    <div style="padding: 4px 4px 3px 4px">
                        <table cellpadding="2" cellspacing="2" width="100%">
                            <tr>
                                <td valign="top" class="TitleText">
                                    <br />
                                    Note:&nbsp;
                                </td>
                                <td>
                                    <br />
                                    <SAHL:DXHtmlEditor ID="notesEditor" Html='<%# Eval("NoteText")%>' runat="server">
                                        <SettingsImageUpload UploadImageFolder="" />
                                        <SettingsValidation>
                                            <RequiredField IsRequired="True" />
                                        </SettingsValidation>
                                    </SAHL:DXHtmlEditor>
                                    <%-- <SAHL:DXMemo runat="server" Visible="false" ID="notesEditor1" Text='<%# Eval("NoteText")%>' Width="550px"
                                        Height="93px" ValidationSettings-ValidationGroup="<%# Container.ValidationGroup %>">
                                        <ValidationSettings ErrorTextPosition="Bottom" ErrorDisplayMode="ImageWithText">
                                            <RequiredField ErrorText="Note is required" IsRequired="true" />
                                        </ValidationSettings>
                                    </SAHL:DXMemo>--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="TitleText">
                                    <br />
                                    Tag:&nbsp;
                                </td>
                                <td>
                                    <br />
                                    <SAHL:SAHLTextBox ID="txtTag" Text='<%# Eval("Tag")%>' runat="server">
                                    </SAHL:SAHLTextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="text-align: right; padding: 2px 2px 2px 2px">
                        <SAHL:DXGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                            runat="server">
                        </SAHL:DXGridViewTemplateReplacement>
                        <SAHL:DXGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                            runat="server">
                        </SAHL:DXGridViewTemplateReplacement>
                    </div>
                </EditForm>
            </Templates>
        </SAHL:DXGridView>
    </div>
    <table width="100%">
        <tr>
            <td align="right">
                <br />
                <SAHL:SAHLButton ID="btnCancel" runat="server" Visible="true" Text="Done" OnClick="CancelButton_Click" />
            </td>
        </tr>
        <tr align="center">
            <td>
                <asp:HiddenField ID="hidGenericKey" runat="server" />
                <asp:HiddenField ID="HidADUserKey" runat="server" />
                <asp:HiddenField ID="HidWorkflowName" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
