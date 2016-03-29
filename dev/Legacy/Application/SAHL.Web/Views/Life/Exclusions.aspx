<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true"
    Inherits="SAHL.Web.Views.Life.Exclusions" Title="Exclusions" Codebehind="Exclusions.aspx.cs" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>

<%@ Register Src="WorkFlowHeader.ascx" TagName="WorkFlowHeader" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <script src="../../Scripts/jquery-1.5.min.js" type="text/javascript"></script>
    <uc1:WorkFlowHeader ID="WorkFlowHeader1" runat="server" />
    <script type="text/javascript" language="javascript">
        var query = ".1_Exclusion2-InstalmentProtectionBenefit,.1_Exclusion7-PsychologicalDisorders,.1_Exclusion8-BackConditions";
        $(document).ready(function () {
            if ($(query).length > 0) {
                $("#exclusionSets").hide();
                $("input:checked").click();
            } else {
                $("#exclusionRadio").hide();
                $("#DeathOnly").attr("checked", "checked");
            }
        });

        function Death() {
            $("#exclusionSets").show();
            $(query).hide();
            $("tr:hidden input:checked").removeAttr('checked'); ;
            allChecked();
        }
        
        function DeathIPB() {
            $("#exclusionSets").show();
            $(query).show();
            allChecked();
        }
    </script>
    <div style="text-align: center">
        <div style="position:relative">
        <div style="width:1;text-align:left;position:absolute;top:0;left:0;" id="exclusionRadio">
            <input type="radio" id="DeathOnly" value="DeathOnly" name="AssuredQualification" onclick="Death()" runat="server" /><label class="error" for="DeathOnly">Assured Qualifies for Death benefit only</label><br />
            <input type="radio" id="DeathAndIPB" value="DeathAndIPB" name="AssuredQualification" onclick="DeathIPB()" runat="server"/><label class="error" for="DeathAndIPB">Assured Qualifies for Death and IPB benefit</label>
        </div>
        <SAHL:SAHLLabel ID="lblPageHeader" runat="server" Text="EXCLUSIONS" Font-Bold="True" Font-Names="Arial" Font-Size="Medium" Font-Underline="True" CssClass="LabelText"></SAHL:SAHLLabel>
        </div>
        <br />
        <table width="100%"  class="tableStandard">
            <tr>
                <td align="center" style="height:20px;vertical-align:bottom">
                    The following exclusions apply :
                </td>
            </tr>
            <tr>
                <td align="center" id="exclusionSets">
                    <div style="height: 440px; overflow: auto">
                        <asp:Table ID="tblText" runat="server" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px">
                        </asp:Table>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <SAHL:SAHLLabel ID="lblFinal" runat="server" Text="It is necessary for this action to be taken in order to maintain equity between risk and premium paid.">
                    </SAHL:SAHLLabel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="center">
                    <SAHL:SAHLButton ID="btnNext" runat="server" Text="Next" OnClick="btnNext_Click" SecurityTag="LifeUpdateAccessWorkflow"/>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="HiddenField1" runat="server" Value="false" />
    </div>
</asp:Content>
