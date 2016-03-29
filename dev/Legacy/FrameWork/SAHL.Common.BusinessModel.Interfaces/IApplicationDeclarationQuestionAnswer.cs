using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationQuestionAnswer_DAO
    /// </summary>
    public partial interface IApplicationDeclarationQuestionAnswer : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationQuestionAnswer_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationQuestionAnswer_DAO.ApplicationDeclarationAnswer
        /// </summary>
        IApplicationDeclarationAnswer ApplicationDeclarationAnswer
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationQuestionAnswer_DAO.ApplicationDeclarationQuestion
        /// </summary>
        IApplicationDeclarationQuestion ApplicationDeclarationQuestion
        {
            get;
            set;
        }
    }
}