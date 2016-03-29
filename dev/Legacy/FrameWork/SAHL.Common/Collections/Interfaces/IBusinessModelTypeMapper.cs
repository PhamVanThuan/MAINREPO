using System;
using System.Collections;
using System.Collections.Generic;

namespace SAHL.Common.Collections.Interfaces
{
    public interface IBusinessModelTypeMapper
    {
        TInterface GetMappedType<TInterface>(object DAOObject);

        TInterface GetMappedType<TInterface, TDAOObject>(TDAOObject DAOObject);

        Type GetDAOFromInterface<TInterface>();

        Type GetBusinessObjectTypeFromDAO(Type DAOType);

        /// <summary>
        /// Gets a list of DAO objects and converts them into a list of SAHL Domain interfaces.
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="daoObjects"></param>
        /// <returns></returns>
        IList<TInterface> GetMappedTypeList<TInterface>(IEnumerable daoObjects);
    }
}