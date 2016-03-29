namespace SAHL.Common.Interfaces
{
    public interface IBusinessModelContainer
    {
        IBusinessModelObject BusinessModelObject
        {
            get;
            set;
        }
    }

    public interface IBusinessModelObject
    {
        /// <summary>
        /// Creates a shallow copy of the business model object.
        /// </summary>
        /// <returns></returns>
        object Clone();

        /// <summary>
        /// Refreshes the instance from the database.  Use with caution as this is only applicable
        /// to the DAO!  This should be overridden if necessary.
        /// </summary>
        void Refresh();
    }
}