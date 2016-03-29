using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class ACBBank : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ACBBank_DAO>, IACBBank
    {
        public IReadOnlyEventList<IACBBranch> GetACBBranchesByPrefix(string prefix, int maxRowCount)
        {
            if (this.Key < 1)
                return new ReadOnlyEventList<IACBBranch>();
            SimpleQuery q = null;
            q = new SimpleQuery(typeof(ACBBranch_DAO), @"
                from ACBBranch_DAO b
                where (b.ACBBank.Key = ? and b.ActiveIndicator = 0)
                and (b.Key LIKE ?  or  b.ACBBranchDescription LIKE ?)",
                this.Key,
                prefix + "%",
                prefix + "%"
            );
            q.SetQueryRange(maxRowCount);
            List<ACBBranch_DAO> lstDao = new List<ACBBranch_DAO>((ACBBranch_DAO[])ActiveRecordBase.ExecuteQuery(q));
            DAOEventList<ACBBranch_DAO, IACBBranch, ACBBranch> branches = new DAOEventList<ACBBranch_DAO, IACBBranch, ACBBranch>(lstDao);
            return new ReadOnlyEventList<IACBBranch>(branches);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnACBBranches_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnACBBranches_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnACBBranches_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnACBBranches_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }
    }
}