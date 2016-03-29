<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true" 
    Codebehind="ClientSuperSearch.aspx.cs" Inherits="SAHL.Web.Views.Common.ClientSuperSearch"
    Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <script language="javascript" type="text/javascript">
    var cssSelectedRow = ' TableSelectRowA';
    var preventToggle = false;
    
    function cancelToggle()
    {
        preventToggle = true;
    }
    
    function changeSearch(searchName)
    {
        var isAdvanced = (searchName == 'ADVANCED');
        var isBasic = !isAdvanced;
      
        document.getElementById('tblAdvancedOptions').style.display = (isAdvanced ? 'block' : 'none');
        document.getElementById('tblBasicOptions').style.display = (isBasic ? 'block' : 'none');
        document.getElementById('<%= txtSearch.ClientID %>').style.display = (isBasic ? 'none' : 'inline');
        
        // clear tab styles
        clearTabStyles('cellTypeBasic');
        clearTabStyles('cellTypeAdvanced');
        
        // set the styles on the selected box
        var objTab = document.getElementById('cellTypeBasic');
        if (isAdvanced) 
            objTab = document.getElementById('cellTypeAdvanced');
            
        // clear out the unneccessary styles
        objTab.className = objTab.className.replace('backgroundDark', '');
        objTab.className = objTab.className.replace('borderRight', '');
        objTab.className = objTab.className.replaceAll('  ', ' ');
        if (objTab.className.indexOf('titleText') == -1)
            objTab.className = objTab.className + ' titleText';
        if (objTab.className.indexOf('backgroundLight') == -1)
            objTab.className = objTab.className + ' backgroundLight';
        objTab.className = objTab.className.trim();
        objTab.style.fontWeight = 'bold';
        objTab.style.fontStyle = 'normal';
        setCookie('<%= CookieSearchType %>', searchName);
        document.getElementById('<%=hidSearch.ClientID %>').value = searchName;
    }
    
    function clearTabStyles(objId)
    {
        var obj = document.getElementById(objId);
        obj.style.fontWeight = 'normal';
        obj.style.fontStyle = 'italic';
        obj.className = 'backgroundDark borderTop borderLeft borderRight borderBottom';
        if (obj.id.toLowerCase().indexOf('basic') > 0)
            obj.className = obj.className + ' ';
        else 
            obj.className = obj.className + ' ';
    }
    
    function doSearch(btnSearchId)
    {
        if (window.event.keyCode == 13)
        {
            window.event.returnValue = false;
            document.getElementById(btnSearchId).click();
        }
    }
    
    function getExpandImage(rowElement)
    {
        return rowElement.getElementsByTagName('img')[0];
    }
    
    function getExtraInfoDiv(rowElement)
    {
        var children = rowElement.getElementsByTagName('div');
        return children[4];
    }
    
    function init()
    {
        var searchType = getCookie('<%= CookieSearchType %>');
        if (searchType != null && searchType.length > 0)
            changeSearch(searchType);
    }
    registerEvent(window, 'load', init);
   
    function toggleExtraInfo(anchor, legalEntityKey)
    {
        var node = anchor.parentElement;
        var img = anchor.getElementsByTagName('img')[0];
        var rowElement = null;
        while (rowElement == null && node != null)
        {
            if (node.tagName.toLowerCase() == 'td')
                rowElement = node;
            else
                node = node.parentElement;
        }
        
        // work out where we're expanding or contracting
        var isExpanded = (img.src.indexOf('arrow_blue_down') > -1);
        setTimeout('doToggle("' + rowElement.id + '", "' + legalEntityKey + '", ' + !isExpanded + ')', 300);
    }
    
    function doToggle(rowElementID, legalEntityKey, expand)
    {
        // check for double-click first
        if (preventToggle)
        {
            preventToggle = false;
            return;
        }

        var rowElement = document.getElementById(rowElementID);  
        var divExtraInfo = getExtraInfoDiv(rowElement);

        // collapse the previously expanded div
        if (!expand) 
        {
            getExtraInfoDiv(rowElement).style.display = 'none';
            rowElement.className = rowElement.className.substring(0, rowElement.className.length - cssSelectedRow.length);
            getExpandImage(rowElement).src = '../../Images/arrow_blue_right.gif';
            return;
        }

        // check to see if we've loaded the div before, if not we need to fire off the AJAX call to 
        // populate the control
        if (divExtraInfo.innerHTML == '')
        {
            divExtraInfo.innerHTML = 'Loading data.....';
            // make the AJAX call
            SAHL.Web.AJAX.LegalEntity.GetClientSearchLegalEntityDetails(legalEntityKey, rowElementID, <%=addToCBO.ToString().ToLower() %>, clientDetailsCallback);        
        }
        
        // update the styles of the new element        
        divExtraInfo.style.display = 'inline';
        rowElement.className = rowElement.className + cssSelectedRow;
        getExpandImage(rowElement).src = '../../Images/arrow_blue_down.gif';
       
    }
    
    function clientDetailsCallback(result)
    {
        var rowElement = document.getElementById(result[0]);
        var divExtraInfo = getExtraInfoDiv(rowElement);
        divExtraInfo.innerHTML = result[1];
    }
    
    function openApplication(id)
    {
        if (confirm('Would you like to perform a workflow search on application ' + id + '?'))
        {
            var hid = document.getElementById('hidAppKey');
            hid.value = id;
            __doPostBack('hidAppKey', '');
        }
    }  
    
    </script>
    <asp:Panel ID="Panel1" runat="server" Width="100%" Style="display: inline;vertical-align: top;">
        <table>
            <tr>
                <td id="cellTypeBasic" class="backgroundLight titleText borderLeft borderTop" style="vertical-align:middle;height:22px;">
                    <a href="#" onclick="changeSearch('BASIC')" style="padding:5px;">Basic Search</a>
                </td>
                <td rowspan="3">
                    <table class="backgroundLight borderTop borderRight borderBottom" style="padding:2px;">
                        <tr>
                            <td style="padding:2px 5px 2px 10px;">
                                <table id="tblBasicOptions" class="tableStandard">
                                    <tr>
                                        <td style="padding-right:5px;">First Name: </td>
                                        <td style="padding-right:10px;">
                                            <SAHL:SAHLTextBox ID="txtFirstName" runat="server" MaxLength="100" />
                                        </td>
                                        <td style="padding-right:5px;">Surname: </td>
                                        <td><SAHL:SAHLTextBox ID="txtSurname" runat="server" MaxLength="100" /></td>
                                    </tr>
                                    <tr>
                                        <td style="padding-right:5px;">ID Number: </td>
                                        <td style="padding-right:10px;">
                                            <SAHL:SAHLTextBox ID="txtID" runat="server" MaxLength="100" DisplayInputType="Number" />
                                        </td>
                                        <td style="padding-right:5px;">Persal / Salary No.: </td>
                                        <td><SAHL:SAHLTextBox ID="txtSalaryNumber" runat="server" MaxLength="25" /></td>
                                    </tr>
                                    <tr>
                                        <td style="padding-right:5px;">Account Key: </td>
                                        <td><SAHL:SAHLTextBox ID="txtAccountKey" runat="server" MaxLength="100" DisplayInputType="Number" /></td>
                                        <td colspan="2" align="right"><SAHL:SAHLButton ID="btnSearchBasic" runat="server" Text="Search" OnClick="btnSearch_Click" /></td>
                                    </tr>
                                </table>
                                <table id="tblAdvancedOptions" style="display:none;" class="tableStandard">
                                    <tr>
                                        <td>Search for:</td>
                                        <td colspan="3"><SAHL:SAHLTextBox ID="txtSearch" runat="server" Width="99%"></SAHL:SAHLTextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="padding-right:5px;">Legal Entity Type: </td>
                                        <td style="padding-right:10px;">
                                            <SAHL:SAHLDropDownList ID="ddlLeType" runat="server" Font-Size="Smaller" PleaseSelectItem="false">
                                                <asp:ListItem>Person</asp:ListItem>
                                                <asp:ListItem>Company</asp:ListItem>
                                                <asp:ListItem Selected="True">Both</asp:ListItem>
                                            </SAHL:SAHLDropDownList>
                                        </td>
                                        <td id="cellAccountTypeLabel" runat="server" style="padding-right:5px;">Account Type: </td>
                                        <td id="cellAccountType" runat="server" align="right">
                                            <SAHL:SAHLDropDownList ID="cbxAccountType" runat="server" Font-Size="Smaller" PleaseSelectItem="false">
                                            </SAHL:SAHLDropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" align="right"><SAHL:SAHLButton ID="btnSearchAdvanced" runat="server" Text="Search" OnClick="btnSearch_Click" /></td>
                                    </tr>
                                </table>                   
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td id="cellTypeAdvanced" class="borderLeft borderRight borderTop borderBottom backgroundDark" style="font-style:italic;vertical-align:middle;height:22px;">
                    <a href="#" onclick="changeSearch('ADVANCED')" style="padding:5px;">Advanced Search</a>
                </td>
            </tr>
            <tr>
                <td class="borderRight">&nbsp;</td>
            </tr>
        </table>
        <asp:HiddenField ID="hidSearch" runat="server" Value="BASIC" />
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlGrid" runat="server" Width="99%" Visible="true">
        <SAHL:SAHLGridView ID="gridSearchResults" runat="server" AutoGenerateColumns="true"
            EmptyDataSetMessage="The search returned no Clients." EnableViewState="false"
            FixedHeader="false" GridHeight="340px" GridWidth="100%" HeaderCaption="Client Search Results"
            NullDataSetMessage="" OnGridDoubleClick="SearchGrid_GridDoubleClick" OnRowDataBound="SearchGrid_RowDataBound"
            PostBackType="None" Width="100%" SelectFirstRow="false" 
            SecurityTag="DisableAddToCBO" SecurityHandler="Custom" SecurityDisplayType="Disable"
            >
            <RowStyle CssClass="TableRowA" />
        </SAHL:SAHLGridView>
    </asp:Panel>
    <div class="row" style="padding-top:10px">
        <div class="cell" id="lblTip" runat="server" style="padding:2px;"><strong>Tip:</strong> Click the blue arrow for more information, or the name of the legal entity to select it</div>
        <div class="cellLast" style="text-align:right;">     
            <SAHL:SAHLButton ID="btnNewLegalEntity" runat="server" Text="New Legal Entity" AccessKey="N"
                OnClick="btnNewLegalEntity_Click" ButtonSize="Size6" />&nbsp;
                <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" AccessKey="C" OnClick="btnCancel_Click"
                Visible="False" />&nbsp;
        </div>
    </div>
    <input type="hidden" id="hidAppKey" name="hidAppKey" value="" />
</asp:Content>
