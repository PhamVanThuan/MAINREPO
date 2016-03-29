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
    /// SAHL.Common.BusinessModel.DAO.ConditionSetUIStatement_DAO
    /// </summary>
    public partial class ConditionSetUIStatement : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ConditionSetUIStatement_DAO>, IConditionSetUIStatement
    {
        public ConditionSetUIStatement(SAHL.Common.BusinessModel.DAO.ConditionSetUIStatement_DAO ConditionUIStatement)
            : base(ConditionUIStatement)
        {
            this._DAO = ConditionUIStatement;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionSetUIStatement_DAO.Key
        /// </summary>
        public int Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionSetUIStatement_DAO.ConditionSet
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
        /// SAHL.Common.BusinessModel.DAO.ConditionSetUIStatement_DAO.UIStatementName
        /// </summary>
        public string UIStatementName
        {
            get { return _DAO.UIStatementName; }
            set { _DAO.UIStatementName = value; }
        }
    }
}
