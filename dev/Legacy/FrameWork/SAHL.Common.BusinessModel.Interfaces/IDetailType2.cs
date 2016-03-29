using SAHL.Common.BusinessModel.Validation;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public partial interface IDetailType : IEntityValidation
    {
        /// <summary>
        ///
        /// </summary>
        IDetailClass DetailClass
        {
            get;
            set;
        }
    }
}