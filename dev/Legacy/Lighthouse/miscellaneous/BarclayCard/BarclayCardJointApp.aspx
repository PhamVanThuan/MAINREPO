<%@ Register TagPrefix="Util" TagName="title" Src="../common/title.ascx" %>

<%@ Page Language="C#" CodeFile="BarclayCardJointApp.aspx.cs" Inherits="BarclayCardJointApp_aspx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>SAHL Gold Card Application - Joint Bond Holders</title>
    <link href="../style.css" rel="stylesheet" type="text/css" />
    <link href="../style.css" rel="stylesheet" type="text/css" />
    <link href="../style.css" rel="stylesheet" type="text/css" />
    <link href="../style.css" rel="stylesheet" type="text/css" />
    <link href="../style.css" rel="stylesheet" type="text/css" />
    <link href="../style.css" rel="stylesheet" type="text/css" />
    <link href="../style.css" rel="stylesheet" type="text/css" />
    <link href="../style.css" rel="stylesheet" type="text/css" />
    <link href="../style.css" rel="stylesheet" type="text/css" />
</head>

<script type="text/javascript" language="javascript">

    // function used to add a Javascript function on page load 
    function addLoadEvent(func) 
    {
      var oldonload = window.onload;
      if (typeof window.onload != 'function') 
      {
        window.onload = func;
      }
      else 
      {
        window.onload = function() {oldonload();func();}
      }
    }
    

    // Add this function to show/hide the Detail table
    addLoadEvent(function() 
    {
            if (window.document.all["chkDisplayDetailsTable"].checked)
                window.document.all["tDetails"].style.display='';
            else
                window.document.all["tDetails"].style.display='none'; 
    }
    )
    function minmax(table,button,button2) 
    {
        if (table.style.display == 'none' ) 
        {
            table.style.display = "";
            button.value  = "Hide Limit Matrix";
            button2.value  = "Hide Limit Matrix";
        }    
        else 
        {
            table.style.display = 'none';
            button.value = "View Limit Matrix";
            button2.value = "View Limit Matrix";
        }
    }
</script>

