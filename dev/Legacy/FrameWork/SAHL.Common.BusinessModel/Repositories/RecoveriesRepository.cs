using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using Castle.ActiveRecord.Queries;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    [FactoryType(typeof(IRecoveriesRepository))]
    public class RecoveriesRepository : AbstractRepositoryBase, IRecoveriesRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IRecoveriesProposal CreateEmptyRecoveriesProposal()
        {
            return base.CreateEmpty<IRecoveriesProposal, RecoveriesProposal_DAO>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recoveriesProposal"></param>
        public void SaveRecoveriesProposal(IRecoveriesProposal recoveriesProposal)
        {
            base.Save<IRecoveriesProposal, RecoveriesProposal_DAO>(recoveriesProposal);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IRecoveriesProposal GetRecoveriesProposalByKey(int key)
        {
            return base.GetByKey<IRecoveriesProposal, RecoveriesProposal_DAO>(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="generalStatus"></param>
        /// <returns></returns>
        public List<IRecoveriesProposal> GetRecoveriesProposalsByAccountKey(int accountKey, Globals.GeneralStatuses generalStatus)
        {
            List<IRecoveriesProposal> recoveriesProposalList = GetRecoveriesProposalsByAccountKey(accountKey);

            List<IRecoveriesProposal> recoveriesProposalListActive = new List<IRecoveriesProposal>();
            foreach (var rp in recoveriesProposalList)
            {
                if (rp.GeneralStatus.Key == (int)generalStatus)
                {
                    recoveriesProposalListActive.Add(rp);
                }
            }

            return recoveriesProposalListActive;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public List<IRecoveriesProposal> GetRecoveriesProposalsByAccountKey(int accountKey)
        {
            List<IRecoveriesProposal> recoveriesProposalList = new List<IRecoveriesProposal>();
            IBusinessModelTypeMapper BMTM = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();

            string query = UIStatementRepository.GetStatement("Repositories.RecoveriesRepository", "GetRecoveriesProposalsByAccountKey");

            SimpleQuery<RecoveriesProposal_DAO> p = new SimpleQuery<RecoveriesProposal_DAO>(QueryLanguage.Sql, query, accountKey);
            p.AddSqlReturnDefinition(typeof(RecoveriesProposal_DAO), "rp");

            RecoveriesProposal_DAO[] res = p.Execute();

            if (res == null || res.Length == 0)
                return recoveriesProposalList;

            foreach (RecoveriesProposal_DAO rp in res)
            {
                recoveriesProposalList.Add(BMTM.GetMappedType<IRecoveriesProposal, RecoveriesProposal_DAO>(rp));
            }

            return recoveriesProposalList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public IRecoveriesProposal GetActiveRecoveriesProposalByAccountKey(int accountKey)
        {
            List<IRecoveriesProposal> recoveriesProposalListActive = GetRecoveriesProposalsByAccountKey(accountKey,GeneralStatuses.Active);

            if (recoveriesProposalListActive.Count > 0)
                return recoveriesProposalListActive[0];
            else
                return null;
        }
    }
}
