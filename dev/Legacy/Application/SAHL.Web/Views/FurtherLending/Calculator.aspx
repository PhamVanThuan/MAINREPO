<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true" CodeBehind="Calculator.aspx.cs" Inherits="SAHL.Web.Views.FurtherLending.Calculator" Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="SAHLWebControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <link href="../../CSS/SAHL.css" rel="stylesheet" />
    <script src="../../Scripts/json2.js"></script>
    <script src="../../Scripts/jquery-1.5.min.js"></script>
    <script language="javascript" type="text/javascript">
        function onPreviewClicked() {
            var rptFrame = $('#reportFrame');
            var reportUrl = document.getElementById('<%=reportPreviewURL.ClientID%>').value;
        rptFrame.attr("src", reportUrl)
        return false;
    };

    function tabsMenu_ClientActiveTabChanged(sender, e) {
        __doPostBack('<%=tabsMenu.ClientID%>', sender.get_activeTabIndex());
    }



    function SetNextBasedOnCorrespondenceSelection() {
        // onKeyUp for the send to contact inputs check to see if the data is valid and we can continue (enable btnNext)
        // CheckBox for correspondence selection should only be enabled if legal entity contact info is valid

        // Pass selection and enable Next if at least one correspondence with any of the contacts is selected
        if (($('[id$="pnlLegalEntities"] [type="checkbox"]:checkbox:checked').length > 0) || ((document.getElementById('<%=tbAlternateEmail.ClientID%>').value.length > 0) && (document.getElementById('lblEmailError').style.display == "none")) || (document.getElementById('<%=tbAlternateFaxNumber.ClientID%>').value.length > 0)) {
            // Pass the details of the selected (SendToOnClick)
            $('[id$="btnNext"]').removeAttr('disabled');
        }
        else {
            $('[id$="btnNext"]').attr("disabled", "disabled");
        }

    }

    function enableCalculate() {
        SAHLButton_setEnabled('<%=btnCalculate.ClientID%>', true);
        var btnSubmit = document.getElementById('<%=btnSubmit.ClientID%>');
        if (btnSubmit != null)
            SAHLButton_setEnabled('<%=btnSubmit.ClientID%>', false);
    }

    function disableNext() {
        var tbApprovalMode = document.getElementById('<%=tbApprovalMode.ClientID%>');

        enableCalculate();

        if (tbApprovalMode.value == "None" || tbApprovalMode.value == "") //not a credit approval
        {
            var btnNext = document.getElementById('<%=btnNext.ClientID%>');
            if (btnNext != null) // added by CF because this button can by hidden via security tag
            {
                var tabsMenu = $get('<%=tabsMenu.ClientID%>').control;

                // We are on the Calculations tab if we are in this method
                // If the varifix tab is not enabled, disable the next button
                if (tabsMenu.get_tabs()[1].get_enabled() != true) {
                    SAHLButton_setEnabled('<%=btnNext.ClientID%>', false);
                    SAHLButton_setEnabled('<%=btnPreview.ClientID%>', false);
                }

                tabsMenu.get_tabs()[2].set_enabled(false);
                tabsMenu.get_tabs()[3].set_enabled(false);
                tabsMenu.get_tabs()[4].set_enabled(false);
            }
        }
    }

    function copy() {
        var tbTotal = document.getElementById('<%=tbTotalCashRequired.ClientID%>');
        var lTotal = document.getElementById('<%=lTotalCashRequired.ClientID%>');

        lTotal.innerText = "R " + String(tbTotal.value);
    }

    function loanConditionsOnClick() {
        var chk = document.getElementById('<%=chkCondition.ClientID%>');
        var tbSendApplication = document.getElementById('<%=tbSendApplication.ClientID%>');

        var tpInformation = $find('<%=tpInformation.ClientID%>');
        var tpConfirmation = $find('<%=tpConfirmation.ClientID%>');

        var btn = '<%=btnNext.UniqueID%>';

        if (chk.checked == false) {
            SAHLButton_setEnabled(btn, false);
            tpInformation.set_enabled(false);
            tpConfirmation.set_enabled(false);
        }
        else {
            //debugger;

            SAHLButton_setEnabled(btn, true);
            if (tbSendApplication.value != "False") //need to show send to info
                tpInformation.set_enabled(true);
            else
                tpConfirmation.set_enabled(true);
        }
    }

    //do not uncomment this function
    function enableUpdateButton(enabled) {
        // Leave this function in, it is called by the common QC control
    }

    function submitMultipleRecipients(legalEntityKey, correspondenceSelection, contactInfoToUse, check) {
        var action = (check.checked) ? "add" : "remove"
        PersistLegalEntityCorrespondenceSelection(legalEntityKey, correspondenceSelection, contactInfoToUse, action);
        CheckBoxSelected();
        return false;
    }

    function CheckBoxSelected() {
        if ($('[id$="pnlLegalEntities"] [type="checkbox"]:checkbox:checked').length > 0) {
            $('[id$="btnNext"]').removeAttr('disabled');
        }
        else {
            $('[id$="btnNext"]').attr("disabled", "disabled");
        }
    }

    function ValidateEmail(email) {
        //RFC 2822
        var emailCheck = /^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$/i;
        return emailCheck.test(email);
    }

    function ValidateAlternateOption(correspondenceMediumKey, contactInfo) {
        //Email = 2
        if (correspondenceMediumKey == "2") {
            var tbAlternateEmail = document.getElementById('<%=tbAlternateEmail.ClientID%>');
            var lblEmailError = document.getElementById('lblEmailError');
            if (ValidateEmail(tbAlternateEmail.value)) {
                lblEmailError.style.display = "none";
                return true;
            }
            else {
                if (contactInfo != '') {
                    lblEmailError.style.display = "";
                } else {
                    lblEmailError.style.display = "none";
                }
                return false;
            }

        } else {
            return true;
        }
    }

    function PersistAlternateLegalEntityCorrespondenceSelection(correspondenceMediumKey, contactInfo) {
        if ((ValidateAlternateOption(correspondenceMediumKey, contactInfo)) || (contactInfo == '')) {
            var action = '';
            if (contactInfo == '') {
                action = 'remove';
            } else {
                action = 'add';
            }
            tblSelectedlegalEntityKey = document.getElementById('<%=tbSelectedLegalEntityKey.ClientID%>');
            var isAlternateContactInfo = true;
            PersistLegalEntityCorrespondenceSelection(tblSelectedlegalEntityKey.value, correspondenceMediumKey, contactInfo, action, isAlternateContactInfo);
        }
        SetNextBasedOnCorrespondenceSelection();
    }

    function PersistLegalEntityCorrespondenceSelection(legalEntityKey, correspondenceMediumKey, contactInfo, action, isAlternateContactInfo) {
        var multiRecipientData = GetRecipientDataFromHiddenField();
        RemoveStaleCorrespondenceData(multiRecipientData, legalEntityKey, correspondenceMediumKey, isAlternateContactInfo)
        if (action === 'add') {
            var recipientData = new Object();
            recipientData.LegalEntityKey = legalEntityKey;
            recipientData.CorrespondenceMediumKey = correspondenceMediumKey;
            recipientData.ContactInfo = contactInfo;
            recipientData.IsUsingAlternateContactInfo = isAlternateContactInfo;
            multiRecipientData.push(recipientData);
        }
        SetRecipientData(multiRecipientData);
    }

    function RemoveStaleCorrespondenceData(collection, legalEntityKey, correspondenceMediumKey, isAlternateContactInfo) {
        var found = false;
        var index = -1;
        for (var i = 0; i < collection.length; i++) {
            if ((collection[i].LegalEntityKey === legalEntityKey) && (collection[i].CorrespondenceMediumKey == correspondenceMediumKey) && (collection[i].IsUsingAlternateContactInfo == isAlternateContactInfo)) {
                index = i;
                found = true;
                break;
            }
        }
        if (index > -1) {
            collection.splice(index, 1);
        }
    }

    function GetRecipientDataFromHiddenField() {
        var persitedData = [];
        if ($('[id$="tbMultiRecipientCorrespondenceData"]').length > 0 && $('[id$="tbMultiRecipientCorrespondenceData"]').val().length > 0) {
            var storedObj = $('[id$="tbMultiRecipientCorrespondenceData"]').val();
            persitedData = $.parseJSON(storedObj);
        }
        return persitedData;
    }

    function SetRecipientData(recipientData) {
        var jsonData = JSON.stringify(recipientData);
        if (jsonData == "[]") {
            jsonData = "";
        }
        $('[id$="tbMultiRecipientCorrespondenceData"]').val(jsonData);
    }

    </script>
    <input type="hidden" id="reportPreviewURL" runat="server" />

    <div id="Panel1" style="background-color:#acacac;border-width:1px;border-style:solid;border-color:Black;padding:10px 10px 20px 10px !important;width:250px;position:absolute;z-index:1000;width:80%;height:90%;display:none;">
         <iframe id="reportFrame" style="width: 100%; height: 100%;"></iframe>
        <div style="float:right">
            <input id="btnClose" value="Close" type="button" />
        </div>        
    </div>

    <ajaxToolkit:ModalPopupExtender runat="server" ID="ModalPopupExtender1"
        CancelControlID="btnClose" TargetControlID="btnPreview"
        PopupControlID="Panel1" Drag="true"
        PopupDragHandleControlID="PopupHeader">
    </ajaxToolkit:ModalPopupExtender>
    
    <ajaxToolkit:TabContainer runat="server" ID="tabsMenu" OnClientActiveTabChanged="tabsMenu_ClientActiveTabChanged" ActiveTabIndex="2">
        <ajaxToolkit:TabPanel runat="server" ID="tpCalculator" HeaderText="Calculations">
            <ContentTemplate>

               <table class="tableStandard" style="font-size: 0.8em;" runat="server" id="CalculationTable">
                    <tr id="Tr1" class="rowSmall" runat="server">
                        <td id="Td5" class="TitleText" runat="server" width="25%" style="width:25%">Employment Type</td>
                        <td id="Td6" style="width: 2px" runat="server"></td>
                        <td id="Td7" align="right" style="border-style:solid; border-color:Gray;border-left:1px;border-right:1px;border-top:1px;width:20%" class="LabelText" runat="server" width="20%">
                            <SAHL:SAHLLabel ID="lEmploymentAccount" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                        <td id="Td8" style="width: 2px" runat="server"></td>
                        <td id="Td9" align="right" style="border-style:solid; border-color:Gray;border-left:1px;border-right:1px;border-top:1px;max-width:25%" class="LabelText" runat="server" width="25%">
                            <SAHL:SAHLDropDownList ID="ddlEmploymentType" runat="server" CssClass="RequiredInput" Width="99.5%" onchange="disableNext()"></SAHL:SAHLDropDownList>
                            <SAHL:SAHLLabel ID="lEmploymentApplication" runat="server" CssClass="LabelText" Visible="False" >-</SAHL:SAHLLabel>
                        </td>
                        <td></td>
                        <td style="border-style:solid; border-color:Gray;border-left:1px;border-top:1px;width:15%" width="15%" ></td>
                        <td style="border-style:solid; border-color:Gray;border-right:1px;border-top:1px;width:15%" width="15%" ></td>
                        <td width="auto"></td>
                    </tr>
                    <tr id="Tr2" class="rowSmall" runat="server">
                        <td id="Td13" class="TitleText" runat="server" width="25%">Household Income</td>
                        <td id="Td14" style="width: 2px" runat="server"></td>
                        <td id="Td15" align="right" style="border-style:solid; border-color:Gray;border-left:1px;border-right:1px;width:20%" class="LabelText" runat="server" width="20%">
                            <SAHL:SAHLLabel ID="lblHouseholdIncome" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                        <td id="Td16" style="width: 2px" runat="server"></td>
                        <td id="Td17" align="right" style="border-style:solid; border-color:Gray;border-left:1px;border-right:1px;" class="LabelText" runat="server" width="25%">
                            <SAHL:SAHLTextBox ID="tbNewIncome1" runat="server" DisplayInputType="Currency" Width="98.5%" onkeyup="disableNext()"></SAHL:SAHLTextBox>
                            <SAHL:SAHLLabel ID="lblNewHouseholdIncome" runat="server" CssClass="LabelText" Visible="False" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                        <td></td>
                        <td style="border-style:solid; border-color:Gray;border-left:1px;width:15%" width="15%"></td>
                        <td style="border-style:solid; border-color:Gray;border-right:1px;width:15%" width="15%"></td>
                    </tr>
                    <tr id="Tr3" class="rowSmall" runat="server">
                        <td id="Td21" class="TitleText" runat="server">Latest Valuation</td>
                        <td id="Td22" style="width: 2px" runat="server"></td>
                        <td id="Td23" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lblValuationAmount" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%"></SAHL:SAHLLabel>
                        </td>
                        <td id="Td24" style="width: 2px" runat="server"></td>
                        <td id="Td25" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLTextBox ID="tbEstimatedValuationAmount" runat="server" Width="98.5%" DisplayInputType="Currency" onkeyup="disableNext()"></SAHL:SAHLTextBox>
                        </td>
                        <td id="Td26" style="width: 2px" runat="server"></td>
                        <td id="Td31" class="TitleText" style="border-style: solid; border-color: Gray; border-left: 1px;" runat="server">Latest Valuation</td>
                        <td id="Td32" align="right" class="LabelText" style="border-style: solid; border-color: Gray; border-right: 1px;" runat="server">
                            <SAHL:SAHLLabel ID="lblLastValuationDate" runat="server" CssClass="LabelText"  >-</SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr>
                        <td id="Td29" class="TitleText" runat="server">Occupancy Type</td>
                        <td id="Td27" style="width: 2px" runat="server"></td>
                        <td id="Td30" align="right" class="LabelText" style="border-style: solid; border-color: Gray; border-left: 1px; gray; border-right: 1px;" runat="server">
                            <SAHL:SAHLLabel ID="lOccupancy" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                        <td id="Td38" style="width: 2px" runat="server"></td>
                        <td align="right" style="border-style:solid; border-color:Gray;border-left:1px;border-right:1px" class="LabelText" width="25%">
                            <SAHL:SAHLDropDownList ID="ddlOccupancy" runat="server" CssClass="RequiredInput" Width="99.5%" ></SAHL:SAHLDropDownList>
                        </td>
                        <td></td>
                        <td style="border-style: solid; border-color: Gray; border-left: 1px"></td>
                        <td style="border-style: solid; border-color: Gray; border-right: 1px"></td>
                    </tr>
                    <tr class="rowSmall" runat="server" id="rowReadvance">
                        <td id="Td1" class="TitleText" runat="server">
                            <div id="rowReadvance_1" runat="server">
                                Readvance Amount
                                <table runat="server" visible="false" id="tblException">
                                    <tr>
                                        <td width="10px"></td>
                                        <td><asp:Image ID="Image1" runat="server" ImageUrl="~/Images/information.png" AlternateText="More..." ToolTip="Application marginally out of criteria" /></td>
                                        <td valign="top" style="color:Red">Application marginally out of criteria</td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                        <td id="Td2" style="width: 2px" runat="server"></td>
                        <td id="Td3" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lblReAdvance" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                        <td id="Td4" style="width: 2px" runat="server"></td>
                        <td id="Td10" align="right" style="border-style:solid; border-color:Gray;border-left:1px;border-right:1px" class="LabelText" runat="server" width="25%">
                            <SAHL:SAHLTextBox ID="tbReadvanceRequired" runat="server" DisplayInputType="Currency" Width="98.5%" onkeyup="disableNext()"></SAHL:SAHLTextBox>
                            <SAHL:SAHLLabel ID="lReadvanceRequired" runat="server" />
                        </td>
                        <td id="Td162" style="width: 2px" runat="server"></td>
                        <td id="Td163" class="TitleText" style="border-style: solid; border-color: Gray; border-left: 1px;" runat="server">Area Class</td>
                        <td id="Td164" align="right" class="LabelText" style="border-style: solid; border-color: Gray; border-right: 1px;" runat="server">
                            <SAHL:SAHLLabel ID="lblAreaClass" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr class="rowSmall" runat="server" id="rowFurtherAdvance">
                        <td id="Td11" runat="server">
                            <table id="rowFurtherAdvance_1" runat="server" width="100%">
                                <tr>
                                    <td class="TitleText" width="30%">Further Advance Limit</td>
                                    <td width="10px"></td>
                                    <td>
                                        <table runat="server" id="tblFAMessage">
                                            <tr>
                                                <td>
                                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/information.png" AlternateText="More..." ToolTip="LAA less the higher of CLV or Loan Balance" /></td>
                                                <td valign="top">Further Advance Limit - CLV to LAA <%=_furtherAdvanceMaxLAA.ToString(SAHL.Common.Constants.CurrencyFormat)%></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td id="Td12" style="width: 2px" runat="server"></td>
                        <td id="Td18" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lblFurtherAdvance" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                        <td id="Td19" style="width: 2px" runat="server"></td>
                        <td id="Td20" align="right" style="border-style:solid; border-color:Gray;border-left:1px;border-right:1px" class="LabelText" runat="server" >
                            <SAHL:SAHLTextBox ID="tbFurtherAdvReq" runat="server" DisplayInputType="Currency" Width="98.5%" onkeyup="disableNext()"></SAHL:SAHLTextBox>
                            <SAHL:SAHLLabel ID="lFurtherAdvReq" runat="server" />
                        </td>
                        <td id="Td176" style="width: 2px" runat="server"></td>
                        <td id="Td177" class="TitleText" style="border-style: solid; border-color: Gray; border-left: 1px;" runat="server">Open Date</td>
                        <td id="Td178" align="right" class="LabelText" style="border-style: solid; border-color: Gray; border-right: 1px;" runat="server">
                            <SAHL:SAHLLabel ID="lAccountOpenDate" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr class="rowSmall" runat="server" id="rowFurtherLoan">
                        <td id="Td33" class="TitleText" runat="server"><span id="rowFurtherLoan_1" runat="server">Further Loan</span></td>
                        <td id="Td34" style="width: 2px" runat="server"></td>
                        <td id="Td35" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lblFurtherLoan" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                        <td id="Td36" style="width: 2px" runat="server"></td>
                        <td id="Td37" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLTextBox ID="tbFurtherLoanReq" runat="server" DisplayInputType="Currency" Width="98.5%" onkeyup="disableNext()"></SAHL:SAHLTextBox>
                            <SAHL:SAHLLabel ID="lFurtherLoanReq" runat="server" />
                        </td>
                        <td id="Td184" style="width: 2px" runat="server"></td>
                        <td id="Td185" class="TitleText" style="border-style: solid; border-color: Gray; border-left: 1px;" runat="server">NCA Compliant</td>
                        <td id="Td186" align="right" class="LabelText" style="border-style: solid; border-color: Gray; border-right: 1px;" runat="server">
                            <SAHL:SAHLLabel ID="lNCACompliant" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr class="rowSmall" runat="server" id="rowFLClientEstimate" width="100%">
                        <td id="Td41" runat="server">
                            <table width="100%" id="rowFLClientEstimate_1" runat="server">
                                <tr>
                                    <td class="TitleText" width="30%">FL Clients Estimate</td>
                                    <td width="10px"></td>
                                    <td width="16px">
                                        <asp:Image ID="imgInfo" runat="server" ImageUrl="~/Images/information.png" AlternateText="More..." ToolTip="Based on the Clients estimated Valuation amount" /></td>
                                    <td valign="top">Based on the Clients<br />
                                        estimated Valuation amount</td>
                                </tr>
                            </table>
                        </td>
                        <td id="Td42" style="width: 2px" runat="server"></td>
                        <td id="Td43" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lblFurtherLoanClientEst" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                        <td id="Td44" style="width: 2px" runat="server"></td>
                        <td id="Td45" align="right" style="border-style:solid; border-color:Gray;border-left:1px;border-right:1px" runat="server"></td>
                        <td id="Td28" style="width: 2px" runat="server"></td>
                        <td id="Td79" class="TitleText" style="border-style: solid; border-color: Gray; border-left: 1px;" runat="server">Committed Loan Value (CLV)</td>
                        <td id="Td80" align="right" class="LabelText" style="border-style: solid; border-color: Gray; border-right: 1px;" runat="server">
                            <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lblCLV">-</SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr class="rowSmall" runat="server" id="rowFLTotal">
                        <td id="Td48" class="TitleText" runat="server"><span id="rowFLTotal_1" runat="server">Further Lending Total</span></td>
                        <td id="Td49" style="width: 2px" runat="server"></td>
                        <td id="Td50" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lblTotalCashRequired" runat="server" />
                        </td>
                        <td id="Td51" style="width: 2px" runat="server"></td>
                        <td id="Td52" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLTextBox ID="tbTotalCashRequired" runat="server" DisplayInputType="Currency" Width="98.5%" BackColor="#FFFF80" BorderStyle="None" onkeyup="disableNext()" onblur="copy()"></SAHL:SAHLTextBox>
                            <SAHL:SAHLLabel ID="lTotalCashRequired" runat="server" />
                        </td>
                        <td id="Td92" style="width: 2px" runat="server"></td>
                        <td id="Td93" class="TitleText" style="border-style: solid; border-color: Gray; border-left: 1px;" runat="server">Remaining Term</td>
                        <td id="Td94" align="right" class="LabelText" style="border-style: solid; border-color: Gray; border-right: 1px" runat="server">
                            <SAHL:SAHLLabel ID="lblRemainingTerm" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr class="rowSmall" runat="server" id="rowBondToRegister">
                        <td id="Td54" class="TitleText" runat="server">FL Bond to Register</td>
                        <td id="Td55" style="width: 2px" runat="server"></td>
                        <td id="Td56" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lBondToRegister" runat="server" />
                        </td>
                        <td id="Td57" style="width: 2px" runat="server"></td>
                        <td id="Td58" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLTextBox ID="tbBondToRegister" runat="server" DisplayInputType="Currency" Width="98.2%" onkeyup="disableNext()"></SAHL:SAHLTextBox>
                        </td>
                        <td id="Td100" style="width: 2px" runat="server"></td>
                        <td id="Td194" class="TitleText" style="border-style: solid; border-color: Gray; border-left: 1px;" runat="server">Accrued Interest</td>
                        <td id="Td195" align="right" class="LabelText" style="border-style: solid; border-color: Gray; border-right: 1px" runat="server">
                            <SAHL:SAHLLabel ID="lAccruedInterest" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr class="rowSmall" runat="server" id="rowFees">
                        <td id="Td62" class="TitleText" runat="server">Fees</td>
                        <td id="Td63" style="width: 2px" runat="server"></td>
                        <td id="Td64" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lFees" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                        <td id="Td65" style="width: 2px" runat="server"></td>
                        <td id="Td66" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px;" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lblFees" runat="server" CssClass="LabelText"  >-</SAHL:SAHLLabel>
                        </td>
                        <td id="Td67" style="width: 2px" runat="server"></td>
                        <td id="Td155" class="TitleText" style="border-style: solid; border-color: Gray; border-left: 1px;" runat="server">Advances this Year</td>
                        <td id="Td156" align="right" class="LabelText" style="border-style: solid; border-color: Gray; border-right: 1px;" runat="server">
                            <SAHL:SAHLLabel ID="lblAdvancesThisYear" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr id="Tr4" class="rowSmall" runat="server">
                        <td id="Td70" class="TitleText" runat="server">Total</td>
                        <td id="Td71" style="width: 2px" runat="server"></td>
                        <td id="Td72" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px; border-bottom: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel runat="server" ID="lblTtlFLInclFees" />
                        </td>
                        <td id="Td73" style="width: 2px" runat="server"></td>
                        <td id="Td74" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px; border-bottom: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel runat="server" ID="lTtlFLInclFees" />
                        </td>
                        <td></td>
                        <td style="border-style: solid; border-color: Gray; border-left: 1px"></td>
                        <td style="border-style: solid; border-color: Gray; border-right: 1px"></td>
                    </tr>
                    <tr id="Tr5" style="height: 5px" runat="server">
                        <td id="Td78" colspan="6" runat="server"></td>
                        <td colspan="2" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px"></td>
                    </tr>
                    <tr id="Tr6" class="rowSmall" runat="server">
                        <td id="Td81" class="TitleText" runat="server"></td>
                        <td id="Td82" style="width: 2px" runat="server"></td>
                        <td id="Td83" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px; border-top: 1px" class="TitleText" runat="server">Current</td>
                        <td id="Td84" style="width: 2px" runat="server"></td>
                        <td id="Td85" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px; border-top: 1px" class="TitleText" runat="server">New</td>
                        <td></td>
                        <td id="Td76" class="TitleText" style="border-style: solid; border-color: Gray; border-left: 1px;" runat="server">Product</td>
                        <td id="Td77" align="right" class="LabelText" style="border-style: solid; border-color: Gray; border-right: 1px" runat="server">
                            <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lblProduct">-</SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr id="Tr8" class="rowSmall" runat="server">
                        <td id="Td95" class="TitleText" runat="server">Loan Balance</td>
                        <td id="Td96" style="width: 2px" runat="server"></td>
                        <td id="Td97" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lblCurrentBalance" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                        <td id="Td98" style="width: 2px" runat="server"></td>
                        <td id="Td99" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lblNewLoanBalance" runat="server" CssClass="LabelText" >-</SAHL:SAHLLabel>
                        </td>
                        <td id="Td75" style="width: 2px" runat="server"></td>
                        <td rowspan="2" colspan="2" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px">
                            <table id="relatedChildAccounts" runat="server" style="width: 100%"></table>
                        </td>
                    </tr>
                    <tr id="Tr9" class="rowSmall" runat="server">
                        <td id="Td103" class="TitleText" runat="server">Total Bond Amount</td>
                        <td id="Td104" style="width: 2px" runat="server"></td>
                        <td id="Td105" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lblBondRegAmount" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                        <td id="Td106" style="width: 2px" runat="server"></td>
                        <td id="Td107" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lTTLBondRegAmount" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                        <td id="Td108" style="width: 2px" runat="server"></td>

                    </tr>
                    <tr id="Tr10" class="rowSmall" runat="server">
                        <td id="Td111" class="TitleText" runat="server">Loan Agreement Amount</td>
                        <td id="Td112" style="width: 2px" runat="server"></td>
                        <td id="Td113" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lblLoanAgreeAmount" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                        <td id="Td114" style="width: 2px" runat="server"></td>
                        <td id="Td115" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lTTLLoanAgreeAmount" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                        <td id="Td39" style="width: 2px" runat="server"></td>
                        <td style="border-style: solid; border-color: Gray; border-left: 1px"></td>
                        <td style="border-style: solid; border-color: Gray; border-right: 1px"></td>
                    </tr>
                    <tr id="Tr11" class="rowSmall" runat="server">
                        <td id="Td119" class="TitleText" runat="server">Instalment</td>
                        <td id="Td120" style="width: 2px" runat="server"></td>
                        <td id="Td121" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lblCurrentInstalment" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                        <td id="Td122" style="width: 2px" runat="server"></td>
                        <td id="Td123" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lblNewInstalment" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%"></SAHL:SAHLLabel>
                        </td>
                        <td></td>
                        <td style="border-style: solid; border-color: Gray; border-left: 1px" class="TitleText">
                            <SAHL:SAHLLabel ID="lbl20yrInstalmentText" runat="server" CssClass="TitleText" Style="width: 100%; height: 100%; color:red;" Font-Bold="true" Text="20yr Instalment" Visible="false"></SAHL:SAHLLabel>
                        </td>
                        <td style="border-style: solid; border-color: Gray; border-right: 1px" class="LabelText" align="right" >
                            <SAHL:SAHLLabel ID="lbl20yrInstalment" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%; color:red" Visible="false"></SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr id="rowAmortisingInstalment" class="rowSmall" runat="server" visible="false">
                        <td id="Td127" class="TitleText" runat="server">Amortising Instalment</td>
                        <td id="Td128" style="width: 2px" runat="server"></td>
                        <td id="Td129" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lblCurrentAMInstalment" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                        <td id="Td130" style="width: 2px" runat="server"></td>
                        <td id="Td131" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lblNewAMInstalment" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%"></SAHL:SAHLLabel>
                        </td>
                        <td id="Td132" style="width: 2px" runat="server"></td>
                        <td style="border-style: solid; border-color: Gray; border-left: 1px"></td>
                        <td style="border-style: solid; border-color: Gray; border-right: 1px"></td>
                    </tr>
                    <tr id="Tr12" class="rowSmall" runat="server">
                        <td id="Td135" class="TitleText" runat="server">LTV</td>
                        <td id="Td136" style="width: 2px" runat="server"></td>
                        <td id="Td137" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lblCurrentLTV" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                        <td id="Td138" style="width: 2px" runat="server"></td>
                        <td id="Td139" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lblNewLTV" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                        <td></td>
                        <td style="border-style: solid; border-color: Gray; border-left: 1px"></td>
                        <td style="border-style: solid; border-color: Gray; border-right: 1px"></td>
                    </tr>
                    <tr id="Tr13" class="rowSmall" runat="server">
                        <td id="Td143" class="TitleText" runat="server">PTI</td>
                        <td id="Td144" style="width: 2px" runat="server"></td>
                        <td id="Td145" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lblCurrentPTI" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                        <td id="Td146" style="width: 2px" runat="server"></td>
                        <td id="Td147" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lblNewPTI" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                        <td id="Td148" style="width: 2px" runat="server"></td>
                        <td style="border-style: solid; border-color: Gray; border-left: 1px" class="TitleText">
                            <SAHL:SAHLLabel ID="lbl20yrPTIText" runat="server" CssClass="TitleText" Style="width: 100%; height: 100%; color:red"  Font-Bold="true" Text="20yr PTI" Visible="false"></SAHL:SAHLLabel>
                        </td>
                        <td style="border-style: solid; border-color: Gray; border-right: 1px" class="LabelText" align="right" >
                            <SAHL:SAHLLabel ID="lbl20yrPTI" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%; color:red" Visible="false"></SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr id="Tr14" class="rowSmall" runat="server">
                        <td id="Td149" class="TitleText" runat="server">Link Rate</td>
                        <td id="Td150" style="width: 2px" runat="server"></td>
                        <td id="Td151" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lblCurrentLinkRate" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                        <td id="Td152" style="width: 2px" runat="server"></td>
                        <td id="Td153" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lblNewLinkRate" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                        <td></td>
                        <td style="border-style: solid; border-color: Gray; border-left: 1px"></td>
                        <td style="border-style: solid; border-color: Gray; border-right: 1px"></td>
                    </tr>
                    <tr id="Tr15" class="rowSmall" runat="server">
                        <td id="Td157" class="TitleText" runat="server">Discount on Rate</td>
                        <td id="Td158" style="width: 2px" runat="server"></td>
                        <td id="Td159" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lblDiscount" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                        <td id="Td160" style="width: 2px" runat="server"></td>
                        <td id="Td161" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lDiscount" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                        <td></td>
                        <td style="border-style: solid; border-color: Gray; border-left: 1px"></td>
                        <td style="border-style: solid; border-color: Gray; border-right: 1px"></td>
                    </tr>
                    <tr id="Tr16" class="rowSmall" runat="server">
                        <td id="Td165" class="TitleText" runat="server">Market Rate</td>
                        <td id="Td166" style="width: 2px" runat="server"></td>
                        <td id="Td167" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lMarketRateAccount" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                        <td id="Td168" style="width: 2px" runat="server"></td>
                        <td id="Td169" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lMarketRateApplication" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                        <td id="Td116" style="width: 2px" runat="server"></td>
                        <td style="border-style: solid; border-color: Gray; border-left: 1px" class="TitleText">Group Exposure LTV</td>
                        <td align="right" style="border-style: solid; border-color: Gray; border-right: 1px" class="LabelText">
                            <SAHL:SAHLLabel ID="lblGroupExposureLTV" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%"></SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr id="Tr17" class="rowSmall" runat="server">
                        <td id="Td171" class="TitleText" runat="server">Loan Rate</td>
                        <td id="Td172" style="width: 2px" runat="server"></td>
                        <td id="Td173" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lblCurrentLoanRate" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                        <td id="Td174" style="width: 2px" runat="server"></td>
                        <td id="Td175" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lblNewLoanRate" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                        <td id="Td124" style="width: 2px" runat="server"></td>
                        <td style="border-style: solid; border-color: Gray; border-left: 1px" class="TitleText">Group Exposure PTI</td>
                        <td align="right" style="border-style: solid; border-color: Gray; border-right: 1px" class="LabelText">
                            <SAHL:SAHLLabel ID="lblGroupExposurePTI" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%"></SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr id="Tr18" class="rowSmall" runat="server">
                        <td id="Td179" class="TitleText" runat="server">Category</td>
                        <td id="Td180" style="width: 2px" runat="server"></td>
                        <td id="Td181" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lCategoryAccount" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                        <td id="Td182" style="width: 2px" runat="server"></td>
                        <td id="Td183" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lCategoryApplication" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                            <SAHL:SAHLTextBox ID="tbNewCategory" runat="server" CssClass="LabelText" Style="display: none;">-</SAHL:SAHLTextBox>
                        </td>
                        <td id="Td140" style="width: 2px" runat="server"></td>
                        <td id="Td117" class="TitleText" style="border-style: solid; border-color: Gray; border-left: 1px;" runat="server">Arrear Balance</td>
                        <td id="Td118" align="right" class="LabelText" style="border-style: solid; border-color: Gray; border-right: 1px" runat="server">
                            <SAHL:SAHLLabel ID="lblArrearsBalance" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                    </tr>
                    <tr id="Tr7" class="rowSmall" runat="server">
                        <td id="Td87" class="TitleText" runat="server">SPV</td>
                        <td id="Td88" style="width: 2px" runat="server"></td>
                        <td id="Td89" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px; border-bottom: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLLabel ID="lblSPV" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel></td>
                        <td id="Td90" style="width: 2px" runat="server"></td>
                        <td id="Td91" align="right" style="border-style: solid; border-color: Gray; border-left: 1px; border-right: 1px; border-bottom: 1px" class="LabelText" runat="server">
                            <SAHL:SAHLDropDownList runat="server" ID="ddlSPV" Visible="False" OnChange="enableCalculate()" Width="99.5%" />
                            <SAHL:SAHLLabel ID="lblNewSPV" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                        <td id="Td154" style="width: 2px" runat="server"></td>
                        <td id="Td125" class="TitleText" style="border-style: solid; border-color: Gray; border-left: 1px;" runat="server">Arrear last 6 months</td>
                        <td id="Td126" align="right" class="LabelText" style="border-style: solid; border-color: Gray; border-right: 1px;" runat="server">
                            <SAHL:SAHLLabel ID="lblArrearsLast6Mth" runat="server" CssClass="LabelText" Style="width: 100%; height: 100%">-</SAHL:SAHLLabel>
                        </td>
                    </tr>
                </table>

                <SAHL:SAHLTextBox runat="server" ID="tbReadvanceAccepted" Style="display: none" />
                <SAHL:SAHLTextBox runat="server" ID="tbFurtherAdvanceAccepted" Style="display: none" />
                <SAHL:SAHLTextBox runat="server" ID="tbFurtherLoanAccepted" Style="display: none" />
                <SAHL:SAHLTextBox runat="server" ID="tbApprovalMode" Style="display: none" />
                <SAHL:SAHLTextBox runat="server" ID="tbProduct" Style="display: none" />
                <table class="tableStandard" runat="server" id="ApplicationInProgressMessage" visible="False">
                    <tr id="Tr20" runat="server">
                        <td id="Td204" runat="server">
                            <SAHL:SAHLLabel runat="server" ID="lblApplicationInProgress" ForeColor="Red" Font-Size="Large"></SAHL:SAHLLabel></td>
                    </tr>
                </table>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel runat="server" ID="tpVarifix" HeaderText="Varifix Comparison" Enabled="False">
            <ContentTemplate>
                <asp:Panel runat="server" ID="pnlVarifix" Style="font-size: 0.8em;">
                    <table>
                        <tr class="rowSmall">
                            <td></td>
                            <td colspan="3" class="TitleText">CURRENT</td>
                            <td style="width: 2px"></td>
                            <td colspan="3" class="TitleText">CALCULATED</td>
                        </tr>
                        <tr class="rowSmall">
                            <td colspan="8"></td>
                        </tr>
                        <tr class="rowSmall">
                            <td></td>
                            <td class="TitleText" style="width: 14%; border-style: solid; border-color: Gray; border-left: 1px; border-top: 1px">Fixed</td>
                            <td class="TitleText" style="width: 14%; border-style: solid; border-color: Gray; border-top: 1px">Variable</td>
                            <td class="TitleText" style="width: 14%; border-style: solid; border-color: Gray; border-right: 1px; border-top: 1px">Total</td>
                            <td style="width: 2px"></td>
                            <td class="TitleText" style="width: 14%; border-style: solid; border-color: Gray; border-left: 1px; border-top: 1px">Fixed</td>
                            <td class="TitleText" style="width: 14%; border-style: solid; border-color: Gray; border-top: 1px">Variable</td>
                            <td class="TitleText" style="width: 14%; border-style: solid; border-color: Gray; border-right: 1px; border-top: 1px">Total</td>
                        </tr>
                        <tr class="rowSmall">
                            <td>Market Rate</td>
                            <td style="border-style: solid; border-color: Gray; border-left: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCurrMarketRateFix" /></td>
                            <td align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCurrMarketRateVar" /></td>
                            <td style="border-style: solid; border-color: Gray; border-right: 1px;"></td>
                            <td style="width: 2px"></td>
                            <td style="border-style: solid; border-color: Gray; border-left: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCalcMarketRateFix" /></td>
                            <td align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCalcMarketRateVar" /></td>
                            <td style="border-style: solid; border-color: Gray; border-right: 1px;"></td>
                        </tr>
                        <tr class="rowSmall">
                            <td>Link Rate</td>
                            <td style="border-style: solid; border-color: Gray; border-left: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCurrLinkRateFix" /></td>
                            <td align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCurrLinkRateVar" /></td>
                            <td style="border-style: solid; border-color: Gray; border-right: 1px;"></td>
                            <td style="width: 2px"></td>
                            <td style="border-style: solid; border-color: Gray; border-left: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCalcLinkRateFix" /></td>
                            <td align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCalcLinkRateVar" /></td>
                            <td style="border-style: solid; border-color: Gray; border-right: 1px;"></td>
                        </tr>
                        <tr class="rowSmall">
                            <td>Loan Rate</td>
                            <td style="border-style: solid; border-color: Gray; border-left: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCurrLoanRateFix" /></td>
                            <td align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCurrLoanRateVar" /></td>
                            <td style="border-style: solid; border-color: Gray; border-right: 1px;"></td>
                            <td style="width: 2px"></td>
                            <td style="border-style: solid; border-color: Gray; border-left: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCalcLoanRateFix" /></td>
                            <td align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCalcLoanRateVar" /></td>
                            <td style="border-style: solid; border-color: Gray; border-right: 1px;"></td>
                        </tr>
                        <tr class="rowSmall">
                            <td>Current Balance</td>
                            <td style="border-style: solid; border-color: Gray; border-left: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCurrCurrentBalanceFix" /></td>
                            <td align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCurrCurrentBalanceVar" /></td>
                            <td style="border-style: solid; border-color: Gray; border-right: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCurrCurrentBalanceTotal" /></td>
                            <td style="width: 2px"></td>
                            <td style="border-style: solid; border-color: Gray; border-left: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCalcCurrentBalanceFix" /></td>
                            <td align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCalcCurrentBalanceVar" /></td>
                            <td style="border-style: solid; border-color: Gray; border-right: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCalcCurrentBalanceTotal" /></td>
                        </tr>
                        <tr class="rowSmall">
                            <td>Readvance</td>
                            <td style="border-style: solid; border-color: Gray; border-left: 1px;"></td>
                            <td></td>
                            <td style="border-style: solid; border-color: Gray; border-right: 1px;"></td>
                            <td style="width: 2px"></td>
                            <td style="border-style: solid; border-color: Gray; border-left: 1px;"></td>
                            <td align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCalcReadvance" /></td>
                            <td style="border-style: solid; border-color: Gray; border-right: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCalcTotalPlusReadvance" /></td>
                        </tr>
                        <tr class="rowSmall">
                            <td>Further Advance</td>
                            <td style="border-style: solid; border-color: Gray; border-left: 1px;"></td>
                            <td></td>
                            <td style="border-style: solid; border-color: Gray; border-right: 1px;"></td>
                            <td style="width: 2px"></td>
                            <td style="border-style: solid; border-color: Gray; border-left: 1px;"></td>
                            <td align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCalcFurtherAdvance" /></td>
                            <td style="border-style: solid; border-color: Gray; border-right: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCalcTotalPlusFurtherAdvance" /></td>
                        </tr>
                        <tr class="rowSmall">
                            <td>Further Loan</td>
                            <td style="border-style: solid; border-color: Gray; border-left: 1px;"></td>
                            <td></td>
                            <td style="border-style: solid; border-color: Gray; border-right: 1px;"></td>
                            <td style="width: 2px"></td>
                            <td style="border-style: solid; border-color: Gray; border-left: 1px;"></td>
                            <td align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCalcFurtherLoan" /></td>
                            <td style="border-style: solid; border-color: Gray; border-right: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCalcTotalPlusFurtherLoan" /></td>
                        </tr>
                        <tr class="rowSmall">
                            <td>Current Instalment</td>
                            <td style="border-style: solid; border-color: Gray; border-left: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCurrInstalmentFix" /></td>
                            <td align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCurrInstalmentVar" /></td>
                            <td style="border-style: solid; border-color: Gray; border-right: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCurrInstalmentTotal" /></td>
                            <td style="width: 2px"></td>
                            <td style="border-style: solid; border-color: Gray; border-left: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCalcInstalmentFix" /></td>
                            <td align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCalcInstalmentVar" /></td>
                            <td style="border-style: solid; border-color: Gray; border-right: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCalcInstalmentTotal" /></td>
                        </tr>
                        <tr class="rowSmall">
                            <td>Interest Accrued</td>
                            <td style="border-style: solid; border-color: Gray; border-left: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCurrInterestAccruedFix" /></td>
                            <td align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCurrInterestAccruedVar" /></td>
                            <td style="border-style: solid; border-color: Gray; border-right: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCurrInterestAccruedTotal" /></td>
                            <td style="width: 2px"></td>
                            <td style="border-style: solid; border-color: Gray; border-left: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCalcInterestAccruedFix" /></td>
                            <td align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCalcInterestAccruedVar" /></td>
                            <td style="border-style: solid; border-color: Gray; border-right: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCalcInterestAccruedTotal" /></td>
                        </tr>
                        <tr class="rowSmall">
                            <td nowrap="nowrap">New Balance + Interest&nbsp;&nbsp;</td>
                            <td style="border-style: solid; border-color: Gray; border-left: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCurrTotalPlusInterestFix" /></td>
                            <td align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCurrTotalPlusInterestVar" /></td>
                            <td style="border-style: solid; border-color: Gray; border-right: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCurrTotalPlusInterestTotal" /></td>
                            <td style="width: 2px"></td>
                            <td style="border-style: solid; border-color: Gray; border-left: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCalcTotalPlusInterestFix" /></td>
                            <td align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCalcTotalPlusInterestVar" /></td>
                            <td style="border-style: solid; border-color: Gray; border-right: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCalcTotalPlusInterestTotal" /></td>
                        </tr>
                        <tr class="rowSmall">
                            <td>New Instalment</td>
                            <td style="border-style: solid; border-color: Gray; border-left: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCurrNewInstalmentFix" /></td>
                            <td align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCurrNewInstalmentVar" /></td>
                            <td style="border-style: solid; border-color: Gray; border-right: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCurrNewInstalmentTotal" /></td>
                            <td style="width: 2px"></td>
                            <td style="border-style: solid; border-color: Gray; border-left: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCalcNewInstalmentFix" /></td>
                            <td align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCalcNewInstalmentVar" /></td>
                            <td style="border-style: solid; border-color: Gray; border-right: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCalcNewInstalmentTotal" /></td>
                        </tr>
                        <tr class="rowSmall">
                            <td>Arrear Balance</td>
                            <td style="border-style: solid; border-color: Gray; border-left: 1px; border-bottom: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCurrArrearFix" /></td>
                            <td style="border-style: solid; border-color: Gray; border-bottom: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCurrArrearVar" /></td>
                            <td style="border-style: solid; border-color: Gray; border-right: 1px; border-bottom: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCurrArrearTotal" /></td>
                            <td style="width: 2px"></td>
                            <td style="border-style: solid; border-color: Gray; border-left: 1px; border-bottom: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCalcArrearFix" /></td>
                            <td style="border-style: solid; border-color: Gray; border-bottom: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCalcArrearVar" /></td>
                            <td style="border-style: solid; border-color: Gray; border-right: 1px; border-bottom: 1px;" align="right">
                                <SAHL:SAHLLabel runat="server" CssClass="LabelText" ID="lVFCalcArrearTotal" /></td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel runat="server" ID="tpLoanCondition" HeaderText="Loan Details" Enabled="False">
            <ContentTemplate>
                <asp:Panel ID="pnlDetail" runat="server" GroupingText="Detail Types" Style="font-size: 0.8em;">
                    <br />
                    <SAHL:SAHLGridView ID="gvDetail" runat="server" AutoGenerateColumns="False"
                        EmptyDataSetMessage="There are no further lending critical detail types loaded against this Account." EnableViewState="False"
                        FixedHeader="False" ShowHeader="False" NullDataSetMessage="There are no further lending critical detail types loaded against this Account."
                        EmptyDataText="There are no further lending critical detail types loaded against this Account." GridHeight="" GridWidth="" Invisible="False" SelectFirstRow="True" TotalsFooter="True">
                    </SAHL:SAHLGridView>
                    <br />
                </asp:Panel>
                <asp:Panel ID="pnlCondition" runat="server" GroupingText="Conditions" Style="font-size: 0.8em;">
                    <br />
                    <SAHL:SAHLGridView ID="gvConditions" runat="server" AutoGenerateColumns="False"
                        EmptyDataSetMessage="There are no Conditions for this Account." EnableViewState="False"
                        FixedHeader="False" ShowHeader="False" NullDataSetMessage="There are no Conditions for this Account."
                        EmptyDataText="There are no Conditions for this Account." GridHeight="" GridWidth="" Invisible="False" SelectFirstRow="True" TotalsFooter="True">
                    </SAHL:SAHLGridView>
                    <br />
                    <asp:CheckBox ID="chkCondition" runat="server" Text="  Conditions have been verified" />
                    <SAHL:SAHLTextBox ID="tbSendApplication" runat="server" Style="display: none" />
                    <br />
                    <br />
                </asp:Panel>

                <asp:Panel ID="pnlSubsidy" runat="server" GroupingText="Subsidy" Style="font-size: 0.8em;">
                    <br />
                    <SAHL:SAHLGridView ID="gvSubsidy" runat="server" AutoGenerateColumns="False"
                        EmptyDataSetMessage="There are no Subsidies for this Account." EnableViewState="False"
                        FixedHeader="False" ShowHeader="False" NullDataSetMessage="There are no Subsidies for this Account."
                        EmptyDataText="There are no Subsidies for this Account." GridHeight="" GridWidth="" Invisible="False" SelectFirstRow="True" TotalsFooter="True">
                    </SAHL:SAHLGridView>
                    <br />
                </asp:Panel>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>

        <ajaxToolkit:TabPanel runat="server" ID="tpInformation" HeaderText="Application Information" Enabled="False">
            <ContentTemplate>
                <table style="width: 98%; font-size: 0.8em;" class="tableStandard">
                    <tr>
                        <td colspan="3">

                            <asp:Panel ID="pnlLegalEntities" runat="server" GroupingText="Legal Entities to Co-Sign for Further Lending Application">
                                <br />
                                <SAHLWebControl:LegalEntityGrid ID="LegalEntityGrid" runat="server" ColumnIDPassportVisible="True" OnSelectedIndexChanged="LegalEntityGrid_SelectedIndexChanged" OnRowDataBound="LegalEntityGrid_RowDataBound">
                                </SAHLWebControl:LegalEntityGrid>
                            </asp:Panel>

                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlLEContactInfo" runat="server" GroupingText="Legal Entity Contact Information">
                                <table style="width: 100%">
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>Home Phone</td>
                                        <td>
                                            <SAHL:SAHLPhone ID="tbHomePhone" runat="server" />
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>Work Phone</td>
                                        <td>
                                            <SAHL:SAHLPhone ID="tbWorkPhone" runat="server" />
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>Fax Number</td>
                                        <td>
                                            <SAHL:SAHLPhone ID="tbFax" runat="server" />
                                            <SAHL:SAHLCustomValidator ID="valtbFax" runat="server" ControlToValidate="tbFax" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Cell Number</td>
                                        <td>
                                            <SAHL:SAHLTextBox ID="tbCellNumber" runat="server" /></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>Email Address</td>
                                        <td>
                                            <SAHL:SAHLTextBox ID="tbEmail" runat="server" Columns="30" MaxLength="50"/></td>
                                    </tr>
                                </table>
                                <!-- hidden inputs -->
                                <SAHL:SAHLTextBox runat="server" ID="tbMultiRecipientCorrespondenceData" Style="display: none" />
                            </asp:Panel>
                        </td>
                        <td>&nbsp;</td>
                        <td valign="top">
                            <asp:Panel ID="pnlAlternateContactInfo" runat="server" GroupingText="Alternate Options">
                                <table style="width: 100%">
                                    <tr>
                                        <td>
                                            <SAHL:SAHLTextBox runat="server" ID="tbSelectedLegalEntityKey" Style="display: none" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Fax Number
                                        </td>
                                        <td>
                                            <SAHL:SAHLTextBox ID="tbAlternateFaxNumber" runat="server" Columns="30" MaxLength="50" onblur="PersistAlternateLegalEntityCorrespondenceSelection('3', this.value)" onkeypress="SAHLTextBox_NumOnly();" /><SAHL:SAHLCustomValidator ID="valtbAlternateFax" runat="server" ControlToValidate="tbAlternateFaxNumber" ErrorMessage="Please enter a valid email address to send to." />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Email Address
                                        </td>
                                        <td>
                                            <SAHL:SAHLTextBox ID="tbAlternateEmail" runat="server" Columns="30" MaxLength="50" onblur="PersistAlternateLegalEntityCorrespondenceSelection('2', this.value)" /><label id="lblEmailError" title="error" style="color: red; display: none;"> Please enter a valid email address to send to.</label><SAHL:SAHLCustomValidator ID="valtbAlternateEmail" runat="server" ControlToValidate="tbAlternateEmail" ErrorMessage="Please enter a valid email address to send to." />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel runat="server" ID="tpConfirmation" HeaderText="Generate Application" Enabled="False">
            <ContentTemplate>
                <asp:Panel ID="pnlConfirmationBody" runat="server" GroupingText="Confirmation" Style="font-size: 0.8em;">
                    <br />
                    <table width="98%" class="tableStandard">
                        <tr>
                            <td>
                                <table width="100%">
                                    <tr>
                                        <td>Account Number</td>
                                        <td>
                                            <SAHL:SAHLLabel ID="lblAccountNumber" runat="server">-</SAHL:SAHLLabel></td>
                                    </tr>
                                    <tr>
                                        <td>Products on Account</td>
                                        <td>
                                            <SAHL:SAHLLabel ID="lblAccountProduct" runat="server">-</SAHL:SAHLLabel></td>
                                    </tr>
                                    <tr>
                                        <td>Created Date</td>
                                        <td>
                                            <SAHL:SAHLLabel ID="lblCreatedDate" runat="server">-</SAHL:SAHLLabel></td>
                                    </tr>
                                    <tr>
                                        <td>Initiated By</td>
                                        <td>
                                            <SAHL:SAHLLabel ID="lblInitiatedBy" runat="server">-</SAHL:SAHLLabel></td>
                                    </tr>
                                    <tr>
                                        <td>Application Created By</td>
                                        <td>
                                            <SAHL:SAHLLabel ID="lblCreatedUser" runat="server">-</SAHL:SAHLLabel></td>
                                    </tr>
                                    <tr>
                                        <td>Total Amount Required</td>
                                        <td>
                                            <SAHL:SAHLLabel ID="lblTotalAmountRequired" runat="server">-</SAHL:SAHLLabel></td>
                                    </tr>
                                    <tr>
                                        <td>Rapid Advance Required</td>
                                        <td>
                                            <SAHL:SAHLLabel ID="lblRapidRequired" runat="server">-</SAHL:SAHLLabel></td>
                                    </tr>
                                    <tr>
                                        <td>Further Advance Required</td>
                                        <td>
                                            <SAHL:SAHLLabel ID="lblFurtherAdvanceRequired" runat="server">-</SAHL:SAHLLabel></td>
                                    </tr>
                                    <tr>
                                        <td>Further Loan Required</td>
                                        <td>
                                            <SAHL:SAHLLabel ID="lblFurtherLoanRequired" runat="server">-</SAHL:SAHLLabel></td>
                                    </tr>
                                    <tr>
                                        <td>Legal Fee Estimate</td>
                                        <td>
                                            <SAHL:SAHLLabel ID="lblLegalFeeEstimate" runat="server">-</SAHL:SAHLLabel></td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:Panel ID="pnlSendingInformation" runat="server" GroupingText="Correspondence Information">
                                                <table width="100%">
                                                    <tbody>
                                                        <asp:Repeater ID="rptSendingInformation" runat="server" OnItemDataBound="rptSendingInformation_OnItemDataBound">
                                                            <HeaderTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <th style="width: 60%;">Contact</th>
                                                                        <th style="width: 15%;">Method</th>
                                                                        <th style="width: 25%;">Contact Info</th>
                                                                    </tr>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td>
                                                                        <SAHL:SAHLLabel ID="SAHLlblDisplayName" runat="server"> </SAHL:SAHLLabel>
                                                                    </td>
                                                                    <td>
                                                                        <SAHL:SAHLLabel ID="SAHLlblCorrespondenceMedium" runat="server"> </SAHL:SAHLLabel>
                                                                    </td>
                                                                    <td>
                                                                        <SAHL:SAHLLabel ID="SAHLlblContactInfo" runat="server"> </SAHL:SAHLLabel>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <AlternatingItemTemplate>
                                                                <tr>
                                                                    <td>
                                                                        <SAHL:SAHLLabel ID="SAHLlblDisplayName" runat="server"> </SAHL:SAHLLabel>
                                                                    </td>
                                                                    <td>
                                                                        <SAHL:SAHLLabel ID="SAHLlblCorrespondenceMedium" runat="server"> </SAHL:SAHLLabel>
                                                                    </td>
                                                                    <td>
                                                                        <SAHL:SAHLLabel ID="SAHLlblContactInfo" runat="server"> </SAHL:SAHLLabel>
                                                                    </td>
                                                                </tr>
                                                            </AlternatingItemTemplate>

                                                        </asp:Repeater>
                                                    </tbody>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="pnlCalculatedAmount" runat="server" GroupingText="Calculated Amounts">
                                                <table width="100%">
                                                    <tr>
                                                        <td>New Loan Balance</td>
                                                        <td>
                                                            <SAHL:SAHLLabel ID="lblConfimNewLoanBalance" runat="server">-</SAHL:SAHLLabel></td>
                                                    </tr>
                                                    <tr>
                                                        <td>New Instalment</td>
                                                        <td>
                                                            <SAHL:SAHLLabel ID="lblConfirmNewInstalment" runat="server">-</SAHL:SAHLLabel></td>
                                                    </tr>
                                                    <tr>
                                                        <td>New LTV %</td>
                                                        <td>
                                                            <SAHL:SAHLLabel ID="lblConfirmNewLTV" runat="server">-</SAHL:SAHLLabel></td>
                                                    </tr>
                                                    <tr>
                                                        <td>New PTI %</td>
                                                        <td>
                                                            <SAHL:SAHLLabel ID="lblConfimrNewPTI" runat="server">-</SAHL:SAHLLabel></td>
                                                    </tr>
                                                    <tr>
                                                        <td>New Loan Rate %</td>
                                                        <td>
                                                            <SAHL:SAHLLabel ID="lblConfirmNewLoanRateV" runat="server">-</SAHL:SAHLLabel></td>
                                                    </tr>
                                                    <tr runat="server" id="rowConfirmationFixRate" visible="false">
                                                        <td>New Loan Rate (Fixed) %</td>
                                                        <td>
                                                            <SAHL:SAHLLabel ID="lblConfirmNewLoanRateF" runat="server">-</SAHL:SAHLLabel></td>
                                                    </tr>
                                                    <tr>
                                                        <td>New Link Rate %</td>
                                                        <td>
                                                            <SAHL:SAHLLabel ID="lblConfirmNewLinkRate" runat="server">-</SAHL:SAHLLabel></td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <asp:Panel ID="pnlAttachForms" runat="server">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chkIncludeNaedoForm" runat="server" Text="Include Naedo Form" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
    </ajaxToolkit:TabContainer>

    <table style="width: 98%" runat="server" id="btnbar" class="tableStandard">
        <tr>
            <td style="height: 4px">&nbsp;</td>

        </tr>
        <tr>
            <td align="right" style="width: 25%">
                <SAHL:SAHLButton ID="btnReset" runat="server" ButtonSize="Size5" CausesValidation="false" OnClick="btnReset_Click" Text="Reset Calculations" Width="142px" />
            </td>
            <td align="left" nowrap="nowrap" style="width: 25%">
                <SAHL:SAHLButton ID="btnPreview" runat="server" ButtonSize="Size5" OnClientClick="return onPreviewClicked()" CausesValidation="false" Text="Preview" Enabled="false" />
                <SAHL:SAHLButton ID="btnCalculate" runat="server" ButtonSize="Size5" OnClick="btnCalculate_Click" CausesValidation="false" Text="Calculate" />
                <SAHL:SAHLButton ID="btnUpdateContact" runat="server" ButtonSize="Size5" OnClick="btnUpdateContact_Click" Style="display: none" CausesValidation="false" Text="Update Contact Info" Width="180px" />
                <SAHL:SAHLButton ID="btnGenerate" runat="server" ButtonSize="Size5" OnClick="btnGenerate_Click" Style="display: none" CausesValidation="false" Text="Generate Application" Width="180px" />

                <SAHL:SAHLButton ID="btnQuickCash" runat="server" ButtonSize="Size5" Visible="false" OnClick="btnQuickCash_Click" CausesValidation="false" Text="Quick Cash" />
                <SAHL:SAHLButton ID="btnSubmit" runat="server" ButtonSize="Size5" Visible="false" OnClick="btnSubmit_Click" CausesValidation="false" Text="-" />
                <SAHL:SAHLButton ID="btnCancel" runat="server" ButtonSize="Size5" Visible="true" OnClick="btnCancel_Click" CausesValidation="false" Text="Cancel" />&nbsp;
            </td>
            <td align="right" nowrap="nowrap">
                <SAHL:SAHLButton ID="btnBack" runat="server" OnClick="btnBack_Click" Text="<- Back" Width="100px" CssClass="BtnNormal4" Style="display: none" />
                <SAHL:SAHLButton ID="btnNext" runat="server" OnClick="btnNext_Click" Text="Next ->" Width="100px" CssClass="BtnNormal4" Enabled="False" SecurityTag="FLCalculatorCreateApplication" /></td>
        </tr>
    </table>

</asp:Content>

