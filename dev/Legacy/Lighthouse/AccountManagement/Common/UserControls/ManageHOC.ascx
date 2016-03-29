<%@ Control Language="C#" CodeFile="ManageHOC.ascx.cs" Inherits="ManageHOC_ascx" %>
<%@ Register TagPrefix="uc1" TagName="toolbar" Src="toolbar.ascx" %>
<link href="../../style.css" type="text/css" rel="stylesheet" />
<table width="100%">
    <tr>
        <td class="ManagementPanelTitle" valign="top">
            HOC</td>
        <td align="right">
            <uc1:toolbar ID="ucToolbar" Runat="server" Visible="false" />
            <asp:ImageButton ID="ibManageHOC" runat="server" ImageUrl="~/Common/Images/edit.gif" />
            &nbsp;</td>
    </tr>
</table>
<asp:Panel ID="pnlEdit" Runat="server">
<table border="0" cellspacing="0" cellpadding="2" align=center class=Normal style="width: 458px">
    <tr>
        <td align="right" class="TableRowSeperator">
            <b>Insurer:</b></td>
        <td colspan="3" class="TableRowSeperator">
            <asp:DropDownList ID="ddInsurer" Runat="server" DataSourceID="oHOCInsurer" DataMember="DefaultView"
                DataTextField="HOCInsurerDescription" DataValueField="HOCInsurerNumber">
            </asp:DropDownList>
            <asp:Label ID="lbErrorMsg" runat="server" CssClass="ErrorMsg" EnableViewState="False"
                Text='"<<" Indicates an invalid value' Visible="False"></asp:Label></td>
    </tr>
    <tr>
        <td align="right" class="TableRowSeperator"  >
            <b>Policy #:</b></td>
        <td class="TableRowSeperator"  >
            <asp:TextBox ID="tbPolicyNumber" Runat="server" Width="93px"></asp:TextBox>
        </td>
        <td align="right" class="TableRowSeperator"  >
            <b>Thatch Valuation:</b>
            </td>
        <td class="TableRowSeperator"  >
            <asp:TextBox ID="tbThatchAmount" Runat="server" Width="70px" CausesValidation="True" MaxLength="9"></asp:TextBox><asp:RangeValidator
                ID="rvThatch" runat="server" ControlToValidate="tbThatchAmount" CssClass="ErrorMsg"
                Display="Dynamic" EnableClientScript="False" ErrorMessage="<<" MaximumValue="99999999"
                MinimumValue="0" Type="Currency"></asp:RangeValidator>
        </td>
    </tr>
    <tr>
        <td align="right" class="TableRowSeperator" >
            <b>Premium:</b></td>
        <td class="TableRowSeperator" >
            <asp:TextBox ID="tbPremium" Runat="server" Width="70px" readonlyForUndo="true" CausesValidation="True" MaxLength="5"></asp:TextBox><asp:RangeValidator
                ID="rvPremium" runat="server" ControlToValidate="tbPremium" CssClass="ErrorMsg"
                Display="Dynamic" EnableClientScript="False" ErrorMessage="<<" MaximumValue="99999"
                MinimumValue="0" Type="Currency"></asp:RangeValidator>
        </td>
        <td align="right" class="TableRowSeperator" >
            <b>Conv. Valuation:</b></td>
        <td class="TableRowSeperator" >
            <asp:TextBox ID="tbConvAmount" Runat="server" Width="70px" CausesValidation="True" MaxLength="9"></asp:TextBox><asp:RangeValidator
                ID="rvConventional" runat="server" ControlToValidate="tbConvAmount" CssClass="ErrorMsg"
                Display="Dynamic" EnableClientScript="False" ErrorMessage="<<" MaximumValue="99999999"
                MinimumValue="0" Type="Currency"></asp:RangeValidator>
        </td>
    </tr>
    <tr>
        <td align="right" class="TableRowSeperator"  >
            <b>Subsidence:</b>
        </td>
        <td class="TableRowSeperator"  >
            <asp:DropDownList ID="ddSubsidence" Runat="server" DataValueField="HOCSubsidenceNumber" DataTextField="HOCSubsidenceDescription" DataMember="DefaultView" DataSourceID="oHOCSubsidence" Width="100px">
            </asp:DropDownList>
        </td>
        <td align="right" class="TableRowSeperator" >
            <b>Shingle Valuation:</b></td>
        <td class="TableRowSeperator"  >
            <asp:TextBox ID="tbShingleAmount" Runat="server" Width="70px" CausesValidation="True" MaxLength="9"></asp:TextBox><asp:RangeValidator
                ID="rvShingle" runat="server" ControlToValidate="tbShingleAmount" CssClass="ErrorMsg"
                Display="Dynamic" EnableClientScript="False" ErrorMessage="<<" MaximumValue="99999999"
                MinimumValue="0" Type="Currency"></asp:RangeValidator>
        </td>
    </tr>
    <tr>
        <td align="right" class="TableRowSeperator" >
            <b>Construction:</b></td>
        <td class="TableRowSeperator" >
            <asp:DropDownList ID="ddConstruction" Runat="server" DataValueField="HOCConstructionNumber" DataTextField="HOCConstructionDescription" DataMember="DefaultView" DataSourceID="oHOCConstruction" Width="100px">
            </asp:DropDownList>
        </td>
        <td align="right" class="TableRowSeperator" >
            <b>Tot. Sum Insured:</b></td>
        <td class="TableRowSeperator" ><asp:TextBox ID="tbTotSumInsured" Runat="server" Width="70px" CausesValidation="True" MaxLength="9"></asp:TextBox><asp:RangeValidator
                ID="rvTotInsured" runat="server" ControlToValidate="tbTotSumInsured" CssClass="ErrorMsg"
                Display="Dynamic" EnableClientScript="False" ErrorMessage="<<" MaximumValue="99999999"
                MinimumValue="0" Type="Currency"></asp:RangeValidator>
        </td>
    </tr>
    <tr>
        <td align="right" class="TableRowSeperator" >
            <b>Roof Type:</b></td>
        <td class="TableRowSeperator" >
            <asp:DropDownList ID="ddRoofType" Runat="server" DataValueField="HOCRoofNumber" DataTextField="HOCRoofDescription" DataMember="DefaultView" DataSourceID="oHOCRoofType" Width="100px">
            </asp:DropDownList>
        </td>
    </tr>
