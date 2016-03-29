using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ClientQuestionnaire", Schema = "Survey")]
    [ConstructorInjector]
    public partial class ClientQuestionnaire_DAO : DB_2AM<ClientQuestionnaire_DAO>
    {
        private int _key;

        private BusinessEventQuestionnaire_DAO _businessEventQuestionnaire;

        private System.DateTime _datePresented;

        private ADUser_DAO _adUser;

        private int _genericKey;

        private GenericKeyType_DAO _genericKeyType;

        private System.DateTime? _dateReceived;

        private LegalEntity_DAO _legalEntity;

        private IList<ClientAnswer_DAO> _clientAnswers;

        private string _gUID;

        [PrimaryKey(PrimaryKeyType.Native, "ClientQuestionnaireKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._key;
            }
            set
            {
                this._key = value;
            }
        }

        [BelongsTo("BusinessEventQuestionnaireKey", NotNull = true)]
        public virtual BusinessEventQuestionnaire_DAO BusinessEventQuestionnaire
        {
            get
            {
                return this._businessEventQuestionnaire;
            }
            set
            {
                this._businessEventQuestionnaire = value;
            }
        }

        [Property("DatePresented", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime DatePresented
        {
            get
            {
                return this._datePresented;
            }
            set
            {
                this._datePresented = value;
            }
        }

        [BelongsTo("ADUserKey", NotNull = false)]
        public virtual ADUser_DAO ADUser
        {
            get
            {
                return this._adUser;
            }
            set
            {
                this._adUser = value;
            }
        }

        [Property("GenericKey", ColumnType = "Int32", NotNull = true)]
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

        [BelongsTo("GenericKeyTypeKey", NotNull = true)]
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

        [Property("DateReceived", ColumnType = "Timestamp", NotNull = false)]
        public virtual System.DateTime? DateReceived
        {
            get
            {
                return this._dateReceived;
            }
            set
            {
                this._dateReceived = value;
            }
        }

        [Property("GUID", ColumnType = "String", NotNull = true)]
        [Lurker]
        public virtual string GUID
        {
            get
            {
                return this._gUID;
            }
            set
            {
                //System.Guid guid = Guid.NewGuid();
                this._gUID = value;//guid.ToString();
            }
        }

        [BelongsTo("LegalEntityKey", NotNull = false)]
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

        [HasMany(typeof(ClientAnswer_DAO), ColumnKey = "ClientQuestionnaireKey", Table = "ClientAnswer", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<ClientAnswer_DAO> ClientAnswers
        {
            get
            {
                return this._clientAnswers;
            }
            set
            {
                this._clientAnswers = value;
            }
        }
    }
}