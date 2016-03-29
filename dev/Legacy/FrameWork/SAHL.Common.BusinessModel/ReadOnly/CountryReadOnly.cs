using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.ReadOnly;
using System.Diagnostics.CodeAnalysis;

namespace SAHL.Common.BusinessModel.ReadOnly
{
    /// <summary>
    /// Lightweight immutable class intended for caching read-only <see cref="SAHL.Common.BusinessModel.Interfaces.ICountry">ICountry</see> values.
    /// </summary>
	/// 
	[SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", Justification = "This is such a small class its not deemed important")]
    public class CountryReadOnly : ICountryReadOnly
    {

        private int _key;
        private string _description;
        private bool _allowFreeTextFormat;

        public CountryReadOnly(int key, string description, bool allowFreeTextFormat)
        {
            _key = key;
            _description = description;
            _allowFreeTextFormat = allowFreeTextFormat;
        }


        /// <summary>
        /// The primary key.
        /// </summary>
        public int Key
        {
            get { return _key; }
        }


        /// <summary>
        /// The country description.
        /// </summary>
        public string Description
        {
            get { return _description; }
        }

        /// <summary>
        /// Whether or not the country allows freetext format addresses.
        /// </summary>
        public bool AllowFreeTextFormat
        {
            get { return _allowFreeTextFormat; }
        }


    }
}
