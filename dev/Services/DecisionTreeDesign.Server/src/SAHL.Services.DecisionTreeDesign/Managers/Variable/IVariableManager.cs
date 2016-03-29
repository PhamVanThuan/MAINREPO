using System;
using System.Collections.Generic;

namespace SAHL.Services.DecisionTreeDesign.Managers.Variable
{
    public interface IVariableManager
    {
        void SaveVariableSet(Guid id, int version, string data);

        void SaveAndPublishVariableSet(Guid id, int version, string data, string publisher);

        IEnumerable<string> GetLatestVariableSetWords();
    }
}