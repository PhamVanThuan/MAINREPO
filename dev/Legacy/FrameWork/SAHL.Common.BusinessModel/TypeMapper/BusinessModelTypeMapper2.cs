using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using NHibernate;
using NHibernate.Proxy;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.TypeMapper
{
    public partial class BusinessModelTypeMapper
    {
        static IDictionary<Type, Type> _abstractMappedInterfaces = new Dictionary<Type, Type>();

        /// <summary>
        /// This method is called at the end of the default constructor in the generated constructor to enable us to add custom mappings for abstract classes or interfaces.
        /// </summary>
        private void ExtendedConstructor()
        {
            // Add manual X2 mapped types.
            _mappedTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.Process_DAO), typeof(SAHL.Common.X2.BusinessModel.Process));
            _mappedTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.ActivityType_DAO), typeof(SAHL.Common.X2.BusinessModel.ActivityType));
            _mappedTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.SecurityGroup_DAO), typeof(SAHL.Common.X2.BusinessModel.SecurityGroup));
            _mappedTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.WorkFlowActivity_DAO), typeof(SAHL.Common.X2.BusinessModel.WorkFlowActivity));
            _mappedTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.WorkFlow_DAO), typeof(SAHL.Common.X2.BusinessModel.WorkFlow));
            _mappedTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.StageActivity_DAO), typeof(SAHL.Common.X2.BusinessModel.StageActivity));
            _mappedTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.Form_DAO), typeof(SAHL.Common.X2.BusinessModel.Form));
            _mappedTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.Log_DAO), typeof(SAHL.Common.X2.BusinessModel.Log));
            _mappedTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.Instance_DAO), typeof(SAHL.Common.X2.BusinessModel.Instance));
            _mappedTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.WorkList_DAO), typeof(SAHL.Common.X2.BusinessModel.WorkList));
            _mappedTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.WorkFlowHistory_DAO), typeof(SAHL.Common.X2.BusinessModel.WorkFlowHistory));
            _mappedTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.State_DAO), typeof(SAHL.Common.X2.BusinessModel.State));
            _mappedTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.InstanceActivitySecurity_DAO), typeof(SAHL.Common.X2.BusinessModel.InstanceActivitySecurity));
            _mappedTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.ExternalActivityTarget_DAO), typeof(SAHL.Common.X2.BusinessModel.ExternalActivityTarget));
            _mappedTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.ExternalActivity_DAO), typeof(SAHL.Common.X2.BusinessModel.ExternalActivity));
            _mappedTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.Activity_DAO), typeof(SAHL.Common.X2.BusinessModel.Activity));
            _mappedTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.ActiveExternalActivity_DAO), typeof(SAHL.Common.X2.BusinessModel.ActiveExternalActivity));
            _mappedTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.StateType_DAO), typeof(SAHL.Common.X2.BusinessModel.StateType));
            _mappedTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.WorkFlowIcon_DAO), typeof(SAHL.Common.X2.BusinessModel.WorkFlowIcon));

            // Add Abstract mapped interfaces.

            _abstractMappedInterfaces.Add(typeof(SAHL.Common.BusinessModel.Interfaces.IMortgageLoanApplicationInformation), typeof(SAHL.Common.BusinessModel.MortgageLoanApplicationInformation));
            _abstractMappedInterfaces.Add(typeof(SAHL.Common.BusinessModel.Interfaces.IReAdvanceApplicationInformation), typeof(SAHL.Common.BusinessModel.ReAdvanceApplicationInformation));
            _abstractMappedInterfaces.Add(typeof(SAHL.Common.BusinessModel.Interfaces.IFurtherAdvanceApplicationInformation), typeof(SAHL.Common.BusinessModel.FurtherAdvanceApplicationInformation));
            _abstractMappedInterfaces.Add(typeof(SAHL.Common.BusinessModel.Interfaces.IFurtherLoanApplicationInformation), typeof(SAHL.Common.BusinessModel.FurtherLoanApplicationInformation));
            _abstractMappedInterfaces.Add(typeof(SAHL.Common.BusinessModel.Interfaces.IOrganisationStructure), typeof(SAHL.Common.BusinessModel.OrganisationStructure));
        }

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
                if (_abstractMappedInterfaces.ContainsKey(InterfaceType))
                {
                    // we have an abstact interface, map it as such.
                    NonDAOType = _abstractMappedInterfaces[InterfaceType];
                    TInterface ATI = (TInterface)Activator.CreateInstance(NonDAOType, new object[] { DAOObject });
                    return ATI;
                }
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
                TInterface TI = default(TInterface);

                object cobj = ((IProxyableDAOObject)DAOObject).ActualDAOObject;
                TI = (TInterface)Activator.CreateInstance(NonDAOType, new object[] { cobj });
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
                // first check if Interface is listed in abstract interfaces.
                if (_abstractMappedInterfaces.ContainsKey(InterfaceType))
                {
                    // we have an abstact interface, map it as such.
                    NonDAOType = _abstractMappedInterfaces[InterfaceType];
                    TInterface ATI = (TInterface)Activator.CreateInstance(NonDAOType, new object[] { DAOObject });
                    return ATI;
                }
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
                TInterface TI = default(TInterface);

                object cobj = ((IProxyableDAOObject)DAOObject).ActualDAOObject;
                TI = (TInterface)Activator.CreateInstance(NonDAOType, new object[] { cobj });

                return TI;
            }
            return default(TInterface);
        }

        public Type GetDAOFromInterface<TInterface>()
        {
            Type DAOType = null;
            Type InterfaceType = typeof(TInterface);
            // first check if Interface is listed in abstract interfaces.
            if (_abstractMappedInterfaces.ContainsKey(InterfaceType))
            {
                // we have an abstact interface, map it as such.
                DAOType = _abstractMappedInterfaces[InterfaceType];
            }
            else
                if (_mappedInterfaceTypes.ContainsKey(InterfaceType))
                {
                    DAOType = _mappedInterfaceTypes[InterfaceType];
                }
            return DAOType;
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

    public class TypeMapperBinder : Binder
    {
        public override FieldInfo BindToField(BindingFlags bindingAttr, FieldInfo[] match, object value, System.Globalization.CultureInfo culture)
        {
            throw new ArgumentException("match");
        }

        public override MethodBase BindToMethod(BindingFlags bindingAttr, MethodBase[] match, ref object[] args, ParameterModifier[] modifiers, System.Globalization.CultureInfo culture, string[] names, out object state)
        {
            if (match == null)
                throw new ArgumentException("match");

            object ostate = new object();
            state = ostate;
            for (int i = 0; i < match.Length; i++)
            {
                ParameterInfo[] parameters = match[i].GetParameters();
                if (parameters.Length == 1)
                {
                    return match[0];
                }
            }
            return null;
        }

        public override object ChangeType(object value, Type type, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        public override void ReorderArgumentArray(ref object[] args, object state)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override MethodBase SelectMethod(BindingFlags bindingAttr, MethodBase[] match, Type[] types, ParameterModifier[] modifiers)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override PropertyInfo SelectProperty(BindingFlags bindingAttr, PropertyInfo[] match, Type returnType, Type[] indexes, ParameterModifier[] modifiers)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}