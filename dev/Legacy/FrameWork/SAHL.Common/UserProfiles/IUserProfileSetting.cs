using System;

namespace SAHL.Common.UserProfiles
{
    public interface IUserProfileSetting
    {
        string Name { get; }

        void Load(string Data);

        string Persist();

        Type SettingType { get; }
    }
}