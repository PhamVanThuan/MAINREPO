namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public partial interface ILegalEntityCleanUpCompositeKey
    {
        /// <summary>
        ///
        /// </summary>
        System.Int32 LegalEntityKey
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.Int32 LegalEntityExceptionReasonKey
        {
            get;
            set;
        }
    }
}