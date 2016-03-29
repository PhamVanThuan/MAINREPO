using System.Configuration;

namespace SAHL.Common.Configuration.Security
{
    public sealed class TagElement : ConfigurationElement
    {
        #region Private Static Variables

        private readonly ConfigurationProperty _name;
        private readonly ConfigurationProperty _description;
        private readonly ConfigurationProperty _views;
        private readonly ConfigurationProperty _presenters;
        private readonly ConfigurationProperty _adGroups;
        private readonly ConfigurationProperty _adUsers;
        private readonly ConfigurationProperty _features;

        #endregion Private Static Variables

        #region Constructors

        public TagElement()
            : base()
        {
            this._name = new ConfigurationProperty("name", typeof(string), null, ConfigurationPropertyOptions.IsKey);
            this._description = new ConfigurationProperty("description", typeof(string), null, ConfigurationPropertyOptions.IsRequired);
            this._views = new ConfigurationProperty("views", typeof(ViewElementCollection), null, ConfigurationPropertyOptions.None);
            this._presenters = new ConfigurationProperty("presenters", typeof(PresenterElementCollection), null, ConfigurationPropertyOptions.None);
            this._adGroups = new ConfigurationProperty("adgroups", typeof(ADGroupElementCollection), null, ConfigurationPropertyOptions.None);
            this._adUsers = new ConfigurationProperty("adusers", typeof(ADUserElementCollection), null, ConfigurationPropertyOptions.None);
            this._features = new ConfigurationProperty("features", typeof(FeatureElementCollection), null, ConfigurationPropertyOptions.None);
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// The name of the security tag.
        /// </summary>
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)base[this._name]; }
            set { base[this._name] = value; }
        }

        /// <summary>
        /// The description of the security tag - describes what the security tag is used for.
        /// </summary>
        [ConfigurationProperty("description", IsRequired = true)]
        public string Description
        {
            get { return (string)base[this._description]; }
            set { base[this._description] = value; }
        }

        /// <summary>
        /// Represents the views linked to this security tag.
        /// </summary>
        [ConfigurationProperty("views")]
        public ViewElementCollection Views
        {
            get { return (ViewElementCollection)base[this._views]; }
            set { base[this._views] = value; }
        }

        /// <summary>
        /// Represents the presenters linked to this security tag.
        /// </summary>
        [ConfigurationProperty("presenters")]
        public PresenterElementCollection Presenters
        {
            get { return (PresenterElementCollection)base[this._presenters]; }
            set { base[this._presenters] = value; }
        }

        /// <summary>
        /// Represents the AD groups linked to this security tag.
        /// </summary>
        [ConfigurationProperty("adgroups")]
        public ADGroupElementCollection ADGroups
        {
            get { return (ADGroupElementCollection)base[this._adGroups]; }
            set { base[this._adGroups] = value; }
        }

        /// <summary>
        /// Represents the AD users linked to this security tag.
        /// </summary>
        [ConfigurationProperty("adusers")]
        public ADUserElementCollection ADUsers
        {
            get { return (ADUserElementCollection)base[this._adUsers]; }
            set { base[this._adUsers] = value; }
        }

        /// <summary>
        /// Represents the HALO features linked to this security tag.
        /// </summary>
        [ConfigurationProperty("features")]
        public FeatureElementCollection Features
        {
            get { return (FeatureElementCollection)base[this._features]; }
            set { base[this._features] = value; }
        }

        #endregion Public Properties
    }
}