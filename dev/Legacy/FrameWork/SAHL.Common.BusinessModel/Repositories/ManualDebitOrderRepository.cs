using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Castle.ActiveRecord.Queries;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(IManualDebitOrderRepository))]
    public class ManualDebitOrderRepository : AbstractRepositoryBase, IManualDebitOrderRepository
    {
        public IManualDebitOrder GetEmptyManualDebitOrder()
        {
            return base.CreateEmpty<IManualDebitOrder, ManualDebitOrder_DAO>();
        }

        public void SaveManualDebitOrder(IManualDebitOrder manualDebitOrder)
        {
            base.Save<IManualDebitOrder, ManualDebitOrder_DAO>(manualDebitOrder);
        }

        public void CancelManualDebitOrder(IManualDebitOrder manualDebitOrder)
        {
            ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

            manualDebitOrder.GeneralStatus = lookupRepo.GeneralStatuses[Globals.GeneralStatuses.Inactive];
        }

        public IManualDebitOrder GetManualDebitOrderByKey(int Key)
        {
            ManualDebitOrder_DAO dao = ManualDebitOrder_DAO.TryFind(Key);
            if (dao == null)
                return null;

            IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return bmtm.GetMappedType<IManualDebitOrder>(dao);
        }

        public IEventList<IManualDebitOrder> GetPendingManualDebitOrdersByFinancialServiceKey(int financialServiceKey)
        {
            IEventList<IManualDebitOrder> lst = new EventList<IManualDebitOrder>();

            string sql = UIStatementRepository.GetStatement("Repositories.ManualDebitOrderRepository", "GetPendingManualDebitOrdersByFinancialServiceKey");

            SimpleQuery<ManualDebitOrder_DAO> query = new SimpleQuery<ManualDebitOrder_DAO>(QueryLanguage.Sql, sql, financialServiceKey);
            query.AddSqlReturnDefinition(typeof(ManualDebitOrder_DAO), "mdo");
            ManualDebitOrder_DAO[] res = query.Execute();

            if (res != null && res.Length > 0)
            {
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                foreach (ManualDebitOrder_DAO mdo in res)
                {
                    lst.Add(null, BMTM.GetMappedType<IManualDebitOrder, ManualDebitOrder_DAO>(mdo));
                }
            }

            return lst;
        }

        public IEventList<IManualDebitOrder> GetCollectedManualDebitOrdersByFinancialServiceKey(int financialServiceKey)
        {
            IEventList<IManualDebitOrder> lst = new EventList<IManualDebitOrder>();

            string sql = UIStatementRepository.GetStatement("Repositories.ManualDebitOrderRepository", "GetCollectedManualDebitOrdersByFinancialServiceKey");

            SimpleQuery<ManualDebitOrder_DAO> query = new SimpleQuery<ManualDebitOrder_DAO>(QueryLanguage.Sql, sql, financialServiceKey);
            query.AddSqlReturnDefinition(typeof(ManualDebitOrder_DAO), "mdo");
            ManualDebitOrder_DAO[] res = query.Execute();

            if (res != null && res.Length > 0)
            {
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                foreach (ManualDebitOrder_DAO mdo in res)
                {
                    lst.Add(null, BMTM.GetMappedType<IManualDebitOrder, ManualDebitOrder_DAO>(mdo));
                }
            }

            return lst;
        }
    }
}