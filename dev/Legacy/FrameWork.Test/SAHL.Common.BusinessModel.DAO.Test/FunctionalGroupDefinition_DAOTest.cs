using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using SAHL.Test;
using SAHL.Common.BusinessModel.DAO;

using Castle.ActiveRecord;
using NUnit.Framework;
using Castle.ActiveRecord.Queries;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    /// <summary>
    /// Class for testing the <see cref="FunctionalGroupDefinition_DAO"/> entity.
    /// </summary>
    [TestFixture]
    public class FunctionalGroupDefinition_DAOTest : TestBase
    {

        /// <summary>
        /// Tests the retrieval of a <see cref="FunctionalGroupDefinition_DAO"/> object.
        /// </summary>
        [Test]
        public void Find()
        {
            base.TestFind<FunctionalGroupDefinition_DAO>("FunctionalGroupDefinition", "FunctionalGroupDefinitionKey");
        }

        [Test]
        public void GetUserGroupMapping()
        {
            using (new TransactionScope())
            {
                //FunctionalGroupDefinition_DAO dao = (FunctionalGroupDefinition_DAO) base.TestFind<FunctionalGroupDefinition_DAO>(2);

                //string HQL = String.Format("from UserGroupMapping_DAO ugm where ugm.FunctionalGroupDefinition.Key = {0}", 2);                
                //SimpleQuery<UserGroupMapping_DAO> qt = new SimpleQuery<UserGroupMapping_DAO>(HQL);
                //UserGroupMapping_DAO[] results = qt.Execute();

                //UserGroupMapping_DAO res = results[0];

                //if (Messages == null)
                //    throw new ArgumentNullException(StaticMessages.NullDomainCollection);
                FunctionalGroupDefinition_DAO[] FGD = FunctionalGroupDefinition_DAO.FindAllByProperty("FunctionalGroupName", "FurtherLendingProcessor");
                FunctionalGroupDefinition fgd = new FunctionalGroupDefinition(FGD[0]);
                int fgdKey = fgd.Key;
                //UserGroupMapping_DAO dao = UserGroupMapping_DAO.Find(fgdKey);
                //IUserGroupMapping iugm = new UserGroupMapping(dao);
                //return iugm;
                string HQL = string.Format("from UserGroupMapping_DAO ugm where ugm.FunctionalGroupDefinition.Key={0}", fgdKey);
                SimpleQuery query = new SimpleQuery(typeof(UserGroupMapping_DAO), HQL);

                object o = UserGroupMapping_DAO.ExecuteQuery(query);
                UserGroupMapping_DAO[] arrugm = (UserGroupMapping_DAO[])o;
                UserGroupMapping_DAO ugm = arrugm[0];
                //IUserGroupMapping iugm = new UserGroupMapping(ugm);
                //return iugm;


            }
        }

    }
}
