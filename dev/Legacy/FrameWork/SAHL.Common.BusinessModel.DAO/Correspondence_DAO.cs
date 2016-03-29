using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Correspondence", Schema = "dbo", Lazy = true)]
    public partial class Correspondence_DAO : DB_2AM<Correspondence_DAO>
    {
        private int _genericKey;

        private string _destinationValue;

        private System.DateTime? _dueDate;

        private System.DateTime? _completedDate;

        private string _userID;

        private System.DateTime _changeDate;

        private string _outputFile;

        private int _Key;

        private CorrespondenceMedium_DAO _correspondenceMedium;

        private GenericKeyType_DAO _genericKeyType;

        private ReportStatement_DAO _reportStatement;

        private IList<CorrespondenceParameters_DAO> _correspondenceParameters;

        private LegalEntity_DAO _legalEntity;

        private CorrespondenceDetail_DAO _correspondenceDetail;

        [Property("GenericKey", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Generic Key is a mandatory field")]
        public virtual int GenericKey
        {
            get
            {
                return this._genericKey;
            }
            set
            {
                this._genericKey = value;
            }
        }

        [Property("DestinationValue", ColumnType = "String")]
        public virtual string DestinationValue
        {
            get
            {
                return this._destinationValue;
            }
            set
            {
                this._destinationValue = value;
            }
        }

        [Property("DueDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? DueDate
        {
            get
            {
                return this._dueDate;
            }
            set
            {
                this._dueDate = value;
            }
        }

        [Property("CompletedDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? CompletedDate
        {
            get
            {
                return this._completedDate;
            }
            set
            {
                this._completedDate = value;
            }
        }

        [Property("UserID", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("User ID is a mandatory field")]
        public virtual string UserID
        {
            get
            {
                return this._userID;
            }
            set
            {
                this._userID = value;
            }
        }

        [Property("ChangeDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Change Date is a mandatory field")]
        public virtual System.DateTime ChangeDate
        {
            get
            {
                return this._changeDate;
            }
            set
            {
                this._changeDate = value;
            }
        }

        [Property("OutputFile", ColumnType = "String")]
        public virtual string OutputFile
        {
            get
            {
                return this._outputFile;
            }
            set
            {
                this._outputFile = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "CorrespondenceKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._Key;
            }
            set
            {
                this._Key = value;
            }
        }

        [BelongsTo("CorrespondenceMediumKey", NotNull = true)]
        [ValidateNonEmpty("Correspondence Medium is a mandatory field")]
        public virtual CorrespondenceMedium_DAO CorrespondenceMedium
        {
            get
            {
                return this._correspondenceMedium;
            }
            set
            {
                this._correspondenceMedium = value;
            }
        }

        [BelongsTo("GenericKeyTypeKey", NotNull = true)]
        [ValidateNonEmpty("Generic Key Type is a mandatory field")]
        public virtual GenericKeyType_DAO GenericKeyType
        {
            get
            {
                return this._genericKeyType;
            }
            set
            {
                this._genericKeyType = value;
            }
        }

        [BelongsTo("ReportStatementKey", NotNull = true)]
        [ValidateNonEmpty("Report Statement is a mandatory field")]
        public virtual ReportStatement_DAO ReportStatement
        {
            get
            {
                return this._reportStatement;
            }
            set
            {
                this._reportStatement = value;
            }
        }

        [HasMany(typeof(CorrespondenceParameters_DAO), ColumnKey = "CorrespondenceKey", Lazy = true, Table = "CorrespondenceParameters", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<CorrespondenceParameters_DAO> CorrespondenceParameters
        {
            get
            {
                return this._correspondenceParameters;
            }
            set
            {
                this._correspondenceParameters = value;
            }
        }

        [BelongsTo("LegalEntityKey")]
        public virtual LegalEntity_DAO LegalEntity
        {
            get
            {
                return this._legalEntity;
            }
            set
            {
                this._legalEntity = value;
            }
        }

        [OneToOne(Cascade = CascadeEnum.All)]
        public virtual CorrespondenceDetail_DAO CorrespondenceDetail
        {
            get
            {
                return this._correspondenceDetail;
            }
            set
            {
                this._correspondenceDetail = value;
            }
        }
    }
}