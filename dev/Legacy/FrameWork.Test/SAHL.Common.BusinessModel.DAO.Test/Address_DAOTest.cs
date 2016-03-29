using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Test;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    /// <summary>
    /// Class for testing the <see cref="Address_DAO"/> entity.
    /// </summary>
    [TestFixture]
    public class Address_DAOTest : TestBase
    {
        [Test, TestCaseSource(typeof(Address_DAOTest), "GetAddressDAOTypes")]
        public void LoadSaveLoad(Type daoType)
        {
            using (new SessionScope())
            {
                object daoInstance = new object();
                daoInstance = Activator.CreateInstance(daoType);
                SAHL.Common.BusinessModel.DAO.Test.DAODataConsistencyChecker.FindFirst(daoInstance, "Key");
                SAHL.Common.BusinessModel.DAO.Test.DAODataConsistencyChecker.LoadSaveLoad(daoInstance, "Key");
            }
        }

        private IEnumerable<Type> GetAddressDAOTypes()
        {
            var list = new Type []{ typeof(AddressBox_DAO), typeof(AddressClusterBox_DAO), typeof(AddressFreeText_DAO), typeof(AddressPostnetSuite_DAO), 
                typeof(AddressPrivateBag_DAO), typeof(AddressStreet_DAO) };
            return list;
        }
    }
}