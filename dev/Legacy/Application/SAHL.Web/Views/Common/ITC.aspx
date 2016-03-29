<%@ Page Language="C#" MasterPageFile="~/MasterPages/Blank.Master" AutoEventWireup="true" CodeBehind="ITC.aspx.cs" Inherits="SAHL.Web.Views.Common.ITC" Title="Untitled Page" %>

<%@ Register Assembly="SAHL.Common.Web.UI" Namespace="SAHL.Common.Web.UI.Controls" TagPrefix="SAHL" %>
<%@ Register Assembly="SAHL.Web" Namespace="SAHL.Web.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
<script language="javascript" type="text/javascript">
function checkDoEnquiry()
{
    //debugger;
    
    if (oneChecked())
        return confirm('Are you sure you want to request these ITCs?')
    else
        alert('Please select who you want to do an enquiry on.');
        
    return false;
}

function oneChecked()
{
    if (CheckBoxIDs != null)
    {
        for (var i = 0; i < CheckBoxIDs.length; i++)
        {
            var cb = document.getElementById(CheckBoxIDs[i]);
            if (cb.checked) return true;
        }
    }
    return false;
}
</script>
<%--    <ajaxToolkit:Accordion ID="accApplicationSummary" runat="server" SelectedIndex="-1"
                        HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="" FadeTransitions="false"
                        FramesPerSecond="40" TransitionDuration="250" AutoSize="None" SuppressHeaderPostbacks="true">
        <Panes>
            <ajaxToolkit:AccordionPane ID="apITC" runat="server">
                <Header>
                    <a href="">ITC Detail</a>
                </Header>
                <Content>--%>
                    <asp:Panel ID="pnlGrid" runat="server" Visible="true" Width="99%">
                        <cc1:ITCGrid ID="grdITC" runat="server" GridHeight="330px" OnRowCommand="grdITC_RowCommand">
                        </cc1:ITCGrid>
                    </asp:Panel>
              <%--  </Content>
            </ajaxToolkit:AccordionPane>
        </Panes>
        <Panes>
            <ajaxToolkit:AccordionPane ID="apOtherAccountITC" runat="server" Visible="false">
                <Header>
                    <a href="">Current ITC Requests on other Accounts and Applications</a>
                </Header>
                <Content>--%>
                    <%--<asp:Panel ID="pnlGrid2" runat="server" Width="99%" Visible="false">
                        <cc1:ITCGrid ID="grdITCOtherAccount" runat="server" GridHeight="330px" OnRowCommand="grdITC_RowCommand" ViewHistoryColumnVisible="false" DoEnquiryColumnVisible="false" AccountColumnVisible="true"> 
                        </cc1:ITCGrid>
                    </asp:Panel>--%>
               <%-- </Content>
            </ajaxToolkit:AccordionPane>
        </Panes>
    </ajaxToolkit:Accordion>--%>
    
    
    
    
    
    <br />
    <div class="buttonBar" style="width:99%">
        <SAHL:SAHLButton ID="btnDoEnquiry" runat="server" Text="Do Enquiry" Visible="True" CssClass="buttonSpacer" OnClientClick="return checkDoEnquiry()" />
        <SAHL:SAHLButton ID="btnBack" runat="server" Text="<- Back" Visible="true" CssClass="buttonSpacer" OnClick="btnBack_Click" />
    </div>

</asp:Content>