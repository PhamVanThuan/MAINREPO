namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class GetNextApplicationNumberQuery : ISqlStatement<ReservedApplicationNumberDataModel>
    {
        public GetNextApplicationNumberQuery() { }

        public string GetStatement()
        {
            return @"SELECT MIN(ApplicationNumber) as [ApplicationNumber], convert(bit,0) as [IsUsed]  FROM [Capitec].[dbo].[ReservedApplicationNumber] where IsUsed = 0";
        }
    }
}