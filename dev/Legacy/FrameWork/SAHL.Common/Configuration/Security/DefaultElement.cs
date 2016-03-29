using System.Configuration;

namespace SAHL.Common.Configuration.Security
{
    public class DefaultElement : ConfigurationElement
    {
        #region Private Static Variables

        private readonly ConfigurationProperty _value;

        #endregion Private Static Variables

        #region Constructors

        public DefaultElement()
        {
            this._value = new ConfigurationProperty("value", typeof(string), null, ConfigurationPropertyOptions.IsKey);
        }

        #endregion Constructors

        #region Public Properties

        [ConfigurationProperty("value", IsRequired = true)]
        public virtual string Value
        {
            get { return (string)base[this._value]; }
            set { base[this._value] = value; }
        }

        #endregion Public Properties
    }
}