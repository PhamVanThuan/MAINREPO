<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.Common.LegalAttorney" Title="LegalAttorney" Codebehind="LegalAttorney.aspx.cs" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <div class="row"> 
        <SAHL:SAHLLabel ID="lblheader" runat="server" Font-Bold="true" Text="-"></SAHL:SAHLLabel>
    </div>
   <div class="row"> 
        <asp:Panel ID="pnlLegalAmin" runat="server" Height="60px" Width="430px" GroupingText="Select Registrations Administrator">
         <br />
            <div class="cellInput TitleText">User Name&nbsp;</div>
            <div class="cellInput" style="width:290px;">
            <SAHL:SAHLDropDownList ID="ddlRegistrationAdmin" runat="server" Width="290px"></SAHL:SAHLDropDownList>
            </div>
            <div>
            <SAHL:SAHLRequiredFieldValidator ID="valRegistrationAdmin" runat="server" ControlToValidate="ddlRegistrationAdmin" ErrorMessage="Please select a Registration User" InitialValue="-select-" />
            </div>
        </asp:Panel>
    </div>
    <div class="row"> 
       <div>
            <asp:Panel ID="pnlLegalAttorney" runat="server" Height="60px" Width="400px" GroupingText="Select Attorney">
            <br />
             <div class="cellInput TitleText" style="width:90px;">Deeds Office</div>
            <div class="cellInput" style="width:290px;">
                     <SAHL:SAHLDropDownList ID="ddlDeedsOffice" runat="server" Width="290px" OnSelectedIndexChanged="ddlDeedsOffice_SelectedIndexChanged" AutoPostBack="true"></SAHL:SAHLDropDownList>
                </div>
                <div class="cellInput TitleText" style="width:90px;">Attorney</div>
                <div class="cellInput" style="width:290px;">
                     <SAHL:SAHLDropDownList ID="ddlRegistrationAttorney" runat="server" Width="290px"></SAHL:SAHLDropDownList>
                </div>
            </asp:Panel>
       </div>
   </div>
    <div class="row">
        <br />
    </div>
    <div class="row">
        <div>
             <asp:Panel ID="pnlPreferredAttorney" runat="server" Height="10px" Width="360px" GroupingText="Preferred Attorney (if any)">
             <br />
                  <div class="cellInput TitleText" style="width:140px;">Preferred Attorney</div>
                  <div class="cellInput" style="width:190px;">
                     <SAHL:SAHLTextBox ID="txtPreferredAttorney" runat="server" Width="190px" Enabled="False"></SAHL:SAHLTextBox>
                  </div>   
            </asp:Panel>
        </div>      
    </div>
     <div class="row">
        <br />
        <br />
        <br />
        <br />
    </div>
    <div class="buttonBar" style="width:50%">
        <SAHL:SAHLButton ID="btnUpdateButton" runat="server" AccessKey="U" OnClick="btnUpdateButton_Click" Text="Update"/>
    </div>
</asp:Content>