<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:TUBureau="https://secure.transunion.co.za/TUBureau" version="1.0">
	<xsl:output method="html" version="1.0" encoding="UTF-8" indent="yes" />
	<xsl:template match="/">
		<xsl:variable name="IDNumber" select="BureauResponse/TUBureau:ConsumerInfoNO04/TUBureau:IdentityNo1" />
		<xsl:variable name="ITCName">
			<xsl:value-of select="BureauResponse/TUBureau:ConsumerInfoNO04/TUBureau:Forename1" />
			<xsl:value-of select="' '" />
			<xsl:value-of select="BureauResponse/TUBureau:ConsumerInfoNO04/TUBureau:Forename2" />
			<xsl:value-of select="' '" />
			<xsl:value-of select="BureauResponse/TUBureau:ConsumerInfoNO04/TUBureau:Forename3" />
			<xsl:value-of select="' '" />
			<xsl:value-of select="BureauResponse/TUBureau:ConsumerInfoNO04/TUBureau:Surname" />
		</xsl:variable>
		<xsl:variable name="maxDefaultDate">
			<xsl:for-each select="BureauResponse/TUBureau:DefaultsND07/TUBureau:DefaultsND07/TUBureau:WrittenOffDate">
				<xsl:sort data-type="number" order="descending" />
				<xsl:if test="position()=1">
					<xsl:value-of select="." />
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="maxJudgementDate">
			<xsl:for-each select="BureauResponse/TUBureau:JudgementsNJ07/TUBureau:JudgementsNJ07/TUBureau:JudgmentDate">
				<xsl:sort data-type="number" order="descending" />
				<xsl:if test="position()=1">
					<xsl:value-of select="." />
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="maxNoticeDate">
			<xsl:for-each select="BureauResponse/TUBureau:NoticesNN08/TUBureau:NoticesNN08/TUBureau:NoticeDate">
				<xsl:sort data-type="number" order="descending" />
				<xsl:if test="position()=1">
					<xsl:value-of select="." />
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<html>
			<link href="../../CSS/ITC.css" rel="stylesheet" type="text/css" />
			<body>
				<table width="650px">
					<xsl:choose>
						<xsl:when test="BureauResponse/TUBureau:ErrorCode">
							<!-- ERROR Reporting-->
							<tr>
								<td align="center">
									<table width="650px">
										<tr>
											<td align="left" />
											<td align="center">
												<img style="Image1" src="../../images/sahl.jpg" />
											</td>
											<td align="right" />
										</tr>
										<tr>
											<td align="center" height="10" colspan="3" />
										</tr>
										<tr>
											<td align="center" class="TealHeaderCell" colspan="3">
												Error reported: <xsl:value-of select="BureauResponse/TUBureau:ErrorMessage" />
											</td>
										</tr>
									</table>
									<hr class="BigHR" />
								</td>
							</tr>
						</xsl:when>
						<xsl:when test="BureauResponse/TUBureau:ErrorMessage">
							<!-- ERROR Reporting-->
							<tr>
								<td align="center">
									<table width="650px">
										<tr>
											<td align="left" />
											<td align="center">
												<img style="Image1" src="../../images/sahl.jpg" />
											</td>
											<td align="right" />
										</tr>
										<tr>
											<td align="center" height="10" colspan="3" />
										</tr>
										<tr>
											<td align="center" class="TealHeaderCell" colspan="3">
												Error reported: <xsl:value-of select="BureauResponse/TUBureau:ErrorMessage" />
											</td>
										</tr>
									</table>
									<hr class="BigHR" />
								</td>
							</tr>
						</xsl:when>
						<xsl:otherwise>
							<!-- Heading -->
							<tr>
								<td align="center">
									<table width="650px">
										<tr>
											<td align="left" />
											<td align="center">
												<img style="Image1" src="../../images/sahl.jpg" />
											</td>
											<td align="right" />
										</tr>
										<tr>
											<td align="center" height="10" colspan="3" />
										</tr>
										<tr>
											<td align="center" class="TealHeaderCell" colspan="3">
												Consumer Enquiry for <xsl:value-of select="$IDNumber" />
											</td>
										</tr>
									</table>
									<hr class="BigHR" />
								</td>
							</tr>
							<!-- SAFPS -->
							<xsl:if test="BureauResponse/TUBureau:SAFPSNF01">
								<tr>
									<td>
										<table width="100%">
											<tr>
												<td class="Label12Red" align="center" colspan="2">
													SAFPS INDICATED
												</td>
											</tr>
											<tr>
												<td class="Label8Red" colspan="2">
													UNDER NO CIRCUMSTANCES IS THE INFORMATION CONTAINED IN THIS WARNING TO BE DIVULGED TO ANY PERSON OTHER THAN YOUR IMMEDIATE SUPERVISOR OR THE FORENSICS DEPARTMENT.
													<BR /><BR />PLEASE CONTACT THE FORENSICS DEPARTMENT WITH THIS INFORMATION.<BR /><BR />
												</td>
											</tr>
											<tr>
												<td class="Label8Red" width="150px">SAFPS Reference Number:</td>
												<td class="Label8Red">
													<xsl:value-of select="BureauResponse/TUBureau:SAFPSNF01/TUBureau:FPSRefNo" />
												</td>
												<td />
											</tr>
											<tr>
												<td class="Label8Red" width="150px">Name of Member Organisation:</td>
												<td class="Label8Red">
													<xsl:value-of select="BureauResponse/TUBureau:SAFPSNF01/TUBureau:OrganisationDivision" />
												</td>
												<td />
											</tr>
											<tr>
												<td class="Label8Red" width="150px">Contact Telephone:</td>
												<td class="Label8Red">
													<xsl:value-of select="BureauResponse/TUBureau:SAFPSNF01/TUBureau:ContactPhoneCode" />
													<xsl:value-of select="BureauResponse/TUBureau:SAFPSNF01/TUBureau:ContactPhoneNo" />
												</td>
												<td />
											</tr>
											<tr>
												<td class="Label8Red" width="150px">Contact Email:</td>
												<td class="Label8Red">
													<xsl:value-of select="BureauResponse/TUBureau:SAFPSNF01/TUBureau:ContactEmail" />
												</td>
												<td />
											</tr>
										</table>
									</td>
								</tr>
							</xsl:if>
							<!-- Dispute Information -->
							<xsl:if test="BureauResponse/TUBureau:DisputeIndicatorDI01">
								<tr>
									<td>
										<table width="100%">
											<tr>
												<td class="Label12Red" align="center" colspan="7">
													DISPUTE INDICATED
												</td>
											</tr>
											<tr>
												<td class="Label8Red" width="150px">Capture Date</td>
												<td class="Label8Red" width="150px">Expiry Date</td>
												<td />
											</tr>
											<tr>
												<td class="Value8Red">
													<xsl:call-template name="format-date">
														<xsl:with-param name="iDate" select="BureauResponse/TUBureau:DisputeIndicatorDI01/TUBureau:CaptureDate" />
													</xsl:call-template>
												</td>
												<td class="Value8Red">
													<xsl:call-template name="format-date">
														<xsl:with-param name="iDate" select="BureauResponse/TUBureau:DisputeIndicatorDI01/TUBureau:ExpiryDate" />
													</xsl:call-template>
												</td>
												<td />
											</tr>
										</table>
									</td>
								</tr>
							</xsl:if>
							<!-- DEBT REVIEW Information -->
							<xsl:if test="BureauResponse/TUBureau:DebtCounsellingDC01">
								<tr>
									<td>
										<table width="100%">
											<tr>
												<td class="Label12Red" align="center" colspan="7">
													DEBT REVIEW INDICATED
												</td>
											</tr>
											<tr>
												<td class="Label8Red" width="150px">Date</td>
												<td class="Label8Red" width="150px">Description</td>
												<td />
											</tr>
											<tr>
												<td class="Value8Red">
													<xsl:call-template name="format-date">
														<xsl:with-param name="iDate" select="BureauResponse/TUBureau:DebtCounsellingDC01/TUBureau:DebtCounsellingDate" />
													</xsl:call-template>
												</td>
												<td class="Value8Red">
													<xsl:value-of select="BureauResponse/TUBureau:DebtCounsellingDC01/TUBureau:DebtCounsellingDescription" />
												</td>
												<td />
											</tr>
										</table>
									</td>
								</tr>
							</xsl:if>
							<!-- Halo Prospect Information -->
							<tr>
								<td>
									<!-- HALO INFO -->
								</td>
							</tr>
							<!-- Fraud Rating -->
							<tr>
								<td>
									<table width="100%">
										<tr>
											<td class="Label9" align="center">
												Fraud Rating for <xsl:value-of select="$IDNumber" /><xsl:value-of select="' '" /><xsl:value-of select="$ITCName" />
											</td>
										</tr>
										<tr>
											<td class="Label8Italic">Rating</td>
										</tr>
										<tr>
											<td class="Value8">
												Level <xsl:value-of select="BureauResponse/TUBureau:FraudScoreFS01/TUBureau:Rating" />: <xsl:value-of select="BureauResponse/TUBureau:FraudScoreFS01/TUBureau:RatingDescription" />
											</td>
										</tr>
										<tr>
											<td class="Value8">Reasons:</td>
										</tr>
										<xsl:for-each select="BureauResponse/TUBureau:FraudScoreFS01/TUBureau:ReasonDescription/TUBureau:string">
											<tr>
												<td class="Value8">
													<xsl:value-of select="." />
												</td>
											</tr>
										</xsl:for-each>
									</table>
								</td>
							</tr>
							<!-- Hawk ID -->
							<tr>
								<td>
									<table width="100%">
										<tr>
											<td class="Label9" align="center">
												Hawk ID Verification for <xsl:value-of select="$IDNumber" /><xsl:value-of select="' '" /><xsl:value-of select="$ITCName" />
											</td>
										</tr>
										<tr>
											<td class="Value8">
												Identity Number <xsl:value-of select="$IDNumber" /><xsl:value-of select="' '" /><xsl:value-of select="BureauResponse/TUBureau:IdvNI01/TUBureau:IDVerifiedDesc" /><xsl:value-of select="' '" />
												Full Name is <xsl:value-of select="$ITCName" />
											</td>
										</tr>
									</table>
								</td>
							</tr>
							<!-- Empirica Score -->
							<tr>
								<td>
									<table width="100%">
										<tr>
											<td class="Label9" align="center" colspan="4">
												Empirica Score for <xsl:value-of select="$IDNumber" /><xsl:value-of select="' '" /><xsl:value-of select="$ITCName" />
											</td>
										</tr>
										<tr>
											<td class="Label9">EmpiricaScore: </td>
											<td class="Value8">
												<xsl:value-of select="BureauResponse/TUBureau:EmpiricaEM05/TUBureau:EmpiricaScore" />
												<xsl:value-of select="' '" />
											</td>
											<xsl:choose>
												<xsl:when test="string-length(BureauResponse/TUBureau:EmpiricaEM05/TUBureau:FirstReasonCode)!=0">
													<td class="Label9">Exclusion: </td>
													<td class="Value8" width="500px">
														<xsl:value-of select="BureauResponse/TUBureau:EmpiricaEM05/TUBureau:FirstReasonCode" />
														<xsl:value-of select="' '" />
													</td>
												</xsl:when>
												<xsl:otherwise>
													<td class="Label9">
														<xsl:value-of select="' '" />
													</td>
													<td class="Value8" width="500px">
														<xsl:value-of select="' '" />
													</td>
												</xsl:otherwise>
											</xsl:choose>
										</tr>
										<tr>
											<td class="Value8" width="500px">
												<xsl:value-of select="BureauResponse/TUBureau:EmpiricaEM05/TUBureau:FirstReasonCode" />
												<xsl:value-of select="' '" />
											</td>
										</tr>
										<xsl:if test="string-length(BureauResponse/TUBureau:EmpiricaEM05/TUBureau:ExclusionCode1)!=0">
											<tr>
												<td class="Label9">Reason 1:</td>
												<td class="Value8" colspan="3">
													<xsl:value-of select="BureauResponse/TUBureau:EmpiricaEM05/TUBureau:ExclusionCode1" />
												</td>
											</tr>
										</xsl:if>
										<xsl:if test="string-length(BureauResponse/TUBureau:EmpiricaEM05/TUBureau:ExclusionCode2)!=0">
											<tr>
												<td class="Label9">Reason 2:</td>
												<td class="Value8" colspan="3">
													<xsl:value-of select="BureauResponse/TUBureau:EmpiricaEM05/TUBureau:ExclusionCode2" />
												</td>
											</tr>
										</xsl:if>
										<xsl:if test="string-length(BureauResponse/TUBureau:EmpiricaEM05/TUBureau:ExclusionCode3)!=0">
											<tr>
												<td class="Label9">Reason 3:</td>
												<td class="Value8" colspan="3">
													<xsl:value-of select="BureauResponse/TUBureau:EmpiricaEM05/TUBureau:ExclusionCode3" />
												</td>
											</tr>
										</xsl:if>
										<xsl:if test="string-length(BureauResponse/TUBureau:EmpiricaEM05/TUBureau:ExclusionCode4)!=0">
											<tr>
												<td class="Label9">Reason 4:</td>
												<td class="Value8" colspan="3">
													<xsl:value-of select="BureauResponse/TUBureau:EmpiricaEM05/TUBureau:ExclusionCode4" />
												</td>
											</tr>
										</xsl:if>
										<xsl:if test="string-length(BureauResponse/TUBureau:EmpiricaEM05/TUBureau:ExpansionScore)!=0">
											<tr>
												<td class="Label9">Expansion Score :</td>
												<td class="Value8" colspan="3">
													<xsl:value-of select="BureauResponse/TUBureau:EmpiricaEM05/TUBureau:ExpansionScore" />
												</td>
											</tr>
										</xsl:if>
									</table>
								</td>
							</tr>
							<!-- Summary Info -->
							<tr>
								<td>
									<table width="100%">
										<tr>
											<td class="Label9" align="center" colspan="5">
												Summary of Information for <xsl:value-of select="$IDNumber" /><xsl:value-of select="' '" /><xsl:value-of select="$ITCName" />
											</td>
										</tr>
										<tr>
											<td class="Label8">Sub Records</td>
											<td class="Label8">Total No</td>
											<td class="Label8">Total Value</td>
											<td class="Label8" />
											<td class="Label8">Most Recent Date</td>
										</tr>
										<tr>
											<td class="Value8">Judgement(s)</td>
											<td class="Value8">
												<xsl:value-of select="format-number(BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:Judgements1YrBack+                         BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:Judgements2YrsBack+                         BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:JudgementsMoreThen2YrsBack, '#0')" />
											</td>
											<td class="Value8" align="right">
												<xsl:if test="sum(BureauResponse/TUBureau:JudgementsNJ07/TUBureau:JudgementsNJ07/TUBureau:Amount[.!=''])&gt;0">
													<xsl:value-of select="format-number(sum(BureauResponse/TUBureau:JudgementsNJ07/TUBureau:JudgementsNJ07/TUBureau:Amount[.!='']), 'R #,###,##0.00')" />
												</xsl:if>
											</td>
											<td class="Label8" />
											<td class="Value8">
												<xsl:call-template name="format-date">
													<xsl:with-param name="iDate" select="$maxJudgementDate" />
												</xsl:call-template>
											</td>
										</tr>
										<tr>
											<td class="Value8">Notice(s)</td>
											<td class="Value8">
												<xsl:value-of select="format-number(BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:Notices1YrBack+                         BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:Notices2YrsBack +                         BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:NoticesMoreThen2YrsBack, '#0')" />
											</td>
											<td class="Value8" align="right">
												<xsl:if test="sum(BureauResponse/TUBureau:NoticesNN08/TUBureau:NoticesNN08/TUBureau:Amount[.!=''])&gt;0">
													<xsl:value-of select="format-number(sum(BureauResponse/TUBureau:NoticesNN08/TUBureau:NoticesNN08/TUBureau:Amount[.!='']), 'R #,###,##0.00')" />
												</xsl:if>
											</td>
											<td class="Value8" />
											<td class="Value8">
												<xsl:call-template name="format-date">
													<xsl:with-param name="iDate" select="$maxNoticeDate" />
												</xsl:call-template>
											</td>
										</tr>
										<tr>
											<td class="Value8">Default Data</td>
											<td class="Value8">
												<xsl:value-of select="format-number(BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:Defaults1YrBack+                         BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:Defaults2YrsBack+               BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:DefaultsMoreThen2YrsBack, '#0')" />
											</td>
											<td class="Value8" align="right">
												<xsl:if test="sum(BureauResponse/TUBureau:DefaultsND07/TUBureau:DefaultsND07/TUBureau:DefaultAmount[.!=''])&gt;0">
													<xsl:value-of select="format-number(sum(BureauResponse/TUBureau:DefaultsND07/TUBureau:DefaultsND07/TUBureau:DefaultAmount[.!='']), 'R #,###,##0.00')" />
												</xsl:if>
											</td>
											<td class="Value8" />
											<td class="Value8">
												<xsl:call-template name="format-date">
													<xsl:with-param name="iDate" select="$maxDefaultDate" />
												</xsl:call-template>
											</td>
										</tr>
										<tr>
											<td class="Value8">Trace Alert(s)</td>
											<td class="Value8">
												<xsl:value-of select="format-number(BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:TraceAlerts1YrBack+                         BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:TraceAlerts2YrsBack+               BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:TraceAlertsMoreThen2YrsBack, '#0')" />
											</td>
											<td class="Value8" />
											<td class="Value8" />
											<td class="Value8" />
										</tr>
										<tr>
											<td class="Value8">Payment Profile</td>
											<td class="Value8">
												<xsl:value-of select="format-number(BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:PaymentProfiles1YrBack+                         BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:PaymentProfiles2YrsBack+               BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:PaymentProfilesMoreThen2YrsBack, '#0')" />
											</td>
											<td class="Value8" align="right">
												<xsl:value-of select="format-number(sum(BureauResponse/TUBureau:PaymentProfileNP09/TUBureau:PaymentProfileNP09/TUBureau:CurrentBalance[.!='']), 'R #,###,##0.00')" />
											</td>
											<td class="Value8" />
											<td class="Value8" />
										</tr>
										<tr>
											<td class="Value8">SAHL Enquiries</td>
											<td class="Value8">
												<xsl:value-of select="format-number(BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:OwnEnquiries1YrBack+                         BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:OwnEnquiries2YrsBack+               BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:OwnEnquiriesMoreThen2YrsBack, '#0')" />
											</td>
											<td class="Value8" />
											<td class="Value8" />
											<td class="Value8" />
										</tr>
										<tr>
											<td class="Value8">Other Enquiries</td>
											<td class="Value8">
												<xsl:value-of select="format-number(BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:OtherEnquiries1YrBack+                         BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:OtherEnquiries2YrsBack+               BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:OtherEnquiriesMoreThen2YrsBack, '#0')" />
											</td>
											<td class="Value8" />
											<td class="Value8" />
											<td class="Value8" />
										</tr>
									</table>
								</td>
							</tr>
							<!-- Payment Profile -->
							<tr>
								<td>
									<table width="100%">
										<tr>
											<td class="Label9" align="center" colspan="8">
												Payment Profile for <xsl:value-of select="$IDNumber" /><xsl:value-of select="' '" /><xsl:value-of select="$ITCName" />
											</td>
										</tr>
										<tr>
											<td class="Label8">Date</td>
											<td class="Label8">Supplier Name</td>
											<td class="Label8">Account Type</td>
											<td class="Label8">Opening Balance</td>
											<td class="Label8">Instalment</td>
											<td class="Label8">Current Balance</td>
											<td class="Label8" />
											<td class="Label8">Monthly Payment Profile</td>
										</tr>
										<xsl:for-each select="BureauResponse/TUBureau:PaymentProfileNP09/TUBureau:PaymentProfileNP09">
											<tr>
												<td class="Value8">
													<xsl:call-template name="format-date">
														<xsl:with-param name="iDate" select="TUBureau:DateOpened" />
													</xsl:call-template>
												</td>
												<td class="Value8">
													<xsl:value-of select="TUBureau:SupplierName" />
												</td>
												<td class="Value8">
													<xsl:value-of select="TUBureau:AccountTypeDesc" />
												</td>
												<td class="Value8" align="right">
													<xsl:if test="TUBureau:OpeningBalance!=0">
														<xsl:value-of select="format-number(TUBureau:OpeningBalance, 'R #,###,##0.00')" />
													</xsl:if>
												</td>
												<td class="Value8" align="right">
													<xsl:if test="TUBureau:Instalment!=0">
														<xsl:value-of select="format-number(TUBureau:Instalment, 'R #,###,##0.00')" />
													</xsl:if>
												</td>
												<td class="Value8" align="right">
													<xsl:if test="TUBureau:CurrentBalance!=0">
														<xsl:value-of select="format-number(TUBureau:CurrentBalance, 'R #,###,##0.00')" />
													</xsl:if>
												</td>
												<td class="Value8" />
												<td class="Value8">
													<!-- Payment String -->
													<xsl:call-template name="PaymentProfile">
														<xsl:with-param name="arrPaymentProfile" select="." />
													</xsl:call-template>
												</td>
											</tr>
										</xsl:for-each>
										<tr>
											<td class="Value8" />
											<td class="Label8">Total</td>
											<td class="Value8" />
											<td class="Value8" />
											<td class="Label8" align="right">
												<xsl:value-of select="format-number(sum(BureauResponse/TUBureau:PaymentProfileNP09/TUBureau:PaymentProfileNP09/TUBureau:Instalment[.!='']), 'R #,###,##0.00')" />
											</td>
											<td class="Label8" align="right">
												<xsl:value-of select="format-number(sum(BureauResponse/TUBureau:PaymentProfileNP09/TUBureau:PaymentProfileNP09/TUBureau:CurrentBalance[.!='']), 'R #,###,##0.00')" />
											</td>
											<td class="Value8" />
											<td class="Value8" />
										</tr>
									</table>
								</td>
							</tr>
							<!-- Judgements -->
							<xsl:if test="BureauResponse/TUBureau:JudgementsNJ07/TUBureau:JudgementsNJ07">
								<tr>
									<td>
										<table width="100%">
											<xsl:for-each select="BureauResponse/TUBureau:JudgementsNJ07/TUBureau:JudgementsNJ07">
												<tr>
													<td class="Label9" align="center" colspan="7">
														<xsl:value-of select="TUBureau:JudgmentTypeDesc" /> Details for <xsl:value-of select="$IDNumber" /><xsl:value-of select="' '" /><xsl:value-of select="$ITCName" />
													</td>
												</tr>
												<tr>
													<td class="Label8">Date</td>
													<td class="Label8">Nature Of Debt</td>
													<td class="Label8">Plaintiff</td>
													<td class="Label8">
														<xsl:value-of select="TUBureau:JudgmentTypeDesc" /> Amount
													</td>
													<td class="Label8" />
													<td class="Label8">Case Number</td>
													<td class="Label8">Capture Date</td>
												</tr>
												<tr>
													<td class="Value8">
														<xsl:call-template name="format-date">
															<xsl:with-param name="iDate" select="TUBureau:JudgmentDate" />
														</xsl:call-template>
													</td>
													<td class="Value8">
														<xsl:value-of select="TUBureau:NatureOfDebtDesc" />
													</td>
													<td class="Value8">
														<xsl:value-of select="TUBureau:Plaintiff" />
													</td>
													<td class="Value8">
														<xsl:value-of select="format-number(TUBureau:Amount, 'R #,###,##0.00')" />
													</td>
													<td class="Value8" />
													<td class="Value8">
														<xsl:value-of select="TUBureau:CaseNo" />
													</td>
													<td class="Value8">
														<xsl:call-template name="format-date">
															<xsl:with-param name="iDate" select="TUBureau:CaptureDate" />
														</xsl:call-template>
													</td>
												</tr>
												<tr>
													<td class="Label8">Attorney</td>
													<td class="Value8" colspan="6">
														<xsl:value-of select="TUBureau:AttorneyName" />
													</td>
												</tr>
												<tr>
													<td class="Label8">Remarks</td>
													<td class="Value8" colspan="6">
														<xsl:value-of select="TUBureau:Remarks" />
													</td>
												</tr>
											</xsl:for-each>
										</table>
									</td>
								</tr>
							</xsl:if>
							<!-- Defaults -->
							<xsl:if test="BureauResponse/TUBureau:DefaultsND07/TUBureau:DefaultsND07">
								<tr>
									<td>
										<table width="100%">
											<xsl:for-each select="BureauResponse/TUBureau:DefaultsND07/TUBureau:DefaultsND07">
												<tr>
													<td class="Label9" align="center" colspan="7">
														Default data for <xsl:value-of select="$IDNumber" /><xsl:value-of select="' '" /><xsl:value-of select="$ITCName" />
													</td>
												</tr>
												<tr>
													<td class="Value8">
														<xsl:value-of select="TUBureau:DefaultType" />
													</td>
													<td class="Value8" />
													<td class="Value8" />
													<td class="Value8" />
													<td class="Value8" />
													<td class="Value8" />
													<td class="Value8" />
												</tr>
												<tr>
													<td class="Label8">Name</td>
													<td class="Label8">Contact Name</td>
													<td class="Label8">Information Date</td>
													<td class="Label8" />
													<td class="Label8">Default Type</td>
													<td class="Label8">Default Amount</td>
													<td class="Label8">Written Off Date</td>
												</tr>
												<tr>
													<td class="Value8">
														?? ToDo ??
														<xsl:value-of select="TUBureau:SupplierName" />
													</td>
													<td class="Value8">
														<xsl:value-of select="TUBureau:ContactName" />
													</td>
													<td class="Value8">
														<xsl:call-template name="format-date">
															<xsl:with-param name="iDate" select="TUBureau:InformationDate" />
														</xsl:call-template>
													</td>
													<td class="Value8" />
													<td class="Value8">
														<xsl:value-of select="TUBureau:DefaultType" />
													</td>
													<td class="Value8">
														<xsl:value-of select="format-number(TUBureau:DefaultAmount, 'R #,###,##0.00')" />
													</td>
													<td class="Value8">
														<xsl:call-template name="format-date">
															<xsl:with-param name="iDate" select="TUBureau:WrittenOffDate" />
														</xsl:call-template>
													</td>
												</tr>
												<tr>
													<td class="Label8">Supplier</td>
													<td class="Value8" colspan="2">
														<xsl:value-of select="TUBureau:SupplierName" />
													</td>
													<td class="Value8" />
													<td class="Label8">Contact No</td>
													<td class="Value8" colspan="2">
														<xsl:value-of select="TUBureau:TelephoneCode" />-<xsl:value-of select="TUBureau:TelephoneNo" />
													</td>
												</tr>
												<tr>
													<td class="Label8">Submission Date</td>
													<td class="Value8" colspan="2">
														<xsl:call-template name="format-date">
															<xsl:with-param name="iDate" select="TUBureau:InformationDate" />
														</xsl:call-template>
													</td>
													<td class="Value8" />
													<td class="Label8">Account Number</td>
													<td class="Value8" colspan="2">
														<xsl:value-of select="TUBureau:AccountNo" />
													</td>
												</tr>
												<tr>
													<td class="Label8">ID Number</td>
													<td class="Value8" colspan="2">
														<xsl:value-of select="TUBureau:IdentityNo" />
													</td>
													<td class="Value8" />
													<td class="Label8">Remarks</td>
													<td class="Value8" colspan="2">
														<xsl:value-of select="TUBureau:Remarks" />
													</td>
												</tr>
											</xsl:for-each>
										</table>
									</td>
								</tr>
							</xsl:if>
							<!-- HawkNH05 -->
							<xsl:if test="BureauResponse/TUBureau:HawkNH05">
								<tr>
									<td>
										<table width="100%">

											<tr>
												<td class="Label9" align="center" colspan="6">
													Hawk Info for <xsl:value-of select="$IDNumber" /><xsl:value-of select="' '" /><xsl:value-of select="$ITCName" />
												</td>
											</tr>
											<tr>
												<td class="Value8" colspan="6">
													Description: <xsl:value-of select="BureauResponse/TUBureau:HawkNH05/TUBureau:HawkDesc" />.
													Found: <xsl:value-of select="BureauResponse/TUBureau:HawkNH05/TUBureau:HawkFound" />.
												</td>
											</tr>
											<tr>
												<td class="Label8">Subscriber Name</td>
												<td class="Label8" />
												<td class="Label8">Reference</td>
												<td class="Label8">Contact</td>
												<td class="Label8" />
												<td class="Label8">Contact Telephone</td>
											</tr>
											<tr>
												<td class="Value8">
													<xsl:value-of select="BureauResponse/TUBureau:HawkNH05/TUBureau:SubscriberName" />
												</td>
												<td class="Value8" />
												<td class="Value8">
													<xsl:value-of select="BureauResponse/TUBureau:HawkNH05/TUBureau:SubscriberRef" />
												</td>
												<td class="Value8">
													<xsl:value-of select="BureauResponse/TUBureau:HawkNH05/TUBureau:ContactName" />
												</td>
												<td class="Value8" />
												<td class="Value8">
													<xsl:value-of select="BureauResponse/TUBureau:HawkNH05/TUBureau:ContactTelCode" />
													<xsl:value-of select="TUBureau:ContactTelNo" />
												</td>
											</tr>

										</table>
									</td>
								</tr>
							</xsl:if>
							<!-- Notices -->
							<xsl:if test="BureauResponse/TUBureau:NoticesNN08/TUBureau:NoticesNN08">
								<tr>
									<td>
										<table width="100%">
											<xsl:for-each select="BureauResponse/TUBureau:NoticesNN08/TUBureau:NoticesNN08">
												<tr>
													<td class="Label9" align="center" colspan="6">
														Consumer Notices for <xsl:value-of select="$IDNumber" /><xsl:value-of select="' '" /><xsl:value-of select="$ITCName" />
													</td>
												</tr>
												<tr>
													<td class="Value8" colspan="6">
														<xsl:value-of select="TUBureau:NoticeType" />
													</td>
												</tr>
												<xsl:if test="TUBureau:NoticeTypeCode='C'" >
													<tr>
														<td class="Value8Red" colspan="6">
															It is important to note that no credit may be granted to an individual that has been placed under Curatorship,
															unless they are duly assisted or represented by the Curator.
														</td>
													</tr>
												</xsl:if>
												<tr>
													<td class="Label8">Notice Date</td>
													<td class="Label8" />
													<td class="Label8">Applicant</td>
													<td class="Label8">Amount</td>
													<td class="Label8" />
													<td class="Label8">Capture Date</td>
												</tr>
												<tr>
													<td class="Value8">
														<xsl:call-template name="format-date">
															<xsl:with-param name="iDate" select="TUBureau:NoticeDate" />
														</xsl:call-template>
													</td>
													<td class="Value8" />
													<td class="Value8">
														<xsl:value-of select="TUBureau:Applicant" />
													</td>
													<td class="Value8">
														<xsl:value-of select="format-number(TUBureau:Amount, 'R #,###,##0.00')" />
													</td>
													<td class="Value8" />
													<td class="Value8">
														<xsl:call-template name="format-date">
															<xsl:with-param name="iDate" select="TUBureau:CaptureDate" />
														</xsl:call-template>
													</td>
												</tr>
											</xsl:for-each>
										</table>
									</td>
								</tr>
							</xsl:if>
							<!-- TraceAlerts -->
							<xsl:if test="BureauResponse/TUBureau:TraceAlertsNT06/TUBureau:TraceAlertsNT06">
								<tr>
									<td>
										<table width="100%">
											<tr>
												<td class="Label9" align="center" colspan="6">
													Trace Alerts for <xsl:value-of select="$IDNumber" /><xsl:value-of select="' '" /><xsl:value-of select="$ITCName" />
												</td>
											</tr>
											<tr>
												<td class="Label8">Source</td>
												<td class="Label8">Trace Type</td>
												<td class="Label8">Contact Person</td>
												<td class="Label8">Telephone Number</td>
												<td class="Label8" />
												<td class="Label8">Remarks</td>
											</tr>
											<xsl:for-each select="BureauResponse/TUBureau:TraceAlertsNT06/TUBureau:TraceAlertsNT06">
												<tr>
													<td class="Value8">
														<xsl:value-of select="TUBureau:SubscriberName" />
													</td>
													<td class="Value8">
														<xsl:value-of select="TUBureau:TraceTypeDesc" />
													</td>
													<td class="Value8">
														<xsl:value-of select="TUBureau:ContactName" />
													</td>
													<td class="Value8">
														<xsl:value-of select="TUBureau:ContactPhone" />
													</td>
													<td class="Value8" />
													<td class="Value8">
														<xsl:value-of select="TUBureau:Comment1" />
													</td>
												</tr>
												<xsl:if test="TUBureau:Comment2">
													<tr>
														<td class="Value8" />
														<td class="Value8" />
														<td class="Value8" />
														<td class="Value8" />
														<td class="Value8" />
														<td class="Value8">
															<xsl:value-of select="TUBureau:Comment2" />
														</td>
													</tr>
												</xsl:if>
											</xsl:for-each>
										</table>
									</td>
								</tr>
							</xsl:if>
							<!-- JointBank -->
							<!--<xsl:if test="BureauResponse/TUBureau:ConsumerJointBankV04/TUBureau:ConsumerJointBankV04">
                <tr>
                  <td>
                    <table width="100%">
                      <xsl:for-each select="BureauResponse/TUBureau:ConsumerJointBankV04/TUBureau:ConsumerJointBankV04">
                        <tr>
                          <td class="Label9" align="center" colspan="5">
                            Joint Banks Credit Bureau for <xsl:value-of select="$IDNumber" /><xsl:value-of select="' '" /><xsl:value-of select="$ITCName" />
                          </td>
                        </tr>
                        <tr>
                          <td class="Label8" colspan="5">
                            <xsl:value-of select="TUBureau:AccountStatusDescription" />
                          </td>
                        </tr>
                        <tr>
                          <td class="Label8">Action Date</td>
                          <td class="Label8" />
                          <td class="Label8">Subscriber Name</td>
                          <td class="Label8">Account Number</td>
                          <td class="Label8">Capture Date</td>
                        </tr>
                        <tr>
                          <td class="Value8">
                            <xsl:value-of select="TUBureau:ActionDate" />
                          </td>
                          <td class="Value8" />
                          <td class="Value8">
                            <xsl:value-of select="TUBureau:SubscriberName" />
                          </td>
                          <td class="Value8">
                            <xsl:value-of select="TUBureau:AccountNumber" />
                          </td>
                          <td class="Value8">
                            <xsl:value-of select="TUBureau:CaptureDate" />
                          </td>
                        </tr>
                      </xsl:for-each>
                    </table>
                  </td>
                </tr>
              </xsl:if>-->
							<!-- Customer Info -->
							<tr>
								<td>
									<table width="100%">
										<tr>
											<td class="Label9" align="center" colspan="5">
												ITC Customer Information for <xsl:value-of select="$IDNumber" /><xsl:value-of select="' '" /><xsl:value-of select="$ITCName" />
											</td>
										</tr>
										<tr>
											<td class="Label8">Surname</td>
											<td class="Value8">
												<xsl:value-of select="BureauResponse/TUBureau:ConsumerInfoNO04/TUBureau:Surname" />
											</td>
											<td class="Label8">Title</td>
											<td class="Value8">
												<xsl:value-of select="BureauResponse/TUBureau:ConsumerInfoNO04/TUBureau:Title" />
											</td>
										</tr>
										<tr>
											<td class="Label8">Forename(s)</td>
											<td class="Value8">
												<xsl:value-of select="BureauResponse/TUBureau:ConsumerInfoNO04/TUBureau:Forename1" />
												<xsl:value-of select="BureauResponse/TUBureau:ConsumerInfoNO04/TUBureau:Forename2" />
												<xsl:value-of select="BureauResponse/TUBureau:ConsumerInfoNO04/TUBureau:Forename3" />
											</td>
											<td class="Label8">Date of Birth</td>
											<td class="Value8">
												<xsl:call-template name="format-date">
													<xsl:with-param name="iDate" select="BureauResponse/TUBureau:ConsumerInfoNO04/TUBureau:DateOfBirth" />
												</xsl:call-template>
											</td>
										</tr>
										<tr>
											<td class="Label8">SA ID Number</td>
											<td class="Value8">
												<xsl:value-of select="BureauResponse/TUBureau:ConsumerInfoNO04/TUBureau:IdentityNo1" />
											</td>
											<td class="Label8">SA ID Number 2</td>
											<td class="Value8">
												<xsl:value-of select="BureauResponse/TUBureau:ConsumerInfoNO04/TUBureau:IdentityNo2" />
											</td>
										</tr>
										<tr>
											<td class="Label8">Spouse Name</td>
											<td class="Value8">
												<xsl:value-of select="BureauResponse/TUBureau:ConsumerInfoNO04/TUBureau:SpouseName1" />
												<xsl:value-of select="BureauResponse/TUBureau:ConsumerInfoNO04/TUBureau:SpouseName2" />
											</td>
											<td class="Label8">Gender</td>
											<td class="Value8">
												<xsl:value-of select="BureauResponse/TUBureau:ConsumerInfoNO04/TUBureau:Gender" />
											</td>
										</tr>
										<tr>
											<td class="Label8">Marital Status</td>
											<td class="Value8">
												<xsl:value-of select="BureauResponse/TUBureau:ConsumerInfoNO04/TUBureau:MaritalStatusDesc" />
											</td>
											<td class="Label8">No Dependants</td>
											<td class="Value8">
												<xsl:value-of select="BureauResponse/TUBureau:ConsumerInfoNO04/TUBureau:Dependants" />
											</td>
										</tr>
										<tr>
											<td class="Label8">Telephone Nos</td>
											<td class="Value8">
												<xsl:value-of select="BureauResponse/TUBureau:ConsumerInfoNO04/TUBureau:TelephoneNumbers" />
											</td>
											<td class="Label8">Deceased Date</td>
											<td class="Value8">
												<xsl:if test="BureauResponse/TUBureau:ConsumerInfoNO04/TUBureau:DeceasedDate!=0">
													<xsl:call-template name="format-date">
														<xsl:with-param name="iDate" select="BureauResponse/TUBureau:ConsumerInfoNO04/TUBureau:DeceasedDate" />
													</xsl:call-template>
												</xsl:if>
											</td>
										</tr>
									</table>
								</td>
							</tr>
							<!-- AKANames -->
							<xsl:if test="BureauResponse/TUBureau:AKANamesNK04/TUBureau:AKANamesNK04">
								<tr>
									<td>
										<table width="100%">
											<tr>
												<td class="Label9" align="center" colspan="3">
													AKA Names for <xsl:value-of select="$IDNumber" /><xsl:value-of select="' '" /><xsl:value-of select="$ITCName" />
												</td>
											</tr>
											<xsl:for-each select="BureauResponse/TUBureau:AKANamesNK04/TUBureau:AKANamesNK04">
												<tr>
													<td class="Value8">
														<xsl:call-template name="format-date">
															<xsl:with-param name="iDate" select="TUBureau:InformationDate" />
														</xsl:call-template>
													</td>
													<td class="Value8">
														<xsl:value-of select="TUBureau:AKAName" />
													</td>
													<td />
												</tr>
											</xsl:for-each>
										</table>
									</td>
								</tr>
							</xsl:if>
							<!-- Address Info -->
							<tr>
								<td>
									<table width="100%">
										<tr>
											<td class="Label9" align="center" colspan="5">
												Address Information for <xsl:value-of select="$IDNumber" /><xsl:value-of select="' '" /><xsl:value-of select="$ITCName" />
											</td>
										</tr>
										<tr>
											<td class="Label8">Date</td>
											<td class="Label8">Years</td>
											<td class="Label8">Address</td>
											<td class="Label8">Owner / Tenant</td>
										</tr>
										<xsl:for-each select="BureauResponse/TUBureau:AddressNA07/TUBureau:AddressNA07">
											<tr>
												<td class="Value8">
													<xsl:call-template name="format-date">
														<xsl:with-param name="iDate" select="TUBureau:InformationDate" />
													</xsl:call-template>
												</td>
												<td class="Value8">
													<xsl:value-of select="TUBureau:AddressPeriod" />
												</td>
												<td class="Value8">
													<xsl:value-of select="TUBureau:Line1" />
													<xsl:if test="string-length(TUBureau:Line1)!=0">
														<xsl:value-of select="', '" />
													</xsl:if>
													<xsl:value-of select="TUBureau:Line2" />
													<xsl:if test="string-length(TUBureau:Line2)!=0">
														<xsl:value-of select="', '" />
													</xsl:if>
													<xsl:value-of select="TUBureau:Suburb" />
													<xsl:if test="string-length(TUBureau:Suburb)!=0">
														<xsl:value-of select="', '" />
													</xsl:if>
													<xsl:value-of select="TUBureau:City" />
													<xsl:if test="string-length(TUBureau:City)!=0">
														<xsl:value-of select="', '" />
													</xsl:if>
													<xsl:value-of select="TUBureau:PostalCode" />
													<xsl:if test="string-length(TUBureau:PostalCode)!=0">
														<xsl:value-of select="' '" />
													</xsl:if>
													<xsl:value-of select="TUBureau:ProvinceCode" />
													<xsl:if test="string-length(TUBureau:ProvinceCode)!=0">
														<xsl:value-of select="' '" />
													</xsl:if>
													<xsl:value-of select="TUBureau:Province" />
												</td>
												<td class="Value8">
													<xsl:value-of select="TUBureau:OwnerTenant" />
												</td>
											</tr>
										</xsl:for-each>
									</table>
								</td>
							</tr>
							<!-- Employment History -->
							<tr>
								<td>
									<table width="100%">
										<tr>
											<td class="Label9" align="center" colspan="5">
												Employment History for <xsl:value-of select="$IDNumber" /><xsl:value-of select="' '" /><xsl:value-of select="$ITCName" />
											</td>
										</tr>
										<tr>
											<td class="Label8">Date</td>
											<td class="Label8">Occupation</td>
											<td class="Label8">Employer Name</td>
											<td class="Label8">Employment Period</td>
										</tr>
										<xsl:for-each select="BureauResponse/TUBureau:EmploymentNM04/TUBureau:EmploymentNM04">
											<tr>
												<td class="Value8">
													<xsl:call-template name="format-date">
														<xsl:with-param name="iDate" select="TUBureau:InformationDate" />
													</xsl:call-template>
												</td>
												<td class="Value8">
													<xsl:value-of select="TUBureau:Occupation" />
												</td>
												<td class="Value8">
													<xsl:value-of select="TUBureau:EmployerName" />
												</td>
												<td class="Value8">
													<xsl:value-of select="TUBureau:EmploymentPeriod" />
												</td>
											</tr>
										</xsl:for-each>
									</table>
								</td>
							</tr>
							<!-- Enquiry History Info -->
							<tr>
								<td>
									<table width="100%">
										<tr>
											<td class="Label9" align="center" colspan="5">
												Enquiry History for <xsl:value-of select="$IDNumber" /><xsl:value-of select="' '" /><xsl:value-of select="$ITCName" />
											</td>
										</tr>
										<tr>
											<td class="Label8">Enquiry Date</td>
											<td class="Label8">Subscriber</td>
											<td class="Label8">Type</td>
											<td class="Label8">Contact</td>
											<td class="Label8">Amount</td>
										</tr>
										<xsl:for-each select="BureauResponse/TUBureau:EnquiriesNE09/TUBureau:EnquiriesNE09">
											<tr>
												<td class="Value8">
													<xsl:call-template name="format-date">
														<xsl:with-param name="iDate" select="TUBureau:DateOfEnquiry" />
													</xsl:call-template>
												</td>
												<td class="Value8">
													<xsl:value-of select="TUBureau:Subscriber" />
												</td>
												<td class="Value8">
													<xsl:value-of select="TUBureau:EnquiryType" />
												</td>
												<td class="Value8">
													<xsl:value-of select="TUBureau:Contact" />
												</td>
												<td class="Value8">
													<xsl:value-of select="format-number(TUBureau:EnquiryAmount, 'R #,###,##0.00')" />
												</td>
											</tr>
										</xsl:for-each>
									</table>
								</td>
							</tr>
						</xsl:otherwise>
					</xsl:choose>
				</table>
			</body>
		</html>
	</xsl:template>
	<xsl:template name="PaymentProfile">
		<xsl:param name="arrPaymentProfile" />
		<!-- Payment Profile Date Range -->
		<xsl:variable name="P1" select="-P1-" />
		<xsl:variable name="P2" select="-P2-" />
		<xsl:variable name="P3" select="-P3-" />
		<xsl:variable name="P4" select="-P4-" />
		<xsl:variable name="P5" select="-P5-" />
		<xsl:variable name="P6" select="-P6-" />
		<xsl:variable name="P7" select="-P7-" />
		<xsl:variable name="P8" select="-P8-" />
		<xsl:variable name="P9" select="-P9-" />
		<xsl:variable name="P10" select="-P10-" />
		<xsl:variable name="P11" select="-P11-" />
		<xsl:variable name="P12" select="-P12-" />
		<xsl:variable name="P13" select="-P13-" />
		<xsl:variable name="P14" select="-P14-" />
		<xsl:variable name="P15" select="-P15-" />
		<xsl:variable name="P16" select="-P16-" />
		<xsl:variable name="P17" select="-P17-" />
		<xsl:variable name="P18" select="-P18-" />
		<xsl:variable name="P19" select="-P19-" />
		<xsl:variable name="P20" select="-P20-" />
		<xsl:variable name="P21" select="-P21-" />
		<xsl:variable name="P22" select="-P22-" />
		<xsl:variable name="P23" select="-P23-" />
		<xsl:variable name="P24" select="-P24-" />
		<xsl:variable name="iP1">
			<xsl:for-each select="$arrPaymentProfile/TUBureau:Date/TUBureau:string">
				<xsl:if test=".=$P1">
					<xsl:value-of select="position()" />
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="iP2">
			<xsl:for-each select="$arrPaymentProfile/TUBureau:Date/TUBureau:string">
				<xsl:if test=".=$P2">
					<xsl:value-of select="position()" />
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="iP3">
			<xsl:for-each select="$arrPaymentProfile/TUBureau:Date/TUBureau:string">
				<xsl:if test=".=$P3">
					<xsl:value-of select="position()" />
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="iP4">
			<xsl:for-each select="$arrPaymentProfile/TUBureau:Date/TUBureau:string">
				<xsl:if test=".=$P4">
					<xsl:value-of select="position()" />
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="iP5">
			<xsl:for-each select="$arrPaymentProfile/TUBureau:Date/TUBureau:string">
				<xsl:if test=".=$P5">
					<xsl:value-of select="position()" />
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="iP6">
			<xsl:for-each select="$arrPaymentProfile/TUBureau:Date/TUBureau:string">
				<xsl:if test=".=$P6">
					<xsl:value-of select="position()" />
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="iP7">
			<xsl:for-each select="$arrPaymentProfile/TUBureau:Date/TUBureau:string">
				<xsl:if test=".=$P7">
					<xsl:value-of select="position()" />
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="iP8">
			<xsl:for-each select="$arrPaymentProfile/TUBureau:Date/TUBureau:string">
				<xsl:if test=".=$P8">
					<xsl:value-of select="position()" />
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="iP9">
			<xsl:for-each select="$arrPaymentProfile/TUBureau:Date/TUBureau:string">
				<xsl:if test=".=$P9">
					<xsl:value-of select="position()" />
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="iP10">
			<xsl:for-each select="$arrPaymentProfile/TUBureau:Date/TUBureau:string">
				<xsl:if test=".=$P10">
					<xsl:value-of select="position()" />
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="iP11">
			<xsl:for-each select="$arrPaymentProfile/TUBureau:Date/TUBureau:string">
				<xsl:if test=".=$P11">
					<xsl:value-of select="position()" />
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="iP12">
			<xsl:for-each select="$arrPaymentProfile/TUBureau:Date/TUBureau:string">
				<xsl:if test=".=$P12">
					<xsl:value-of select="position()" />
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="iP13">
			<xsl:for-each select="$arrPaymentProfile/TUBureau:Date/TUBureau:string">
				<xsl:if test=".=$P13">
					<xsl:value-of select="position()" />
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="iP14">
			<xsl:for-each select="$arrPaymentProfile/TUBureau:Date/TUBureau:string">
				<xsl:if test=".=$P14">
					<xsl:value-of select="position()" />
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="iP15">
			<xsl:for-each select="$arrPaymentProfile/TUBureau:Date/TUBureau:string">
				<xsl:if test=".=$P15">
					<xsl:value-of select="position()" />
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="iP16">
			<xsl:for-each select="$arrPaymentProfile/TUBureau:Date/TUBureau:string">
				<xsl:if test=".=$P16">
					<xsl:value-of select="position()" />
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="iP17">
			<xsl:for-each select="$arrPaymentProfile/TUBureau:Date/TUBureau:string">
				<xsl:if test=".=$P17">
					<xsl:value-of select="position()" />
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="iP18">
			<xsl:for-each select="$arrPaymentProfile/TUBureau:Date/TUBureau:string">
				<xsl:if test=".=$P18">
					<xsl:value-of select="position()" />
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="iP19">
			<xsl:for-each select="$arrPaymentProfile/TUBureau:Date/TUBureau:string">
				<xsl:if test=".=$P19">
					<xsl:value-of select="position()" />
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="iP20">
			<xsl:for-each select="$arrPaymentProfile/TUBureau:Date/TUBureau:string">
				<xsl:if test=".=$P20">
					<xsl:value-of select="position()" />
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="iP21">
			<xsl:for-each select="$arrPaymentProfile/TUBureau:Date/TUBureau:string">
				<xsl:if test=".=$P21">
					<xsl:value-of select="position()" />
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="iP22">
			<xsl:for-each select="$arrPaymentProfile/TUBureau:Date/TUBureau:string">
				<xsl:if test=".=$P22">
					<xsl:value-of select="position()" />
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="iP23">
			<xsl:for-each select="$arrPaymentProfile/TUBureau:Date/TUBureau:string">
				<xsl:if test=".=$P23">
					<xsl:value-of select="position()" />
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="iP24">
			<xsl:for-each select="$arrPaymentProfile/TUBureau:Date/TUBureau:string">
				<xsl:if test=".=$P24">
					<xsl:value-of select="position()" />
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<!-- Writing out the payment profile -->
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P1])&gt;0">
			<xsl:value-of select="$arrPaymentProfile/TUBureau:Status/TUBureau:string[position()=$iP1]" />
		</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P1])=0">=</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P2])&gt;0">
			<xsl:value-of select="$arrPaymentProfile/TUBureau:Status/TUBureau:string[position()=$iP2]" />
		</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P2])=0">=</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P3])&gt;0">
			<xsl:value-of select="$arrPaymentProfile/TUBureau:Status/TUBureau:string[position()=$iP3]" />
		</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P3])=0">=</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P4])&gt;0">
			<xsl:value-of select="$arrPaymentProfile/TUBureau:Status/TUBureau:string[position()=$iP4]" />
		</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P4])=0">=</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P5])&gt;0">
			<xsl:value-of select="$arrPaymentProfile/TUBureau:Status/TUBureau:string[position()=$iP5]" />
		</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P5])=0">=</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P6])&gt;0">
			<xsl:value-of select="$arrPaymentProfile/TUBureau:Status/TUBureau:string[position()=$iP6]" />
		</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P6])=0">=</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P7])&gt;0">
			<xsl:value-of select="$arrPaymentProfile/TUBureau:Status/TUBureau:string[position()=$iP7]" />
		</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P7])=0">=</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P8])&gt;0">
			<xsl:value-of select="$arrPaymentProfile/TUBureau:Status/TUBureau:string[position()=$iP8]" />
		</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P8])=0">=</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P9])&gt;0">
			<xsl:value-of select="$arrPaymentProfile/TUBureau:Status/TUBureau:string[position()=$iP9]" />
		</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P9])=0">=</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P10])&gt;0">
			<xsl:value-of select="$arrPaymentProfile/TUBureau:Status/TUBureau:string[position()=$iP10]" />
		</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P10])=0">=</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P11])&gt;0">
			<xsl:value-of select="$arrPaymentProfile/TUBureau:Status/TUBureau:string[position()=$iP11]" />
		</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P11])=0">=</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P12])&gt;0">
			<xsl:value-of select="$arrPaymentProfile/TUBureau:Status/TUBureau:string[position()=$iP12]" />
		</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P12])=0">=</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P13])&gt;0">
			<xsl:value-of select="$arrPaymentProfile/TUBureau:Status/TUBureau:string[position()=$iP13]" />
		</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P13])=0">=</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P14])&gt;0">
			<xsl:value-of select="$arrPaymentProfile/TUBureau:Status/TUBureau:string[position()=$iP14]" />
		</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P14])=0">=</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P15])&gt;0">
			<xsl:value-of select="$arrPaymentProfile/TUBureau:Status/TUBureau:string[position()=$iP15]" />
		</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P15])=0">=</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P16])&gt;0">
			<xsl:value-of select="$arrPaymentProfile/TUBureau:Status/TUBureau:string[position()=$iP16]" />
		</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P16])=0">=</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P17])&gt;0">
			<xsl:value-of select="$arrPaymentProfile/TUBureau:Status/TUBureau:string[position()=$iP17]" />
		</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P17])=0">=</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P18])&gt;0">
			<xsl:value-of select="$arrPaymentProfile/TUBureau:Status/TUBureau:string[position()=$iP18]" />
		</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P18])=0">=</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P19])&gt;0">
			<xsl:value-of select="$arrPaymentProfile/TUBureau:Status/TUBureau:string[position()=$iP19]" />
		</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P19])=0">=</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P20])&gt;0">
			<xsl:value-of select="$arrPaymentProfile/TUBureau:Status/TUBureau:string[position()=$iP20]" />
		</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P20])=0">=</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P21])&gt;0">
			<xsl:value-of select="$arrPaymentProfile/TUBureau:Status/TUBureau:string[position()=$iP21]" />
		</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P21])=0">=</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P22])&gt;0">
			<xsl:value-of select="$arrPaymentProfile/TUBureau:Status/TUBureau:string[position()=$iP22]" />
		</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P22])=0">=</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P23])&gt;0">
			<xsl:value-of select="$arrPaymentProfile/TUBureau:Status/TUBureau:string[position()=$iP23]" />
		</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P23])=0">=</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P24])&gt;0">
			<xsl:value-of select="$arrPaymentProfile/TUBureau:Status/TUBureau:string[position()=$iP24]" />
		</xsl:if>
		<xsl:if test="string-length($arrPaymentProfile/TUBureau:Date/TUBureau:string[.=$P24])=0">=</xsl:if>
	</xsl:template>
	<xsl:template name="format-date">
		<xsl:param name="iDate" />
		<!--<xsl:value-of select="$iDate"/>-->
		<xsl:if test="string-length($iDate)=8">
			<xsl:variable name="Year" select="substring($iDate, 1, 4)" />
			<xsl:variable name="Month" select="substring($iDate, 5, 2)" />
			<xsl:variable name="Day" select="substring($iDate, 7, 2)" />
			<xsl:value-of select="concat($Year, '/', $Month, '/', $Day)" />
		</xsl:if>
		<xsl:if test="string-length($iDate)!=8">
			<xsl:value-of select="$iDate" />
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>