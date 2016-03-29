<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Codebehind="RuleDetails.aspx.cs" Inherits="SAHL.Web.Views.Administration.AddRule" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <table width="790" runat="server" id="tblSearch" visible="true">
        <tr>
            <td width="20%">
                <SAHL:SAHLLabel ID="SAHLLabel1" runat="server" CssClass="LabelText" Font-Bold="True"
                    Font-Names="Arial" Font-Size="Medium" Font-Underline="True" Text="Rule Name"
                    TextAlign="Left"></SAHL:SAHLLabel></td>
            <td colspan="2">
                <SAHL:SAHLTextBox runat="server" ID="txtRuleName" Mandatory="True" Width="550px"></SAHL:SAHLTextBox>
                <SAHL:SAHLRequiredFieldValidator ID="rfvSearch" runat="server" ControlToValidate="txtRuleName" />
                &nbsp;<SAHL:SAHLButton runat="server" Text="Search" ID="btnSearch" OnClick="btnSearch_OnClick" />&nbsp;
                </td>
        </tr>
    </table>
    <table runat="server" id="tblRules" visible="false" width="790">
        <tr>
            <td colspan="3">
                <SAHL:SAHLGridView ID="RulesGrid" runat="server" AutoGenerateColumns="false" EmptyDataSetMessage="There are no Rules Defined."
                    EnableViewState="false" FixedHeader="false" GridHeight="300px" GridWidth="1200px" Width="100%" 
                    HeaderCaption="Rule" NullDataSetMessage="" EmptyDataText="There are no Rules Defined."
                    OnGridDoubleClick="RulesGrid_GridDoubleClick" PostBackType="DoubleClick">
                    <HeaderStyle CssClass="TableHeaderB" />
                    <RowStyle CssClass="TableRowA" />
                </SAHL:SAHLGridView>
            </td>
        </tr>
    </table>
    <table id="tblMaintRule" runat="server" visible="false" width="790">
        <tr>
			<td colspan="2">
				<input runat="server" type="hidden" id="txtMaintRuleKey" value=""/>
			</td>
		</tr>
        <tr>
            <td width="20%">
				<SAHL:SAHLLabel runat="server" ID="label1">Enforce Rule</SAHL:SAHLLabel>
            </td>
            <td>
				<SAHL:SAHLDropDownList ID="ddlEnforce" runat="Server" Mandatory="true" CssClass="mandatory" PleaseSelectItem="false"></SAHL:SAHLDropDownList>
            </td>
        </tr>
        <tr>
            <td width="20%">
                <SAHL:SAHLLabel ID="SAHLLabel2" runat="server">Rule Status</SAHL:SAHLLabel>
            </td>
            <td>
                <SAHL:SAHLDropDownList ID="ddlStatus" runat="Server" Mandatory="true" CssClass="mandatory" PleaseSelectItem="false"></SAHL:SAHLDropDownList>
            </td>
        </tr>
        <tr>
            <td width="20%">
				<SAHL:SAHLLabel runat="server" ID="SAHLLabel3">Rule Status Reason</SAHL:SAHLLabel>
            </td>
            <td>
				<SAHL:SAHLTextBox runat="Server" ID="txtRuleStatusReason" width="600px" Mandatory="true" CssClass="mandatory"></SAHL:SAHLTextBox>
				<SAHL:SAHLRequiredFieldValidator ID="SAHLRequiredFieldValidator1" runat="server" ControlToValidate="txtRuleStatusReason" ErrorMessage="*" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="left">
            <SAHL:SAHLButton runat="server" Text="Submit" ID="btnMaint" OnClick="btnSubmitRule_Click" />
            </td>
        </tr>
    </table>
    <table runat="server" id="tblRuleParams" visible="false" width="790">
        <tr>
            <td colspan="2">
                <SAHL:SAHLGridView ID="RulesParamGrid" runat="server" AutoGenerateColumns="false"
                    EmptyDataSetMessage="There are no Rule Parameters Defined." EnableViewState="false"
                    FixedHeader="false" GridHeight="100px" GridWidth="1200px" Width="100%" HeaderCaption="Rule" NullDataSetMessage=""
                    EmptyDataText="There are no Rule Parameters Defined." OnGridDoubleClick="RulesParamGrid_GridDoubleClick"
                    PostBackType="DoubleClick">
                    <HeaderStyle CssClass="TableHeaderB" />
                    <RowStyle CssClass="TableRowA" />
                </SAHL:SAHLGridView>
            </td>
        </tr>
    </table>
    <table id="tblMaintParam" runat="server" visible="false" width="790">
        <tr><td colspan="2"><input runat="server" type="hidden" id="txtMaintRuleParamKey" value=""/></td></tr>
        <tr>
            <td width="20%" align="left"><SAHL:SAHLLabel runat="server" ID="SAHLLabel5">Parameter Name</SAHL:SAHLLabel>
            </td>
            <td><SAHL:SAHLTextBox runat="Server" ID="txtPMaintName" Width="577px"></SAHL:SAHLTextBox>
            <SAHL:SAHLRequiredFieldValidator ID="SAHLRequiredFieldValidator5" runat="server" ControlToValidate="txtPMaintName" ErrorMessage="*" />
            </td>
        </tr>
        <tr>
        <td width="20%" align="left"><SAHL:SAHLLabel runat="server" ID="SAHLLabel7">Type</SAHL:SAHLLabel>
            </td>
            <td>
            <SAHL:SAHLDropDownList runat="server" ID="ddlParamType" PleaseSelectItem="False"></SAHL:SAHLDropDownList>
            </td>
        </tr>
        <tr>
            <td width="20%" align="left"><SAHL:SAHLLabel runat="server" ID="SAHLLabel6">Value</SAHL:SAHLLabel>
            </td>
            <td><SAHL:SAHLTextBox runat="Server" ID="txtPMaintVal" Width="577px"></SAHL:SAHLTextBox>
            <SAHL:SAHLRequiredFieldValidator ID="SAHLRequiredFieldValidator6" runat="server" ControlToValidate="txtPMaintVal" ErrorMessage="*" />
            </td>
        </tr>
        <tr>
        <td colspan="2" align="left">
            <SAHL:SAHLButton runat="server" Text="Submit" ID="btnSubmitParam" Visible="false" OnClick="btnSubmitParam_Click" />
            </td>
        </tr>
      </table>
</asp:Content>
