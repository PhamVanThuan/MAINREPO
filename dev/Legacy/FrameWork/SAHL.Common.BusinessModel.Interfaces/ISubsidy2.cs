namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface ISubsidy
    {
        /// <summary>
        /// Gets/sets the account subsidy attached to the subsidy.
        /// </summary>
        IAccount Account
        {
            get;
            set;
        }

        /// <summary>
        /// Gets/sets the application subsidy attached to the subsidy.
        /// </summary>
        IApplication Application
        {
            get;
            set;
        }
    }
}