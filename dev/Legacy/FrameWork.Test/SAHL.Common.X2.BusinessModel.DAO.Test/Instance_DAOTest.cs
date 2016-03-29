using NUnit.Framework;
using SAHL.Common.X2.BusinessModel.DAO;
using SAHL.Test;

namespace SAHL.Common.X2.BusinessModel.Test
{
    [TestFixture]
    public class Instance_DAOTest : TestBase
    {
        [Test]
        public void Find()
        {
            base.TestFind<Instance_DAO>("X2.X2.Instance", "ID");
        }
    }
}