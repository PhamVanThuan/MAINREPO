using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Proxy;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.X2.BusinessModel.TypeMapper
{
    public partial class BusinessModelTypeMapper : IBusinessModelTypeMapper
    {
        public Type GetBusinessObjectTypeFromDAO(Type DAOType)
        {
            Type IType = null;
            if (_mappedTypes.ContainsKey(DAOType))
            {
                IType = _mappedInterfaceTypes[DAOType];
            }
            return IType;
        }

        public TInterface GetMappedType<TInterface>(object DAOObject)
        {
            if (DAOObject != null)
            {
                // if the DAO object is an IBusinessModelContainer and the a business model class has already
                // been added, then just return the class - this prevents multiple instances from being
                // created needlessly
                IBusinessModelContainer bmcDao = DAOObject as IBusinessModelContainer;
                if (bmcDao != null && bmcDao.BusinessModelObject != null)
                    return (TInterface)bmcDao.BusinessModelObject;

                Type InterfaceType = typeof(TInterface);
                Type NonDAOType;
                // first check if Interface is listed in abstract interfaces.

                // now do normal DAO mapping.
                Type DAOType = null;
                // check if the DAOObject is proxied, if it is get the orignal type with the nhibernate utilities
                if (DAOObject is INHibernateProxy)
                {
                    DAOType = NHibernateUtil.GetClass(DAOObject);
                }
                else
                {
                    DAOType = DAOObject.GetType();
                }

                if (!_mappedTypes.ContainsKey(DAOType))
                {
                    // if its not there try look down the type hierarchy
                    if (_mappedTypes.ContainsKey(DAOType.BaseType))
                    {
                        NonDAOType = _mappedTypes[DAOType.BaseType];
                    }
                    else
                        return default(TInterface);
                }
                else
                    NonDAOType = _mappedTypes[DAOType];
                TInterface TI = (TInterface)Activator.CreateInstance(NonDAOType, new object[] { DAOObject });

                if (TI is IBusinessModelObject && bmcDao != null)
                {
                    bmcDao.BusinessModelObject = TI as IBusinessModelObject;
                }

                return TI;
            }
            return default(TInterface);
        }

        public TInterface GetMappedType<TInterface, TDAOObject>(TDAOObject DAOObject)
        {
            if (DAOObject != null)
            {
                // if the DAO object is an IBusinessModelContainer and the a business model class has already
                // been added, then just return the class - this prevents multiple instances from being
                // created needlessly
                IBusinessModelContainer bmcDao = DAOObject as IBusinessModelContainer;
                if (bmcDao != null && bmcDao.BusinessModelObject != null)
                    return (TInterface)bmcDao.BusinessModelObject;

                Type InterfaceType = typeof(TInterface);
                Type NonDAOType;
                // now do normal DAO mapping.
                Type DAOType = null;
                // check if the DAOObject is proxied, if it is get the orignal type with the nhibernate utilities
                if (DAOObject is INHibernateProxy)
                {
                    DAOType = NHibernateUtil.GetClass(DAOObject);
                }
                else
                {
                    DAOType = DAOObject.GetType();
                }

                if (!_mappedTypes.ContainsKey(DAOType))
                {
                    // if its not there try look down the type hierarchy
                    if (_mappedTypes.ContainsKey(DAOType.BaseType))
                    {
                        NonDAOType = _mappedTypes[DAOType.BaseType];
                    }
                    else
                        return default(TInterface);
                }
                else
                    NonDAOType = _mappedTypes[DAOType];
                TInterface TI = (TInterface)Activator.CreateInstance(NonDAOType, new object[] { DAOObject });

                if (TI is IBusinessModelObject && bmcDao != null)
                {
                    bmcDao.BusinessModelObject = TI as IBusinessModelObject;
                }

                return TI;
            }
            return default(TInterface);
        }

        /// <summary>
        /// Gets a list of DAO objects and converts them into a list of SAHL Domain interfaces.
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="daoObjects"></param>
        /// <returns></returns>
        public IList<TInterface> GetMappedTypeList<TInterface>(IEnumerable daoObjects)
        {
            List<TInterface> result = new List<TInterface>();
            foreach (object dao in daoObjects)
                result.Add(GetMappedType<TInterface>(dao));
            return result;
        }
    }
}