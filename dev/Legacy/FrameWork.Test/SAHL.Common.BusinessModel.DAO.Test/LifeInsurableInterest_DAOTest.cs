using NUnit.Framework;
using SAHL.Test;
using SAHL.Test.DAOHelpers;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    /// <summary>
    /// Class for testing the <see cref="LifeInsurableInterest_DAO"/> entity.
    /// </summary>
    [TestFixture]
    public class LifeInsurableInterest_DAOTest : TestBase
    {
        /// <summary>
        /// Tests the ability to save a LifeInsurableInterest entity.
        /// </summary>
        [Test]
        public void SaveLifeInsurableInterest()
        {
            // create the object and save
            LifeInsurableInterestHelper helper = new LifeInsurableInterestHelper();
            LifeInsurableInterest_DAO lifeInsurableInterest = helper.CreateLifeInsurableInterest();

            try
            {
                lifeInsurableInterest.SaveAndFlush();
                int key = lifeInsurableInterest.Key;
                lifeInsurableInterest = null;

                // retrieve a new version of the object and ensure the value has been changed
                LifeInsurableInterest_DAO lifeInsurableInterestCheck = LifeInsurableInterest_DAO.Find(key) as LifeInsurableInterest_DAO;
                Assert.AreEqual(lifeInsurableInterestCheck.Key, key);
            }
            finally
            {
                helper.Dispose();
            }
        }
    }
}
