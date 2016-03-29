using Castle.ActiveRecord.Queries;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(IVendorRepository))]
    public class VendorRepository : AbstractRepositoryBase, IVendorRepository
    {
        private ICastleTransactionsService castleTransactionService;

        public VendorRepository(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        public VendorRepository()
        {
            this.castleTransactionService = new CastleTransactionsService();
        }

        public IVendor GetVendorByLegalEntityKey(int legalEntityKey)
        {
            string HQL = "from Vendor_DAO v where v.LegalEntity.Key = ?";
            SimpleQuery<Vendor_DAO> q = new SimpleQuery<Vendor_DAO>(HQL, legalEntityKey);
            Vendor_DAO[] res = q.Execute();

            if (res != null && res.Length > 0)
            {
                IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return bmtm.GetMappedType<IVendor, Vendor_DAO>(res[0]);
            }

            return null;    
        }
    }
}
