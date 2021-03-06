USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[test].[InsertITC]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure [test].[InsertITC]
	Print 'Dropped procedure [test].[InsertITC]'
End
Go

CREATE PROCEDURE [test].[InsertITC]

@OfferKey Int,
@EmpiricaUpperBoundary Int = 999,
@EmpiricaLowerBoundary Int = 711,
@ADUser Varchar(20)

AS

Declare @Account Int,
@LegalEntity Int,
@EmpiricaScore VarChar(5),
@IDNumber VarChar(13),
@FirstName VarChar(50),
@Surname VarChar (50),
@Random Int,
@RandomEmpirica VarChar(5),
@countITC Int

--@OfferKey Int,
--@EmpiricaUpperBoundary Int,
--@EmpiricaLowerBoundary Int,
--@ADUser Varchar(20)
--
--set @OfferKey = 767348
--set @EmpiricaUpperBoundary = 999
--set @EmpiricaLowerBoundary = 711
--set @ADUser= 'AndrewK'

--@DateOfEnquiry VarChar(8)

--Set @DateOfEnquiry = cast(Year(getdate()) as VarChar(4)) +
--	case 
--		when month(getdate()) < 10
--			then '0' + cast(Month(getdate()) as VarChar(1))
--			else  cast(Month(getdate()) as VarChar(2))
--	end +
--	case
--		when day(getdate()) < 10
--			then '0' + cast(day(getdate()) as VarChar(1))
--			else cast(day(getdate()) as VarChar(2))
--	end

Declare ITCLegalEntity Cursor for
	Select le.LegalEntityKey
	from offer o 
		join offerrole ro on ro.offerkey=o.offerkey and ro.offerroletypekey in (8,10,11,12)
		join legalentity le on le.legalentitykey=ro.legalentitykey
	where 
		o.offerkey = @OfferKey

Open ITCLegalEntity;

Fetch next from ITCLegalEntity into @LegalEntity

