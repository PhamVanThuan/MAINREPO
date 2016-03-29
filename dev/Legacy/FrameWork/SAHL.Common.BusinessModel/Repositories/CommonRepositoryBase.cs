using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Repositories
{
    public class CommonRepositoryBase : AbstractRepositoryBase
    {
        /// <summary>
        /// Gets an entity by its primary key. This is a useful helper function to use inside any repository to 
        /// get any business object by its primary key.
        /// </summary>
        /// <typeparam name="TInterface">The Interface to map to.</typeparam>
        /// <typeparam name="TDAO">The DAO object to map from.</typeparam>
        /// <param name="Key">The primary key of the business object to find.</param>
        /// <returns>Returns the business object for the primary key value.</returns>
        public TInterface GetByKey<TInterface, TDAO>(int Key) where TDAO : DB_Base<TDAO>
        {
            return base.GetByKey<TInterface, TDAO>(Key);
        }


        /// <summary>
        /// Creates and empty business object. This is a useful helper function to use inside any repository to 
        /// create an empty business object.
        /// </summary>
        /// <typeparam name="TInterface">The Interface to map to.</typeparam>
        /// <typeparam name="TDAO">The DAO object to map from.</typeparam>
        /// <returns>A new instance of the businessobject wrapped in an interface.</returns>
        public TInterface CreateEmpty<TInterface, TDAO>() where TDAO : DB_Base<TDAO>, new()
        {
            return base.CreateEmpty<TInterface, TDAO>();
        }

        /// <summary>
        /// Saves a business object. This is a useful helper function to use inside any repository to 
        /// save any business object.
        /// </summary>
        /// <typeparam name="TInterface">The Interface to map to.</typeparam>
        /// <typeparam name="TDAO">The DAO object to map from.</typeparam>
        /// <param name="ObjectToSave">The object to save to the database.</param>
        public void Save<TInterface, TDAO>(TInterface ObjectToSave)
            where TInterface : IDAOObject
            where TDAO : DB_Base<TDAO>
        {
            base.Save<TInterface, TDAO>(ObjectToSave);
        }
    }
}
