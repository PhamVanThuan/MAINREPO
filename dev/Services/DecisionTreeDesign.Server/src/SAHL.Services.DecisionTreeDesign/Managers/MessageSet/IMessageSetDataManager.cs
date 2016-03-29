using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.DecisionTreeDesign.Managers.MessageSet
{
    public interface IMessageSetDataManager
    {
        void InsertMessageSet(Guid id, int version, string data);
        void UpdateMessageSet(Guid id, int version, string data);
        void InsertPublishedMessageSet(Guid id, Guid messageSetId, Guid publishStatusId, DateTime publishDate, string publisher);
        bool IsMessageSetVersionPublished(int version);
        bool DoesMessageSetExist(Guid id);

        Core.Data.Models.DecisionTree.MessageSetDataModel GetLatestMessageSet();
    }
}