</table>
</asp:Panel>
<asp:Panel ID="pnlDisplay" Runat="server">
<asp:Repeater ID="HOCRepeater" Runat="server" DataSourceID="oHOC">
<ItemTemplate>
<table cellspacing="0" cellpadding="2" align=center width=400 class=Normal>
    <tr>
        <td nowrap="nowrap" align="right" class="TableRowSeperator" >
            <b>Insurer:</b></td>
        <td colspan="4" class="TableRowSeperator" ><%# DataBinder.Eval(Container.DataItem, "HOCInsurerDescription") %></td>
    </tr>
    <tr>
        <td nowrap="nowrap" align="right" class="TableRowSeperator" >
            <b>Policy #:</b></td>
        <td class="TableRowSeperator">
            <%# DataBinder.Eval(Container.DataItem, "HOCPolicyNumber") %></td>
        <td class="TableRowSeperator">&nbsp;
        </td>
        <td nowrap="nowrap" align="right" class="TableRowSeperator">
            <b>Thatch Valuation:</b></td>
        <td ><%# Convert.ToDouble(DataBinder.Eval(Container.DataItem, "HOCThatchAmount")).ToString(Constants.CURRENCY_FORMAT) %>
            </td>
    </tr>
    <tr>
        <td nowrap="nowrap" align="right" class="TableRowSeperator" >
            <b>Premium:</b></td>
        <td  class="TableRowSeperator"><%# Convert.ToDouble(DataBinder.Eval(Container.DataItem, "HOCMonthlyPremium")).ToString(Constants.CURRENCY_FORMAT) %>
        </td>
        <td class="TableRowSeperator">&nbsp;
        </td>
        <td nowrap="nowrap" align="right" class="TableRowSeperator" >
            <b>Conv. Valuation:</b></td>
        <td class="TableRowSeperator" ><%# Convert.ToDouble(DataBinder.Eval(Container.DataItem, "HOCConventionalAmount")).ToString(Constants.CURRENCY_FORMAT) %>
        </td>
    </tr>
    <tr>
        <td nowrap="nowrap" align="right" class="TableRowSeperator">
            <b>Subsidence:</b>
        </td>
        <td class="TableRowSeperator">
            <%# DataBinder.Eval(Container.DataItem, "HOCSubsidenceDescription") %></td>
        <td class="TableRowSeperator"  >&nbsp;
        </td><td align="right" class="TableRowSeperator" nowrap=nowrap >
            <b>Shingle Valuation:</b></td>
        <td class="TableRowSeperator"><%# Convert.ToDouble(DataBinder.Eval(Container.DataItem, "HOCShingleAmount")).ToString(Constants.CURRENCY_FORMAT) %>
            </td>
    </tr>
    <tr>
        <td nowrap="nowrap" align="right" class="TableRowSeperator">
            <b>Construction:</b></td>
        <td class="TableRowSeperator">
            <%# DataBinder.Eval(Container.DataItem, "HOCConstructionDescription") %></td>
        <td class="TableRowSeperator" >&nbsp;
        </td><td nowrap="nowrap" align="right" class="TableRowSeperator">
            <b>Tot. Sum Insured:</b></td>
        <td class="TableRowSeperator"><%# Convert.ToDouble(DataBinder.Eval(Container.DataItem, "HOCTotalSumInsured")).ToString(Constants.CURRENCY_FORMAT) %>
            </td>
    </tr>
    <tr>
        <td nowrap="nowrap" align="right" class="TableRowSeperator">
            <b>Roof Type:</b></td>
        <td class="TableRowSeperator">
            <%# DataBinder.Eval(Container.DataItem, "HOCRoofDescription") %></td>
        <td class="TableRowSeperator">&nbsp;
        </td>
        <td align="right" class="TableRowSeperator">&nbsp;
            </td>
        <td class="TableRowSeperator">&nbsp;
            </td>
    </tr>
