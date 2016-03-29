using System;
using System.Collections;
using System.Data.SqlClient;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Xml;
using Castle.ActiveRecord.Framework.Internal;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.DataAccess;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.Service
{
    [FactoryType(typeof(IAuditsAndMetricsService), LifeTime = FactoryTypeLifeTime.Singleton)]
    public class AuditsAndMetricsService : IAuditsAndMetricsService
    {
        #region IAuditsAndMetricsService Members

        /// <summary>
        /// Stores audit information for a modified DAO object.
        /// </summary>
        /// <param name="daoEntity"></param>
        /// <param name="previousState"></param>
        /// <param name="currentState"></param>
        public void StoreAudit(object daoEntity, System.Collections.IDictionary previousState, System.Collections.IDictionary currentState)
        {
            // first get the activerecord model for this DAO type
            ActiveRecordModel Model = ActiveRecordModel.GetModel(daoEntity.GetType());

            PrimaryKeyModel pkm = Model.PrimaryKey;

            string tableName = "";

            while (pkm == null)
            {
                if (Model.Parent == null)
                    throw new Exception("Unable to determine primary key.");

                pkm = Model.Parent.PrimaryKey;
                if (pkm != null)
                    tableName = Model.Parent.ActiveRecordAtt.Table;
            }
            string id = pkm.Property.GetValue(daoEntity, null).ToString();

            // create a stram and xmltextwriter in order to produce the change set
            MemoryStream MS = new MemoryStream(1024);
            XmlTextWriter XTW = new XmlTextWriter(MS, Encoding.UTF8);

            if (tableName == "")
                tableName = Model.ActiveRecordAtt.Table;

            XTW.WriteStartDocument();
            XTW.WriteStartElement("Object"); // + Object

            XTW.WriteElementString("TableName", tableName); // +- TableName
            PrimaryKeyModel PK = Model.PrimaryKey;
            if (Model.IsDiscriminatorSubClass || Model.IsJoinedSubClass)
                PK = Model.Parent.PrimaryKey;
            XTW.WriteElementString("PrimaryKeyColumnName", PK.PrimaryKeyAtt.Column); // +- PrimaryKeyColumnName
            XTW.WriteElementString("PrimaryKeyValue", id); // +- PrimaryKeyValue

            XTW.WriteStartElement("PreviousValues"); // + PreviousValues

            // previousState is null during create
            if (previousState != null)
            {
                ICollection previousKeys = previousState.Keys;
                WriteProperties(XTW, previousKeys, previousState);
            }

            XTW.WriteEndElement();  // - PreviousValues

            XTW.WriteStartElement("CurrentValues"); // + CurrentValues

            // this can be null in the case of deletes
            if (currentState != null)
            {
                ICollection currentKeys = currentState.Keys;
                WriteProperties(XTW, currentKeys, currentState);
            }

            XTW.WriteEndElement();  // - CurrentValues

            XTW.WriteEndElement();  // - Object
            XTW.WriteEndDocument();

            XTW.Flush();

            // determine the form name - if we have an HTTP context then pull out the current running type,
            // and even better, if we can cast to an IView then get the view and presenter
            string formName = String.Empty;
            if (HttpContext.Current != null)
            {
                IView view = HttpContext.Current.Handler as IView;
                if (view != null)
                {
                    formName = "View: {0}, Presenter: {1}";

                    // add the presenter name
                    ObjectTypeSettings presenterSettings = UIPConfiguration.Config.GetPresenterSettings(view.ViewName);
                    formName = String.Format(formName, view.ViewName, presenterSettings.Name);
                }
                else
                {
                    // unable to determine UIP stuff - just use the current page type
                    formName = "Page: " + HttpContext.Current.Handler.GetType().FullName;
                }
            }

            // now write the xml packet to the audit table
            //IDbConnection con = Helper.GetSQLDBConnection("Warehouse");
            string query = @"INSERT INTO [Warehouse].[dbo].[Audits]
           ([AuditDate],[ApplicationName],[HostName],[WorkStationID],[WindowsLogon]
           ,[FormName],[TableName],[PrimaryKeyName],[PrimaryKeyValue],[AuditData])
     VALUES
           (@AuditDate,@ApplicationName,@HostName,@WorkStationID,@WindowsLogon
            ,@FormName,@TableName,@PrimaryKeyName,@PrimaryKeyValue,@AuditData)";

            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@AuditDate", DateTime.Now));
            parameters.Add(new SqlParameter("@ApplicationName", "AuditFrameWork"));
            parameters.Add(new SqlParameter("@HostName", Environment.MachineName));
            parameters.Add(new SqlParameter("@WorkStationID", ""));
            parameters.Add(new SqlParameter("@WindowsLogon", WindowsIdentity.GetCurrent().Name));
            parameters.Add(new SqlParameter("@FormName", formName));
            parameters.Add(new SqlParameter("@TableName", tableName));
            parameters.Add(new SqlParameter("@PrimaryKeyName", PK.PrimaryKeyAtt.Column));
            parameters.Add(new SqlParameter("@PrimaryKeyValue", id));

            MS.Seek(0, SeekOrigin.Begin);
            StreamReader SR = new StreamReader(MS);
            parameters.Add(new SqlParameter("@AuditData", SR.ReadToEnd()));

            CastleTransactionsServiceHelper.ExecuteNonQueryOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);

            MS.SetLength(0);
        }

        private IApplicationRepository Repository(object cmd, Type type, ParameterCollection parameters)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        private void IApplicationRepository(object cmd, Type type, ParameterCollection parameters)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        private void WriteProperties(XmlTextWriter XTW, ICollection Keys, IDictionary data)
        {
            foreach (object Key in Keys)
            {
                if (data[Key] != null)
                {
                    GetPropertyString(data[Key], Key, XTW);
                }
            }
        }

        private void GetPropertyString(object dataValue, object Key, XmlTextWriter XTW)
        {
            Type Value = dataValue.GetType();
            if (Value.IsValueType || Value.Name == "String")
            {
                XTW.WriteElementString(Key.ToString(), dataValue.ToString());
            }
            else
            {
                if (Value.IsSubclassOf(typeof(Castle.ActiveRecord.ActiveRecordBase)))
                {
                    ActiveRecordModel M = ActiveRecordModel.GetModel(Value);

                    PrimaryKeyModel PK = M.PrimaryKey;
                    if (M.IsDiscriminatorSubClass || M.IsJoinedSubClass)
                        PK = M.Parent.PrimaryKey;

                    object KeyValue = PK.Property.GetValue(dataValue, null);
                    XTW.WriteElementString(Key.ToString(), KeyValue.ToString());
                }

                //NOTE: DO NOT LOG COLLECTIONS - THESE THROW AN ERROR WHEN WRAPPED IN A TRANSACTION - MIGHT
                // BE SOMETHING TO DO WITH THE NHIBERNATE COLLECTION??
                //else
                //    if (Value.GetInterface(typeof(IEnumerable).ToString()) != null)
                //    {
                //        XTW.WriteStartElement(Key.ToString()); // + Items
                //        IEnumerable En = dataValue as IEnumerable;
                //        foreach(object o in En)
                //        {
                //            GetPropertyString(o, "Item", XTW);
                //        }
                //        XTW.WriteEndElement(); // - Items
                //    }
            }
        }

        #endregion IAuditsAndMetricsService Members
    }
}