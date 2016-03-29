using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ConditionSet_DAO
    /// </summary>
    public partial class ConditionSet : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ConditionSet_DAO>, IConditionSet
    {
        public ConditionSet(SAHL.Common.BusinessModel.DAO.ConditionSet_DAO ConditionSet)
            : base(ConditionSet)
        {
            this._DAO = ConditionSet;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionSet_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionSet_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionSet_DAO.ConditionSetConditions
        /// </summary>
        private DAOEventList<ConditionSetCondition_DAO, IConditionSetCondition, ConditionSetCondition> _ConditionSetConditions;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionSet_DAO.ConditionSetConditions
        /// </summary>
        public IEventList<IConditionSetCondition> ConditionSetConditions
        {
            get
            {
                if (null == _ConditionSetConditions)
                {
                    if (null == _DAO.ConditionSetConditions)
                        _DAO.ConditionSetConditions = new List<ConditionSetCondition_DAO>();
                    _ConditionSetConditions = new DAOEventList<ConditionSetCondition_DAO, IConditionSetCondition, ConditionSetCondition>(_DAO.ConditionSetConditions);
                    _ConditionSetConditions.BeforeAdd += new EventListHandler(OnConditionSetConditions_BeforeAdd);
                    _ConditionSetConditions.BeforeRemove += new EventListHandler(OnConditionSetConditions_BeforeRemove);
                    _ConditionSetConditions.AfterAdd += new EventListHandler(OnConditionSetConditions_AfterAdd);
                    _ConditionSetConditions.AfterRemove += new EventListHandler(OnConditionSetConditions_AfterRemove);
                }
                return _ConditionSetConditions;
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionSet_DAO.ConditionConfigurations
        /// </summary>
        private DAOEventList<ConditionConfiguration_DAO, IConditionConfiguration, ConditionConfiguration> _ConditionConfigurations;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionSet_DAO.ConditionConfigurations
        /// </summary>
        public IEventList<IConditionConfiguration> ConditionConfigurations
        {
            get
            {
                if (null == _ConditionConfigurations)
                {
                    if (null == _DAO.ConditionConfigurations)
                        _DAO.ConditionConfigurations = new List<ConditionConfiguration_DAO>();
                    _ConditionConfigurations = new DAOEventList<ConditionConfiguration_DAO, IConditionConfiguration, ConditionConfiguration>(_DAO.ConditionConfigurations);
                    _ConditionConfigurations.BeforeAdd += new EventListHandler(OnConditionConfigurations_BeforeAdd);
                    _ConditionConfigurations.BeforeRemove += new EventListHandler(OnConditionConfigurations_BeforeRemove);
                    _ConditionConfigurations.AfterAdd += new EventListHandler(OnConditionConfigurations_AfterAdd);
                    _ConditionConfigurations.AfterRemove += new EventListHandler(OnConditionConfigurations_AfterRemove);
                }
                return _ConditionConfigurations;
            }
        }

        private DAOEventList<ConditionSetUIStatement_DAO, IConditionSetUIStatement, ConditionSetUIStatement> _ConditionSetUIStatements;

        public IEventList<IConditionSetUIStatement> ConditionSetUIStatements
        {
            get
            {
                if (null == _ConditionSetUIStatements)
                {
                    if (null == _DAO.ConditionSetUIStatements)
                        _DAO.ConditionSetUIStatements = new List<ConditionSetUIStatement_DAO>();
                    _ConditionSetUIStatements = new DAOEventList<ConditionSetUIStatement_DAO, IConditionSetUIStatement, ConditionSetUIStatement>(_DAO.ConditionSetUIStatements);
                }
                return _ConditionSetUIStatements;
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _ConditionSetConditions = null;
            _ConditionConfigurations = null;
        }
    }
}