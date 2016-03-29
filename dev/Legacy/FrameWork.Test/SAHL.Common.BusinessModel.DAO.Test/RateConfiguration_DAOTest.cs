using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    /// <summary>
    /// Class for testing the <see cref="RateConfiguration_DAO"/> entity.
    /// </summary>
    [TestFixture]
    public class RateConfiguration_DAOTest : TestBase
    {
        /// <summary>
        /// Tests loading, saving and loading of RateConfiguration_DAO.  This cannot be done via the
        /// generic tests otherwise you get random unique key violation errors
        /// </summary>
        [Test]
        public void LoadSaveLoad()
        {
            using (new SessionScope())
            {
                // create a fake market rate
                MarketRate_DAO marketRate = DAODataConsistencyChecker.GetDAO<MarketRate_DAO>();
                marketRate.SaveAndFlush();

                // try and load any random item
                RateConfiguration_DAO rateConfig = new RateConfiguration_DAO();
                rateConfig.Margin = Margin_DAO.FindFirst();
                rateConfig.MarketRate = marketRate;
                rateConfig.SaveAndFlush();

                int key = rateConfig.Key;

                // now try and load it
                RateConfiguration_DAO rateConfig2 = RateConfiguration_DAO.Find(key);
                Assert.IsNotNull(rateConfig2);

                // delete all the data
                rateConfig2.DeleteAndFlush();
                marketRate.DeleteAndFlush();
            }
        }
    }
}