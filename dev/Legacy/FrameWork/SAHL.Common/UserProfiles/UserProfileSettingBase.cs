using System;

namespace SAHL.Common.UserProfiles
{
    public abstract class UserProfileSettingBase : IUserProfileSetting
    {
        private string _name;

        public UserProfileSettingBase(string name)
        {
            _name = name;
        }

        public string Name
        {
            get { return _name; }
        }

        public Type SettingType
        {
            get { return this.GetType(); }
        }

        public abstract void Load(string Data);

        public abstract string Persist();
    }
}