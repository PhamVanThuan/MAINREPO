using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.UserProfile.Models;
using SAHL.Services.Interfaces.UserProfile.Models.Shared;
using SAHL.Services.Interfaces.UserProfile.Queries;
using System;
using System.Linq;

namespace SAHL.Services.UserProfile.QueryHandlers
{
    public class GetUserDetailsForUserQueryHandler : IServiceQueryHandler<GetUserDetailsForUserQuery>
    {
        private IUserManager userManager;

        public GetUserDetailsForUserQueryHandler(Core.Identity.IUserManager userManager)
        {
            this.userManager = userManager;
        }

        public ISystemMessageCollection HandleQuery(GetUserDetailsForUserQuery query)
        {
            if (string.IsNullOrEmpty(query.ADUsername))
            {
                throw new ArgumentNullException("GetUserDetailsForUserQuery.ADUsername cannot be null");
            }
            
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            IUserDetails userDetails = userManager.GetUserDetails(query.ADUsername);
            
            if (userDetails == null)
            {
                throw new Exception("ADUsername does not exist");
            }
            
            var userRoles = userDetails.UserRoles.Select(x => new UserAreaRole(x.OrganisationArea, x.RoleName, x.UserOrganisationStructureKey)).ToList();
            var result = new GetUserDetailsForUserQueryResult(userDetails.UserName, userDetails.Domain, userDetails.DisplayName, userDetails.EmailAddress,userRoles);
            query.Result = new ServiceQueryResult<GetUserDetailsForUserQueryResult>(new GetUserDetailsForUserQueryResult[]{result});
            
            return messages;
        }
    }
}