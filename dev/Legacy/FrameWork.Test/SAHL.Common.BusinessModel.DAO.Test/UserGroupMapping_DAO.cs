using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Test;
using NUnit.Framework;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Test.DAOHelpers;

namespace SAHL.Common.BusinessModel.DAO.Test
{
  /// <summary>
  /// 
  /// </summary>
  [TestFixture]
  public class UserGroupMapping_DAOTest : TestBase
  {

    /// <summary>
    /// Tests the retrieval of a <see cref="UserGroupMapping_DAO"/> object.
    /// </summary>
    [NUnit.Framework.Test]
    public void Find()
    {
        using (new SessionScope())
        {

            UserGroupMapping_DAO ugm = base.TestFind<UserGroupMapping_DAO>("UserGroupMapping", "UserGroupMappingKey");
           // GroupMapping_DAO os = ugm.GroupMapping;
           // Assert.IsNotNull(os);
           // string desc = os.Description;
            // todo: Investigate why test is failing
            //IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            //IUserGroupMapping Iugm = BMTM.GetMappedType<IUserGroupMapping, UserGroupMapping_DAO>(ugm);
            //Assert.IsNotNull(Iugm.GroupMapping);
        }
    }

    
      [Test]
      public void Save()
      {

          // create the object and save
          UserGroupMappingHelper helper = new UserGroupMappingHelper();
          UserGroupMapping_DAO UserGroupMapping = helper.CreateUserGroupMapping();

          try
          {
              UserGroupMapping.Save();
          }
          finally
          {
              helper.Dispose();
          }

      }

  }
}
