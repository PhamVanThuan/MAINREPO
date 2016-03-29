using System;
using System.Collections.Generic;
using SAHL.Core.Data;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Halo.Server.QueryHandlers.Statements;
using SAHL.Services.Interfaces.Halo.Models;
using SAHL.Services.Interfaces.Halo.Queries;

namespace SAHL.Services.Halo.Server.QueryHandlers
{
    public class GetLoanTransactionDataByAccountKeyQueryHandler : QueryHandlerBase, IServiceQueryHandler<GetLoanTransactionDataByAccountKeyQuery>
    {
        private IDbFactory dbFactory;

        public GetLoanTransactionDataByAccountKeyQueryHandler(IDbFactory dbFactory, IRawLogger rawLogger, ILoggerSource loggerSource, ILoggerAppSource loggerAppSource)
            : base(rawLogger, loggerSource, loggerAppSource)
        {
            this.dbFactory = dbFactory;
        }

        public ISystemMessageCollection HandleQuery(GetLoanTransactionDataByAccountKeyQuery query)
        {
            var messages = SystemMessageCollection.Empty();
            try
            {
                IEnumerable<LoanTransactionDetailModel> results = this.GetLoanTransactionData(query.AccountKey);
                query.Result = new ServiceQueryResult<LoanTransactionDetailModel>(results);
            }
            catch (Exception runtimeException)
            {
                messages.AddMessage(new SystemMessage(string.Format("Failed to retrieve loan transaction data.\n{0}", runtimeException),
                                                      SystemMessageSeverityEnum.Exception));
            }
            return messages;
        }

        public IEnumerable<LoanTransactionDetailModel> GetLoanTransactionData(int accountKey)
        {
            var sql = new GetLoanTransactionDataForAccountKeyStatement(accountKey);
            using (var dbContext = dbFactory.NewDb().InReadOnlyAppContext())
            {
                return dbContext.Select(sql);
            }
        }
    }
}