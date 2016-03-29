using System;
using System.Collections.Generic;
using System.Text;
using X2DomainService.Interface.WTFSimpleTest;
using System.Security.Permissions;
using SAHL.X2.Common;
using Castle.ActiveRecord;
using SAHL.X2.Framework.Common;
using System.Data;
using SAHL.Common.BusinessModel.DAO;
using SAHL.X2.Common.DataAccess;

namespace DomainService.Workflow
{
    [Serializable]
    [ServiceAttribute("WTFSimpleTest", WorkflowPorts.WorkflowPort, typeof(IWTFSimpleTest), typeof(Workflow.WTFSimpleTest))]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class WTFSimpleTest : WorkflowBase, IWTFSimpleTest
    {
        public WTFSimpleTest() : base()
        {
        }

        protected override SAHL.Common.BusinessModel.DAO.ApplicationRoleType_DAO GetApplicationRoleType(SAHL.Common.BusinessModel.DAO.ADUser_DAO user)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Test()
        {
            return true;
        }

        public IX2ReturnData GetUserWhoWorkedOnThisLEsOtherCasesForDynamicRole(out string ADUserName, int OfferRoleTypeKey, int ApplicationKey)
        {
            ADUserName = string.Empty;
            StringBuilder sb = new StringBuilder();
            using (new SessionScope())
            {
                try
                {
                    sb.AppendLine("select ad.AdUserName,orr.OfferRoleTypeKey, ad.generalStatusKey ");
                    sb.AppendLine("from [TestDB_WTF]..OfferRole orr (nolock) ");
                    sb.AppendLine("join  ");
                    sb.AppendLine("( ");
                    sb.AppendLine("select distinct(orr1.offerkey) ");
                    sb.AppendLine("from [TestDB_WTF]..offerrole orr (nolock) ");
                    sb.AppendLine("join [TestDB_WTF]..offerrole orr1 (nolock) on orr.legalentitykey=orr1.legalentitykey  ");
                    sb.AppendLine("and orr1.offerroletypekey in (8,10,11,12) ");
                    sb.AppendFormat("where orr.offerkey={0} ", ApplicationKey);
                    sb.AppendLine(") OL--OfferList ");
                    sb.AppendLine("on OL.OfferKey=orr.OfferKey ");
                    sb.AppendLine("join [TestDB_WTF]..LegalEntity le (nolock) on orr.legalentitykey=le.legalentitykey ");
                    sb.AppendLine("join [TestDB_WTF]..AdUser ad (nolock) on le.legalentitykey=ad.legalentitykey and ad.GeneralStatusKey=1 ");
                    sb.AppendFormat("Where orr.OfferRoleTYpeKey={0} ", OfferRoleTypeKey);
                    sb.AppendLine("order by orr.OfferRoleKey ");
                    DataSet ds = Helpers.ExecuteQueryOnCastleTran(sb.ToString(), typeof(GeneralStatus_WTF_DAO));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ADUserName = ds.Tables[0].Rows[0]["AdUserName"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    LogPlugin.LogError("Unable to GetUserWhoWorkedOnThisLEsOtherCasesForDynamicRole() {0}{1}{2}",
                        sb.ToString(), Environment.NewLine, ex.ToString());
                }
            }
            return new X2ReturnData(HandleX2Messages(), ADUserName);
        }

        public IX2ReturnData WTFRoundRobinAssignForGivenOrgStructure(IActiveDataTransaction Tran, out string AssignedUser, string DynamicRole, int GenericKey, List<int> OrgStructureKeys)
        {
            AssignedUser = string.Empty;
            
            using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(Tran.CurrentTransaction))
            {
                using (Castle.ActiveRecord.TransactionScope ts = new Castle.ActiveRecord.TransactionScope())
                {
                    try
                    {
                        CleanDomainMessages();
                        int LEKey;
                        AssignedUser = GetASUserForAssignByRoleDescription(DynamicRole, Tran, out LEKey);

                        // create an offerrole 
                        DomainService.Repository.WTF.WorkflowRepository.Instance().CreateAndSaveApplicationRole_WTF_DAO(GenericKey, DynamicRole, LEKey);

                        ts.VoteCommit();
                        
                    }
                    catch (Exception ex)
                    {
                        ts.VoteRollBack();
                        LogPlugin.LogError("Unable to RoundRobinAssignForGivenOrgStructure() {0} {1}", DynamicRole, ex.ToString());
                    }
                }
                scope.Complete();
            }
            return new X2ReturnData(HandleX2Messages(), AssignedUser);
        }
        
        public IX2ReturnData WTFRoundRobinAssignForGivenOrgStructure(IActiveDataTransaction Tran, out string AssignedUser, string DynamicRole, int GenericKey, int OrgStructureKey)
        {
            List<int> OSKeys = new List<int>();
            OSKeys.Add(OrgStructureKey);
            return WTFRoundRobinAssignForGivenOrgStructure(Tran, out AssignedUser, DynamicRole, GenericKey, OSKeys);
        }

        protected string GetASUserForAssignByRoleDescription(string DynamicRole, IActiveDataTransaction Tran, out int LEKey)
        {
            string ADUser = String.Empty;
            string strSQL = String.Format(@"select TOP 1 adu.LegalEntityKey, adu.ADUserName
                        --* 
                        from OfferRoleType ort (nolock)
                        join OfferRoleTypeOrganisationStructureMapping ortOS (nolock) on ortOS.OfferRoleTypeKey = ort.OfferRoleTypeKey
                        join ADUSer adu (nolock) on ortOS.OfferRoleTypeKey = ort.OfferRoleTypeKey
                            and adu.GeneralStatusKey = 1
                        left join OfferRole ofr (nolock) on adu.LegalEntityKey = ofr.LegalEntityKey
                            and ofr.GeneralStatusKey = 1
                        where Description = '{0}'
                        Group by adu.LegalEntityKey, adu.ADUserName 
                        order by count(ofr.LegalEntityKey) asc", DynamicRole);

            DataSet ds = Helpers.ExecuteQueryOnCastleTran(strSQL, typeof(ADUser_WTF_DAO));

            //DataSet ds = Helpers.ExecuteQuery(strSQL, Tran);


            if (ds.Tables[0].Rows.Count > 0)
            {
                LEKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                return Convert.ToString(ds.Tables[0].Rows[0][1]); ;
            }

            LEKey = 0;
            return "";

        }
    }
}
