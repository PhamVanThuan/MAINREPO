namespace SAHL.Common.BusinessModel.Interfaces.ReadOnly
{
    /// <summary>
    /// Lightweight immutable class intended for caching read-only <see cref="ICountry"/> values.
    /// </summary>
    public interface ICountryReadOnly
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
        /// Whether or not the country allows freetext format addresses.
        /// </summary>
        bool AllowFreeTextFormat { get; }
    }
}