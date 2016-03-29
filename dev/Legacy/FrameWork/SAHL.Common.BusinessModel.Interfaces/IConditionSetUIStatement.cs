using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public interface IConditionSetUIStatement : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionSetUIStatement_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionSetUIStatement_DAO.ConditionSet
        /// </summary>
        IConditionSet ConditionSet
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ConditionSetUIStatement_DAO.UIStatementName
        /// </summary>
        System.String UIStatementName
        {
            get;
            set;
        }
    }
}
