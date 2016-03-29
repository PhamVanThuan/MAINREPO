<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LoyaltyBenefit.ascx.cs" Inherits="LoyaltyBenefit_ascx" %>
<link href="../../style.css" type="text/css" rel="stylesheet" />

<table width="100%">
    <tr>
        <td class="ManagementPanelTitle">Loyalty Benefit</td>
        <td align="right">
            &nbsp;</td>
    </tr>
</table>

<table border="0" style="text-align:center" cellpadding="0" cellspacing="0" width="395px" class="Normal">
    <tr>
        <td>
            <table cellpadding="2" cellspacing="0" width="300px">
                <tr>
                    <td class="TableRowSeperator" align="right" style="width:50%">
                        &nbsp;<b>Accumulated to Date:</b>
                    </td>
                    <td nowrap="nowrap" class="TableRowSeperator">
                        &nbsp;</td>
                    <td nowrap="nowrap" class="TableRowSeperator" align="left">
                        &nbsp;<% =sAccumulatedTotal%></td>
                </tr>
                <tr>
                    <td class="TableRowSeperator" align="right">
                        &nbsp;<b>Next Payment Date:</b>
                    </td>
                    <td nowrap="nowrap" class="TableRowSeperator">
                        &nbsp;</td>
                    <td nowrap="nowrap" class="TableRowSeperator" align="left">
                        &nbsp;<% =PaymentDate %></td>
                </tr>
            </table>
        </td>
    </tr>
</table>