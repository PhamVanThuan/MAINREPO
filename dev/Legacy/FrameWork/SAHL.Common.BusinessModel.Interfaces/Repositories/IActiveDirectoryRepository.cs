using System.Collections.Generic;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IActiveDirectoryRepository
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="ADUserNamePartial"></param>
        /// <returns></returns>
        IList<ActiveDirectoryUserBindableObject> GetActiveDirectoryUsers(string ADUserNamePartial);

        /// <summary>
        ///
        /// </summary>
        /// <param name="ADUserName"></param>
        /// <returns></returns>
        ActiveDirectoryUserBindableObject GetActiveDirectoryUser(string ADUserName);
    }

    /// <summary>
    /// Simple class for binding active directory users to a grid.
    /// </summary>
    public class ActiveDirectoryUserBindableObject
    {
        private string _adUserName;
        private string _firstName;
        private string _surName;
        private string _emailAddress;
        private string _cellNumber;

        public ActiveDirectoryUserBindableObject()
        {
        }

        public string ADUserName
        {
            get { return _adUserName; }
            set { _adUserName = value; }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public string Surname
        {
            get { return _surName; }
            set { _surName = value; }
        }

        public string EmailAddress
        {
            get { return _emailAddress; }
            set { _emailAddress = value; }
        }

        public string CellNumber
        {
            get { return _cellNumber; }
            set { _cellNumber = value; }
        }
    }
}