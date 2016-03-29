using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ConditionSetCondition_DAO
    /// </summary>
    public partial class ConditionSetCondition : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ConditionSetCondition_DAO>, IConditionSetCondition
    {
        public ConditionSetCondition(SAHL.Common.BusinessModel.DAO.ConditionSetCondition_DAO ConditionSetCondition)
            : base(ConditionSetCondition)
        {
            this._DAO = ConditionSetCondition;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionSetCondition_DAO.RequiredCondition
        /// </summary>
        public Boolean RequiredCondition
        {
            get { return _DAO.RequiredCondition; }
            set { _DAO.RequiredCondition = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionSetCondition_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionSetCondition_DAO.ConditionSet
        /// </summary>
        public IConditionSet ConditionSet
        {
            get
            {
                if (null == _DAO.ConditionSet) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IConditionSet, ConditionSet_DAO>(_DAO.ConditionSet);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ConditionSet = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ConditionSet = (ConditionSet_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionSetCondition_DAO.Condition
        /// </summary>
        public ICondition Condition
        {
            get
            {
                if (null == _DAO.Condition) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICondition, Condition_DAO>(_DAO.Condition);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Condition = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Condition = (Condition_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}