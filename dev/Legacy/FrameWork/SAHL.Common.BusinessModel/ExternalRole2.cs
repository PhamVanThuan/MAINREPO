using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ExternalRole_DAO
    /// </summary>
    public partial class ExternalRole : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ExternalRole_DAO>, IExternalRole
    {
        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
            Rules.Add("ExternalRoleLitigationLegalEntityMandatoryDetail");
        }

        public void OnExternalRoleDeclarations_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        public void OnExternalRoleDeclarations_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        public void OnExternalRoleDeclarations_AfterRemove(ICancelDomainArgs args, object Item)
        {
        }

        public void OnExternalRoleDeclarations_AfterAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        /// A collection of external role domicilium that are defined for this ExternalRole.
        /// </summary>
        private IExternalRoleDomicilium _pendingExternalRoleDomicilium;

        /// <summary>
        /// A collection of external role domicilium that are defined for this ExternalRole.
        /// </summary>
        public IExternalRoleDomicilium PendingExternalRoleDomicilium
        {
            get
            {
                if (_pendingExternalRoleDomicilium == null)
                {
                    if (_DAO.ExternalRoleDomiciliums != null || _DAO.ExternalRoleDomiciliums.Count > 0)
                    {
                        IBusinessModelTypeMapper businessModelTypeMapper = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                        ExternalRoleDomicilium_DAO pendingExternalRoleDomicilium_DAO;

                        pendingExternalRoleDomicilium_DAO = _DAO.ExternalRoleDomiciliums.FirstOrDefault(x => x.LegalEntityDomicilium.GeneralStatus.Key == (int)GeneralStatuses.Pending);

                        _pendingExternalRoleDomicilium = businessModelTypeMapper.GetMappedType<IExternalRoleDomicilium>(pendingExternalRoleDomicilium_DAO);
                    }
                }
                return _pendingExternalRoleDomicilium;
            }

            set
            {
                if (value == null)
                {
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ExternalRoleDomiciliums.Add((ExternalRoleDomicilium_DAO)obj.GetDAOObject());
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}