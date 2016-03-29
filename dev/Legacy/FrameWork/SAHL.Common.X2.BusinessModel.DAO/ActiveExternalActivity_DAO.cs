using System;
using Castle.ActiveRecord;
using SAHL.Common.X2.BusinessModel.DAO.Database;

namespace SAHL.Common.X2.BusinessModel.DAO
{
    [ActiveRecord("ActiveExternalActivity", Schema = "X2")]
    public partial class ActiveExternalActivity_DAO : DB_X2<ActiveExternalActivity_DAO>
    {
        private int _workFlowID;

        private Int64? _activatingInstanceID;

        private System.DateTime _activationTime;

        private string _activityXMLData;

        private string _workFlowProviderName;

        private int _iD;

        private ExternalActivity_DAO _externalActivity;

        [Property("WorkFlowID", ColumnType = "Int32", NotNull = true)]
        public virtual int WorkFlowID
        {
            get
            {
                return this._workFlowID;
            }
            set
            {
                this._workFlowID = value;
            }
        }

        [Property("ActivatingInstanceID")]
        public virtual Int64? ActivatingInstanceID
        {
            get
            {
                return this._activatingInstanceID;
            }
            set
            {
                this._activatingInstanceID = value;
            }
        }

        [Property("ActivationTime", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime ActivationTime
        {
            get
            {
                return this._activationTime;
            }
            set
            {
                this._activationTime = value;
            }
        }

        [Property("ActivityXMLData", ColumnType = "StringClob")]
        public virtual string ActivityXMLData
        {
            get
            {
                return this._activityXMLData;
            }
            set
            {
                this._activityXMLData = value;
            }
        }

        [Property("WorkFlowProviderName", ColumnType = "String")]
        public virtual string WorkFlowProviderName
        {
            get
            {
                return this._workFlowProviderName;
            }
            set
            {
                this._workFlowProviderName = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "ID", ColumnType = "Int32")]
        public virtual int ID
        {
            get
            {
                return this._iD;
            }
            set
            {
                this._iD = value;
            }
        }

        [BelongsTo("ExternalActivityID", NotNull = true)]
        public virtual ExternalActivity_DAO ExternalActivity
        {
            get
            {
                return this._externalActivity;
            }
            set
            {
                this._externalActivity = value;
            }
        }
    }
}