<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true" CodeBehind="ApplicationWizardApplicant.aspx.cs" Inherits="SAHL.Web.Views.Origination.ApplicationWizardApplicant" Title="Untitled Page" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
<script language="javascript" type="text/javascript">
    function IDNumberChanged()
    {
        var tbFirstNames = document.getElementById('<%=txtFirstNames.ClientID%>');
        var lblFirstNames = document.getElementById('<%=lblFirstNames.ClientID%>');
        var tbSurname = document.getElementById('<%=txtSurname.ClientID%>');
        var lblSurname = document.getElementById('<%=lblSurname.ClientID%>');
//        var phContact = document.getElementById('<%=phContact.ClientID%>');
//        var lblContact = document.getElementById('<%=lblContact.ClientID%>');
        
        var tbIDNumber = document.getElementById('<%=txtIDNumber.ClientID%>');                            
        
        tbFirstNames.style.display = 'inline';
        tbSurname.style.display = 'inline';
//        phContact.style.display = 'inline';
            
        if (lblFirstNames != null)
        {
            lblFirstNames.style.display = 'none';
            lblSurname.style.display = 'none';
//            lblContact.style.display = 'none';
        }
    }
    </script>
    <asp:Panel ID="pnlLegalEntity" runat="server" GroupingText="Applicant" Width="50%">
        <table class="tableStandard">
            <tr>
                <td colspan="4" style="height: 5px">
                </td>
            </tr>
            <tr>
                <td align="right">
                    Marketing Source:
                </td>
                <td>
                    <sahl:sahldropdownlist id="ddlMarketingSource" runat="server" cssclass="mandatory" TabIndex="1"
                        ></sahl:sahldropdownlist>
                </td>
                <td style="width: 10px">
                    &nbsp;</td>
                <td>
                    <img align="middle" alt="" src="../../Images/help.gif" title="Where did this person get to hear about us." /></td>
            </tr>
            <tr>
                <td align="right">
                    Identity Number:
                </td>
                <td>
                    <sahl:sahltextbox id="txtIDNumber" runat="server" maxlength="20" onchange="IDNumberChanged()"
                        width="182px" TabIndex="2"></sahl:sahltextbox>
                    <sahl:sahlautocomplete id="acNatAddIDNumber" runat="server" autopostback="true" mincharacters="6"
                        onitemselected="acNatAddIDNumber_ItemSelected" servicemethod="SAHL.Web.AJAX.LegalEntity.GetLegalEntitiesByIDNumber"
                        targetcontrolid="txtIDNumber" width="282px" TabIndex="2"></sahl:sahlautocomplete>
                </td>
                <td style="width: 10px">
                    &nbsp;</td>
                <td>
                    <img align="middle" alt="" src="../../Images/help.gif" title="This persons ID number." /></td>
            </tr>
            <tr>
                <td align="right">
                    First Names:
                </td>
                <td>
                    <sahl:sahltextbox id="txtFirstNames" runat="server" cssclass="mandatory" maxlength="50"
                       tabindex="3" width="182px"></sahl:sahltextbox>
                    <sahl:sahllabel id="lblFirstNames" runat="server" visible="false"></sahl:sahllabel>
                </td>
                <td style="width: 10px">
                    &nbsp;</td>
                <td>
                    <img align="middle" alt="" src="../../Images/help.gif" title="This persons first names." /></td>
            </tr>
            <tr>
                <td align="right">
                    Surname:
                </td>
                <td>
                    <sahl:sahltextbox id="txtSurname" runat="server" cssclass="mandatory" maxlength="50"
                       tabindex="4" width="182px"></sahl:sahltextbox>
                    <sahl:sahllabel id="lblSurname" runat="server" visible="false"></sahl:sahllabel>
                </td>
                <td style="width: 10px">
                    &nbsp;</td>
                <td>
                    <img align="middle" alt="" src="../../Images/help.gif" title="This persons last name." /></td>
            </tr>
            <tr>
                <td align="right">
                    Contact Number:
                </td>
                <td>
                    <sahl:sahlphone id="phContact" runat="server" cssclass="mandatory" TabIndex="5"></sahl:sahlphone>
                    <sahl:sahllabel id="lblContact" runat="server" visible="false"></sahl:sahllabel>
                </td>
                <td style="width: 10px">
                    &nbsp;</td>
                <td>
                    <img align="middle" alt="" src="../../Images/help.gif" title="The primary contact telephone number for this client." /></td>
            </tr>
            <tr>
                <td align="right">
                    Number of Applicants:
                </td>
                <td>
                    <sahl:sahltextbox id="tbNumApplicants" runat="server" columns="3" displayinputtype="Number"
                        mandatory="true" TabIndex="6"></sahl:sahltextbox>
                </td>
                <td style="width: 10px">
                    &nbsp;</td>
                <td>
                    <img align="middle" alt="" src="../../Images/help.gif" title="The number of clients estimated for this loan application." /></td>
            </tr>
            <tr runat="server" id="rowEstateAgentDeal">
                <td align="right">Estate Agent Deal:</td>
                <td><asp:CheckBox ID="chkEstateAgent" runat="server" /></td>
                <td style="width:10px;">&nbsp;</td>
                <td><img alt="" src="../../Images/help.gif" title="Ticking this will require an Estate Agent to be captured further in the process." /></td>
            </tr>
            <tr runat="server" id="Tr1">
                <td align="right">Old Mutual Developer Loan:</td>
                <td><asp:CheckBox ID="chkDevelopmentLoan" runat="server" /></td>
                <td style="width:10px;">&nbsp;</td>
                <td><img alt="" src="../../Images/help.gif" title="Ticking this will mark the loan as an Old Mutual Developer Loan" /></td>
            </tr>
            <tr>
                <td>
                    <SAHL:SAHLTextBox runat="server" ID="tbLegalEntityKey" style="display:none" />
                </td>
            </tr>
            
        </table>        
    </asp:Panel>
    <br />
    <table width="50%">
            <tr>
                <td align="left" style="width: 40%; height: 21px">
                    <SAHL:SAHLButton ID="btnBack" runat="server" ButtonSize="Size4" CausesValidation="false"
                        CssClass="BtnNormal4" OnClick="OnCalculate_Click" Text="Back" Visible="False" /></td>
                <td style="width: 60%; height: 21px" align="right">
                    <SAHL:SAHLButton ID="btnCancel" runat="server" ButtonSize="Size4" CausesValidation="false"
                        CssClass="BtnNormal4" OnClick="OnCancel_Click" Text="Cancel" TabIndex="7" />
                    <SAHL:SAHLButton ID="btnNext" runat="server" ButtonSize="Size4" CausesValidation="false"
                        CssClass="BtnNormal4" OnClick="OnNext_Click" Text="Next" TabIndex="6" /></td>
            </tr>
        </table>
</asp:Content>
