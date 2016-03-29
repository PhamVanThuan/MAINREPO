using System;

namespace SAHL.Common.BusinessModel.DataTransferObjects
{
    public interface IRow
    {
		string OfferAttributes { get; set; }

        int FLAllowed { get; set; }

        int HasBeenInCompany2 { get; set; }

        decimal LTV { get; set; }

        int TermChangeAllowed { get; set; }

        decimal LoanAmount { get; set; }

        int IsGEPF { get; set; }
    }
}