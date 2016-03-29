using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.Data.Models.X2
{
    public class UIStatements_Recalc : IUIStatementsProvider
    {
        public string UIStatementContext
        {
            get { return "SAHL.Tools.Workflow.Common.Database.SecurityRecalculating"; }
        }
        public const string securityrecalculation_deletewhere = "DELETE FROM [x2].[staging].SecurityRecalculation WHERE";
    }
}
