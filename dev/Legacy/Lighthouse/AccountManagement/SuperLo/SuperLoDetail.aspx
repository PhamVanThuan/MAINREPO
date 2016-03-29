<%@ Page Language="C#" CodeFile="SuperLoDetail.aspx.cs" Inherits="SuperLoDetail_aspx" %>

<%@ Register TagPrefix="uc1" TagName="ManageLoan" Src="../common/UserControls/ManageLoan.ascx" %>
<%@ Register TagPrefix="uc2" TagName="PrepaymentThresholds" Src="../common/UserControls/PrepaymentThresholds.ascx" %>
<%@ Register TagPrefix="uc3" TagName="LoyaltyBenefit" Src="../common/UserControls/LoyaltyBenefit.ascx" %>
<%@ Register TagPrefix="uc4" TagName="ThresholdBreachHistory" Src="../common/UserControls/ThresholdBreachHistory.ascx" %>
<%@ Register TagPrefix="uc5" TagName="LoyaltyBenefitPaymentHistory" Src="../common/UserControls/LoyaltyBenefitPaymentHistory.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <link href="../style.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td colspan="2">
                    <table border="0" cellpadding="0" cellspacing="0" width="900">
                        <tr>
                            <td class="ManagementPanel">
                                <uc1:ManageLoan id="ucMLManageLoan" runat="server" Visible="true"/>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <table border="0" cellpadding="0" cellspacing="1" width="900">
                        <tr>
                            <td class="ManagementPanel" rowspan="2">
                                <uc2:PrepaymentThresholds id="ucPrepaymentThresholds" runat="server" visible="true" />
                            </td>
                            <td class="ManagementPanel">
                                <uc3:LoyaltyBenefit id="ucLoyaltyBenefit" runat="server" visible="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="ManagementPanel" valign="top">
                                <uc4:ThresholdBreachHistory ID="ucThresholdBreachHistory" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <table border="0" cellpadding="0" cellspacing="1" width="900">
                        <tr>
                            <td class="ManagementPanel" valign="top">
                                <uc5:LoyaltyBenefitPaymentHistory id="ucLoyaltyBenefitPaymentHistory" runat="server" visible="true" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
