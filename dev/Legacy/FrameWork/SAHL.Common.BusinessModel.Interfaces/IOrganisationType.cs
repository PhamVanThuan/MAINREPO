using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.OrganisationType_DAO
    /// </summary>
    public partial interface IOrganisationType : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OrganisationType_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OrganisationType_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }
    }
}