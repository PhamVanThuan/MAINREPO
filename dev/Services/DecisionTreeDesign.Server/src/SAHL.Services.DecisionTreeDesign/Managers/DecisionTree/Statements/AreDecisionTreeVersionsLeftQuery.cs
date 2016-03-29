using SAHL.Core.Attributes;
using SAHL.Core.Data;
using System;

namespace SAHL.Services.DecisionTreeDesign.Managers.DecisionTree.Statements
{
    [NolockConventionExclude]
    public class AreDecisionTreeVersionsLeftQuery : ISqlStatement<Nullable<Int32>>
    {
        public Guid DecisionTreeId { get; protected set; }

        public AreDecisionTreeVersionsLeftQuery(Guid decisionTreeId)
        {
            this.DecisionTreeId = decisionTreeId;
        }

        public string GetStatement()
        {
            return @"SELECT Max(dtv.[Version])
            FROM [DecisionTree].[dbo].[DecisionTreeVersion] dtv (NOLOCK)
            WHERE dtv.DecisionTreeId = @DecisionTreeId";
        }
    }
}