using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationAnswer_DAO
    /// </summary>
    public partial interface IApplicationDeclarationAnswer : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationAnswer_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationAnswer_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationAnswer_DAO.ApplicationDeclarations
        /// </summary>
        IEventList<IApplicationDeclaration> ApplicationDeclarations
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationAnswer_DAO.ApplicationDeclarationQuestionAnswers
        /// </summary>
        IEventList<IApplicationDeclarationQuestionAnswer> ApplicationDeclarationQuestionAnswers
        {
            get;
        }
    }
}