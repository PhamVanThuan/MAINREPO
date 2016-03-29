using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.ClientDomain.Managers.Client.Statements
{
    public class GetClientStatement : ISqlStatement<LegalEntityDataModel>
    {
        public int ClientKey { get; private set; }

        public GetClientStatement(int clientKey)
        {
            this.ClientKey = clientKey;
        }

        public string GetStatement()
        {
            return @"SELECT 
                       [LegalEntityKey]
                      ,[LegalEntityTypeKey]
                      ,[MaritalStatusKey]
                      ,[GenderKey]
                      ,[PopulationGroupKey]
                      ,[IntroductionDate]
                      ,[Salutationkey]
                      ,[FirstNames]
                      ,[Initials]
                      ,[Surname]
                      ,[PreferredName]
                      ,[IDNumber]
                      ,[PassportNumber]
                      ,[TaxNumber]
                      ,[RegistrationNumber]
                      ,[RegisteredName]
                      ,[TradingName]
                      ,[DateOfBirth]
                      ,[HomePhoneCode]
                      ,[HomePhoneNumber]
                      ,[WorkPhoneCode]
                      ,[WorkPhoneNumber]
                      ,[CellPhoneNumber]
                      ,[EmailAddress]
                      ,[FaxCode]
                      ,[FaxNumber]
                      ,[Password]
                      ,[CitizenTypeKey]
                      ,[LegalEntityStatusKey]
                      ,[Comments]
                      ,[LegalEntityExceptionStatusKey]
                      ,[UserID]
                      ,[ChangeDate]
                      ,[EducationKey]
                      ,[HomeLanguageKey]
                      ,[DocumentLanguageKey]
                      ,[ResidenceStatusKey]
                  FROM [2AM].[dbo].[LegalEntity]
                  WHERE LegalEntityKey = @ClientKey";
        }
    }
}