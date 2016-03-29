using System;
using System.Collections.Generic;

namespace SAHL.Services.DecisionTreeDesign.Managers.EnumerationSet
{
    public interface IEnumerationSetManager
    {
        void SaveEnumerationSet(Guid id, int version, string data);

        void SaveAndPublishEnumerationSet(Guid id, int version, string data, string publisher);

        IEnumerable<string> GetLatestEnumerationSetWords();
    }
}