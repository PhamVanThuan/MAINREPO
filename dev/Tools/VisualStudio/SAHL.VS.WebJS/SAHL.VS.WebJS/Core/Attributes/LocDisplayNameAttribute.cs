using SAHomeloans.SAHL_VS_WebJSTemplate;
using System;
using System.ComponentModel;

namespace SAHomeloans.SAHL_VS_WebJS.Core.Attributes
{
    /// <summary>
    /// Specifies the display name for a property, event,
    /// or public void method which takes no arguments.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    internal sealed class LocalDisplayNameAttribute : DisplayNameAttribute
    {
        #region Fields

        private string name;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="name">Attribute display name.</param>
        public LocalDisplayNameAttribute(string name)
        {
            this.name = name;
        }

        #endregion Constructors

        #region Overriden Implementation

        /// <summary>
        /// Gets attribute display name.
        /// </summary>
        public override string DisplayName
        {
            get
            {
                string result = Resource.GetString(this.name);

                if (result == null)
                {
                    result = this.name;
                }

                return result;
            }
        }

        #endregion Overriden Implementation
    }
}