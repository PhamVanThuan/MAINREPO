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
    /// Class for testing the <see cref="Salutation_DAO"/> entity.
    /// </summary>
//    [TestFixture]
    public class Salutation_DAOTest : TestBase
    {

        #region Static helper methods

        public static Salutation_DAO CreateSalutation()
        {
            Salutation_DAO Salutation = new Salutation_DAO();
            Salutation.Description = "Test Description";
            Salutation.TranslatableItem = TranslatableItem_DAO.FindFirst();
            return Salutation;
        }

        #endregion

    }
}
