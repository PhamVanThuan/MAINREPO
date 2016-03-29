<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Common.AccountMailingAddress" Title="CBO Page" Codebehind="AccountMailingAddress.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table border="0" cellspacing="0" cellpadding="0" class="tableStandard">
        <tr>
            <td align="left" style="height: 100%; width: 100%;" valign="top">
                <table border="0" id="InfoTable" runat="server" width="100%">
                    <tr id="CorrespondenceMediumRow" visible="false">
                        <td>
                            <SAHL:SAHLLabel ID="CorrespondenceMedium" runat="server" Font-Bold="true">Correspondence Medium</SAHL:SAHLLabel>
                        </td>
                        <td>
                            <SAHL:SAHLLabel ID="lblCorrespondenceMedium" runat="server"></SAHL:SAHLLabel>
                        </td>
                        <td>
                            <SAHL:SAHLDropDownList ID="ddlCorrespondenceMedium" runat="server" AutoPostBack="true"
                                PleaseSelectItem="true" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlCorrespondenceMedium_SelectedIndexChanged">
                            </SAHL:SAHLDropDownList>
                            <SAHL:SAHLRequiredFieldValidator ID="valCorrespondenceMedium" runat="server" ControlToValidate="ddlCorrespondenceMedium" ErrorMessage="Please select a Correspondence Medium" InitialValue="-select-" />                                                        
                        </td>
                    </tr>
                    <tr>
                    </tr>
                    <tr id="CorrespondenceMailAddressRow" visible="false">
                        <td>
                            <SAHL:SAHLLabel ID="CorrespondenceMailAddress" runat="server" Font-Bold="true">Email Address</SAHL:SAHLLabel>
                        </td>
                        <td>
                            <SAHL:SAHLLabel ID="lblCorrespondenceMailAddress" runat="server"></SAHL:SAHLLabel>
                        </td>
                        <td>
                            <SAHL:SAHLDropDownList ID="ddlCorrespondenceMailAddress" runat="server"
                                AutoPostBack="false" PleaseSelectItem="true">
                            </SAHL:SAHLDropDownList>
                            <SAHL:SAHLRequiredFieldValidator ID="valCorrespondenceMailAddress" runat="server" ControlToValidate="ddlCorrespondenceMailAddress" ErrorMessage="Please select an Email Address" InitialValue="-select-" />                                                        
                        </td>
                    </tr>
                    <tr id="AddressDropDown">
                        <td>
                            <SAHL:SAHLLabel ID="lblMailingAddress" runat="server" Font-Bold="true">Address</SAHL:SAHLLabel>
                        </td>
                        <td>
                        </td>
                        <td>
                            <SAHL:SAHLDropDownList ID="ddlMailingAddress" runat="server" AutoPostBack="True"
                                PleaseSelectItem="true" OnSelectedIndexChanged="ddlMailingAddressSelectedIndexChanged">
                            </SAHL:SAHLDropDownList>
                            <SAHL:SAHLRequiredFieldValidator ID="valMailingAddress" runat="server" ControlToValidate="ddlMailingAddress" ErrorMessage="Please select a Mailing Address" InitialValue="-select-" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlMailingAddressDetDisp" runat="server" Height="110px" Width="100%" GroupingText="Mailing Address"
                                Font-Bold="true">
                                <SAHL:SAHLLabel ID="AddressLineDisp" runat="server" CssClass="LabelText" Height="87px"
                                    Width="196px">-</SAHL:SAHLLabel><br />
                            </asp:Panel>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:Panel ID="pnlMailingAddressDetails" runat="server" Height="110px" Width="100%" GroupingText="Mailing Address"
                                Font-Bold="true">
                                <SAHL:SAHLLabel ID="AddressLine" runat="server" CssClass="LabelText" Height="87px"
                                    Width="196px">-</SAHL:SAHLLabel><br />
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                    </tr>
                    <tr>
                        <td>
                            <SAHL:SAHLLabel ID="CorrespondenceLanguage" runat="server" Font-Bold="true">Correspondence Language</SAHL:SAHLLabel>
                        </td>
                        <td>
                            <SAHL:SAHLLabel ID="lblCorrespondenceLanguage" runat="server"></SAHL:SAHLLabel>
                        </td>
                        <td>
                            <SAHL:SAHLDropDownList ID="ddlCorrespondenceLanguage" runat="server"
                                AutoPostBack="false" PleaseSelectItem="true">
                            </SAHL:SAHLDropDownList>
                            <SAHL:SAHLRequiredFieldValidator ID="valCorrespondenceLanguage" runat="server" ControlToValidate="ddlCorrespondenceLanguage" ErrorMessage="Please select a Correspondence Language" InitialValue="-select-" />                            
                        </td>
                    </tr>
                    <tr>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkOnlineStatement" runat="server" Text="Online Statement" AutoPostBack="True"
                                OnCheckedChanged="ChkOnlineStatement_CheckChanged" />
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <SAHL:SAHLLabel ID="lblOnlineStatementFormat" runat="server" Font-Bold="true">Online Statement Format</SAHL:SAHLLabel>
                        </td>
                        <td>
                            <SAHL:SAHLLabel ID="OnlineStatementFormat" runat="server"></SAHL:SAHLLabel>
                        </td>
                        <td>
                            <SAHL:SAHLDropDownList ID="ddlOnlineStatementFormat" runat="server" />
                        </td>
                    </tr>
                    <tr>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="ViewButtons" runat="server">
            <td align="right" valign="bottom" style="width: 100%">
                <SAHL:SAHLButton ID="CancelButton" runat="server" Text="Cancel" AccessKey="C" OnClick="CancelButton_Click"
                    CausesValidation="false" />
                <SAHL:SAHLButton ID="SubmitButton" runat="server" Text="Submit" AccessKey="S" OnClick="SubmitButton_Click" 
                CausesValidation="true"/>&nbsp;
            </td>
        </tr>
        <tr>
            <td align="right" style="width: 100%">
                <SAHL:SAHLButton ID="btnAuditTrail" runat="server" Text="Audit Trail" CausesValidation="false"
                    OnClick="btnAuditTrail_Click" Visible="false" />
            </td>
        </tr>
    </table>
</asp:Content>
