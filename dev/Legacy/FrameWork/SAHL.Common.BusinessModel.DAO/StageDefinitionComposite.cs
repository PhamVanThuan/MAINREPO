//using System;
//using System.Collections.Generic;
//using System.Text;
//using Castle.ActiveRecord;
//using SAHL.Common.BusinessModel.DAO.Database;

//namespace SAHL.Common.BusinessModel.DAO
//{
//    [ActiveRecord(DiscriminatorValue = "True")]
//    public partial class StageDefinitionComposite_DAO : StageDefinition_DAO
//    {
//        private string _compositeTypeName;

//        private IList<StageDefinitionStageDefinitionGroup_DAO> _childStageDefinitions;

//        [Property("CompositeTypeName", ColumnType = "String")]
//        public virtual string CompositeTypeName
//        {
//            get
//            {
//                return this._compositeTypeName;
//            }
//            set
//            {
//                this._compositeTypeName = value;
//            }
//        }

//        [HasAndBelongsToMany(typeof(StageDefinitionStageDefinitionGroup_DAO), ColumnKey = "StageDefinitionStageDefinitionGroupKey", ColumnRef = "StageDefinitionKey", Table = "StageDefinitionComposite", Inverse = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Lazy = true)]
//        public virtual IList<StageDefinitionStageDefinitionGroup_DAO> ChildStageDefinitions
//        {
//            get
//            {
//                return this._childStageDefinitions;
//            }
//            set
//            {
//                this._childStageDefinitions = value;
//            }
//        }

//        #region Static Overrides

//        /// <summary>
//        /// Overridden for correct type.
//        /// </summary>
//        /// <returns></returns>
//        public static StageDefinitionComposite_DAO Find(int id)
//        {
//            return ActiveRecordBase<StageDefinitionComposite_DAO>.Find(id);
//        }

//        /// <summary>
//        /// Overridden for correct type.
//        /// </summary>
//        /// <returns></returns>
//        public static StageDefinitionComposite_DAO FindFirst()
//        {
//            return ActiveRecordBase<StageDefinitionComposite_DAO>.FindFirst();
//        }

//        /// <summary>
//        /// Overridden for correct type.
//        /// </summary>
//        /// <returns></returns>
//        public static StageDefinitionComposite_DAO FindOne()
//        {
//            return ActiveRecordBase<StageDefinitionComposite_DAO>.FindOne();
//        }

//        #endregion
//    }
//}