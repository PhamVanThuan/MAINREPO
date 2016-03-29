using System;
using System.Collections.Generic;

namespace SAHL.Services.DecisionTreeDesign.Managers.MessageSet
{
    public interface IMessageSetManager
    {
        void SaveMessageSet(Guid id, int version, string data);

        void SaveAndPublishMessageSet(Guid id, int version, string data, string publisher);

        IEnumerable<string> GetLatestMessageSetWords();
    }
}