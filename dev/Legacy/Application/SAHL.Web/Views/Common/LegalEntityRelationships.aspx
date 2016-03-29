<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.Common.LegalEntityRelationships" Title="Untitled Page" Codebehind="LegalEntityRelationships.aspx.cs" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
<table border="0" cellspacing="0" cellpadding="0" class="TableStandard" width="100%"><tr><td align="left" style="height:99%;" valign="top">
    
    <SAHL:SAHLGridView ID="grdRelatedLegalEntities" runat="server" AutoGenerateColumns="false" FixedHeader="false" 
        EnableViewState="false" GridHeight="200px" GridWidth="100%" Width="100%"
        HeaderCaption="Related Legal Entities"
        NullDataSetMessage="No Related Legal Entities to display."
        EmptyDataSetMessage="No Related Legal Entities to display." OnRowDataBound="grdRelatedLegalEntities_RowDataBound"
        OnGridDoubleClick="grdRelatedLegalEntities_GridDoubleClick" OnSelectedIndexChanged="grdRelatedLegalEntities_SelectedIndexChanged">
        <RowStyle CssClass="TableRowA" />
    </SAHL:SAHLGridView>
    <br />

    <table border="0" id="tblLEInfo" runat="server" width="100%">
        <tr>
            <td class="TitleText" style="width: 280px">
                Legal Entity to create Relationship with :</td>
            <td>
                <SAHL:SAHLLabel ID="lblMessage" runat="server" CssClass="LabelText">-</SAHL:SAHLLabel>
            </td>
        </tr>
    </table>
    <br />
    <table border="0" id="tblActionTable" runat="server" width="100%">
        <tr>
            <td class="TitleText" style="width:150px">
                Relationship Type :
            </td>
            <td>
                <SAHL:SAHLDropDownList ID="ddlRelationshipType" runat="server" CssClass="CboText" Width="219px">
                </SAHL:SAHLDropDownList>
           </td>
        </tr>
    </table>

</td></tr>
<tr id="trButtonRow" runat="server"><td align="right">
    <SAHL:SAHLButton ID="btnAddToCbo" runat="server" Text="Add to Menu" AccessKey="A" OnClick="btnAddToCbo_Click" CausesValidation="False" />
    <SAHL:SAHLButton ID="btnSubmitButton" runat="server" Text="Submit" AccessKey="S" OnClick="btnSubmitButton_Click" />
    <SAHL:SAHLButton ID="btnCancelButton" runat="server" Text="Cancel" AccessKey="C" OnClick="btnCancelButton_Click" CausesValidation="False" />
</td></tr></table>
</asp:Content>