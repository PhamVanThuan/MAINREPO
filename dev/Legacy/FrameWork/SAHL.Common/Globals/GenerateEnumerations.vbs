Option Explicit

' set up the array of items to script out - each entry is a comma-delimited entry of
' Schema,TableName,KeyColumn,DescriptionColumn,Where clause (optional),Enumeration name(optional)
' make the array size 1 less than the number of items you actually have
Dim arrTables(120)
arrTables(0) = "dbo,ACBType,ACBTypeNumber,ACBTypeDescription"
arrTables(1) = "dbo,AddressFormat,AddressFormatKey,Description"
arrTables(2) = "dbo,AddressType,AddressTypeKey,Description"
arrTables(3) = "dbo,FinancialServiceType,FinancialServiceTypeKey,Description"
arrTables(4) = "dbo,GeneralStatus,GeneralStatusKey,Description"
arrTables(5) = "dbo,Product,ProductKey,Description"
arrTables(6) = "dbo,RoleType,RoleTypeKey,Description"
arrTables(7) = "dbo,TextStatementType,TextStatementTypeKey,Description"
arrTables(8) = "dbo,AccountStatus,AccountStatusKey,Description"
arrTables(9) = "dbo,LegalEntityType,LegalEntityTypeKey,Description"
arrTables(10) = "dbo,LifeInsurableInterestType,LifeInsurableInterestTypeKey,Description"
arrTables(11) = "dbo,LifePolicyStatus,PolicyStatusKey,Description"
arrTables(12) = "dbo,EmploymentStatus,EmploymentStatusKey,Description"
arrTables(13) = "dbo,EmploymentType,EmploymentTypeKey,Description"
arrTables(14) = "dbo,CorrespondenceMedium,CorrespondenceMediumKey,Description"
arrTables(15) = "dbo,GenericKeyType,GenericKeyTypeKey,Description"
arrTables(16) = "fin,FinancialAdjustmentType,FinancialAdjustmentTypeKey,Description"
arrTables(17) = "dbo,StageDefinitionGroup,StageDefinitionGroupKey,Description"
arrTables(18) = "dbo,MortgageLoanPurpose,MortgageLoanPurposeKey,Description"
arrTables(19) = "dbo,OfferStatus,OfferStatusKey,Description"
arrTables(20) = "dbo,OfferType,OfferTypeKey,Description"
arrTables(21) = "dbo,LegalEntityExceptionStatus,LegalEntityExceptionStatusKey,Description"
arrTables(22) = "dbo,RemunerationType,RemunerationTypeKey,Description"
arrTables(23) = "dbo,ReasonType,ReasonTypeKey,Description"
arrTables(24) = "dbo,ReasonTypeGroup,ReasonTypeGroupKey,Description"
arrTables(25) = "dbo,LegalEntityStatus,LegalEntityStatusKey,Description"
arrTables(26) = "dbo,Insurer,InsurerKey,Description"
arrTables(27) = "dbo,FinancialServiceGroup,FinancialServiceGroupKey,Description"
arrTables(28) = "dbo,HocStatus,HocStatusKey,Description"
arrTables(29) = "dbo,HOCInsurer,HOCInsurerKey,Description,WHERE HOCInsurerStatus = 1"
arrTables(30) = "dbo,HOCSubsidence,HOCSubsidenceKey,Description"
arrTables(31) = "dbo,HOCConstruction,HOCConstructionKey,Description"
arrTables(32) = "dbo,HOCRoof,HOCRoofKey,Description"
arrTables(33) = "dbo,Gender,GenderKey,Description"
arrTables(34) = "dbo,MaritalStatus,MaritalStatusKey,Description"
arrTables(35) = "dbo,PopulationGroup,PopulationGroupKey,Description"
arrTables(36) = "dbo,Education,EducationKey,Description"
arrTables(37) = "dbo,Language,LanguageKey,Description"
arrTables(38) = "dbo,CitizenType,CitizenTypeKey,Description"
arrTables(39) = "dbo,OriginationSource,OriginationSourceKey,Description"
arrTables(40) = "dbo,DisbursementStatus,DisbursementStatusKey,Description"
arrTables(41) = "dbo,OfferInformationType,OfferInformationTypeKey,Description"
arrTables(42) = "dbo,OnlineStatementFormat,OnlineStatementFormatKey,Description"
arrTables(43) = "dbo,SubsidyProviderType,SubsidyProviderTypeKey,Description"
arrTables(44) = "dbo,OfferRoleType,OfferRoleTypeKey,Description"
arrTables(45) = "dbo,FinancialServicePaymentType,FinancialServicePaymentTypeKey,Description"
arrTables(46) = "dbo,DataProvider,DataProviderKey,Description"
arrTables(47) = "dbo,DataService,DataServiceKey,Description"
arrTables(48) = "dbo,ValuationStatus,ValuationStatusKey,Description"
arrTables(49) = "dbo,FutureDatedChangeType,FutureDatedChangeTypeKey,Description"
arrTables(50) = "dbo,OfferAttributeType,OfferAttributeTypeKey,Description"
arrTables(51) = "dbo,CapStatus,CapStatusKey,Description"
arrTables(52) = "dbo,AssetLiabilityType,AssetLiabilityTypeKey,Description"
arrTables(53) = "dbo,BulkBatchStatus,BulkBatchStatusKey,Description"
arrTables(54) = "dbo,BulkBatchType,BulkBatchTypeKey,Description"
arrTables(55) = "dbo,LegalEntityRelationshipType,RelationshipTypeKey,Description"
arrTables(56) = "dbo,ReportParameterType,ReportParameterTypeKey,Description"
arrTables(57) = "dbo,MortgageLoanPurposeGroup,MortgageLoanPurposeGroupKey,Description"
arrTables(58) = "dbo,ExternalRoleTypeGroup,ExternalRoleTypeGroupKey,Description"
arrTables(59) = "dbo,OfferRoleAttributeType,OfferRoleAttributeTypeKey,Description"
arrTables(60) = "dbo,DetailClass,DetailClassKey,Description"
arrTables(61) = "dbo,ValuationRoofType,ValuationRoofTypeKey,Description"
arrTables(62) = "dbo,OfferDeclarationAnswer,OfferDeclarationAnswerKey,Description"
arrTables(63) = "dbo,ExpenseType,ExpenseTypeKey,Description"
arrTables(64) = "dbo,OfferRoleTypeGroup,OfferRoleTypeGroupKey,Description"
arrTables(65) = "dbo,ReportType,ReportTypeKey,Description"
arrTables(66) = "dbo,DisbursementType,DisbursementTypeKey,Description"
arrTables(67) = "dbo,AffordabilityType,AffordabilityTypeKey,Description"
arrTables(68) = "dbo,DisbursementTransactionType,DisbursementTransactionTypeKey,Description"
arrTables(69) = "dbo,QuickCashPaymentType,QuickCashPaymentTypeKey,Description"
arrTables(70) = "dbo,ApplicantType,ApplicantTypeKey,Description"
arrTables(71) = "dbo,PaymentType,PaymentTypeKey,Description"
arrTables(72) = "dbo,AccountIndicationType,AccountIndicationTypeKey,Description"
arrTables(73) = "dbo,ImportStatus,ImportStatusKey,Description"
arrTables(74) = "dbo,OccupancyType,OccupancyTypeKey,Description"
arrTables(75) = "dbo,SalutationType,SalutationKey,Description"
arrTables(76) = "dbo,RuleExclusionSet,RuleExclusionSetKey,Description"
arrTables(77) = "dbo,CAPPaymentOption,CAPPaymentOptionKey,Description"
arrTables(78) = "dbo,RecurringTransactionType,RecurringTransactionTypeKey,Description"
arrTables(79) = "dbo,DeedsPropertyType,DeedsPropertyTypeKey,Description"
arrTables(80) = "dbo,ConditionSet,ConditionSetKey,Description"
arrTables(81) = "dbo,MessageType,MessageTypeKey,Description"
arrTables(82) = "dbo,BatchTransactionStatus,BatchTransactionStatusKey,Description"
arrTables(83) = "dbo,TitleType,TitleTypeKey,Description"
arrTables(84) = "fin,FinancialAdjustmentSource,FinancialAdjustmentSourceKey,Description"
arrTables(85) = "dbo,RoundRobinPointer,RoundRobinPointerKey,Description"
arrTables(86) = "dbo,LifePolicyType,LifePolicyTypeKey,Description"
arrTables(87) = "dbo,OrganisationType,OrganisationTypeKey,Description"
arrTables(88) = "survey,AnswerType,AnswerTypeKey,Description"
arrTables(89) = "dbo,BehaviouralScoreCategory,BehaviouralScoreCategoryKey,Description"
arrTables(90) = "dbo,MarketingOptionRelevance,MarketingOptionRelevanceKey,Description"
arrTables(91) = "dbo,CampaignTargetResponse,CampaignTargetResponseKey,Description"
arrTables(92) = "dbo,MarketingOption,MarketingOptionKey,Description"
arrTables(93) = "dbo,ContentType,ContentTypeKey,Description"
arrTables(94) = "dbo,ExternalRoleType,ExternalRoleTypeKey,Description"
arrTables(95) = "dbo,WorkflowRoleTypeGroup,WorkflowRoleTypeGroupKey,Description"
arrTables(96) = "dbo,WorkflowRoleType,WorkflowRoleTypeKey,Description"
arrTables(97) = "debtcounselling,ProposalStatus,ProposalStatusKey,Description"
arrTables(98) = "debtcounselling,ProposalType,ProposalTypeKey,Description"
arrTables(99) = "debtcounselling,CourtType,CourtTypeKey,Description"
arrTables(100) = "debtcounselling,HearingType,HearingTypeKey,Description"
arrTables(101) = "debtcounselling,DebtCounsellingStatus,DebtCounsellingStatusKey,Description"
arrTables(102) = "dbo,FormatType,FormatTypeKey,Description"
arrTables(103) = "dbo,RateAdjustmentGroup,RateAdjustmentGroupKey,Description"
arrTables(104) = "dbo,CorrespondenceTemplate,CorrespondenceTemplateKey,Name"
arrTables(105) = "fin,FinancialAdjustmentStatus,FinancialAdjustmentStatusKey,Description"
arrTables(106) = "fin,BalanceType,BalanceTypeKey,Description"
arrTables(107) = "fin,TransactionGroup,TransactionGroupKey,Description"
arrTables(108) = "product,FinancialServiceAttributeType,FinancialServiceAttributeTypeKey,Description"
arrTables(109) = "dbo,CancellationReason,CancellationReasonKey,Description"
arrTables(110) = "dbo,riskMatrix,RiskMatrixKey,Description"
arrTables(111) = "dbo,ExpenseTypeGroup,ExpenseTypeGroupKey,Description"
arrTables(112) = "dbo,AffordabilityTypeGroup,AffordabilityTypeGroupKey,Description"
arrTables(113) = "dbo,ClaimType,ClaimTypeKey,Description"
arrTables(114) = "dbo,ClaimStatus,ClaimStatusKey,Description"
arrTables(115) = "dbo,ReportFormatType,ReportFormatTypeKey,Description"
arrTables(116) = "dbo,DisabilityType,DisabilityTypeKey,Description"
arrTables(117) = "dbo,DisabilityClaimStatus,DisabilityClaimStatusKey,Description"
arrTables(118) = "dbo,DisabilityPaymentStatus,DisabilityPaymentStatusKey,Description"
arrTables(119) = "dbo,AffordabilityAssessmentStatus,AffordabilityAssessmentStatusKey,Description"
arrTables(120) = "dbo,AccountInformationType,AccountInformationTypeKey,Description"

