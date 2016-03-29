<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PrepaymentThresholds.ascx.cs" Inherits="PrepaymentThresholds_ascx" %>
<link href="../../style.css" type="text/css" rel="stylesheet" />
<table width="100%">
    <tr>
        <td class="ManagementPanelTitle">Prepayment Thresholds</td>
        <td align="right">&nbsp;</td>
    </tr>
</table>

<table border="0" style="text-align:center" cellpadding="0" cellspacing="0" width="495px" class="Normal">
    <tr>
        <td>
            <table width="400px" cellpadding="2" cellspacing="0">
                <tr>
                    <td nowrap="nowrap" align="right" class="TableRowSeperator">
                        &nbsp;</td>
                    <td class="TableRowSeperator">
                        &nbsp;<b>Annual</b>
                    </td>
                    <td nowrap="nowrap" class="TableRowSeperator">
                        &nbsp;
                    </td>
                    <td nowrap="nowrap" class="TableRowSeperator">
                        <b>Cumulative</b></td>
                </tr>
                <tr>
                    <td nowrap="nowrap" align="right" class="TableRowSeperator">
                        &nbsp;<b>Year 1:</b></td>
                    <td class="TableRowSeperator">
                        &nbsp;<% =PPThresholdYr1 %>
                    </td>
                    <td nowrap="nowrap" class="TableRowSeperator">
                        &nbsp;
                    </td>
                    <td nowrap="nowrap" class="TableRowSeperator">
                        &nbsp;<% =PPThresholdYr1 %></td>
                </tr>
                <tr>
                    <td nowrap="nowrap" align="right" class="TableRowSeperator">
                        &nbsp;<b>Year 2:</b></td>
                    <td class="TableRowSeperator">
                        &nbsp;<% =PPThresholdYr2 %>
                    </td>
                    <td nowrap="nowrap" class="TableRowSeperator">
                        &nbsp;
                    </td>
                    <td nowrap="nowrap" class="TableRowSeperator">
                        &nbsp;<% =Cumulative2 %></td>
                </tr>
                <tr>
                    <td nowrap="nowrap" align="right" class="TableRowSeperator">
                        &nbsp;<b>Year 3:</b></td>
                    <td class="TableRowSeperator">
                        &nbsp;<% =PPThresholdYr3 %>
                    </td>
                    <td nowrap="nowrap" class="TableRowSeperator">
                        &nbsp;
                    </td>
                    <td nowrap="nowrap" class="TableRowSeperator">
                        &nbsp;<% =Cumulative3 %></td>
                </tr>
                <tr>
                    <td nowrap="nowrap" align="right" class="TableRowSeperator">
                        &nbsp;<b>Year 4:</b></td>
                    <td class="TableRowSeperator">
                        &nbsp;<% =PPThresholdYr4 %>
                    </td>
                    <td nowrap="nowrap" class="TableRowSeperator">
                        &nbsp;
                    </td>
                    <td nowrap="nowrap" class="TableRowSeperator">
                        &nbsp;<% =Cumulative4 %></td>
                </tr>
                <tr>
                    <td nowrap="nowrap" align="right" class="TableRowSeperator">
                        &nbsp;<b>Year 5:</b></td>
                    <td class="TableRowSeperator">
                        &nbsp;<% =PPThresholdYr5 %>
                    </td>
                    <td nowrap="nowrap" class="TableRowSeperator">
                        &nbsp;
                    </td>
                    <td nowrap="nowrap" class="TableRowSeperator">
                        &nbsp;<% =Cumulative5 %></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
