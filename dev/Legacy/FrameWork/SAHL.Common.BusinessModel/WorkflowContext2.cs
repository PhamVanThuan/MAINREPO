using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using System.Linq;

namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.WorkflowContext_DAO
	/// </summary>
	public partial class WorkflowContext : BusinessModelBase<SAHL.Common.BusinessModel.DAO.WorkflowContext_DAO>, IWorkflowContext
	{

        /// <summary>
        /// Build Join
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="workflowDatas"></param>
        /// <param name="workflowContext"></param>
        /// <param name="filtersToApply"></param>
        /// <returns></returns>
		public static string BuildQuery(IList<IFilter> filters, IList<IWorkflowData> workflowDatas, IWorkflowContext workflowContext, params string[] filtersToApply)
		{
			const string insertIntoInstance = "insert into #instances select {0}.*, 2 as ranking from {1} {2} with(nolock) \n";

			var sqlBuilder = new StringBuilder();

			var workflowContextWorkflowDatas = workflowDatas.Where(x => x.WorkflowContextKey == workflowContext.Key);
			if (workflowContextWorkflowDatas == null)
			{
				return String.Empty;
			}

			//Build the Sql
			const string joinDataTable = " join {0} {3} on {1}.{2} = {3}.{4}";
			const string filterOutExistingOffers = " join #offers offers on offers.offerkey = {0}.{1} \n\n";
			const string filterOutExistingInstances = " left join #instances ialready on ialready.id = {1}.{2} and ialready.{2} is null \n\n";
			foreach (var workflowData in workflowContextWorkflowDatas)
			{
				sqlBuilder.AppendFormat(insertIntoInstance, workflowContext.Alias, workflowContext.TableName, workflowContext.Alias);

				sqlBuilder.AppendFormat(joinDataTable, workflowData.TableName,
													   workflowContext.Alias,
													   workflowContext.PrimaryKeyColumn,
													   workflowData.Alias,
													   workflowData.PrimaryKeyColumn);

				sqlBuilder.AppendFormat(filterOutExistingOffers, workflowData.Alias,
																 workflowData.ForeignKeyColumn);

				sqlBuilder.AppendFormat(filterOutExistingInstances, workflowData.TableName,
																	workflowContext.Alias,
																	workflowContext.PrimaryKeyColumn,
																	workflowData.Alias,
																	workflowData.PrimaryKeyColumn);

				//Apply the Filters if there are any
				foreach (var filter in filters.Where(x => x.WorkflowContextKey == workflowContext.Key && filtersToApply.Contains(x.Name)))
				{
					sqlBuilder.AppendLine(filter.Query);
				}
			}



			return sqlBuilder.ToString();
		}
	}
}


