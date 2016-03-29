using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationMarketingSurveyType_DAO
    /// </summary>
    public partial interface IApplicationMarketingSurveyType : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationMarketingSurveyType_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationMarketingSurveyType_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationMarketingSurveyType_DAO.ApplicationMarketingSurveyTypeGroup
        /// </summary>
        IApplicationMarketingSurveyTypeGroup ApplicationMarketingSurveyTypeGroup
        {
            get;
            set;
        }
    }
}