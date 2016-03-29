using Castle.ActiveRecord.Queries;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Service;
using System.Collections.Generic;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(IAffordabilityAssessmentRepository))]
    public class AffordabilityAssessmentRepository : AbstractRepositoryBase, IAffordabilityAssessmentRepository
    {
        public AffordabilityAssessmentRepository()
        {
            if (castleTransactionService == null)
            {
                castleTransactionService = new CastleTransactionsService();
            }
        }

        public AffordabilityAssessmentRepository(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        #region IAffordabilityAssessmentRepository members

        public IList<IAffordabilityAssessment> GetActiveApplicationAffordabilityAssessments(int applicationKey)
        {
            SimpleQuery<AffordabilityAssessment_DAO> query = 
                new SimpleQuery<AffordabilityAssessment_DAO>(@"select a 
from AffordabilityAssessment_DAO a 
where a.GenericKey = ? 
and a.GenericKeyType.Key = 2 
and a.GeneralStatus.Key = 1", applicationKey);

            AffordabilityAssessment_DAO[] affAssDAOs = query.Execute();

            IList<IAffordabilityAssessment> list = new List<IAffordabilityAssessment>();
            for (int i = 0; i < affAssDAOs.Length; i++)
            {
                list.Add(new AffordabilityAssessment(affAssDAOs[i]));
            }
            return list;
        }

        public IAffordabilityAssessment GetAffordabilityAssessmentByKey(int affordabilityAssessmentKey)
        {
            return base.GetByKey<IAffordabilityAssessment, AffordabilityAssessment_DAO>(affordabilityAssessmentKey);
        }

        public void SaveAffordabilityAssessment(IAffordabilityAssessment affordabilityAssessment)
        {
            base.Save<IAffordabilityAssessment, AffordabilityAssessment_DAO>(affordabilityAssessment);
        }

        #endregion IAffordabilityAssessmentRepository members
    }
}