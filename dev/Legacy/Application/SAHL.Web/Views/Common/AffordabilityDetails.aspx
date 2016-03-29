<%@ Page Language="C#"
    MasterPageFile="~/MasterPages/Blank.master"
    AutoEventWireup="true" CodeBehind="AffordabilityDetails.aspx.cs"
    Inherits="SAHL.Web.Views.Common.AffordabilityDetails" Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <script src="../../Scripts/jquery-1.5.min.js"></script>
    <script type="text/javascript">

        function GetCurrencyValue(element) {
            var rands = $("#" + $(element).attr('id') + "_txtRands");
            var cents = $("#" + $(element).attr('id') + "_txtCents");
            if (rands.val() == 0 && cents.val() == 0)
                return '';
            else
                return rands.val() + '.' + cents.val();
        }

        function calculateTotals(lblTotal, arrElements, add) {
            var total = 0;

            for (var i = 0; i < arrElements.length; i++) {
                var val = GetCurrencyValue(arrElements[i]);
                if (isNaN(val) || isNaN(parseFloat(val)))
                    continue;
                if (i == 0 || add)
                    total += parseFloat(val);
                else
                    total -= parseFloat(val);
            }

            // format the number
            total = formatNumber(total, 2, ',', '.');
            $(lblTotal).text(total);
        }

        function calculateAffordability() {

            var income = $('span[id$=lblTotalIncome]').text().replace(/\,/g, "");
            var expenses = $('span[id$=lblTotalExpenses]').text().replace(/\,/g, "");
            var total = parseFloat(income) - parseFloat(expenses);

            // format the number
            total = formatNumber(total, 2, ',', '.');
            $('span[id$=lblAfordability]').text(total);
        }

        function SetTotals() {
            $('span[id$=lblTotalIncome]').text($('#Income').find('span[id$=lblTotal]').text());

            var monthlyExpense = $('#MonthlyExpense').find('span[id$=lblTotal]').text().replace(/\,/g, "");
            var debtRepayment = $('#DebtRepayment').find('span[id$=lblTotal]').text().replace(/\,/g, "");

            if (debtRepayment.length == 0) {
                debtRepayment = 0;
            }
            if (monthlyExpense.length == 0) {
                monthlyExpense = 0;
            }
            $('span[id$=lblTotalExpenses]').text(formatNumber(parseFloat(monthlyExpense) + parseFloat(debtRepayment), 2, ',', '.'));
        }

        function OnIncomeFieldChanged(sender) {

            var elements = $(sender.parentNode.parentNode.parentNode).find('input[id$=txtAmount]');

            calculateTotals($(sender.parentNode.parentNode.parentNode).find('span[id$=lblTotal]'), elements, true);
            SetTotals();
            calculateAffordability();
        }

        $(function () {
            init();
        })

        function init() {
            $('span[id$=lblTotal]').each(
                function () {
                    OnIncomeFieldChanged(this);
                });
        }

    </script>

    <div style="display: inline-block; width: 50%; float: left">
        <div id="Income" style="width: 100%">
            <asp:Panel runat="server" GroupingText="INCOME" CssClass="affordability">
                <asp:ListView ID="IncomeListView" DataKeyNames="Key" runat="server" Style="vertical-align: top">
                    <LayoutTemplate>
                        <table style="width: 100%">
                            <td runat="server" id="itemPlaceholder"></td>
                            <tr>
                                <td>Total:</td>
                                <td>
                                    <SAHL:SAHLLabel runat="server" ID="lblTotal" CssClass="income" Width="100px" Text="0.00" TextAlign="Right"/>
                                </td>
                                <td>&nbsp</td>
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr runat="server">
                            <td style="width:100px;">
                                <asp:Label ID="Label3" runat="server" Text='<%# ((SAHL.Web.Views.Common.Models.Affordability.AffordabilityModel)Container.DataItem).Name %>'/>
                            </td>
                            <td style="width:100px" align="Right">
                                <asp:HiddenField runat="server" ID="Key" Value='<%# ((SAHL.Web.Views.Common.Models.Affordability.AffordabilityModel)Container.DataItem).Key %>'/><SAHL:SAHLCurrencyBox ID="txtAmount" Amount='<%#  ((SAHL.Web.Views.Common.Models.Affordability.AffordabilityModel)Container.DataItem).Amount %>' runat="server" OnBlur="OnIncomeFieldChanged(this)" Width="50px" DisplayInputType="Normal" Height="20px" Wrap="False" TextAlign="Right">0..00</SAHL:SAHLCurrencyBox>
                            </td>
                            <td style="width:100px">
                                <SAHL:SAHLTextBox ID="txtDescription" runat="server" Text="<%# ((SAHL.Web.Views.Common.Models.Affordability.AffordabilityModel)Container.DataItem).Description %>" Visible="<%#  ((SAHL.Web.Views.Common.Models.Affordability.AffordabilityModel)Container.DataItem).DescriptionRequired %>" Enabled="<%# ((SAHL.Web.Views.Common.Models.Affordability.AffordabilityModel)Container.DataItem).DescriptionRequired %>" CssClass="marginLeft"></SAHL:SAHLTextBox>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </asp:Panel>
        </div>
        <div id="MonthlyExpense">
            <asp:Panel ID="Panel1" runat="server" GroupingText="MONTHLY EXPENSE" CssClass="affordability">
                <asp:ListView ID="MonthlyExpenseListView" DataKeyNames="Key" runat="server" Style="vertical-align: top">
                    <LayoutTemplate>
                        <table style="width: 100%">
                            <td runat="server" id="itemPlaceholder"></td>
                            <tr>
                                <td>Total:</td>
                                <td>
                                    <SAHL:SAHLLabel runat="server" ID="lblTotal" Width="100px" Text="0.00" TextAlign="right" />
                                </td>
                                <td>&nbsp</td>
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr id="Tr1" runat="server">
                            <td style="width: 100px;">
                                <asp:Label ID="Label1" runat="server" Text='<%# ((SAHL.Web.Views.Common.Models.Affordability.AffordabilityModel)Container.DataItem).Name %>' />
                            </td>
                            <td style="width: 100px" align="Right">
                                <asp:HiddenField runat="server" ID="Key" Value='<%# ((SAHL.Web.Views.Common.Models.Affordability.AffordabilityModel)Container.DataItem).Key %>' />
                                <SAHL:SAHLCurrencyBox ID="txtAmount" Amount='<%#  ((SAHL.Web.Views.Common.Models.Affordability.AffordabilityModel)Container.DataItem).Amount %>' runat="server" OnBlur="OnIncomeFieldChanged(this)" Width="50px" DisplayInputType="Normal" Height="20px" Wrap="False" TextAlign="Right">0..00</SAHL:SAHLCurrencyBox>
                            </td>
                            <td style="width: 100px">
                                <SAHL:SAHLTextBox ID="txtDescription" runat="server" Text="<%# ((SAHL.Web.Views.Common.Models.Affordability.AffordabilityModel)Container.DataItem).Description %>" Visible="<%#  ((SAHL.Web.Views.Common.Models.Affordability.AffordabilityModel)Container.DataItem).DescriptionRequired %>" Enabled="<%#  ((SAHL.Web.Views.Common.Models.Affordability.AffordabilityModel)Container.DataItem).DescriptionRequired %>" CssClass="marginLeft"></SAHL:SAHLTextBox>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </asp:Panel>
        </div>
    </div>
    <div style="display: inline-block; width: 50%; float: left">
        <div id="DebtRepayment">
            <asp:Panel ID="Panel2" runat="server" GroupingText="DEBT REPAYMENT" CssClass="affordability">
                <asp:ListView ID="DebtRepaymentListView" DataKeyNames="Key" runat="server" Style="vertical-align: top">
                    <LayoutTemplate>
                        <table style="width: 100%">
                            <td runat="server" id="itemPlaceholder" />
                            <tr>
                                <td>
                                    Total:</td>
                                <td>
                                    <SAHL:SAHLLabel runat="server" ID="lblTotal" Width="100px" Text="0.00" TextAlign="right" />
                                </td>
                                <td>&nbsp</td>
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr id="Tr2" runat="server">
                            <td style="width: 100px;">
                                <asp:Label ID="Label2" runat="server" Text='<%# ((SAHL.Web.Views.Common.Models.Affordability.AffordabilityModel)Container.DataItem).Name %>' />
                            </td>
                            <td style="width: 100px;" align="Right">
                                <asp:HiddenField runat="server" ID="Key" Value='<%# ((SAHL.Web.Views.Common.Models.Affordability.AffordabilityModel)Container.DataItem).Key %>' />
                                <SAHL:SAHLCurrencyBox ID="txtAmount" Amount='<%#  ((SAHL.Web.Views.Common.Models.Affordability.AffordabilityModel)Container.DataItem).Amount %>' runat="server" OnBlur="OnIncomeFieldChanged(this)" Width="50px" DisplayInputType="Normal" Height="20px" Wrap="False" TextAlign="Right">0..00</SAHL:SAHLCurrencyBox>
                            </td>
                            <td style="width: 100px;">
                                <SAHL:SAHLTextBox ID="txtDescription" runat="server" Text="<%# ((SAHL.Web.Views.Common.Models.Affordability.AffordabilityModel)Container.DataItem).Description %>" Visible="<%#  ((SAHL.Web.Views.Common.Models.Affordability.AffordabilityModel)Container.DataItem).DescriptionRequired %>" Enabled="<%#  ((SAHL.Web.Views.Common.Models.Affordability.AffordabilityModel)Container.DataItem).DescriptionRequired %>" CssClass="marginLeft"></SAHL:SAHLTextBox>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </asp:Panel>
        </div>
        <div>
            <asp:Panel runat="server" GroupingText="SUMMARY" CssClass="affordability">
                <table>
                    <tr>
                        <td>
                            <SAHL:SAHLLabel ID="SAHLLabel1" runat="server" CssClass="LabelText" TextAlign="left">Dependants in Household :</SAHL:SAHLLabel>&nbsp;               
                        </td>
                        <td class="alignright">
                            <SAHL:SAHLTextBox ID="txtDependantsInHousehold" runat="server" DisplayInputType="Number" Width="50px" MaxLength="2" CssClass="alignright">0</SAHL:SAHLTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <SAHL:SAHLLabel ID="SAHLLabel2" runat="server" CssClass="LabelText" TextAlign="left">Contributing Dependants   :</SAHL:SAHLLabel>&nbsp;

                        </td>
                        <td class="alignright">
                            <SAHL:SAHLTextBox ID="txtContributingDependants" runat="server" DisplayInputType="Number" Width="50px" MaxLength="2" CssClass="alignright">0</SAHL:SAHLTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <SAHL:SAHLLabel ID="SAHLLabel3" runat="server" CssClass="LabelText" TextAlign="left" Height="20px">Total Income:</SAHL:SAHLLabel></td>
                        <td class="alignright">
                            <SAHL:SAHLLabel runat="server" ID="lblTotalIncome" CssClass="LabelText alignright" Width="100px" Height="20px" Text="0.00" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <SAHL:SAHLLabel ID="SAHLLabel4" runat="server" CssClass="LabelText" TextAlign="left" Height="20px">Total Expenses:</SAHL:SAHLLabel></td>
                        <td class="alignright">
                            <SAHL:SAHLLabel runat="server" ID="lblTotalExpenses" CssClass="LabelText alignright" Width="100px" Height="20px" Text="0.00" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <SAHL:SAHLLabel ID="SAHLLabel5" runat="server" CssClass="LabelText" TextAlign="left" Height="20px">Affordability:</SAHL:SAHLLabel></td>
                        <td class="alignright">
                            <SAHL:SAHLLabel runat="server" ID="lblAfordability" CssClass="LabelText alignright" Width="100px" Height="20px" Text="0.00" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
    </div>

    <div style="clear: both" />
    <div style="float: right">
        <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
        <SAHL:SAHLButton ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSaveUpdate_Click" />
    </div>
</asp:Content>
