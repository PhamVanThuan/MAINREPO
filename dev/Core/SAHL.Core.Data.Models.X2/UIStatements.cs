using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.X2
{
    public partial class UIStatements : IUIStatementsProvider
    {
        public string UIStatementContext
        {
            get
            {
                return "SAHL.Core.Data.Models.X2";
            }
        }
    }
}