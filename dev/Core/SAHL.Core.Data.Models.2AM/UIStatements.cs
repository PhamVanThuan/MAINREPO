using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models._2AM
{
    public partial class UIStatements : IUIStatementsProvider
    {
        public string UIStatementContext
        {
            get
            {
                return "SAHL.Core.Data.Models._2AM";
            }
        }
    }
}