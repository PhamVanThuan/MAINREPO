<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LegalEntityDetail.ascx.cs" Inherits="LegalEntityDetail" %>
<link href="../../style.css" type="text/css" rel="stylesheet" />
<table width="100%">
    <tr>
        <td class="ManagementPanelTitle">
            Legal Entity Details:</td>
        <td align="right">
        </td>
    </tr>
</table>
<asp:Panel ID="pnlDisplay" runat="server" Wrap="False">
    <asp:Repeater ID="Repeater1" runat="server" DataSourceID="oLegalEntity" EnableViewState="False">
        <ItemTemplate>
            <table align="center" border="0" cellpadding="2" cellspacing="0" width="100%">
                <tr>
                    <td align="left" class="TableRowSeperator" nowrap="nowrap">
                        <b>Salutation:</b></td>
                    <td class="TableRowSeperator">
                        &nbsp;<%# DataBinder.Eval(Container.DataItem, "Salutation") %>
                    </td>
                    <td align="left" class="TableRowSeperator" nowrap="nowrap">
                        <b>Trading Name:</b></td>
                    <td class="TableRowSeperator">
                        &nbsp;<%# DataBinder.Eval(Container.DataItem, "TradingName") %>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="TableRowSeperator" nowrap="nowrap">
                        <b>Initials:</b></td>
                    <td class="TableRowSeperator">
                        &nbsp;<%# DataBinder.Eval(Container.DataItem, "Initials") %>
                    </td>
                    <td align="left" class="TableRowSeperator" nowrap="nowrap">
                        <b>Registered Name:</b></td>
                    <td class="TableRowSeperator">
                        &nbsp;<%# DataBinder.Eval(Container.DataItem, "RegisteredName") %>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="TableRowSeperator" nowrap="nowrap">
                        <b>First names:</b></td>
                    <td class="TableRowSeperator">
                        &nbsp;<%# DataBinder.Eval(Container.DataItem, "firstnames") %>
                    </td>
                    <td align="left" class="TableRowSeperator" nowrap="nowrap">
                        <b>Type:</b></td>
                    <td class="TableRowSeperator">
                        &nbsp;<%# DataBinder.Eval(Container.DataItem, "Type") %>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="TableRowSeperator" nowrap="nowrap">
                        <b>Surname:</b></td>
                    <td class="TableRowSeperator">
                        &nbsp;<%# DataBinder.Eval(Container.DataItem, "surname") %>
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:Repeater>
</asp:Panel>
<asp:HiddenField ID="hfLEKey" runat="server" />
<asp:HiddenField ID="hfAccountKey" runat="server" />
<asp:HiddenField ID="hfRoleTypeKey" runat="server" />
<asp:SqlDataSource ID="oLegalEntity" runat="server" ConnectionString="Server=SAHLS11;User ID=kirstenw;Database=2am;Persist Security Info=True"
    DeleteCommand="DELETE FROM dbo.Role WHERE &#10;(LegalEntityKey = @LEKeyDel) AND (AccountKey = @AcctKeyDel) AND (RoleTypeKey = @RoleTypeKeyDel)&#10;"
    InsertCommand="Insert into Role (LegalEntityKey, AccountKey, RoleTypeKey)&#10;Values(@LEKeyIns,@AcctKeyIns,@RoleTypeKeyIns)"
    OldValuesParameterFormatString="{0}" SelectCommand="select le.LegalEntityKey, le.Salutation, le.Firstnames, le.Initials,&#10;le.Surname, le.TradingName, le.RegisteredName, let.[description] as type &#10;from legalentity le &#10;inner join legalentitytype let on let.legalentitytypekey = le.legalentitytypekey&#10;where le.legalentitykey = @LEKeySel">
    <DeleteParameters>
        <asp:ControlParameter ControlID="hfLEKey" Name="LEKeyDel" PropertyName="Value" />
        <asp:ControlParameter ControlID="hfAccountKey" Name="AcctKeyDel" PropertyName="Value" />
        <asp:ControlParameter ControlID="hfRoleTypeKey" Name="RoleTypeKeyDel" PropertyName="Value" />
    </DeleteParameters>
    <SelectParameters>
        <asp:ControlParameter ControlID="hfLEKey" Name="LEKeySel" PropertyName="Value" />
    </SelectParameters>
    <InsertParameters>
        <asp:ControlParameter ControlID="hfLEKey" Name="LEKeyIns" PropertyName="Value" />
        <asp:ControlParameter ControlID="hfAccountKey" Name="AcctKeyIns" PropertyName="Value" />
        <asp:ControlParameter ControlID="hfRoleTypeKey" Name="RoleTypeKeyIns" PropertyName="Value" />
    </InsertParameters>
</asp:SqlDataSource>
<table border="0" width="100%">
    <tr>
        <td align="left">
            <asp:Button ID="cmdAdd" runat="server" CssClass="style.css" Text="Add" />
            <asp:Button ID="cmdRemove" runat="server" CssClass="style.css" Text="Remove" />
        </td>
        <td>
        </td>
        <td align="right">
            <asp:Button ID="cmdCancel" runat="server" CssClass="style.css" Text="Cancel" />
        </td>
    </tr>
</table>
<br />
<br />
