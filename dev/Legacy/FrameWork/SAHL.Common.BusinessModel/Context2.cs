using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Context_DAO
    /// </summary>
    public partial class Context : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Context_DAO>, IContext
    {
        public static string BuildQuery(IList<IFilter> filters, IList<IInternalRole> roles, IContext context, params string[] filtersToApply)
        {
            var internalRole = roles.Where(x => x.ContextTableKey == context.Key).FirstOrDefault();
            if (internalRole == null)
            {
                return String.Empty;
            }

            const string insertIntoOffers = "insert into #offers select distinct {0}.{1} from {2} {3} \n";

            var sqlBuilder = new StringBuilder();

            //Build the Sql
            sqlBuilder.AppendFormat(insertIntoOffers, context.Alias, context.PrimaryKeyColumn, context.TableName, context.Alias);

            sqlBuilder.AppendFormat(" join {0} {1} on {1}.{2} = {3}.{4} ", internalRole.TableName,
                                                                       internalRole.Alias,
                                                                       internalRole.PrimaryKeyColumn,
                                                                       context.Alias,
                                                                       internalRole.ContextTableJoinKey);

            //Apply the Filters if there are any
            foreach (var filter in filters.Where(x => x.ContextKey == context.Key && filtersToApply.Contains(x.Name)))
            {
                sqlBuilder.AppendLine(filter.Query);
            }
            return sqlBuilder.ToString();
        }
    }
}