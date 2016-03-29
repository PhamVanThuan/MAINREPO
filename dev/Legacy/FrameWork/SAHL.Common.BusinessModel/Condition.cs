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
    /// SAHL.Common.BusinessModel.DAO.Condition_DAO
    /// </summary>
    public partial class Condition : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Condition_DAO>, ICondition
    {
        public Condition(SAHL.Common.BusinessModel.DAO.Condition_DAO Condition)
            : base(Condition)
        {
            this._DAO = Condition;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Condition_DAO.ConditionPhrase
        /// </summary>
        public String ConditionPhrase
        {
            get { return _DAO.ConditionPhrase; }
            set { _DAO.ConditionPhrase = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Condition_DAO.TokenDescriptions
        /// </summary>
        public String TokenDescriptions
        {
            get { return _DAO.TokenDescriptions; }
            set { _DAO.TokenDescriptions = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Condition_DAO.TranslatableItem
        /// </summary>
        public ITranslatableItem TranslatableItem
        {
            get
            {
                if (null == _DAO.TranslatableItem) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ITranslatableItem, TranslatableItem_DAO>(_DAO.TranslatableItem);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.TranslatableItem = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.TranslatableItem = (TranslatableItem_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Condition_DAO.ConditionName
        /// </summary>
        public String ConditionName
        {
            get { return _DAO.ConditionName; }
            set { _DAO.ConditionName = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Condition_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Condition_DAO.ConditionSetConditions
        /// </summary>
        private DAOEventList<ConditionSetCondition_DAO, IConditionSetCondition, ConditionSetCondition> _ConditionSetConditions;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Condition_DAO.ConditionSetConditions
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
        /// SAHL.Common.BusinessModel.DAO.Condition_DAO.ConditionTokens
        /// </summary>
        private DAOEventList<ConditionToken_DAO, IConditionToken, ConditionToken> _ConditionTokens;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Condition_DAO.ConditionTokens
        /// </summary>
        public IEventList<IConditionToken> ConditionTokens
        {
            get
            {
                if (null == _ConditionTokens)
                {
                    if (null == _DAO.ConditionTokens)
                        _DAO.ConditionTokens = new List<ConditionToken_DAO>();
                    _ConditionTokens = new DAOEventList<ConditionToken_DAO, IConditionToken, ConditionToken>(_DAO.ConditionTokens);
                    _ConditionTokens.BeforeAdd += new EventListHandler(OnConditionTokens_BeforeAdd);
                    _ConditionTokens.BeforeRemove += new EventListHandler(OnConditionTokens_BeforeRemove);
                    _ConditionTokens.AfterAdd += new EventListHandler(OnConditionTokens_AfterAdd);
                    _ConditionTokens.AfterRemove += new EventListHandler(OnConditionTokens_AfterRemove);
                }
                return _ConditionTokens;
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Condition_DAO.ConditionType
        /// </summary>
        public IConditionType ConditionType
        {
            get
            {
                if (null == _DAO.ConditionType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IConditionType, ConditionType_DAO>(_DAO.ConditionType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ConditionType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ConditionType = (ConditionType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _ConditionSetConditions = null;
            _ConditionTokens = null;
        }
    }
}