<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    CodeBehind="WorkflowSuperSearch.aspx.cs" Inherits="SAHL.Web.Views.Common.WorkflowSuperSearch"
    Title="Untitled Page" EnableEventValidation="false" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <script language="javascript" type="text/javascript">

        function addWorkflowState() {
            var ddlWorkflows = document.getElementById('<%= ddlWorkflows.ClientID %>');
            var ddlStates = document.getElementById('<%= ddlStates.ClientID %>');
            var lstWorkflowStates = document.getElementById('<%= lstWorkflowStates.ClientID %>');

            // make sure a valid workflow and state have been selected
            var optWorkflow = ddlWorkflows.options[ddlWorkflows.selectedIndex];
            var optState = ddlStates.options[ddlStates.selectedIndex];
            if (optWorkflow.value == '-select-' || optState.value == '-1') {
                alert('You need to select a valid workflow and state.');
                return;
            }

            // run through the items and ensure it hasn't been added already - if it has just exit out
            var optionValue = '<%= WorkflowFilterOptionValueFormat %>'.replace('{0}', optWorkflow.value).replace('{1}', optState.value);
            for (var i = 0; i < lstWorkflowStates.options.length; i++) {
                if (lstWorkflowStates.options[i].value == optionValue)
                    return;
            }

            // if the list only contains one item, then clear it as it's the default message
            if (lstWorkflowStates.options.length == 1 && lstWorkflowStates.options[0].value == '-1')
                lstWorkflowStates.options[0] = null;

            // add the new option to the target box
            var optionText = '<%= WorkflowFilterOptionTextFormat %>'.replace('{0}', optWorkflow.text).replace('{1}', optState.text);
            lstWorkflowStates.options[lstWorkflowStates.options.length] = new Option(optionText, optionValue);

            updateWorkflowStates();
        }

        function clearWorkflowStates() {
            var ddl = document.getElementById('<%= lstWorkflowStates.ClientID %>');
            var items = ddl.options.length;
            while (items > 0) {
                ddl.options[0] = null;
                items--;
            }
        }

        // event handler when one of the header images are clicked
        function clickHeader(imageId) {
            document.getElementById(imageId).click();
        }

        // initialisation function for the screen    
        function init() {
            // add an event handler for when the selected workflow changes - this needs to be done this way 
            // rather than in markup otherwise it interferes with the cascading extenders
            var ddlWorkflows = document.getElementById('<%=ddlWorkflows.ClientID %>');
            if (ddlWorkflows != null)
                registerEvent(ddlWorkflows, 'change', clearWorkflowStates);
        }
        registerEvent(window, 'load', init);

        function removeWorkflowState() {
            var lstWorkflowStates = document.getElementById('<%= lstWorkflowStates.ClientID %>');

            for (var i = lstWorkflowStates.options.length - 1; i >= 0; i--) {

                if (lstWorkflowStates.options[i].selected)
                    lstWorkflowStates.options[i] = null;
            }

            // if there are no options, add the default option back
            if (lstWorkflowStates.options.length == 0) {
                var option = new Option('No workflow states selected', '-1');
                option.className = 'disabled';
                lstWorkflowStates.options[0] = option;
            }

            updateWorkflowStates();
        }

        // called when the page unloads - this moves the values in the workflow/state list to 
        // a hidden value - we need to do this as list values are not posted through unless 
        // they're selected and it's just easier to do it this way and rely on the value from 
        // the hidden control
        function updateWorkflowStates() {
            var lstWorkflowStates = document.getElementById('<%= lstWorkflowStates.ClientID %>');
            var hidWorkflowStates = document.getElementById('<%= hidWorkflowStates.ClientID %>');
            var allValues = '';

            for (var i = 0; i < lstWorkflowStates.options.length; i++) {
                if (allValues.length > 0)
                    allValues += ',';
                allValues += lstWorkflowStates.options[i].value;
            }
            if (allValues == '-1')
                allValues = '';
            hidWorkflowStates.value = allValues;
        }

        function tabChanged(sender, args) {
            var hidActiveTabIndex = document.getElementById('<%= hidActiveTabIndex.ClientID %>');
            hidActiveTabIndex.value = sender.get_activeTabIndex();
        }
   
    </script>
    <ajaxToolkit:TabContainer ID="tcMain" runat="server" OnClientActiveTabChanged="tabChanged"
        ActiveTabIndex="1">
        <ajaxToolkit:TabPanel ID="tabFilter" runat="server" HeaderText="Filters">
            <ContentTemplate>
                <table width="100%" class="tableStandard" style="font-size: 85%">
                    <tr>
                        <td>
                            <asp:Panel ID="pnlMain" runat="server" Width="99%">
                                <div class="headerPanel row">
                                    <div class="cell">
                                        <asp:Image ID="imgCaseFilter" runat="server" ImageUrl="~/Images/arrow_blue_down.gif"
                                            Style="cursor: pointer;" TabIndex="0" /></div>
                                    <div class="cell" style="margin-left: 5px; height: 16px; vertical-align: middle;
                                        cursor: hand;" onclick="clickHeader('<%=imgCaseFilter.ClientID%>')">
                                        Case Filters</div>
                                </div>
                                <asp:Panel ID="pnlCaseFilter" runat="server" Style="padding: 3px;">
                                    <table class="tableStandard">
                                        <tr>
                                            <td class="TitleText" style="width: 130px;">
                                                Search In:
                                            </td>
                                            <td class="cellInput" style="width: 200px">
                                                <asp:DropDownList ID="ddlSearchIn" runat="server" Width="199px" OnSelectedIndexChanged="ddlSearchIn_SelectedIndexChanged"
                                                    AutoPostBack="True" TabIndex="1" />
                                            </td>
                                            <td style="width: 30px">
                                            </td>
                                            <td style="width: 110px">
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkHistoricUsers" runat="server" Checked="true" Text="Include historic users"
                                                    TextAlign="Right" Visible="true" ToolTip="Search will include all users that have played a role on a case since creation"
                                                    TabIndex="2"></asp:CheckBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Application Number:
                                            </td>
                                            <td>
                                                <SAHL:SAHLTextBox ID="txtApplicationNo" runat="server" DisplayInputType="Number"
                                                    MaxLength="10" TabIndex="3"></SAHL:SAHLTextBox>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                Application Type:
                                            </td>
                                            <td>
                                                <SAHL:SAHLDropDownList ID="ddlAppTypes" runat="server" Width="177px" PleaseSelectItem="false"
                                                    TabIndex="4">
                                                </SAHL:SAHLDropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Name:
                                            </td>
                                            <td>
                                                <SAHL:SAHLTextBox ID="txtFirstName" runat="server" MaxLength="50" TabIndex="5"></SAHL:SAHLTextBox>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                Surname:
                                            </td>
                                            <td>
                                                <SAHL:SAHLTextBox ID="txtSurname" runat="server" MaxLength="50" TabIndex="6"></SAHL:SAHLTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                ID Number:
                                            </td>
                                            <td>
                                                <SAHL:SAHLTextBox ID="txtIDNumber" runat="server" DisplayInputType="Number" MaxLength="13"
                                                    TabIndex="7"></SAHL:SAHLTextBox>
                                            </td>
                                            <td colspan="3">
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <ajaxToolkit:CollapsiblePanelExtender ID="cpCaseFilter" runat="Server" TargetControlID="pnlCaseFilter"
                                    Collapsed="False" CollapsedSize="0" ImageControlID="imgCaseFilter" ExpandedImage="~/images/arrow_blue_down.gif"
                                    CollapsedImage="~/images/arrow_blue_right.gif" ExpandControlID="imgCaseFilter"
                                    CollapseControlID="imgCaseFilter" ExpandDirection="Vertical" />
                                <div>
                                    &nbsp;</div>
                                <div class="headerPanel row">
                                    <div class="cell">
                                        <asp:Image ID="imgWorkflowFilter" runat="server" ImageUrl="~/Images/arrow_blue_down.gif"
                                            Style="cursor: pointer;" TabIndex="8" /></div>
                                    <div class="cell" style="margin-left: 5px; height: 16px; vertical-align: middle;
                                        cursor: hand;" onclick="clickHeader('<%=imgWorkflowFilter.ClientID%>')">
                                        Workflow Filters</div>
                                </div>
                                <asp:Panel ID="pnlWorkflowFilter" runat="server" Style="padding: 3px;">
                                    <table class="tableStandard">
                                        <tr runat="server" id="rowWorkflows">
                                            <td colspan="5">
                                                <div class="TitleText">
                                                    Workflows:</div>
                                                <div class="borderAll" style="margin-top: 3px; padding: 3px;">
                                                    <table width="100%" class="tableStandard">
                                                        <tr>
                                                            <td style="padding-right: 10px;">
                                                                Workflow:
                                                            </td>
                                                            <td>
                                                                <SAHL:SAHLDropDownList ID="ddlWorkflows" runat="server" PleaseSelectItem="true" Width="200px"
                                                                    TabIndex="9" />
                                                            </td>
                                                            <td rowspan="2" style="width: 30px; vertical-align: middle; text-align: center;">
                                                                <a href="#" onclick="addWorkflowState()">
                                                                    <img src="../../Images/arrow_green_right.gif" alt="arrow_right" title="Add" class="action"
                                                                        tabindex="11" /></a>
                                                                <br />
                                                                <a href="#" onclick="removeWorkflowState()">
                                                                    <img src="../../Images/arrow_green_left.gif" alt="arrow_left" title="Remove" tabindex="12" /></a>
                                                            </td>
                                                            <td rowspan="2">
                                                                <asp:ListBox runat="server" ID="lstWorkflowStates" Rows="5" SelectionMode="Multiple"
                                                                    TabIndex="13">
                                                                    <asp:ListItem Value="-1" Text="No workflow states selected" class="disabled">
                                                                    </asp:ListItem>
                                                                </asp:ListBox>
                                                                <asp:HiddenField ID="hidWorkflowStates" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr style="margin-top: 3px;">
                                                            <td style="padding-right: 10px;">
                                                                State:
                                                            </td>
                                                            <td>
                                                                <SAHL:SAHLDropDownList ID="ddlStates" runat="server" Width="200px" TabIndex="10" />
                                                                <ajaxToolkit:CascadingDropDown ID="ccdStates" runat="server" TargetControlID="ddlStates"
                                                                    Category="Workflow" PromptText="- Please select -" PromptValue="-1" ServicePath="~/AJAX/X2.asmx"
                                                                    ServiceMethod="GetWorkFlowStatesByWorkFlowKey" ParentControlID="ddlWorkflows"
                                                                    EmptyValue="-1" EmptyText="- Please select -" LoadingText="[Loading...]" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="rowOrgStructLabel">
                                            <td class="TitleText" colspan="5" style="padding-bottom: 3px; padding-top: 5px;">
                                                Organisational Structure:
                                            </td>
                                        </tr>
                                        <tr runat="server" id="rowOrgStruct">
                                            <td colspan="5" class="borderAll">
                                                <SAHL:SAHLTreeView runat="server" ID="tvOrgStruct" CheckBoxesVisible="true" Style="height: 110px;
                                                    overflow-y: scroll;" TabIndex="13" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <ajaxToolkit:CollapsiblePanelExtender ID="cpWorkflowFilter" runat="Server" TargetControlID="pnlWorkflowFilter"
                                    Collapsed="True" CollapsedSize="0" ImageControlID="imgWorkflowFilter" ExpandedImage="~/images/arrow_blue_down.gif"
                                    CollapsedImage="~/images/arrow_blue_right.gif" ExpandControlID="imgWorkflowFilter"
                                    CollapseControlID="imgWorkflowFilter" ExpandDirection="Vertical" />
                                <div style="padding-top: 5px; float: left;" class="row">
                                    <div style="float: left; display: inline; padding-left: 5px;">
                                        Maximum records returned:
                                        <SAHL:SAHLDropDownList ID="selMaxResults" runat="server">
                                            <asp:ListItem Value="50" Selected="True">50</asp:ListItem>
                                            <asp:ListItem Value="100">100</asp:ListItem>
                                            <asp:ListItem Value="150">150</asp:ListItem>
                                            <asp:ListItem Value="200">200</asp:ListItem>
                                            <asp:ListItem Value="250">250</asp:ListItem>
                                            <asp:ListItem Value="300">300</asp:ListItem>
                                            <asp:ListItem Value="350">350</asp:ListItem>
                                            <asp:ListItem Value="400">400</asp:ListItem>
                                            <asp:ListItem Value="450">450</asp:ListItem>
                                            <asp:ListItem Value="500">500</asp:ListItem>
                                        </SAHL:SAHLDropDownList>
                                    </div>
                                    <div style="float: right; display: inline; text-align: right;">
                                        <SAHL:SAHLButton ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click"
                                            TabIndex="14" />
                                    </div>
                                </div>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="tabResults" runat="server" HeaderText="Results">
            <ContentTemplate>
                <div style="font-size: 85%;">
                    <div id="divMaxResultsError" runat="server" class="row" visible="False" style="padding-bottom: 5px;">
                        <div class="cell">
                            <img src="../../images/flag_red.png" width="16" height="16" alt="" /></div>
                        <strong>Note: </strong>The query returned more than the selected limit of
                        <asp:Label ID="lblMaxCount" runat="server" />
                        cases. Please refine your search criteria.
                    </div>
                    <SAHL:DXGridView ID="grdResults" runat="server" AutoGenerateColumns="False" Width="100%"
                        EnableViewState="False" KeyFieldName="InstanceID" PostBackType="DoubleClick"
                        OnSelectionChanged="grdResults_SelectionChanged" OnLoad="grdResults_Load">
                        <SettingsText Title="Search Results" />
                        <Settings ShowGroupPanel="True" />
                        <Columns>
                            <SAHL:DXGridViewFormattedTextColumn FieldName="InstanceID" Caption="ID" Visible="False"
                                Format="GridString" FormatString="" />
                            <SAHL:DXGridViewFormattedTextColumn FieldName="IsCapitec" Caption=" " Width="1%" VisibleIndex="0"/>
                            <SAHL:DXGridViewFormattedTextColumn FieldName="AppKey" Caption="App No." Width="10%"
                                Format="GridString" FormatString="" VisibleIndex="1" />                       
                            <SAHL:DXGridViewFormattedTextColumn FieldName="AppDetails" Caption="App Details"
                                Width="30%" Format="GridString" FormatString="" VisibleIndex="2" />
                            <SAHL:DXGridViewFormattedTextColumn FieldName="Workflow" Caption="Workflow" Width="15%"
                                Format="GridString" FormatString="" VisibleIndex="3" />
                            <SAHL:DXGridViewFormattedTextColumn FieldName="Stage" Caption="Stage" Width="15%"
                                Format="GridString" FormatString="" VisibleIndex="4" />
                            <SAHL:DXGridViewFormattedTextColumn FieldName="AppType" Caption="App Type" Width="15%"
                                Format="GridString" FormatString="" VisibleIndex="5" />
                            <SAHL:DXGridViewFormattedTextColumn FieldName="AssignedTo" Caption="Assigned To"
                                Width="15%" Format="GridString" FormatString="" VisibleIndex="6" />
                        </Columns>
                    </SAHL:DXGridView>
                    <div style="padding-top: 5px; padding-left: 5px;">
                        <span id="spanResultCount" runat="server" style="font-weight: bold;">0</span> instances
                        found.</div>
                </div>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
    </ajaxToolkit:TabContainer>
    <!--
    <br /><br /><br /><br /><br /><br /><br /><br />
    
    <asp:Panel ID="Panel3" runat="server" Height="50px" Style="display: inline" Width="770px">
        <asp:Panel ID="Panel2" runat="server" Width="280px" Style="display: inline">
        </asp:Panel>
        &nbsp;
        <asp:Panel ID="pnlSavedSearches" runat="server" Width="430px" Style="display: inline" Visible = "false">
            <asp:Label ID="Label2" runat="server" Text="Saved Searches:" CssClass="TitleText"></asp:Label><asp:DropDownList
                ID="ddlSavedSearches" runat="server" Width="177px">
            </asp:DropDownList>&nbsp;&nbsp;<SAHL:SAHLButton ID="btnSave" runat="server" Text="Save "
                Width="71px" OnClick="btnSave_Click" /><SAHL:SAHLButton ID="btnManage" runat="server"
                    Text="Manage" OnClick="btnManage_Click" />
                    </asp:Panel>
    </asp:Panel>
  
    <br />
    <br />
    <asp:Table ID="tblFilter" runat="server" Width="770px" CssClass="tableStandard">
        <asp:TableRow runat="server" ID="trchkIncludeMyApps">
            <asp:TableCell runat="server" ColumnSpan="2">
                <asp:CheckBox ID="chkIncludeMyCreatedCases" runat="server" Text="Include open applications I created"
                    TextAlign="Left" Visible = "false"></asp:CheckBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRowLe" runat="server">
            <asp:TableCell Width="30%">
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" ID="trorgStructFilter">
            <asp:TableCell ColumnSpan="2">
                <table width="100%">
                    <tbody>
                        <tr>
                            <td style="width: 48%">
                                Organsational Structure:
                            </td>
                            <td style="width:4%"></td>
                            <td style="width: 48%">
                                Selected Users:
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 48%">
                                <asp:Panel runat="server" ScrollBars="Vertical" ID="pnltvOrgStruct" Width="338px" Height="80px"
                                    Style="overflow-y: scroll" BorderWidth="1px" BorderStyle="Solid" BorderColor="Black">
                                </asp:Panel>
                            </td>
                            <td style="width: 4%">
                            </td>
                            <td style="width: 48%; text-align: right">
                                <asp:Panel ID="pnlUsers" runat="server" Height="80px" BorderWidth="1px" BorderStyle="Solid"
                                    BorderColor="Black" Width="338px">
                                    <asp:CheckBoxList ID="chklstUsers" runat="server" Width="100%" RepeatLayout="Flow" TEXT-ALIGN ="right">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2">
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2">
                <SAHL:SAHLButton ID="btnReset" Text="Reset" runat="server" OnClick="btnReset_Click" Width="65px" />&nbsp;
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    &nbsp;
    <div style="width: 770px">
    </div>
    -->
    <asp:HiddenField runat="server" ID="hidSelectedWorkflow" />
    <asp:HiddenField runat="server" ID="hidSelectedWorkflowState" />
    <asp:HiddenField runat="server" ID="hidActiveTabIndex" />
</asp:Content>
