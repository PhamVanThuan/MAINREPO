using SAHL.Core.Attributes;

namespace SAHL.Core.Data.Models.Capitec.Statements
{
    [NolockConventionExclude]
    public class GetPersonFromIdentityNumberQuery : ISqlStatement<PersonDataModel>
    {
        public GetPersonFromIdentityNumberQuery(string identityNumber)
        {
            this.IdentityNumber = identityNumber;
        }

        public string IdentityNumber { get; protected set; }

        public string GetStatement()
        {
            return @"SELECT [Id],[SalutationEnumId],[FirstName],[Surname],[IdentityNumber]
                    FROM [Capitec].[dbo].[Person] (nolock)
                    WHERE [IdentityNumber] = @IdentityNumber";
        }
    }
}