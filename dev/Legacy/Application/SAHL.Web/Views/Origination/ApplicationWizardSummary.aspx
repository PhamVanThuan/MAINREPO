<%@ Page Language="C#" MasterPageFile="~/MasterPages/SAHL.Master" AutoEventWireup="true" CodeBehind="ApplicationWizardSummary.aspx.cs" Inherits="SAHL.Web.Views.Origination.ApplicationWizardSummary" Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <SAHL:SAHLGridView ID="grdLoan" runat="server" Height="50" GridWidth="100%" Width="100%" AutoGenerateColumns="False" HeaderCaption="Application Details">
    </SAHL:SAHLGridView>
    &nbsp;<br />
    <br />
    <SAHL:SAHLGridView ID="grdLegalEntity" runat="server" AutoGenerateColumns="False"
        GridWidth="100%" Height="37px" PostBackType="SingleClick" Width="100%">
    </SAHL:SAHLGridView>
    <br />
    <br />
    <asp:Panel ID="Panel1" runat="server" Height="50px" Width="100%">
        <table style="width: 100%" border="2">
            <tr>
                <td style="width: 20%; height: 21px;">
                </td>                
                <td style="width: 20%; height: 21px">
                </td>               
                <td style="width: 20%">
                </td>
            </tr>
            <tr>
                <td style="width: 20%">
                    <SAHL:SAHLButton ID="btnCalculator" runat="server" Text="Update Calculations" OnClick="btnCalculator_Click" /></td>              
                <td style="width: 20%" align="right">
                    <SAHL:SAHLButton ID="btnUpdate" runat="server" Text="Update Applicant" OnClick="btnUpdate_Click" /></td>
                <td style="width:20%" align="left">
                    <SAHL:SAHLButton ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Next Applicant" /></td>                              
                <td style="width: 20%" align="right">
                    <SAHL:SAHLButton ID="btnFinish" runat="server" Text="Finish" OnClick="btnFinish_Click"  />&nbsp;</td>                
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
