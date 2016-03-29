using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System.Data;
using SAHL.Common.DataAccess;
using System.Data.SqlClient;
using System.Collections.Generic;
using Castle.ActiveRecord.Queries;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Service;

namespace SAHL.Common.BusinessModel.Repositories
{
	[FactoryType(typeof(INoteRepository))]
	public class NoteRepository : AbstractRepositoryBase, INoteRepository
	{
		private ICastleTransactionsService castleTransactionsService;
		public NoteRepository()
			: this(new CastleTransactionsService())
		{
		}
		public NoteRepository(ICastleTransactionsService castleTransactionsService)
		{
			this.castleTransactionsService = castleTransactionsService;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public INote CreateEmptyNote()
		{
			return CreateEmpty<INote, Note_DAO>();

		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public INoteDetail CreateEmptyNoteDetail()
		{
			return CreateEmpty<INoteDetail, NoteDetail_DAO>();

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="note"></param>
		public void SaveNote(INote note)
		{
			Save<INote, Note_DAO>(note);

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="notedetail"></param>
		public void SaveNoteDetail(INoteDetail notedetail)
		{
			Save<INoteDetail, NoteDetail_DAO>(notedetail);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public INote GetNoteByKey(int key)
		{
			return GetByKey<INote, Note_DAO>(key);

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public INoteDetail GetNoteDetailByKey(int key)
		{
			return GetByKey<INoteDetail, NoteDetail_DAO>(key);

		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="GenericKey"></param>
		/// <param name="GenericKeyTypeKey"></param>
		/// <returns></returns>
		public List<INoteDetail> GetNoteDetailsByGenericKeyAndType(int GenericKey, int GenericKeyTypeKey)
		{
			List<INoteDetail> noteDetails = new List<INoteDetail>();
			string sql = UIStatementRepository.GetStatement("COMMON", "GetNoteDetails");
			SimpleQuery<NoteDetail_DAO> q = new SimpleQuery<NoteDetail_DAO>(QueryLanguage.Sql, sql, GenericKey, GenericKeyTypeKey);
			q.AddSqlReturnDefinition(typeof(NoteDetail_DAO), "n");
			NoteDetail_DAO[] res = q.Execute();

			if (res != null && res.Length > 0)
			{
				IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
				foreach (NoteDetail_DAO nd in res)
				{
					noteDetails.Add(BMTM.GetMappedType<INoteDetail, NoteDetail_DAO>(nd));
				}
			}
			return noteDetails;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="accountKey"></param>
		/// <returns></returns>
		public List<INoteDetail> GetAllDebtcounsellingNoteDetailsForAccount(int accountKey)
		{
			List<INoteDetail> noteDetails = new List<INoteDetail>();
			string sql = UIStatementRepository.GetStatement("COMMON", "GetDebtCounsellingNoteDetailsForAccount");
			SimpleQuery<NoteDetail_DAO> q = new SimpleQuery<NoteDetail_DAO>(QueryLanguage.Sql, sql, accountKey);
			q.AddSqlReturnDefinition(typeof(NoteDetail_DAO), "n");
			NoteDetail_DAO[] res = q.Execute();

			if (res != null && res.Length > 0)
			{
				IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
				foreach (NoteDetail_DAO nd in res)
				{
					noteDetails.Add(BMTM.GetMappedType<INoteDetail, NoteDetail_DAO>(nd));
				}
			}
			return noteDetails;
		}

		public INote GetNoteByGenericKeyAndType(int genericKey, int genericKeyTypeKey)
		{
			string hql = "select note from Note_DAO note where GenericKey = ? and GenericKeyType.Key = ?";
			return castleTransactionsService.Single<INote>(Globals.QueryLanguages.Hql, hql, Globals.Databases.TwoAM, genericKey, genericKeyTypeKey );
		}
	}
}
