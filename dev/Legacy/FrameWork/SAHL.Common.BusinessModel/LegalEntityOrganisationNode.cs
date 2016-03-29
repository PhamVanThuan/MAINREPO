using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Common.BusinessModel
{
    public class LegalEntityOrganisationNode : BusinessModelBase<SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO>, ILegalEntityOrganisationNode
    {
        protected SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

        #region ILegalEntityOrganisationNode Members

        public virtual void AddLegalEntity(ILegalEntity LegalEntity)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

            //we need to add to the DAOEventList _legalEntities (can not add to the ReadOnlyEventList
            InitializeLegalentities();

            this._legalEntities.Add(spc.DomainMessages, LegalEntity);
        }

        /// <summary>
        /// Find Child
        /// </summary>
        /// <param name="OrganisationType"></param>
        /// <param name="OrganisationStructureDescription"></param>
        /// <returns></returns>
        public ILegalEntityOrganisationNode FindChild(IOrganisationType OrganisationType, string OrganisationStructureDescription)
        {
            return FindChild(OrganisationType, OrganisationStructureDescription, OrganisationStructureNodeTypes.LegalEntity);
        }

        /// <summary>
        /// Find Child
        /// </summary>
        /// <param name="OrganisationType"></param>
        /// <param name="OrganisationStructureDescription"></param>
        /// <param name="organisationStructureNodeType"></param>
        /// <returns></returns>
        public ILegalEntityOrganisationNode FindChild(IOrganisationType OrganisationType, string OrganisationStructureDescription, OrganisationStructureNodeTypes organisationStructureNodeType)
        {
            di.OrganisationStructureNodeType = organisationStructureNodeType;
            foreach (ILegalEntityOrganisationNode eaON in this.ChildOrganisationStructures)
            {
                if (eaON.OrganisationType.Key == OrganisationType.Key && eaON.Description == OrganisationStructureDescription)
                    return eaON;
            }
            return null;
        }

        SAHL.Common.Interfaces.IOrganisationStructureFactory di = new SAHL.Common.BusinessModel.Repositories.OrganisationStructureRepository.OrganisationStructureFactory();

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.ChildOrganisationStructures
        /// </summary>
        private DomainDAOEventList<OrganisationStructure_DAO, ILegalEntityOrganisationNode, LegalEntityOrganisationNode> _ChildOrganisationStructures;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.ChildOrganisationStructures
        /// </summary>
        public IEventList<ILegalEntityOrganisationNode> ChildOrganisationStructures
        {
            get
            {
                if (null == _ChildOrganisationStructures)
                {
                    if (null == _DAO.ChildOrganisationStructures)
                        _DAO.ChildOrganisationStructures = new List<OrganisationStructure_DAO>();
                    _ChildOrganisationStructures = new DomainDAOEventList<OrganisationStructure_DAO, ILegalEntityOrganisationNode, LegalEntityOrganisationNode>(_DAO.ChildOrganisationStructures, di);

                    //_ChildOrganisationStructures.BeforeAdd += new EventListHandler(OnChildOrganisationStructures_BeforeAdd);
                    //_ChildOrganisationStructures.BeforeRemove += new EventListHandler(OnChildOrganisationStructures_BeforeRemove);
                    //_ChildOrganisationStructures.AfterAdd += new EventListHandler(OnChildOrganisationStructures_AfterAdd);
                    //_ChildOrganisationStructures.AfterRemove += new EventListHandler(OnChildOrganisationStructures_AfterRemove);
                }
                return _ChildOrganisationStructures;
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.LegalEntities
        /// </summary>
        private DAOEventList<LegalEntity_DAO, ILegalEntity, LegalEntity> _legalEntities;

        private void InitializeLegalentities()
        {
            if (null == _legalEntities)
            {
                if (null == _DAO.LegalEntities)
                    _DAO.LegalEntities = new List<LegalEntity_DAO>();

                _legalEntities = new DAOEventList<LegalEntity_DAO, ILegalEntity, LegalEntity>(_DAO.LegalEntities);
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.LegalEntities
        /// </summary>
        public IReadOnlyEventList<ILegalEntity> LegalEntities
        {
            get
            {
                InitializeLegalentities();
                return new ReadOnlyEventList<ILegalEntity>(_legalEntities);
            }
        }

        public virtual void RemoveLegalEntity(ILegalEntity LegalEntity)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

            LegalEntity_DAO le_dao = _DAO.LegalEntities.Where(x => x.Key == LegalEntity.Key).FirstOrDefault();

            if (le_dao != null)
            {
                _DAO.LegalEntities.Remove(le_dao);
            }

            this._legalEntities = new DAOEventList<LegalEntity_DAO, ILegalEntity, LegalEntity>(_DAO.LegalEntities);
        }

        public virtual ILegalEntityOrganisationNode AddChildNode(IOrganisationType OrganisationType, string OrganisationStructureDescription, OrganisationStructureNodeTypes organisationStructureNodeType)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

            OrganisationStructureRepository.OrganisationStructureFactory factory = new OrganisationStructureRepository.OrganisationStructureFactory();
            ILegalEntityOrganisationNode leosN = factory.CreateLegalEntityOrganisationNode(OrganisationType, organisationStructureNodeType);
            leosN.Description = OrganisationStructureDescription;
            leosN.GeneralStatus = LKRepo.GeneralStatuses[GeneralStatuses.Active];

            //set up the bidirectional reference
            leosN.Parent = this;
            this.ChildOrganisationStructures.Add(spc.DomainMessages, leosN);

            return leosN;
        }

        public virtual void RemoveChild(ILegalEntityOrganisationNode leON)
        {
            throw new NotImplementedException("This functionality is not yet available.");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="newParent"></param>
        /// <returns></returns>
        public ILegalEntityOrganisationNode MoveMe(ILegalEntityOrganisationNode newParent)
        {
            //reset my parent
            this.Parent = newParent;

            //we need to refresh this object so all the collection population happens correctly
            //so we have to call save. This is not standard in business model objects
            OSRepo.SaveLegalEntityOrganisationNode(this);
            this.Refresh();
            return this;
        }

        public virtual void RemoveMe(ILegalEntity LegalEntity)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool HasActiveChildren
        {
            get
            {
                foreach (ILegalEntityOrganisationNode leoNode in this.ChildOrganisationStructures)
                {
                    if (leoNode.GeneralStatus.Key == (int)GeneralStatuses.Active)
                        return true;
                }
                return false;
            }
        }

        public virtual ILegalEntityOrganisationNode GetOrgstructureParentItem(OrganisationTypes orgType, string description)
        {
            ////Current Orgitem is at the branch level we need
            if (this.OrganisationType.Key == (int)orgType && String.Compare(this.Description, description, true) == 0)
            {
                return this;
            }

            if (this.Parent == null)
            {
                //We are at top item as it either has no parent and we can not find the orgTypeKey
                return null;
            }

            // We look through parent items looking for description and orgtype
            // This should only be done when looking for designations
            if (orgType == OrganisationTypes.Designation)
            {
                foreach (ILegalEntityOrganisationNode leon in this.Parent.ChildOrganisationStructures)
                {
                    //Found the org type we are looking for
                    if (leon.GeneralStatus.Key == (int)GeneralStatuses.Active && leon.OrganisationType.Key == (int)orgType && String.Compare(leon.Description, description, true) == 0)
                        return leon;
                }
            }

            // Else we another another pass
            return this.Parent.GetOrgstructureParentItem(orgType, description);
        }

        public virtual ILegalEntityOrganisationNode GetOrgstructureParentItem(OrganisationTypes orgType)
        {
            ////Current Orgitem is active and the type we want
            if (this.GeneralStatus.Key == (int)GeneralStatuses.Active && this.OrganisationType.Key == (int)orgType)
            {
                return this;
            }

            if (this.Parent == null)
            {
                //We are at top item as it either has no parent and we can not find the orgTypeKey
                return null;
            }

            // Else we another another pass
            return this.Parent.GetOrgstructureParentItem(orgType);
        }

        public virtual ILegalEntityOrganisationNode GetOrgstructureTopParentItem(OrganisationTypes orgType, int rootNodeKey)
        {
            // Current Orgitem is active and the type we want and is top level below root
            if (this.GeneralStatus.Key == (int)GeneralStatuses.Active
                && this.OrganisationType.Key == (int)orgType
                && this.Parent.Key == rootNodeKey)
            {
                return this;
            }

            if (this.Parent == null)
            {
                //We are at top item as it either has no parent and we can not find the orgTypeKey
                return null;
            }

            // Else we another another pass
            return this.Parent.GetOrgstructureTopParentItem(orgType, rootNodeKey);
        }

        #endregion ILegalEntityOrganisationNode Members

        #region IGenericOrganisationNode Members

        public LegalEntityOrganisationNode(SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO OrganisationStructure)
            : base(OrganisationStructure)
        {
            this._DAO = OrganisationStructure;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.OrganisationType
        /// </summary>
        public IOrganisationType OrganisationType
        {
            get
            {
                if (null == _DAO.OrganisationType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IOrganisationType, OrganisationType_DAO>(_DAO.OrganisationType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.OrganisationType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.OrganisationType = (OrganisationType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.GeneralStatus
        /// </summary>
        public IGeneralStatus GeneralStatus
        {
            get
            {
                if (null == _DAO.GeneralStatus) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IGeneralStatus, GeneralStatus_DAO>(_DAO.GeneralStatus);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.GeneralStatus = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.GeneralStatus = (GeneralStatus_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.Parent
        /// </summary>
        public ILegalEntityOrganisationNode Parent
        {
            get
            {
                if (null == _DAO.Parent)
                    return null;
                else
                {
                    OrganisationStructureRepository.OrganisationStructureFactory factory = new OrganisationStructureRepository.OrganisationStructureFactory();
                    return factory.GetLEOSNode(_DAO.Parent);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Parent = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Parent = (OrganisationStructure_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.ApplicationRoleTypes
        /// </summary>
        private DAOEventList<ApplicationRoleType_DAO, IApplicationRoleType, ApplicationRoleType> _ApplicationRoleTypes;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.OrganisationStructure_DAO.ApplicationRoleTypes
        /// </summary>
        public IEventList<IApplicationRoleType> ApplicationRoleTypes
        {
            get
            {
                if (null == _ApplicationRoleTypes)
                {
                    if (null == _DAO.ApplicationRoleTypes)
                        _DAO.ApplicationRoleTypes = new List<ApplicationRoleType_DAO>();
                    _ApplicationRoleTypes = new DAOEventList<ApplicationRoleType_DAO, IApplicationRoleType, ApplicationRoleType>(_DAO.ApplicationRoleTypes);

                    //_ApplicationRoleTypes.BeforeAdd += new EventListHandler(OnApplicationRoleTypes_BeforeAdd);
                    //_ApplicationRoleTypes.BeforeRemove += new EventListHandler(OnApplicationRoleTypes_BeforeRemove);
                    //_ApplicationRoleTypes.AfterAdd += new EventListHandler(OnApplicationRoleTypes_AfterAdd);
                    //_ApplicationRoleTypes.AfterRemove += new EventListHandler(OnApplicationRoleTypes_AfterRemove);
                }
                return _ApplicationRoleTypes;
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _ChildOrganisationStructures = null;
            _ApplicationRoleTypes = null;
            _legalEntities = null;
        }

        #endregion IGenericOrganisationNode Members

        private IOrganisationStructureRepository _osRepo;

        protected IOrganisationStructureRepository OSRepo
        {
            get
            {
                if (_osRepo == null)
                    _osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                return _osRepo;
            }
        }

        private ILookupRepository _lkRepo;

        protected ILookupRepository LKRepo
        {
            get
            {
                if (_lkRepo == null)
                    _lkRepo = RepositoryFactory.GetRepository<ILookupRepository>();

                return _lkRepo;
            }
        }
    }
}