using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using Castle.ActiveRecord.Queries;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(IHelpDeskRepository))]
    public class HelpDeskRepository : AbstractRepositoryBase, IHelpDeskRepository
    {
		public IReadOnlyEventList<IHelpDeskQuery> GetHelpDeskQueryByInstanceID(Int64 instanceID)
		{
			IX2Service x2Service = ServiceFactory.GetService<IX2Service>();
			IDictionary<string, object> hdDict = x2Service.GetX2DataRow(instanceID);

			if (hdDict != null)
			{
				if (hdDict.ContainsKey("HelpDeskQueryKey") && hdDict["HelpDeskQueryKey"] != DBNull.Value)
				{
					int helpDeskQueryKey = Convert.ToInt32(hdDict["HelpDeskQueryKey"]);
					if (helpDeskQueryKey > 0)
						return GetHelpDeskQueryByHelpDeskQueryKey(helpDeskQueryKey);
					else
						return null;
				}
				else
					return null;
			}
			else
				return null;
		}

		public IReadOnlyEventList<IHelpDeskQuery> GetHelpDeskQueryByHelpDeskQueryKey(int helpDeskQueryKey)
		{
			string HQL = "from HelpDeskQuery_DAO HD where HD.Key = ?";
			SimpleQuery<HelpDeskQuery_DAO> q = new SimpleQuery<HelpDeskQuery_DAO>(HQL, helpDeskQueryKey);
			HelpDeskQuery_DAO[] res = q.Execute();
			IEventList<IHelpDeskQuery> list = new DAOEventList<HelpDeskQuery_DAO, IHelpDeskQuery, HelpDeskQuery>(res);
			return new ReadOnlyEventList<IHelpDeskQuery>(list);

		}

		public IReadOnlyEventList<IHelpDeskQuery> GetHelpDeskCallSummaryByLegalEntityKey(int legalEntityKey)
		{
			if (legalEntityKey > 0)
			{
				string sql = @"SELECT HDQ.*
				    FROM HelpDeskQuery HDQ (nolock)
				    JOIN Memo M (nolock) ON HDQ.MemoKey = M.MemoKey AND M.GenericKeyTypeKey = :genericKeyTypeLE
				    WHERE M.GenericKey = :legalEntityKey
				    UNION
				    SELECT HDQ.*
				    FROM HelpDeskQuery HDQ (nolock)
				    JOIN Memo M (nolock) ON HDQ.MemoKey = M.MemoKey AND M.GenericKeyTypeKey = :genericKeyTypeAccount
				    AND M.GenericKey IN (SELECT AccountKey FROM Role (nolock) WHERE LegalEntityKey = :legalEntityKey AND RoleTypeKey = :roleTypeKey) ";

                SimpleQuery<HelpDeskQuery_DAO> q1 = new SimpleQuery<HelpDeskQuery_DAO>(QueryLanguage.Sql, sql);
                q1.SetParameter("legalEntityKey", legalEntityKey);
                q1.SetParameter("roleTypeKey", (int)RoleTypes.MainApplicant);
                q1.SetParameter("genericKeyTypeLE", (int)GenericKeyTypes.LegalEntity);
                q1.SetParameter("genericKeyTypeAccount", (int)GenericKeyTypes.Account);
                q1.AddSqlReturnDefinition(typeof(HelpDeskQuery_DAO), "HDQ");

				HelpDeskQuery_DAO[] res1 = q1.Execute();

				if (res1 == null)
					return null;

				if (res1.GetLength(0) == 0)
					return null;

				IEventList<IHelpDeskQuery> list1 = new DAOEventList<HelpDeskQuery_DAO, IHelpDeskQuery, HelpDeskQuery>(res1);

				if (list1 == null)
					return null;

				if (list1.Count < 1)
					return null;

				string HQL = String.Format("FROM HelpDeskQuery_DAO HD WHERE HD.Key IN ({0})", GetHelpDeskQueryKeysAsStringForQuery(new ReadOnlyEventList<IHelpDeskQuery>(list1)));

				SimpleQuery q2 = new SimpleQuery(typeof(SAHL.Common.BusinessModel.DAO.HelpDeskQuery_DAO), HQL);
				HelpDeskQuery_DAO[] res2 = HelpDeskQuery_DAO.ExecuteQuery(q2) as HelpDeskQuery_DAO[];

				if (res2 == null)
					return null;

				if (res2.GetLength(0) == 0)
					return null;

				IEventList<IHelpDeskQuery> list2 = new DAOEventList<HelpDeskQuery_DAO, IHelpDeskQuery, HelpDeskQuery>(res2);
				return new ReadOnlyEventList<IHelpDeskQuery>(list2);
			}
			else
				return null;
		}

		/// <summary>
		/// reurns a comma-delimited list of HelpDeskQuery Keys  
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		public string GetHelpDeskQueryKeysAsStringForQuery(IReadOnlyEventList<IHelpDeskQuery> list)
		{
			string keys = "";

			foreach (IHelpDeskQuery hdq in list)
			{
				keys += "," + hdq.Key.ToString();
			}

			if (keys.StartsWith(","))
				keys = keys.Remove(0, 1);

			return keys;
		}

		public void UpdateX2HelpDeskData(Int64 instanceID, IDictionary<string, object> hdDict)
		{
			IX2Service x2Service = ServiceFactory.GetService<IX2Service>();
			x2Service.SetX2DataRow(instanceID, hdDict);
		}

		public IHelpDeskQuery CreateEmptyHelpDeskQuery()
		{
			return base.CreateEmpty<IHelpDeskQuery, HelpDeskQuery_DAO>();
		}

		public int SaveHelpDeskQuery(IHelpDeskQuery helpDeskquery)
		{
			IDAOObject iDao = helpDeskquery as IDAOObject;
			HelpDeskQuery_DAO DAO = (HelpDeskQuery_DAO)iDao.GetDAOObject();

			DAO.SaveAndFlush();
			return helpDeskquery.Key;
		}

		public bool X2AutoArchive2AM_Update(int helpDeskQueryKey)
		{
			//get HD data
			IReadOnlyEventList<IHelpDeskQuery> helpDeskQuery = GetHelpDeskQueryByHelpDeskQueryKey(helpDeskQueryKey);
			if (helpDeskQuery != null)
			{
				ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
				helpDeskQuery[0].Memo.GeneralStatus = lookupRepo.GeneralStatuses[GeneralStatuses.Inactive];
				helpDeskQuery[0].ResolvedDate = DateTime.Now;
				IMemoRepository memoRepo = RepositoryFactory.GetRepository<IMemoRepository>();
				memoRepo.SaveMemo(helpDeskQuery[0].Memo);
				int result = SaveHelpDeskQuery(helpDeskQuery[0]);
				if (result > 0)
					//update success
					return true;
				else
					// update failed
					return false;
			}
			else
				//no HD data
				return false;
		}
    }
}
