using System.Collections.Generic;

namespace SAHL.X2.Framework.Common
{
    public interface IX2WorkFlowDataProvider
    {
        /*
        void LoadData(IActiveDataTransaction Tran, Int64 InstanceID);

        void SaveData(IActiveDataTransaction Tran, Int64 InstanceID);

        void InsertData(IActiveDataTransaction Tran, Int64 InstanceID, Dictionary<string, string> Fields);

        */

        void SetDataFields(Dictionary<string, string> Fields);

        Dictionary<string, string> GetDataFields();

        void SetDataField(string name, object value);

        object GetDataField(string name);

        Dictionary<string, string> DataFields { get; }

        string DataProviderName { get; }

        bool HasChanges { get; }

        bool Contains(string FieldName);

        //string GetField(string FieldName);
    }
}