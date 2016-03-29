
using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO.Database;
using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("StageTransitionComposite", Schema = "dbo")]
    public partial class StageTransitionComposite_WTF_DAO : DB_Test_WTF<StageTransitionComposite_WTF_DAO>
    {

        private int _genericKey;

        private System.DateTime _transitionDate;

        private string _comments;

        private int _stageTransitionReasonKey;

        private int _key;

        private ADUser_WTF_DAO _aDUser;

        private StageDefinitionStageDefinitionGroup_WTF_DAO _stageDefinitionStageDefinitionGroup;

        private StageTransition_WTF_DAO _stageTransition;

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

        [Property("StageTransitionReasonKey", ColumnType = "Int32")]
        public virtual int StageTransitionReasonKey
        {
            get
            {
                return this._stageTransitionReasonKey;
            }
            set
            {
                this._stageTransitionReasonKey = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "StageTransitionCompositeKey", ColumnType = "Int32")]
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

        [BelongsTo("StageDefinitionStageDefinitionGroupKey", NotNull = true)]
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

        [BelongsTo("StageTransitionKey", NotNull = true)]
        public virtual StageTransition_WTF_DAO StageTransition
        {
            get
            {
                return this._stageTransition;
            }
            set
            {
                this._stageTransition = value;
            }
        }
    }
}

