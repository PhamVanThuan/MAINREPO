USE [2AM]

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS
	(SELECT * FROM dbo.SysObjects WHERE ID=Object_ID(N'[test].[InsertITCV4]') AND ObjectProperty(ID,N'IsProcedure')=1)
BEGIN
	DROP PROCEDURE [test].[InsertITCV4]
	PRINT 'DROPPED PROCEDURE [test].[InsertITCV4]'
END
GO
	
/********************************************************************************************************************
PROC INSERTS AN ITC V4 RECORD FOR EACH INDIVIDUAL CLIENT ON THE APPLICATION. THE SBC VALUES DEFAULT TO A CS DECISION
OF ACCEPT WITH THE OPTION TO SET THE EACH VALUE.

REQUIRED PARAMETER:
@OfferKey (application number)
	
CREDIT SCORING SBC INTERVALS:
AT094 (-1 or greater) 
AT107 (-1 or greater) 
AT164 (-1 or greater) 
IN106 (0 to 100) 
IN141 (0 to 100) 
OT007 (0 or greater) 
OT041 (-2 or -1 or greater) 
RE105 (-4 or -1 or 0 to 100)
RE140 (-4 or -3 or -2 or -1 or 0 to 100)
*********************************************************************************************************************/

CREATE PROCEDURE [test].[InsertITCV4]
	
	(
		@OfferKey int,
		@EmpiricaScore varchar(5) = '00720',
		@AT094 varchar(8) = '7',
		@AT107 varchar(8) = '0',
		@AT164 varchar(8) = '40',
		@IN106 varchar(8) = '0',
		@IN141 varchar(8) = '35',
		@OT007 varchar(8) = '1',
		@OT041 varchar(8) = '0',
		@RE105 varchar(8) = '25',
		@RE140 varchar(8) = '25'
	)

AS
BEGIN

DECLARE @AccountKey int,
		@LegalEntityKey int,
		@IDNumber varchar(13),
		@FirstNames varchar(50),
		@Surname varchar(50),
		@xml varchar(max),
		@offertypeKey int
--check the offertype
select @offertypeKey = offertypeKey from [2am]..offer where offerKey = @offerKey

if (@OfferTypeKey = 11)	

begin
	
DECLARE ITCLE CURSOR FOR
	SELECT le.LegalEntityKey FROM [2AM].dbo.Offer o (NoLock)
		JOIN [2AM].dbo.externalRole r (NoLock) ON o.OfferKey=r.genericKey 
		AND r.externalRoleTypeKey = 1
		AND r.genericKeyTypeKey = 2
		JOIN [2AM].dbo.LegalEntity le (NoLock) ON r.LegalEntityKey=le.LegalEntityKey
	WHERE o.OfferKey=@OfferKey	
	
OPEN ITCLE;

end

if (@OfferTypeKey <> 11)	

begin
	
DECLARE ITCLE CURSOR FOR
	SELECT le.LegalEntityKey FROM [2AM].dbo.Offer o (NoLock)
		JOIN [2AM].dbo.OfferRole r (NoLock) ON o.OfferKey=r.OfferKey AND r.OfferRoleTypeKey in (8,10,11,12)
		JOIN [2AM].dbo.LegalEntity le (NoLock) ON r.LegalEntityKey=le.LegalEntityKey
	WHERE o.OfferKey=@OfferKey	
	
OPEN ITCLE;

end




FETCH NEXT FROM ITCLE INTO @LegalEntityKey

