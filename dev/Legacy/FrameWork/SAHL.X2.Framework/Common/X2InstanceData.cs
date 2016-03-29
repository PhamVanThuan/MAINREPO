using System;
using SAHL.Common.Logging;
using SAHL.X2.Framework.DataSets;
using SAHL.X2.Framework.Logging;
using SAHL.Shared.Extensions;

namespace SAHL.X2.Framework.Common
{
    [Serializable]
    public class X2InstanceData : MarshalByRefObject, IX2InstanceData
    {
        dsX2InstanceData.X2InstanceDataRow m_InstanceData;
        dsX2InstanceData.X2InstanceDataDataTable m_DT = null;
        private bool m_HasChanges = false;

        public X2InstanceData(dsX2InstanceData.X2InstanceDataDataTable dataTable)
        {
            m_DT = dataTable;

            if (m_DT.Count == 1)
                m_InstanceData = m_DT[0];
			try
			{
				Logger.ThreadContext.AddOrUpdateValue(X2Logging.INSTANCEID, SourceInstanceID);
				Logger.ThreadContext.AddOrUpdateValue(X2Logging.WORKFLOWNAME, WorkFlowName);
				Logger.ThreadContext.AddOrUpdateValue(X2Logging.WORKFLOWACTIVITY, ActivityName);
				Logger.ThreadContext.AddOrUpdateValue(X2Logging.WORKFLOWSTATE, StateName);
			}
			catch (Exception ex)
			{

			}
        }

        public dsX2InstanceData.X2InstanceDataDataTable GetDataToSave()
        {
            return this.m_DT;
        }

        public void Dispose()
        {
            if (m_DT != null)
                m_DT.Dispose();
        }

        #region Properties

        public bool HasChanges
        {
            get { return m_HasChanges; }
        }

        public Int64 InstanceID
        {
            get
            {
                return m_InstanceData.ID;
            }
        }

        public Int64 ParentInstanceID
        {
            get
            {
                return m_InstanceData.IsParentInstanceIDNull() ? -1 : m_InstanceData.ParentInstanceID;
            }
        }

        public Int64? SourceInstanceID
        {
            get
            {
                if (m_InstanceData.IsSourceInstanceIDNull())
                    return null;
                return m_InstanceData.SourceInstanceID;
                //return m_InstanceData.IsSourceInstanceIDNull() ? null : m_InstanceData.SourceInstanceID;
            }
        }

        public Int32? ReturnActivityID
        {
            get
            {
                if (m_InstanceData.IsReturnActivityIDNull())
                    return null;
                return m_InstanceData.ReturnActivityID;
                //return m_InstanceData.IsReturnActivityIDNull() ? null : m_InstanceData.ReturnActivityID;
            }
        }

        public string WorkFlowName
        {
            get
            {
                return m_InstanceData.WorkFlowName;
            }
        }

        public string CreatorADUserName
        {
            get
            {
                return m_InstanceData.CreatorADUserName;
            }
        }

        public DateTime CreationDate
        {
            get
            {
                return m_InstanceData.CreationDate;
            }
        }

        public string ActivityName
        {
            get
            {
                if (m_InstanceData.IsActivityNameNull())
                    return string.Empty;
                else
                    return m_InstanceData.ActivityName;
            }
        }

        public string ActivityADUser
        {
            get
            {
                if (m_InstanceData.IsActivityADUserNameNull())
                    return "";
                else
                    return m_InstanceData.ActivityADUserName;
            }
        }

        public string WorkFlowProvider
        {
            get
            {
                return m_InstanceData.WorkFlowProvider;
            }
        }

        // changeable properties

        public string Name
        {
            get
            {
                return m_InstanceData.Name;
            }
            set
            {
                m_InstanceData.Name = value;
            }
        }

        public string Subject
        {
            get
            {
                return m_InstanceData.IsSubjectNull() ? "" : m_InstanceData.Subject;
            }
            set
            {
                m_InstanceData.Subject = value;
            }
        }

        public int Priority
        {
            get
            {
                return m_InstanceData.IsPriorityNull() ? 0 : m_InstanceData.Priority;
            }
            set
            {
                m_InstanceData.Priority = value;
            }
        }

        public int StateID
        {
            get
            {
                if (m_InstanceData.IsStateIDNull())
                    return -1;
                else
                    return m_InstanceData.StateID;
            }
            set
            {
                m_InstanceData.StateID = value;
            }
        }

        public string StateName
        {
            get
            {
                if (m_InstanceData.IsStateNameNull())
                    return "";
                else
                    return m_InstanceData.StateName;
            }
            set
            {
                m_InstanceData.StateName = value;
            }
        }

        public DateTime StateChangeDate
        {
            get
            {
                if (m_InstanceData.IsStateChangeDateNull())
                    return DateTime.MinValue;
                else
                    return m_InstanceData.StateChangeDate;
            }
            set
            {
                m_InstanceData.StateChangeDate = value;
            }
        }

        public DateTime DeadlineDate
        {
            get
            {
                return m_InstanceData.IsDeadlineDateNull() ? DateTime.MinValue : m_InstanceData.DeadlineDate;
            }
            set
            {
                m_InstanceData.DeadlineDate = value;
            }
        }

        #endregion Properties
    }
}