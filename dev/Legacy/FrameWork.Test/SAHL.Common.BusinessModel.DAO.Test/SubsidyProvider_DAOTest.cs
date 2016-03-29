using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using SAHL.Test;
using SAHL.Common.BusinessModel.DAO;

using Castle.ActiveRecord;
using NUnit.Framework;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    /// <summary>
    /// Class for testing the <see cref="SubsidyProvider"/> entity.
    /// </summary>
    //[TestFixture]
    public class SubsidyProvider_DAOTest : TestBase
    {

        //[Test]
        public void Create()
        {
            using (new SessionScope())
            {
                SubsidyProvider_DAO SP = CreateSubsidyProvider();
                SP.LegalEntity = LegalEntity_DAO.FindFirst();

                SP.SaveAndFlush();

                SP.DeleteAndFlush();
            }
        }


        #region Static helper methods

        public static SubsidyProvider_DAO CreateSubsidyProvider()
        {
            SubsidyProvider_DAO SubsidyProvider = new SubsidyProvider_DAO();
            SubsidyProvider.SubsidyProviderType = SubsidyProviderType_DAO.FindFirst();
            SubsidyProvider.UserID = "Test";
           // SubsidyProvider.PostOffice = PostOffice_DAO.FindFirst();
            SubsidyProvider.ContactPerson = "Test Contact Person";
           // SubsidyProvider.AddressFormat = AddressFormat_DAO.FindFirst();
           // SubsidyProvider.BoxNumber = "Test Box";
            SubsidyProvider.ChangeDate = DateTime.Now;
           // SubsidyProvider.Description = "Test Description";    
          //  SubsidyProvider.EmailAddress = "Test@test.com";
            //SubsidyProvider.ParentSubsidyProvider = SubsidyProvider_DAO.FindFirst();
            SubsidyProvider.PersalOrganisationCode = "Test Persal Code";
          //  SubsidyProvider.PhoneCode = "Test Phone Code";
          //  SubsidyProvider.PhoneNumber = "031 123 1234";
            //SubsidyProvider.Subsidies = Subsidy_DAO.FindFirst();
            //SubsidyProvider.SubsidyProviders = SubsidyProvider_DAO.FindFirst();
            return SubsidyProvider;
        }

        #endregion
    }
}
