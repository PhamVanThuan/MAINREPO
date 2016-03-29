using System;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    //[GenericTest(Globals.TestType.Find)]
    [ActiveRecord("StageTransition", Schema = "dbo", Lazy = true)]
    public partial class StageTransition_DAO : DB_2AM<StageTransition_DAO>
    {
        private int _genericKey;

        private ADUser_DAO _aDUser;

        private System.DateTime _transitionDate;
        DateTime? _EndTransitionDate;

        private string _comments;

        private int _key;

        private StageDefinitionStageDefinitionGroup_DAO _stageDefinitionStageDefinitionGroup;

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

        [BelongsTo("ADUserKey", NotNull = true)]
        [ValidateNonEmpty("AD User is a mandatory field")]
        public virtual ADUser_DAO ADUser
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

        [Property("TransitionDate", ColumnType = "Timestamp", NotNull = true, Insert = false)]
        [ValidateNonEmpty("Transition Date is a mandatory field")]
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

        [BelongsTo("StageDefinitionStageDefinitionGroupKey", NotNull = true)]
        [ValidateNonEmpty("Stage Defintion/Stage Definition Group is a mandatory field")]
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

        [Property("EndTransitionDate", ColumnType = "Timestamp", NotNull = false)]
        public virtual System.DateTime? EndTransitionDate
        {
            get
            {
                return this._EndTransitionDate;
            }
            set
            {
                this._EndTransitionDate = value;
            }
        }
    }
}