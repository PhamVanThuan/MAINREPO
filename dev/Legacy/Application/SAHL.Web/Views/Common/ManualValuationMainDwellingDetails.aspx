<%@ Page Language="C#" MasterPageFile="~/MasterPages/SAHL.Master" AutoEventWireup="true" CodeBehind="ManualValuationMainDwellingDetails.aspx.cs" Inherits="SAHL.Web.Views.Common.ManualValuationMainDwellingDetails" Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="SAHLWebControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
<script language="javascript" type="text/javascript">
    function calculateMainbuildingReplacementValue()
    {
        var extentvalue;
        var ratevalue;
        extentvalue = document.getElementById('<%=txtMainBuildingExtent.ClientID%>').value ; 
        ratevalue =   document.getElementById('<%=txtMainBuildingRate.ClientID%>').value ;  
        if (isNaN(parseFloat(extentvalue)) || isNaN(parseFloat(ratevalue)))
        {
            document.getElementById('<%=txtMainBuildingReplaceValue.ClientID%>').value = "R 0.00";
        }
        else
        {
            document.getElementById('<%=txtMainBuildingReplaceValue.ClientID%>').value  = "R " + (parseFloat(extentvalue)*parseFloat(ratevalue)).toString();
        }        
    }
    
     function calculateCottageReplacementValue()
    {
        var extentvalue;
        var ratevalue;
        extentvalue = document.getElementById('<%=txtCottageExtent.ClientID%>').value ; 
        ratevalue =   document.getElementById('<%=txtCottageRate.ClientID%>').value ;  
        if (isNaN(parseFloat(extentvalue)) || isNaN(parseFloat(ratevalue)))
        {
            document.getElementById('<%=txtCottageReplaceValue.ClientID%>').value = "R 0.00";
        }
        else
        {
            document.getElementById('<%=txtCottageReplaceValue.ClientID%>').value  = "R " + (parseFloat(extentvalue)*parseFloat(ratevalue)).toString();
        }               
    }
    
      function calculateOutbuildingReplacementValue()
      {
        var extentvalue;
        var ratevalue;
        extentvalue = document.getElementById('<%=txtCombinedOutbuildingsExtent.ClientID%>').value ; 
        ratevalue =   document.getElementById('<%=txtCombinedOutbuildingsRate.ClientID%>').value ;  
        if (isNaN(parseFloat(extentvalue)) || isNaN(parseFloat(ratevalue)))
        {
            document.getElementById('<%=txtCombinedOutbuildingsReplaceValue.ClientID%>').value = "R 0.00";
        }
        else
        {
            document.getElementById('<%=txtCombinedOutbuildingsReplaceValue.ClientID%>').value  = "R " + (parseFloat(extentvalue)*parseFloat(ratevalue)).toString();
        }               
     }
