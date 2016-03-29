using System;
using System.Collections.Generic;
using System.Text;
using X2DomainService.Interface.Origination;
using System.Data;
using SAHL.X2.Common;
using System.Security;
using System.Security.Permissions;
using SAHL.X2.Common.DataAccess;
using SAHL.Common.X2.BusinessModel.DAO;
using SAHL.X2.Framework.Common;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO;

namespace DomainService.Workflow
{
    [Serializable]
    [ServiceAttribute("Messages", WorkflowPorts.WorkflowPort, typeof(IStageAndActivityMessages), typeof(Workflow.StageAndActivityMessage))]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class StageAndActivityMessage : WorkflowBase, IStageAndActivityMessages
    {
        public IX2ReturnData ReturnToProcessor(Int64 InstanceID, out int WorkflowHistoryID)
        {
            string User = string.Empty;
            WorkflowHistoryID = -1;
            using (new SessionScope())
            {
                try
                {
                    // Check where the case came from and decide which message to use.
                    // 1: Check to see if it came from credit.
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("declare @PIID bigint ");
                    sb.AppendFormat("set @PIID = (select sourceinstanceid from x2.instance (nolock) where id={0}) ", InstanceID);
                    sb.AppendLine("select i.id, i.name, wfh.ADUserName, a.Name, s.Name, wfh.id WFH ");
                    sb.AppendLine("from ");
                    sb.AppendLine("x2.instance i (nolock)  ");
                    sb.AppendLine("join x2.workflowhistory wfh (nolock) on i.id=wfh.instanceid  ");
                    sb.AppendLine("join x2.activity a (nolock) on wfh.activityid=a.id  ");
                    sb.AppendLine("join x2.state s (nolock) on wfh.stateid=s.id  ");
                    sb.AppendLine("where a.name in ('Rework Application','FL Rework Application') and wfh.id > ");
                    sb.AppendLine("( ");
                    sb.AppendLine("select max(wfh.id)--, a.name, i.sourceinstanceid ");
                    sb.AppendLine("from ");
                    sb.AppendLine("x2.instance i (nolock)  ");
                    sb.AppendLine("join x2.workflowhistory wfh (nolock) on i.id=wfh.instanceid  ");
                    sb.AppendLine("join x2.activity a (nolock) on wfh.activityid=a.id  ");
                    sb.AppendLine("join x2.state s (nolock) on wfh.stateid=s.id  ");
                    sb.AppendLine("where i.id=@PIID ");
                    sb.AppendLine("and a.name='Return Processor Credit'  ");
                    sb.AppendLine(") ");
                    sb.AppendLine("and wfh.instanceid=@PIID ");
                    sb.AppendLine("order by wfh.id desc ");
                    DataSet ds = Helpers.ExecuteQueryOnCastleTran(sb.ToString(), typeof(Instance_DAO));

                    if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        User = ds.Tables[0].Rows[0]["ADUserName"].ToString();
                        WorkflowHistoryID = Convert.ToInt32(ds.Tables[0].Rows[0]["WFH"]);
                    }
                }
                catch (Exception ex)
                {
                    LogPlugin.LogError("Unable to StageAndActivityMessage.ReturnToProcessor() {0} IID:{1}", ex.ToString(), InstanceID);
                }
            }
            return new X2ReturnData(HandleX2Messages(), User);
        }

