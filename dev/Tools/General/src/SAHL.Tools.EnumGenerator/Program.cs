using CommandLine;
using SAHL.Tools.EnumGenerator.Commands;
using SAHL.Tools.EnumGenerator.Templates;
using System;
using System.Collections.Generic;
using System.IO;

namespace SAHL.Tools.EnumGenerator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Schema,  TableName,  KeyColumn,  DescriptionColumn,  Where clause (optional),    Enumeration name(optional)
            List<String> enumsToGenerate = new List<string>();
            enumsToGenerate.Add("dbo,GenericKeyType,GenericKeyTypeKey,Description");
            enumsToGenerate.Add("fin,FinancialAdjustmentType,FinancialAdjustmentTypeKey,Description");
            enumsToGenerate.Add("dbo,StageDefinitionGroup,StageDefinitionGroupKey,Description");
            enumsToGenerate.Add("dbo,MortgageLoanPurpose,MortgageLoanPurposeKey,Description");
            enumsToGenerate.Add("dbo,OfferStatus,OfferStatusKey,Description");
            enumsToGenerate.Add("dbo,OfferType,OfferTypeKey,Description");
            enumsToGenerate.Add("dbo,LegalEntityExceptionStatus,LegalEntityExceptionStatusKey,Description");
            enumsToGenerate.Add("dbo,RemunerationType,RemunerationTypeKey,Description");
            enumsToGenerate.Add("dbo,ReasonType,ReasonTypeKey,Description");
            enumsToGenerate.Add("dbo,ReasonTypeGroup,ReasonTypeGroupKey,Description");
            enumsToGenerate.Add("dbo,LegalEntityStatus,LegalEntityStatusKey,Description");
            enumsToGenerate.Add("dbo,Insurer,InsurerKey,Description");
            enumsToGenerate.Add("dbo,FinancialServiceGroup,FinancialServiceGroupKey,Description");
            enumsToGenerate.Add("dbo,HocStatus,HocStatusKey,Description");
            enumsToGenerate.Add("dbo,HOCInsurer,HOCInsurerKey,Description,WHERE HOCInsurerStatus = 1");
            enumsToGenerate.Add("dbo,HOCSubsidence,HOCSubsidenceKey,Description");
            enumsToGenerate.Add("dbo,HOCConstruction,HOCConstructionKey,Description");
            enumsToGenerate.Add("dbo,HOCRoof,HOCRoofKey,Description");
            enumsToGenerate.Add("dbo,Gender,GenderKey,Description");
            enumsToGenerate.Add("dbo,MaritalStatus,MaritalStatusKey,Description");
            enumsToGenerate.Add("dbo,PopulationGroup,PopulationGroupKey,Description");
            enumsToGenerate.Add("dbo,Education,EducationKey,Description");
            enumsToGenerate.Add("dbo,Language,LanguageKey,Description");
            enumsToGenerate.Add("dbo,CitizenType,CitizenTypeKey,Description");
            enumsToGenerate.Add("dbo,OriginationSource,OriginationSourceKey,Description");
            enumsToGenerate.Add("dbo,DisbursementStatus,DisbursementStatusKey,Description");
            enumsToGenerate.Add("dbo,OfferInformationType,OfferInformationTypeKey,Description");
            enumsToGenerate.Add("dbo,OnlineStatementFormat,OnlineStatementFormatKey,Description");
            enumsToGenerate.Add("dbo,SubsidyProviderType,SubsidyProviderTypeKey,Description");
            enumsToGenerate.Add("dbo,OfferRoleType,OfferRoleTypeKey,Description");
            enumsToGenerate.Add("dbo,FinancialServicePaymentType,FinancialServicePaymentTypeKey,Description");
            enumsToGenerate.Add("dbo,DataProvider,DataProviderKey,Description");
            enumsToGenerate.Add("dbo,DataService,DataServiceKey,Description");
            enumsToGenerate.Add("dbo,ValuationStatus,ValuationStatusKey,Description");
            enumsToGenerate.Add("dbo,FutureDatedChangeType,FutureDatedChangeTypeKey,Description");
            enumsToGenerate.Add("dbo,OfferAttributeType,OfferAttributeTypeKey,Description");
            enumsToGenerate.Add("dbo,OfferAttributeTypeGroup,OfferAttributeTypeGroupKey,Description");
            enumsToGenerate.Add("dbo,CapStatus,CapStatusKey,Description");
            enumsToGenerate.Add("dbo,AssetLiabilityType,AssetLiabilityTypeKey,Description");
            enumsToGenerate.Add("dbo,BulkBatchStatus,BulkBatchStatusKey,Description");
            enumsToGenerate.Add("dbo,BulkBatchType,BulkBatchTypeKey,Description");
            enumsToGenerate.Add("dbo,LegalEntityRelationshipType,RelationshipTypeKey,Description");
            enumsToGenerate.Add("dbo,ReportParameterType,ReportParameterTypeKey,Description");
            enumsToGenerate.Add("dbo,MortgageLoanPurposeGroup,MortgageLoanPurposeGroupKey,Description");
            enumsToGenerate.Add("dbo,ExternalRoleTypeGroup,ExternalRoleTypeGroupKey,Description");
            enumsToGenerate.Add("dbo,OfferRoleAttributeType,OfferRoleAttributeTypeKey,Description");
            enumsToGenerate.Add("dbo,DetailClass,DetailClassKey,Description");
            enumsToGenerate.Add("dbo,ValuationRoofType,ValuationRoofTypeKey,Description");
            enumsToGenerate.Add("dbo,OfferDeclarationAnswer,OfferDeclarationAnswerKey,Description");
            enumsToGenerate.Add("dbo,ExpenseType,ExpenseTypeKey,Description");
            enumsToGenerate.Add("dbo,OfferRoleTypeGroup,OfferRoleTypeGroupKey,Description");
            enumsToGenerate.Add("dbo,ReportType,ReportTypeKey,Description");
            enumsToGenerate.Add("dbo,DisbursementType,DisbursementTypeKey,Description");
            enumsToGenerate.Add("dbo,AffordabilityType,AffordabilityTypeKey,Description");
            enumsToGenerate.Add("dbo,DisbursementTransactionType,DisbursementTransactionTypeKey,Description");
            enumsToGenerate.Add("dbo,QuickCashPaymentType,QuickCashPaymentTypeKey,Description");
            enumsToGenerate.Add("dbo,ApplicantType,ApplicantTypeKey,Description");
            enumsToGenerate.Add("dbo,PaymentType,PaymentTypeKey,Description");
            enumsToGenerate.Add("dbo,AccountIndicationType,AccountIndicationTypeKey,Description");
            enumsToGenerate.Add("dbo,ImportStatus,ImportStatusKey,Description");
            enumsToGenerate.Add("dbo,OccupancyType,OccupancyTypeKey,Description");
            enumsToGenerate.Add("dbo,SalutationType,SalutationKey,Description");
            enumsToGenerate.Add("dbo,RuleExclusionSet,RuleExclusionSetKey,Description");
            enumsToGenerate.Add("dbo,CAPPaymentOption,CAPPaymentOptionKey,Description");
            enumsToGenerate.Add("dbo,RecurringTransactionType,RecurringTransactionTypeKey,Description");
            enumsToGenerate.Add("dbo,DeedsPropertyType,DeedsPropertyTypeKey,Description");
            enumsToGenerate.Add("dbo,ConditionSet,ConditionSetKey,Description");
            enumsToGenerate.Add("dbo,MessageType,MessageTypeKey,Description");
            enumsToGenerate.Add("dbo,BatchTransactionStatus,BatchTransactionStatusKey,Description");
            enumsToGenerate.Add("dbo,TitleType,TitleTypeKey,Description");
            enumsToGenerate.Add("fin,FinancialAdjustmentSource,FinancialAdjustmentSourceKey,Description");
            enumsToGenerate.Add("dbo,RoundRobinPointer,RoundRobinPointerKey,Description");
            enumsToGenerate.Add("dbo,LifePolicyType,LifePolicyTypeKey,Description");
            enumsToGenerate.Add("dbo,OrganisationType,OrganisationTypeKey,Description");
            enumsToGenerate.Add("survey,AnswerType,AnswerTypeKey,Description");
            enumsToGenerate.Add("dbo,BehaviouralScoreCategory,BehaviouralScoreCategoryKey,Description");
            enumsToGenerate.Add("dbo,MarketingOptionRelevance,MarketingOptionRelevanceKey,Description");
            enumsToGenerate.Add("dbo,CampaignTargetResponse,CampaignTargetResponseKey,Description");
            enumsToGenerate.Add("dbo,MarketingOption,MarketingOptionKey,Description");
            enumsToGenerate.Add("dbo,ContentType,ContentTypeKey,Description");
            enumsToGenerate.Add("dbo,ExternalRoleType,ExternalRoleTypeKey,Description");
            enumsToGenerate.Add("dbo,WorkflowRoleTypeGroup,WorkflowRoleTypeGroupKey,Description");
            enumsToGenerate.Add("dbo,WorkflowRoleType,WorkflowRoleTypeKey,Description");
            enumsToGenerate.Add("debtcounselling,ProposalStatus,ProposalStatusKey,Description");
            enumsToGenerate.Add("debtcounselling,ProposalType,ProposalTypeKey,Description");
            enumsToGenerate.Add("debtcounselling,CourtType,CourtTypeKey,Description");
            enumsToGenerate.Add("debtcounselling,HearingType,HearingTypeKey,Description");
            enumsToGenerate.Add("debtcounselling,DebtCounsellingStatus,DebtCounsellingStatusKey,Description");
            enumsToGenerate.Add("dbo,FormatType,FormatTypeKey,Description");
            enumsToGenerate.Add("dbo,RateAdjustmentGroup,RateAdjustmentGroupKey,Description");
            enumsToGenerate.Add("dbo,CorrespondenceTemplate,CorrespondenceTemplateKey,Name");
            enumsToGenerate.Add("fin,FinancialAdjustmentStatus,FinancialAdjustmentStatusKey,Description");
            enumsToGenerate.Add("fin,BalanceType,BalanceTypeKey,Description");
            enumsToGenerate.Add("fin,TransactionGroup,TransactionGroupKey,Description");
            enumsToGenerate.Add("product,FinancialServiceAttributeType,FinancialServiceAttributeTypeKey,Description");
            enumsToGenerate.Add("dbo,CancellationReason,CancellationReasonKey,Description");
            enumsToGenerate.Add("dbo,riskMatrix,RiskMatrixKey,Description");
            enumsToGenerate.Add("dbo,ExpenseTypeGroup,ExpenseTypeGroupKey,Description");
            enumsToGenerate.Add("dbo,AffordabilityTypeGroup,AffordabilityTypeGroupKey,Description");
            enumsToGenerate.Add("dbo,AddressType,AddressTypeKey,Description");
            //Newly added
            enumsToGenerate.Add("dbo,ACBType,ACBTypeNumber,ACBTypeDescription");
            enumsToGenerate.Add("dbo,AccountInformationType,AccountInformationTypeKey,Description");
            enumsToGenerate.Add("dbo,AccountStatus,AccountStatusKey,Description");
            enumsToGenerate.Add("dbo,AddressFormat,AddressFormatKey,Description");
            enumsToGenerate.Add("dbo,CAPPaymentOption,CAPPaymentOptionKey,Description");
            enumsToGenerate.Add("dbo,ClaimStatus,ClaimStatusKey,Description");
            enumsToGenerate.Add("dbo,ClaimType,ClaimTypeKey,Description");
            enumsToGenerate.Add("dbo,ClientOfferStatus,ClientOfferStatusKey,Description");
            enumsToGenerate.Add("dbo,ConditionType,ConditionTypeKey,Description");
            enumsToGenerate.Add("dbo,CreditCriteriaAttributeType,CreditCriteriaAttributeTypeKey,Description");
            enumsToGenerate.Add("dbo,DocumentGroup,DocumentGroupKey,Description");
            enumsToGenerate.Add("dbo,EmployerBusinessType,EmployerBusinessTypeKey,Description");
            enumsToGenerate.Add("dbo,EmploymentStatus,EmploymentStatusKey,Description");
            enumsToGenerate.Add("dbo,EmploymentType,EmploymentTypeKey,Description");
            enumsToGenerate.Add("dbo,GeneralStatus,GeneralStatusKey,Description");
            enumsToGenerate.Add("dbo,LegalEntityType,LegalEntityTypeKey,Description");
            enumsToGenerate.Add("dbo,LifeInsurableInterestType,LifeInsurableInterestTypeKey,Description");
            enumsToGenerate.Add("dbo,LifePolicyStatus,PolicyStatusKey,Description");
            enumsToGenerate.Add("dbo,MessageType,MessageTypeKey,Description");
            enumsToGenerate.Add("dbo,OfferExceptionTypeGroup,OfferExceptionTypeGroupKey,Description");
            enumsToGenerate.Add("dbo,OfferMarketingSurveyTypeGroup,OfferMarketingSurveyTypeGroupKey,Description");
            enumsToGenerate.Add("dbo,PropertyType,PropertyTypeKey,Description");
            enumsToGenerate.Add("dbo,ResidenceStatus,ResidenceStatusKey,Description");
            enumsToGenerate.Add("dbo,RoleType,RoleTypeKey,Description");
            enumsToGenerate.Add("dbo,TrancheType,TrancheTypeKey,Description");
            enumsToGenerate.Add("dbo,ValuationImprovementType,ValuationImprovementTypeKey,Description");
            enumsToGenerate.Add("dbo,ValuationStatus,ValuationStatusKey,Description");
            enumsToGenerate.Add("deb,BatchStatus,BatchStatusKey,Description");
            enumsToGenerate.Add("deb,TransactionStatus,TransactionStatusKey,Description");
            enumsToGenerate.Add("fin,FinancialAdjustmentType,FinancialAdjustmentTypeKey,Description");
            enumsToGenerate.Add("spv,SPVAttributeType,SPVAttributeTypeKey,Description");
            enumsToGenerate.Add("spv,SPVFeeType,SPVFeeTypeKey,Description");
            enumsToGenerate.Add("dbo,Product,ProductKey,Description");
            enumsToGenerate.Add("dbo,AssetLiabilitySubType,AssetLiabilitySubTypeKey,Description");
            enumsToGenerate.Add("dbo,DomainProcessStatus,DomainProcessStatusKey,Description");
            enumsToGenerate.Add("dbo,DisabilityType,DisabilityTypeKey,Description");
            enumsToGenerate.Add("dbo,AffordabilityAssessmentStatus,AffordabilityAssessmentStatusKey,Description");
            enumsToGenerate.Add("dbo,AffordabilityAssessmentItemCategory,AffordabilityAssessmentItemCategoryKey,Description");
            enumsToGenerate.Add("dbo,AffordabilityAssessmentItemType,AffordabilityAssessmentItemTypeKey,Description");
            enumsToGenerate.Add("dbo,InvoiceStatus,InvoiceStatusKey,Description");
            enumsToGenerate.Add("dbo,ThirdPartyType,ThirdPartyTypeKey,Description");
            enumsToGenerate.Add("orgstruct,Capability, CapabilityKey, Description");
            enumsToGenerate.Add("dbo,CATSPaymentBatchType,CATSPaymentBatchTypeKey,Description");
            enumsToGenerate.Add("dbo,CATSPaymentBatchStatus,CATSPaymentBatchStatusKey,Description");
            CommandLineArguments commands = new CommandLineArguments();
            Parser parser = new Parser();
            bool parserResult = parser.ParseArguments(args, commands);
            if (parserResult == true)
            {
                var generator = new EnumGenerator();
                string connectionString = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User Id={2};Password={3};Connect Timeout=300;",
                                                        commands.Server, commands.Database, commands.UserName, commands.Password);
                IEnumerable<EnumData> enumDataList = generator.GenerateEnums(connectionString, commands.OutputDirectory, commands.Database, enumsToGenerate);

                foreach (var enumData in enumDataList)
                {
                    EnumTemplate enumTemplate = new EnumTemplate(enumData.EnumName, enumData.EnumValues);
                    string sourceCode = enumTemplate.TransformText();

                    string outputPath = Path.Combine(commands.OutputDirectory, string.Format("{0}Enum.cs", enumData.EnumName));
                    using (StreamWriter sw = new StreamWriter(outputPath))
                    {
                        sw.Write(sourceCode);
                        sw.Flush();
                    }

                }

                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Error parsing command line arguments.");
                Environment.Exit(-1);
            }
        }
    }
}