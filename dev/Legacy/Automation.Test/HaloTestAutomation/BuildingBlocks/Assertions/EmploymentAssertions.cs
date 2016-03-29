using BuildingBlocks.Services.Contracts;
using NUnit.Framework;

using System.Linq;

namespace BuildingBlocks.Assertions
{
    public static class EmploymentAssertions
    {
        private static readonly IEmploymentService employmentService;

        static EmploymentAssertions()
        {
            employmentService = ServiceLocator.Instance.GetService<IEmploymentService>();
        }

        /// <summary>
        /// This assertion fetches the employer from the database by using the employer name and then ensures the employer retrieved from the
        /// database matches the one provided.
        /// </summary>
        /// <param name="expectedEmployer"></param>
        public static void AssertEmployer(Automation.DataModels.Employer expectedEmployer)
        {
            var savedEmployer = employmentService.GetEmployer(expectedEmployer.Name);
            Assert.AreEqual(1, expectedEmployer.CompareTo(savedEmployer), string.Format("Expected employer {0} to be updated.", savedEmployer.Name));
        }

        public static void AssertSalaryPaymentDay(int employmentKey, int expectedSalaryPaymentDay)
        {
            var employment = employmentService.GetEmploymentByGenericKey(employmentKey, false, true);

            var savedSalaryPaymentDay = (from r in employment
                                         select r.Column("SalaryPaymentDay").GetValueAs<int>()).FirstOrDefault();
            Assert.AreEqual(expectedSalaryPaymentDay, savedSalaryPaymentDay);
        }

        public static void AssertUnionMember(int employmentKey, bool unionMember)
        {
            var employment = employmentService.GetEmploymentByGenericKey(employmentKey, false, true);

            var savedUnionMember = (from r in employment
                                    select r.Column("UnionMember").GetValueAs<bool>()).FirstOrDefault();
            Assert.AreEqual(unionMember, savedUnionMember);
        }
    }
}