<%@ Register TagPrefix="Util" TagName="title" Src="../common/title.ascx" %>

<%@ Page Language="C#" CodeFile="BarclayCardApp_Old.aspx.cs" Inherits="BarclayCardApp_Old_aspx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

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
    
    function minmax(table,button) {
        if (table.style.display == 'none' ) {
            table.style.display = "";
            button.value  = "Hide Limit Matrix";
        }    
        else {
            table.style.display = 'none';
            button.value = "View Limit Matrix";
        }
    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>SAHL Gold Card Application</title>
    <link href="../style.css" rel="stylesheet" type="text/css" />
    <link href="../style.css" rel="stylesheet" type="text/css" />
    <link href="../style.css" rel="stylesheet" type="text/css" />
    <link href="../style.css" rel="stylesheet" type="text/css" />
    <link href="../style.css" rel="stylesheet" type="text/css" />
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
<body style="text-align: left">
    <form runat="server" id="Form1">
           <table width="700">
            <tr>
               <td width="700" style="text-align: center">
                   &nbsp;<asp:HyperLink ID="HyperLink1" runat="server">Back</asp:HyperLink></td>
            </tr>
        </table>
    
        <table id="tDetails" class="SAHLTable" style="width: 700px">
            <tr>
                <td colspan="2" style="text-align: center">
                    <asp:Label ID="lblProspLoanNum" runat="server"></asp:Label>
                    <asp:Label ID="lblProspLoanNumVal"
                        runat="server" Font-Bold="True"></asp:Label>
                    <asp:Label ID="lblLoanUnregistered" runat="server" Text="(Not Registered)" Visible="False"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 50%">
                </td>
                <td style="width: 50%">
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center">
                    <asp:Button ID="btnUpdate" runat="server" Text="Edit" Width="71px" OnClick="btnUpdate_Click" CausesValidation="False" /><asp:Button
                        ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Enabled="False"
                        Width="71px" /><br />
                    <asp:Label ID="lblNoUpdate" runat="server" Text="Application cannot be Edited as it has already been submitted to the Bank"
                        ForeColor="Red" Visible="False" Width="487px" Font-Bold="True"></asp:Label><br />
                    <asp:Button ID="btnReSubmit" runat="server" Text="Re-Submit"
                       OnClientClick="return confirm('Are you sure you want to Re-Submit this application?\r\n\r\nSelecting OK will allow the application to be edited before being Re-Submitted.')" /></td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Panel ID="pnlDetails" runat="server" Width="100%">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 40%; text-align: right">
                                </td>
                                <td style="width: 60%; text-align: left">
                                    <asp:CheckBox ID="chkNotInterested" runat="server" Text="Not Interested in Gold Card"
                                        AutoPostBack="True" OnCheckedChanged="chkNotInterested_CheckedChanged" /></td>
                            </tr>
                            <tr>
                                <td style="width: 40%; text-align: right">
                                    Salutation</td>
                                <td style="width: 60%; text-align: left">
                                    <asp:TextBox ID="txtSalutation" runat="server" Width="133px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvSalutation" runat="server" ControlToValidate="txtSalutation"
                                        ErrorMessage="<< Salutation must be entered"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td style="width: 40%; text-align: right">
                                    First Names</td>
                                <td style="width: 60%; text-align: left">
                                    <asp:TextBox ID="txtFirstNames" runat="server" Width="133px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvFirstNames" runat="server" ControlToValidate="txtFirstNames"
                                        ErrorMessage="<< First Names must be entered"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td style="width: 40%; text-align: right">
                                    Surname</td>
                                <td style="width: 60%; text-align: left">
                                    <asp:TextBox ID="txtSurname" runat="server" Width="133px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvSurname" runat="server" ControlToValidate="txtSurname"
                                        ErrorMessage="<< Surname must be entered"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td style="width: 40%; text-align: right">
                                    ID Number</td>
                                <td style="width: 60%; text-align: left">
                                    <asp:TextBox ID="txtIDnumber" runat="server" Width="133px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvIDNumber" runat="server" ControlToValidate="txtIDnumber"
                                        ErrorMessage="<< ID must be entered"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td style="width: 40%; text-align: right">
                                    Salary</td>
                                <td style="width: 60%; text-align: left">
                                    <asp:TextBox ID="txtSalary" runat="server" Width="133px"></asp:TextBox>
                                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtSalary"
                                        ErrorMessage="<< Numeric values required" Operator="DataTypeCheck" SetFocusOnError="True"
                                        Type="Currency"></asp:CompareValidator></td>
                            </tr>
                            <tr>
                                <td style="width: 40%; text-align: right">
                                    Card Type</td>
                                <td style="width: 60%; text-align: left">
                                    <asp:DropDownList ID="ddCardType" runat="server" Width="101px" Height="22px" DataTextField="DDValue"
                                        DataValueField="DDOptionsID" DataSourceID="oSqlDSCardType" AutoPostBack="True">
                                    </asp:DropDownList>
                                    </td>
                            </tr>
                            <tr>
                                <td style="width: 40%; text-align: right">
                                    Qualified Credit Limit</td>
                                <td style="width: 60%; text-align: left">
                                    <asp:TextBox ID="txtCreditLimit" runat="server" Width="103px"></asp:TextBox><input
                                        id="btnViewLimitMatrix" onclick="minmax(tlimitmatrix,btnViewLimitMatrix);" type="button"
                                        value="View Limit Matrix" style="cursor: hand;display:none"/><br />
                                    <asp:Label ID="lblRefusalMessage" runat="server" ForeColor="Red" Visible="False"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width: 40%; text-align: right">
                                    <asp:Label ID="lblOverrideReason" runat="server" Text="Override Reason" Visible="False"></asp:Label></td>
                                <td style="width: 60%; text-align: left">
                                    <asp:Button ID="btnOveride" runat="server" Text="Override Application" OnClick="btnOveride_Click" Visible="False" /><br />
                                    <asp:TextBox ID="txtReasonOveride" runat="server" TextMode="MultiLine" Width="384px" ></asp:TextBox><br />
                                    <asp:RequiredFieldValidator ID="oRVOverrideReason" runat="server" ControlToValidate="txtReasonOveride"
                                        ErrorMessage="Must Enter a Reason for Overriding the Application" Width="301px" Enabled="False"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: right; width: 40%;">
                                    <table id="tlimitmatrix" style="width: 100%; display: none;">
                                        <tr>
                                            <td colspan="3">
                                                <asp:GridView ID="GridView1" runat="server" ForeColor="Black" HorizontalAlign="Center"
                                                    DataSourceID="oGridDataSouce" BorderWidth="1px" BackColor="White" GridLines="Vertical"
                                                    CellPadding="4" BorderColor="#DEDFDE" BorderStyle="None" Font-Size="XX-Small"
                                                    Width="100%">
                                                    <FooterStyle BackColor="#CCCC99"></FooterStyle>
                                                    <PagerStyle ForeColor="Black" HorizontalAlign="Right" BackColor="#F7F7DE"></PagerStyle>
                                                    <HeaderStyle Font-Bold="True" BackColor="Silver" Wrap="False" ForeColor="White" Font-Size="X-Small">
                                                    </HeaderStyle>
                                                    <AlternatingRowStyle BackColor="White" Font-Size="X-Small"></AlternatingRowStyle>
                                                    <SelectedRowStyle ForeColor="White" BackColor="#CE5D5A" Font-Bold="True"></SelectedRowStyle>
                                                    <RowStyle BackColor="#F7F7DE" Font-Size="X-Small"></RowStyle>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 40%; text-align: right">
                                    Garage Card Required ?</td>
                                <td style="width: 60%; text-align: left">
                                    <asp:DropDownList ID="ddGarageCard" runat="server" Width="109px" Height="22px" DataTextField="DDValue"
                                        DataValueField="DDOptionsID" DataSourceID="oSqlDSYesNo">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="width: 40%; text-align: right">
                                    Payment Method</td>
                                <td style="width: 60%; text-align: left">
                                    <asp:DropDownList ID="ddPayMethod" runat="server" Width="109px" Height="22px" DataTextField="DDValue"
                                        DataValueField="DDOptionsID" DataSourceID="oSqlDSPaymentMethod" OnSelectedIndexChanged="ddPayMethod_SelectedIndexChanged">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="width: 40%; text-align: right">
                                    APO Type</td>
                                <td style="width: 60%; text-align: left">
                                    <asp:DropDownList ID="ddAPOType" runat="server" Width="109px" Height="22px" DataTextField="DDValue"
                                        DataValueField="DDOptionsID" DataMember="DefaultView" DataSourceID="oSqlDSAPOType"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddAPOType_SelectedIndexChanged">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="width: 40%; text-align: right">
                                    APO Amount</td>
                                <td style="width: 60%; text-align: left">
                                    <asp:TextBox ID="txtAPOAmount" runat="server" Width="101px"></asp:TextBox><asp:RequiredFieldValidator
                                        ID="oRfvAPOAmount" runat="server" Display="Dynamic" ErrorMessage="<< Required"
                                        Enabled="False" ControlToValidate="txtAPOAmount"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="oRVAPOAmount" runat="server" ControlToValidate="txtAPOAmount"
                                        ErrorMessage="<< Numeric values required" Operator="DataTypeCheck" SetFocusOnError="True"
                                        Type="Currency"></asp:CompareValidator></td>
                            </tr>
                            <tr>
                                <td style="width: 40%; text-align: right">
                                    APO Day</td>
                                <td style="width: 60%; text-align: left">
                                    <asp:TextBox ID="txtAPODay" runat="server" Width="101px"></asp:TextBox><asp:RangeValidator
                                        ID="oRVAPODay" runat="server" Display="Dynamic" ErrorMessage="<< 1 - 31 required"
                                        MinimumValue="1" ControlToValidate="txtAPODay" MaximumValue="31" Type="Integer">
                                    </asp:RangeValidator><asp:RequiredFieldValidator ID="oRfvAPODay" runat="server" Display="Dynamic"
                                        ControlToValidate="txtAPODay" ErrorMessage="<< Required" Enabled="False">
                                    </asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td style="width: 40%; text-align: right">
                                    Bank</td>
                                <td style="width: 60%; text-align: left">
                                    <asp:DropDownList ID="ddBank" runat="server" Width="221px" DataTextField="ACBBankDescription"
                                        DataValueField="ACBBankCode" AutoPostBack="True" DataMember="DefaultView" DataSourceID="oSqlDSACBBank"
                                        OnSelectedIndexChanged="ddBank_SelectedIndexChanged">
                                    </asp:DropDownList><asp:RangeValidator ID="oRVBank" runat="server"
                                        ErrorMessage="<< Required" MinimumValue="0" ControlToValidate="ddBank" MaximumValue="999999"
                                        Display="Dynamic"></asp:RangeValidator></td>
                            </tr>
                            <tr>
                                <td style="width: 40%; text-align: right">
                                    Branch</td>
                                <td style="width: 60%; text-align: left">
                                    <asp:DropDownList ID="ddBranch" runat="server" Width="221px" DataTextField="ACBBranchDescription"
                                        DataValueField="ACBBranchCode" DataMember="DefaultView" DataSourceID="oSqlDSACBBranch">
                                    </asp:DropDownList><asp:RangeValidator ID="oRVBranch" runat="server"
                                        ErrorMessage="<< Required" MinimumValue="0" ControlToValidate="ddBranch" MaximumValue="999999"
                                        Display="Dynamic"></asp:RangeValidator></td>
                            </tr>
                            <tr>
                                <td style="width: 40%; text-align: right">
                                    Account Type</td>
                                <td style="width: 60%; text-align: left">
                                    <asp:DropDownList ID="ddAccountType" runat="server" Width="221px" DataTextField="ACBTypeDescription"
                                        DataValueField="ACBTypeNumber" DataMember="DefaultView" DataSourceID="oSqlDSACBType">
                                    </asp:DropDownList><asp:RangeValidator ID="oRVAccountType" runat="server"
                                        ErrorMessage="<< Required" MinimumValue="0" ControlToValidate="ddAccountType"
                                        MaximumValue="99" Display="Dynamic"></asp:RangeValidator></td>
                            </tr>
                            <tr>
                                <td style="width: 40%; text-align: right">
                                    Account Number</td>
                                <td style="width: 60%; text-align: left">
                                    <asp:TextBox ID="txtAccountNum" runat="server" Width="133px"></asp:TextBox><asp:RequiredFieldValidator
                                        ID="oRfvAccountNumber" runat="server" ControlToValidate="txtAccountNum"
                                        ErrorMessage="<< Required" Display="Dynamic"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td style="width: 40%; text-align: right">
                                    Group Products &amp; Services</td>
                                <td style="width: 60%; text-align: left">
                                    <asp:DropDownList ID="ddGroupProductsSrevices" runat="server" Width="109px" DataTextField="DDValue"
                                        DataValueField="DDOptionsID" DataMember="DefaultView" DataSourceID="oSqlDSYesNo">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="width: 40%; text-align: right">
                                    Research Permissions</td>
                                <td style="width: 60%; text-align: left">
                                    <asp:DropDownList ID="ddResearchPermission" runat="server" Width="109px" DataTextField="DDValue"
                                        DataValueField="DDOptionsID" DataMember="DefaultView" DataSourceID="oSqlDSYesNo">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="width: 40%; text-align: right">
                                    Company Products & Services</td>
                                <td style="width: 60%; text-align: left">
                                    <asp:DropDownList ID="ddOtherProductsServices" runat="server" Width="109px" DataTextField="DDValue"
                                        DataValueField="DDOptionsID" DataMember="DefaultView" DataSourceID="oSqlDSYesNo">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td colspan="2" style="width: 40%; text-align: center">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: center; width: 40%;">
                                    <asp:Label ID="lblSigned" runat="server" Font-Bold="True" Text="Please only complete the section below once a SIGNED application is received from the client..."></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width: 40%; text-align: right">
                                    Signed Application Received?</td>
                                <td style="width: 60%; text-align: left">
                                    <asp:DropDownList ID="ddSignedApp" runat="server" Width="96px" DataTextField="DDValue"
                                        DataValueField="DDOptionsID" DataMember="DefaultView" DataSourceID="oSqlDSYesNo">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="width: 40%; text-align: right">
                                    <asp:Label ID="lblApplicationSubmittedOn" runat="server" Text="Application Submitted on"></asp:Label></td>
                                <td style="width: 60%; text-align: left">
                                    <asp:TextBox ID="txtApplicationSubmittedOn" runat="server" ReadOnly="True"></asp:TextBox></td>
                            </tr>
                        </table>
                        <asp:CheckBox ID="chkLoanRegisteredMoreThan3Months" runat="server" /></asp:Panel>
                </td>
            </tr>
        </table>
        <br />
        <table style="width: 700px;">
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
        <asp:CheckBox ID="chkGoldCardManager" runat="server" />
        <asp:SqlDataSource ID="oGridDataSouce" runat="server" ProviderName="System.Data.SqlClient"
            SelectCommand="Select  Category, &#10; Case EmploymentTypeNr &#10;  when 1 then 'Salaried'&#10;  when 2 then 'Self-Employed'&#10;  when 3 then 'Subsidised'&#10;  else 'n/a' end as 'Employment Type', &#10; Case &#10;  when (category = 1 and LTV = 79.99) then '<80%' &#10;  when (category = 1 and LTV = 100 and &#10;   Case EmploymentTypeNr &#10;    when 1 then 'Salaried'&#10;    when 2 then 'Self-Employed'&#10;    when 3 then 'Subsidised'&#10;    else 'n/a' end = 'Salaried') then '>=80%' &#10;  when (category = 1 and LTV = 100 and &#10;   Case EmploymentTypeNr &#10;    when 1 then 'Salaried'&#10;    when 2 then 'Self-Employed'&#10;    when 3 then 'Subsidised'&#10;    else 'n/a' end <> 'Salaried') then 'n/a' &#10;  else 'n/a' end as LTV, &#10; Sum(Income1) '4000   - 5500', &#10; Sum(Income2) '5501   - 6999', &#10; Sum(Income3) '7000   - 8499', &#10; Sum(Income4) '8500   - 12499', &#10; Sum(Income5) '12500  - 20000', &#10; sum(Income6) '20000  - 40000', &#10; Sum(Income7) '40001  - 500000'&#10;from &#10;(Select Category, EmploymentTypeNr, LTV,&#10; CASE (convert(char(6),MinIncome) + ' - ' + convert(char(6),MaxIncome))&#10;  When '4000   - 5500'  then Limit else '' end as 'Income1',&#10; CASE (convert(char(6),MinIncome) + ' - ' + convert(char(6),MaxIncome))&#10;  When '5501   - 6999'  then Limit else '' end as 'Income2',&#10; CASE (convert(char(6),MinIncome) + ' - ' + convert(char(6),MaxIncome))&#10;  When '7000   - 8499'  then Limit else '' end as 'Income3',&#10; CASE (convert(char(6),MinIncome) + ' - ' + convert(char(6),MaxIncome))&#10;  When '8500   - 12499'  then Limit else '' end as 'Income4',&#10; CASE (convert(char(6),MinIncome) + ' - ' + convert(char(6),MaxIncome)) &#10;  When '12500  - 20000'  then Limit else '' end as 'Income5',&#10; CASE (convert(char(6),MinIncome) + ' - ' + convert(char(6),MaxIncome)) &#10;  When '20001  - 40000'  then Limit else '' end as 'Income6', &#10; CASE (convert(char(6),MinIncome) + ' - ' + convert(char(6),MaxIncome)) &#10;  When '40001  - 500000'  then Limit else '' end as 'Income7' &#10;from barclayCardMatrix ) as tmp&#10; &#10;group by Category, Case EmploymentTypeNr &#10;  when 1 then 'Salaried'&#10;  when 2 then 'Self-Employed'&#10;  when 3 then 'Subsidised'&#10;  else 'n/a' end, LTV&#10;"
            DataSourceMode="DataReader"></asp:SqlDataSource>
        <asp:SqlDataSource ID="oSqlDSYesNo" runat="server" SelectCommand="SELECT DDOptionsID,DDValue FROM dbo.BarclayCardDDOptions where DDField = 'YesNo' &#10;union &#10;select -1 as DDOptionsID, null as DDValue"
            ProviderName="System.Data.SqlClient" DataSourceMode="DataReader"></asp:SqlDataSource>
        <asp:SqlDataSource ID="oSqlDSCardType" runat="server" SelectCommand="SELECT [DDOptionsID], [DDValue] FROM [BarclayCardDDOptions] WHERE ([DDField] = 'CardType') &#10;union &#10;select -1 as DDOptionsID, null as DDValue order by ddvalue">
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="oSqlDSPaymentMethod" runat="server" SelectCommand="SELECT [DDOptionsID], [DDValue] FROM [BarclayCardDDOptions] WHERE ([DDField] = 'PaymentType') &#10;union &#10;select -1 as DDOptionsID, null as DDValue"
            ProviderName="System.Data.SqlClient" DataSourceMode="DataReader"></asp:SqlDataSource>
        <asp:SqlDataSource ID="oSqlDSAPOType" runat="server" SelectCommand="SELECT DDOptionsID, DDValue FROM dbo.BarclayCardDDOptions where DDField = 'APOType' &#10;union &#10;select -1 as DDOptionsID,null as DDValue"
            ProviderName="System.Data.SqlClient" DataSourceMode="DataReader"></asp:SqlDataSource>
        <asp:SqlDataSource ID="oSqlDSACBBank" runat="server" SelectCommand="SELECT [ACBBankCode], [ACBBankDescription] FROM [ACBBank] &#10;union &#10;select -1 as ACBBankCode,  null as ACBBankDescription order by acbbankdescription"
            ProviderName="System.Data.SqlClient" DataSourceMode="DataReader" ConnectionString="Data Source=SAHLS11;Initial Catalog=SAHLDB;User ID=williamr">
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="oSqlDSACBBranch" runat="server" SelectCommand="&#10;SELECT ACBBranchCode, ACBBranchDescription,acbbankcode&#10;FROM ACBBranch &#10;WHERE (ActiveIndicator = 0) &#10;union &#10;select -1 as ACBBranchCode, null as ACBBranchDescription,-1&#10;order by ACBBranchDescription"
            ProviderName="System.Data.SqlClient" FilterExpression="acbbankcode={0}" ConnectionString="Data Source=SAHLS11;Initial Catalog=SAHLDB;User ID=williamr">
            <FilterParameters>
                <asp:ControlParameter Name="bank" DefaultValue="-1" ControlID="ddBank" PropertyName="SelectedValue">
                </asp:ControlParameter>
            </FilterParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="oSqlDSACBType" runat="server" SelectCommand="SELECT [ACBTypeNumber], [ACBTypeDescription] FROM [ACBType] &#10;union &#10;select -1 as ACBTypeNumber, null as ACBTypeDescription order by acbtypedescription"
            ProviderName="System.Data.SqlClient" DataSourceMode="DataReader"></asp:SqlDataSource>
    </form>
</body>
</html>
