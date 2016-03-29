using System.Configuration;

namespace SAHL.Common.Configuration.Security
{
    public enum SecurityElementRestrictions
    {
        Include,
        Exclude
    }

    public class SecurityElement : DefaultElement
    {
        #region Private Static Variables

        private readonly ConfigurationProperty _restriction;

        #endregion Private Static Variables

        #region Constructors

        public SecurityElement()
        {
            this._restriction = new ConfigurationProperty("restriction", typeof(SecurityElementRestrictions), null, ConfigurationPropertyOptions.None);
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Whether the element members should be included or excluded by default.  This defaults to Include.
        /// </summary>
        [ConfigurationProperty("restriction", IsRequired = false, DefaultValue = SecurityElementRestrictions.Include)]
        public virtual SecurityElementRestrictions Restriction
        {
            get { return (SecurityElementRestrictions)base[this._restriction]; }
            set { base[this._restriction] = value; }
        }

        #endregion Public Properties
    }
}