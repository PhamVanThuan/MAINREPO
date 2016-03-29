<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.ReleaseAndVariations.Conditions"
    Title="Release And Variations Conditions" Codebehind="ReleaseAndVariationsConditions.aspx.cs" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<%@ Register Src="WorkFlowHeader.ascx" TagName="WorkFlowHeader" TagPrefix="uc1" %>
<asp:Content id="Content2" ContentPlaceHolderID="Main" runat="server">
    <asp:UpdatePanel ID="UpdatePanel2"  runat="server">
        <ContentTemplate>
    <table style="width: 95%">
        <tr>
            <td align="right" colspan="2">
                <asp:Panel id="Panel2" runat="server" GroupingText="Release & Variation Conditions" Height="250px"
        width="99%">
                    <SAHL:SAHLGridView id="gridConditions" runat="server" gridheight="250px" gridwidth="100%"
            width="99%" autogeneratecolumns="False" OnSelectedIndexChanged="gridConditions_Click" HeaderCaption="The conditions below have been added: " BackColor="#FFFFC0" GridLines="Horizontal" HorizontalAlign="Left" PostBackType="SingleAndDoubleClick" SelectFirstRow="False">
                        <SelectedRowStyle BackColor="Black" />
                    </SAHL:SAHLGridView>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td align="right" colspan="2">
            <asp:Panel id="Panel3" runat="server" GroupingText="Release & Variation Condition Update" Height="50px"
        width="100%">
                    <SAHL:SAHLTextBox id="txtNotes" runat="server" rows="10" BackColor="#FFFFC0" textmode="MultiLine" width="100%"></SAHL:SAHLTextBox></asp:Panel>
            </td>
        </tr>
        <tr>
            <td align="right" colspan="2" style="text-align: right">
                <SAHL:SAHLButton id="btnAdd" runat="server" buttonsize="Size4"  text="Add New" width="150px" cssclass="BtnNormal4 " visible="False" OnClick="btnAdd_Click"/>
                <SAHL:SAHLButton id="btnUpdate" runat="server" buttonsize="Size4" text="Save" width="150px" cssclass="BtnNormal4 " visible="False" OnClick="btnUpdate_Click" />
                <SAHL:SAHLButton id="btnCancel" runat="server" text="Done" width="147px" buttonsize="Size4" cssclass="BtnNormal4" visible="False" OnClick="btnCancel_Click" /></td>
        </tr>
    </table>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>