using SAHL.Core.Data.Models.DecisionTree;
using System;

namespace SAHL.Services.DecisionTreeDesign.Managers.Variable
{
    public interface IVariableDataManager
    {
        void SaveVariableSet(Guid id, int version, string data);

        void UpdateVariableSet(Guid id, int version, string data);

        bool IsVariableSetVersionPublished(int version);

        void InsertPublishedVariableSet(Guid id, Guid variableSetID, Guid statusID, DateTime publishedDate, string publisher);

        bool DoesVariableSetExist(Guid id);

        VariableSetDataModel GetLatestVariableSet();
    }
}