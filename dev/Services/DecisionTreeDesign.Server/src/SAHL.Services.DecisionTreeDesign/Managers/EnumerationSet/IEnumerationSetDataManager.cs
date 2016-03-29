using SAHL.Core.Data.Models.DecisionTree;
using System;

namespace SAHL.Services.DecisionTreeDesign.Managers.EnumerationSet
{
    public interface IEnumerationSetDataManager
    {
        bool IsEnumerationSetVersionPublished(int version);

        void InsertEnumerationSet(Guid id, int version, string data);

        void UpdateEnumerationSet(Guid id, int version, string data);

        void InsertPublishedEnumerationSet(Guid id, Guid enumerationSetId, Guid publishStatusId, DateTime publishDate, string publisher);

        bool DoesEnumerationSetExist(Guid id);

        EnumerationSetDataModel GetLatestEnumerationSet();
    }
}