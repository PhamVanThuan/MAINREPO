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
    /// SAHL.Common.BusinessModel.DAO.ApplicationRoleType_DAO
    /// </summary>
    public partial class ApplicationRoleType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationRoleType_DAO>, IApplicationRoleType
    {
        public ApplicationRoleType(SAHL.Common.BusinessModel.DAO.ApplicationRoleType_DAO ApplicationRoleType)
            : base(ApplicationRoleType)
        {
            this._DAO = ApplicationRoleType;
        }

        /// <summary>
        /// The description of the Application Role Type
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// This is the relationship between the OrganisationStructure and the ApplicationRoleType as defined in the
        /// OfferRoleTypeOrganisationStructureMapping. An ApplicationRoleType can have many OrganisationStructures and thus an
        /// OrganisationStructure can be related to many ApplicationRoleTypes.
        /// </summary>
        private DAOEventList<OrganisationStructure_DAO, IOrganisationStructure, OrganisationStructure> _OfferRoleTypeOrganisationStructures;

        /// <summary>
        /// This is the relationship between the OrganisationStructure and the ApplicationRoleType as defined in the
        /// OfferRoleTypeOrganisationStructureMapping. An ApplicationRoleType can have many OrganisationStructures and thus an
        /// OrganisationStructure can be related to many ApplicationRoleTypes.
        /// </summary>
        public IEventList<IOrganisationStructure> OfferRoleTypeOrganisationStructures
        {
            get
            {
                if (null == _OfferRoleTypeOrganisationStructures)
                {
                    if (null == _DAO.OfferRoleTypeOrganisationStructures)
                        _DAO.OfferRoleTypeOrganisationStructures = new List<OrganisationStructure_DAO>();
                    _OfferRoleTypeOrganisationStructures = new DAOEventList<OrganisationStructure_DAO, IOrganisationStructure, OrganisationStructure>(_DAO.OfferRoleTypeOrganisationStructures);
                    _OfferRoleTypeOrganisationStructures.BeforeAdd += new EventListHandler(OnOfferRoleTypeOrganisationStructures_BeforeAdd);
                    _OfferRoleTypeOrganisationStructures.BeforeRemove += new EventListHandler(OnOfferRoleTypeOrganisationStructures_BeforeRemove);
                    _OfferRoleTypeOrganisationStructures.AfterAdd += new EventListHandler(OnOfferRoleTypeOrganisationStructures_AfterAdd);
                    _OfferRoleTypeOrganisationStructures.AfterRemove += new EventListHandler(OnOfferRoleTypeOrganisationStructures_AfterRemove);
                }
                return _OfferRoleTypeOrganisationStructures;
            }
        }

        /// <summary>
        /// Determines the Application Role Type Group to which the Application Role Type belongs.
        /// </summary>
        public IApplicationRoleTypeGroup ApplicationRoleTypeGroup
        {
            get
            {
                if (null == _DAO.ApplicationRoleTypeGroup) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationRoleTypeGroup, ApplicationRoleTypeGroup_DAO>(_DAO.ApplicationRoleTypeGroup);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ApplicationRoleTypeGroup = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ApplicationRoleTypeGroup = (ApplicationRoleTypeGroup_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _OfferRoleTypeOrganisationStructures = null;
        }
    }
}