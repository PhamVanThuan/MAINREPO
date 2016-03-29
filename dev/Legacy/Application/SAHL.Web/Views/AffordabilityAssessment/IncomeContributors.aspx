<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.AffordabilityAssessment.IncomeContributors" Title="Income Contributors" CodeBehind="IncomeContributors.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <script src="../../Scripts/jquery-1.5.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            // when any checkbox in the list is clicked
            $("#divFields input[type=checkbox]").click(function () {

                // count the number of checkboxed clicked
                var checkedCount = $(".chklist").find("input:checked").length;

                // get the textbox and set the value
                var txtbox = $("#<%=txtNumberOfContributingApplicants.ClientID%>");
                txtbox.val(checkedCount);
            });
        });
    </script>
    <div id="divFields" style="text-align: center">
        <table runat="server" style="width: 100%" class="tableStandard">
            <tr>
                <td style="width: 5%"></td>
                <td style="width: 15%"></td>
                <td style="width: 20%"></td>
                <td style="width: 5%"></td>
                <td style="width: 35%"></td>
                <td style="width: 20%"></td>
            </tr>
            <tr>
                <td></td>
                <td colspan="2" align="left" class="TitleText">Select the Income Contributors below to link to the Affordability Assessment.
                </td>
                <td></td>
                <td align="left" class="TitleText">If there are any Non-Applicant Income Contributors, Select or Add them below.
                </td>
                <td></td>
            </tr>
            <tr>
                <td></td>
                <td colspan="2">
                    <asp:Panel ID="pnlApplicantContributors" runat="server" GroupingText=" " TabIndex="1">
                        <SAHL:SAHLCheckboxList ID="chklApplicantContributors" runat="server" Style="width: 400px" CssClass="chklist"
                            CellPadding="1" CellSpacing="1" RepeatColumns="1" RepeatDirection="Vertical" RepeatLayout="Flow">
                        </SAHL:SAHLCheckboxList>
                    </asp:Panel>
                </td>
                <td></td>
                <td align="right">
                    <asp:Panel ID="pnlNonApplicantContributors" runat="server" GroupingText=" ">
                        <SAHL:SAHLDropDownList runat="server" ID="ddlLegalEntity" PleaseSelectItem="true" PleaseSelectTextOverride="- select the applicant to relate the non-applicant income contributor to -" />
                        <SAHL:SAHLButton ID="btnAddContributor" runat="server" Text="Add" OnClick="btnAddContributor_Click" ToolTip="Add a related legal entity as an income contributor" />
                        <br />
                        <SAHL:SAHLCheckboxList ID="chklNonApplicantContributors" runat="server" Style="width: 400px" CssClass="chklist"
                            CellPadding="1" CellSpacing="1" RepeatColumns="1" RepeatDirection="Vertical" RepeatLayout="Flow">
                        </SAHL:SAHLCheckboxList>
                    </asp:Panel>
                </td>
                <td></td>
            </tr>
            <tr>
                <td></td>
                <td align="left" class="TitleText">Contributing Applicants:&nbsp&nbsp<SAHL:SAHLTextBox ID="txtNumberOfContributingApplicants" runat="server" DisplayInputType="Number" TextAlign="Right" Width="20px" MaxLength="2" TabIndex="2" /></td>
                <td colspan="4"></td>
            </tr>
            <tr>
                <td></td>
                <td align="left" class="TitleText">Household Dependants:&nbsp&nbsp&nbsp<SAHL:SAHLTextBox ID="txtNumberOfHouseholdDependants" runat="server" DisplayInputType="Number" TextAlign="Right" Width="20px" MaxLength="2" TabIndex="3" /></td>
                <td colspan="4"></td>
            </tr>
            <tr>
                <td colspan="4"></td>
                <td align="right">
                    <SAHL:SAHLButton ID="btnSubmit" runat="server" Text="Save" OnClick="btnSubmit_Click" TabIndex="4" />
                    <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="False" TabIndex="5" />
                </td>
                <td></td>
            </tr>
        </table>
    </div>
</asp:Content>