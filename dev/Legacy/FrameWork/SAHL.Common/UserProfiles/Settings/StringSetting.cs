namespace SAHL.Common.UserProfiles.Settings
{
    public class StringSetting : UserProfileSettingBase, IUserProfileSetting
    {
        private string _value;

        public StringSetting(string name)
            : base(name)
        {
        }

        public StringSetting(string name, string value)
            : base(name)
        {
            _value = value;
        }

        public override void Load(string Data)
        {
            _value = Data;
        }

        public override string Persist()
        {
            return _value;
        }

        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
    }
}