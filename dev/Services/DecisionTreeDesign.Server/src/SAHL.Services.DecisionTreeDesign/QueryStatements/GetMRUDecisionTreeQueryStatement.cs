using SAHL.Core.Attributes;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.DecisionTreeDesign.Models;
using SAHL.Services.Interfaces.DecisionTreeDesign.Queries;
using System;

namespace SAHL.Services.DecisionTreeDesign.QueryStatements
{
    [NolockConventionExclude]
    public class GetMRUDecisionTreeQueryStatement : IServiceQuerySqlStatement<GetMRUDecisionTreeQuery, GetMRUDecisionTreeQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT TOP 5 mrudt.Id, mrudt.UserName, mrudt.TreeId, mrudt.ModifiedDate,dt.Id DecisionTreeId, dt.Name, dt.[Description], dt.IsActive, dt.Thumbnail, dtv.Id DecisionTreeVersionId, IIF(pdt.Id Is Null, 0, 1) IsPublished, dtv.[Version], mrudt.pinned,codt.Username as CurrentlyOpenBy
                    FROM [DecisionTree].[dbo].[UserMRUDecisionTree] mrudt (NOLOCK)
                    LEFT JOIN [DecisionTree].[dbo].[DecisionTreeVersion] dtv (NOLOCK) on dtv.Id = mrudt.TreeId
                    LEFT JOIN [DecisionTree].[dbo].[DecisionTree] dt (NOLOCK) on dt.id = dtv.DecisionTreeId
                    LEFT JOIN [DecisionTree].[dbo].[PublishedDecisionTree] pdt (NOLOCK) on pdt.DecisionTreeVersionId = dtv.Id
                    LEFT OUTER JOIN [DecisionTree].[dbo].[CurrentlyOpenDocument] codt (NOLOCK) ON dtv.Id = codt.DocumentVersionId
                    WHERE  mrudt.UserName = @UserName
                    order by mrudt.pinned  desc, mrudt.ModifiedDate desc";
        }
    }
}