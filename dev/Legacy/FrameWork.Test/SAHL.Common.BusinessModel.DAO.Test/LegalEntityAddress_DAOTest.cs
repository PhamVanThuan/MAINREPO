using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using SAHL.Test;
using SAHL.Common.BusinessModel.DAO;

using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Test.DAOHelpers;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    /// <summary>
    /// Class for testing the <see cref="LegalEntityAddress_DAO"/> entity.
    /// </summary>
    [TestFixture]
    public class LegalEntityAddress_DAOTest : TestBase
    {

        #region Static Helper Methods

        /// <summary>
        /// Deletes a LegalEntityAddress entity from the database.
        /// </summary>
        /// <param name="legalEntityAddress"></param>
        public static void DeleteLegalEntityAddress(LegalEntityAddress_DAO legalEntityAddress)
        {
            if (legalEntityAddress != null && legalEntityAddress.Key > 0)
                TestBase.DeleteRecord("LegalEntityAddress", "LegalEntityAddressKey", legalEntityAddress.Key.ToString());
        }

        /// <summary>
        /// Helper method to create a new <see cref="LegalEntityAddress_DAO"/> entity.
        /// </summary>
        /// <returns>A new AddressBox entity (not saved to the database).</returns>
        public static LegalEntityAddress_DAO CreateLegalEntityAddress(Address_DAO address, LegalEntity_DAO legalEntity)
        {
            LegalEntityAddress_DAO lea = new LegalEntityAddress_DAO();
            lea.Address = address;
            lea.AddressType = AddressType_DAO.FindFirst();
            lea.EffectiveDate = DateTime.Now;
            lea.GeneralStatus = GeneralStatus_DAO.FindFirst();
            lea.LegalEntity = legalEntity;
            return lea;
        }        

        #endregion

    }
}
