using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    /// <summary>
    /// Class for testing the <see cref="MortgageLoan_DAO"/> entity.
    /// </summary>
    [TestFixture]
    public class MortgageLoan_DAOTest : TestBase
    {
        [Test]
        public void SaveRelationship()
        {
            using (new TransactionScope())
            {
                MortgageLoan_DAO Ml = base.TestFind<MortgageLoan_DAO>("fin.MortgageLoan", "FinancialServiceKey");
                Bond_DAO Bond = Bond_DAO.FindFirst();

                Ml.Bonds.Add(Bond);

                Ml.SaveAndFlush();
                Ml.Bonds.Remove(Bond);
                Ml.SaveAndFlush();
            }
        }
    }
}

