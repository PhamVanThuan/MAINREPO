<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="Applicants.aspx.cs" Inherits="SAHL.Web.Views.Common.Applicants" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <table class="tableStandard" width="100%">
        <tr id="trLinkedDebtCounsellingAccountsWarningMessage" runat="server">
            <td align="center">
                <SAHL:SAHLLabel ID="lblLinkedDebtCounsellingAccountsWarningMessage" runat="server" TextAlign="Left" BackColor="Red"
                    ForeColor="White" Font-Size="Larger"></SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td>
                <SAHL:SAHLGridView ID="grdApplicants" runat="server" AutoGenerateColumns="false"
                    FixedHeader="false" EnableViewState="false" GridHeight="200px" GridWidth="100%"
                    Width="100%" HeaderCaption="Applicants" NullDataSetMessage="There are no Legal Entities defined for this Application.<BR> Use the Add Applicant action to add Legal Entities for this application."
                    EmptyDataSetMessage="There are no Legal Entities defined for this Application.<BR> Use the Add Applicant action to add Legal Entities for this application."
                    OnSelectedIndexChanged="grdApplicants_SelectedIndexChanged" PostBackType="SingleClick"
                    OnRowDataBound="grdApplicants_RowDataBound">
                    <RowStyle CssClass="TableRowA" />
                </SAHL:SAHLGridView>
            </td>
        </tr>
        <tr id="trApplicantsDetails" runat="server">
            <td>
                <asp:Panel ID="pnlLegalEntity" runat="server" Width="100%">
                    <table class="tableStandard" width="100%">
                        <tr>
                            <td>
                           
                                <table id="tblNaturalLegalEntityDetails" runat="server" class="tableStandard" width="100%">
                                    <tr>
                                        <td class="TitleText" style="width:50%">
                                            Legal Entity Name</td>
                                        <td style="width:50%">
                                            <asp:Label ID="lblNaturalLegalEntityName" runat="server" Text="-"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TitleText">
                                            Preferred Name</td>
                                        <td>
                                            <asp:Label ID="lblNaturalPreferredName" runat="server" Text="-"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TitleText">
                                            ID Number</td>
                                        <td>
                                            <asp:Label ID="lblNaturalIDNumber" runat="server" Text="-"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TitleText">
                                            Date of Birth</td>
                                        <td>
                                            <asp:Label ID="lblNaturalDateOfBirth" runat="server" Text="-"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td class="TitleText">
                                            Marital Status</td>
                                        <td>
                                            <asp:Label ID="lblNaturalMaritalStatus" runat="server" Text="-"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TitleText">
                                            Gender</td>
                                        <td>
                                            <asp:Label ID="lblNaturalGender" runat="server" Text="-"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td class="TitleText">
                                            Legal Entity Status</td>
                                        <td>
                                            <asp:Label ID="lblNaturalLegalEntityStatus" runat="server" Text="-"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <table id="tblCompanyLegalEntityDetails" runat="server" class="tableStandard" width="100%">
                                    <tr>
                                        <td class="TitleText" style="width:50%">
                                            Company Name</td>
                                        <td style="width:50%">
                                            <asp:Label ID="lblCompanyName" runat="server" Text="-"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TitleText">
                                            Trading Name</td>
                                        <td>
                                            <asp:Label ID="lblCompanyTradingName" runat="server" Text="-"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TitleText">
                                            Registration Number</td>
                                        <td>
                                            <asp:Label ID="lblCompanyRegistrationNumber" runat="server" Text="-"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TitleText">
                                            Legal Entity Status</td>
                                        <td>
                                            <asp:Label ID="lblCompanyLegalEntityStatus" runat="server" Text="-"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <asp:Panel ID="pnlContactDetails" runat="server" Width="100%" GroupingText="Legal Entity Contact Details">
                                    <table id="tblNaturalContactDetails" runat="server" class="tableStandard" width="100%">
                                        <tr>
                                        <td class="TitleText" style="width:50%">
                                                Home Phone</td>
                                            <td style="width:50%">
                                                <asp:Label ID="lblNaturalHomePhone" runat="server" Text="-"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TitleText">
                                                Work Phone</td>
                                            <td>
                                                <asp:Label ID="lblNaturalWorkPhone" runat="server" Text="-"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TitleText">
                                                Fax Number</td>
                                            <td>
                                                <asp:Label ID="lblNaturalFaxNumber" runat="server" Text="-"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TitleText">
                                                Cellphone</td>
                                            <td>
                                                <asp:Label ID="lblNaturalCellphone" runat="server" Text="-"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TitleText">
                                                Email Address</td>
                                            <td>
                                                <asp:Label ID="lblNaturalEmailAddress" runat="server" Text="-"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblCompanyContactDetails" runat="server" class="tableStandard" width="100%">
                                        <tr>
                                        <td class="TitleText" style="width:50%">
                                                Work Phone</td>
                                            <td style="width:50%">
                                                <asp:Label ID="lblCompanyWorkPhone" runat="server" Text="-"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TitleText">
                                                Fax Number</td>
                                            <td>
                                                <asp:Label ID="lblCompanyFaxNumber" runat="server" Text="-"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TitleText">
                                                Cellphone</td>
                                            <td>
                                                <asp:Label ID="lblCompanyCellphone" runat="server" Text="-"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TitleText">
                                                Email Address</td>
                                            <td>
                                                <asp:Label ID="lblCompanyEmailAddress" runat="server" Text="-"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr id="trButtons" runat="server">
                            <td align="right" colspan="2">
                                <SAHL:SAHLButton ID="btnAdd" runat="server" AccessKey="A" Text="Add" OnClick="btnAdd_Click" />
                                <SAHL:SAHLButton ID="btnRemove" runat="server" AccessKey="R" Text="Remove" OnClick="btnRemove_Click" />
                                <SAHL:SAHLButton ID="btnCancel" runat="server" AccessKey="C" Text="Cancel" OnClick="btnCancel_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
