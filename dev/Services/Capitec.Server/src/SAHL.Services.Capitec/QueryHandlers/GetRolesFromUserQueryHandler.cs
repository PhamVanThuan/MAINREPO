using Capitec.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Capitec.QueryHandlers
{
    public class GetRolesFromUserQueryHandler : IServiceQueryHandler<GetRolesFromUserQuery>
    {
        private IUserDataManager userDataManager;

        public GetRolesFromUserQueryHandler(IUserDataManager userDataManager)
        {
            this.userDataManager = userDataManager;
        }

        public ISystemMessageCollection HandleQuery(GetRolesFromUserQuery query)
        {
            var messages = SystemMessageCollection.Empty();
            var roles = userDataManager.GetRolesFromUser(query.UserId);
            var convertedRoles = roles.Select(
                p => new GetRolesFromUserQueryResult
                {
                    Id = p.Id,
                    Name = p.Name
                }
            );
            query.Result = new ServiceQueryResult<GetRolesFromUserQueryResult>(convertedRoles);
            return messages;
        }
    }
}
