using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AffordabilityAssessmentStatus_DAO
    /// </summary>
    public partial class AffordabilityAssessmentStatus : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AffordabilityAssessmentStatus_DAO>, IAffordabilityAssessmentStatus
    {
        public AffordabilityAssessmentStatus(SAHL.Common.BusinessModel.DAO.AffordabilityAssessmentStatus_DAO AffordabilityAssessmentStatus)
            : base(AffordabilityAssessmentStatus)
        {
            this._DAO = AffordabilityAssessmentStatus;
        }

        
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AffordabilityAssessmentStatus_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AffordabilityAssessmentStatus_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}
