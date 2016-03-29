<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Blank.Master"
    CodeBehind="DocumentChecklist.aspx.cs" Inherits="SAHL.Web.Views.DebtCounselling.DocumentChecklist" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <script src="../../Scripts/jquery-1.5.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function onRowSel(rowId) {
            //debugger;
            if (rowId > -1 &&
                (grid.GetDataRow(rowId).cells[0].outerText == "17.1"
                    || grid.GetDataRow(rowId).cells[0].outerText == "17.2")) {
                //Need to set the data
                $("#<%= pnlSave.ClientID %>").attr("style", "display:inline");
                $("#<%= lblDteDesc.ClientID %>").text("Date: " + grid.GetDataRow(rowId).cells[0].outerText);

                if (grid.GetDataRow(rowId).cells[0].outerText == "17.1") {
                    $("#<%= txtType.ClientID %>").val("0");
                }
                else {
                    $("#<%= txtType.ClientID %>").val("1");
                }


                $("#<%= dteNewDate.ClientID %>").val("");
                $("#<%= txtComments.ClientID %>").text("");


            }
            else {
                $("#<%= pnlSave.ClientID %>").attr("style", "display:none");
            }
        }
        
        function  OnGridRowSelected(s, e)
        {
            onRowSel(e.visibleIndex);
        }
    </script>
    <asp:Panel runat="server" ID="pnlGrid">
        <SAHL:DXGridView runat="server" 
        ID="grdDate" ClientInstanceName="grid" Settings-ShowTitlePanel="true"
            SettingsText-Title="Date Summary" SettingsText-EmptyDataRow="No dates saved yet"
            Width="99%">
            <ClientSideEvents SelectionChanged="function(s, e) {
                                                      if(grid.GetSelectedRowCount() != 0)
                                                            OnGridRowSelected(s, e);
                                                }" />
        </SAHL:DXGridView>
        <asp:Panel runat="server" ID="pnlSave" GroupingText="Save Date" style="display: none">
            <table width="100%" border="1">
                <tr>
                    <td class="TitleText" width="80px" style="vertical-align:top">
                        <SAHL:SAHLLabel runat="server" ID="lblDteDesc" />
                        <SAHL:SAHLTextBox runat="server" ID="txtType" style="display:none" />
                    </td>
                    <td class="LabelText" width="120px" style="vertical-align:top">
                        <SAHL:SAHLDateBox runat="server" ID="dteNewDate" />
                    </td>
                    <td class="LabelText">
                        <SAHL:SAHLTextBox runat="server" ID="txtComments" TextMode="MultiLine" Width="98%" Height="64px" MaxLength="2000" />
                    </td>
                </tr>
                <tr>
                    <td align="right" colspan="3">
                        <SAHL:SAHLButton runat="server" ID="btnSave" Text="Save" OnClick="btnSubmit_Click" />
                        <SAHL:SAHLButton runat="server" ID="btnCancel" Text="Cancel" OnClientClick="onRowSel(-1); return false;" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
