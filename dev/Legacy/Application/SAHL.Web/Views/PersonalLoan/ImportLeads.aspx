<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" CodeBehind="ImportLeads.aspx.cs" Inherits="SAHL.Web.Views.PersonalLoan.ImportLeads" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <script src="../../Scripts/jquery-1.5.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function RemoveValidationSummary()
        {
            
            $(".labelError").remove();
            masterCancelBeforeUnload();
        }
    </script>
    <div runat="server" id="leadImport" visible="false">
        <table class="tableStandard" width="100%">
            <tr runat="server">
                <td class="TitleText" align="left">Personal Loan Lead Import
                </td>
            </tr>
            <tr id="FileNameRow" runat="server">
                <td class="TitleText" align="left">File Name
                </td>
                <td>
                    <asp:FileUpload ID="FileName" runat="server" Style="width: 600px;" />
                </td>
            </tr>
            <tr id="ButtonRow" runat="server">
                <td colspan="2" align="right">

                    <SAHL:SAHLButton ID="btnImport" runat="server" Text="Import" AccessKey="I" />
                </td>
            </tr>
        </table>
    </div>
    <div runat="server" id="LeadSummary" visible="false">
        <table width="100%">
            <tr>
                <td>
                    <SAHL:DXGridView ID="LeadSummaryGrid" runat="server" AutoGenerateColumns="False" Width="100%"
                        ClientInstanceName="gridLeadSummary" Settings-ShowTitlePanel="true" SettingsText-Title="Lead Import Results" EnableViewState="false">
                    </SAHL:DXGridView>
                </td>
                <td></td>
            </tr>
            <tr>
            </tr>
            <tr>
                <td align="center">
                    <SAHL:SAHLButton ID="btnRefresh" runat="server" Text="Refresh" AccessKey="R" OnClick="SAHLRefresh_Click" />
                    <SAHL:SAHLButton runat="server" ID="btnDownload" OnClick="OnDownload_Click" OnClientClick="RemoveValidationSummary()" Text="Download" />
                    </td>
                <td></td>
            </tr>
            <tr></tr>
            <tr></tr>
            <tr id="leadValidation" class="labelError" >
                <td align="center">
                    <SAHL:SAHLLabel runat="server" ID="lblError" ForeColor="Red"></SAHL:SAHLLabel>
                 </td>
            </tr>
        </table>

        <ul class="legendText">
            <li>Complete - Successfully created lead.</li>
            <li>Pending - Waiting to create lead.</li>
            <li>Failed - Unable to create lead “after 3 attempts” due to a system failure.</li>
            <li>Unsuccessful - Unable to create lead due to a business rule validation failing.</li>
        </ul>

    </div>
</asp:Content>