        public IX2ReturnData DeclinedByCredit(Int64 InstanceID, string CallingMap, out int WorkflowHistoryID)
        {
            string User = string.Empty;
            WorkflowHistoryID = -1;
            using (new SessionScope())
            {
                try
                {
                    // to be called at the review decision state and then once it gets to an archive bin that ISNT approved 
                    // where the case is returned to origination call this same method as well
                    StringBuilder sb = new StringBuilder();
                    switch (CallingMap.ToUpper())
                    {
                        case "ORIGINATION":
                            {
                                sb.AppendFormat("select a.Name + ' by: ' + wfh.ADUserName, wfh.ID WFH ");
                                sb.AppendFormat("from  ");
                                sb.AppendFormat("x2.instance i (nolock) ");
                                sb.AppendFormat("join x2.workflowhistory wfh (nolock) on i.id=wfh.instanceid ");
                                sb.AppendFormat("join x2.activity a (nolock) on wfh.activityid=a.id ");
                                sb.AppendFormat("where a.name in ('Approve with Pricing Changes','Decline with Offer', 'Dispute Indicated','Request Further Info', 'Decline Application')  ");
                                sb.AppendFormat("and i.sourceinstanceid={0} ", InstanceID);
                                sb.AppendFormat("order by i.id desc ");
                                break;
                            }
                        case "CREDIT":
                            {
                                sb.AppendFormat("select a.Name + ' by: ' + wfh.ADUserName, wfh.ID WFH ");
                                sb.AppendFormat("from  ");
                                sb.AppendFormat("x2.instance i (nolock) ");
                                sb.AppendFormat("join x2.workflowhistory wfh (nolock) on i.id=wfh.instanceid ");
                                sb.AppendFormat("join x2.activity a (nolock) on wfh.activityid=a.id ");
                                sb.AppendFormat("where a.name in ('Approve with Pricing Changes','Decline with Offer', 'Dispute Indicated','Request Further Info', 'Decline Application')  ");
                                sb.AppendFormat("and i.id={0} ", InstanceID);
                                sb.AppendFormat("order by i.id desc ");
                                break;
                            }
                        default:
                            {
                                return new X2ReturnData(HandleX2Messages(), User);
                            }
                    }
                    DataSet ds = Helpers.ExecuteQueryOnCastleTran(sb.ToString(), typeof(Instance_DAO));
                    if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        User = ds.Tables[0].Rows[0][0].ToString();
                        WorkflowHistoryID = Convert.ToInt32(ds.Tables[0].Rows[0]["WFH"]);
                    }
                }
                catch (Exception ex)
                {
                    LogPlugin.LogError("Unable to StageAndActivityMessage.DeclinedByCredit() {0} IID:{1}", ex.ToString(), InstanceID);
                }
            }
            return new X2ReturnData(HandleX2Messages(), User);
        }

        public IX2ReturnData IsMotivate(Int64 InstanceID, out int WorkflowHistoryID)
        {
            string User = string.Empty;
            WorkflowHistoryID = -1;
            using (SessionScope s = new SessionScope())
            {
                try
                {
                    GeneralStatus_DAO g = GeneralStatus_DAO.Find(1);
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("select  ");
                    sb.AppendFormat("i.id, i.sourceinstanceid, iSource.ID, a.Name, wfh.adusername, wfh.ID WFH ");
                    sb.AppendFormat("from x2.instance i (nolock)  ");
                    sb.AppendFormat("join x2.instance iSource (nolock) on i.sourceinstanceid=iSource.id ");
                    sb.AppendFormat("join x2.workflowhistory wfh (nolock) on iSource.id=wfh.instanceid ");
                    sb.AppendFormat("join x2.activity a (nolock) on wfh.activityid=a.id ");
                    sb.AppendFormat("where a.name='motivate' ");
                    sb.AppendFormat("and i.id={0} order by wfh.id desc", InstanceID);
                    DataSet ds = Helpers.ExecuteQueryOnCastleTran(sb.ToString(), typeof(Instance_DAO));
                    if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        User = ds.Tables[0].Rows[0]["adusername"].ToString();
                        WorkflowHistoryID = Convert.ToInt32(ds.Tables[0].Rows[0]["WFH"]);
                    }
                }
                catch (Exception ex)
                {
                    LogPlugin.LogError("Unable to StageAndActivityMessage.IsMotivate({0}){1}{2}", InstanceID, Environment.NewLine, ex.ToString());
                }
            }
            return new X2ReturnData(HandleX2Messages(), User);

        }

        protected override SAHL.Common.BusinessModel.DAO.ApplicationRoleType_DAO GetApplicationRoleType(SAHL.Common.BusinessModel.DAO.ADUser_DAO user)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
