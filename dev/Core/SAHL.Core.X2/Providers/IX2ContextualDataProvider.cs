using System;
using System.Collections.Generic;

namespace SAHL.Core.X2.Providers
{
    public interface IX2ContextualDataProvider
    {
        void SetMapVariables(Dictionary<string, string> mapVariables);

        void InsertData(Int64 InstanceID, Dictionary<string, string> Fields);

        void LoadData(long instanceId);

        object GetDataField(string keyName);

        Dictionary<string, string> GetData();

        void SaveData(long InstanceID);
    }
}