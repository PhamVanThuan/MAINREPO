<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true" CodeBehind="SubsidyDetails.aspx.cs" Inherits="SAHL.Web.Views.Common.SubsidyDetails" Title="Untitled Page" EnableViewState="False" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <script language="javascript" type="text/javascript">
        function SetGEPFMemberEnabled(subsidyProvider) {
            var gepfCheckbox = document.getElementById('<%=chkGEPFMember.ClientID%>');

            if (subsidyProvider.value.indexOf("(GEPF)") > -1) {
                gepfCheckbox.disabled = false;
            }
            else {
                gepfCheckbox.checked = false;
                gepfCheckbox.disabled = true;
            }
        }
    </script>
    <asp:Panel ID="pnlGrid" runat="server" Visible="true" Width="99%">
        <SAHL:SAHLGridView ID="grdSubsidy" runat="server" AutoGenerateColumns="false" FixedHeader="false"
            EnableViewState="false" GridHeight="100px" GridWidth="100%" Width="100%"
            PostBackType="SingleAndDoubleClick"
            SelectFirstRow="false"
            HeaderCaption="Subsidy Details"
            NullDataSetMessage=""
            EmptyDataSetMessage="There are no subsidy records."
            OnSelectedIndexChanged="grdSubsidy_SelectedIndexChanged" OnRowDataBound="grdSubsidy_RowDataBound">
            <RowStyle CssClass="TableRowA" />
        </SAHL:SAHLGridView>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlInput" runat="server" Style="margin-left: 10px;" Width="99%">

        <div class="row" style="width: 99%">
            <div class="cellInput titleText labelText" style="width: 220px">
                Account Number &nbsp
            </div>
            <div>
                <SAHL:SAHLLabel ID="lblAccountKey" runat="server"></SAHL:SAHLLabel>
                <span runat="server" id="spanAccountKey">
                    <SAHL:SAHLDropDownList ID="ddlAccountKey" runat="server" PleaseSelectItem="true" Mandatory="true" />
                    <span class="smaller">(items marked '*' are open applications)</span>
                </span>
            </div>
        </div>
        <div class="row" style="width: 99%">
            <div class="cellInput titleText labelText" style="width: 220px">Subsidy Provider &nbsp;</div>
            <div>
                <SAHL:SAHLLabel ID="lblSubsidyProvider" runat="server"></SAHL:SAHLLabel>
                <SAHL:SAHLTextBox ID="txtSubsidyProvider" runat="server" Style="width: 350" Mandatory="true" />
                <SAHL:SAHLAutoComplete ID="acSubsidyProvider" runat="server" ClientClickFunction="SetGEPFMemberEnabled" TargetControlID="txtSubsidyProvider" ServiceMethod="SAHL.Web.AJAX.Employment.GetSubsidyProviders" AutoPostBack="false" />
            </div>
        </div>
        <div class="row" style="width: 99%">
            <div class="cellInput titleText labelText" style="width: 220px">Salary / Persal No &nbsp;</div>
            <div>
                <SAHL:SAHLLabel ID="lblSalaryNumber" runat="server" CssClass="LabelText" Width="1px"></SAHL:SAHLLabel>
                <SAHL:SAHLTextBox ID="txtSalaryNumber" runat="server" Style="width: 220px" Mandatory="true" />
            </div>
        </div>
        <div class="row" style="width: 99%">
            <div class="cellInput titleText labelText" style="width: 220px">Paypoint &nbsp;</div>
            <div>
                <SAHL:SAHLLabel ID="lblPayPoint" runat="server"></SAHL:SAHLLabel>
                <SAHL:SAHLTextBox ID="txtPaypoint" runat="server" Style="width: 220px" />
            </div>
        </div>
        <div class="row" style="width: 99%">
            <div class="cellInput titleText labelText" style="width: 220px">Rank &nbsp;</div>
            <div>
                <SAHL:SAHLLabel ID="lblRank" runat="server"></SAHL:SAHLLabel>
                <SAHL:SAHLTextBox ID="txtRank" runat="server" Style="width: 220px" />
            </div>
        </div>
        <div class="row" style="width: 99%">
            <div class="cellInput titleText labelText" style="width: 220px">Notch &nbsp;</div>
            <div>
                <SAHL:SAHLLabel ID="lblNotch" runat="server"></SAHL:SAHLLabel>
                <SAHL:SAHLTextBox ID="txtNotch" runat="server" Width="220" />
            </div>
        </div>
        <div class="row" style="width: 99%">
            <div class="cellInput titleText labelText" style="width: 220px">Stop Order Amount &nbsp;</div>
            <div>
                <SAHL:SAHLLabel ID="lblStopOrderAmt" runat="server"></SAHL:SAHLLabel>
                <SAHL:SAHLCurrencyBox ID="currStopOrder" runat="server" Width="100" Mandatory="true" />
            </div>
        </div>
        <div class="row" style="width: 99%">
            <div class="cellInput titleText labelText" style="width: 220px">GEPF Member &nbsp;</div>
            <div>
                <SAHL:SAHLLabel ID="lblGEPFMember" runat="server"></SAHL:SAHLLabel>
                <asp:CheckBox ID="chkGEPFMember" runat="server" />
            </div>
        </div>
        <div class="row" id="divStatus" runat="server" style="width: 99%">
            <div class="cellInput titleText labelText" style="width: 220px">Status &nbsp;</div>
            <div>
                <asp:Label ID="lblStatus" runat="server" />
            </div>
        </div>
        <div class="buttonBar" style="width: 99%">
            <SAHL:SAHLButton ID="btnBack" runat="server" Text="Back" CssClass="buttonSpacer" CausesValidation="false" />
            <SAHL:SAHLButton ID="btnSave" runat="server" Text="Save" CssClass="buttonSpacer" />
            <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonSpacer" CausesValidation="false" />
        </div>
    </asp:Panel>
</asp:Content>