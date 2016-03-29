<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.Common.ConditionsSet" Title="Conditions Edit" Codebehind="ConditionsSet.aspx.cs" EnableEventValidation="false" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
    
    
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <table width="100%">
        <tr>
            <td align="left">
         <asp:Panel ID="Panel1" runat="server" GroupingText="Loan Conditions" Width="100%">
         
             <table width="100%">
            <tr>
                <td align="left" colspan="3">
                    Available loan conditions</td>
            </tr>
            <tr>
                <td align="left" colspan="3">
                    
                <asp:ListBox ID="listGenericConditions" style="height: 150px; overflow: auto"  runat="server" Rows="8" Width="100%"></asp:ListBox></td>
            </tr>
           <tr>
                <td align="center" colspan="3" style="height: 30px">
                    <input id="btnAdd" class="BtnNormal4" type="button" value="Select Condition" onclick="addIt();" />
                    </td>
            </tr>
             <tr>
                <td align="left" colspan="3">
                    Selected loan condition set&nbsp;</td>
            </tr>
                 <tr>
                     <td align="left" colspan="3" style="height: 26px">
                     <select size="8" name="ctl00$Main$listSelectedConditions" id="ctl00_Main_listSelectedConditions" onchange="cleartext(ChosenArrayValue[selectedIndex]);CanEdit(this.options[selectedIndex].text); document.getElementById('ctl00_Main_listGenericConditions').selectedIndex = -1;document.getElementById('btnAdd').disabled = true;" style="width:100%; height: 200px; overflow: auto">
                     </select>
                     </td>
                     
                 </tr>
                 <tr>
                     <td align="center" colspan="3" style="height: 26px">
                         &nbsp;Note: (<span style="color: #ff0033">*Red = required condition : </span>&nbsp;*<span
                             style="background-color: silver">Silver Background</span> =
                         user <span style="text-decoration: underline">
                                 must</span> edit selected condition: <span style="background-color: #ffff99">Yellow
                                     Background</span> = user<span style="text-decoration: underline"> has</span>
                         edited) &nbsp;&nbsp;</td>
                 </tr>
                 <tr>
                     <td align="right" colspan="3" style="text-align: left">
                         Loan Condition Text:<br />
                         <SAHL:SAHLTextBox ID="txtDisplay" runat="server" Rows="5" TextMode="MultiLine"
                        Width="100%" ReadOnly="True" BackColor="#FFFFC0"></SAHL:SAHLTextBox></td>
                 </tr>
                 <tr>
                     <td align="center" colspan="3">
                         &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                         <SAHL:SAHLButton ID="btnAddCondition" runat="server" ButtonSize="Size6" Text="Add Condition" Width="150px" CssClass="BtnNormal6" Visible="False" OnClick="btnAddCondition_Click" CausesValidation="False" />
                         <SAHL:SAHLButton ID="btnEditCondition" runat="server" ButtonSize="Size6" Text="Edit Condition" Width="150px" CssClass="BtnNormal6" Visible="False" OnClick="btnEditCondition_Click" />
                    <input id="btnRemove" class="BtnNormal4" type="button" value="Remove Condition " onclick="delIt();" /><br />
                         <asp:HiddenField ID="txtSelectArrayStrings" runat="server" />
                         <asp:HiddenField ID="txtSelectArrayCSSColor" runat="server" />
                         <asp:HiddenField ID="txtSelectArrayCSSWeight" runat="server"/>
                         <asp:HiddenField ID="txtSelectArrayValue" runat="server"/>
                         <asp:HiddenField ID="txtSelectArrayID" runat="server"/>
                         <asp:HiddenField ID="txtSelectArrayUserEdited" runat="server"/>
                         <asp:HiddenField ID="txtSelectUserConditionType" runat="server"/>
                         <asp:HiddenField ID="txtSelectedIndex" runat="server"/>
                         <asp:HiddenField ID="txtSelectedOfferConditionKeys" runat="server"/>
                         <asp:HiddenField ID="txtChosenArrayStrings" runat="server"/>
                         <asp:HiddenField ID="txtChosenArrayCSSColor" runat="server"/>
                         <asp:HiddenField ID="txtChosenArrayCSSWeight" runat="server"/>
                         <asp:HiddenField ID="txtChosenArrayValue" runat="server"/>
                         <asp:HiddenField ID="txtChosenArrayID" runat="server"/>
                         <asp:HiddenField ID="txtChosenArrayUserEdited" runat="server"/>
                         <asp:HiddenField ID="txtChosenUserConditionType" runat="server"/>
                         &nbsp;&nbsp;
                         <asp:HiddenField ID="txtChosenOfferConditionKeys" runat="server"/><asp:HiddenField ID="txtConditionTableKey" runat="server"/><asp:HiddenField ID="txtChosenOfferConditionSetKeys" runat="server"/>
                         <asp:HiddenField ID="txtSelectedOfferConditionSetKeys" runat="server"/>
                      </td>
                     </tr>
                     <tr >
                     <td align="right" colspan="3" >
                         <br />
                     <SAHL:SAHLButton ID="btnCancel" runat="server" ButtonSize="Size6" Text="Cancel Edit" Width="150px" CssClass="BtnNormal6 " OnClick="btnCancel_Click"/>
                         <SAHL:SAHLButton ID="btnUpdate" runat="server" ButtonSize="Size6" Text="Update Condition Set" Width="150px" CssClass="BtnNormal6 " Visible="False" OnClick="btnUpdate_Click"/>
                      <SAHL:SAHLButton ID="btnSave" runat="server" ButtonSize="Size6" Text="Save Condition Set" Width="150px" CssClass="BtnNormal6 " Visible="False" OnClick="btnSave_Click"/>
                     </td>
                     </tr>
                 
        </table>
             <asp:Label ID="lblErrorMessage" runat="server" Font-Bold="True" ForeColor="Red" Text="You have included conditions within your condition set that contain [Tokens] that you need to edit before saving."
                 Visible="False" Width="100%"></asp:Label></asp:Panel>

         </td>
        </tr>
        <tr>
        <td align="right">
            &nbsp;</td>
        </tr>
        </table>
</asp:Content>