WHILE @@FETCH_STATUS = 0

	BEGIN
	
	IF @OfferTypeKey <> 11
	begin
		SELECT @IDNumber=le.IDNumber,@FirstNames=le.FirstNames,
			   @Surname=le.Surname,@AccountKey=o.ReservedAccountKey
		FROM [2AM].dbo.Offer o (NoLock)
			JOIN [2AM].dbo.OfferRole r (NoLock) ON o.OfferKey=r.OfferKey AND r.OfferRoleTypeKey in (8,10,11,12)
			JOIN [2AM].dbo.LegalEntity le (NoLock) ON r.LegalEntityKey=le.LegalEntityKey
		WHERE o.OfferKey=@OfferKey AND le.LegalEntityKey=@LegalEntityKey
	end
	
	IF @OfferTypeKey = 11
	begin
			SELECT @IDNumber=le.IDNumber,@FirstNames=le.FirstNames,
			   @Surname=le.Surname,@AccountKey=o.ReservedAccountKey
		FROM [2AM].dbo.Offer o (NoLock)
			JOIN [2AM].dbo.externalRole r (NoLock) ON o.OfferKey=r.genericKey 
			AND r.externalRoleTypeKey in (1)
			and r.genericKeyTypeKey = 2
			JOIN [2AM].dbo.LegalEntity le (NoLock) 
			ON r.LegalEntityKey=le.LegalEntityKey
		WHERE o.OfferKey=@OfferKey AND le.LegalEntityKey=@LegalEntityKey
	end
		--archive the existing ITC record for the legalentity
		IF EXISTS (SELECT * FROM [2AM].dbo.ITC itc WHERE itc.accountKey=@accountKey AND itc.legalEntityKey=@legalEntityKey)
			BEGIN
				INSERT INTO archive.ITC 
				SELECT	itc.ITCKey, 
						itc.LegalEntityKey, 
						itc.AccountKey, 
						itc.ChangeDate, 
						itc.ResponseXML, 
						itc.ResponseStatus, 
						itc.UserID, 
						'test.InsertITCV4', 
						GetDate(),
						itc.RequestXML
				FROM dbo.ITC itc
				WHERE itc.accountKey=@accountKey AND itc.legalEntityKey=@legalEntityKey
        
				--delete the record
				DELETE FROM dbo.ITC
				WHERE itc.accountKey=@accountKey AND itc.legalEntityKey=@legalEntityKey
			END
	
		SELECT @xml='<BureauResponse xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <RawData xmlns="https://secure.transunion.co.za/TUBureau" />
  <ResponseStatus xmlns="https://secure.transunion.co.za/TUBureau">Success</ResponseStatus>
  <ProcessingStartDate xmlns="https://secure.transunion.co.za/TUBureau">2010-07-07T14:10:17.6855+02:00</ProcessingStartDate>
  <ProcessingTimeSecs xmlns="https://secure.transunion.co.za/TUBureau">6.265625</ProcessingTimeSecs>
  <UniqueRefGuid xmlns="https://secure.transunion.co.za/TUBureau">c6bfe414-5b2a-4c36-85c4-0bba15f2671f</UniqueRefGuid>
  <V1Segment xmlns="https://secure.transunion.co.za/TUBureau">
    <v1Segs>
      <v1Seg>
        <Code>00</Code>
        <Value>01</Value>
      </v1Seg>
      <v1Seg>
        <Code>01</Code>
        <Value>02</Value>
      </v1Seg>
      <v1Seg>
        <Code>NA</Code>
        <Value>07</Value>
      </v1Seg>
      <v1Seg>
        <Code>NC</Code>
        <Value>04</Value>
      </v1Seg>
      <v1Seg>
        <Code>D7</Code>
        <Value>01</Value>
      </v1Seg>
      <v1Seg>
        <Code>NE</Code>
        <Value>50</Value>
      </v1Seg>
      <v1Seg>
        <Code>NJ</Code>
        <Value>07</Value>
      </v1Seg>
      <v1Seg>
        <Code>NK</Code>
        <Value>04</Value>
      </v1Seg>
      <v1Seg>
        <Code>NM</Code>
        <Value>04</Value>
      </v1Seg>
      <v1Seg>
        <Code>NO</Code>
        <Value>04</Value>
      </v1Seg>
      <v1Seg>
        <Code>P7</Code>
        <Value>01</Value>
      </v1Seg>
      <v1Seg>
        <Code>NI</Code>
        <Value>01</Value>
      </v1Seg>
      <v1Seg>
        <Code>NH</Code>
        <Value>05</Value>
      </v1Seg>
      <v1Seg>
        <Code>FS</Code>
        <Value>01</Value>
      </v1Seg>
      <v1Seg>
        <Code>NR</Code>
        <Value>01</Value>
      </v1Seg>
      <v1Seg>
        <Code>NN</Code>
        <Value>08</Value>
      </v1Seg>
      <v1Seg>
        <Code>EM</Code>
        <Value>07</Value>
      </v1Seg>
      <v1Seg>
        <Code>NT</Code>
        <Value>06</Value>
      </v1Seg>
      <v1Seg>
        <Code>V1</Code>
        <Value>01</Value>
      </v1Seg>
      <v1Seg>
        <Code>DI</Code>
        <Value>01</Value>
      </v1Seg>
      <v1Seg>
        <Code>NF</Code>
        <Value>01</Value>
      </v1Seg>
      <v1Seg>
        <Code>DC</Code>
        <Value>01</Value>
      </v1Seg>
      <v1Seg>
        <Code>NX</Code>
        <Value>09</Value>
      </v1Seg>
      <v1Seg>
        <Code>SB</Code>
        <Value>33</Value>
      </v1Seg>
      <v1Seg>
        <Code>MX</Code>
        <Value>01</Value>
      </v1Seg>
      <v1Seg>
        <Code>MC</Code>
        <Value>01</Value>
      </v1Seg>
      <v1Seg>
        <Code>MY</Code>
        <Value>01</Value>
      </v1Seg>
      <v1Seg>
        <Code>M7</Code>
        <Value>01</Value>
      </v1Seg>
      <v1Seg>
        <Code>ME</Code>
        <Value>50</Value>
      </v1Seg>
    </v1Segs>
  </V1Segment>
  <ConsEnqTransInfo0102 xmlns="https://secure.transunion.co.za/TUBureau">
    <DefiniteMatchCount>1</DefiniteMatchCount>
    <PossibleMatchCount>00</PossibleMatchCount>
    <MatchedConsumerNo>596234740</MatchedConsumerNo>
    <PossibleConsumerNo />
    <PossibleAdverseIndicator />
  </ConsEnqTransInfo0102>
  <EchoData0001 xmlns="https://secure.transunion.co.za/TUBureau">
    <SubscriberCode>00151</SubscriberCode>
    <ClientReference>Home Loan Enqui</ClientReference>
    <BranchNumber />
    <BatchNumber />
  </EchoData0001>
  <ConsumerInfoNO04 xmlns="https://secure.transunion.co.za/TUBureau">
    <RecordSeq>01</RecordSeq>
    <Part>001</Part>
    <PartSeq>01</PartSeq>
    <ConsumerNo>596234740</ConsumerNo>
    <Surname>'+@Surname+'</Surname>
    <Forename1>'+@FirstNames+'</Forename1>
    <Forename2 />
    <Forename3 />
    <Title>MR</Title>
    <Gender>M</Gender>
    <NameInfoDate>20100707</NameInfoDate>
    <DateOfBirth>19471208</DateOfBirth>
    <IdentityNo1>'+@IDNumber+'</IdentityNo1>
    <IdentityNo2 />
    <MaritalStatusCode>M</MaritalStatusCode>
    <MaritalStatusDesc>MARRIED</MaritalStatusDesc>
    <Dependants>01</Dependants>
    <SpouseName1>MARIA</SpouseName1>
    <SpouseName2>CHRISTINA</SpouseName2>
    <TelephoneNumbers>H(011)3072250  B(011)3075523</TelephoneNumbers>
    <DeceasedDate>00000000</DeceasedDate>
  </ConsumerInfoNO04>
  <FraudScoreFS01 xmlns="https://secure.transunion.co.za/TUBureau">
    <RecordSequence>01</RecordSequence>
    <Part>001</Part>
    <PartSequence>01</PartSequence>
    <ConsumerNo>596234740</ConsumerNo>
    <Rating>2</Rating>
    <RatingDescription>MINOR IRREGULARITIES - LOW PROBABILITY OF SUSPICIOUS DATA</RatingDescription>
    <ReasonCode>
      <string>BS28</string>
      <string>BS32</string>
      <string>BS36</string>
      <string>BS40</string>
      <string>BS44</string>
    </ReasonCode>
    <ReasonDescription>
      <string>CELLULAR NUMBER ON BUREAU HEADER AND NO CELLULAR NUMBER ON APPLICATION</string>
      <string>WORK TEL NUMBER ON BUREAU HEADER AND NO WORK TEL NUMBER ON APPLICATION</string>
      <string>HOME TEL NUMBER ON BUREAU HEADER AND NO HOME TEL NUMBER ON APPLICATION</string>
      <string>EMPLOYER NAME ON BUREAU HEADER AND NO EMPLOYER NAME ON APPLICATION</string>
      <string>ADDRESS1 ON BUREAU HEADER AND NO ADDRESS1 ON APPLICATION</string>
    </ReasonDescription>
  </FraudScoreFS01>
  <EmpiricaEM07 xmlns="https://secure.transunion.co.za/TUBureau">
    <ConsumerNo>596234740</ConsumerNo>
    <EmpiricaScore>'+@EmpiricaScore+'</EmpiricaScore>
    <ExclusionCode />
    <ExclusionCodeDescription />
    <ReasonCode>
      <string>16</string>
      <string>12</string>
      <string>23</string>
      <string>10</string>
    </ReasonCode>
    <ReasonDescription>
      <string>Serious delinquency or derogatory public records</string>
      <string>Number of accounts with delinquency</string>
      <string>Number of accounts now paid as agreed</string>
      <string>Time since delinquency</string>
    </ReasonDescription>
    <ExpansionScore />
    <ExpansionScoreDescription />
    <EmpiricaVersion>AOV4.0</EmpiricaVersion>
  </EmpiricaEM07>
  <ConsumerCountersNC04 xmlns="https://secure.transunion.co.za/TUBureau">
    <ConsumerNo>596234740</ConsumerNo>
    <OwnEnquiries1YrBack>001</OwnEnquiries1YrBack>
    <OwnEnquiries2YrsBack>000</OwnEnquiries2YrsBack>
    <OwnEnquiriesMoreThen2YrsBack>000</OwnEnquiriesMoreThen2YrsBack>
    <OtherEnquiries1YrBack>003</OtherEnquiries1YrBack>
    <OtherEnquiries2YrsBack>004</OtherEnquiries2YrsBack>
    <OtherEnquiriesMoreThen2YrsBack>000</OtherEnquiriesMoreThen2YrsBack>
    <Judgements1YrBack>000</Judgements1YrBack>
    <Judgements2YrsBack>000</Judgements2YrsBack>
    <JudgementsMoreThen2YrsBack>000</JudgementsMoreThen2YrsBack>
    <Notices1YrBack>000</Notices1YrBack>
    <Notices2YrsBack>000</Notices2YrsBack>
    <NoticesMoreThen2YrsBack>000</NoticesMoreThen2YrsBack>
    <Defaults1YrBack>000</Defaults1YrBack>
    <Defaults2YrsBack>002</Defaults2YrsBack>
    <DefaultsMoreThen2YrsBack>000</DefaultsMoreThen2YrsBack>
    <PaymentProfiles1YrBack>003</PaymentProfiles1YrBack>
    <PaymentProfiles2YrsBack>002</PaymentProfiles2YrsBack>
    <PaymentProfilesMoreThen2YrsBack>000</PaymentProfilesMoreThen2YrsBack>
    <TraceAlerts1YrBack>000</TraceAlerts1YrBack>
    <TraceAlerts2YrsBack>000</TraceAlerts2YrsBack>
    <TraceAlertsMoreThen2YrsBack>000</TraceAlertsMoreThen2YrsBack>
  </ConsumerCountersNC04>
  <IdvNI01 xmlns="https://secure.transunion.co.za/TUBureau">
    <IDVerifiedCode>V0</IDVerifiedCode>
    <IDVerifiedDesc>ID and Surname Verified</IDVerifiedDesc>
    <IDWarning />
    <IDDesc />
    <VerifiedSurname>VENTER</VerifiedSurname>
    <VerifiedForename1>HENDRIK</VerifiedForename1>
    <VerifiedForename2 />
    <DeceasedDate />
  </IdvNI01>
  <AddressNA07 xmlns="https://secure.transunion.co.za/TUBureau">
    <AddressNA07>
      <ConsumerNo>596234740</ConsumerNo>
      <InformationDate>20100428</InformationDate>
      <Line1>29 EDMUND RD</Line1>
      <Line2 />
      <Suburb />
      <City>FLORIDA</City>
      <PostalCode>1709</PostalCode>
      <ProvinceCode />
      <Province />
      <AddressPeriod>00</AddressPeriod>
      <OwnerTenant>O</OwnerTenant>
      <AddressChanged>N</AddressChanged>
    </AddressNA07>
    <AddressNA07>
      <ConsumerNo>596234740</ConsumerNo>
      <InformationDate>20090129</InformationDate>
      <Line1>7 FLOCKFONTAIN ST</Line1>
      <Line2 />
      <Suburb>PELLISSIER</Suburb>
      <City>BLOEMFONTEIN</City>
      <PostalCode>9301</PostalCode>
      <ProvinceCode />
      <Province />
      <AddressPeriod>01</AddressPeriod>
      <OwnerTenant />
      <AddressChanged>N</AddressChanged>
    </AddressNA07>
    <AddressNA07>
      <ConsumerNo>596234740</ConsumerNo>
      <InformationDate>20050408</InformationDate>
      <Line1>BOX 30155</Line1>
      <Line2 />
      <Suburb>PELLISSIER</Suburb>
      <City>BLOEMFONTEIN</City>
      <PostalCode>9322</PostalCode>
      <ProvinceCode />
      <Province />
      <AddressPeriod>00</AddressPeriod>
      <OwnerTenant />
      <AddressChanged>N</AddressChanged>
    </AddressNA07>
    <AddressNA07>
      <ConsumerNo>596234740</ConsumerNo>
      <InformationDate>20050408</InformationDate>
      <Line1>P O BOX 30155</Line1>
      <Line2 />
      <Suburb>PELLISSIER</Suburb>
      <City>BLOEMFONTEIN</City>
      <PostalCode>9322</PostalCode>
      <ProvinceCode />
      <Province />
      <AddressPeriod>00</AddressPeriod>
      <OwnerTenant />
      <AddressChanged>N</AddressChanged>
    </AddressNA07>
  </AddressNA07>
  <DefaultsD701Part1 xmlns="https://secure.transunion.co.za/TUBureau">
    <DefaultD701Part1>
      <RecordSequence>01</RecordSequence>
      <Part>001</Part>
      <PartSequence>01</PartSequence>
      <ConsumerNo>596234740</ConsumerNo>
      <ContactName>ABSA PERSONAL LOANS</ContactName>
      <TelephoneCode />
      <TelephoneNumber />
      <InformationDate>20081210</InformationDate>
      <SupplierName />
      <AccountNo>0000003017182913</AccountNo>
      <SubAccount />
      <Branch>00008018</Branch>
      <DefaultTypeCode>BDWO</DefaultTypeCode>
      <DefaultType>Bad Debt Written Off</DefaultType>
      <DefaultAmount>000006494</DefaultAmount>
      <WrittenOffDate>20081210</WrittenOffDate>
      <Surname1>VENTER</Surname1>
      <Forename1>H</Forename1>
      <Forename2>P</Forename2>
      <Forename3 />
      <IdentityNumber>4712085106082</IdentityNumber>
      <DateOfBirth>19471208</DateOfBirth>
      <AddressLine1>7 VLAKFONTEIN ST</AddressLine1>
      <AddressLine2 />
      <AddressLine3 />
      <AddressLine4>BLOEMFONTEIN</AddressLine4>
      <Postcode>9301</Postcode>
      <Remarks1 />
      <Remarks2 />
    </DefaultD701Part1>
    <DefaultD701Part1>
      <RecordSequence>02</RecordSequence>
      <Part>001</Part>
      <PartSequence>01</PartSequence>
      <ConsumerNo>596234740</ConsumerNo>
      <ContactName>BLUE BEAN</ContactName>
      <TelephoneCode />
      <TelephoneNumber />
      <InformationDate>20080903</InformationDate>
      <SupplierName />
      <AccountNo>0000000008076432</AccountNo>
      <SubAccount />
      <Branch>00007505</Branch>
      <DefaultTypeCode>BDWO</DefaultTypeCode>
      <DefaultType>Bad Debt Written Off</DefaultType>
      <DefaultAmount>000008842</DefaultAmount>
      <WrittenOffDate>20080903</WrittenOffDate>
      <Surname1>VENTER</Surname1>
      <Forename1>H</Forename1>
      <Forename2 />
      <Forename3 />
      <IdentityNumber>4712085106082</IdentityNumber>
      <DateOfBirth>19471208</DateOfBirth>
      <AddressLine1>7 VLAKFONTEIN ST</AddressLine1>
      <AddressLine2 />
      <AddressLine3>PELLISSIER</AddressLine3>
      <AddressLine4>BLOEMFONTEIN</AddressLine4>
      <Postcode>9301</Postcode>
      <Remarks1 />
      <Remarks2 />
    </DefaultD701Part1>
  </DefaultsD701Part1>
  <EnquiriesNE50 xmlns="https://secure.transunion.co.za/TUBureau">
    <EnquiryNE50>
      <ConsumerNo>596234740</ConsumerNo>
      <DateOfEnquiry>20100707</DateOfEnquiry>
      <Subscriber>S A HOME LOANS</Subscriber>
      <Contact>SA HOME LOANS 031 560 5300</Contact>
      <EnquiryTypeCode />
      <EnquiryTypeDescription>Y</EnquiryTypeDescription>
      <OwnAccount />
    </EnquiryNE50>
    <EnquiryNE50>
      <ConsumerNo>596234740</ConsumerNo>
      <DateOfEnquiry>20100430</DateOfEnquiry>
      <Subscriber>WESBANK ACQUISTIO</Subscriber>
      <Contact>WESBANK 011 632 6000</Contact>
      <EnquiryTypeCode />
      <EnquiryTypeDescription />
      <OwnAccount />
    </EnquiryNE50>
    <EnquiryNE50>
      <ConsumerNo>596234740</ConsumerNo>
      <DateOfEnquiry>20100429</DateOfEnquiry>
      <Subscriber>WESBANK ACQUISTIO</Subscriber>
      <Contact>WESBANK 011 632 6000</Contact>
      <EnquiryTypeCode />
      <EnquiryTypeDescription />
      <OwnAccount />
    </EnquiryNE50>
    <EnquiryNE50>
      <ConsumerNo>596234740</ConsumerNo>
      <DateOfEnquiry>20100428</DateOfEnquiry>
      <Subscriber>WESBANK ACQUISTIO</Subscriber>
      <Contact>WESBANK 011 632 6000</Contact>
      <EnquiryTypeCode />
      <EnquiryTypeDescription />
      <OwnAccount />
    </EnquiryNE50>
    <EnquiryNE50>
      <ConsumerNo>596234740</ConsumerNo>
      <DateOfEnquiry>20090129</DateOfEnquiry>
      <Subscriber>OUTSURANCE H/O</Subscriber>
      <Contact>OWEN TAFADZWA SANDE 012 673 3000</Contact>
      <EnquiryTypeCode />
      <EnquiryTypeDescription />
      <OwnAccount />
    </EnquiryNE50>
    <EnquiryNE50>
      <ConsumerNo>596234740</ConsumerNo>
      <DateOfEnquiry>20080812</DateOfEnquiry>
      <Subscriber>NEDCOR JHB</Subscriber>
      <Contact>NEDBANK LIMITED 0860555111</Contact>
      <EnquiryTypeCode />
      <EnquiryTypeDescription />
      <OwnAccount />
    </EnquiryNE50>
  </EnquiriesNE50>
  <AKANamesNK04 xmlns="https://secure.transunion.co.za/TUBureau">
    <AKANamesNK04>
      <RecordSeq>01</RecordSeq>
      <Part>001</Part>
      <PartSeq>01</PartSeq>
      <ConsumerNo>596234740</ConsumerNo>
      <InformationDate>20090428</InformationDate>
      <AKAName>VENTER,HENDRICK,PETRU</AKAName>
    </AKANamesNK04>
    <AKANamesNK04>
      <RecordSeq>02</RecordSeq>
      <Part>001</Part>
      <PartSeq>01</PartSeq>
      <ConsumerNo>596234740</ConsumerNo>
      <InformationDate>20090129</InformationDate>
      <AKAName>VENTER,HENDRICK,PETRUS</AKAName>
    </AKANamesNK04>
    <AKANamesNK04>
      <RecordSeq>03</RecordSeq>
      <Part>001</Part>
      <PartSeq>01</PartSeq>
      <ConsumerNo>596234740</ConsumerNo>
      <InformationDate>20031129</InformationDate>
      <AKAName>VENTER PETRUS HENDRIK,</AKAName>
    </AKANamesNK04>
  </AKANamesNK04>
  <EmploymentNM04 xmlns="https://secure.transunion.co.za/TUBureau">
    <EmploymentNM04>
      <RecordSeq>01</RecordSeq>
      <Part>001</Part>
      <PartSeq>01</PartSeq>
      <ConsumerNo>596234740</ConsumerNo>
      <InformationDate>20100430</InformationDate>
      <Occupation />
      <EmployerName>WESBANK</EmployerName>
      <EmploymentPeriod>00</EmploymentPeriod>
    </EmploymentNM04>
    <EmploymentNM04>
      <RecordSeq>02</RecordSeq>
      <Part>001</Part>
      <PartSeq>01</PartSeq>
      <ConsumerNo>596234740</ConsumerNo>
      <InformationDate>20080812</InformationDate>
      <Occupation />
      <EmployerName>AWERBUCHS HOUSE PTY</EmployerName>
      <EmploymentPeriod>00</EmploymentPeriod>
    </EmploymentNM04>
    <EmploymentNM04>
      <RecordSeq>03</RecordSeq>
      <Part>001</Part>
      <PartSeq>01</PartSeq>
      <ConsumerNo>596234740</ConsumerNo>
      <InformationDate>20080702</InformationDate>
      <Occupation>SALESMAN</Occupation>
      <EmployerName>AWERBUCHS BARGAIN</EmployerName>
      <EmploymentPeriod>00</EmploymentPeriod>
    </EmploymentNM04>
  </EmploymentNM04>
  <PaymentProfilesP701 xmlns="https://secure.transunion.co.za/TUBureau">
    <PaymentProfileP701>
      <ConsumerNo>596234740</ConsumerNo>
      <LastUpdateDate>20100322</LastUpdateDate>
      <SupplierName>MARKHAMS</SupplierName>
      <IndustryCode>CL</IndustryCode>
      <Industry>Clothing</Industry>
      <AccountTypeCode>R</AccountTypeCode>
      <AccountTypeDesc>REVOLVING</AccountTypeDesc>
      <AccountNumber />
      <SubAccount />
      <DateOpened>20011031</DateOpened>
      <OpeningBalance>000001452</OpeningBalance>
      <Instalment>000000145</Instalment>
      <CurrentBalance>000000578</CurrentBalance>
      <Terms>0000</Terms>
      <OverdueAmount>000000000</OverdueAmount>
      <OwnershipType>00</OwnershipType>
      <NumberOfParticipantsInJointLoan>00</NumberOfParticipantsInJointLoan>
      <PaymentType>00</PaymentType>
      <RepaymentFrequency>03</RepaymentFrequency>
      <DeferredPaymentDate>00000000</DeferredPaymentDate>
      <AccSoldTo3rdParty>00</AccSoldTo3rdParty>
      <ThirdPartyName />
      <PaymentHistories>
        <PaymentHistory>
          <Date>201007</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201006</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201005</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201004</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201003</Date>
          <StatusCode>0</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201002</Date>
          <StatusCode>1</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201001</Date>
          <StatusCode>0</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200912</Date>
          <StatusCode>1</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200911</Date>
          <StatusCode>2</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200910</Date>
          <StatusCode>1</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200909</Date>
          <StatusCode>1</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200908</Date>
          <StatusCode>0</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200907</Date>
          <StatusCode>1</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200906</Date>
          <StatusCode>0</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200905</Date>
          <StatusCode>0</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200904</Date>
          <StatusCode>2</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200903</Date>
          <StatusCode>2</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200902</Date>
          <StatusCode>1</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200901</Date>
          <StatusCode>1</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200812</Date>
          <StatusCode>0</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200811</Date>
          <StatusCode>0</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200810</Date>
          <StatusCode>1</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200809</Date>
          <StatusCode>0</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200808</Date>
          <StatusCode>1</StatusCode>
        </PaymentHistory>
      </PaymentHistories>
    </PaymentProfileP701>
    <PaymentProfileP701>
      <ConsumerNo>596234740</ConsumerNo>
      <LastUpdateDate>20100416</LastUpdateDate>
      <SupplierName>EDGARS</SupplierName>
      <IndustryCode>CL</IndustryCode>
      <Industry>Clothing</Industry>
      <AccountTypeCode>R</AccountTypeCode>
      <AccountTypeDesc>REVOLVING</AccountTypeDesc>
      <AccountNumber />
      <SubAccount />
      <DateOpened>20040517</DateOpened>
      <OpeningBalance>000002440</OpeningBalance>
      <Instalment>000000397</Instalment>
      <CurrentBalance>000002177</CurrentBalance>
      <Terms>0000</Terms>
      <OverdueAmount>000000000</OverdueAmount>
      <OwnershipType>00</OwnershipType>
      <NumberOfParticipantsInJointLoan>00</NumberOfParticipantsInJointLoan>
      <PaymentType>00</PaymentType>
      <RepaymentFrequency>03</RepaymentFrequency>
      <DeferredPaymentDate>00000000</DeferredPaymentDate>
      <AccSoldTo3rdParty>00</AccSoldTo3rdParty>
      <ThirdPartyName />
      <PaymentHistories>
        <PaymentHistory>
          <Date>201007</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201006</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201005</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201004</Date>
          <StatusCode>0</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201003</Date>
          <StatusCode>0</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201002</Date>
          <StatusCode>2</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201001</Date>
          <StatusCode>2</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200912</Date>
          <StatusCode>1</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200911</Date>
          <StatusCode>1</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200910</Date>
          <StatusCode>1</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200909</Date>
          <StatusCode>1</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200908</Date>
          <StatusCode>1</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200907</Date>
          <StatusCode>1</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200906</Date>
          <StatusCode>1</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200905</Date>
          <StatusCode>0</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200904</Date>
          <StatusCode>0</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200903</Date>
          <StatusCode>0</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200902</Date>
          <StatusCode>0</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200901</Date>
          <StatusCode>1</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200812</Date>
          <StatusCode>0</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200811</Date>
          <StatusCode>2</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200810</Date>
          <StatusCode>1</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200809</Date>
          <StatusCode>1</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200808</Date>
          <StatusCode>1</StatusCode>
        </PaymentHistory>
      </PaymentHistories>
    </PaymentProfileP701>   
    <PaymentProfileP701>
      <ConsumerNo>596234740</ConsumerNo>
      <LastUpdateDate>20100330</LastUpdateDate>
      <SupplierName>FIRSTRAND STI</SupplierName>
      <IndustryCode>IN</IndustryCode>
      <Industry>Insurance</Industry>
      <AccountTypeCode>S</AccountTypeCode>
      <AccountTypeDesc>SHORT TERM INSURANCE</AccountTypeDesc>
      <AccountNumber />
      <SubAccount />
      <DateOpened>20090301</DateOpened>
      <OpeningBalance>000000000</OpeningBalance>
      <Instalment>000000602</Instalment>
      <CurrentBalance>000000602</CurrentBalance>
      <Terms />
      <OverdueAmount>000000000</OverdueAmount>
      <OwnershipType>00</OwnershipType>
      <NumberOfParticipantsInJointLoan>00</NumberOfParticipantsInJointLoan>
      <PaymentType>00</PaymentType>
      <RepaymentFrequency>03</RepaymentFrequency>
      <DeferredPaymentDate>00000000</DeferredPaymentDate>
      <AccSoldTo3rdParty>00</AccSoldTo3rdParty>
      <ThirdPartyName />
      <PaymentHistories>
        <PaymentHistory>
          <Date>201007</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201006</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201005</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201004</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201003</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201002</Date>
          <StatusCode>0</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201001</Date>
          <StatusCode>0</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200912</Date>
          <StatusCode>0</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200911</Date>
          <StatusCode>0</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200910</Date>
          <StatusCode>0</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200909</Date>
          <StatusCode>0</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200908</Date>
          <StatusCode>0</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200907</Date>
          <StatusCode>0</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200906</Date>
          <StatusCode>0</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200905</Date>
          <StatusCode>0</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200904</Date>
          <StatusCode>0</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200903</Date>
          <StatusCode>0</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200902</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200901</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200812</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200811</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200810</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200809</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200808</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
      </PaymentHistories>
    </PaymentProfileP701>
    <PaymentProfileP701>
      <ConsumerNo>596234740</ConsumerNo>
      <LastUpdateDate>20100325</LastUpdateDate>
      <SupplierName>NEDBANK MORTGAGE LOANS</SupplierName>
      <IndustryCode>MB</IndustryCode>
      <Industry>Mortgage Bonds</Industry>
      <AccountTypeCode>H</AccountTypeCode>
      <AccountTypeDesc>HOME LOAN</AccountTypeDesc>
      <AccountNumber />
      <SubAccount />
      <DateOpened>20000324</DateOpened>
      <OpeningBalance>000280321</OpeningBalance>
      <Instalment>000002926</Instalment>
      <CurrentBalance>000262312</CurrentBalance>
      <Terms>0180</Terms>
      <OverdueAmount>000000000</OverdueAmount>
      <OwnershipType>02</OwnershipType>
      <NumberOfParticipantsInJointLoan>02</NumberOfParticipantsInJointLoan>
      <PaymentType>00</PaymentType>
      <RepaymentFrequency>00</RepaymentFrequency>
      <DeferredPaymentDate>00000000</DeferredPaymentDate>
      <AccSoldTo3rdParty>00</AccSoldTo3rdParty>
      <ThirdPartyName />
      <PaymentHistories>
        <PaymentHistory>
          <Date>201007</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201006</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201005</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201004</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201003</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201002</Date>
          <StatusCode>0</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201001</Date>
          <StatusCode>1</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200912</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200911</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200910</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200909</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200908</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200907</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200906</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200905</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200904</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200903</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200902</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200901</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200812</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200811</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200810</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200809</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200808</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
      </PaymentHistories>
    </PaymentProfileP701>
  </PaymentProfilesP701>
  <NLRAccountsInformationM701 xmlns="https://secure.transunion.co.za/TUBureau">
    <NLRAccountInformationM701>
      <ConsumerNo>596234740</ConsumerNo>
      <EnquiryReferenceNo>00000000000</EnquiryReferenceNo>
      <SubscriberCode>0000000000</SubscriberCode>
      <SubscriberName>RCS - GENERAL PURPOS</SubscriberName>
      <BranchCode />
      <AccountNo>0518020000831293169</AccountNo>
      <SubAccountNo />
      <AccountType>J</AccountType>
      <AccountOpenDate>20051214</AccountOpenDate>
      <LastUpdateDate>20091120</LastUpdateDate>
      <LoanAmount>000000000</LoanAmount>
      <InstalmentAmount>000000000</InstalmentAmount>
      <CurrentBalance>000000000</CurrentBalance>
      <BalanceOverdue>000000000</BalanceOverdue>
      <PaymentDuration>000</PaymentDuration>
      <WorstPaymentHistory>00</WorstPaymentHistory>
      <EndUseCode>F</EndUseCode>
      <LoanRegistrationNo>00000000000</LoanRegistrationNo>
      <OwnershipType>00</OwnershipType>
      <NoOfParticipantsJointLoan>00</NoOfParticipantsJointLoan>
      <PaymentType>00</PaymentType>
      <RepaymentFrequency>03</RepaymentFrequency>
      <DeferredPaymentDate>00000000</DeferredPaymentDate>
      <AccSoldTo3rdParty>00</AccSoldTo3rdParty>
      <ThirdPartyName />
      <PaymentHistories>
        <PaymentHistory>
          <Date>201007</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201006</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201005</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201004</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201003</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201002</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201001</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200912</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200911</Date>
          <StatusCode>P</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200910</Date>
          <StatusCode>9</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200909</Date>
          <StatusCode>9</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200908</Date>
          <StatusCode>9</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200907</Date>
          <StatusCode>9</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200906</Date>
          <StatusCode>9</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200905</Date>
          <StatusCode>9</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200904</Date>
          <StatusCode>9</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200903</Date>
          <StatusCode>9</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200902</Date>
          <StatusCode>9</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200901</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200812</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200811</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200810</Date>
          <StatusCode>W</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200809</Date>
          <StatusCode>7</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200808</Date>
          <StatusCode>6</StatusCode>
        </PaymentHistory>
      </PaymentHistories>
    </NLRAccountInformationM701>
    <NLRAccountInformationM701>
      <ConsumerNo>596234740</ConsumerNo>
      <EnquiryReferenceNo>00000000000</EnquiryReferenceNo>
      <SubscriberCode>0000000000</SubscriberCode>
      <SubscriberName>NEDBANK LIMITED</SubscriberName>
      <BranchCode />
      <AccountNo>0008000059327601</AccountNo>
      <SubAccountNo />
      <AccountType>K</AccountType>
      <AccountOpenDate>20061004</AccountOpenDate>
      <LastUpdateDate>20100412</LastUpdateDate>
      <LoanAmount>000000000</LoanAmount>
      <InstalmentAmount>000000000</InstalmentAmount>
      <CurrentBalance>000000000</CurrentBalance>
      <BalanceOverdue>000000000</BalanceOverdue>
      <PaymentDuration>001</PaymentDuration>
      <WorstPaymentHistory>00</WorstPaymentHistory>
      <EndUseCode>O</EndUseCode>
      <LoanRegistrationNo>00000000000</LoanRegistrationNo>
      <OwnershipType>00</OwnershipType>
      <NoOfParticipantsJointLoan>00</NoOfParticipantsJointLoan>
      <PaymentType>00</PaymentType>
      <RepaymentFrequency>03</RepaymentFrequency>
      <DeferredPaymentDate>00000000</DeferredPaymentDate>
      <AccSoldTo3rdParty>00</AccSoldTo3rdParty>
      <ThirdPartyName />
      <PaymentHistories>
        <PaymentHistory>
          <Date>201007</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201006</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201005</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201004</Date>
          <StatusCode>C</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201003</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201002</Date>
          <StatusCode>2</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>201001</Date>
          <StatusCode>3</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200912</Date>
          <StatusCode>3</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200911</Date>
          <StatusCode>4</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200910</Date>
          <StatusCode>=</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200909</Date>
          <StatusCode>5</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200908</Date>
          <StatusCode>6</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200907</Date>
          <StatusCode>8</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200906</Date>
          <StatusCode>7</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200905</Date>
          <StatusCode>9</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200904</Date>
          <StatusCode>9</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200903</Date>
          <StatusCode>9</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200902</Date>
          <StatusCode>9</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200901</Date>
          <StatusCode>9</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200812</Date>
          <StatusCode>9</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200811</Date>
          <StatusCode>9</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200810</Date>
          <StatusCode>9</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200809</Date>
          <StatusCode>9</StatusCode>
        </PaymentHistory>
        <PaymentHistory>
          <Date>200808</Date>
          <StatusCode>9</StatusCode>
        </PaymentHistory>
      </PaymentHistories>
    </NLRAccountInformationM701> 
  </NLRAccountsInformationM701>
  <StandardBatchCharsSB33 xmlns="https://secure.transunion.co.za/TUBureau">
    <ConsumerNo>596234740</ConsumerNo>
    <AT094>'+@AT094+'</AT094>
    <AT107>'+@AT107+'</AT107>
    <AT164>'+@AT164+'</AT164>
    <AT179>+0021983</AT179>
    <G012>-0000002</G012>
    <G032>-0000002</G032>
    <G044>+0000000</G044>
    <IN106>'+@IN106+'</IN106>
    <IN141>'+@IN141+'</IN141>
    <ML179>+0001323</ML179>
    <OT007>'+@OT007+'</OT007>
    <OT041>'+@OT041+'</OT041>
    <RE105>'+@RE105+'</RE105>
    <RE140>'+@RE140+'</RE140>
    <WEPP_Status>0</WEPP_Status>
  </StandardBatchCharsSB33>
  <NLRCounterSeqmentMC01 xmlns="https://secure.transunion.co.za/TUBureau">
    <NLRCounterSeqmentMC01>
      <SegmentCode>MC</SegmentCode>
      <ConsumerNumber>596234740</ConsumerNumber>
      <CurrentYearEnquiriesClient>000</CurrentYearEnquiriesClient>
      <CurrentYearEnquiriesOtherSubscribers>000</CurrentYearEnquiriesOtherSubscribers>
      <CurrentYearPositiveNLRLoans>000</CurrentYearPositiveNLRLoans>
      <CurrentYearHighestMonthsInArrears>000</CurrentYearHighestMonthsInArrears>
      <PreviousYearEnquiriesClient>000</PreviousYearEnquiriesClient>
      <PreviousYearEnquiriesOtherSubscribers>000</PreviousYearEnquiriesOtherSubscribers>
      <PreviousYearPositiveNLRLoans>000</PreviousYearPositiveNLRLoans>
      <PreviousYearHighestMonthsInArrears>000</PreviousYearHighestMonthsInArrears>
      <AllOtherYearEnquiriesClient>000</AllOtherYearEnquiriesClient>
      <AllOtherYearEnquiriesOtherSubscribers>000</AllOtherYearEnquiriesOtherSubscribers>
      <AllOtherYearPositiveNLRLoans>000</AllOtherYearPositiveNLRLoans>
      <AllOtherYearsHighestMonthsInArrears>000</AllOtherYearsHighestMonthsInArrears>
      <CumulativeInstalmentValue>000000000</CumulativeInstalmentValue>
      <CumulativeOutstandingBalance>000000000</CumulativeOutstandingBalance>
      <WorstMonthInArrearsEver>000</WorstMonthInArrearsEver>
    </NLRCounterSeqmentMC01>
  </NLRCounterSeqmentMC01>
  <AggregateNX09 xmlns="https://secure.transunion.co.za/TUBureau">
    <ConsumerNo>596234740</ConsumerNo>
    <PPWorstArrears6Months>2</PPWorstArrears6Months>
    <PPWorstArrears24Months />
  </AggregateNX09>
  <NLRSummaryMY01 xmlns="https://secure.transunion.co.za/TUBureau">
    <ConsumerNo>596234740</ConsumerNo>
    <TotalActiveAccounts>002</TotalActiveAccounts>
    <TotalClosedAccounts24Mths>002</TotalClosedAccounts24Mths>
    <TotalAdverseAccounts24Mths>002</TotalAdverseAccounts24Mths>
    <HighestActualMonths24Mths>009</HighestActualMonths24Mths>
    <NoRevolvingAccounts>000</NoRevolvingAccounts>
    <NoCurrentInstallmentAccounts>000</NoCurrentInstallmentAccounts>
    <NoCurrentOpenAccounts>000</NoCurrentOpenAccounts>
    <CurrentBalance>000005943</CurrentBalance>
    <CurrentBalanceInd />
    <CurrentMonthlyInstallment>000000476</CurrentMonthlyInstallment>
    <CurrentMonthlyInstallmentBalInd />
    <CumulativeArrearsAmount>000000000</CumulativeArrearsAmount>
    <CumulativeArrearsAmountBalanceInd />
  </NLRSummaryMY01>
  <CCASummaryMX01 xmlns="https://secure.transunion.co.za/TUBureau">
    <ConsumerNo>596234740</ConsumerNo>
    <TotalActiveAccounts>003</TotalActiveAccounts>
    <TotalClosedAccounts24Mths>000</TotalClosedAccounts24Mths>
    <TotalAdverseAccounts24Mths>003</TotalAdverseAccounts24Mths>
    <HighestActualMonths24Mths>009</HighestActualMonths24Mths>
    <NoRevolvingAccounts>002</NoRevolvingAccounts>
    <NoCurrentInstallmentAccounts>000</NoCurrentInstallmentAccounts>
    <NoCurrentOpenAccounts>000</NoCurrentOpenAccounts>
    <CurrentBalance>000012199</CurrentBalance>
    <CurrentBalanceInd />
    <CurrentMonthlyInstallment>000004806</CurrentMonthlyInstallment>
    <CurrentMonthlyInstallmentBalInd />
    <CumulativeArrearsAmount>000000000</CumulativeArrearsAmount>
    <CumulativeArrearsAmountBalanceInd />
  </CCASummaryMX01>
</BureauResponse>'

		BEGIN TRANSACTION
	 	
	 		IF @IDNumber IS NOT NULL
	 			BEGIN
					
					INSERT INTO ITC
					(LegalEntityKey,AccountKey,ChangeDate,ResponseXML,ResponseStatus,UserID)
					VALUES
					(@LegalEntityKey,@AccountKey,GETDATE(),@xml,'Success','SAHL\BCUser')
					
					COMMIT TRANSACTION
				END
			ELSE
				BEGIN
					ROLLBACK TRANSACTION
				END
				
		FETCH NEXT FROM ITCLE INTO @LegalEntityKey
		
		END
		
CLOSE ITCLE
DEALLOCATE ITCLE; 
	
END
GO
