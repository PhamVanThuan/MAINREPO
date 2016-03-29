using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AllocationMandateSetGroup_DAO
    /// </summary>
    public partial class AllocationMandateSetGroup : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AllocationMandateSetGroup_DAO>, IAllocationMandateSetGroup
    {
        public AllocationMandateSetGroup(SAHL.Common.BusinessModel.DAO.AllocationMandateSetGroup_DAO AllocationMandateSetGroup)
            : base(AllocationMandateSetGroup)
        {
            this._DAO = AllocationMandateSetGroup;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandateSetGroup_DAO.AllocationGroupName
        /// </summary>
        public String AllocationGroupName
        {
            get { return _DAO.AllocationGroupName; }
            set { _DAO.AllocationGroupName = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandateSetGroup_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandateSetGroup_DAO.AllocationMandateSets
        /// </summary>
        private DAOEventList<AllocationMandateSet_DAO, IAllocationMandateSet, AllocationMandateSet> _AllocationMandateSets;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandateSetGroup_DAO.AllocationMandateSets
        /// </summary>
        public IEventList<IAllocationMandateSet> AllocationMandateSets
        {
            get
            {
                if (null == _AllocationMandateSets)
                {
                    if (null == _DAO.AllocationMandateSets)
                        _DAO.AllocationMandateSets = new List<AllocationMandateSet_DAO>();
                    _AllocationMandateSets = new DAOEventList<AllocationMandateSet_DAO, IAllocationMandateSet, AllocationMandateSet>(_DAO.AllocationMandateSets);
                    _AllocationMandateSets.BeforeAdd += new EventListHandler(OnAllocationMandateSets_BeforeAdd);
                    _AllocationMandateSets.BeforeRemove += new EventListHandler(OnAllocationMandateSets_BeforeRemove);
                    _AllocationMandateSets.AfterAdd += new EventListHandler(OnAllocationMandateSets_AfterAdd);
                    _AllocationMandateSets.AfterRemove += new EventListHandler(OnAllocationMandateSets_AfterRemove);
                }
                return _AllocationMandateSets;
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandateSetGroup_DAO.OrganisationStructure
        /// </summary>
        public IOrganisationStructure OrganisationStructure
        {
            get
            {
                if (null == _DAO.OrganisationStructure) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IOrganisationStructure, OrganisationStructure_DAO>(_DAO.OrganisationStructure);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.OrganisationStructure = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.OrganisationStructure = (OrganisationStructure_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _AllocationMandateSets = null;
        }
    }
}