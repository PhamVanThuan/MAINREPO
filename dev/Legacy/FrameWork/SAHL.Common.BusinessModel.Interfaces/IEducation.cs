using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Education_DAO
    /// </summary>
    public partial interface IEducation : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Education_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Education_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}