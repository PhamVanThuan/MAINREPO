using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Test;
using System.Collections.Generic;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    /// <summary>
    /// Class for testing the <see cref="Application_DAO"/> entity.
    /// </summary>
    [TestFixture]
    public class Application_DAOTest : TestBase
    {
        /// <summary>
        /// Tests the retrieval of applications off an Account.
        /// </summary>
        [Test]
        public void ApplicationInformations()
        {
            using (new SessionScope())
            {
                object key = base.GetPrimaryKey("Offer", "OfferKey", "OfferTypeKey = 6");  // we know RCS loan accounts have offers.
                Application_DAO app = (Application_DAO)base.TestFind<Application_DAO>(key);
                IList<ApplicationInformation_DAO> informations = app.ApplicationInformations;

                for (int i = 0; i < informations.Count; i++)
                {
                    //int cnt = ai.ApplicationInformations.Count;
                    IApplicationMortgageLoanSwitch ia = informations[i] as IApplicationMortgageLoanSwitch;
                }
            }
        }
    }
}