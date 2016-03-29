<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" Inherits="SAHL.Web.Views.Common.ClientDetails" Title="Untitled Page" Codebehind="ClientDetails.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">

<table class="tableStandard" width="100%">

    <tr><td align="left" style="height:99%;" valign="top">

    <table id="LECompanyTable" runat="server" border="0" class="tableStandard" width="100%">
        <tr>
            <td style="width:160px;" class="TitleText">
                Introduction Date
            </td>
            <td style="width:300px;">
                <SAHL:SAHLLabel ID="lblCLEIntroductionDate" runat="server">-</SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td class="TitleText">
                Company Name</td>  
            <td>
                <SAHL:SAHLLabel ID="lblCLECompanyName" runat="server">-</SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td class="TitleText">
                Trading Name</td>
            <td>
                <SAHL:SAHLLabel ID="lblCLETradingName" runat="server">-</SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td class="TitleText">
                Registration Number
            </td>
            <td>
                <SAHL:SAHLLabel ID="lblCLERegistrationNumber" runat="server">-</SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td class="TitleText">
                Legal Entity Status
            </td>
            <td >
                <SAHL:SAHLLabel ID="lblCLEStatus" runat="server">-</SAHL:SAHLLabel>
            </td>
        </tr>
    </table>
    
    <table id="LENaturalPersonTable" runat="server" border="0" class="tableStandard" width="100%">
        <tr>
            <td style="width:160px;" class="TitleText">
                Introduction Date
            </td>
            <td style="width:300px;">
                <SAHL:SAHLLabel ID="lblNLEIntroductionDate" runat="server">-</SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td class="TitleText">
                Legal Entity Name
            </td>
            <td>
                <SAHL:SAHLLabel ID="lblNLEName" runat="server">-</SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td class="TitleText">
                Preferred Name
            </td>
            <td>
                <SAHL:SAHLLabel ID="lblNLEPreferredName" runat="server">-</SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td class="TitleText">
                ID Number
            </td>
            <td>
                <SAHL:SAHLLabel ID="lblNLEIDNumber" runat="server">-</SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td class="TitleText">
                Passport Number
            </td>
            <td>
                <SAHL:SAHLLabel ID="lblNLEPassPortNumber" runat="server">-</SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td class="TitleText">
                Date of Birth
            </td>
            <td>
                <SAHL:SAHLLabel ID="lblNLEDateOfBirth" runat="server">-</SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td class="TitleText">
                Gender
            </td>
            <td>
                <SAHL:SAHLLabel ID="lblNLEGender" runat="server">-</SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td class="TitleText" >
                Marital Status
            </td>
            <td>
                <SAHL:SAHLLabel ID="lblNLEMaritalStatus" runat="server">-</SAHL:SAHLLabel>
            </td>
        </tr>
        <tr>
            <td class="TitleText">
                Legal Entity Status
            </td>
            <td>
                <SAHL:SAHLLabel ID="lblNLEStatus" runat="server">-</SAHL:SAHLLabel></td>
        </tr>
    </table>
    
    <br />

    <table border="0" class="tableStandard" width="100%">
        <tr>
            <td valign="top">
            
                <asp:Panel ID="pnlContactDetailsPerson" runat="server" GroupingText="Legal Entity Contact Details" Height="110px">
                    <table border="0" width="100%" class="tableStandard">
                        <tr>
                            <td style="width:160px;" class="TitleText">
                                Home Phone
                             </td>
                            <td style="width:200px;">
                                <SAHL:SAHLLabel ID="lblNLEHomePhone" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr>
                            <td class="TitleText">
                                Work Phone
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="lblNLEWorkPhone" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr>
                            <td class="TitleText">
                                Fax Number
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="lblNLEFaxNumber" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr>
                            <td class="TitleText">
                                Cellphone
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="lblNLECellphone" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr>
                            <td class="TitleText">
                                Email Address
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="lblNLEEmailAddress" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                
                <asp:Panel ID="pnlContactDetailsCompany" runat="server" GroupingText="Legal Entity Contact Details" Height="110px">
                    <table border="0" width="100%" class="tableStandard">
                        <tr>
                            <td style="width:160px;" class="TitleText">
                                Work Phone
                            </td>
                            <td style="width:200px;">
                                <SAHL:SAHLLabel ID="lblCLEWorkPhone" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr>
                            <td class="TitleText">
                                Fax Number
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="lblCLEFaxNumber" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr>
                            <td class="TitleText">
                                Cellphone
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="lblCLECellPhone" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr>
                            <td class="TitleText">
                                Email Address
                            </td>
                            <td>
                                <SAHL:SAHLLabel ID="lblCLEEmailAddress" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                
            </td>
            <td valign="top">
            
                <asp:Panel ID="pnlLEAddress" runat="server" GroupingText="Legal Entity Address - Street" Height="110px">
                    <table border="0" width="100%" class="tableStandard">
                        <tr>
                            <td style="width:360px;">
                                <SAHL:SAHLLabel ID="lblAddressLine1" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <SAHL:SAHLLabel ID="lblAddressLine2" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <SAHL:SAHLLabel ID="lblAddressLine3" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <SAHL:SAHLLabel ID="lblAddressLine4" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <SAHL:SAHLLabel ID="lblAddressLine5" runat="server">-</SAHL:SAHLLabel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                
            </td>
        </tr>
    </table>
    
</td></tr></table>
</asp:Content>
