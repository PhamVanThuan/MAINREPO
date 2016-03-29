<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ErrorPage.aspx.cs" Inherits="Views_ErrorPage" %>
<%@ Register Src="~/Components/Other/ErrorHandler.ascx" TagPrefix="SAHL" TagName="ErrorHandler" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35" Namespace="System.Web.UI" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Sorry, your requested page was not found.</title>
    <link id="_Portals__default_Skins_DNN___SA_Home_Loans_" rel="stylesheet" type="text/css" href="/Portals/_default/Skins/DNN - SA Home Loans/404skin.css" />

	<style type="text/css">
		/* Widget content container */
	   #goog-wm { 
	    background-color: white;
	   	width:100%;
		overflow:hidden;
		padding:10px 0;
	   }
	   .content {
	   border-bottom: 0px;
	   border-left: 0px;
	   border-right: 0px;
	   border-top: 0px;
	   	background-color:#f37021;
		width:100%;
		overflow:hidden;
		}
		/* Heading for "Closest match"
	   #goog-wm h3.closest-match { }

		/* "Closest match" link
	   #goog-wm h3.closest-match a { }

		/* Heading for "Other things" */
	   #goog-wm h3.other-things { }

		/* "Other things" list item */
	   #goog-wm ul li { }

		/* Site search box */
	   #goog-wm li.search-goog { display: list-item; }
   </style>

</head>
<body>
    <div id="Container">
        <div id="main">
	        <!-- header start -->
            <div id="header">
	            <div class="header-b">
		            <!-- logo start -->
		            <strong><a href="http://www.sahomeloans.com/" id="dnn_headerNavControl_A1" class="logo">SAHomeLoans</a></strong>
		            <!-- logo end -->
		            <div class="r-header">
			            <!-- flash start -->
			            <div class="flash"><embed src="/Portals/3/Contact Us.swf" width="477" height="25" scale="showall" play="true" loop="true" menu="true" wmode="Window" quality="1" type="application/x-shockwave-flash"></embed></div>
			            <!-- flash end -->
			            <div style="height: 46px;"><img src="/images/pxl-trans.gif" id="dnn_headerNavControl_Img2" height="45" width="1" /></div>
			            <!-- gray navigation start -->
			            <ul class="g-nav">
				            <li><a href="Home.aspx" id="dnn_headerNavControl_A2" class="home-nav">Home</a></li>
				            <li><a href="/AboutUs/AboutSAHomeLoans.aspx" id="dnn_headerNavControl_A3">About SA Home Loans</a></li>
				            <li><a href="/UsefulInfo/StepbyStepGettingaHomeLoan.aspx" id="dnn_headerNavControl_A4">Useful Info</a></li>
				            <li><a href="/BranchLocator/National.aspx" id="dnn_headerNavControl_A5">Branch Locator</a></li>
				            <li><a href="/GeneralInformation/FAQs.aspx" id="dnn_headerNavControl_A6">FAQs</a></li>
			            </ul>
			            <!-- gray navigation end -->
			            <div style="height: 10px;"><img src="/images/pxl-trans.gif" id="dnn_headerNavControl_Img1" height="10" width="1" /></div>
			            <!-- orange navigation start -->
			            <ul class="o-nav">
				            <li class="first"><a href="/Products/OurHomeLoanProducts.aspx" id="dnn_headerNavControl_A7">PRODUCTS</a></li>
				            <li><a href="/NewPurchase/BuyingaHome.aspx" id="dnn_headerNavControl_A8">BUYING A HOME</a></li>
				            <li><a href="/HomeRefinance/SwitchandSavewithSAHomeLoans.aspx" id="dnn_headerNavControl_A9">HOME REFINANCE</a></li>
				            <li><a href="/Calculators/AffordabilityCalculator.aspx" id="dnn_headerNavControl_A12">CALCULATORS</a></li>
				            <li><a href="/Application/ApplyNow.aspx" id="dnn_headerNavControl_A11">APPLY NOW</a></li>
			            </ul>
			            <!-- orange navigation end -->
		            </div>
	            </div>
            </div>
	        <!-- header end -->
	        <!-- content start -->
            <div id="404content">
	            <!--breadcrumb start -->
	            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td valign="top" align="right">
                            &nbsp;
                        </td>
                    </tr>
                </table>
	            <!--breadcrumb end -->
	            <div class="fullcontent">
		            <div class="full-b-content">
			            <!-- right content start -->
			            <div id="full-right-cell">
							<table border="0" cellpadding="10px" cellspacing="10px" style="width: 100%">
								<tr>
									<td style="height: 250px" align="right" valign="top">
									</td>
									<td style="height: 250px">
										<SAHL:ErrorHandler ID="ErrorHandler1" runat="server" />
									</td>
								</tr>
								<tr>
									<td colspan="2" style="height: 50px">
									</td>
								</tr>
							</table>
						</div>
			
			            <!-- right content end -->
					</div>
		        </div>
            </div>

            <!-- content end -->
	        <!-- footer start -->
            <div id="footer">
	            <ul class="bottom-nav">
					<li><a target="new" href="http://shl.careerjunction.co.za/car/home/default.asp">Careers</a></li>            
		            <li><a href="/WelcometoSAHomeLoans.aspx">Existing Client</a></li>
		            <li><a href="/GeneralInformation/InformationforInvestors.aspx">Business Partners</a></li>
		            <li><a href="/GeneralInformation/PromotionofInformationAct.aspx">Promotion to Access of Information Act</a></li>
		            <li><a href="/GeneralInformation/NationalCreditAct.aspx">National Credit Act</a></li>
		            <li><a href="/Documentation/Downloads/FSBLicense.pdf" target="_blank">Authorised financial services and registered credit provider, FSB No. 2428</a></li>
	            </ul>
	            <div class="wrapp">
		            <ul class="footer-nav">
			            <li><a href="/GeneralInformation/TermsofUse.aspx">Terms of Use</a></li>
			            <li><a href="/GeneralInformation/PrivacyStatement.aspx">Privacy Statement</a></li>
		            </ul>
		            &copy; SA Home Loans 2008. All rights reserved
	            </div>
            </div>
	        <!-- footer end -->
        </div>
    </div>
</body>
</html>