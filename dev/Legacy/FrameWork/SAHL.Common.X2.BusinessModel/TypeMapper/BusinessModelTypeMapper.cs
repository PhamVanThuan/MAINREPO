using System;
using System.Collections.Generic;
using SAHL.Common.Attributes;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.X2.BusinessModel.TypeMapper
{
    [FactoryType(typeof(IBusinessModelTypeMapper), LifeTime = FactoryTypeLifeTime.Singleton)]
    public partial class BusinessModelTypeMapper : IBusinessModelTypeMapper
    {
        static IDictionary<Type, Type> _mappedTypes = new Dictionary<Type, Type>();
        static IDictionary<Type, Type> _mappedInterfaceTypes = new Dictionary<Type, Type>();

        public BusinessModelTypeMapper()
        {
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
        }

        public Type GetDAOFromInterface<TInterface>()
        {
            Type DAOType = null;
            Type InterfaceType = typeof(TInterface);
            if (_mappedInterfaceTypes.ContainsKey(InterfaceType))
            {
                DAOType = _mappedInterfaceTypes[InterfaceType];
            }

            return DAOType;
        }
    }
}