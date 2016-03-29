using SAHL.Core.Data;
using SAHL.Services.Query.Models.Lookup;

namespace SAHL.Services.Query.DataManagers.Statements.Lookup
{

    public class GetInvoiceLineItemDescriptionStatement : ISqlStatement<InvoiceLineItemDescriptionDataModel>
    {
        public string GetStatement()
        {
            return @"Select 
                        InvoiceLineItemDescriptionKey As Id,
                        InvoiceLineItemDescription as Description,                    
                        InvoiceLineItemCategoryKey
                    From [2AM].[dbo].[InvoiceLineItemDescription]";
        }
    }

}