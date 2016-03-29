//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Text;
//using SAHL.Test;
//using SAHL.Common.BusinessModel.DAO;

//using Castle.ActiveRecord;
//using NUnit.Framework;

//namespace SAHL.Common.BusinessModel.DAO.Test
//{
//    /// <summary>
//    /// Class for testing the <see cref="RuleSetRule_DAO"/> entity.
//    /// </summary>
//    [TestFixture]
//    public class RuleSetRule_DAOTest : TestBase
//    {

//        /// <summary>
//        /// Tests the retrieval of a <see cref="RuleSetRule_DAO"/> object.
//        /// </summary>
//        [Test]
//        public void Find()
//        {
//            base.TestFind<RuleSetRule_DAO>("RuleSetRule", "RuleSetRuleKey");
//        }



//        [Test]
//        [Ignore]
//        public void Save()
//        {
//            RuleSetRule_DAO RuleSetRule = CreateRuleSetRule();
//            RuleSetRule.Save();
//            RuleSetRule.EnForceRule = true;
//            RuleSetRule.Save();
//            RuleSetRule.Delete();
//        }

//        #region Static helper methods

//        public static RuleSetRule_DAO CreateRuleSetRule()
//        {
//            RuleSetRule_DAO RuleSetRule = new RuleSetRule_DAO();
//            RuleSetRule.RuleSet = RuleSet_DAO.FindFirst();
//            RuleSetRule.RuleItem = RuleItem_DAO.FindFirst();
//            RuleSetRule.EnForceRule = true;
//            return RuleSetRule;
//        }

//        #endregion

//    }
//}
