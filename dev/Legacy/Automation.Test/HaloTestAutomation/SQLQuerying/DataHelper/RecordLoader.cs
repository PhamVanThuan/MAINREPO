using Automation.DataModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Automation.DataAccess.Interfaces;
using System.Threading;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        public T Load<T>(int primaryKey)
           where T : Record
        {
            var record = default(T);
            while (record == null)
            {
                record = this.Load<T>()
                    .Where(x=>x.Key==primaryKey)
                    .FirstOrDefault();
            }
            return record;
        }

        public IEnumerable<T> Load<T>()
              where T : Record
        {
            T info = (T)Activator.CreateInstance<T>();
            var currentKeys = new int[0];
            var tableName = info.Table;
            var schemaName = info.Schema;
            var dbName = info.DB;
            var primaryKeyName =
                this.dataContext.Query<string>(String.Format(@"declare @column_name varchar(max)
                       SELECT column_name
                       FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
                       WHERE 
                       OBJECTPROPERTY(OBJECT_ID(constraint_name), 'IsPrimaryKey') = 1
                       AND table_name = '{0}'", info.Table)).FirstOrDefault();
            currentKeys = this.dataContext.Query<int>(String.Format(@"select {0} from [{1}].[{2}].[{3}]"
                                    , primaryKeyName, dbName, schemaName, tableName)).ToArray();
            var noKeys = currentKeys.Count();
            currentKeys.OrderBy(x => x).ToArray();
            var records = new Record[noKeys];
            var indices = new Dictionary<int, int[]>();
            for (int i = 0; i < noKeys; i += 400)
            {
                var min = currentKeys[i];
                var max = currentKeys[i+ 400] ;
                var take = currentKeys.Where(x => x > min && x < max).ToArray();
                indices.Add(i, take);
            }
            currentKeys = null;
            Func<T[]> get100Records = () =>
            {
                var keysToQuery = new int[0];
                var index = -1;
                lock (indices)
                {
                    index = indices.Keys.FirstOrDefault();
                    indices.Remove(index);
                    if (indices.Count == 0)
                        return null;
                }
                var keyStr = String.Join(",", indices[index]);
                var results = this.dataContext.Query<T>(
                    String.Format(@"select [{3}] as [Key],* from [{0}].[{1}].[{2}] where [{3}] in ({4})"
                    , dbName, schemaName, tableName, primaryKeyName, keyStr)).ToArray();
                return results;
            };

            var source = get100Records.Invoke();
            Array.Copy(source, 0, records, 0, source.Length);
            var c = 0;
            var taskCount = 0;
            while (c < records.Length)
            {
                if (taskCount < 20)
                {
                    Task.Run(() =>
                    {
                        taskCount++;
                        source = get100Records.Invoke();
                        if (source == null)
                            return;
                        var index = c;
                        while (records[index] != null)
                            index++;
                        Array.Copy(source, 0, records, index, source.Length);
                        var len = c - 120;
                        for (int x = 0; x < len; x++)
                        {
                            records[x] = null;
                        }
                    });
                }
                if (records[c] != null)
                {
                    yield return (T)records[c];
                    c++;
                }
            }
        }

        public void Save<T>(T obj)
            where T : Record
        {

            var properties = typeof(T).GetProperties().Where(x => x.Name != "Id").ToArray();
            var propertiesToSet = String.Format("set {0}={1}", String.Join("={0},", properties.Select(x => x.Name)), "{0}");


            var propertiesToSetSplit = propertiesToSet.Split(',');
            for (int i = 0; i < properties.Count(); i++)
            {
                var val = properties[i].GetValue(obj, null);
                if (val.GetType() == typeof(String))
                    propertiesToSetSplit[i] = propertiesToSetSplit[i].Replace("{0}", String.Format("'{0}'", properties[i].GetValue(obj, null).ToString()));
                else if (val.GetType() == typeof(Boolean))
                {
                    if (properties[i].GetValue(obj, null).ToString() == "True")
                        propertiesToSetSplit[i] = propertiesToSetSplit[i].Replace("{0}", "1");
                    else
                        propertiesToSetSplit[i] = propertiesToSetSplit[i].Replace("{0}", "0");
                }
                else
                    propertiesToSetSplit[i] = propertiesToSetSplit[i].Replace("{0}", properties[i].GetValue(obj, null).ToString());
            }
            propertiesToSet = String.Join(",", propertiesToSetSplit);
            var updateQuery = String.Format(@"update [2am].test.{0} {1} where Id = {2}",typeof(T).Name, propertiesToSet, obj.Key);
            dataContext.Execute(updateQuery);
        }
    }
}
