using System;
using System.Collections.Generic;
using System.Text;

namespace SAHL.TestWTF.DAOHelpers
{
    public abstract class BaseHelper<T> : TestBase, IDisposable
    {

        private List<T> _createdEntities = new List<T>();

        #region Constructors and Destructor

        /// <summary>
        /// Destructor - ensures the Dispose method is called.
        /// </summary>
        ~BaseHelper()
        {
            try
            {
                Dispose();
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Contains a collection of objects.  This should be used to store all entities created by the 
        /// helper class, and used during the Dispose method.
        /// </summary>
        protected List<T> CreatedEntities
        {
            get
            {
                return _createdEntities;
            }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Dispose method, where any created objects should be destroyed.  Database records created must 
        /// be removed from the database.
        /// </summary>
        public abstract void Dispose();

        #endregion
    }
}
