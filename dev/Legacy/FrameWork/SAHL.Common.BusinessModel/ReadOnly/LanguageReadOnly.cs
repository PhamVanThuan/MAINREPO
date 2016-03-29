using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.ReadOnly;

namespace SAHL.Common.BusinessModel.ReadOnly
{
    /// <summary>
    /// Lightweight immutable class intended for caching read-only <see cref="SAHL.Common.BusinessModel.Interfaces.ILanguage">ILanguage</see> values.
    /// </summary>
    public class LanguageReadOnly : ILanguageReadOnly
    {

        private int _key;
        private string _description;
        private bool _translatable;

        public LanguageReadOnly(int key, string description, bool translatable)
        {
            _key = key;
            _description = description;
            _translatable = translatable;
        }


        /// <summary>
        /// The primary key.
        /// </summary>
        public int Key
        {
            get { return _key; }
        }


        /// <summary>
        /// The language description.
        /// </summary>
        public string Description
        {
            get { return _description; }
        }

        /// <summary>
        /// Whether or not the language is translatable.
        /// </summary>
        public bool Translatable
        {
            get { return _translatable; }
        }


    }
}
