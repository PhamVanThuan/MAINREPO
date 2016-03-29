<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    CodeBehind="CorrespondenceProcessing.aspx.cs" Inherits="SAHL.Web.Views.Correspondence.CorrespondenceProcessing"
    Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.2, Version=10.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2, Version=10.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Src="../Life/WorkFlowHeader.ascx" TagName="WorkFlowHeader" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <script language="javascript" type="text/javascript">
        var cssSelectedRow = ' TableCorrespondenceSelectRowA';
        var preventToggle = false;

        function cancelToggle() {
            preventToggle = true;
        }

        function getExpandImage(rowElement) {
            return rowElement.getElementsByTagName('img')[0];
        }

        function getExtraInfoDiv(rowElement) {
            var children = rowElement.getElementsByTagName('div');
            return children[5];
        }

        function toggleExtraInfo(anchor, legalEntityKey, genericKey, genericKeyTypeKey) {
            var node = anchor.parentElement;
            var img = anchor.getElementsByTagName('img')[0];
            var rowElement = null;
            while (rowElement == null && node != null) {
                if (node.tagName.toLowerCase() == 'td')
                    rowElement = node;
                else
                    node = node.parentElement;
            }

            // work out where we're expanding or contracting
            var isExpanded = (img.src.indexOf('arrow_blue_down') > -1);
            setTimeout('doToggle("' + rowElement.id + '", "' + legalEntityKey + '", "' + genericKey + '", "' + genericKeyTypeKey + '", ' + !isExpanded + ')', 300);
        }

        function doToggle(rowElementID, legalEntityKey, genericKey, genericKeyTypeKey, expand) {
            // check for double-click first
            if (preventToggle) {
                preventToggle = false;
                return;
            }

            var rowElement = document.getElementById(rowElementID);
            var divExtraInfo = getExtraInfoDiv(rowElement);

            // collapse the previously expanded div
            if (!expand) {
                getExtraInfoDiv(rowElement).style.display = 'none';
                //rowElement.className = rowElement.className.substring(0, rowElement.className.length - cssSelectedRow.length);
                getExpandImage(rowElement).src = '../../Images/arrow_blue_right.gif';
                return;
            }

            // check to see if we've loaded the div before, if not we need to fire off the AJAX call to 
            // populate the control
            if (divExtraInfo.innerHTML == '') {

                divExtraInfo.innerHTML = 'Loading data.....';
                // make the AJAX call
                SAHL.Web.AJAX.LegalEntity.GetCorrespondenceExtraLegalEntityDetails(legalEntityKey, genericKey, genericKeyTypeKey, rowElementID, clientDetailsCallback);
            }

            // update the styles of the new element        
            divExtraInfo.style.display = 'inline';
            //rowElement.className = rowElement.className + cssSelectedRow;
            getExpandImage(rowElement).src = '../../Images/arrow_blue_down.gif';

        }

        function clientDetailsCallback(result) {
            var rowElement = document.getElementById(result[0]);
            var divExtraInfo = getExtraInfoDiv(rowElement);
            divExtraInfo.innerHTML = result[1];
        }

    </script>
    <uc1:WorkFlowHeader ID="WorkFlowHeader1" runat="server" />
    <div style="text-align: center; width: 100%">
        <table width="100%">
            <tr>
                <td align="left" colspan="2">
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:Panel ID="Panel2" runat="server" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px"
                        Width="100%" HorizontalAlign="left">
                        <table>
                            <tr>
                                <td class="TitleText">
                                    <asp:Label ID="Label1" runat="server" Text="Document(s) to send:" Width="150px"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblDocument" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="TitleText">
                                    <asp:Label ID="lblDocumentLanguageHeader" runat="server" Text="Document Language:"
                                        Width="150px"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblDocumentLanguage" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr id="trRecipients" runat="server">
                <td align="left" colspan="2">
                    <cc1:LegalEntityGrid ID="gridRecipients" runat="server" OnSelectedIndexChanged="gridRecipients_SelectedIndexChanged"
                        ColumnIDPassportVisible="True">
                    </cc1:LegalEntityGrid>
                    <SAHL:SAHLGridView ID="gridRecipientsMultiple" runat="server" AutoGenerateColumns="true"
                        EnableViewState="false" FixedHeader="false" GridHeight="280px"
                        GridWidth="100%" OnRowDataBound="gridRecipientsMultiple_RowDataBound"
                        PostBackType="None" Width="100%" SelectFirstRow="false">
                        <RowStyle CssClass="TableRowA" />
                    </SAHL:SAHLGridView>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <cc1:AddressGrid ID="gridAddress" runat="server">
                    </cc1:AddressGrid>
                </td>
            </tr>
            <tr>
                <td align="left" valign="middle" style="text-align: center" colspan="2">
                    <SAHL:SAHLButton ID="btnAddAddress" runat="server" ButtonSize="Size6" Text="Add Address"
                        Width="85px" OnClick="btnAddAdress_Click" CausesValidation="False" SecurityTag="LifeUpdateAccessWorkflow" />
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                </td>
            </tr>
            <tr>
                <td id="tblSendOptions" runat="server" align="left" style="width: 40%; vertical-align: top">
                    <asp:Panel ID="pnlSendOptions" runat="server" BorderColor="Silver" BorderStyle="None"
                        GroupingText="Correspondence Options (Tick one or many)" HorizontalAlign="Left"
                        Width="100%">
                        <table>
                            <tr>
                                <td style="width: 50px">
                                    <asp:CheckBox ID="chkFax" runat="server" Text="Fax" Enabled="False" />
                                </td>
                                <td style="width: 10px">
                                </td>
                                <td style="width: 300px">
                                    <SAHL:SAHLTextBox ID="txtFaxCode" runat="server" DisplayInputType="Number" MaxLength="10"
                                        Width="42px" ReadOnly="True"></SAHL:SAHLTextBox>
                                    <SAHL:SAHLTextBox ID="txtFax" runat="server" DisplayInputType="Number" MaxLength="15"
                                        Width="136px" ReadOnly="True"></SAHL:SAHLTextBox>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkEmail" runat="server" Text="Email" Enabled="False" />
                                </td>
                                <td>
                                </td>
                                <td>
                                    <SAHL:SAHLTextBox ID="txtEmail" runat="server" Width="300px" ReadOnly="True"></SAHL:SAHLTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkPost" runat="server" Text="Post" Enabled="False" />
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkSMS" runat="server" Text="SMS" Enabled="False" />
                                </td>
                                <td>
                                </td>
                                <td>
                                    <SAHL:SAHLTextBox ID="txtSMS" runat="server" ReadOnly="True" Enabled="False" Visible="False"></SAHL:SAHLTextBox>&nbsp;
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
                <td id="tblExtraParameters" runat="server" align="left" style="width: 60%; vertical-align: top">
                    <asp:Panel ID="pnlExtraParameters" runat="server" Width="100%" BorderColor="Silver"
                        BorderStyle="None" GroupingText="Extra Parameters" HorizontalAlign="Left" Visible="false">
                        <asp:Table ID="tblParameters" runat="server" BackColor="White" Width="100%">
                        </asp:Table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                </td>
            </tr>
            <tr>
                <td align="right" valign="top" colspan="2">
                    <SAHL:SAHLButton ID="btnPreview" runat="server" Text="Preview" OnClick="btnPreview_Click"
                        SecurityTag="LifeUpdateAccessWorkflow" />
                    <SAHL:SAHLButton ID="btnSend" runat="server" Text="Send Correspondence" OnClick="btnSend_Click"
                        SecurityTag="LifeUpdateAccessWorkflow" />
                    <SAHL:SAHLButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel"
                        CausesValidation="False" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
