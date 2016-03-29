using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Test;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAHL.Common.BusinessModel.Spec.Test.Repositories.CreditMatrix
{
    public class CreditCriteriaTestBase : TestBase
    {
        protected static void GetCreditCriteriaForCreditCriteriaAssertTest(List<CreditCriteriaAssertTest> creditCriteriaAssertTests)
        {
            foreach (var creditCriteriaAssertTest in creditCriteriaAssertTests)
            {
                string HQL = @"select cc from CreditCriteria_DAO cc join cc.CreditMatrix as cm join cc.CreditMatrix.OriginationSourceProducts as osp where cc.MortgageLoanPurpose.Key = ?
                AND cc.EmploymentType.Key = ? AND cc.MaxLoanAmount >= ? AND cm.NewBusinessIndicator = 'Y' AND osp.OriginationSource.Key = ? AND osp.Product.Key = ?
                AND cc.Category.Key = ?
                ORDER BY cc.MaxLoanAmount asc, cc.Margin.Value asc, cc.LTV asc";

                SimpleQuery<CreditCriteria_DAO> q = new SimpleQuery<CreditCriteria_DAO>(HQL, creditCriteriaAssertTest.MortgageLoanPurposeKey, creditCriteriaAssertTest.EmploymentTypeKey, creditCriteriaAssertTest.TotalLoanAmount,
                    creditCriteriaAssertTest.OriginationSourceKey, creditCriteriaAssertTest.ProductKey, creditCriteriaAssertTest.CategoryKey);

                CreditCriteria_DAO[] res = q.Execute();

                if (res.Length > 0)
                {
                    IBusinessModelTypeMapper typeMapper = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    creditCriteriaAssertTest.DeterminedCreditCriteria = typeMapper.GetMappedType<ICreditCriteria, CreditCriteria_DAO>(res[0]);
                    creditCriteriaAssertTest.DeterminedPTI = creditCriteriaAssertTest.DeterminedCreditCriteria.PTI.Value;
                }
                else
                {
                    creditCriteriaAssertTest.DeterminedCreditCriteria = null;
                }
            }
        }

        protected static ICreditCriteriaRepository creditCriteriaRepository;

        protected CreditCriteriaTestBase()
        {
            creditCriteriaRepository = new CreditCriteriaRepository();
        }

        protected static void GetCreditCriteria(List<CreditCriteriaDetermineTest> creditCriteriaTestPacks)
        {
            using (new SessionScope())
            {
                foreach (var testPack in creditCriteriaTestPacks)
                {
                    ICreditCriteria creditCriteria = null;
                    if (testPack.CreditCriteriaAttributeTypeKey > 0)
                    {
                        creditCriteria = creditCriteriaRepository.GetCreditCriteriaByLTVAndIncome(null, testPack.OriginationSourceKey, testPack.ProductKey, testPack.MortgageLoanPurposeKey, testPack.EmploymentTypeKey,
                            testPack.TotalLoanAmount, testPack.LTV / 100, testPack.Income, (CreditCriteriaAttributeTypes)Enum.Parse(typeof(CreditCriteriaAttributeTypes),testPack.CreditCriteriaAttributeTypeKey.ToString()));
                    }
                    else
                    {
                        creditCriteria = creditCriteriaRepository.GetCreditCriteriaByLTVAndIncome(null, testPack.OriginationSourceKey, testPack.ProductKey, testPack.MortgageLoanPurposeKey, testPack.EmploymentTypeKey, testPack.TotalLoanAmount, testPack.LTV / 100, testPack.Income,Globals.CreditCriteriaAttributeTypes.NewBusiness);
                    }
                    if (creditCriteria != null)
                    {
                        testPack.DeterminedCategoryKey = creditCriteria.Category.Key;
                    }
                    else
                    {
                        testPack.DeterminedCategoryKey = -1;
                    }
                }
            }
        }

        protected static void CheckDeterminedCategory(List<CreditCriteriaDetermineTest> creditCriteriaTestPacks, string contextClassName)
        {
            foreach (var testPack in creditCriteriaTestPacks)
            {
                testPack.ContextClassName = contextClassName;
                Assert.AreEqual(testPack.ExpectedCategoryKey, testPack.DeterminedCategoryKey, testPack.ToString());
            }
        }

        protected static void CheckAssertionTests(List<CreditCriteriaAssertTest> CreditCriteriaAssertTests, string contextClassName)
        {
            foreach (var test in CreditCriteriaAssertTests)
            {
                test.ContextClassName = contextClassName;
                Assert.AreEqual(test.ExpectedPTI, test.DeterminedPTI, test.ToString());
            }
        }
    }

    public class CreditCriteriaDetermineTest
    {
        public string ContextClassName { get; set; }

        public int OriginationSourceKey { get; set; }

        public int ProductKey { get; set; }

        public int MortgageLoanPurposeKey { get; set; }

        public int EmploymentTypeKey { get; set; }

        public int ExpectedCategoryKey { get; set; }

        public int DeterminedCategoryKey { get; set; }

        public int CreditCriteriaAttributeTypeKey { get; set; }

        public double TotalLoanAmount
        {
            get;
            set;
        }

        public double LTV { get; set; }

        public double Income { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} Failed.", ContextClassName);

            if (ExpectedCategoryKey != DeterminedCategoryKey)
            {
                sb.AppendLine(Environment.NewLine);
                sb.AppendFormat("Expected CategoryKey: {0} Determined CategoryKey: {1}", ExpectedCategoryKey, DeterminedCategoryKey);
            }

            sb.AppendLine(Environment.NewLine);
            sb.AppendFormat("LTV:{0} EmploymentTypeKey:{1} Income:{2} MortgageLoanPurposeKey:{3} TotalLoanAmount:{4}", LTV, EmploymentTypeKey, Income, MortgageLoanPurposeKey, TotalLoanAmount);
            sb.AppendLine(Environment.NewLine);
            return sb.ToString();
        }
    }

    public class CreditCriteriaAssertTest
    {
        public ICreditCriteria DeterminedCreditCriteria { get; set; }

        public string ContextClassName { get; set; }

        public int OriginationSourceKey { get; set; }

        public int ProductKey { get; set; }

        public int MortgageLoanPurposeKey { get; set; }

        public int EmploymentTypeKey { get; set; }

        public int CategoryKey { get; set; }

        public double DeterminedPTI { get; set; }

        public int ExpectedPTI { get; set; }

        public double TotalLoanAmount
        {
            get;
            set;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} Failed.", ContextClassName);

            if (ExpectedPTI != DeterminedPTI)
            {
                sb.AppendLine(Environment.NewLine);
                sb.AppendFormat("Expected PTI: {0} Determined PTI: {1}", ExpectedPTI, DeterminedPTI);
            }

            sb.AppendLine(Environment.NewLine);
            sb.AppendFormat("CategoryKey:{0} EmploymentTypeKey:{1} MortgageLoanPurposeKey:{2} TotalLoanAmount:{3}", CategoryKey, EmploymentTypeKey, MortgageLoanPurposeKey, TotalLoanAmount);
            sb.AppendLine(Environment.NewLine);
            return sb.ToString();
        }
    }
}