<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="RulesAdministration.aspx.cs" Inherits="SAHL.Web.Views.RulesAdministration" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"
    TagPrefix="SAHL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <script language="javascript">
//    function myTest() 
//    {
//        var elements = document.getElementsByTagName('INPUT');
//        var str = '';
//        for (var i=0; i<elements.length; i++) 
//        {
//            try {
//                str = str + elements[i].name + '....' + elements[i].value + '\n';
//            }
//            catch (exception) {
//            }
//        }
//        alert(str);
//    }
//    
//    window.attachEvent("onbeforeunload", myTest);
    </script>    
    <SAHL:SAHLGridView id="gridRules" runat="server" Caption="Available Rules" Width="770" Height="82px" PostBackType="SingleClick" OnSelectedIndexChanged="gridRules_SelectedIndexChanged">
    </SAHL:SAHLGridView>
    <br />
    <br />
    <asp:Panel ID="panelAddUpdate" runat="server" Height="69px" Width="771px">
        <table style="width: 766px; height: 83px">
            <tr>
                <td style="width: 91px">
                    <SAHL:SAHLLabel ID="lblName" runat="server" CssClass="LabelText" TextAlign="left" Width="77px">Rule Name:</SAHL:SAHLLabel></td>
                <td style="width: 561px" align="left">
                    <SAHL:SAHLTextBox ID="txtName" runat="server" Width="544px"></SAHL:SAHLTextBox></td>
                <td>
                </td>
            </tr>
            <tr>
                <td style="width: 91px; height: 27px">
                    <SAHL:SAHLLabel ID="lblDesc" runat="server" CssClass="LabelText" TextAlign="left">Description:</SAHL:SAHLLabel></td>
                <td style="height: 27px; width: 561px;" align="left">
                    <SAHL:SAHLTextBox ID="txtDescription" runat="server" Width="544px"></SAHL:SAHLTextBox></td>
                <td style="height: 27px">
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="txtOriginalName" runat="server" Height="214px" Width="770px" GroupingText="Parameters">
        <input id="OriginalName" type="hidden" style="width: 251px" runat="server" />
        <input id="OriginalType" type="hidden" style="width: 210px" runat="server" />
        <input id="OriginalValue" type="hidden" style="width: 268px" runat="server" /><br />
        
            
        <div style="width: 404px; height: 118px; float: right;" id="divParameters">
            <table style="width: 400px; height: 147px">
                <tr>
                    <td style="width: 352px">
                        &nbsp;<SAHL:SAHLLabel ID="lblParamName" runat="server" CssClass="LabelText" TextAlign="left"
                            Width="57px">Name:</SAHL:SAHLLabel>
                        <SAHL:SAHLTextBox ID="txtParamDesc" runat="server" Width="251px"></SAHL:SAHLTextBox></td>
                    <td style="width: 64px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 352px">
                        <SAHL:SAHLLabel ID="lblParamType" runat="server" CssClass="LabelText" TextAlign="left"
                            Width="62px">Type:</SAHL:SAHLLabel>
                        <SAHL:SAHLDropDownList ID="cbxParameterType" runat="server" Width="257px">
                        </SAHL:SAHLDropDownList></td>
                    <td style="width: 64px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 352px">
                        <SAHL:SAHLLabel ID="SAHLLabel1" runat="server" CssClass="LabelText" TextAlign="left"
                            Width="62px">Value:</SAHL:SAHLLabel>
                        <SAHL:SAHLTextBox ID="txtValue" runat="server" Width="251px"></SAHL:SAHLTextBox></td>
                    <td style="width: 64px">
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 352px">
                        <SAHL:SAHLButton ID="btnDelete" runat="server" ButtonSize="Size2"
                            Text="Delete" OnClick="btnDelete_Click" />
                        <SAHL:SAHLButton ID="btnUpdate" runat="server" Text="Update" ButtonSize="Size2" CausesValidation="False" OnClick="btnUpdate_Click1" UseSubmitBehavior="False" /></td>
                    <td style="width: 64px">
                        <SAHL:SAHLButton ID="btnNew" runat="server" ButtonSize="Size2" Text="New" Width="60px" OnClientClick="NewParameter();return false" CausesValidation="False" /></td>
                </tr>
            </table>
        </div>
        <div style="width: 348px; height: 163px">
            &nbsp;<asp:ListBox ID="lstParameters"  runat="server" Height="157px" Width="331px"></asp:ListBox></div>
    </asp:Panel>
    <table style="width: 770px">
        <tr>
            <td>
                <SAHL:SAHLLabel ID="lblError" runat="server" CssClass="LabelText" ForeColor="Red"
                    TextAlign="left"></SAHL:SAHLLabel></td>
            <td style="width: 54px">
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td style="width: 54px">
                    <SAHL:SAHLButton ID="btnSave" runat="server" OnClick="btnSave_Click"
                        Text="Save" Width="80px" /></td>
        </tr>
    </table>
    <div>
        &nbsp;</div>
</asp:Content>
