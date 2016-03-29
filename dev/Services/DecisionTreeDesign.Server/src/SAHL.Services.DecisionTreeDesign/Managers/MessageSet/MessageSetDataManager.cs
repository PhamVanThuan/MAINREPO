using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;
using SAHL.Core.Data.Models.DecisionTree.Statements;
using SAHL.Services.DecisionTreeDesign.Managers.MessageSet.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.DecisionTreeDesign.Managers.MessageSet
{
    public class MessageSetDataManager : IMessageSetDataManager
    {
        public void InsertMessageSet(Guid id, int version, string data)
        {
            MessageSetDataModel messageSetModel = new MessageSetDataModel(id, version, data);
            using (var db = new Db().InAppContext())
            {
                db.Insert<MessageSetDataModel>(messageSetModel);
                db.Complete();
            }
        }

        public void UpdateMessageSet(Guid id, int version, string data)
        {
            MessageSetDataModel messageSetModel = new MessageSetDataModel(id, version, data);
            using (var db = new Db().InAppContext())
            {
                db.Update<MessageSetDataModel>(messageSetModel);
                db.Complete();
            }
        }

        public bool IsMessageSetVersionPublished(int version)
        {
            IsMessageSetVersionPublishedQuery query = new IsMessageSetVersionPublishedQuery(version);
            bool published = false;
            using (var db = new Db().InReadOnlyAppContext())
            {
                published = db.Select<PublishedMessageSetDataModel>(query).Count() > 0;
                db.Complete();
            }
            return published;
        }


        public void InsertPublishedMessageSet(Guid id, Guid messageSetId, Guid publishStatusId, DateTime publishDate, string publisher)
        {
            PublishedMessageSetDataModel publishedMessageSetModel = new PublishedMessageSetDataModel(id, messageSetId, publishStatusId, publishDate, publisher);
            using (var db = new Db().InAppContext())
            {
                db.Insert<PublishedMessageSetDataModel>(publishedMessageSetModel);
                db.Complete();
            }
        }

        public bool DoesMessageSetExist(Guid id)
        {
            DoesMessageSetExistsQuery query = new DoesMessageSetExistsQuery(id);
            bool exists = false;
            using (var db = new Db().InReadOnlyAppContext())
            {
                exists = db.Select<MessageSetDataModel>(query).Count() > 0;
                db.Complete();
            }
            return exists;
        }


        public MessageSetDataModel GetLatestMessageSet()
        {
            GetLatestMessageSetQuery query = new GetLatestMessageSetQuery();
            MessageSetDataModel model = null;
            using (var db = new Db().InReadOnlyAppContext())
            {
                model = db.SelectOne<MessageSetDataModel>(query);
                db.Complete();
            }
            return model;
        }
    }
}
