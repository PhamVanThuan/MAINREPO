namespace SAHL.Common.BusinessModel.Interfaces.ReadOnly
{
    /// <summary>
    /// Lightweight immutable class intended for caching read-only <see cref="ILanguage"/> values.
    /// </summary>
    public interface ILanguageReadOnly
    {
        /// <summary>
        /// The primary key.
        /// </summary>
        int Key { get; }

        /// <summary>
        /// The country description.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Whether or not the language is translatable.
        /// </summary>
        bool Translatable { get; }
    }
}