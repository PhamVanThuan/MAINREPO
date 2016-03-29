using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Permissions;
using X2DomainService.Interface.X2EngineTest;
using SAHL.X2.Common.DataAccess;
using SAHL.X2.Common;
using SAHL.X2.Framework.Common;
using System.Data;
using SAHL.Common.BusinessModel.DAO;


namespace DomainService.Workflow
{
    [Serializable]
    [ServiceAttribute("X2EngineTest", WorkflowPorts.WorkflowPort, typeof(IX2EngineTest), typeof(Workflow.X2EngineTest))]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class X2EngineTest : WorkflowBase, IX2EngineTest
    {
        public X2EngineTest(): base()
        {
        }

        protected override SAHL.Common.BusinessModel.DAO.ApplicationRoleType_DAO GetApplicationRoleType(SAHL.Common.BusinessModel.DAO.ADUser_DAO user)
        {
            throw new Exception("The method or operation is not implemented.");
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
                            and adu.LegalEntityKey is not null
							and adu.ADUserName not like 'RCS%'
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
