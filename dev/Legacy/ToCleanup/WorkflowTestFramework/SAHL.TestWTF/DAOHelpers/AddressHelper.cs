using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO;

namespace SAHL.TestWTF.DAOHelpers
{
    /// <summary>
    /// Provides data access methods for testing the <see cref="Address_DAO"/> domain entity.
    /// </summary>
    public class AddressHelper : BaseHelper<Address_DAO>
    {
        /// <summary>
        /// Creates a new <see cref="AddressBox_DAO"/> entity.
        /// </summary>
        /// <returns>A new AddressBox_DAO entity (not yet persisted).</returns>
        public AddressBox_DAO CreateBoxAddress()
        {
            AddressBox_DAO address = new AddressBox_DAO();
            address.BoxNumber = "UnitTest";
            address.PostOffice = PostOffice_DAO.FindFirst() as PostOffice_DAO;
            address.ChangeDate = DateTime.Now;
            address.UserID = TestConstants.UnitTestUserID;

            CreatedEntities.Add(address);

            return address;
        }

        /// <summary>
        /// Ensures that all addresses created are deleted from the database.
        /// </summary>
        public override void Dispose()
        {
            foreach (Address_DAO address in CreatedEntities)
            {
                if (address.Key > 0)
                    TestBase.DeleteRecord("Address", "AddressKey", address.Key.ToString());
            }

            CreatedEntities.Clear();
        }
    }

    //public interface IHelperTest
    //{
    //    bool LoadSaveLoad
    //}
}
