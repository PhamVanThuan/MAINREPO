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
    /// Class for testing the <see cref="SubsidyProviderType"/> entity.
    /// </summary>
//    [TestFixture]
    public class SubsidyProviderType_DAOTest : TestBase
    {
        #region Static helper methods

        public static SubsidyProviderType_DAO CreateSubsidyProviderType()
        {
            SubsidyProviderType_DAO SubsidyProviderType = new SubsidyProviderType_DAO();
            SubsidyProviderType.Description = "Test Description";
            return SubsidyProviderType;
        }

        #endregion
    }
}
