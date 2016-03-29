
using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("StageTransition", Schema = "dbo")]
    public partial class StageTransition_WTF_DAO : DB_Test_WTF<StageTransition_WTF_DAO>
    {

        private int _genericKey;

        private System.DateTime _transitionDate;

        private string _comments;

        private System.DateTime? _endTransitionDate;

        private int _key;

        private IList<StageTransitionComposite_WTF_DAO> _stageTransitionComposites;

        private ADUser_WTF_DAO _aDUser;

        private StageDefinitionStageDefinitionGroup_WTF_DAO _stageDefinitionStageDefinitionGroup;

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

        [Property("TransitionDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime TransitionDate
        {
            get
            {
                return this._transitionDate;
            }
            set
            {
                this._transitionDate = value;
            }
        }

        [Property("Comments", ColumnType = "String")]
        public virtual string Comments
        {
            get
            {
                return this._comments;
            }
            set
            {
                this._comments = value;
            }
        }

        [Property("EndTransitionDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? EndTransitionDate
        {
            get
            {
                return this._endTransitionDate;
            }
            set
            {
                this._endTransitionDate = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "StageTransitionKey", ColumnType = "Int32")]
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

        [HasMany(typeof(StageTransitionComposite_WTF_DAO), ColumnKey = "StageTransitionKey", Table = "StageTransitionComposite")]
        public virtual IList<StageTransitionComposite_WTF_DAO> StageTransitionComposites
        {
            get
            {
                return this._stageTransitionComposites;
            }
            set
            {
                this._stageTransitionComposites = value;
            }
        }

        [BelongsTo("ADUserKey", NotNull = true)]
        public virtual ADUser_WTF_DAO ADUser
        {
            get
            {
                return this._aDUser;
            }
            set
            {
                this._aDUser = value;
            }
        }

        [BelongsTo("StageDefinitionStageDefinitionGroupKey")]
        public virtual StageDefinitionStageDefinitionGroup_WTF_DAO StageDefinitionStageDefinitionGroup
        {
            get
            {
                return this._stageDefinitionStageDefinitionGroup;
            }
            set
            {
                this._stageDefinitionStageDefinitionGroup = value;
            }
        }
    }
}

