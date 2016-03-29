using SAHL.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.FrontEndTest.Managers.Statements
{
    public class RemoveApplicationRoleStatement : ISqlStatement<int>
    {
        public int OfferRoleKey { get; protected set; }

        public RemoveApplicationRoleStatement(int OfferRoleKey)
        {
            this.OfferRoleKey = OfferRoleKey;
        }
        public string GetStatement()
        {
            return @"delete from [2am].dbo.OfferRole where OfferRoleKey = @OfferRoleKey";
        }
    }
}
