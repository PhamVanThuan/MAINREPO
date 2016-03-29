using SAHL.Core.UI.Models;
using System;

namespace SAHL.Core.UI.Halo.Tiles.Affordability.Default
{
    public class LegalEntityAffordabilitySummaryMinorTileModel : ITileModel
    {
        public Decimal TotalIncome { get; set; }
        public Decimal TotalExpenses { get; set; }
        public Decimal DisposableIncomeAmount { get; set; }
    }
}