Dim iTabs : iTabs = 0

' create a connection to the database
Dim sConn : sConn = "Provider=SQLOLEDB.1;Data Source=DEVC03; Initial Catalog=2AM;user id = 'ServiceArchitect';password='Service1Architect'"
Dim oConn
Dim oFS
Dim oStream

' open the connection to the database
Set oConn = CreateObject("ADODB.Connection")
oConn.Open(sConn)


Dim arrFiles(0)
arrFiles(0) = "Enumerations.cs"
Dim fileName
Dim nameSpace : nameSpace = "namespace Client.Enumerations"
Set oFS = CreateObject("Scripting.FileSystemObject")
For Each fileName in arrFiles
  ' create the new text file
  Set oStream = oFS.CreateTextFile(fileName, True)
  
  ' header 
  oStream.WriteLine("using System;")
  oStream.WriteLine("")
  oStream.WriteLine("// **************************** DO NOT MODIFY ****************************")
  oStream.WriteLine("// This file is automatically generated using GenerateEnumerations.vbs.")
  oStream.WriteLine("// It can be regenerated by double clicking the vbs file.")
  oStream.WriteLine("// ***********************************************************************")
  If fileName = "Enumerations.cs" Then
    nameSpace = "namespace SAHL.Common.Globals"
  End If
  oStream.WriteLine(nameSpace)
  oStream.WriteLine("{")
  oStream.WriteLine("")
  
  iTabs = iTabs + 1
  
  ' loop through all tables to script out, stored in the array
  Dim sTable
  Dim sql
  Dim oRs
  Dim i
  Set oRs = CreateObject("ADODB.RecordSet")
  For Each sTable In arrTables
  	Dim arrItems : arrItems = Split(sTable, ",")
      Dim sSchemaName : sSchemaName = arrItems(0)
      Dim sTableName : sTableName = arrItems(1)
      Dim sKeyCol : sKeyCol = arrItems(2)
      Dim sDescCol : sDescCol = arrItems(3)
  	Dim sWhere : sWhere = ""
      Dim iRecNum : iRecNum = 0
      Dim sValues
      Dim sEnumName : sEnumName = sTableName
  
      ' if table name ends with an "s", insert an "e" so we don't get names ending with double "s"
      If Right(sEnumName, 1) = "s" Then
          sEnumName = sEnumName & "e"
      End If
  	sEnumName = sEnumName & "s"
  	
  	' check for where clause
  	If UBound(arrItems) >= 4 Then
  		If Len(Trim(arrItems(4))) > 0 Then
  			sWhere = " " & Trim(arrItems(4))
  		End If
  	End If
  	
  	' if an enumeration name has been specified, then override the value here
  	If UBound(arrItems) >= 5 Then
  		If Len(Trim(arrItems(5))) > 0 Then
  			sEnumName = Trim(arrItems(5))
  		End If
  	End If
  	
      
      ' write the enumeration    
      oStream.WriteLine(GetTabs & "/// <summary>")
      oStream.WriteLine(GetTabs & "/// Automatically generated enumeration for static values pertaining to " & sTableName & ".")
      oStream.WriteLine(GetTabs & "/// </summary>")
      oStream.WriteLine(GetTabs & "public enum " & sEnumName)
      oStream.WriteLine(GetTabs & "{")
      iTabs = iTabs + 1
  
      ' get all the values from the table
      sql = "SELECT * FROM " & sSchemaName & "." & sTableName & sWhere
      oRs.Open sql, oConn, 3
  
      While iRecNum < oRs.RecordCount
          Dim sLine : sLine = GetTabs & Replace(oRs(sDescCol), " ", "") & " = " & oRs(sKeyCol)
          If iRecNum <> (oRs.RecordCount - 1) Then
              sLine = sLine & ","
          End If
          
          ' this part is a hack - remove anything that can cause compilation errors or is unwanted
          sLine = Replace(sLine, "(HOC)", "")
          sLine = Replace(sLine, Chr(45), "", 1, -1, 1)
          sLine = Replace(sLine, "–", "")
          sLine = Replace(sLine, "&", "")
          sLine = Replace(sLine, "+", "And")
          sLine = Replace(sLine, "/", "_")
  	    sLine = Replace(sLine, "'","")
  	    sLine = Replace(sLine, ".","")
  	    sLine = Replace(sLine, "(","")
  	    sLine = Replace(sLine, ")","")
          
          oStream.WriteLine(sLine)
          oRs.MoveNext
          iRecNum = iRecNum + 1
      Wend
      oRs.Close
      ' oStream.WriteLine sValues
      
      iTabs = iTabs - 1
      oStream.WriteLine(GetTabs & "}")
      oStream.WriteLine(GetTabs)
      'WriteLine sValues
      ' MsgBox sql
  Next
  
  ' footer
  iTabs = iTabs - 1
  oStream.WriteLine("}")
  
  ' close the stream and clean up file system objects
  oStream.Close
Next

Set oStream = Nothing
Set oFS = Nothing

' close and clean up
oConn.Close
Set oConn = Nothing

MsgBox "The FrameWork and Client enumerations have been rebuilt - please recompile the framework to ensure there are no problems with the new file." & vbNewLine _
      & "Please remove the reference to the Client in any maps that are using enumerations and re-add the reference and recompile the map.", 0, "GenerateEnumerations"
     
Function GetTabs()
    Dim result
    Dim cnt : cnt = 0
    While cnt < iTabs
        result = result & oStream.Write(Chr(9))
        cnt = cnt + 1
    Wend
    GetTabs = result
End Function
