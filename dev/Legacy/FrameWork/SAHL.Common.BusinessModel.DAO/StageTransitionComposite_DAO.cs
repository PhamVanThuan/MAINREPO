using System;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    //[DoNotTestWithGenericTest()]
    [ActiveRecord("StageTransitionComposite", Schema = "dbo", Lazy = true)]
    public partial class StageTransitionComposite_DAO : DB_2AM<StageTransitionComposite_DAO>
    {
        private int _key;

        private int _genericKey;

        private int _stageTransitionReasonKey;

        private DateTime _transitionDate;

        private string _comments;

        private StageDefinitionStageDefinitionGroup_DAO _stageDefinitionStageDefinitionGroup;

        private StageTransition_DAO _stageTransition;

        private ADUser_DAO _adUser;

        [Property("GenericKey", ColumnType = "Int32")]
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

        [Property("TransitionDate", ColumnType = "DateTime")]
        public virtual DateTime TransitionDate
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

        [BelongsTo("StageDefinitionStageDefinitionGroupKey", NotNull = true)]
        public virtual StageDefinitionStageDefinitionGroup_DAO StageDefinitionStageDefinitionGroup
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
        [ValidateNonEmpty("Stage Transition is a mandatory field")]
        public virtual StageTransition_DAO StageTransition
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

        [BelongsTo("ADUserKey", NotNull = true)]
        [ValidateNonEmpty("AD User Key is a mandatory field")]
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
    }
}