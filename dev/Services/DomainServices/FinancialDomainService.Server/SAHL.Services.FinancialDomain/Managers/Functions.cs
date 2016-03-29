using System;
using System.Globalization;
using System.Linq;

namespace SAHL.Services.FinancialDomain.Managers
{
    public static class Functions
    {
        const string ROW_XML_TEMPLATE = @"
<Row>
    <LTV>{0}</LTV>
	<HasBeenInCompany2>0</HasBeenInCompany2>
	<FLAllowed>0</FLAllowed>
	<TermChangeAllowed>0</TermChangeAllowed>
	<OfferAttributes>{1}</OfferAttributes>
	<LoanAmount>0</LoanAmount>
</Row>";


        public static string GenerateGetValidSPVxml(decimal ltv, string offerAttributesCSV)
        {
            return string.Format(ROW_XML_TEMPLATE, ltv.ToString(CultureInfo.InvariantCulture), offerAttributesCSV);
        }
    }
}