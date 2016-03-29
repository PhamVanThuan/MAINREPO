using SAHL.Core.Data;
using SAHL.Core.Data.Models.DecisionTree;
using SAHL.Core.Data.Models.DecisionTree.Statements;
using SAHL.Services.DecisionTreeDesign.Managers.Variable.Statements;
using System;
using System.Linq;

namespace SAHL.Services.DecisionTreeDesign.Managers.Variable
{
    public class VariableDataManager : IVariableDataManager
    {
        public void SaveVariableSet(Guid id, int version, string data)
        {
            VariableSetDataModel variableSet = new VariableSetDataModel(id, version, data);
            using (var db = new Db().InAppContext())
            {
                db.Insert<VariableSetDataModel>(variableSet);
                db.Complete();
            }
        }

        public void UpdateVariableSet(Guid id, int version, string data)
        {
            UpdateVariableSetQuery command = new UpdateVariableSetQuery(id, version, data);
            using (var db = new Db().InAppContext())
            {
                db.Insert<VariableSetDataModel>(command);
                db.Complete();
            }
        }

        public bool IsVariableSetVersionPublished(int version)
        {
            IsVariableSetVersionPublishedQuery command = new IsVariableSetVersionPublishedQuery(version);
            bool published = false;
            using (var db = new Db().InReadOnlyAppContext())
            {
                published = db.Select<PublishedVariableSetDataModel>(command).Count() > 0;
                db.Complete();
            }
            return published;
        }

        public void InsertPublishedVariableSet(Guid id, Guid variableSetID, Guid publishedStatusID, DateTime publishedDate, string publisher)
        {
            PublishedVariableSetDataModel publishedSet = new PublishedVariableSetDataModel(id, variableSetID, publishedStatusID, publishedDate, publisher);
            using (var db = new Db().InAppContext())
            {
                db.Insert(publishedSet);
                db.Complete();
            }
        }

        public bool DoesVariableSetExist(Guid id)
        {
            DoesVariableSetExistQuery query = new DoesVariableSetExistQuery(id);
            bool exists = false;
            using (var db = new Db().InReadOnlyAppContext())
            {
                exists = db.Select<VariableSetDataModel>(query).Count() > 0;
                db.Complete();
            }
            return exists;
        }

        public VariableSetDataModel GetLatestVariableSet()
        {
            GetLatestVariableSetQuery query = new GetLatestVariableSetQuery();
            VariableSetDataModel model = null;
            using (var db = new Db().InReadOnlyAppContext())
            {
                model = db.SelectOne<VariableSetDataModel>(query);
                db.Complete();
            }
            return model;
        }
    }
}