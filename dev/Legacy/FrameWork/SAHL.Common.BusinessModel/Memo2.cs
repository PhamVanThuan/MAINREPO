using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Utils;
using SAHL.Common.Globals;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.Base;
using NHibernate.Transform;

namespace SAHL.Common.BusinessModel
{
    public partial class Memo : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Memo_DAO>, IMemo,IComparable
	{
        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
            Rules.Add("MemoAddUpdateMemoReminderDate");
            Rules.Add("MemoAddUpdateMemoExpiryDate");
            Rules.Add("MemoAddUpdateReminderDate");
            Rules.Add("MemoAddUpdateMemoReminderDateMandatory");
            Rules.Add("MemoAddUpdateMemoExpiryDateMandatory");
            Rules.Add("MemoAddUpdateDescription");
        }

        public static IEventList<IMemo> GetByGenericKey(int GenericKey, int GenericKeyTypeKey)
        {
            string HQL = string.Format("from Memo_DAO m where m.GenericKey = {0} and m.GenericKeyType.Key = {1} order by m.InsertedDate desc", GenericKey, GenericKeyTypeKey);
            SimpleQuery query = new SimpleQuery(typeof(Memo_DAO), HQL);
            query.SetResultTransformer(new DistinctRootEntityResultTransformer());

            Memo_DAO[] result = Memo_DAO.ExecuteQuery(query) as Memo_DAO[];

            if (result == null)
                result = new Memo_DAO[0];

            List<Memo_DAO> list = new List<Memo_DAO>(result);          
            return new DAOEventList<Memo_DAO, IMemo, Memo>(list);
        }

        public static IEventList<IMemo> GetByGenericKey(int GenericKey, int GenericKeyTypeKey, int GeneralStatusKey)
        {
            string HQL = string.Format("from Memo_DAO m where m.GenericKey = {0} and m.GenericKeyType.Key = {1} and m.GeneralStatus.Key = {2} order by m.InsertedDate desc", GenericKey, GenericKeyTypeKey, GeneralStatusKey);
            SimpleQuery query = new SimpleQuery(typeof(Memo_DAO), HQL);
            query.SetResultTransformer(new DistinctRootEntityResultTransformer());

            Memo_DAO[] result = Memo_DAO.ExecuteQuery(query) as Memo_DAO[];

            if (result == null)
                result = new Memo_DAO[0];

            List<Memo_DAO> list = new List<Memo_DAO>(result);          
            return new DAOEventList<Memo_DAO, IMemo, Memo>(list);
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            Memo g = (Memo)obj;
            return DateTime.Compare(g.InsertedDate, this.InsertedDate);
        }

        #endregion
    }
}


