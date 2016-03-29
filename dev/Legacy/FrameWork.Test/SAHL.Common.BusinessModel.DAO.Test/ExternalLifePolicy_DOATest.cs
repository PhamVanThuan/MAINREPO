using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Test;
using System;
using System.Linq;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    public class ExternalLifePolicy_DOATest : TestBase
    {
      
        public void ExternalLifePolicyCreate()
        {
            using (new SessionScope())
            {
                Insurer_DAO insurer = Insurer_DAO.Find(3);
                LegalEntity_DAO legalEntity = LegalEntity_DAO.FindFirst();
                LifePolicyStatus_DAO lifePolicyStatus = LifePolicyStatus_DAO.Find(3);

                ExternalLifePolicy_DAO externalLifePolicy = new ExternalLifePolicy_DAO();
                externalLifePolicy.CloseDate = DateTime.Now.AddDays(10);
                externalLifePolicy.CommencementDate = DateTime.Now;
                externalLifePolicy.SumInsured = 1000;
                externalLifePolicy.PolicyCeded = true;
                externalLifePolicy.Insurer = insurer;
                externalLifePolicy.LifePolicyStatus = lifePolicyStatus;
                externalLifePolicy.PolicyNumber = "156464";
                externalLifePolicy.LegalEntity = legalEntity; 
                externalLifePolicy.CreateAndFlush();
            }
        }
        public void CreateAccountRelationship()
        {
            using (new SessionScope())
            {
                Account_DAO account = Account_DAO.FindFirst();
                ExternalLifePolicy_DAO externalLifePolicy = ExternalLifePolicy_DAO.FindFirst();
                account.ExternalLifePolicy.Add(externalLifePolicy);
            }
        }

        public void CreateOfferRelationship()
        {
            using (new SessionScope())
            {
                Application_DAO application = Application_DAO.FindFirst();
                ExternalLifePolicy_DAO externalLifePolicy = ExternalLifePolicy_DAO.FindFirst();
                application.ExternalLifePolicy.Add(externalLifePolicy);
            }
        }
    }
}