While @@fetch_status = 0

	begin

		Select 
			@IDNumber=le.idnumber, 
			@Firstname = le.firstnames,
			@Surname = le.surname,
			@Account=o.ReservedAccountKey
		from offer o 
			join offerrole ro on ro.offerkey=o.offerkey and ro.offerroletypekey in (8,10,11,12)
			join legalentity le on le.legalentitykey=ro.legalentitykey
		where 
			o.offerkey = @OfferKey
			and le.legalentitykey = @LegalEntity

		---- This will create a random number between @EmpiricaLowerBoundary and @EmpiricaUpperBoundary
		Select @Random = ROUND(((@EmpiricaUpperBoundary - @EmpiricaLowerBoundary -1) * RAND() + @EmpiricaLowerBoundary), 0)
		Select @RandomEmpirica = '00'+cast(@Random as varchar(3))

		-- use our random in range Empirica for this record
		Set @EmpiricaScore = @RandomEmpirica

		Begin Transaction

			if @IDNumber is not null 
				Begin
					insert into ITC 
						(LegalEntityKey, AccountKey, ChangeDate, ResponseXML, ResponseStatus, UserID)
						Values(@LegalEntity, @Account, getdate(), '<BureauResponse xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
						  <RawData xmlns="https://secure.transunion.co.za/TUBureau" />
						  <ResponseStatus xmlns="https://secure.transunion.co.za/TUBureau">Success</ResponseStatus>
						  <ProcessingTimeSecs xmlns="https://secure.transunion.co.za/TUBureau">3.15625</ProcessingTimeSecs>
						  <ConsEnqTransInfo0102 xmlns="https://secure.transunion.co.za/TUBureau">
							<DefiniteMatchCount>1</DefiniteMatchCount>
							<PossibleMatchCount>00</PossibleMatchCount>
							<MatchedConsumerNo>229312537</MatchedConsumerNo>
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
							<ConsumerNo>229312537</ConsumerNo>
							<Surname>' + @Surname + '</Surname>
							<Forename1>' + @Firstname + '</Forename1>
							<Forename2 />
							<Forename3 />
							<Title>MR</Title>
							<Gender>M</Gender>
							<NameInfoDate>20090929</NameInfoDate>
							<DateOfBirth>19731211</DateOfBirth>
							<IdentityNo1>' + @IDNumber + '</IdentityNo1>
							<IdentityNo2 />
							<MaritalStatusCode>M</MaritalStatusCode>
							<MaritalStatusDesc>MARRIED</MaritalStatusDesc>
							<Dependants>00</Dependants>
							<SpouseName1 />
							<SpouseName2 />
							<TelephoneNumbers>H(031)2013911  B(031)5605300</TelephoneNumbers>
							<DeceasedDate>00000000</DeceasedDate>
						  </ConsumerInfoNO04>
						  <FraudScoreFS01 xmlns="https://secure.transunion.co.za/TUBureau">
							<RecordSequence>01</RecordSequence>
							<Part>001</Part>
							<PartSequence>01</PartSequence>
							<ConsumerNo>229312537</ConsumerNo>
							<Rating>2</Rating>
							<RatingDescription>MINOR IRREGULARITIES - LOW PROBABILITY OF SUSPICIOUS DATA</RatingDescription>
							<ReasonCode>
							  <string>BS18</string>
							  <string>BS40</string>
							  <string>BS48</string>
							  <string>HA94</string>
							</ReasonCode>
							<ReasonDescription>
							  <string>ALERT - HIGH NUMBER OF HOME TEL NUMBER CHANGES WITHIN 30 DAYS</string>
							  <string>EMPLOYER NAME ON BUREAU HEADER AND NO EMPLOYER NAME ON APPLICATION</string>
							  <string>Address2 on bureau header and No Address2 on application</string>
							  <string>HIGH NUMBER OF SEARCHES ON ADDRESS 1 WITHIN 12 HOURS</string>
							</ReasonDescription>
						  </FraudScoreFS01>
						  <EmpiricaEM05 xmlns="https://secure.transunion.co.za/TUBureau">
							<ConsumerNo>229312537</ConsumerNo>
							<EmpiricaScore>' + @EmpiricaScore + '</EmpiricaScore>
							<ExclusionCode>N</ExclusionCode>
							<ReasonCode1 />
							<ReasonCode2 />
							<ReasonCode3 />
							<ReasonCode4 />
							<ExpansionScore />
						  </EmpiricaEM05>
						  <ConsumerCountersNC04 xmlns="https://secure.transunion.co.za/TUBureau">
							<ConsumerNo>229312537</ConsumerNo>
							<OwnEnquiries1YrBack>002</OwnEnquiries1YrBack>
							<OwnEnquiries2YrsBack>012</OwnEnquiries2YrsBack>
							<OwnEnquiriesMoreThen2YrsBack>000</OwnEnquiriesMoreThen2YrsBack>
							<OtherEnquiries1YrBack>000</OtherEnquiries1YrBack>
							<OtherEnquiries2YrsBack>001</OtherEnquiries2YrsBack>
							<OtherEnquiriesMoreThen2YrsBack>000</OtherEnquiriesMoreThen2YrsBack>
							<Judgements1YrBack>000</Judgements1YrBack>
							<Judgements2YrsBack>000</Judgements2YrsBack>
							<JudgementsMoreThen2YrsBack>000</JudgementsMoreThen2YrsBack>
							<Notices1YrBack>000</Notices1YrBack>
							<Notices2YrsBack>000</Notices2YrsBack>
							<NoticesMoreThen2YrsBack>000</NoticesMoreThen2YrsBack>
							<Defaults1YrBack>000</Defaults1YrBack>
							<Defaults2YrsBack>000</Defaults2YrsBack>
							<DefaultsMoreThen2YrsBack>000</DefaultsMoreThen2YrsBack>
							<PaymentProfiles1YrBack>000</PaymentProfiles1YrBack>
							<PaymentProfiles2YrsBack>000</PaymentProfiles2YrsBack>
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
							<VerifiedSurname>DANIELL</VerifiedSurname>
							<VerifiedForename1>GARY</VerifiedForename1>
							<VerifiedForename2 />
							<DeceasedDate />
						  </IdvNI01>
						  <AddressVerificationNR01 xmlns="https://secure.transunion.co.za/TUBureau">
							<Last24Hours>000000</Last24Hours>
							<Last48Hours>000000</Last48Hours>
							<Last96Hours>000000</Last96Hours>
							<Last30Days>000001</Last30Days>
							<AddressMessage>Address 1</AddressMessage>
						  </AddressVerificationNR01>
						  <AddressNA07 xmlns="https://secure.transunion.co.za/TUBureau">
							<AddressNA07>
							  <ConsumerNo>229312537</ConsumerNo>
							  <InformationDate>20070517</InformationDate>
							  <Line1>416 MUSGRAVE RD</Line1>
							  <Line2 />
							  <Suburb>MUSGRAVE</Suburb>
							  <City>DURBAN</City>
							  <PostalCode>4001</PostalCode>
							  <ProvinceCode />
							  <Province />
							  <AddressPeriod>02</AddressPeriod>
							  <OwnerTenant>T</OwnerTenant>
							  <AddressChanged>N</AddressChanged>
							</AddressNA07>
							<AddressNA07>
							  <ConsumerNo>229312537</ConsumerNo>
							  <InformationDate>20070517</InformationDate>
							  <Line1>6 416 MUSGRAVE RD</Line1>
							  <Line2 />
							  <Suburb>MUSGRAVE</Suburb>
							  <City>DURBAN</City>
							  <PostalCode>4001</PostalCode>
							  <ProvinceCode />
							  <Province />
							  <AddressPeriod>00</AddressPeriod>
							  <OwnerTenant />
							  <AddressChanged>N</AddressChanged>
							</AddressNA07>
							<AddressNA07>
							  <ConsumerNo>229312537</ConsumerNo>
							  <InformationDate>20070219</InformationDate>
							  <Line1>UNIT 6</Line1>
							  <Line2>416 MUSGRAVE RD</Line2>
							  <Suburb>MUSGRAVE</Suburb>
							  <City>DURBAN</City>
							  <PostalCode>4001</PostalCode>
							  <ProvinceCode />
							  <Province />
							  <AddressPeriod>00</AddressPeriod>
							  <OwnerTenant />
							  <AddressChanged>N</AddressChanged>
							</AddressNA07>
							<AddressNA07>
							  <ConsumerNo>229312537</ConsumerNo>
							  <InformationDate>20061011</InformationDate>
							  <Line1>18 SILVERLEA</Line1>
							  <Line2>37 SILVERTON RD</Line2>
							  <Suburb>MUSGRAVE</Suburb>
							  <City>DURBAN</City>
							  <PostalCode>4001</PostalCode>
							  <ProvinceCode />
							  <Province />
							  <AddressPeriod>00</AddressPeriod>
							  <OwnerTenant />
							  <AddressChanged>N</AddressChanged>
							</AddressNA07>
						  </AddressNA07>
						  <EnquiriesNE09 xmlns="https://secure.transunion.co.za/TUBureau">
							<EnquiriesNE09>
							  <RecordSeq>00</RecordSeq>
							  <Part>001</Part>
							  <PartSeq>01</PartSeq>
							  <ConsumerNo>229312537</ConsumerNo>
							  <DateOfEnquiry>20090917</DateOfEnquiry>
							  <Subscriber>S A HOME LOANS</Subscriber>
							  <Contact>SA HOME LOANS 031 560 5300</Contact>
							  <EnquiryAmount>000000</EnquiryAmount>
							  <EnquiryTypeCode>45</EnquiryTypeCode>
							  <EnquiryType>OTHER</EnquiryType>
							</EnquiriesNE09>
							<EnquiriesNE09>
							  <RecordSeq>01</RecordSeq>
							  <Part>001</Part>
							  <PartSeq>01</PartSeq>
							  <ConsumerNo>229312537</ConsumerNo>
							  <DateOfEnquiry>20090917</DateOfEnquiry>
							  <Subscriber>S A HOME LOANS</Subscriber>
							  <Contact>MR DAVE WRIGHT 031 560 5300</Contact>
							  <EnquiryAmount>000000</EnquiryAmount>
							  <EnquiryTypeCode>45</EnquiryTypeCode>
							  <EnquiryType>OTHER</EnquiryType>
							</EnquiriesNE09>
							<EnquiriesNE09>
							  <RecordSeq>02</RecordSeq>
							  <Part>001</Part>
							  <PartSeq>01</PartSeq>
							  <ConsumerNo>229312537</ConsumerNo>
							  <DateOfEnquiry>20080821</DateOfEnquiry>
							  <Subscriber>S A HOME LOANS</Subscriber>
							  <Contact>SA HOME LOANS 031 560 5300</Contact>
							  <EnquiryAmount>000000</EnquiryAmount>
							  <EnquiryTypeCode>45</EnquiryTypeCode>
							  <EnquiryType>OTHER</EnquiryType>
							</EnquiriesNE09>
							<EnquiriesNE09>
							  <RecordSeq>03</RecordSeq>
							  <Part>001</Part>
							  <PartSeq>01</PartSeq>
							  <ConsumerNo>229312537</ConsumerNo>
							  <DateOfEnquiry>20080820</DateOfEnquiry>
							  <Subscriber>S A HOME LOANS</Subscriber>
							  <Contact>SA HOME LOANS 031 560 5300</Contact>
							  <EnquiryAmount>000000</EnquiryAmount>
							  <EnquiryTypeCode>45</EnquiryTypeCode>
							  <EnquiryType>OTHER</EnquiryType>
							</EnquiriesNE09>
							<EnquiriesNE09>
							  <RecordSeq>04</RecordSeq>
							  <Part>001</Part>
							  <PartSeq>01</PartSeq>
							  <ConsumerNo>229312537</ConsumerNo>
							  <DateOfEnquiry>20080818</DateOfEnquiry>
							  <Subscriber>S A HOME LOANS</Subscriber>
							  <Contact>SA HOME LOANS 031 560 5300</Contact>
							  <EnquiryAmount>000000</EnquiryAmount>
							  <EnquiryTypeCode>45</EnquiryTypeCode>
							  <EnquiryType>OTHER</EnquiryType>
							</EnquiriesNE09>
							<EnquiriesNE09>
							  <RecordSeq>05</RecordSeq>
							  <Part>001</Part>
							  <PartSeq>01</PartSeq>
							  <ConsumerNo>229312537</ConsumerNo>
							  <DateOfEnquiry>20080722</DateOfEnquiry>
							  <Subscriber>S A HOME LOANS</Subscriber>
							  <Contact>SA HOME LOANS 031 560 5300</Contact>
							  <EnquiryAmount>000000</EnquiryAmount>
							  <EnquiryTypeCode>45</EnquiryTypeCode>
							  <EnquiryType>OTHER</EnquiryType>
							</EnquiriesNE09>
							<EnquiriesNE09>
							  <RecordSeq>06</RecordSeq>
							  <Part>001</Part>
							  <PartSeq>01</PartSeq>
							  <ConsumerNo>229312537</ConsumerNo>
							  <DateOfEnquiry>20080704</DateOfEnquiry>
							  <Subscriber>S A HOME LOANS</Subscriber>
							  <Contact>MR DAVE WRIGHT 031 560 5300</Contact>
							  <EnquiryAmount>000000</EnquiryAmount>
							  <EnquiryTypeCode>45</EnquiryTypeCode>
							  <EnquiryType>OTHER</EnquiryType>
							</EnquiriesNE09>
							<EnquiriesNE09>
							  <RecordSeq>07</RecordSeq>
							  <Part>001</Part>
							  <PartSeq>01</PartSeq>
							  <ConsumerNo>229312537</ConsumerNo>
							  <DateOfEnquiry>20080702</DateOfEnquiry>
							  <Subscriber>S A HOME LOANS</Subscriber>
							  <Contact>MR DAVE WRIGHT 031 560 5300</Contact>
							  <EnquiryAmount>000000</EnquiryAmount>
							  <EnquiryTypeCode>45</EnquiryTypeCode>
							  <EnquiryType>OTHER</EnquiryType>
							</EnquiriesNE09>
							<EnquiriesNE09>
							  <RecordSeq>08</RecordSeq>
							  <Part>001</Part>
							  <PartSeq>01</PartSeq>
							  <ConsumerNo>229312537</ConsumerNo>
							  <DateOfEnquiry>20080701</DateOfEnquiry>
							  <Subscriber>S A HOME LOANS</Subscriber>
							  <Contact>MR DAVE WRIGHT 031 560 5300</Contact>
							  <EnquiryAmount>000000</EnquiryAmount>
							  <EnquiryTypeCode>45</EnquiryTypeCode>
							  <EnquiryType>OTHER</EnquiryType>
							</EnquiriesNE09>
							<EnquiriesNE09>
							  <RecordSeq>09</RecordSeq>
							  <Part>001</Part>
							  <PartSeq>01</PartSeq>
							  <ConsumerNo>229312537</ConsumerNo>
							  <DateOfEnquiry>20080630</DateOfEnquiry>
							  <Subscriber>S A HOME LOANS</Subscriber>
							  <Contact>SA HOME LOANS 031 560 5300</Contact>
							  <EnquiryAmount>000000</EnquiryAmount>
							  <EnquiryTypeCode>45</EnquiryTypeCode>
							  <EnquiryType>OTHER</EnquiryType>
							</EnquiriesNE09>
							<EnquiriesNE09>
							  <RecordSeq>10</RecordSeq>
							  <Part>001</Part>
							  <PartSeq>01</PartSeq>
							  <ConsumerNo>229312537</ConsumerNo>
							  <DateOfEnquiry>20080424</DateOfEnquiry>
							  <Subscriber>S A HOME LOANS</Subscriber>
							  <Contact>SA HOME LOANS 031 560 5300</Contact>
							  <EnquiryAmount>000000</EnquiryAmount>
							  <EnquiryTypeCode>45</EnquiryTypeCode>
							  <EnquiryType>OTHER</EnquiryType>
							</EnquiriesNE09>
							<EnquiriesNE09>
							  <RecordSeq>11</RecordSeq>
							  <Part>001</Part>
							  <PartSeq>01</PartSeq>
							  <ConsumerNo>229312537</ConsumerNo>
							  <DateOfEnquiry>20080326</DateOfEnquiry>
							  <Subscriber>S A HOME LOANS</Subscriber>
							  <Contact>SA HOME LOANS 031 560 5300</Contact>
							  <EnquiryAmount>000000</EnquiryAmount>
							  <EnquiryTypeCode>45</EnquiryTypeCode>
							  <EnquiryType>OTHER</EnquiryType>
							</EnquiriesNE09>
							<EnquiriesNE09>
							  <RecordSeq>12</RecordSeq>
							  <Part>001</Part>
							  <PartSeq>01</PartSeq>
							  <ConsumerNo>229312537</ConsumerNo>
							  <DateOfEnquiry>20080103</DateOfEnquiry>
							  <Subscriber>S A HOME LOANS</Subscriber>
							  <Contact>SA HOME LOANS 031 560 5300</Contact>
							  <EnquiryAmount>000000</EnquiryAmount>
							  <EnquiryTypeCode>45</EnquiryTypeCode>
							  <EnquiryType>OTHER</EnquiryType>
							</EnquiriesNE09>
							<EnquiriesNE09>
							  <RecordSeq>13</RecordSeq>
							  <Part>001</Part>
							  <PartSeq>01</PartSeq>
							  <ConsumerNo>229312537</ConsumerNo>
							  <DateOfEnquiry>20080103</DateOfEnquiry>
							  <Subscriber>OLD MUTUAL BUS</Subscriber>
							  <Contact>SA HOME LOANS 031 560 5300</Contact>
							  <EnquiryAmount>000000</EnquiryAmount>
							  <EnquiryTypeCode>45</EnquiryTypeCode>
							  <EnquiryType>OTHER</EnquiryType>
							</EnquiriesNE09>
							<EnquiriesNE09>
							  <RecordSeq>99</RecordSeq>
							  <Part>001</Part>
							  <PartSeq>01</PartSeq>
							  <ConsumerNo>229312537</ConsumerNo>
							  <DateOfEnquiry>20071115</DateOfEnquiry>
							  <Subscriber>S A HOME LOANS</Subscriber>
							  <Contact>MR DAVE WRIGHT 031 560 5300</Contact>
							  <EnquiryAmount>000000</EnquiryAmount>
							  <EnquiryTypeCode>45</EnquiryTypeCode>
							  <EnquiryType>OTHER</EnquiryType>
							</EnquiriesNE09>
						  </EnquiriesNE09>
						  <EmploymentNM04 xmlns="https://secure.transunion.co.za/TUBureau">
							<EmploymentNM04>
							  <RecordSeq>01</RecordSeq>
							  <Part>001</Part>
							  <PartSeq>01</PartSeq>
							  <ConsumerNo>229312537</ConsumerNo>
							  <InformationDate>20000310</InformationDate>
							  <Occupation />
							  <EmployerName>NEW HORIZONS COMPUTER LEARNING</EmployerName>
							  <EmploymentPeriod>00</EmploymentPeriod>
							</EmploymentNM04>
							<EmploymentNM04>
							  <RecordSeq>02</RecordSeq>
							  <Part>001</Part>
							  <PartSeq>01</PartSeq>
							  <ConsumerNo>229312537</ConsumerNo>
							  <InformationDate>19990202</InformationDate>
							  <Occupation>WAITER</Occupation>
							  <EmployerName>LEGENDS CAFE</EmployerName>
							  <EmploymentPeriod>00</EmploymentPeriod>
							</EmploymentNM04>
						  </EmploymentNM04>
						</BureauResponse>', 'Success', @ADUser)

					Commit Transaction
				End
			Else
				Begin
					Rollback Transaction
				End

		fetch next from ITCLegalEntity into @LegalEntity

	end -- LE cursor

	close ITCLegalEntity
	deallocate ITCLegalEntity;

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
	