<body style="text-align: left">
    <form id="Form1" runat="server">
        <table width="1000">
            <tr>
                <td width="1000" style="text-align: center">
                    &nbsp;<asp:HyperLink ID="HyperLink1" runat="server">Back</asp:HyperLink></td>
            </tr>
        </table>
        <table id="tDetails" class="SAHLTable" width="1100">
            <tr>
                <td colspan="2" style="text-align: center">
                    <asp:Label ID="lblProspLoanNum" runat="server"></asp:Label>
                    <asp:Label ID="lblProspLoanNumVal" runat="server" Font-Bold="True"></asp:Label>
                    <asp:Label ID="lblLoanUnregistered" runat="server" Text="(Not Registered)" Visible="False"></asp:Label></td>
            </tr>
            <tr>
                <td style="text-align: center; width: 537px;">
                    <asp:Label ID="lblPrincipal" runat="server" Text="Principal Bond Holder"></asp:Label></td>
                <td style="text-align: center">
                    <asp:Label ID="lblSecondary" runat="server" Text="Secondary Bond Holder"></asp:Label></td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center;">
                    <asp:Button ID="btnEdit" runat="server" Text="Edit" Width="71px" OnClick="btnEdit_Click"
                        CausesValidation="False" /><asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click"
                            Text="Save" Width="71px" />
                    <br />
                    <asp:Label ID="lblEditMessage" runat="server" ForeColor="Red" Text="Application cannot be Edited as it has already been submitted to the Bank"
                        Visible="False" Font-Bold="True"></asp:Label>
                    <br />
                    <asp:Button ID="btnReSubmit" runat="server" Text="Re-Submit" OnClientClick="return confirm('Are you sure you want to Re-Submit this application?\r\n\r\nSelecting OK will allow the application to be edited before being Re-Submitted.')" /></td>
            </tr>
            <tr>
                <td style="width: 537px">
                    <asp:Panel ID="pnlDetails1" runat="server" Width="100%">
                        <table>
                            <tr>
                                <td style="width: 190px; text-align: right">
                                </td>
                                <td style="width: 310px; text-align: left">
                                    <asp:CheckBox ID="chkNotInterested" runat="server" Text="Not Interested in Gold Card"
                                        AutoPostBack="True" OnCheckedChanged="chkNotInterested_CheckedChanged" /></td>
                            </tr>
                            <tr>
                                <td style="width: 190px; text-align: right;">
                                    Salutation</td>
                                <td style="width: 310px; text-align: left;">
                                    <asp:TextBox ID="txtSalutation" runat="server" Width="133px"></asp:TextBox><asp:RequiredFieldValidator
                                        ID="oRVSalutation" runat="server" ControlToValidate="txtSalutation" Display="Dynamic"
                                        ErrorMessage="Salutation must be entered"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td style="width: 190px; text-align: right;">
                                    First Names</td>
                                <td style="width: 310px; text-align: left;">
                                    <asp:TextBox ID="txtFirstName" runat="server" Width="133px" EnableTheming="True"></asp:TextBox><asp:RequiredFieldValidator
                                        ID="oRVFirstNames" runat="server" ControlToValidate="txtFirstName" Display="Dynamic"
                                        ErrorMessage="First Names must be entered"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td style="width: 190px; text-align: right;">
                                    Surname</td>
                                <td style="width: 310px; text-align: left;">
                                    <asp:TextBox ID="txtSurname" runat="server" Width="133px"></asp:TextBox><asp:RequiredFieldValidator
                                        ID="oRVSurname" runat="server" ControlToValidate="txtSurname" Display="Dynamic"
                                        ErrorMessage="Surname must be entered"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td style="width: 190px; text-align: right;">
                                    ID Number</td>
                                <td style="width: 310px; text-align: left;">
                                    <asp:TextBox ID="txtIDNumber" runat="server" Width="133px"></asp:TextBox><asp:RequiredFieldValidator
                                        ID="oRVIDNumber" runat="server" ControlToValidate="txtIDNumber" Display="Dynamic"
                                        ErrorMessage="ID must be entered"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td style="width: 190px; text-align: right;">
                                    Salary</td>
                                <td style="width: 310px; text-align: left;">
                                    <asp:TextBox ID="txtSalary" runat="server" Width="133px"></asp:TextBox><asp:CompareValidator
                                        ID="CompareValidator1" runat="server" ControlToValidate="txtSalary" ErrorMessage="Numeric values required"
                                        Operator="DataTypeCheck" SetFocusOnError="True" Type="Currency"></asp:CompareValidator></td>
                            </tr>
                            <tr>
                                <td style="width: 190px; text-align: right;" valign="top">
                                    Card Type</td>
                                <td style="width: 310px; text-align: left;">
                                    <asp:DropDownList ID="ddCardType" runat="server" Width="109px" DataTextField="DDValue"
                                        DataValueField="DDOptionsID" DataSourceID="oSqlDSCardType" OnSelectedIndexChanged="ddCardType_SelectedIndexChanged">
                                    </asp:DropDownList><asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="ddCardType"
                                        Display="Dynamic" Enabled="False" ErrorMessage="Card Type must be selected"
                                        MaximumValue="99" MinimumValue="1"></asp:RangeValidator><asp:CustomValidator ID="oCVCardType"
                                            runat="server" ControlToValidate="ddCardType" ErrorMessage="Secondary Card Holder already assigned"
                                            ClientValidationFunction="PrincipalBondHolderCardTypeValidate"></asp:CustomValidator></td>
                            </tr>
                            <tr>
                                <td style="width: 190px; text-align: right;">
                                    Qualified Credit Limit</td>
                                <td style="width: 310px; text-align: left;">
                                    <asp:TextBox ID="txtCreditLimit" runat="server" Width="103px"></asp:TextBox><asp:RegularExpressionValidator
                                        ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCreditLimit"
                                        ErrorMessage="Must enter a Credit Limit > 0" ValidationExpression="^[1-9][0-9]*$"></asp:RegularExpressionValidator></td>
                            </tr>
                            <tr>
                                <td style="width: 190px; text-align: right;">
                                    <asp:Label ID="lblOverrideReason" runat="server" Text="Override Reason"></asp:Label></td>
                                <td style="width: 310px; text-align: left;">
                                    <asp:TextBox ID="txtOverrideReason" runat="server"
                                            TextMode="MultiLine" Width="269px"></asp:TextBox></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
                <td>
                    <asp:Panel ID="pnlDetails2" runat="server" Width="100%">
                        <table>
                            <tr>
                                <td style="width: 190px; text-align: right; height: 22px;">
                                </td>
                                <td style="width: 310px; text-align: left; height: 22px;">
                                    <asp:CheckBox ID="chkNotInterested2" runat="server" Text="Not Interested in Gold Card"
                                        AutoPostBack="True" OnCheckedChanged="chkNotInterested2_CheckedChanged" /></td>
                            </tr>
                            <tr>
                                <td style="width: 190px; text-align: right;">
                                    Salutation</td>
                                <td style="width: 310px; text-align: left;">
                                    <asp:TextBox ID="txtSalutation2" runat="server" Width="133px"></asp:TextBox><asp:RequiredFieldValidator
                                        ID="oRVSalutation2" runat="server" ControlToValidate="txtSalutation2" Display="Dynamic"
                                        ErrorMessage="Salutation must be entered"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td style="width: 190px; text-align: right;">
                                    First Names</td>
                                <td style="width: 310px; text-align: left;">
                                    <asp:TextBox ID="txtFirstName2" runat="server" Width="133px"></asp:TextBox><asp:RequiredFieldValidator
                                        ID="oRVFirstNames2" runat="server" ControlToValidate="txtFirstName2" Display="Dynamic"
                                        ErrorMessage="First Names must be entered"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td style="width: 190px; text-align: right;">
                                    Surname</td>
                                <td style="width: 310px; text-align: left;">
                                    <asp:TextBox ID="txtSurname2" runat="server" Width="133px"></asp:TextBox><asp:RequiredFieldValidator
                                        ID="oRVSurname2" runat="server" ControlToValidate="txtSurname2" Display="Dynamic"
                                        ErrorMessage="Surname must be entered"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td style="width: 190px; text-align: right;">
                                    ID Number</td>
                                <td style="width: 310px; text-align: left;">
                                    <asp:TextBox ID="txtIDNumber2" runat="server" Width="133px"></asp:TextBox><asp:RequiredFieldValidator
                                        ID="oRVIDNumber2" runat="server" ControlToValidate="txtIDNumber2" Display="Dynamic"
                                        ErrorMessage="ID must be entered"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td style="width: 190px; text-align: right;">
                                    Salary</td>
                                <td style="width: 310px; text-align: left;">
                                    <asp:TextBox ID="txtSalary2" runat="server" Width="133px"></asp:TextBox><asp:CompareValidator
                                        ID="CompareValidator2" runat="server" ControlToValidate="txtSalary2" ErrorMessage="Numeric values required"
                                        Operator="DataTypeCheck" SetFocusOnError="True" Type="Currency"></asp:CompareValidator></td>
                            </tr>
                            <tr>
                                <td style="width: 190px; text-align: right;" valign="top">
                                    Card Type</td>
                                <td style="width: 310px; text-align: left;">
                                    <asp:DropDownList ID="ddCardType2" runat="server" Width="109px" DataTextField="DDValue"
                                        DataValueField="DDOptionsID" DataSourceID="oSqlDSCardType" OnSelectedIndexChanged="ddCardType2_SelectedIndexChanged">
                                    </asp:DropDownList><asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="ddCardType2"
                                        Display="Dynamic" Enabled="False" ErrorMessage="Card Type must be selected"
                                        MaximumValue="99" MinimumValue="1"></asp:RangeValidator><asp:CustomValidator ID="oCVCardType2"
                                            runat="server" ControlToValidate="ddCardType2" ErrorMessage="Secondary Card Holder already assigned"
                                            ClientValidationFunction="SecondaryBondHolderCardTypeValidate"></asp:CustomValidator></td>
                            </tr>
                            <tr>
                                <td style="width: 190px; text-align: right;">
                                    Qualified Credit Limit</td>
                                <td style="width: 310px; text-align: left;">
                                    <asp:TextBox ID="txtCreditLimit2" runat="server" Width="103px"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtCreditLimit2"
                                        ErrorMessage="Must enter a Credit Limit > 0" ValidationExpression="^[1-9][0-9]*$"></asp:RegularExpressionValidator></td>
                            </tr>
                            <tr>
                                <td style="width: 190px; text-align: right;">
                                    <asp:Label ID="lblOverrideReason2" runat="server" Text="Override Reason"></asp:Label></td>
                                <td style="width: 310px; text-align: left;">
                                    <asp:TextBox ID="txtOverrideReason2"
                                            runat="server" TextMode="MultiLine" Width="269px"></asp:TextBox></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="pnlDetails1b" runat="server" Width="100%">
                        <table>
                            <tr>
                                <td colspan="2" style="width: 190px; text-align: center">
                                    <asp:Label ID="lblSecondaryMsg" runat="server" Font-Bold="True" ForeColor="Red" Text="All Details below as per Primary Card Holder"
                                        Width="288px"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width: 190px; text-align: right">
                                    Garage Card Required?</td>
                                <td style="width: 310px; text-align: left">
                                    <asp:DropDownList ID="ddGarageCard" runat="server" Width="109px" DataTextField="DDValue"
                                        DataValueField="DDOptionsID" DataSourceID="oSqlDSYesNo">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="width: 190px; text-align: right">
                                    Payment Method</td>
                                <td style="width: 310px; text-align: left">
                                    <asp:DropDownList ID="ddPayMethod" runat="server" Width="109px" DataTextField="DDValue"
                                        DataValueField="DDOptionsID" DataSourceID="oSqlDSPaymentMethod" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddPayMethod_SelectedIndexChanged">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="width: 190px; text-align: right">
                                    APO Type</td>
                                <td style="width: 310px; text-align: left">
                                    <asp:DropDownList ID="ddAPOType" runat="server" Width="109px" DataTextField="DDValue"
                                        DataValueField="DDOptionsID" DataMember="DefaultView" DataSourceID="oSqlDSAPOType"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddAPOType_SelectedIndexChanged">
                                    </asp:DropDownList><asp:RangeValidator ID="oRVAPOType" runat="server" ControlToValidate="ddAPOType"
                                        Display="Dynamic" Enabled="False" ErrorMessage="APO Type must be selected"
                                        MaximumValue="99" MinimumValue="1"></asp:RangeValidator></td>
                            </tr>
                            <tr>
                                <td style="width: 190px; text-align: right">
                                    APO Amount</td>
                                <td style="width: 310px; text-align: left">
                                    <asp:TextBox ID="txtAPOAmount" runat="server" Width="103px"></asp:TextBox><asp:RequiredFieldValidator
                                        ID="oRfvAPOAmount" runat="server" Display="Dynamic" ErrorMessage=" Required"
                                        Enabled="False" ControlToValidate="txtAPOAmount"></asp:RequiredFieldValidator><asp:CompareValidator ID="oRVAPOAmount" runat="server"
                                        ControlToValidate="txtAPOAmount" ErrorMessage="Numeric values required" Operator="DataTypeCheck"
                                        SetFocusOnError="True" Type="Currency"></asp:CompareValidator></td>
                            </tr>
                            <tr>
                                <td style="width: 190px; text-align: right">
                                    APO Day</td>
                                <td style="width: 310px; text-align: left">
                                    <asp:TextBox ID="txtAPODay" runat="server" Width="103px"></asp:TextBox><asp:RangeValidator
                                        ID="oRVAPODay" runat="server" Display="Dynamic" ErrorMessage="1 - 31 required"
                                        MinimumValue="1" ControlToValidate="txtAPODay" MaximumValue="31" Type="Integer"></asp:RangeValidator><asp:RequiredFieldValidator ID="oRfvAPODay" runat="server" Display="Dynamic"
                                        ControlToValidate="txtAPODay" ErrorMessage="Required" Enabled="False"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td style="width: 190px; text-align: right">
                                    Bank</td>
                                <td style="width: 310px; text-align: left">
                                    <asp:DropDownList ID="ddBank" runat="server" Width="221px" DataTextField="ACBBankDescription"
                                        DataValueField="ACBBankCode" AutoPostBack="True" DataMember="DefaultView" DataSourceID="oSqlDSACBBank">
                                    </asp:DropDownList><asp:RangeValidator ID="oRVBank" runat="server" Enabled="False"
                                        ErrorMessage="Required" MinimumValue="0" ControlToValidate="ddBank" MaximumValue="999999"
                                        Display="Dynamic"></asp:RangeValidator></td>
                            </tr>
                            <tr>
                                <td style="width: 190px; text-align: right">
                                    Branch</td>
                                <td style="width: 310px; text-align: left">
                                    <asp:DropDownList ID="ddBranch" runat="server" Width="221px" DataTextField="ACBBranchDescription"
                                        DataValueField="ACBBranchCode" DataMember="DefaultView" DataSourceID="oSqlDSACBBranch">
                                    </asp:DropDownList><asp:RangeValidator ID="oRVBranch" runat="server" Enabled="False"
                                        ErrorMessage="Required" MinimumValue="0" ControlToValidate="ddBranch" MaximumValue="999999"
                                        Display="Dynamic"></asp:RangeValidator></td>
                            </tr>
                            <tr>
                                <td style="width: 190px; text-align: right">
                                    Account Type</td>
                                <td style="width: 310px; text-align: left">
                                    <asp:DropDownList ID="ddAccountType" runat="server" Width="221px" DataTextField="ACBTypeDescription"
                                        DataValueField="ACBTypeNumber" DataMember="DefaultView" DataSourceID="oSqlDSACBType">
                                    </asp:DropDownList><asp:RangeValidator ID="oRVAccountType" runat="server" Enabled="False"
                                        ErrorMessage="Required" MinimumValue="0" ControlToValidate="ddAccountType"
                                        MaximumValue="99" Display="Dynamic"></asp:RangeValidator></td>
                            </tr>
                            <tr>
                                <td style="width: 190px; text-align: right">
                                    Account Number</td>
                                <td style="width: 310px; text-align: left">
                                    <asp:TextBox ID="txtAccountNum" runat="server" Width="133px"></asp:TextBox><asp:RequiredFieldValidator
                                        ID="oRfvAccountNumber" runat="server" Enabled="False" ControlToValidate="txtAccountNum"
                                        ErrorMessage="Required" Display="Dynamic"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td style="width: 190px; text-align: right">
                                    Group Products &amp; Services</td>
                                <td style="width: 310px; text-align: left">
                                    <asp:DropDownList ID="ddGroupProductsServices" runat="server" Width="109px" DataTextField="DDValue"
                                        DataValueField="DDOptionsID" DataMember="DefaultView" DataSourceID="oSqlDSYesNo">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="width: 190px; text-align: right">
                                    Research Permissions</td>
                                <td style="width: 310px; text-align: left">
                                    <asp:DropDownList ID="ddResearchPermission" runat="server" Width="109px" DataTextField="DDValue"
                                        DataValueField="DDOptionsID" DataMember="DefaultView" DataSourceID="oSqlDSYesNo">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="width: 190px; text-align: right">
                                    Company Products &amp; Services</td>
                                <td style="width: 310px; text-align: left">
                                    <asp:DropDownList ID="ddOtherProductsServices" runat="server" Width="109px" DataTextField="DDValue"
                                        DataValueField="DDOptionsID" DataMember="DefaultView" DataSourceID="oSqlDSYesNo">
                                    </asp:DropDownList><asp:TextBox ID="txtBarclayCardKey1" runat="server" Visible="False"
                                        Width="29px"></asp:TextBox></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
                <td>
                    <asp:Panel ID="pnlDetails2b" runat="server" Width="100%">
                        <table>
                            <tr>
                                <td colspan="2" style="text-align: center; width: 190px;">
                                    <asp:Label ID="lblSecondaryMsg2" runat="server" Font-Bold="True" ForeColor="Red"
                                        Text="All Details below as per Primary Card Holder" Width="288px"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 190px; height: 4px;">
                                    Garage Card Required?</td>
                                <td style="width: 310px; text-align: left; height: 4px;">
                                    <asp:DropDownList ID="ddGarageCard2" runat="server" Width="109px" DataTextField="DDValue"
                                        DataValueField="DDOptionsID" DataSourceID="oSqlDSYesNo">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 190px;">
                                    Payment Method</td>
                                <td style="width: 310px; text-align: left">
                                    <asp:DropDownList ID="ddPayMethod2" runat="server" Width="109px" DataTextField="DDValue"
                                        DataValueField="DDOptionsID" DataSourceID="oSqlDSPaymentMethod" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddPayMethod2_SelectedIndexChanged">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 190px;">
                                    APO Type</td>
                                <td style="width: 310px; text-align: left">
                                    <asp:DropDownList ID="ddAPOType2" runat="server" Width="109px" DataTextField="DDValue"
                                        DataValueField="DDOptionsID" DataMember="DefaultView" DataSourceID="oSqlDSAPOType"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddAPOType2_SelectedIndexChanged">
                                    </asp:DropDownList><asp:RangeValidator ID="oRVAPOType2" runat="server" ControlToValidate="ddAPOType2"
                                        Display="Dynamic" Enabled="False" ErrorMessage="APO Type must be selected"
                                        MaximumValue="99" MinimumValue="1"></asp:RangeValidator></td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 190px;">
                                    APO Amount</td>
                                <td style="width: 310px; text-align: left">
                                    <asp:TextBox ID="txtAPOAmount2" runat="server" Width="103px"></asp:TextBox><asp:RequiredFieldValidator
                                        ID="oRfvAPOAmount2" runat="server" Display="Dynamic" ErrorMessage="Required"
                                        Enabled="False" ControlToValidate="txtAPOAmount2"></asp:RequiredFieldValidator><asp:CompareValidator ID="oRVAPOAmount2" runat="server"
                                        ControlToValidate="txtAPOAmount2" ErrorMessage="Numeric values required" Operator="DataTypeCheck"
                                        SetFocusOnError="True" Type="Currency"></asp:CompareValidator></td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 190px;">
                                    APO Day</td>
                                <td style="width: 310px; text-align: left">
                                    <asp:TextBox ID="txtAPODay2" runat="server" Width="103px"></asp:TextBox><asp:RangeValidator
                                        ID="oRVAPODay2" runat="server" Display="Dynamic" ErrorMessage="1 - 31 required"
                                        MinimumValue="1" ControlToValidate="txtAPODay2" MaximumValue="31" Type="Integer"></asp:RangeValidator><asp:RequiredFieldValidator ID="oRfvAPODay2" runat="server"
                                        Display="Dynamic" ControlToValidate="txtAPODay2" ErrorMessage="Required" Enabled="False"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 190px;">
                                    Bank</td>
                                <td style="width: 310px; text-align: left">
                                    <asp:DropDownList ID="ddBank2" runat="server" Width="221px" DataTextField="ACBBankDescription"
                                        DataValueField="ACBBankCode" AutoPostBack="True" DataMember="DefaultView" DataSourceID="oSqlDSACBBank">
                                    </asp:DropDownList><asp:RangeValidator ID="oRVBank2" runat="server" Enabled="False"
                                        ErrorMessage="Required" MinimumValue="0" ControlToValidate="ddBank2" MaximumValue="999999"
                                        Display="Dynamic"></asp:RangeValidator></td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 190px;">
                                    Branch</td>
                                <td style="width: 310px; text-align: left">
                                    <asp:DropDownList ID="ddBranch2" runat="server" Width="221px" DataTextField="ACBBranchDescription"
                                        DataValueField="ACBBranchCode" DataMember="DefaultView" DataSourceID="oSqlDSACBBranch2">
                                    </asp:DropDownList><asp:RangeValidator ID="oRVBranch2" runat="server" Enabled="False"
                                        ErrorMessage="Required" MinimumValue="0" ControlToValidate="ddBranch2" MaximumValue="999999"
                                        Display="Dynamic"></asp:RangeValidator></td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 190px;">
                                    Account Type</td>
                                <td style="width: 310px; text-align: left">
                                    <asp:DropDownList ID="ddAccountType2" runat="server" Width="221px" DataTextField="ACBTypeDescription"
                                        DataValueField="ACBTypeNumber" DataMember="DefaultView" DataSourceID="oSqlDSACBType">
                                    </asp:DropDownList><asp:RangeValidator ID="oRVAccountType2" runat="server" Enabled="False"
                                        ErrorMessage="Required" MinimumValue="0" ControlToValidate="ddAccountType2"
                                        MaximumValue="99" Display="Dynamic"></asp:RangeValidator></td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 190px;">
                                    Account Number</td>
                                <td style="width: 310px; text-align: left">
                                    <asp:TextBox ID="txtAccountNum2" runat="server" Width="133px"></asp:TextBox><asp:RequiredFieldValidator
                                        ID="oRfvAccountNumber2" runat="server" Enabled="False" ControlToValidate="txtAccountNum2"
                                        ErrorMessage="Required" Display="Dynamic"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 190px;">
                                    Group Products &amp; Services</td>
                                <td style="width: 310px; text-align: left">
                                    <asp:DropDownList ID="ddGroupProductsServices2" runat="server" Width="109px" DataTextField="DDValue"
                                        DataValueField="DDOptionsID" DataMember="DefaultView" DataSourceID="oSqlDSYesNo">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 190px;">
                                    Research Permissions</td>
                                <td style="width: 310px; text-align: left">
                                    <asp:DropDownList ID="ddResearchPermission2" runat="server" Width="109px" DataTextField="DDValue"
                                        DataValueField="DDOptionsID" DataMember="DefaultView" DataSourceID="oSqlDSYesNo">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 190px;">
                                    Company's Products &amp; Services</td>
                                <td style="width: 310px; text-align: left">
                                    <asp:DropDownList ID="ddOtherProductsServices2" runat="server" Width="109px" DataTextField="DDValue"
                                        DataValueField="DDOptionsID" DataMember="DefaultView" DataSourceID="oSqlDSYesNo">
                                    </asp:DropDownList><asp:TextBox ID="txtBarclayCardKey2" runat="server" Visible="False"
                                        Width="29px"></asp:TextBox></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center">
                    <asp:Label ID="lblSigned" runat="server" Text="Please only complete the section below once a SIGNED application is received from the client..."
                        Font-Bold="True"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 537px">
                    <asp:Panel ID="pnlDetails1c" runat="server" Width="100%">
                        <table>
                            <tr>
                                <td style="width: 190px; text-align: right">
                                    Signed Application Recieved?</td>
                                <td style="width: 310px; text-align: left">
                                    <asp:DropDownList ID="ddSignedApp" runat="server" Width="109px" DataTextField="DDValue"
                                        DataValueField="DDOptionsID" DataMember="DefaultView" DataSourceID="oSqlDSYesNo"
                                        OnSelectedIndexChanged="ddSignedApp_SelectedIndexChanged" AutoPostBack="True">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="width: 190px; text-align: right">
                                    Application Submitted on</td>
                                <td style="width: 310px; text-align: left">
                                    <asp:TextBox ID="txtSubmittedDate" runat="server" ReadOnly="True"></asp:TextBox></td>
                            </tr>
                        </table>
                        <asp:CheckBox ID="chkLoanRegisteredMoreThan3Months" runat="server" />&nbsp;
                    </asp:Panel>
                </td>
                <td>
                    <asp:Panel ID="pnlDetails2c" runat="server" Width="100%">
                        <table>
                            <tr>
                                <td style="width: 190px; text-align: right">
                                    Signed Application Recieved?</td>
                                <td style="width: 310px; text-align: left">
                                    <asp:DropDownList ID="ddSignedApp2" runat="server" Width="109px" DataTextField="DDValue"
                                        DataValueField="DDOptionsID" DataMember="DefaultView" DataSourceID="oSqlDSYesNo"
                                        OnSelectedIndexChanged="ddSignedApp2_SelectedIndexChanged" AutoPostBack="True">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="width: 190px; text-align: right">
                                    Application Submitted on</td>
                                <td style="width: 310px; text-align: left">
                                    <asp:TextBox ID="txtSubmittedDate2" runat="server" Enabled="False" ReadOnly="True"></asp:TextBox></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <table style="width: 1000px">
            <tr>
                <td>
                    <asp:Panel ID="pMessage" runat="server" Width="100%">
                        <asp:Label ID="lblMsg" runat="server"></asp:Label>
                        <br />
                        <asp:Button ID="bMsgReturn" OnClick="bMsgReturn_Click" runat="server" Text="Return" /></asp:Panel>
                </td>
            </tr>
        </table>
        <asp:CheckBox ID="chkDisplayDetailsTable" runat="server" />
        <asp:CheckBox ID="chkGoldCardManager" runat="server" />&nbsp;&nbsp;
        <asp:SqlDataSource ID="oSqlDSMatrix" runat="server" ProviderName="System.Data.SqlClient"
            SelectCommand="Select  Category, &#10; Case EmploymentTypeNr &#10;  when 1 then 'Salaried'&#10;  when 2 then 'Self-Employed'&#10;  when 3 then 'Subsidised'&#10;  else 'n/a' end as 'Employment Type', &#10; Case &#10;  when (category = 1 and LTV = 79.99) then '<80%' &#10;  when (category = 1 and LTV = 100 and &#10;   Case EmploymentTypeNr &#10;    when 1 then 'Salaried'&#10;    when 2 then 'Self-Employed'&#10;    when 3 then 'Subsidised'&#10;    else 'n/a' end = 'Salaried') then '>=80%' &#10;  when (category = 1 and LTV = 100 and &#10;   Case EmploymentTypeNr &#10;    when 1 then 'Salaried'&#10;    when 2 then 'Self-Employed'&#10;    when 3 then 'Subsidised'&#10;    else 'n/a' end <> 'Salaried') then 'n/a' &#10;  else 'n/a' end as LTV, &#10; Sum(Income1) '4000   - 5500', &#10; Sum(Income2) '5501   - 6999', &#10; Sum(Income3) '7000   - 8499', &#10; Sum(Income4) '8500   - 12499', &#10; Sum(Income5) '12500  - 20000', &#10; sum(Income6) '20000  - 40000', &#10; Sum(Income7) '40001  - 500000'&#10;from &#10;(Select Category, EmploymentTypeNr, LTV,&#10; CASE (convert(char(6),MinIncome) + ' - ' + convert(char(6),MaxIncome))&#10;  When '4000   - 5500'  then Limit else '' end as 'Income1',&#10; CASE (convert(char(6),MinIncome) + ' - ' + convert(char(6),MaxIncome))&#10;  When '5501   - 6999'  then Limit else '' end as 'Income2',&#10; CASE (convert(char(6),MinIncome) + ' - ' + convert(char(6),MaxIncome))&#10;  When '7000   - 8499'  then Limit else '' end as 'Income3',&#10; CASE (convert(char(6),MinIncome) + ' - ' + convert(char(6),MaxIncome))&#10;  When '8500   - 12499'  then Limit else '' end as 'Income4',&#10; CASE (convert(char(6),MinIncome) + ' - ' + convert(char(6),MaxIncome)) &#10;  When '12500  - 20000'  then Limit else '' end as 'Income5',&#10; CASE (convert(char(6),MinIncome) + ' - ' + convert(char(6),MaxIncome)) &#10;  When '20001  - 40000'  then Limit else '' end as 'Income6', &#10; CASE (convert(char(6),MinIncome) + ' - ' + convert(char(6),MaxIncome)) &#10;  When '40001  - 500000'  then Limit else '' end as 'Income7' &#10;from barclayCardMatrix ) as tmp&#10; &#10;group by Category, Case EmploymentTypeNr &#10;  when 1 then 'Salaried'&#10;  when 2 then 'Self-Employed'&#10;  when 3 then 'Subsidised'&#10;  else 'n/a' end, LTV&#10;"
            DataSourceMode="DataReader"></asp:SqlDataSource>
        <asp:SqlDataSource ID="oSqlDSYesNo" runat="server" SelectCommand="SELECT DDOptionsID,DDValue FROM dbo.BarclayCardDDOptions where DDField = 'YesNo' &#10;union &#10;select -1 as DDOptionsID, null as DDValue"
            ProviderName="System.Data.SqlClient" DataSourceMode="DataReader"></asp:SqlDataSource>
        <asp:SqlDataSource ID="oSqlDSPaymentMethod" runat="server" SelectCommand="SELECT [DDOptionsID], [DDValue] FROM [BarclayCardDDOptions] WHERE ([DDField] = 'PaymentType') &#10;union &#10;select -1 as DDOptionsID, null as DDValue"
            ProviderName="System.Data.SqlClient" DataSourceMode="DataReader"></asp:SqlDataSource>
        <asp:SqlDataSource ID="oSqlDSAPOType" runat="server" SelectCommand="SELECT DDOptionsID, DDValue FROM dbo.BarclayCardDDOptions where DDField = 'APOType' &#10;union &#10;select -1 as DDOptionsID,null as DDValue"
            ProviderName="System.Data.SqlClient" DataSourceMode="DataReader"></asp:SqlDataSource>
        <asp:SqlDataSource ID="oSqlDSACBBank" runat="server" SelectCommand="SELECT [ACBBankCode], [ACBBankDescription] FROM [ACBBank] &#10;union &#10;select -1 as ACBBankCode,  null as ACBBankDescription ORDER BY [ACBBankDescription]"
            ProviderName="System.Data.SqlClient" DataSourceMode="DataReader" ConnectionString="Data Source=SAHLS11;Initial Catalog=SAHLDB;User ID=williamr">
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="oSqlDSACBBranch" runat="server" SelectCommand="&#10;SELECT ACBBranchCode, ACBBranchDescription,acbbankcode&#10; FROM ACBBranch &#10;WHERE (ActiveIndicator = 0) &#10;union &#10;select -1 as ACBBranchCode, null as ACBBranchDescription,-1&#10;order by ACBBranchDescription"
            ProviderName="System.Data.SqlClient" FilterExpression="acbbankcode={0}">
            <FilterParameters>
                <asp:ControlParameter Name="bank" DefaultValue="-1" ControlID="ddBank" PropertyName="SelectedValue">
                </asp:ControlParameter>
            </FilterParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="oSqlDSACBBranch2" runat="server" SelectCommand="&#10;SELECT ACBBranchCode, ACBBranchDescription,acbbankcode &#10;FROM ACBBranch &#10;WHERE (ActiveIndicator = 0) &#10;union &#10;select -1 as ACBBranchCode, null as ACBBranchDescription,-1&#10;order by ACBBranchDescription"
            ProviderName="System.Data.SqlClient" FilterExpression="acbbankcode={0}">
            <FilterParameters>
                <asp:ControlParameter ControlID="ddBank2" DefaultValue="-1" Name="bank" PropertyName="SelectedValue" />
            </FilterParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="oSqlDSACBType" runat="server" SelectCommand="SELECT [ACBTypeNumber], [ACBTypeDescription] FROM [ACBType] &#10;union &#10;select -1 as ACBTypeNumber, null as ACBTypeDescription"
            ProviderName="System.Data.SqlClient" DataSourceMode="DataReader"></asp:SqlDataSource>
        <asp:SqlDataSource ID="oSqlDSCardType" runat="server" SelectCommand="SELECT DDOptionsID,DDValue FROM dbo.BarclayCardDDOptions where DDField = 'CardType' &#10;union &#10;select -1 as DDOptionsID, null as DDValue"
            ProviderName="System.Data.SqlClient" DataSourceMode="DataReader"></asp:SqlDataSource>
    </form>
</body>
</html>
