//using System;
//using System.Text;
//using System.Collections.Generic;
//using SAHL.Common.BusinessModel.Validation;
//using SAHL.Common.BusinessModel.Base;
//using SAHL.Common.BusinessModel.Interfaces.Repositories;
//using SAHL.Common.BusinessModel.DAO;
//using SAHL.Common.Factories;
//using SAHL.Common.Collections;
//using SAHL.Common.Collections.Interfaces;
//using SAHL.Common.BusinessModel.Interfaces;
//namespace SAHL.Common.BusinessModel
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    public partial class StageDefinitionComposite : StageDefinition, IStageDefinitionComposite
//    {
//        protected new SAHL.Common.BusinessModel.DAO.StageDefinitionComposite_DAO _DAO;
//        public StageDefinitionComposite(SAHL.Common.BusinessModel.DAO.StageDefinitionComposite_DAO StageDefinitionComposite) : base(StageDefinitionComposite)
//        {
//            this._DAO = StageDefinitionComposite;
//        }
//        /// <summary>
//        /// 
//        /// </summary>
//        public String CompositeTypeName 
//        {
//            get { return _DAO.CompositeTypeName; }
//            set { _DAO.CompositeTypeName = value;}
//        }
//        /// <summary>
//        /// 
//        /// </summary>
//        private DAOEventList<StageDefinition_DAO, IStageDefinition, StageDefinition> _ChildStageDefinitions;
//        /// <summary>
//        /// 
//        /// </summary>
//        public IEventList<IStageDefinition> ChildStageDefinitions
//        {
//            get
//            {
//                if (null == _ChildStageDefinitions) 
//                {
//                    if(null == _DAO.ChildStageDefinitions)
//                        _DAO.ChildStageDefinitions = new List<StageDefinition_DAO>();
//                    _ChildStageDefinitions = new DAOEventList<StageDefinition_DAO, IStageDefinition, StageDefinition>(_DAO.ChildStageDefinitions);
//                    _ChildStageDefinitions.BeforeAdd += new EventListHandler(OnChildStageDefinitions_BeforeAdd);					
//                    _ChildStageDefinitions.BeforeRemove += new EventListHandler(OnChildStageDefinitions_BeforeRemove);
//                }
//                return _ChildStageDefinitions;
//            }
//        }
//    }
//}


