<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true" CodeBehind="ClientCommuncation.aspx.cs" Inherits="SAHL.Web.Views.Correspondence.ClientCommunication" Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<%@ Register Assembly="DevExpress.Web.ASPxSpellChecker.v10.2, Version=10.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSpellChecker" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v10.2, Version=10.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <script src="../../Scripts/jquery-1.5.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $(".smsBody").hide(); // hide the sms tetxbox at start
            if ($("#<%=ddlSMSType.ClientID%>").val() != "-select-")
                $(".smsBody").show();
        });

        function SMSTypeChanged() {
            // get selected sms type (key value)
            var selectedSMSType = $("#<%=ddlSMSType.ClientID%>").val();

            if (selectedSMSType == "-select-") // if no sms type selected then hide the sms row
            {
                $(".smsBody").hide();
                $("#<%=txtSMSText.ClientID%>").val("");
            }
            else  // if we have selected a valid sms type then show the row
            {
                $(".smsBody").show();

                // Set the result into sms TextBox
                if (selectedSMSType == 1) // free format 
                {
                    $("#<%=txtSMSText.ClientID%>").removeAttr("readonly"); // make editable
                    $("#<%=txtSMSText.ClientID%>").val(""); // reset text
                }
                else if (selectedSMSType == 2)  // banking details
                {
                    $("#<%=txtSMSText.ClientID%>").attr("readonly", "true"); // make readonly
                    var bankDetails = $("#<%=hidBankDetails.ClientID%>").val();
                    $("#<%=txtSMSText.ClientID%>").val(bankDetails); // set text to banking details
                }


        }
    }
    </script>
    <div style="text-align: center; width: 100%">
        <table id="tblRecipients" width="100%" class="tableStandard" style="text-align: left;">
            <tr>
                <td style="width: 50px; vertical-align: top; text-align: right; padding-right: 5px; padding-top: 5px">
                    <SAHL:SAHLLabel ID="lblTo" runat="server" Text="To:" Font-Bold="true"></SAHL:SAHLLabel>
                </td>
                <td rowspan="2" style="width: 100%;">
                    <SAHL:DXGridView ID="gridRecipients" ClientInstanceName="gridRecipients" runat="server"
                        AutoGenerateColumns="False" Width="100%" KeyFieldName="Key" PostBackType="None"
                        OnHtmlCommandCellPrepared="gridRecipients_HtmlCommandCellPrepared" OnCommandButtonInitialize="gridRecipients_CommandButtonInitialize">
                        <Settings ShowTitlePanel="true" ShowGroupPanel="false" ShowVerticalScrollBar="true" />
                        <SettingsBehavior AllowGroup="false" AllowSort="true" AllowDragDrop="false" AllowSelectSingleRowOnly="false" />
                        <SettingsPager Visible="true" Mode="ShowAllRecords" />
                        <Columns>
                            <SAHL:DXGridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="5%" Caption="Sel">
                                <HeaderStyle HorizontalAlign="Center" />
                            </SAHL:DXGridViewCommandColumn>
                            <SAHL:DXGridViewFormattedTextColumn FieldName="Key" VisibleIndex="1" Visible="false" />
                            <SAHL:DXGridViewFormattedTextColumn FieldName="RecipientName" VisibleIndex="2" Width="40%" />
                            <SAHL:DXGridViewFormattedTextColumn FieldName="Role" VisibleIndex="3" Width="30%" />
                            <SAHL:DXGridViewFormattedTextColumn FieldName="CellPhoneNumber" VisibleIndex="4" Width="25%" />
                            <SAHL:DXGridViewFormattedTextColumn FieldName="EmailAddress" VisibleIndex="5" Width="25%" />
                        </Columns>
                    </SAHL:DXGridView>
                </td>
            </tr>
            <tr>
                <td style="width: 50px; vertical-align: bottom; text-align: right; padding-right: 5px; padding-top: 5px">
                    <SAHL:SAHLImageButton ID="btnSend" runat="server" OnClick="btnSend_Click" ImageUrl="~/Images/EmailForward.gif"
                        BorderStyle="Outset" BorderWidth="1px" Width="50px" ImageAlign="Middle" ToolTip="Send Email" />
                </td>
                <td></td>
            </tr>
            <tr>
                <tr>
                    <td colspan="2" style="height: 5px"></td>
                </tr>
            </tr>
            <tr id="trEmailSubject" runat="server">
                <td style="width: 50px; text-align: right; padding-right: 5px">
                    <SAHL:SAHLLabel ID="lblSubject" runat="server" Text="Subject:" Font-Bold="true"></SAHL:SAHLLabel>
                </td>
                <td>
                    <SAHL:SAHLTextBox ID="txtEmailSubject" runat="server" MaxLength="80" Width="99%" />
                </td>
            </tr>
            <tr id="trSMSType" runat="server">
                <td style="text-align: right; padding-right: 5px">
                    <SAHL:SAHLLabel ID="lblSMSType" runat="server" class="TitleText" Text="Type:" Font-Bold="true" />
                </td>
                <td>
                    <SAHL:SAHLDropDownList ID="ddlSMSType" runat="server" onchange="SMSTypeChanged()" />
                </td>
            </tr>
            <tr>
                <td colspan="2" style="height: 5px"></td>
            </tr>
        </table>
        <table id="tBody" width="100%" class="tableStandard" style="text-align: left">
            <tr>
                <td style="width: 50px"></td>
                <td></td>
            </tr>
            <tr id="trEmailBody" runat="server">
                <td colspan="2">
                    <SAHL:DXHtmlEditor ID="htmlEmailEditor" runat="server" Width="100%" ClientInstanceName="htmlEmail"
                        Height="200px">
                        <SettingsImageUpload UploadImageFolder="">
                        </SettingsImageUpload>
                        <Settings AllowHtmlView="false" AllowPreview="false" AllowDesignView="true" />
                    </SAHL:DXHtmlEditor>
                </td>
            </tr>
            <tr id="trSMSBody" runat="server" class="smsBody">
                <td></td>
                <td>
                    <SAHL:SAHLTextBox ID="txtSMSText" runat="server" Width="99%" MaxLength="160"></SAHL:SAHLTextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <SAHL:SAHLButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Complete" Visible="false"
                        CausesValidation="False" />
                </td>
            </tr>
        </table>
        <asp:HiddenField runat="server" ID="hidBankDetails" />
    </div>
</asp:Content>
