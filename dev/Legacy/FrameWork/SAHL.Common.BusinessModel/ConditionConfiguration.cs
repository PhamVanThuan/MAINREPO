using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ConditionConfiguration_DAO
    /// </summary>
    public partial class ConditionConfiguration : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ConditionConfiguration_DAO>, IConditionConfiguration
    {
        public ConditionConfiguration(SAHL.Common.BusinessModel.DAO.ConditionConfiguration_DAO ConditionConfiguration)
            : base(ConditionConfiguration)
        {
            this._DAO = ConditionConfiguration;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionConfiguration_DAO.GenericColumnDefinitionValue
        /// </summary>
        public Int32 GenericColumnDefinitionValue
        {
            get { return _DAO.GenericColumnDefinitionValue; }
            set { _DAO.GenericColumnDefinitionValue = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionConfiguration_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionConfiguration_DAO.GenericKeyType
        /// </summary>
        public IGenericKeyType GenericKeyType
        {
            get
            {
                if (null == _DAO.GenericKeyType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IGenericKeyType, GenericKeyType_DAO>(_DAO.GenericKeyType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.GenericKeyType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.GenericKeyType = (GenericKeyType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionConfiguration_DAO.GenericColumnDefinition
        /// </summary>
        public IGenericColumnDefinition GenericColumnDefinition
        {
            get
            {
                if (null == _DAO.GenericColumnDefinition) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IGenericColumnDefinition, GenericColumnDefinition_DAO>(_DAO.GenericColumnDefinition);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.GenericColumnDefinition = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.GenericColumnDefinition = (GenericColumnDefinition_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionConfiguration_DAO.ConditionSets
        /// </summary>
        private DAOEventList<ConditionSet_DAO, IConditionSet, ConditionSet> _ConditionSets;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionConfiguration_DAO.ConditionSets
        /// </summary>
        public IEventList<IConditionSet> ConditionSets
        {
            get
            {
                if (null == _ConditionSets)
                {
                    if (null == _DAO.ConditionSets)
                        _DAO.ConditionSets = new List<ConditionSet_DAO>();
                    _ConditionSets = new DAOEventList<ConditionSet_DAO, IConditionSet, ConditionSet>(_DAO.ConditionSets);
                    _ConditionSets.BeforeAdd += new EventListHandler(OnConditionSets_BeforeAdd);
                    _ConditionSets.BeforeRemove += new EventListHandler(OnConditionSets_BeforeRemove);
                    _ConditionSets.AfterAdd += new EventListHandler(OnConditionSets_AfterAdd);
                    _ConditionSets.AfterRemove += new EventListHandler(OnConditionSets_AfterRemove);
                }
                return _ConditionSets;
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _ConditionSets = null;
        }
    }
}