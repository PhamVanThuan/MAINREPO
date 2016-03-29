using System.Configuration;

namespace SAHL.Common.Configuration.Security
{
    /// <summary>
    /// Used to add a security section to an SAHL config file.  This section is made up of a number
    /// of "tags", which are security definitions.  These are made up of a number of "views" and
    /// "presenters" to which the security applies, and also a number of ad user groups, features,
    /// and other security checking identifiers.
    /// </summary>
    public sealed class SecuritySection : ConfigurationSection
    {
        private static readonly ConfigurationProperty _tags;

        static SecuritySection()
        {
            SecuritySection._tags = new ConfigurationProperty("tags", typeof(TagElementCollection), null, ConfigurationPropertyOptions.IsRequired);
        }

        /// <summary>
        /// This property represents the <code>tags</code> subsection.  This will contain a list
        /// of security tags that represent security features.
        /// </summary>
        [ConfigurationProperty("tags")]
        public TagElementCollection Tags
        {
            get { return (TagElementCollection)base[SecuritySection._tags]; }
            set { base[SecuritySection._tags] = value; }
        }
    }
}