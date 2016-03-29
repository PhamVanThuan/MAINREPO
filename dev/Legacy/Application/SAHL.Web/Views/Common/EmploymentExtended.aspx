<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"
    Codebehind="EmploymentExtended.aspx.cs" Inherits="SAHL.Web.Views.Common.EmploymentExtended"
    Title="Untitled Page" EnableViewState="False" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">

    <script type="text/javascript" language="javascript">
    function calculateTotals(txtTotalId, arrElements, add)
    {
        // debugger;
        var total = 0;
        
        for (var i=0; i<arrElements.length; i++)
        {
            var val = SAHLCurrencyBox_getValue(arrElements[i]).replaceAll(',', '');
            if (isNaN(val) || isNaN(parseFloat(val))) 
                continue; 
            if (i == 0 || add)
                total += parseFloat(val);
            else
                total -= parseFloat(val);
        }
        
        // format the number
        total = formatNumber(total, 2, ',', '.'); 
        SAHLCurrencyBox_setValue(txtTotalId, total);
    }
    
    function calculateTotalsConfirmed()
    {
        calculateTotals('<%=txtConfVariableTotal.ClientID%>'
            ,new Array('<%=txtConfCommission.ClientID%>'
                ,'<%=txtConfOvertime.ClientID%>'
                ,'<%=txtConfShift.ClientID%>'
                ,'<%=txtConfPerformance.ClientID%>'
                ,'<%=txtConfAllowances.ClientID%>')
            ,true
        );
        calculateTotals('<%=txtConfGrossIncome.ClientID%>'
            ,new Array('<%=txtConfBasicIncome.ClientID%>'
                ,'<%=txtConfVariableTotal.ClientID%>')
            ,true
        );
        calculateTotals('<%=txtConfDeductions.ClientID%>'
            ,new Array('<%=txtConfPAYE.ClientID%>'
                ,'<%=txtConfUIF.ClientID%>'
                ,'<%=txtConfPension.ClientID%>'
                ,'<%=txtConfMedicalAid.ClientID%>')
            ,true
        );
        calculateTotals('<%=txtConfNetIncome.ClientID%>'
            ,new Array('<%=txtConfGrossIncome.ClientID%>'
                ,'<%=txtConfDeductions.ClientID%>')
            ,false
        );    
    }
    
    function calculateTotalsMonthly()
    {
        calculateTotals('<%=txtMonthVariableTotal.ClientID%>'
            ,new Array('<%=txtMonthCommission.ClientID%>'
                ,'<%=txtMonthOvertime.ClientID%>'
                ,'<%=txtMonthShift.ClientID%>'
                ,'<%=txtMonthPerformance.ClientID%>'
                ,'<%=txtMonthAllowances.ClientID%>')
            ,true
        );
        calculateTotals('<%=txtMonthGrossIncome.ClientID%>'
            ,new Array('<%=txtMonthBasicIncome.ClientID%>'
                ,'<%=txtMonthVariableTotal.ClientID%>')
            ,true
        );
        calculateTotals('<%=txtMonthDeductions.ClientID%>'
            ,new Array('<%=txtMonthPAYE.ClientID%>'
                ,'<%=txtMonthUIF.ClientID%>'
                ,'<%=txtMonthPension.ClientID%>'
                ,'<%=txtMonthMedicalAid.ClientID%>')
            ,true
        );
        calculateTotals('<%=txtMonthNetIncome.ClientID%>'
            ,new Array('<%=txtMonthGrossIncome.ClientID%>'
                ,'<%=txtMonthDeductions.ClientID%>')
            ,false
        );
        
    }
    
    function init()
    {
        calculateTotalsMonthly();
        calculateTotalsConfirmed();
    }
    
    window.onload = init;
    
    </script>

    <asp:Panel ID="pnlMain" runat="server" Visible="true" Width="99%">
        <div style="float: left; margin-left: 20px; width: 99%;">
            <%-- // Panel containing labels for rows --%>
            <asp:Panel runat="server" ID="pnlLabels" CssClass="cell" Style="width: 25%;">
                <div class="row">
                    <div class="cellInput">
                        &nbsp;</div>
                </div>
                <div class="row">
                    <div class="cellInput titleText">
                        Basic Income (Gross)</div>
                </div>
                <div class="row">
                    <div class="cellInput titleText">
                        Variable Monthly Income (Total)</div>
                </div>
                <div class="row">
                    <div class="cellInput">
                        Commission</div>
                </div>
                <div class="row">
                    <div class="cellInput">
                        Overtime</div>
                </div>
                <div class="row">
                    <div class="cellInput">
                        Shift</div>
                </div>
                <div class="row">
                    <div class="cellInput">
                        Performance</div>
                </div>
                <div class="row">
                    <div class="cellInput">
                        Allowances</div>
                </div>
                <div class="row">
                    <div class="cellInput titleText">
                        Gross Monthly Income</div>
                </div>
                <div class="row">
                    <div class="cellInput titleText">
                        Deductions (Total)</div>
                </div>
                <div class="row">
                    <div class="cellInput">
                        PAYE</div>
                </div>
                <div class="row">
                    <div class="cellInput">
                        UIF</div>
                </div>
                <div class="row">
                    <div class="cellInput">
                        Pension/Provident/RA</div>
                </div>
                <div class="row">
                    <div class="cellInput">
                        Medical Aid</div>
                </div>
                <div class="row">
                    <div class="cellInput titleText">
                        Net Income</div>
                </div>
            </asp:Panel>
            <%-- // Panel containing monthly income input boxes --%>
            <asp:Panel runat="server" ID="pnlMonthly" CssClass="cell" Style="width: 15%; text-align: left;"
                GroupingText="Monthly">
                <div class="row">
                    <div class="cellInput">
                        <SAHL:SAHLCurrencyBox ID="txtMonthBasicIncome" runat="server" TabIndex="1" TextAlign="Right"
                            Width="50" onblur="calculateTotalsMonthly()" />
                    </div>
                </div>
                <div class="row">
                    <div class="cellInput">
                        <SAHL:SAHLCurrencyBox ID="txtMonthVariableTotal" runat="server" TabIndex="-1" TextAlign="Right"
                            Width="50" ReadOnly="true" CssClass="subTotal" onblur="calculateTotalsMonthly()" />
                    </div>
                </div>
                <div class="row">
                    <div class="cellInput">
                        <SAHL:SAHLCurrencyBox ID="txtMonthCommission" runat="server" TabIndex="2" TextAlign="Right"
                            Width="50" onblur="calculateTotalsMonthly()" />
                    </div>
                </div>
                <div class="row">
                    <div class="cellInput">
                        <SAHL:SAHLCurrencyBox ID="txtMonthOvertime" runat="server" TabIndex="3" TextAlign="Right"
                            Width="50" onblur="calculateTotalsMonthly()" />
                    </div>
                </div>
                <div class="row">
                    <div class="cellInput">
                        <SAHL:SAHLCurrencyBox ID="txtMonthShift" runat="server" TabIndex="4" TextAlign="Right"
                            Width="50" onblur="calculateTotalsMonthly()" />
                    </div>
                </div>
                <div class="row">
                    <div class="cellInput">
                        <SAHL:SAHLCurrencyBox ID="txtMonthPerformance" runat="server" TabIndex="5" TextAlign="Right"
                            Width="50" onblur="calculateTotalsMonthly()" />
                    </div>
                </div>
                <div class="row">
                    <div class="cellInput">
                        <SAHL:SAHLCurrencyBox ID="txtMonthAllowances" runat="server" TabIndex="6" TextAlign="Right"
                            Width="50" onblur="calculateTotalsMonthly()" />
                    </div>
                </div>
                <div class="row">
                    <div class="cellInput">
                        <SAHL:SAHLCurrencyBox ID="txtMonthGrossIncome" runat="server" TabIndex="-1" TextAlign="Right"
                            Width="50" ReadOnly="true" CssClass="total" onblur="calculateTotalsMonthly()" />
                    </div>
                </div>
                <div class="row">
                    <div class="cellInput">
                        <SAHL:SAHLCurrencyBox ID="txtMonthDeductions" runat="server" TabIndex="-1" TextAlign="Right"
                            Width="50" ReadOnly="true" CssClass="subTotal" onblur="calculateTotalsMonthly()" />
                    </div>
                </div>
                <div class="row">
                    <div class="cellInput">
                        <SAHL:SAHLCurrencyBox ID="txtMonthPAYE" runat="server" TabIndex="7" TextAlign="Right"
                            Width="50" onblur="calculateTotalsMonthly()" />
                    </div>
                </div>
                <div class="row">
                    <div class="cellInput">
                        <SAHL:SAHLCurrencyBox ID="txtMonthUIF" runat="server" TabIndex="8" TextAlign="Right"
                            Width="50" onblur="calculateTotalsMonthly()" />
                    </div>
                </div>
                <div class="row">
                    <div class="cellInput">
                        <SAHL:SAHLCurrencyBox ID="txtMonthPension" runat="server" TabIndex="9" TextAlign="Right"
                            Width="50" onblur="calculateTotalsMonthly()" />
                    </div>
                </div>
                <div class="row">
                    <div class="cellInput">
                        <SAHL:SAHLCurrencyBox ID="txtMonthMedicalAid" runat="server" TabIndex="10" TextAlign="Right"
                            Width="50" onblur="calculateTotalsMonthly()" />
                    </div>
                </div>
                <div class="row">
                    <div class="cellInput">
                        <SAHL:SAHLCurrencyBox ID="txtMonthNetIncome" runat="server" TabIndex="-1" AllowNegative="true"
                            TextAlign="Right" Width="50" ReadOnly="true" CssClass="total" onblur="calculateTotalsMonthly()" />
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel runat="server" CssClass="cell" Style="width: 1%; text-align: left;">
            </asp:Panel>
            <%-- // Panel containing confirmed income input boxes --%>
            <SAHL:SAHLPanel runat="server" ID="pnlConfirmed" CssClass="cell" Style="width: 55%;"
                SecurityTag="EmploymentConfirmIncomeOnly" SecurityHandler="Custom" SecurityDisplayType="Disable"
                GroupingText="Confirmed">
                <table runat="server" style="width: 100%">
                    <tr runat="server">
                        <td runat="server" style="width: 25%">
                            <asp:Panel runat="server" ID="pnlConfirmedValues">
                                <div class="row">
                                    <div class="cellInput">
                                        <SAHL:SAHLCurrencyBox ID="txtConfBasicIncome" runat="server" TabIndex="11" TextAlign="Right"
                                            Width="50" onblur="calculateTotalsConfirmed()" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="cellInput">
                                        <SAHL:SAHLCurrencyBox ID="txtConfVariableTotal" runat="server" TabIndex="-1" TextAlign="Right"
                                            Width="50" ReadOnly="true" CssClass="subTotal" onblur="calculateTotalsConfirmed()" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="cellInput">
                                        <SAHL:SAHLCurrencyBox ID="txtConfCommission" runat="server" TabIndex="12" TextAlign="Right"
                                            Width="50" onblur="calculateTotalsConfirmed()" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="cellInput">
                                        <SAHL:SAHLCurrencyBox ID="txtConfOvertime" runat="server" TabIndex="13" TextAlign="Right"
                                            Width="50" onblur="calculateTotalsConfirmed()" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="cellInput">
                                        <SAHL:SAHLCurrencyBox ID="txtConfShift" runat="server" TabIndex="14" TextAlign="Right"
                                            Width="50" onblur="calculateTotalsConfirmed()" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="cellInput">
                                        <SAHL:SAHLCurrencyBox ID="txtConfPerformance" runat="server" TabIndex="15" TextAlign="Right"
                                            Width="50" onblur="calculateTotalsConfirmed()" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="cellInput">
                                        <SAHL:SAHLCurrencyBox ID="txtConfAllowances" runat="server" TabIndex="16" TextAlign="Right"
                                            Width="50" onblur="calculateTotalsConfirmed()" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="cellInput">
                                        <SAHL:SAHLCurrencyBox ID="txtConfGrossIncome" runat="server" TabIndex="-1" TextAlign="Right"
                                            Width="50" ReadOnly="true" CssClass="total" onblur="calculateTotalsConfirmed()" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="cellInput">
                                        <SAHL:SAHLCurrencyBox ID="txtConfDeductions" runat="server" TabIndex="-1" TextAlign="Right"
                                            Width="50" ReadOnly="true" CssClass="subTotal" onblur="calculateTotalsConfirmed()" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="cellInput">
                                        <SAHL:SAHLCurrencyBox ID="txtConfPAYE" runat="server" TabIndex="17" TextAlign="Right"
                                            Width="50" onblur="calculateTotalsConfirmed()" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="cellInput">
                                        <SAHL:SAHLCurrencyBox ID="txtConfUIF" runat="server" TabIndex="18" TextAlign="Right"
                                            Width="50" onblur="calculateTotalsConfirmed()" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="cellInput">
                                        <SAHL:SAHLCurrencyBox ID="txtConfPension" runat="server" TabIndex="19" TextAlign="Right"
                                            Width="50" onblur="calculateTotalsConfirmed()" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="cellInput">
                                        <SAHL:SAHLCurrencyBox ID="txtConfMedicalAid" runat="server" TabIndex="20" TextAlign="Right"
                                            Width="50" onblur="calculateTotalsConfirmed()" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="cellInput">
                                        <SAHL:SAHLCurrencyBox ID="txtConfNetIncome" runat="server" TabIndex="-1" TextAlign="Right"
                                            Width="50" ReadOnly="true" CssClass="total" onblur="calculateTotalsConfirmed()" />
                                    </div>
                                </div>
                            </asp:Panel>
                        </td>
                        <td runat="server" style="width: 70%">
                            <asp:Panel runat="server" ID="pnlConfirmedDetails">
                                <br />
                                <br />
                                <table runat="server" style="width: 100%">
                                    <tr style="height: 24px;">
                                        <td runat="server" style="width: 50%">
                                            <SAHL:SAHLLabel runat="server" Text="Confirmed by" CssClass="LabelText" Font-Bold="true"></SAHL:SAHLLabel>
                                        </td>
                                        <td runat="server" style="width: 45%">
                                            <SAHL:SAHLLabel runat="server" ID="lblConfirmedBy"></SAHL:SAHLLabel>
                                        </td>
                                        <td runat="server" style="width: 5%">
                                        </td>
                                    </tr>
                                    <tr style="height: 24px;">
                                        <td>
                                            <SAHL:SAHLLabel runat="server" Text="Confirmed date" CssClass="LabelText" Font-Bold="true"></SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                            <SAHL:SAHLLabel runat="server" ID="lblConfirmedDate"></SAHL:SAHLLabel>
                                        </td>
                                    </tr>
                                    <tr style="height: 24px;">
                                        <td>
                                            <SAHL:SAHLLabel runat="server" Text="Contact Person" CssClass="LabelText" Font-Bold="true"></SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                            <SAHL:SAHLLabel runat="server" ID="lblContactPerson"></SAHL:SAHLLabel>
                                            <SAHL:SAHLTextBox runat="server" ID="txtContactPerson"></SAHL:SAHLTextBox>
                                        </td>
                                    </tr>
                                    <tr style="height: 24px;">
                                        <td>
                                            <SAHL:SAHLLabel runat="server" Text="Phone Number" CssClass="LabelText" Font-Bold="true"></SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                            <SAHL:SAHLLabel runat="server" ID="lblPhoneNumber"></SAHL:SAHLLabel>
                                            <SAHL:SAHLPhone runat="server" ID="spPhoneNumber" />
                                        </td>
                                    </tr>
                                    <tr style="height: 24px;">
                                        <td>
                                            <SAHL:SAHLLabel runat="server" Text="Department" CssClass="LabelText" Font-Bold="true"></SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                            <SAHL:SAHLLabel runat="server" ID="lblDepartment"></SAHL:SAHLLabel>
                                            <SAHL:SAHLTextBox runat="server" ID="txtDepartment"></SAHL:SAHLTextBox>
                                        </td>
                                    </tr>
                                    <tr style="height: 24px;">
                                        <td>
                                            <SAHL:SAHLLabel ID="SAHLLabel1" runat="server" Text="Salary Payment Day" CssClass="LabelText" Font-Bold="true"></SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                            <SAHL:SAHLLabel runat="server" ID="lblSalaryPayDay"></SAHL:SAHLLabel>
                                            <SAHL:SAHLTextBox DisplayInputType="Number" runat="server" ID="txtSalaryPayDay"></SAHL:SAHLTextBox>
                                        </td>
                                    </tr>
                                    <tr style="height: 24px;">
                                        <td>
                                            <SAHL:SAHLLabel runat="server" Text="Confirmation Source" CssClass="LabelText" Font-Bold="true"></SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                            <SAHL:SAHLLabel runat="server" ID="lblConfirmationSource"></SAHL:SAHLLabel>
                                            <SAHL:SAHLDropDownList runat="server" ID="ddlConfirmationSource" PleaseSelectItem="true">
                                            </SAHL:SAHLDropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height: 24px;">
                                        <td>
                                            <SAHL:SAHLLabel ID="SAHLLabel2" runat="server" Text="Union Member" CssClass="LabelText" Font-Bold="true"></SAHL:SAHLLabel>
                                        </td>
                                        <td>
                                            <SAHL:SAHLLabel runat="server" ID="lblUnionMemberShip"></SAHL:SAHLLabel>
                                            <SAHL:SAHLDropDownList runat="server" ID="ddlUnionMembership" PleaseSelectItem="true">
                                            </SAHL:SAHLDropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                            </asp:Panel>
                            <table runat="server" style="width: 100%">
                                <tr runat="server" style="width: 100%">
                                    <td runat="server" style="width: 100%">
                                        <asp:Panel ID="chkVerificationProcessPanel" runat="server" CssClass="TitleText" GroupingText="Verification Process"
                                            Width="100%">
                                            <SAHL:SAHLCheckboxList ID="chkVerificationProcessList" runat="server" OnDataBound="chkVerificationProcessList_DataBound">
                                            </SAHL:SAHLCheckboxList>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <br />
                        </td>
                        <td runat="server" style="width: 5%">
                        </td>
                    </tr>
                </table>
            </SAHL:SAHLPanel>
        </div>
    </asp:Panel>
    <br />
    <div class="buttonBar" style="width: 99%">
        <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonSpacer"
            CausesValidation="false" />
        <SAHL:SAHLButton ID="btnBack" runat="server" Text="Back" CssClass="buttonSpacer" />
        <SAHL:SAHLButton ID="btnSave" runat="server" Text="Add" CssClass="buttonSpacer" />
    </div>
</asp:Content>
