using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.InputGenericType_DAO
    /// </summary>
    public partial interface IInputGenericType : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.InputGenericType_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.InputGenericType_DAO.CoreBusinessObjectMenu
        /// </summary>
        ICBOMenu CoreBusinessObjectMenu
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.InputGenericType_DAO.GenericKeyTypeParameter
        /// </summary>
        IGenericKeyTypeParameter GenericKeyTypeParameter
        {
            get;
            set;
        }
    }
}