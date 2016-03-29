<%@ Import Namespace="SAHL.Internet.Components.Other" %>
<%@ Control Language="C#" AutoEventWireup="true" Codebehind="SpectacularAdCampaign.ascx.cs"
    Inherits="SAHL.Internet.Components.Other.SpectacularAdCampaign" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<style type="text/css">

pre {
color:#58595B;
font-family:"Trebuchet MS",Trebuchet,Arial,Helvetica,sans-serif;
font-size:17px;
margin:0;
white-space:normal;
}

p {
font-size:14px;
line-height:16px;
margin: 0;
}

.radio{
	font-size:18px;
	line-height:16px;
	margin:0 0 10px 0;
	color: #999999;
}

.email{
	font-size:18px;
	line-height:16px;
	margin:0 0 0 2px;
	padding: 10px 0 10px 0;
	color: #999999;
}

label{
	font-size:18px;
	*font-size:18px;
	_font-size:18px;
	line-height:16px;
	margin:0 0 20px 0;
	color: #999999;
}

input {
	font-size:16px;
	color: #f37021;
	line-height:16px;
	
}

br {
	line-height:10px;
}


</style>

<script type="text/javascript">


function ClearInput(value, id){
	var input = document.getElementById(id);
	
	if(value == input.value){
		input.value = '';
	}else{
		input.value = input.value;
	}
}

function insertInput(value, id){
	var input = document.getElementById(id);
	
	if(input.value == ''){
		input.value = value;
	}
}

function validateForm() {
	if ((document.Form.sahl_checkbox1.checked == true) || (document.Form.sahl_checkbox2.checked == true)) {
		if ((document.Form.email.value == '') || (document.Form.email.value == 'youremail@domain.co.za')) {
			alert('Please enter a valid email address');
		} else {
			document.Form.submit();
		}
	} else {
		document.Form.submit();
	}
}
</script>

<asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlWebApplicationFormSummary" runat="server" Visible="true">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td align="left">
                        <div class="inner-breadcrumb">
                            <a id="A2" class="inner-breadcrumb-title" runat="server" href="">
                              <h4> 
                              CHOOSE WHAT HAPPENS NEXT!</h4>
                            </a>
                        </div>
                        <br />
                        &nbsp;
                        <div>
                            <img src="../../images/SAHomeLoans/Advertising/Spectacular/what_happens_banner.jpg"
                                width="910" height="215" /></div>
                    </td>
                </tr>
                <tr>
                    <td class="rowspacer">
                    </td>
                </tr>
                <tr>
                    <td class="rowspacer">
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <span class="radio">You can't always choose what happens when things change in your
                            life. But with SA Home Loans, you have more options than you thought, because we
                            offer flexible home loans that meet your changing needs.
                            <br />
                            <br />
                            <b style="font-size: 18px;">So you choose: what happens next with this family?</b>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td class="rowspacer">
                        <br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="redline">
                    </td>
                </tr>
                <tr>
                    <td class="rowspacer">
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RadioButtonList CssClass="radio" ID="criteria" runat="server">
                            <asp:ListItem Selected="True" Value="0">Gran loses her knitting as the dog jumps on her lap.</asp:ListItem>
                            <asp:ListItem Value="1">The kids, the dog and gran take over the couch.</asp:ListItem>
                            <asp:ListItem Value="2"> Dad moves out of the lounge and mows the lawn.</asp:ListItem>
                            <asp:ListItem Value="3">Dad lies down with his head on mom's lap - and gran stands along side - looking disgruntled.</asp:ListItem>
                            <asp:ListItem Value="4">Dad sits on the couch next to mom - and leaves gran sitting on the side table  holding the fishbowl.</asp:ListItem>
                            <asp:ListItem Value="5">Dad starts knitting and the dog jumps on the couch and everyone leaves.</asp:ListItem>
                            <asp:ListItem Value="6">Gran and the dog take over the couch. Mom is on the floor. Dad and the kids leave.</asp:ListItem>
                            <asp:ListItem Value="7">Kids take over the couch and destroy gran's knitting.</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td class="rowspacer">
                    </td>
                </tr>
                <tr>
                    <td class="redline">
                    </td>
                </tr>
                <tr>
                    <td class="rowspacer">
                    </td>
                </tr>
                <tr>
                    <td height="24" align="left">
                        <br />
                        <asp:CheckBox ID="chkFeedback" runat="server" Checked="True" Text="Do you want us to let you know what finally happened next via email?  " /><br />
                        <br />
                        <asp:CheckBox ID="chkMoreInfo" runat="server" Checked="True" Text="Please email me more information about SA HomeLoans? " /><br />
                        <br />
                        <span id="email_item">Email:
                            <asp:TextBox ID="txtEmail" runat="server" Width="300px">youremail@domain.co.za</asp:TextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td class="rowspacer">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="redline">
                    </td>
                </tr>
                <tr>
                    <td class="rowspacer">
                    </td>
                </tr>
                <tr>
                    <td class="rowspacer">
                        <asp:ImageButton ImageUrl="~/images/SAHomeLoans/Buttons/send.jpg" runat="server"
                            OnCommand="ApplicationFormCommand" ID="btnSubmit"></asp:ImageButton>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="resultsPanel" runat="server" Visible="false">
            <table border="0" cellpadding="0" cellspacing="3" width="100%">
                <tr>
                    <td align="left">
                        <div class="inner-breadcrumb">
                            <a id="A1" class="inner-breadcrumb-title" runat="server" href="">
                                <h4>
                                Thank You For Your Submission</h4>
                                
                            </a>
                        </div>
                        <br />
                        &nbsp;
                        <div>
                            <img src="../../images/SAHomeLoans/Advertising/Spectacular/what_happens_banner.jpg"
                                width="910" height="215" /></div>
                    </td>
                </tr>
                <tr>
                    <td class="rowspacer">
                    </td>
                </tr>
                <tr>
                    <td height="24" align="left" valign="top">
                        <div align="center" style="font-size: 24px; color: #999999;">
                            Thank You For Your Submission
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
