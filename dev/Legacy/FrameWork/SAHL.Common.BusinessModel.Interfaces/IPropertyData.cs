using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.PropertyData_DAO
    /// </summary>
    public partial interface IPropertyData : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.PropertyData_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.PropertyData_DAO.Property
        /// </summary>
        IProperty Property
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.PropertyData_DAO.PropertyDataProviderDataService
        /// </summary>
        IPropertyDataProviderDataService PropertyDataProviderDataService
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.PropertyData_DAO.PropertyID
        /// </summary>
        System.String PropertyID
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.PropertyData_DAO.Data
        /// </summary>
        System.String Data
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.PropertyData_DAO.InsertDate
        /// </summary>
        System.DateTime InsertDate
        {
            get;
            set;
        }
    }
}