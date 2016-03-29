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
    /// SAHL.Common.BusinessModel.DAO.AllocationMandateSet_DAO
    /// </summary>
    public partial class AllocationMandateSet : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AllocationMandateSet_DAO>, IAllocationMandateSet
    {
        public AllocationMandateSet(SAHL.Common.BusinessModel.DAO.AllocationMandateSet_DAO AllocationMandateSet)
            : base(AllocationMandateSet)
        {
            this._DAO = AllocationMandateSet;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandateSet_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandateSet_DAO.AllocationMandateOperators
        /// </summary>
        private DAOEventList<AllocationMandateOperator_DAO, IAllocationMandateOperator, AllocationMandateOperator> _AllocationMandateOperators;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandateSet_DAO.AllocationMandateOperators
        /// </summary>
        public IEventList<IAllocationMandateOperator> AllocationMandateOperators
        {
            get
            {
                if (null == _AllocationMandateOperators)
                {
                    if (null == _DAO.AllocationMandateOperators)
                        _DAO.AllocationMandateOperators = new List<AllocationMandateOperator_DAO>();
                    _AllocationMandateOperators = new DAOEventList<AllocationMandateOperator_DAO, IAllocationMandateOperator, AllocationMandateOperator>(_DAO.AllocationMandateOperators);
                    _AllocationMandateOperators.BeforeAdd += new EventListHandler(OnAllocationMandateOperators_BeforeAdd);
                    _AllocationMandateOperators.BeforeRemove += new EventListHandler(OnAllocationMandateOperators_BeforeRemove);
                    _AllocationMandateOperators.AfterAdd += new EventListHandler(OnAllocationMandateOperators_AfterAdd);
                    _AllocationMandateOperators.AfterRemove += new EventListHandler(OnAllocationMandateOperators_AfterRemove);
                }
                return _AllocationMandateOperators;
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandateSet_DAO.UserOrganisationStructures
        /// </summary>
        private DAOEventList<UserOrganisationStructure_DAO, IUserOrganisationStructure, UserOrganisationStructure> _UserOrganisationStructures;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandateSet_DAO.UserOrganisationStructures
        /// </summary>
        public IEventList<IUserOrganisationStructure> UserOrganisationStructures
        {
            get
            {
                if (null == _UserOrganisationStructures)
                {
                    if (null == _DAO.UserOrganisationStructures)
                        _DAO.UserOrganisationStructures = new List<UserOrganisationStructure_DAO>();
                    _UserOrganisationStructures = new DAOEventList<UserOrganisationStructure_DAO, IUserOrganisationStructure, UserOrganisationStructure>(_DAO.UserOrganisationStructures);
                    _UserOrganisationStructures.BeforeAdd += new EventListHandler(OnUserOrganisationStructures_BeforeAdd);
                    _UserOrganisationStructures.BeforeRemove += new EventListHandler(OnUserOrganisationStructures_BeforeRemove);
                    _UserOrganisationStructures.AfterAdd += new EventListHandler(OnUserOrganisationStructures_AfterAdd);
                    _UserOrganisationStructures.AfterRemove += new EventListHandler(OnUserOrganisationStructures_AfterRemove);
                }
                return _UserOrganisationStructures;
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandateSet_DAO.AllocationMandateSetGroup
        /// </summary>
        public IAllocationMandateSetGroup AllocationMandateSetGroup
        {
            get
            {
                if (null == _DAO.AllocationMandateSetGroup) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IAllocationMandateSetGroup, AllocationMandateSetGroup_DAO>(_DAO.AllocationMandateSetGroup);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.AllocationMandateSetGroup = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.AllocationMandateSetGroup = (AllocationMandateSetGroup_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _AllocationMandateOperators = null;
            _UserOrganisationStructures = null;
        }
    }
}