using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(IMemoReadOnlyRepository))]
    public class MemoReadOnlyRepository : AbstractRepositoryBase, IMemoReadOnlyRepository
    {
        public IMemo GetMemoByKey(int Key)
        {
            using (new TransactionScope(TransactionMode.New, System.Data.IsolationLevel.ReadUncommitted, OnDispose.Commit))
            {
                return base.GetByKey<IMemo, Memo_DAO>(Key);
            }
        }
    }
}