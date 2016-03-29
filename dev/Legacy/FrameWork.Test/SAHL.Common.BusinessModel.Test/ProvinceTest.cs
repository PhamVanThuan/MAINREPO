using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Test;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel.Test
{
    [TestFixture]
    public class ProvinceTest : TestBase
    {

        /// <summary>
        /// Tests the <see cref="Province.SuburbsByPrefix"/> property.
        /// </summary>
        [Test]
        public void SuburbsByPrefix()
        {
            Province_DAO provinceDao = Province_DAO.FindFirst();
            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            Province province = BMTM.GetMappedType<Province>(provinceDao);
            // Province province = base.TestFind<Province_DAO>("Province", "ProvinceKey");
            IReadOnlyEventList<ISuburb> suburbs = province.SuburbsByPrefix("A", 10);
        }

    }
}
