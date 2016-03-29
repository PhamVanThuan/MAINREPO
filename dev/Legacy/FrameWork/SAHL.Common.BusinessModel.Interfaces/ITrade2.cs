using SAHL.Common.BusinessModel.Validation;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface ITrade : IEntityValidation
    {
        /// <summary>
        ///
        /// </summary>
        ICapType CapType
        {
            get;
            set;
        }
    }
}