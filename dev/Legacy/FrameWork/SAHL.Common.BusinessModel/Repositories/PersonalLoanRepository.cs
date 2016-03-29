using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;
using System.Data;
using SAHL.Common.DataAccess;
using System.Reflection;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(IPersonalLoanRepository))]
    public class PersonalLoanRepository : AbstractRepositoryBase, IPersonalLoanRepository
    {
        private ICastleTransactionsService castleTransactionService;

        public PersonalLoanRepository()
        {
            if (castleTransactionService == null)
            {
                castleTransactionService = new CastleTransactionsService();
            }
        }

        public PersonalLoanRepository(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="term"></param>
        /// <param name="userid"></param>
        public void ChangeTerm(int accountKey, int term, string userid)
        {
            // Update the required fields on the Account
            SAHL.Common.DataAccess.ParameterCollection parameters = new SAHL.Common.DataAccess.ParameterCollection();
            parameters.Add(new SqlParameter("@AccountKey", accountKey));
            parameters.Add(new SqlParameter("@RemainingInstalments", term));
            parameters.Add(new SqlParameter("@UserID", userid));

            this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Repositories.PersonalLoanRepository", "ChangeTerm", parameters);

        }

        public void ChangeInstalment(int accountKey, string userId)
        {
            IMortgageLoanRepository mlRepo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
            mlRepo.InstallmentChange(accountKey, userId);
        }

        public List<BatchServiceResult> GetBatchServiceResults(BatchServiceTypes batchServiceType)
        {
            SAHL.Common.DataAccess.ParameterCollection parameters = new SAHL.Common.DataAccess.ParameterCollection();
            parameters.Add(new SqlParameter("@BatchServiceTypeKey", (int)batchServiceType));
            var query = UIStatementRepository.GetStatement("Repositories.PersonalLoanRepository", "GetBatchServiceResults");
            List<BatchServiceResult> results = Helper.Many<BatchServiceResult>(query,parameters);
            return results;
        }
    }
}
