using SAHL.Core.Data;
using SAHL.Core.Messaging.Shared;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.DomainQuery.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.DomainQuery.Queries
{
    public class GetAccountByAccountNumberQuery : ServiceQuery<GetAccountByAccountNumberQueryResult>, 
                                            IDomainQueryQuery, ISqlServiceQuery<GetAccountByAccountNumberQueryResult>, 
                                            IServiceQuery<IServiceQueryResult<GetAccountByAccountNumberQueryResult>>, 
                                            IServiceQuery, IServiceCommand, IMessage
    {
        [Required]
        public int AccountNumber { get; protected set; }

        public GetAccountByAccountNumberQuery(int accountNumber)
        {
            this.AccountNumber = accountNumber;
        }
    }
}
