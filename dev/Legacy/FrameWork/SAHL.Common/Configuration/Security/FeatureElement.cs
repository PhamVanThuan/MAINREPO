using System;

namespace SAHL.Common.Configuration.Security
{
    /// <summary>
    /// Defines an element that is used to specify a HALO feature that is applicable to a security feature.
    /// </summary>
    public sealed class FeatureElement : SecurityElement
    {
        /// <summary>
        /// Override the value to ensure it's always an int.
        /// </summary>
        public new int? Value
        {
            get
            {
                int result;
                if (Int32.TryParse(base.Value, out result))
                    return new int?(result);
                else
                    return new int?();
            }
            set
            {
                if (value.HasValue)
                    base.Value = "";
                else
                    base.Value = value.Value.ToString();
            }
        }
    }
}