</table>
</ItemTemplate>
</asp:Repeater></asp:Panel>
<asp:SqlDataSource ID="oHOCInsurer" Runat="server" SelectCommand="select 1 grp,* from sahldb..hocinsurer&#10;where HOCInsurerNumber < 3&#10;union&#10;select 2 grp,* from sahldb..hocinsurer&#10;where HOCInsurerNumber > 2&#10;order by grp,HOCInsurerDescription">
</asp:SqlDataSource>
<asp:SqlDataSource ID="oHOCSubsidence" SelectCommand="&#10;SELECT * from SAHLDB..HOCSubsidence&#10;&#10;"
    Runat="server">
</asp:SqlDataSource>
<asp:SqlDataSource ID="oHOCConstruction" SelectCommand="SELECT * from  SAHLDB..HOCConstruction"
    Runat="server">
</asp:SqlDataSource>
<asp:SqlDataSource ID="oHOCRoofType" SelectCommand="SELECT * from  SAHLDB..HOCRoof"
    Runat="server">
</asp:SqlDataSource>
<asp:SqlDataSource ID="oHOC" SelectCommand="select hocinsurer.hocinsurernumber,&#10;       hocinsurer.hocinsurerdescription,&#10;       hoc.HOCPolicyNumber,&#10;       isNull(hoc.hocMonthlyPremium,0) hocMonthlyPremium,&#10;       isNull(hoc.hocthatchamount,0) hocthatchamount,&#10;       isNull(hoc.hocconventionalamount,0) hocconventionalamount,&#10;       isNull(hoc.hocshingleamount,0) hocshingleamount,&#10;       isNull(hoc.hoctotalsuminsured,0) hoctotalsuminsured,&#10;       hocsubsidence.hocsubsidencenumber,&#10;       hocsubsidence.hocsubsidencedescription,&#10;       hocroof.hocroofnumber,&#10;       hocroof.hocroofdescription,&#10;       hocconstruction.hocconstructionnumber,&#10;       hocconstruction.hocconstructiondescription&#10;from sahldb..hoc hoc&#10;inner join sahldb..hocinsurer hocinsurer on hocinsurer.hocinsurernumber = hoc.hocinsurernumber&#10;inner join sahldb..hocsubsidence hocsubsidence on hocsubsidence.hocsubsidencenumber = hoc.hocsubsidencenumber&#10;inner join sahldb..hocroof hocroof on hocroof.hocroofnumber = hoc.hocroofnumber&#10;inner join sahldb..hocconstruction hocconstruction on hocconstruction.hocconstructionnumber = hoc.hocconstructionnumber&#10;where loannumber = @loannumber"
    Runat="server" UpdateCommand="update sahldb..hoc&#10;set hocinsurernumber = @insurer ,&#10;HOCPolicyNumber = @policynum,&#10;hocMonthlyPremium = @monthlypremium,&#10;hocthatchamount = @thatchamount,&#10;hocconventionalamount = @conventionalamount,&#10;hocshingleamount = @shingleamount,&#10;hoctotalsuminsured = @totsuminsured,&#10;hocsubsidencenumber = @subsidencenumber,&#10;hocroofnumber = @roofnumber,&#10;hocconstructionnumber = @constructionnumber&#10;where loannumber = @loannumber">
    <UpdateParameters>
        <asp:ControlParameter Name="insurer" ControlID="ddInsurer" PropertyName="SelectedValue"></asp:ControlParameter>
        <asp:ControlParameter Name="policynum" ControlID="tbPolicyNumber" PropertyName="Text"></asp:ControlParameter>
        <asp:ControlParameter Name="monthlypremium" ControlID="tbPremium" PropertyName="Text"></asp:ControlParameter>
        <asp:ControlParameter Name="thatchamount" ControlID="tbThatchAmount" PropertyName="Text"></asp:ControlParameter>
        <asp:ControlParameter Name="conventionalamount" ControlID="tbConvAmount" PropertyName="Text"></asp:ControlParameter>
        <asp:ControlParameter Name="shingleamount" ControlID="tbShingleAmount" PropertyName="Text"></asp:ControlParameter>
        <asp:ControlParameter Name="totsuminsured" ControlID="tbTotSumInsured" PropertyName="Text"></asp:ControlParameter>
        <asp:ControlParameter Name="subsidencenumber" ControlID="ddSubsidence" PropertyName="SelectedValue"></asp:ControlParameter>
        <asp:ControlParameter Name="roofnumber" ControlID="ddRoofType" PropertyName="SelectedValue"></asp:ControlParameter>
        <asp:ControlParameter Name="constructionnumber" ControlID="ddConstruction" PropertyName="SelectedValue"></asp:ControlParameter>
        <asp:QueryStringParameter Name="loannumber" DefaultValue="0" QueryStringField="param0"></asp:QueryStringParameter>
    </UpdateParameters>
    <SelectParameters>
        <asp:QueryStringParameter Name="loannumber" DefaultValue="0" QueryStringField="param0"></asp:QueryStringParameter>
    </SelectParameters>
</asp:SqlDataSource>


