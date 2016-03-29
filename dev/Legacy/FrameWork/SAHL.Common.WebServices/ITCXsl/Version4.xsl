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
      <xsl:for-each select="BureauResponse/TUBureau:DefaultsD701Part1/TUBureau:DefaultD701Part1/TUBureau:WrittenOffDate">
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
	<xsl:variable name="empiricaVersion">
	  <xsl:choose>
        <xsl:when test="BureauResponse/TUBureau:EmpiricaEM05">
          <xsl:value-of select="'3'" />
        </xsl:when>
		<xsl:when test="BureauResponse/TUBureau:EmpiricaEM07">
          <xsl:value-of select="'4'" />
        </xsl:when>
		<xsl:otherwise>
		  <xsl:value-of select="'Unknown'" />
		</xsl:otherwise>
	  </xsl:choose>
    </xsl:variable>
    <html>
      <link href="../../CSS/ITC.css" rel="stylesheet" type="text/css" />
      <body>
        <table width="800px">
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
												Error reported: <xsl:value-of select="BureauResponse/TUBureau:ErrorMessage" /></td>
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
												Error reported: <xsl:value-of select="BureauResponse/TUBureau:ErrorMessage" /></td>
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
												Consumer Enquiry for <xsl:value-of select="$IDNumber" /></td>
                    </tr>
					<tr>
                      <td align="center" class="Label9" colspan="3">
												Empirica Version: <xsl:value-of select="$empiricaVersion" /></td>
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
													<BR /><BR />PLEASE CONTACT THE FORENSICS DEPARTMENT WITH THIS INFORMATION.<BR /><BR /></td>
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
              <!-- ID Verification-->
              <tr>
                <td>
                  <table width="100%">
                    <tr>
                      <td class="Label9" align="center">
						ID Verification for <xsl:value-of select="$IDNumber" /><xsl:value-of select="' '" /><xsl:value-of select="$ITCName" />
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
			  <!-- Hawk ID -->
			  <xsl:if test="BureauResponse/TUBureau:HawkNH05">
			  <tr>
                <td>
                  <table width="100%">
                    <tr>
                      <td class="Label9" align="center" colspan="2">
						Hawk Detail exists for <xsl:value-of select="$IDNumber" /><xsl:value-of select="' '" /><xsl:value-of select="$ITCName" />
					  </td>
                    </tr>
					<xsl:if test="string-length(BureauResponse/TUBureau:HawkNH05/TUBureau:HawkDesc)!=0">
                    <tr>
					  <td class="Label9">
						Description: 
					  </td>
                      <td class="Value8">
						<xsl:value-of select="BureauResponse/TUBureau:HawkNH05/TUBureau:HawkDesc" /><xsl:value-of select="' '" />
					  </td>
                    </tr>
					</xsl:if>
					<xsl:if test="string-length(BureauResponse/TUBureau:HawkNH05/TUBureau:DeceasedDate)!=0">
                    <tr>
					  <td class="Label9">
						Deceased Date: 
					  </td>
                      <td class="Value8">
						<xsl:value-of select="BureauResponse/TUBureau:HawkNH05/TUBureau:DeceasedDate" /><xsl:value-of select="' '" />
					  </td>
                    </tr>
					</xsl:if>
					<xsl:if test="string-length(BureauResponse/TUBureau:HawkNH05/TUBureau:VictimReference)!=0">
                    <tr>
					  <td class="Label9">
						Victim Reference: 
					  </td>
                      <td class="Value8">
						<xsl:value-of select="BureauResponse/TUBureau:HawkNH05/TUBureau:VictimReference" /><xsl:value-of select="' '" />
					  </td>
                    </tr>
					<tr>
					  <td class="Label9">
						Victim Phone: 
					  </td>
                      <td class="Value8">
						<xsl:value-of select="BureauResponse/TUBureau:HawkNH05/TUBureau:VictimTelCode" /><xsl:value-of select="' '" /><xsl:value-of select="BureauResponse/TUBureau:HawkNH05/TUBureau:VictimTelNo" />
					  </td>
                    </tr>
					</xsl:if>
                  </table>
                </td>
              </tr>
			  </xsl:if>
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
                      <td class="Label9">EmpiricaScore (Version <xsl:value-of select="$empiricaVersion" />): </td>
                      <td class="Value8">
                        <xsl:value-of select="BureauResponse/TUBureau:EmpiricaEM05/TUBureau:EmpiricaScore" />
						<xsl:value-of select="BureauResponse/TUBureau:EmpiricaEM07/TUBureau:EmpiricaScore" />
                        <xsl:value-of select="' '" />
                      </td>
                      <xsl:choose>
                        <xsl:when test="string-length(BureauResponse/TUBureau:EmpiricaEM05/TUBureau:ExclusionCode)!=0">
                          <td class="Label9">Exclusion: </td>
                          <td class="Value8" width="500px">
                            <xsl:value-of select="BureauResponse/TUBureau:EmpiricaEM05/TUBureau:ExclusionCode" />
                            <xsl:value-of select="' '" />
                          </td>
                        </xsl:when>
						<xsl:when test="string-length(BureauResponse/TUBureau:EmpiricaEM07/TUBureau:ExclusionCodeDescription)!=0">
                          <td class="Label9">Exclusion: </td>
                          <td class="Value8" width="500px">
                            <xsl:value-of select="BureauResponse/TUBureau:EmpiricaEM07/TUBureau:ExclusionCodeDescription" />
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
					<!-- Empirica Version 3 -->
                    <xsl:if test="string-length(BureauResponse/TUBureau:EmpiricaEM05/TUBureau:ReasonCode1)!=0">
                      <tr>
                        <td class="Label9">Reason 1:</td>
                        <td class="Value8" colspan="3">
                          <xsl:value-of select="BureauResponse/TUBureau:EmpiricaEM05/TUBureau:ReasonCode1" />
                        </td>
                      </tr>
                    </xsl:if>
                    <xsl:if test="string-length(BureauResponse/TUBureau:EmpiricaEM05/TUBureau:ReasonCode2)!=0">
                      <tr>
                        <td class="Label9">Reason 2:</td>
                        <td class="Value8" colspan="3">
                          <xsl:value-of select="BureauResponse/TUBureau:EmpiricaEM05/TUBureau:ReasonCode2" />
                        </td>
                      </tr>
                    </xsl:if>
                    <xsl:if test="string-length(BureauResponse/TUBureau:EmpiricaEM05/TUBureau:ReasonCode3)!=0">
                      <tr>
                        <td class="Label9">Reason 3:</td>
                        <td class="Value8" colspan="3">
                          <xsl:value-of select="BureauResponse/TUBureau:EmpiricaEM05/TUBureau:ReasonCode3" />
                        </td>
                      </tr>
                    </xsl:if>
                    <xsl:if test="string-length(BureauResponse/TUBureau:EmpiricaEM05/TUBureau:ReasonCode4)!=0">
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
					<!-- Empirica Version 4 -->
					<xsl:if test="string-length(BureauResponse/TUBureau:EmpiricaEM07/TUBureau:ExpansionScore)!=0">
                      <tr>
                        <td class="Label9">Expansion Score :</td>
                        <td class="Value8">
                          <xsl:value-of select="BureauResponse/TUBureau:EmpiricaEM07/TUBureau:ExpansionScore" />
                        </td>
						<td class="Label9">Description :</td>
                        <td class="Value8">
                          <xsl:value-of select="BureauResponse/TUBureau:EmpiricaEM07/TUBureau:ExpansionScoreDescription" />
                        </td>
                      </tr>
                    </xsl:if>
					<xsl:for-each select="BureauResponse/TUBureau:EmpiricaEM07/TUBureau:ReasonDescription/TUBureau:string">
						<xsl:if test="position()=1">
						  <tr>
							<td class="Label9" colspan="4">Empirica Reasons:</td>
						  </tr>
						</xsl:if>
						<tr>
                        <td class="Value8" colspan="4">
                          <xsl:value-of select="." />
                        </td>
                      </tr>
					</xsl:for-each>
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
                        <xsl:value-of select="format-number(BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:Judgements1YrBack+
															BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:Judgements2YrsBack+
															BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:JudgementsMoreThen2YrsBack, '#0')" />
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
                        <xsl:value-of select="format-number(BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:Notices1YrBack+
															BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:Notices2YrsBack +
															BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:NoticesMoreThen2YrsBack, '#0')" />
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
                        <xsl:value-of select="format-number(BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:Defaults1YrBack+
															BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:Defaults2YrsBack+               
															BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:DefaultsMoreThen2YrsBack, '#0')" />
                      </td>
                      <td class="Value8" align="right">
                        <xsl:if test="sum(BureauResponse/TUBureau:DefaultsD701Part1/TUBureau:DefaultD701Part1/TUBureau:DefaultAmount[.!=''])&gt;0">
                          <xsl:value-of select="format-number(sum(BureauResponse/TUBureau:DefaultsD701Part1/TUBureau:DefaultD701Part1/TUBureau:DefaultAmount[.!='']), 'R #,###,##0.00')" />
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
                        <xsl:value-of select="format-number(BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:TraceAlerts1YrBack+
															BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:TraceAlerts2YrsBack+
															BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:TraceAlertsMoreThen2YrsBack, '#0')" />
                      </td>
                      <td class="Value8" />
                      <td class="Value8" />
                      <td class="Value8" />
                    </tr>
                    <tr>
                      <td class="Value8">Payment Profile</td>
                      <td class="Value8">
                        <xsl:value-of select="format-number(BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:PaymentProfiles1YrBack+
															BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:PaymentProfiles2YrsBack+
															BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:PaymentProfilesMoreThen2YrsBack, '#0')" />
                      </td>
                      <td class="Value8" align="right">
                        <xsl:value-of select="format-number(sum(BureauResponse/TUBureau:PaymentProfilesP701/TUBureau:PaymentProfileP701/TUBureau:CurrentBalance[.!='']), 'R #,###,##0.00')" />
                      </td>
                      <td class="Value8" />
                      <td class="Value8" />
                    </tr>
                    <tr>
                      <td class="Value8">SAHL Enquiries</td>
                      <td class="Value8">
                        <xsl:value-of select="format-number(BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:OwnEnquiries1YrBack+
															BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:OwnEnquiries2YrsBack+
															BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:OwnEnquiriesMoreThen2YrsBack, '#0')" />
                      </td>
                      <td class="Value8" />
                      <td class="Value8" />
                      <td class="Value8" />
                    </tr>
                    <tr>
                      <td class="Value8">Other Enquiries</td>
                      <td class="Value8">
                        <xsl:value-of select="format-number(BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:OtherEnquiries1YrBack+
															BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:OtherEnquiries2YrsBack+
															BureauResponse/TUBureau:ConsumerCountersNC04/TUBureau:OtherEnquiriesMoreThen2YrsBack, '#0')" />
                      </td>
                      <td class="Value8" />
                      <td class="Value8" />
                      <td class="Value8" />
                    </tr>
                  </table>
                </td>
              </tr>
			  <!-- CCA Summary -->
              <xsl:if test="BureauResponse/TUBureau:CCASummaryMX01">
                <tr>
                  <td>
                    <table width="100%">
                      <xsl:for-each select="BureauResponse/TUBureau:CCASummaryMX01">
                        <tr>
                          <td class="Label9" align="center" colspan="4">
                            CCA Summary Payment Profile for <xsl:value-of select="$IDNumber" /><xsl:value-of select="' '" /><xsl:value-of select="$ITCName" />
                          </td>
                        </tr>
                        <tr>
                          <td class="Label8">Consumer Number</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:ConsumerNo" /></td>
                          <td class="Label8">No of current Open Accounts</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:NoCurrentOpenAccounts" /></td>
                        </tr>
						<tr>
                          <td class="Label8">Total number of Active Accounts</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:TotalActiveAccounts" /></td>
                          <td class="Label8">Current Balance</td>
                          <td class="Value8" align="right"><xsl:value-of select="format-number(TUBureau:CurrentBalance, 'R #,###,##0.00')" /></td>
                        </tr>
						<tr>
                          <td class="Label8">Total number of Closed Accounts in last 24 months</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:TotalClosedAccounts24Mths" /></td>
                          <td class="Label8">Current Balance Indicator D or C</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:CurrentBalanceInd" /></td>
                        </tr>
						<tr>
                          <td class="Label8">Total Number of Adverse Accounts in last 24 months</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:TotalAdverseAccounts24Mths" /></td>
                          <td class="Label8">Current Monthly Installment</td>
                          <td class="Value8" align="right"><xsl:value-of select="format-number(TUBureau:CurrentMonthlyInstallment, 'R #,###,##0.00')" /></td>
                        </tr>
						<tr>
                          <td class="Label8">Highest actual months in arrears in last 24 months on any account</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:HighestActualMonths24Mths" /></td>
                          <td class="Label8">Current Monthly Installment Balance Indicator D or C</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:CurrentMonthlyInstallmentBalInd" /></td>
                        </tr>
						<tr>
                          <td class="Label8">No of current Revolving Accounts</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:NoRevolvingAccounts" /></td>
                          <td class="Label8">Cumulative Arrears amount</td>
                          <td class="Value8" align="right"><xsl:value-of select="format-number(TUBureau:CumulativeArrearsAmount, 'R #,###,##0.00')" /></td>
                        </tr>
						<tr>
                          <td class="Label8">No of current Installment Accounts</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:NoCurrentInstallmentAccounts" /></td>
                          <td class="Label8">Cumulative Arrears amount, Balance Indicator D or C</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:CumulativeArrearsAmountBalanceInd" /></td>
                        </tr>
                      </xsl:for-each>
                    </table>
                  </td>
                </tr>
              </xsl:if>
              <!-- Payment Profile -->
              <tr>
                <td>
                  <table width="100%">
                    <tr>
                      <td class="Label9" align="center" colspan="4">
						Payment Profile for <xsl:value-of select="$IDNumber" /><xsl:value-of select="' '" /><xsl:value-of select="$ITCName" />
					  </td>
                    </tr>
					<!--
			                    <tr>
			                      <td class="Label8">Instalment Total:</td>
			                      <td class="Value8" align="right"><xsl:value-of select="format-number(sum(BureauResponse/TUBureau:PaymentProfilesP701/TUBureau:PaymentProfileP701/TUBureau:Instalment[.!='']), 'R #,###,##0.00')" /></td>
			                      <td class="Label8">Current Balance Total:</td>
			                      <td class="Value8" align="right"><xsl:value-of select="format-number(sum(BureauResponse/TUBureau:PaymentProfilesP701/TUBureau:PaymentProfileP701/TUBureau:CurrentBalance[.!='']), 'R #,###,##0.00')" /></td>
			                    </tr>
					-->
                    <xsl:for-each select="BureauResponse/TUBureau:PaymentProfilesP701/TUBureau:PaymentProfileP701">
                      <tr>
						<td class="Label8">Date</td>
                        <td class="Value8">
                          <xsl:call-template name="format-date">
                            <xsl:with-param name="iDate" select="TUBureau:DateOpened" />
                          </xsl:call-template>
                        </td>
						<td class="Label8">Overdue-amount</td>
                        <td class="Value8" align="right"><xsl:value-of select="format-number(TUBureau:OverdueAmount, 'R #,###,##0.00')" /></td>
					  </tr>
                      <tr>
                        <td class="Label8">Supplier Name</td>
						<td class="Value8">
                          <xsl:value-of select="TUBureau:SupplierName" />
                        </td>
						<td class="Label8">Ownership-type</td>
                        <td class="Value8"><xsl:value-of select="TUBureau:OwnershipType" /></td>
					  </tr>
                      <tr>
                        <td class="Label8">Account Type</td>
						<td class="Value8">
                          <xsl:value-of select="TUBureau:AccountTypeDesc" />
                        </td>
						<td class="Label8">Number of Participants in Joint Loan</td>
                        <td class="Value8"><xsl:value-of select="TUBureau:NumberOfParticipantsInJointLoan" /></td>
					  </tr>
                      <tr>
                        <td class="Label8">Opening Balance</td>
						<td class="Value8" align="right">
                          <xsl:if test="TUBureau:OpeningBalance!=0">
                            <xsl:value-of select="format-number(TUBureau:OpeningBalance, 'R #,###,##0.00')" />
                          </xsl:if>
                        </td>
						<td class="Label8">Payment-type</td>
                        <td class="Value8"><xsl:value-of select="TUBureau:PaymentType" /></td>
					  </tr>
                      <tr>
                        <td class="Label8">Instalment</td>
						<td class="Value8" align="right">
                          <xsl:if test="TUBureau:Instalment!=0">
                            <xsl:value-of select="format-number(TUBureau:Instalment, 'R #,###,##0.00')" />
                          </xsl:if>
                        </td>
						<td class="Label8">Repayment-freq</td>
                        <td class="Value8"><xsl:value-of select="TUBureau:RepaymentFrequency" /></td>
					  </tr>
                      <tr>
						<td class="Label8">Current Balance</td>
                        <td class="Value8" align="right">
                          <xsl:if test="TUBureau:CurrentBalance!=0">
                            <xsl:value-of select="format-number(TUBureau:CurrentBalance, 'R #,###,##0.00')" />
                          </xsl:if>
                        </td>
						<td class="Label8">Deferred-pay-date</td>
                        <td class="Value8">
						  <xsl:call-template name="format-date">
                            <xsl:with-param name="iDate" select="TUBureau:DeferredPaymentDate" />
                          </xsl:call-template>
						</td>
					  </tr>
                      <tr>
						<td class="Label8">Monthly Payment Profile</td>
                        <td class="Value8">
                          <!-- Payment String -->
                          <xsl:for-each select="TUBureau:PaymentHistories/TUBureau:PaymentHistory">
							<xsl:sort select="TUBureau:Date" data-type="number" order="ascending" /> 
							<xsl:value-of select="TUBureau:StatusCode" />
						  </xsl:for-each>
                        </td>
						<td class="Label8">Acc Sold to 3rd Party</td>
                        <td class="Value8"><xsl:value-of select="TUBureau:AccSoldTo3rdParty" /></td>
                      </tr>
					  <tr>
						<td class="Label8"></td>
                        <td class="Value8"></td>
						<td class="Label8">Third Party Name</td>
                        <td class="Value8"><xsl:value-of select="TUBureau:ThirdPartyName" /></td>
                      </tr>
					  <tr>
						<td class="Label8" colspan="4"><hr /></td>
                      </tr>
                    </xsl:for-each>
                  </table>
                </td>
              </tr>
			  <!-- NLR Summary: NLRCounterSeqmentMC01 -->
              <xsl:if test="BureauResponse/TUBureau:NLRCounterSeqmentMC01">
                <tr>
                  <td>
                    <table width="100%">
                      <xsl:for-each select="BureauResponse/TUBureau:NLRCounterSeqmentMC01/TUBureau:NLRCounterSeqmentMC01">
                        <tr>
                          <td class="Label9" align="center" colspan="4">
                            NLR Summary for<xsl:value-of select="$IDNumber" /><xsl:value-of select="' '" /><xsl:value-of select="$ITCName" />
                          </td>
                        </tr>
                        <tr>
                          <td class="Label8">Consumer Number</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:ConsumerNumber" /></td>
                          <td class="Label8">Previous Year Highest Months in arrears</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:PreviousYearHighestMonthsInArrears" /></td>
                        </tr>
						<tr>
                          <td class="Label8">Current Year Enquiries Client</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:CurrentYearEnquiriesClient" /></td>
                          <td class="Label8">All Other Year Enquiries Client</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:AllOtherYearEnquiriesClient" /></td>
                        </tr>
						<tr>
                          <td class="Label8">Current Year Enquiries Other Subscribers</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:CurrentYearEnquiriesOtherSubscribers" /></td>
                          <td class="Label8">All Other Year Enquiries Other Subscribers</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:AllOtherYearEnquiriesOtherSubscribers" /></td>
                        </tr>
						<tr>
                          <td class="Label8">Current Year Positive NLR Loans</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:CurrentYearPositiveNLRLoans" /></td>
                          <td class="Label8">All Other Year Positive NLR Loans</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:AllOtherYearPositiveNLRLoans" /></td>
                        </tr>
						<tr>
                          <td class="Label8">Current Year Highest Months in arrears</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:CurrentYearHighestMonthsInArrears" /></td>
                          <td class="Label8">All other Years Highest Months in arrears</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:AllOtherYearsHighestMonthsInArrears" /></td>
                        </tr>
						<tr>
                          <td class="Label8">Previous Year Enquiries Client</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:PreviousYearEnquiriesClient" /></td>
                          <td class="Label8">Cumulative Instalment Value</td>
                          <td class="Value8" align="right"><xsl:value-of select="format-number(TUBureau:CumulativeInstalmentValue, 'R #,###,##0.00')" /></td>
                        </tr>
						<tr>
                          <td class="Label8">Previous Year Enquiries Other Subscribers</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:PreviousYearEnquiriesOtherSubscribers" /></td>
                          <td class="Label8">Cumulative Outstanding Balance</td>
                          <td class="Value8" align="right"><xsl:value-of select="format-number(TUBureau:CumulativeOutstandingBalance, 'R #,###,##0.00')" /></td>
                        </tr>
						<tr>
                          <td class="Label8">Previous Year Positive NLR Loans</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:PreviousYearPositiveNLRLoans" /></td>
                          <td class="Label8">Worst months in arrears ever</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:WorstMonthInArrearsEver" /></td>
                        </tr>
                      </xsl:for-each>
                    </table>
                  </td>
                </tr>
              </xsl:if>
			  <!-- NLR Summary Payment Profile: NLRSummaryMY01 -->
              <xsl:if test="BureauResponse/TUBureau:NLRSummaryMY01">
                <tr>
                  <td>
                    <table width="100%">
                      <xsl:for-each select="BureauResponse/TUBureau:NLRSummaryMY01">
                        <tr>
                          <td class="Label9" align="center" colspan="4">
                            NLR Summary Payment Profile for<xsl:value-of select="$IDNumber" /><xsl:value-of select="' '" /><xsl:value-of select="$ITCName" />
                          </td>
                        </tr>
                        <tr>
                          <td class="Label8">Consumer Number</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:ConsumerNo" /></td>
                          <td class="Label8">No of current Open Accounts</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:NoCurrentOpenAccounts" /></td>
                        </tr>
						<tr>
                          <td class="Label8">Total number of Active Accounts</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:TotalActiveAccounts" /></td>
                          <td class="Label8">Current Balance</td>
                          <td class="Value8" align="right"><xsl:value-of select="format-number(TUBureau:CurrentBalance, 'R #,###,##0.00')" /></td>
                        </tr>
						<tr>
                          <td class="Label8">Total number of Closed Accounts in last 24 months</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:TotalClosedAccounts24Mths" /></td>
                          <td class="Label8">Current Balance Indicator D or C</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:CurrentBalanceInd" /></td>
                        </tr>
						<tr>
                          <td class="Label8">Total Number of Adverse Accounts in last 24 months</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:TotalAdverseAccounts24Mths" /></td>
                          <td class="Label8">Current Monthly Installment</td>
                          <td class="Value8" align="right"><xsl:value-of select="format-number(TUBureau:CurrentMonthlyInstallment, 'R #,###,##0.00')" /></td>
                        </tr>
						<tr>
                          <td class="Label8">Highest actual months in arrears in last 24 months on any account</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:HighestActualMonths24Mths" /></td>
                          <td class="Label8">Current Monthly Installment Balance Indicator D or C</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:CurrentMonthlyInstallmentBalInd" /></td>
                        </tr>
						<tr>
                          <td class="Label8">No of current Revolving Accounts</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:NoRevolvingAccounts" /></td>
                          <td class="Label8">Cumulative Arrears amount</td>
                          <td class="Value8" align="right"><xsl:value-of select="format-number(TUBureau:CumulativeArrearsAmount, 'R #,###,##0.00')" /></td>
                        </tr>
						<tr>
                          <td class="Label8">No of current Installment Accounts</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:NoCurrentInstallmentAccounts" /></td>
                          <td class="Label8">Cumulative Arrears amount, Balance Indicator D or C</td>
                          <td class="Value8"><xsl:value-of select="TUBureau:CumulativeArrearsAmountBalanceInd" /></td>
                        </tr>
                      </xsl:for-each>
                    </table>
                  </td>
                </tr>
              </xsl:if>
			  <!-- NLR Detailed Payment Profile: NLRAccountsInformationM701 -->
              <tr>
                <td>
                  <table width="100%">
                    <tr>
                      <td class="Label9" align="center" colspan="4">
						NLR Detailed Payment Profile for <xsl:value-of select="$IDNumber" /><xsl:value-of select="' '" /><xsl:value-of select="$ITCName" /></td>
                    </tr>
					<!--
			                    <tr>
			                      <td class="Label8">Instalment Total:</td>
			                      <td class="Value8" align="right"><xsl:value-of select="format-number(sum(BureauResponse/TUBureau:NLRAccountsInformationM701/TUBureau:NLRAccountInformationM701/TUBureau:InstalmentAmount[.!='']), 'R #,###,##0.00')" /></td>
			                      <td class="Label8">Current Balance Total:</td>
			                      <td class="Value8" align="right"><xsl:value-of select="format-number(sum(BureauResponse/TUBureau:NLRAccountsInformationM701/TUBureau:NLRAccountInformationM701/TUBureau:CurrentBalance[.!='']), 'R #,###,##0.00')" /></td>
			                    </tr>
					-->
                    <xsl:for-each select="BureauResponse/TUBureau:NLRAccountsInformationM701/TUBureau:NLRAccountInformationM701">
                      <tr>
						<td class="Label8">Subscriber</td>
                        <td class="Value8"><xsl:value-of select="TUBureau:SubscriberName" /></td>
						<td class="Label8">End use code</td>
                        <td class="Value8"><xsl:value-of select="TUBureau:EndUseCode" /></td>
					  </tr>
					  <tr>
						<td class="Label8">Account Type</td>
                        <td class="Value8"><xsl:value-of select="TUBureau:AccountType" /></td>
						<td class="Label8">Ownership Type</td>
                        <td class="Value8"><xsl:value-of select="TUBureau:OwnershipType" /></td>
					  </tr>
                      <tr>
                        <td class="Label8">Account Open date</td>
						<td class="Value8">
                          <xsl:call-template name="format-date">
                            <xsl:with-param name="iDate" select="TUBureau:AccountOpenDate" />
                          </xsl:call-template>
                        </td>
						<td class="Label8">No. of Participants in Joint Loan</td>
                        <td class="Value8"><xsl:value-of select="TUBureau:NoOfParticipantsJointLoan" /></td>
					  </tr>
                      <tr>
                        <td class="Label8">Loan Amount</td>
						<td class="Value8" align="right"><xsl:value-of select="format-number(TUBureau:LoanAmount, 'R #,###,##0.00')" /></td>
						<td class="Label8">Payment Type</td>
                        <td class="Value8"><xsl:value-of select="TUBureau:PaymentType" /></td>
					  </tr>
                      <tr>
                        <td class="Label8">Instalment amount</td>
						<td class="Value8" align="right"><xsl:value-of select="format-number(TUBureau:InstalmentAmount, 'R #,###,##0.00')" /></td>
						<td class="Label8">Repayment Frequency</td>
                        <td class="Value8"><xsl:value-of select="TUBureau:RepaymentFrequency" /></td>
					  </tr>
                      <tr>
                        <td class="Label8">Current Balance</td>
						<td class="Value8" align="right"><xsl:value-of select="format-number(TUBureau:CurrentBalance, 'R #,###,##0.00')" /></td>
						<td class="Label8">Deferred Payment Date</td>
                        <td class="Value8">
						  <xsl:call-template name="format-date">
                            <xsl:with-param name="iDate" select="TUBureau:DeferredPaymentDate" />
                          </xsl:call-template>
						</td>
					  </tr>
                      <tr>
						<td class="Label8">Balance Overdue</td>
                        <td class="Value8" align="right"><xsl:value-of select="format-number(TUBureau:BalanceOverdue, 'R #,###,##0.00')" /></td>
						<td class="Label8">Acc Sold to 3rd Party</td>
                        <td class="Value8"><xsl:value-of select="TUBureau:AccSoldTo3rdParty" /></td>
                      </tr>
					  <tr>
						<td class="Label8">Payment duration / items</td>
                        <td class="Value8"><xsl:value-of select="TUBureau:PaymentDuration" /></td>
						<td class="Label8">Third Party Name</td>
                        <td class="Value8"><xsl:value-of select="TUBureau:ThirdPartyName" /></td>
                      </tr>
					  <tr>
						<td class="Label8">Worst Payment history</td>
                        <td class="Value8"><xsl:value-of select="TUBureau:WorstPaymentHistory" /></td>
						<td class="Label8">Payment History</td>
                        <td class="Value8">
						  <!-- Payment String --> 
						  <xsl:for-each select="TUBureau:PaymentHistories/TUBureau:PaymentHistory">
							<xsl:sort select="TUBureau:Date" data-type="number" order="ascending" /> 
							<xsl:value-of select="TUBureau:StatusCode" />
						  </xsl:for-each>
						</td>
                      </tr>
					  <tr>
						<td class="Label8" colspan="4"><hr /></td>
                      </tr>
                    </xsl:for-each>
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
                            <xsl:value-of select="TUBureau:JudgmentTypeDesc" /> Details for <xsl:value-of select="$IDNumber" /><xsl:value-of select="' '" /><xsl:value-of select="$ITCName" /></td>
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
                          <td class="Value8" align="right">
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
						<tr>
							<td class="Label8" colspan="7"><hr /></td>
						</tr>
                      </xsl:for-each>
                    </table>
                  </td>
                </tr>
              </xsl:if>
              <!-- Defaults -->
              <xsl:if test="BureauResponse/TUBureau:DefaultsD701Part1/TUBureau:DefaultD701Part1">
                <tr>
                  <td>
                    <table width="100%">
					  <tr>
					    <td class="Label9" align="center" colspan="7">
						  Default data for <xsl:value-of select="$IDNumber" /><xsl:value-of select="' '" /><xsl:value-of select="$ITCName" />
					    </td>
					  </tr>
                      <xsl:for-each select="BureauResponse/TUBureau:DefaultsD701Part1/TUBureau:DefaultD701Part1">
                        <tr>
						  <td class="Label8">Default Type</td>
                          <td class="Value8">
                            <xsl:value-of select="TUBureau:DefaultType" />
                          </td>
                          <td class="Value8" />
                          <td class="Value8" />
                          <td class="Value8" />
                          <td class="Value8" />
                          <td class="Value8" />
                        </tr>
                        <tr>
                          <td class="Label8">Supplier</td>
                          <td class="Value8" colspan="2">
                            <xsl:value-of select="TUBureau:SupplierName" />
                          </td>
                          <td class="Value8" />
                          <td class="Label8">Contact No</td>
                          <td class="Value8" colspan="2">
                            <xsl:value-of select="TUBureau:TelephoneCode" />-<xsl:value-of select="TUBureau:TelephoneNumber" />
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
                          <td class="Label8">Contact</td>
                          <td class="Value8" colspan="2">
                            <xsl:value-of select="TUBureau:ContactName" />
                          </td>
                        </tr>
                        <tr>
                          <td class="Label8">Amount</td>
                          <td class="Value8" colspan="2" align="right">
                            <xsl:value-of select="format-number(TUBureau:DefaultAmount, 'R #,###,##0.00')" />
                          </td>
                          <td class="Value8" />
                          <td class="Label8">Account Number</td>
                          <td class="Value8" colspan="2">
                            <xsl:value-of select="TUBureau:AccountNo" />
                          </td>
                        </tr>
						<tr>
						  <td class="Label8">Written Off Date</td>
                          <td class="Value8" colspan="2">
                            <xsl:call-template name="format-date">
                              <xsl:with-param name="iDate" select="TUBureau:WrittenOffDate" />
                            </xsl:call-template>
                          </td>
                          <td class="Value8" />
                          <td class="Label8">Remarks</td>
                          <td class="Value8" colspan="2">
                            <xsl:value-of select="TUBureau:Remarks1" />
                          </td>
						</tr>
						<xsl:if test="string-length(TUBureau:Remarks2)!=0">
						<tr>
						  <td class="Label8"></td>
                          <td class="Value8" colspan="2"></td>
                          <td class="Value8" />
                          <td class="Label8"></td>
                          <td class="Value8" colspan="2">
							<xsl:value-of select="TUBureau:Remarks2" />
                          </td>
						</tr>
						</xsl:if>
						<tr>
							<td class="Label8" colspan="7"><hr /></td>
						</tr>
                      </xsl:for-each>
                    </table>
                  </td>
                </tr>
              </xsl:if>
              <!-- Notices -->
              <xsl:if test="BureauResponse/TUBureau:NoticesNN08/TUBureau:NoticesNN08">
                <tr>
                  <td>
                    <table width="100%">
					  <tr>
                          <td class="Label9" align="center" colspan="6">
							Consumer Notices for <xsl:value-of select="$IDNumber" /><xsl:value-of select="' '" /><xsl:value-of select="$ITCName" />
						  </td>
                      </tr>
                      <xsl:for-each select="BureauResponse/TUBureau:NoticesNN08/TUBureau:NoticesNN08">
                        <tr>
                          <td class="Value8" colspan="6">
                            <xsl:value-of select="TUBureau:NoticeType" />
                          </td>
                        </tr>
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
                          <td class="Value8" align="right">
                            <xsl:value-of select="format-number(TUBureau:Amount, 'R #,###,##0.00')" />
                          </td>
                          <td class="Value8" />
                          <td class="Value8">
                            <xsl:call-template name="format-date">
                              <xsl:with-param name="iDate" select="TUBureau:CaptureDate" />
                            </xsl:call-template>
                          </td>
                        </tr>
						<tr>
							<td class="Label8" colspan="6"><hr /></td>
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
						<tr>
							<td class="Label8" colspan="6"><hr /></td>
						</tr>
                      </xsl:for-each>
                    </table>
                  </td>
                </tr>
              </xsl:if>
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
                      <td class="Label8">Type Code</td>
                      <td class="Label8">Contact</td>
                    </tr>
                    <xsl:for-each select="BureauResponse/TUBureau:EnquiriesNE50/TUBureau:EnquiryNE50">
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
                          <xsl:value-of select="TUBureau:EnquiryTypeCode" />
                        </td>
                        <td class="Value8">
                          <xsl:value-of select="TUBureau:Contact" />
                        </td>
                      </tr>
                    </xsl:for-each>
                  </table>
                </td>
              </tr>
			  <!-- NLR Enquiry History -->
              <tr>
                <td>
                  <table width="100%">
                    <tr>
                      <td class="Label9" align="center" colspan="5">
						NLR Enquiry History for <xsl:value-of select="$IDNumber" /><xsl:value-of select="' '" /><xsl:value-of select="$ITCName" />
					  </td>
                    </tr>
                    <tr>
                      <td class="Label8">Enquiry Date</td>
                      <td class="Label8">Subscriber</td>
                      <td class="Label8">Type</td>
                      <td class="Label8">Contact</td>
					  <!-- only available on ME01
					  <td class="Label8">Amount</td>
					  -->
                    </tr>
                    <xsl:for-each select="BureauResponse/TUBureau:NLREnquiriesME50/TUBureau:NLREnquiriesME50">
                      <tr>
                        <td class="Value8">
                          <xsl:call-template name="format-date">
                            <xsl:with-param name="iDate" select="TUBureau:EnquiryDate" />
                          </xsl:call-template>
                        </td>
                        <td class="Value8"><xsl:value-of select="TUBureau:EnquirySubscriberName" /></td>
                        <td class="Value8"><xsl:value-of select="TUBureau:EnquiryType" /></td>
                        <td class="Value8"><xsl:value-of select="TUBureau:EnquirySubscriberContact" /></td>
						<!-- only available on ME01
						<td class="Value8"><xsl:value-of select="TUBureau:EnquiryLoanAmount" /></td>
						-->
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