using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;
using SAHL.Core.Data.Models.DecisionTree.Statements;
using System;
using System.Linq;

namespace SAHL.Services.DecisionTreeDesign.Managers.EnumerationSet
{
    public class EnumerationSetDataManager : IEnumerationSetDataManager
    {
        public bool IsEnumerationSetVersionPublished(int version)
        {
            IsEnumerationSetVersionPublishedQuery command = new IsEnumerationSetVersionPublishedQuery(version);
            bool published = false;
            using (var db = new Db().InReadOnlyAppContext())
            {
                published = db.Select<PublishedEnumerationSetDataModel>(command).Count() > 0;
                db.Complete();
            }
            return published;
        }

        public void InsertEnumerationSet(Guid id, int version, string data)
        {
            EnumerationSetDataModel command = new EnumerationSetDataModel(id, version, data);
            using (var db = new Db().InAppContext())
            {
                db.Insert<EnumerationSetDataModel>(command);
                db.Complete();
            }
        }

        public void UpdateEnumerationSet(Guid id, int version, string data)
        {
            UpdateEnumerationSetQuery command = new UpdateEnumerationSetQuery(id, version, data);
            using (var db = new Db().InAppContext())
            {
                db.Insert<EnumerationSetDataModel>(command);
                db.Complete();
            }
        }

        public void InsertPublishedEnumerationSet(Guid id, Guid enumerationSetId, Guid publishStatusId, DateTime publishDate, string publisher)
        {
            PublishedEnumerationSetDataModel command = new PublishedEnumerationSetDataModel(id, enumerationSetId, publishStatusId, publishDate, publisher);
            using (var db = new Db().InAppContext())
            {
                db.Insert<PublishedEnumerationSetDataModel>(command);
                db.Complete();
            }
        }

        public bool DoesEnumerationSetExist(Guid id)
        {
            DoesEnumerationSetExistQuery query = new DoesEnumerationSetExistQuery(id);
            bool exists = false;
            using (var db = new Db().InReadOnlyAppContext())
            {
                exists = db.Select<EnumerationSetDataModel>(query).Count() > 0;
                db.Complete();
            }
            return exists;
        }


        public EnumerationSetDataModel GetLatestEnumerationSet()
        {
            GetLatestEnumerationSetQuery query = new GetLatestEnumerationSetQuery();
            EnumerationSetDataModel model = null;
            using (var db = new Db().InReadOnlyAppContext())
            {
                model = db.SelectOne<EnumerationSetDataModel>(query);
                db.Complete();
            }
            return model;
        }
    }
}