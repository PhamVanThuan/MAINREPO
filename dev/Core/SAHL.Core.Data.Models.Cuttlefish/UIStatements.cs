using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Cuttlefish
{
    public partial class UIStatements : IUIStatementsProvider
    {
        public string UIStatementContext
        {
            get
            {
                return "SAHL.Core.Data.Models.Cuttlefish";
            }
        }
    }
}