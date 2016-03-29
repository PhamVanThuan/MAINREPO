using System.Collections.Generic;

namespace SAHL.Common.UserProfiles
{
    /// <summary>
    /// Holds a list of settings for a particular principal. The class
    /// </summary>
    public class UserProfile
    {
        IDictionary<string, IUserProfileSetting> _settings = null;

        public UserProfile()
        {
        }

        public IDictionary<string, IUserProfileSetting> Settings
        {
            get
            {
                return _settings;
            }
        }
    }
}