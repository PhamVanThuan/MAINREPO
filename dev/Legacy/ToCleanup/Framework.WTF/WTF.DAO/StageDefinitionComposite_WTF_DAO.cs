
using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO.Database;
using Castle.ActiveRecord;
using NHibernate;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("StageDefinitionComposite", Schema = "dbo")]
    public partial class StageDefinitionComposite_WTF_DAO : DB_Test_WTF<StageDefinitionComposite_WTF_DAO>
    {

        private bool _useThisDate;

        private int _sequence;

        private bool _useThisReason;

        private int _key;

        private StageDefinitionStageDefinitionGroup_WTF_DAO _stageDefinitionStageDefinitionGroup;

        [Property("UseThisDate", ColumnType = "Boolean", NotNull = true)]
        public virtual bool UseThisDate
        {
            get
            {
                return this._useThisDate;
            }
            set
            {
                this._useThisDate = value;
            }
        }

        [Property("Sequence", ColumnType = "Int32", NotNull = true)]
        public virtual int Sequence
        {
            get
            {
                return this._sequence;
            }
            set
            {
                this._sequence = value;
            }
        }

        [Property("UseThisReason", ColumnType = "Boolean", NotNull = true)]
        public virtual bool UseThisReason
        {
            get
            {
                return this._useThisReason;
            }
            set
            {
                this._useThisReason = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "StageDefinitionCompositeKey", ColumnType = "Int32")]
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

        [BelongsTo("StageDefinitionStageDefinitionGroupCompositeKey", NotNull = true)]
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