</script>  
<table width="100%" class="tableStandard">
<tr><td align="left">
  
    <SAHL:SAHLGridView ID="PropertyGrid" runat="server" AutoGenerateColumns="false" FixedHeader="false" 
        EnableViewState="false" GridHeight="90px" GridWidth="100%"  Width="100%"
        HeaderCaption="Property"
        PostBackType="SingleClick" 
        NullDataSetMessage="" OnRowDataBound="gridProperty_RowDataBound"
        EmptyDataSetMessage="There are no Properties.">
        <RowStyle CssClass="TableRowA" />
    </SAHL:SAHLGridView>
    
    <br />
    <SAHL:SAHLGridView ID="ImprovementsGrid" runat="server" AutoGenerateColumns="false" FixedHeader="false" 
        EnableViewState="false" GridHeight="120px" GridWidth="100%"  Width="100%"
        HeaderCaption="Outbuildings and Improvements"  
        PostBackType="SingleClick" 
        NullDataSetMessage="" OnSelectedIndexChanged="ImprovementsGrid_SelectedIndexChanged"
        EmptyDataSetMessage="No outbuildings or improvements added">
        <RowStyle CssClass="TableRowA" />
    </SAHL:SAHLGridView>
    <table border="0" id="ExtentTable" runat="server">
        <tr>
            <td valign="top" style="width: 390px;">
                <asp:Panel ID="MainBuildingPanel" runat="server" GroupingText="Main Building" Width="390px">
                    <table border="0">
                        <tr>
                            <td class="TitleText" style="width: 210px">
                                Classification
                            </td>
                            <td style="width: 210px">
                                <SAHL:SAHLLabel ID="lblMainBuildingClassification" runat="server">-</SAHL:SAHLLabel>
                                <SAHL:SAHLDropDownList ID="ddlMainBuildingClassification" runat="server" AutoPostBack="false" PleaseSelectItem="true" Width="185px" >
                                </SAHL:SAHLDropDownList>
                                <SAHL:SAHLRequiredFieldValidator ID="valBuildingClassification" runat="server" ControlToValidate="ddlMainBuildingClassification"
                            ErrorMessage="Please select building type" InitialValue="-select-" />
                            </td>
                         </tr>
                         <tr>
                            <td class="TitleText" style="width: 210px">
                                Roof Type
                            </td>
                            <td style="width: 210px">
                                <SAHL:SAHLLabel ID="lblMainBuildingRoofType" runat="server">-</SAHL:SAHLLabel>
                                <SAHL:SAHLDropDownList ID="ddlMainBuildingRoofType" runat="server" AutoPostBack="false" PleaseSelectItem="true" Width="185px">
                                </SAHL:SAHLDropDownList>
                                 <SAHL:SAHLRequiredFieldValidator ID="valMainBuildingRoofType" runat="server" ControlToValidate="ddlMainBuildingRoofType"
                            ErrorMessage="Please select Roof type" InitialValue="-select-" />
                            </td>
                         </tr>
                        <tr>
                            <td class="TitleText" style="width: 210px">
                                Extent (sq meters)
                            </td>
                            <td style="width: 210px">
                                <SAHL:SAHLLabel ID="lblMainBuildingExtent" runat="server">-</SAHL:SAHLLabel>
                                <SAHL:SAHLTextBox ID="txtMainBuildingExtent" runat="server" DisplayInputType="Number" AutoPostBack="false">
                                </SAHL:SAHLTextBox>
                            </td>
                           
                        </tr>
                        <tr>
                            <td class="TitleText" style="width:210px">
                                Rate (R / sq meter)
                            </td>
                            <td style="width: 210px">
                                <SAHL:SAHLLabel ID="lblMainBuildingRate" runat="server">-</SAHL:SAHLLabel>
                                <SAHL:SAHLTextBox ID="txtMainBuildingRate" runat="server" DisplayInputType="Currency" AutoPostBack="false" >
                                </SAHL:SAHLTextBox>
                            </td>
                           
                        </tr>
                         <tr>
                            <td class="TitleText" style="width: 210px">
                                Replacement Value
                            </td>
                            <td style="width: 210px;">
                                <SAHL:SAHLLabel ID="lblMainBuildingReplacementValue" runat="server">-</SAHL:SAHLLabel>
                                <SAHL:SAHLTextBox ID="txtMainBuildingReplaceValue" runat="server" Enabled="false"></SAHL:SAHLTextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
            <td valign="top" style="width: 360px;">
                <asp:Panel ID="CottagePanel" runat="server" GroupingText="Cottage" Width="360px" >
                    <table border="0">
                        <tr>
                            <td class="TitleText" style="width: 210px" >
                                Roof Type
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="lblCottageRoofType" runat="server">-</SAHL:SAHLLabel>
                                <SAHL:SAHLDropDownList ID="ddlCottageRoofType" runat="server"  AutoPostBack="false" PleaseSelectItem="true" Width="115px">
                                </SAHL:SAHLDropDownList>
                            </td>
                         </tr>
                        <tr>
                            <td class="TitleText"  style="width: 210px;">
                                Extent (sq meters)
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="lblCottageExtent" runat="server">-</SAHL:SAHLLabel>
                                <SAHL:SAHLTextBox ID="txtCottageExtent" runat="server" DisplayInputType="Number" AutoPostBack="false" Width="100px"></SAHL:SAHLTextBox>
                            </td>
                          
                        </tr>
                        <tr>
                            <td class="TitleText"  style="width: 210px">
                                Rate (R / sq meter)
                            </td>
                            <td >
                                <SAHL:SAHLLabel ID="lblCottageRate" runat="server">-</SAHL:SAHLLabel>
                                <SAHL:SAHLTextBox ID="txtCottageRate" runat="server" DisplayInputType="Currency" AutoPostBack="false" Width="100px"></SAHL:SAHLTextBox>
                            </td>
                        </tr>
                         <tr>
                            <td class="TitleText"  style="width: 210px">
                                Replacement Value
                            </td>
                            <td >
                               <SAHL:SAHLLabel ID="lblCottageReplacementValue" runat="server">-</SAHL:SAHLLabel>
                               <SAHL:SAHLTextBox ID="txtCottageReplaceValue" runat="server" Enabled="false" Width="100px"></SAHL:SAHLTextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>           
            </td>
        </tr>
   </table>
    <table border="0" id="AdditionsTable" runat="server">
        <tr id="BuildingType" style="width: 390px">
            <td class="TitleText" style="width: 210px">Building Type</td>
            <td >
                <SAHL:SAHLDropDownList ID="ddlBuildingType" runat="server" PleaseSelectItem="false" AutoPostBack="true" Width="190px" OnSelectedIndexChanged="ddlBuildingType_SelectedIndexChanged" >
                    <asp:ListItem Value="1">Outbuilding</asp:ListItem>
                    <asp:ListItem Value="2">Improvement</asp:ListItem>
                </SAHL:SAHLDropDownList>
            </td>
        </tr>
        <tr>
            <td valign="top" colspan="3" style="width: 390px">
            <asp:Panel ID="CombinedOutbuildingsPanel" runat="server" GroupingText="Combined Outbuilding" Width="390px">
                    <table border="0" id="TABLE1">
                        <tr id="CombinedOutbuildingRoofType" runat="server">
                            <td style="width:210px;" class="TitleText">
                                Roof Type
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="lblOutBuildingRoofType" runat="server">-</SAHL:SAHLLabel>
                                <SAHL:SAHLDropDownList ID="ddlCombinedOutbuildingsRoofType" runat="server" AutoPostBack="false" PleaseSelectItem="true" Width="110px" >
                                </SAHL:SAHLDropDownList>
                            </td>
                        
                        </tr>
                        <tr>
                            <td class="TitleText" style="width:210px">
                                Extent (sq meters)
                            </td>
                            <td style="width: 210px">
                                <SAHL:SAHLLabel ID="lblCombinedOutbuildingsExtent" runat="server">-</SAHL:SAHLLabel>
                                <SAHL:SAHLTextBox ID="txtCombinedOutbuildingsExtent" runat="server" DisplayInputType="Number" AutoPostBack="false" >
                                </SAHL:SAHLTextBox>
                            </td>
                         
                        </tr>
                        <tr>
                            <td class="TitleText">
                                Rate (R / sq meter)
                            </td>
                            <td style="width: 210px">
                                <SAHL:SAHLLabel ID="lblCombinedOutbuildingsRate" runat="server">-</SAHL:SAHLLabel>
                                <SAHL:SAHLTextBox ID="txtCombinedOutbuildingsRate" runat="server" DisplayInputType="Currency" AutoPostBack="false">
                                </SAHL:SAHLTextBox>
                            </td>
                        </tr>
                         <tr>
                            <td class="TitleText">
                                Replacement Value
                            </td>
                            <td style="width: 210px">
                                <SAHL:SAHLLabel ID="lblCombinedOutbuildingsReplaceValue" runat="server">-</SAHL:SAHLLabel>
                                <SAHL:SAHLTextBox ID="txtCombinedOutbuildingsReplaceValue" runat="server" Enabled="false" >-</SAHL:SAHLTextBox>
                            </td>
                          
                        </tr>
                    </table>
                </asp:Panel>   
            </td>
            <td>
                <asp:Panel ID="CombinedImprovementsPanel" runat="server" GroupingText="Combined Improvements"  Width="360px">
                    <table border="0">
                        <tr id="CombinedImprovementType" runat="server">
                            <td style="width:150px; height: 24px;" class="TitleText">
                                Improvement Type
                            </td>
                            <td style="width:150px; height: 24px;">
                                <SAHL:SAHLDropDownList ID="ddlCombinedImprovementType" runat="server" AutoPostBack="false" PleaseSelectItem="true" Width="120px"  >
                                </SAHL:SAHLDropDownList>
                            </td>
                        </tr>
                         <tr id="TrImprovementExt" runat="server">
                            <td style="width:150px" class="TitleText" >
                                Extent (sq meters)
                            </td>
                            <td style="width:150px">
                                  <SAHL:SAHLLabel ID="lblCombineImprovementExtent" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                         <tr id="TrImprovementRate" runat="server">
                            <td style="width:150px" class="TitleText">
                                Rate
                            </td>
                            <td style="width:150px">
                                  <SAHL:SAHLLabel ID="lblConbinedImprovementRate" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr id="TrImprovementDate" runat="server">
                            <td style="width:150px" class="TitleText">
                                Date 
                            </td>
                            <td style="width:150px">
                                  <SAHL:SAHLLabel ID="lblCombinedImprovementDate" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                         <tr>
                            <td class="TitleText" style="width:150px" >
                                Replacement Value
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="lblCombinedImprovementsReplaceValue" runat="server">-</SAHL:SAHLLabel>
                                <SAHL:SAHLTextBox ID="txtCombinedImprovementsReplaceValue" runat="server" DisplayInputType="Currency" AutoPostBack="false"></SAHL:SAHLTextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>   
            </td>
     </tr>       
    </table>
   <table>
     <tr>
        <td valign="top" style="width: 390px">
            <asp:Panel ID="CombinedThatchPanel" runat="server" GroupingText="Combined Thatch" Width="390px">
                    <table border="0" style="width:390px">
                        <tr id="CombinedThatchValueRow">
                            <td class="TitleText" style="width: 210px">
                                Value
                            </td>
                            <td style="width: 220px">
                                <SAHL:SAHLLabel ID="lblCombinedThatchValue" runat="server">-</SAHL:SAHLLabel>
                                <SAHL:SAHLTextBox ID="txtCombinedThatchValue" runat="server" DisplayInputType="Currency" AutoPostBack="false">
                                </SAHL:SAHLTextBox>
                            </td>
                       </tr>
                       <tr id="CombinedThatchCheckRow" runat="server">
                            <td class="TitleText" style="width: 210px">
                               Thatch Value % Check
                            </td>
                            <td style="width: 200px">
                                <SAHL:SAHLLabel ID="lblCombinedThatchValuePercCheck" runat="server">-</SAHL:SAHLLabel>
                            </td>
                       </tr>
                       <tr id="CombinedThatchExtentRow" runat="server">
                            <td class="TitleText" style="width: 210px">
                               Thatch Extent % Check
                            </td>
                            <td style="width: 200px">
                                <SAHL:SAHLLabel ID="lblCombinedThatchPercExtCheck" runat="server">-</SAHL:SAHLLabel>
                            </td>
                       </tr> 
                    </table>
                </asp:Panel>   
            </td>
       <td valign="top" style="width: 360px">
        <table border="0" id="tblValuation" runat="server" style="width: 353px">
        <tr id="ValuationEscalation">
            <td class="TitleText" style="width: 220px;">
                Valuation Escalation %
            </td>
            <td>
                <SAHL:SAHLLabel ID="lblEscalation" runat="server">-</SAHL:SAHLLabel>
            </td>
            <td>
                <SAHL:SAHLTextBox ID="txtEscalation" runat="server" DisplayInputType="Currency" AutoPostBack="false" Width="110px">20</SAHL:SAHLTextBox>
            </td>
        </tr>
        <tr id="ValuationConventionalValue">
            <td class="TitleText">
                Total Conventional Value
            </td>
            <td >
                <SAHL:SAHLLabel ID="lblTotalConventionalValue" runat="server">-</SAHL:SAHLLabel>
            </td>
        </tr>
        <tr id="ValuationThatchValue">
            <td class="TitleText">
                Total Thatch Value
            </td>
            <td>
                <SAHL:SAHLLabel ID="lblTotalThatchValue" runat="server">-</SAHL:SAHLLabel>
            </td>
          
        </tr>
        <tr id="ValuationTotalValue">
            <td class="TitleText">
                Total Value
            </td>
            <td >
                <SAHL:SAHLLabel ID="lblTotalValue" runat="server">-</SAHL:SAHLLabel>
            </td>
       </tr>
    </table>
    </td>
   </tr>
   <tr>
   <td style="width: 384px">
     <asp:Panel ID="pnlImprovement" runat="server" GroupingText="Improvements"  Width="360px">
         <table border="0">
                        <tr id="trImprovement" runat="server">
                            <td style="width:210px" class="TitleText">
                                Improvement Type
                            </td>
                              <td >
                                 <SAHL:SAHLLabel ID="lblImprovementType" runat="server">-</SAHL:SAHLLabel>
                                  <SAHL:SAHLDropDownList ID="ddlImprovementType" runat="server" AutoPostBack="false" PleaseSelectItem="true" Width="110px" >
                                </SAHL:SAHLDropDownList>
                             </td>
                        </tr>
                        <tr id="trImprovementDateRow" runat="server">
                            <td style="width:210px" class="TitleText">
                               Improvement Date
                            </td>
                              <td >
                                 <SAHL:SAHLLabel ID="lblImprovementDate" runat="server">-</SAHL:SAHLLabel>
                                  <SAHL:SAHLDateBox ID="dteImprovementDate" runat="server" AutoPostBack="false" Width="110px" >
                                </SAHL:SAHLDateBox>
                             </td>
                        </tr>
                        <tr>
                            <td class="TitleText" >
                                Replacement Value
                            </td>
                            <td>
                               <SAHL:SAHLTextBox ID="txtImprovementReplacementValue" runat="server" DisplayInputType="Currency" AutoPostBack="false"></SAHL:SAHLTextBox>
                            </td>
                        </tr>
           </table>
     </asp:Panel>   
    </td></tr>
   </table>
</td></tr>
<tr id="ButtonRow" runat="server"><td align="right" style="width: 588px">
    <SAHL:SAHLButton ID="CancelButton" runat="server" Text="Cancel" AccessKey="C" CausesValidation="False" OnClick="Cancel_ButtonClick" />
    <SAHL:SAHLButton ID="BackButton" runat="server" Text="Back" AccessKey="B" CausesValidation="False" OnClick="BackButton_Click"/>
    <SAHL:SAHLButton ID="AddNewButton" runat="server" Text="Add New Entry" AccessKey="U" CausesValidation="true" ButtonSize="Size5" OnClick="AddNew_ButtonClick" />
    <SAHL:SAHLButton ID="AddButton" runat="server" Text="Add" AccessKey="A" CausesValidation="true" OnClick="Add_ButtonClicked" />
    <SAHL:SAHLButton ID="SubmitButton" runat="server" Text="Next" AccessKey="N" OnClick="Next_ButtonClick" />
    <SAHL:SAHLTextBox ID="txtHiddenBox" runat="server" style="display:none;" Width="30px"></SAHL:SAHLTextBox>
</td></tr></table>
</asp:Content>
