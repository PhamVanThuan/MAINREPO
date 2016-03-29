using System.Collections.Generic;

namespace SAHL.Core.Identity
{
    public class UserDetails : IUserDetails
    {
        public string FullADUsername { get; internal set; }

        public string Domain { get; internal set; }

        public string UserName { get; internal set; }

        public string DisplayName { get; internal set; }

        public string EmailAddress { get; internal set; }

        public byte[] UserPhoto { get; internal set; }

        public IUserRole ActiveRole { get; internal set; }

        public IEnumerable<IUserRole> UserRoles { get; internal set; }
    }
}