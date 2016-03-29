using SAHomeloans.SAHL_VS_WebJSTemplate;
using System;
using System.ComponentModel;

namespace SAHomeloans.SAHL_VS_WebJS.Core.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    internal sealed class ResourcesDescriptionAttribute : DescriptionAttribute
    {
        #region Fields

        private bool replaced;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="description">Attribute description.</param>
        public ResourcesDescriptionAttribute(string description)
            : base(description)
        {
        }

        #endregion Constructors

        #region Overriden Implementation

        /// <summary>
        /// Gets attribute description.
        /// </summary>
        public override string Description
        {
            get
            {
                if (!replaced)
                {
                    replaced = true;
                    DescriptionValue = Resource.GetString(base.Description);
                }

                return base.Description;
            }
        }

        #endregion Overriden Implementation
    }
}