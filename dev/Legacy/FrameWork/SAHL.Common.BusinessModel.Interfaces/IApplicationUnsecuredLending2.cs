using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Derived from the Application_DAO and instantiated to represent an Unsecured Lending Application.
    /// DiscriminatorValue = "11"
    /// </summary>
    public partial interface IApplicationUnsecuredLending : IEntityValidation, IBusinessModelObject, IApplication
    {
        void SetProduct(SAHL.Common.Globals.ProductsUnsecuredLending product);

        IReadOnlyEventList<IExternalRole> ActiveClientRoles { get; }
    }
}