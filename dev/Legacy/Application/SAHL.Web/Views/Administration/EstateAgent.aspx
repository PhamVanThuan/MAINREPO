<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="EstateAgent.aspx.cs" Inherits="SAHL.Web.Views.Administration.EstateAgent"
    Title="Untitled Page" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.2, Version=10.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <div style="text-align: center">
        <table class="tableStandard" width="100%">
            <tr>
                <td>
                    <SAHL:SAHLLabel ID="lblHeader" runat="server" Text="Estate Agencies / Agents" CssClass="LabelText" Font-Size="Medium"></SAHL:SAHLLabel>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="overflow: auto; height: 400px; width: 820px;">
                        <SAHL:DXTreeList ID="tlOrgStructure" runat="server" Width="800px" Height="400px" 
                            OnProcessDragNode="tlOrgStructure_ProcessDragNode" AutoGenerateColumns="False"
                            OnHtmlDataCellPrepared="tlOrgStructure_HtmlDataCellPrepared"
                            OnHtmlRowPrepared="tlOrgStructure_HtmlRowPrepared">
                            <Columns>
                                <SAHL:DXTreeListDataColumn Caption="Description" VisibleIndex="0" ReadOnly="True" Width="50px" Name="colImage">
                                    <DataCellTemplate>
                                        <dx:aspximage id="imgType" runat="server">
                                        </dx:aspximage>
                                        <SAHL:SAHLLabel id="lblOSDescription" runat="server">
                                        </SAHL:SAHLLabel>
                                    </DataCellTemplate>
                                </SAHL:DXTreeListDataColumn>

                                <SAHL:DXTreeListDataColumn FieldName="OSDescription" Caption="Description" VisibleIndex="1" Visible="False"/>
                                <SAHL:DXTreeListDataColumn FieldName="OSTypeDescription" Caption="Type" VisibleIndex="1" />
                                <SAHL:DXTreeListDataColumn FieldName="DisplayName" Caption="Display Name" VisibleIndex="2" />
                            </Columns>
                            <Settings GridLines="Both" ShowFooter="True"/>
                            <ClientSideEvents 
                                EndDragNode="function(s, e) {
	                            var btnAdd = document.getElementById('ctl00_Main_btnAdd');
	                            var btnUpdate = document.getElementById('ctl00_Main_btnUpdate');
	                            var btnRemove = document.getElementById('ctl00_Main_btnRemove');
                            	
	                            var allowedToDrag=false;
	                            
	                            var osType = e.targetElement.getAttribute('organisationType');
	                            if (osType == null || osType == '')
	                            {
	                                e.cancel=true;
	                            }
	                            else
	                            {
                            	    if (btnAdd!=null && (btnAdd.disabled==false || btnAdd.style.display=='inline'))
		                                allowedToDrag=true;
	                                else if (btnUpdate!=null && (btnUpdate.disabled==false || btnUpdate.style.display=='inline'))
		                                allowedToDrag=true;
	                                else if (btnRemove!=null && (btnRemove.disabled==false || btnRemove.style.display=='inline'))
	                                    allowedToDrag=true;
                                			
		                                if (allowedToDrag==true)
		                                {

        	                                e.cancel = !confirm('Are you sure you want to move this Estate Agent ?');
        	                            }
                                        else
                                        {
                                             alert('You do not have access to update/move Estate Agents.');
                                             e.cancel=true;
                                        }
                                }
                              }" 
                              NodeDblClick="function(s, e) 
                                            {
                                                var btnAddToCBO = document.getElementById('ctl00_Main_btnAddToCBO');
                                                if (btnAddToCBO!=null && (btnAddToCBO.disabled==false || btnAddToCBO.style.display=='inline'))
                                                    btnAddToCBO.click();                                                                                              
                                            }" 
                            />
                            
                        </SAHL:DXTreeList>
                    </div>
                </td>
            </tr>
            <tr id="AdminButtonRow" runat="server">
                <td align="center">
                    <SAHL:SAHLButton ID="btnAddToCBO" runat="server" Text="Add To Menu" 
                        CausesValidation="False" OnClick="btnAddToCBO_Click" 
                        ToolTip="can also double-click on an estate agency/agent to add to menu."/>
                    <SAHL:SAHLButton ID="btnAdd" runat="server" Text="Add" CausesValidation="False" SecurityTag="EstateAgentAdd" SecurityDisplayType="Disable" OnClick="btnAdd_Click"/>
                    <SAHL:SAHLButton ID="btnRemove" runat="server" Text="Remove" CausesValidation="False" SecurityTag="EstateAgentRemove" SecurityDisplayType="Disable" OnClick="btnRemove_Click" />
                    <SAHL:SAHLButton ID="btnUpdate" runat="server" Text="Update" CausesValidation="False" SecurityTag="EstateAgentUpdate" SecurityDisplayType="Disable" OnClick="btnUpdate_Click" />
                    <SAHL:SAHLButton ID="btnSelect" runat="server" Text="Select" CausesValidation="False" OnClick="btnSelect_Click" />
                    <SAHL:SAHLButton ID="btnView" runat="server" Text="View" CausesValidation="False" SecurityTag="EstateAgentView" SecurityDisplayType="Disable" OnClick="btnView_Click" />
                    <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False" OnClick="btnCancel_Click" />
                </td>
            </tr>
        </table>
    </div>
    <div class="cell" id="lblTip" runat="server" style="padding:2px;"><strong>Tip:</strong> drag and drop rows to move agency/agents.</div>
</asp:Content>
