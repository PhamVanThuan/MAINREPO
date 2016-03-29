using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using System;
using System.Linq;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// Derived from the Application_DAO and instantiated to represent an Unsecured Lending Application.
    /// DiscriminatorValue = "11"
    /// </summary>
    public partial class ApplicationUnsecuredLending : Application, IApplicationUnsecuredLending
    {
        /// <summary>
        /// Set the specific product
        /// </summary>
        /// <param name="product"></param>
        public void SetProduct(SAHL.Common.Globals.ProductsUnsecuredLending product)
        {
            switch (product)
            {
                case SAHL.Common.Globals.ProductsUnsecuredLending.PersonalLoan:
                    base._currentProduct = new ApplicationProductPersonalLoan(this, true);
                    break;
            }

            Product_DAO Prod = Product_DAO.Find(Convert.ToInt32(product));
            if (null == Prod)
                throw new Exception("Product could not be found, database may be missing data.");

            GetLatestApplicationInformation().Product = new Product(Prod);
        }

        private IReadOnlyEventList<IExternalRole> _activeClientRoles;

        /// <summary>
        /// Gets the application roles associated with the Application.  This is read-only, so
        /// roles cannot be added or removed.
        /// </summary>
        public IReadOnlyEventList<IExternalRole> ActiveClientRoles
        {
            get
            {
                if (null == _activeClientRoles)
                {
                    var legalEntityRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();

                    _activeClientRoles = legalEntityRepository.GetExternalRoles(this.Key, Globals.GenericKeyTypes.Offer, Globals.ExternalRoleTypes.Client, Globals.GeneralStatuses.Active);
                }
                return _activeClientRoles;
            }
        }

        private IReadOnlyEventList<ILegalEntity> _activeClients;

        public override IReadOnlyEventList<ILegalEntity> ActiveClients
        {
            get
            {
                if (null == _activeClients)
                {
                    var les = ActiveClientRoles.Select(x => x.LegalEntity).ToList();
                    _activeClients = new ReadOnlyEventList<ILegalEntity>(les);
                }
                return _activeClients;
            }
        }
    }
}