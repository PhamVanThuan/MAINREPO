using System;
using SAHL.X2.Framework.DataSets;

namespace SAHL.X2.Framework.Common
{
    public interface IX2InstanceData
    {
        string ActivityADUser { get; }

        string ActivityName { get; }

        DateTime CreationDate { get; }

        string CreatorADUserName { get; }

        DateTime DeadlineDate { get; set; }

        void Dispose();

        bool HasChanges { get; }

        long InstanceID { get; }

        string Name { get; set; }

        long ParentInstanceID { get; }

        int Priority { get; set; }

        int? ReturnActivityID { get; }

        dsX2InstanceData.X2InstanceDataDataTable GetDataToSave();

        //void SaveX2InstanceData(IActiveDataTransaction Tran);

        long? SourceInstanceID { get; }

        DateTime StateChangeDate { get; set; }

        int StateID { get; set; }

        string StateName { get; set; }

        string Subject { get; set; }

        string WorkFlowName { get; }

        string WorkFlowProvider { get; }
    }
}