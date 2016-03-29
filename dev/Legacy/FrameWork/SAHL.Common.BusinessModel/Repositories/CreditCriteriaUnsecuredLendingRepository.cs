using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Collections;
using Castle.ActiveRecord.Queries;
using System.Collections;
using System.Collections.Generic;
using SAHL.Common.Factories;


namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(ICreditCriteriaUnsecuredLendingRepository))]
    public class CreditCriteriaUnsecuredLendingRepository : AbstractRepositoryBase, ICreditCriteriaUnsecuredLendingRepository
    {

        /// <summary>
        /// Default Constructor for CreditCriteriaUnsecuredLendingRepository class.
        /// </summary>

        public CreditCriteriaUnsecuredLendingRepository()
        { 
        }

		public ICreditCriteriaUnsecuredLending GetCreditCriteriaUnsecuredLendingByKey(int key)
		{
			CreditCriteriaUnsecuredLending_DAO creditCriteriaUnsecuredLending = CreditCriteriaUnsecuredLending_DAO.TryFind(key);
			if (creditCriteriaUnsecuredLending != null)
			{
				IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
				return BMTM.GetMappedType<ICreditCriteriaUnsecuredLending, CreditCriteriaUnsecuredLending_DAO>(creditCriteriaUnsecuredLending);
			}
			return null;
		}

        /// <summary>
        /// Gets the data from CreditCriteriaUnsecuredLending for a particular loan amount, new business indicator = Y.
        /// </summary>
        /// <param name="LoanAmount"></param>
        /// <returns></returns>
        public IReadOnlyEventList<ICreditCriteriaUnsecuredLending> GetCreditCriteriaUnsecuredLendingByLoanAmount(double LoanAmount)
        {
            string sql = string.Format(@";with terms (term, rate) as
                            (
	                            SELECT cc.Term, min(mg.Value)
	                            FROM [2am]..CreditCriteriaUnsecuredLending cc 
	                            JOIN [2am]..CreditMatrixUnsecuredLending as cm
		                            on cc.CreditMatrixUnsecuredLendingKey = cm.CreditMatrixUnsecuredLendingKey
	                            JOIN [2am]..Margin mg
		                            on mg.MarginKey = cc.MarginKey
	                            WHERE cc.MinLoanAmount <= {0} AND cc.MaxLoanAmount >= {0} 
	                            AND cm.NewBusinessIndicator = 'Y' 
	                            group by cc.Term
                            )
                            SELECT cc.*, mg.Value
                            FROM [2am]..CreditCriteriaUnsecuredLending cc 
                            JOIN [2am]..CreditMatrixUnsecuredLending as cm 
	                            on cc.CreditMatrixUnsecuredLendingKey = cm.CreditMatrixUnsecuredLendingKey
                            JOIN [2am]..Margin mg
	                            on mg.MarginKey = cc.MarginKey
                            JOIN Terms t
	                            on t.term = cc.Term and t.rate = mg.Value
                            WHERE cc.MinLoanAmount <= {0} AND cc.MaxLoanAmount >= {0} 
                            AND cm.NewBusinessIndicator = 'Y' 
                            order by term, mg.Value asc", LoanAmount);

            SimpleQuery<CreditCriteriaUnsecuredLending_DAO> q = new SimpleQuery<CreditCriteriaUnsecuredLending_DAO>(QueryLanguage.Sql, sql);
            q.AddSqlReturnDefinition(typeof(CreditCriteriaUnsecuredLending_DAO), "cc");
            CreditCriteriaUnsecuredLending_DAO[] res = q.Execute();

            IEventList<ICreditCriteriaUnsecuredLending> list = new DAOEventList<CreditCriteriaUnsecuredLending_DAO, ICreditCriteriaUnsecuredLending, CreditCriteriaUnsecuredLending>(res);
            return new ReadOnlyEventList<ICreditCriteriaUnsecuredLending>(list);

        }

        /// <summary>
        /// For closest matching criteria record for amoutn and term
        /// </summary>
        /// <param name="loanAmount"></param>
        /// <param name="term"></param>
        /// <returns></returns>
        public ICreditCriteriaUnsecuredLending GetCreditCriteriaUnsecuredLendingByLoanAmountAndTerm(double loanAmount, int term)
        {
            string sql = string.Format(@";with terms (term, rate) as
                                            (
	                                            SELECT cc.Term, min(mg.Value)
	                                            FROM [2am]..CreditCriteriaUnsecuredLending cc 
	                                            JOIN [2am]..CreditMatrixUnsecuredLending as cm
		                                            on cc.CreditMatrixUnsecuredLendingKey = cm.CreditMatrixUnsecuredLendingKey
	                                            JOIN [2am]..Margin mg
		                                            on mg.MarginKey = cc.MarginKey
	                                            WHERE cc.MinLoanAmount <= {0} AND cc.MaxLoanAmount >= {0}
	                                            AND cm.NewBusinessIndicator = 'Y' 
	                                            group by cc.Term
                                            )
                                            SELECT cc.*
                                            FROM [2am]..CreditCriteriaUnsecuredLending cc 
                                            JOIN [2am]..CreditMatrixUnsecuredLending as cm 
	                                            on cc.CreditMatrixUnsecuredLendingKey = cm.CreditMatrixUnsecuredLendingKey
                                            JOIN [2am]..Margin mg
	                                            on mg.MarginKey = cc.MarginKey
                                            JOIN Terms t
	                                            on t.term = cc.Term and t.rate = mg.Value
                                            WHERE cc.MinLoanAmount <= {0} AND cc.MaxLoanAmount >= {0} and cc.Term > {1}
                                            AND cm.NewBusinessIndicator = 'Y' 
                                            order by term",loanAmount,term);

            SimpleQuery<CreditCriteriaUnsecuredLending_DAO> q = new SimpleQuery<CreditCriteriaUnsecuredLending_DAO>(QueryLanguage.Sql, sql);
            q.AddSqlReturnDefinition(typeof(CreditCriteriaUnsecuredLending_DAO), "cc");
            CreditCriteriaUnsecuredLending_DAO[] res = q.Execute();
            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return BMTM.GetMappedType<ICreditCriteriaUnsecuredLending, CreditCriteriaUnsecuredLending_DAO>(res[0]);
        }

        /// <summary>
        /// Gets the MinLoanAmount, MaxLoanAmount and Term from the CreditCriteriaUnsecuredLending table having new business indicator = Y.
        /// </summary>
        /// <returns></returns>
        public IReadOnlyEventList<ICreditCriteriaUnsecuredLending> GetCreditCriteriaUnsecuredLendingList()
        {
            string HQL = "SELECT cc FROM CreditCriteriaUnsecuredLending_DAO cc JOIN cc.CreditMatrixUnsecuredLending as cm WHERE cm.NewBusinessIndicator = 'Y'";

            SimpleQuery<CreditCriteriaUnsecuredLending_DAO> q = new SimpleQuery<CreditCriteriaUnsecuredLending_DAO>(HQL);
            CreditCriteriaUnsecuredLending_DAO[] res = q.Execute();

            IEventList<ICreditCriteriaUnsecuredLending> list = new DAOEventList<CreditCriteriaUnsecuredLending_DAO, ICreditCriteriaUnsecuredLending, CreditCriteriaUnsecuredLending>(res);
            return new ReadOnlyEventList<ICreditCriteriaUnsecuredLending>(list);
            
        }


    }
}
