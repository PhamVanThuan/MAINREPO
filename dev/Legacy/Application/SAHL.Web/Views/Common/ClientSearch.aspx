<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true"   Codebehind="ClientSearch.aspx.cs" Inherits="SAHL.Web.Views.Common.ClientSearch"   Title="Untitled Page" %>
<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls"    TagPrefix="SAHL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">


    <script type="text/javascript">
    function SearchClear()
    {
    
      document.getElementById("<%= AccountNumber.ClientID %>").value = "";    
      document.getElementById("<%= Surname.ClientID %>").value = "";    
      document.getElementById("<%= SearchName.ClientID %>").value = "";    
      document.getElementById("<%= IDNumber.ClientID %>").value = "";    
      document.getElementById("<%= Passport.ClientID %>").value = "";    
      document.getElementById("<%= CompanyName.ClientID %>").value = "";    
      document.getElementById("<%= CompanyNumber.ClientID %>").value = "";    
      document.getElementById("<%= PersalNumber.ClientID %>").value = "";   
       
//      document.getElementById("< % = TeleNumberCode.ClientID % >").value = "";    
//      document.getElementById("< % = TeleNumber.ClientID % >").value = "";      
           
    }
    function showSearchType(val)
    {
        if ( val == "Natural Person" )
        {
            showSearchTypeEx(true);
        }
        else
        {
            showSearchTypeEx(false);
        }
    }
    function showSearchTypeEx(bVal)
    {
        var iC;
        var natual;
        var company;
        var list = document.getElementsByName("NaturalGroup");
        
        if ( bVal )
        {
            natural = "";
            company = "none";
        }
        else
        {
            natural = "none";
            company = "";
        }
        
        // set the natural person search
        for ( iC=0; iC<list.length; iC++ )
        {
            list[iC].style.display = natural;
        }
        
        // set the natural person search
        list = document.getElementsByName("CompanyGroup");
        for ( iC=0; iC<list.length; iC++ )
        {
            list[iC].style.display = company;
        }
    }
    
    function changedIDType(val)
    {
        showIDType(val);
        document.getElementById("ctl00_Main_IDNumber").value = "";    
        document.getElementById("ctl00_Main_Passport").value = "";   
        
//        document.getElementById("ctl00_Main_TeleNumberCode").value = "";    
//        document.getElementById("ctl00_Main_TeleNumber").value = "";    
    }
    
    function changedNameType(val)
    {
        document.getElementById("ctl00_Main_SearchName").value = "";    
    }
    
    function changedSearchType(val)
    {
        //SearchClear();
        showSearchType(val);
    }
    
    function changedCompNameType(val)
    {
      document.getElementById("ctl00_Main_CompanyName").value = "";    
    }
    function showIDType(val)
    {
        var oID = document.getElementById("tdID");
        var oPass = document.getElementById("tdPassport");
//        var oTeleNumber = document.getElementById("tdTeleNumber");
        
        if ( val == "ID Number" )
        {
            oID.style.display = "";
            oPass.style.display = "none";
//            oTeleNumber.style.display = "none";
        } else if ( val == "Passport Number" )        
        {
            oID.style.display = "none";
            oPass.style.display = "";            
//            oTeleNumber.style.display = "none";
        }
        else
        {
            oID.style.display = "none";
            oPass.style.display = "none";            
//            oTeleNumber.style.display = "";  
        }
    }
    </script>

    <table border="0" width="100%">
        <tr>
            <td style="width: 195px;" align="left" class="TitleText">
                Search Type
            </td>
            <td align="left">
                <SAHL:SAHLDropDownList ID="cbxSearchType" runat="server" PleaseSelectItem="false"
                    onchange="changedSearchType(this.value);">
                    <asp:ListItem Value="Natural Person">Natural Person</asp:ListItem>
                    <asp:ListItem Value="Company">Company</asp:ListItem>
                </SAHL:SAHLDropDownList>
            </td>
        </tr>
        <tr>
            <td align="left" class="TitleText" style="width: 195px">
                Account Type</td>
            <td align="left">
                <SAHL:SAHLDropDownList ID="AccountType" runat="server" PleaseSelectItem="false">
                </SAHL:SAHLDropDownList></td>
        </tr>
        <tr>
            <td align="left" style="width: 195px" class="TitleText">
                Account Number
            </td>
            <td align="left">
                <SAHL:SAHLTextBox ID="AccountNumber" runat="server" Style="width: 200px;" onkeyup="pushTheButton();"
                    MaxLength="7" DisplayInputType="Number"></SAHL:SAHLTextBox>
            </td>
        </tr>
        <tr id="NaturalGroup" name="NaturalGroup">
            <td align="left" style="width: 195px" class="TitleText">
                Surname
            </td>
            <td align="left">
                <SAHL:SAHLTextBox ID="Surname" runat="server" Style="width: 400px;" onkeyup="pushTheButton();"
                    MaxLength="50"></SAHL:SAHLTextBox>
            </td>
        </tr>
        <tr id="Tr1" name="NaturalGroup">
            <td align="left" style="width: 195px">
                <SAHL:SAHLDropDownList ID="NameType" runat="server" Style="font-weight: bold;" Width="172px"
                    PleaseSelectItem="false" onchange="changedNameType(this.value);">
                    <asp:ListItem Value="First Names">First Names</asp:ListItem>
                    <asp:ListItem Value="Preferred Name">Preferred Name</asp:ListItem>
                </SAHL:SAHLDropDownList>
            </td>
            <td align="left">
                <SAHL:SAHLTextBox ID="SearchName" runat="server" Style="width: 400px;" onkeyup="pushTheButton();"
                    MaxLength="50"></SAHL:SAHLTextBox>
            </td>
        </tr>
        <tr id="Tr2" name="NaturalGroup">
            <td align="left" style="width: 195px">
                <SAHL:SAHLDropDownList ID="IDType" runat="server" Width="173px" Style="font-weight: bold;"
                    onchange="changedIDType(this.value);" PleaseSelectItem="false">
                    <asp:ListItem Value="ID Number">ID Number</asp:ListItem>
                    <asp:ListItem Value="Passport Number">Passport Number</asp:ListItem>
                </SAHL:SAHLDropDownList>
            </td>
            <td id="tdID" name="tdID" align="left">
                <SAHL:SAHLTextBox ID="IDNumber" runat="server" Style="width: 400px;" onkeyup="pushTheButton();"
                    DisplayInputType="Number" MaxLength="13"></SAHL:SAHLTextBox>
            </td>
            <td id="tdPassport" name="tdPassport" align="left" style="display: none;">
                <SAHL:SAHLTextBox ID="Passport" runat="server" Style="width: 400px;" onkeyup="pushTheButton();"
                    MaxLength="20"></SAHL:SAHLTextBox>
            </td>
        </tr>
        <tr id="CompanyGroup" name="CompanyGroup" style="display: none;">
            <td align="left" style="width: 195px">
            </td>
            <td align="left">
                <!-- Dummy textbox to keep the height -->
                <asp:TextBox ID="TextBox1" runat="server" Style="width: 400px;" MaxLength="50" CssClass="InText2"
                    Text="     "></asp:TextBox>
            </td>
        </tr>
        <tr id="Tr3" name="CompanyGroup" style="display: none;">
            <td align="left" style="width: 195px">
                <SAHL:SAHLDropDownList ID="CompNameType" runat="server" Style="font-weight: bold;"
                    Width="173px" PleaseSelectItem="false" onchange="changedCompNameType(this.value);">
                    <asp:ListItem Value="Registered Name">Registered Name</asp:ListItem>
                    <asp:ListItem Value="Trading Name">Trading Name</asp:ListItem>
                </SAHL:SAHLDropDownList>
            </td>
            <td align="left">
                <SAHL:SAHLTextBox ID="CompanyName" runat="server" Style="width: 400px;" onkeyup="pushTheButton();"
                    MaxLength="50"></SAHL:SAHLTextBox>
            </td>
        </tr>
        <tr id="Tr4" name="CompanyGroup" style="display: none;">
            <td align="left" style="width: 195px" class="TitleText">
                Company Number
            </td>
            <td align="left">
                <SAHL:SAHLTextBox ID="CompanyNumber" runat="server" Style="width: 400px;" onkeyup="pushTheButton();"
                    MaxLength="25"></SAHL:SAHLTextBox>
            </td>
        </tr>
        <tr id="Tr5" name="NaturalGroup">
            <td align="left" style="width: 195px" class="TitleText">
                Persal / Salary Number
            </td>
            <td align="left">
                <SAHL:SAHLTextBox ID="PersalNumber" runat="server" Style="width: 200px;" onkeyup="pushTheButton();"
                    MaxLength="10"></SAHL:SAHLTextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                &nbsp; &nbsp;
                <SAHL:SAHLButton ID="btnSearch" runat="server" Text="Search" AccessKey="S" OnClick="btnSearch_Click" />
                &nbsp;
                <SAHL:SAHLCustomValidator ID="ValidateSearchCtrl" EnableViewState="true" OnServerValidate="ValidateSearch"
                    runat="server" ErrorMessage="No Search Criteria selected" />
                <!--<asp:Button ID="ClearAction" runat="server" Text="Clear" CssClass="btnNormal3" AccessKey="C" OnClientClick="Validate()" />-->
                <input type="button" class="BtnNormal3" accesskey="C" value="Clear" onclick="SearchClear()" />
            </td>
        </tr>
    </table>
    <br />
    <SAHL:SAHLGridView ID="SearchGrid" runat="server" AutoGenerateColumns="false" FixedHeader="false"
        EnableViewState="false" GridHeight="200px" GridWidth="100%" Width="100%" HeaderCaption="Client Search Results"
        PostBackType="DoubleClickWithClientSelect" NullDataSetMessage="" EmptyDataSetMessage="The search returned no Clients."
        OnRowDataBound="SearchGrid_RowDataBound" OnGridDoubleClick="SearchGrid_GridDoubleClick">
        <RowStyle CssClass="TableRowA" />
    </SAHL:SAHLGridView>
    <asp:Panel id="ButtonRow" runat="server" style="height: 24px;text-align:right;">
        <SAHL:SAHLButton ID="btnCancel" runat="server" Text="Cancel" AccessKey="C" OnClick="btnCancel_Click"
            Visible="False" />&nbsp;
        <SAHL:SAHLButton ID="btnNewLegalEntity" runat="server" Text="New Legal Entity" AccessKey="N"
            OnClick="btnNewLegalEntity_Click" ButtonSize="Size6" />&nbsp;
        <SAHL:SAHLButton ID="Select" runat="server" Text="Select" AccessKey="L" Enabled="false"
            OnClick="Select_Click" />
    </asp:Panel>
    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />

    <script type="text/javascript">
    
        var oSearchType = document.getElementById("<%= cbxSearchType.ClientID %>");
        showSearchType(oSearchType.value);
        
        var oIdType = document.getElementById("<%= IDType.ClientID %>");
        showIDType(oIdType.value);
        
    </script>

</asp:Content>
