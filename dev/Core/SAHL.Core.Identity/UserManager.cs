using SAHL.Core.Identity.Model;
using System;
using System.Collections.Generic;

namespace SAHL.Core.Identity
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository userRepository;

        public UserManager(IUserRepository userRepository)
        {
            if (userRepository == null)
            {
                throw new ArgumentNullException("userRepository");
            }
            this.userRepository = userRepository;
        }

        public IUserDetails GetUserDetails(string adUserName)
        {
            if (string.IsNullOrWhiteSpace(adUserName))
            {
                throw new ArgumentNullException("adUserName");
            }

            IUserDetails userDetails = new UserDetails
            {
                FullADUsername = adUserName,
            };

            userDetails = userRepository.ADFindUser(userDetails);
            if (userDetails == null)
            {
                return null;
            }

            userDetails = userRepository.FindUserRoles(userDetails);
            return userDetails;
        }


        public IEnumerable<string> GetUserCapabilitiesForOrganisationStructureKey(int userOrganisationStructureKey)
        {
            return userRepository.GetUserCapabilitiesForOrganisationStructure(userOrganisationStructureKey);
        }

        public IEnumerable<OrganisationStructureCapability> GetUserCapabilities(string username)
        {
            return userRepository.GetUserCapabilities(username);
        }
    }
}