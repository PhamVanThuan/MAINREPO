using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Security;
using System.Security.Principal;
using SAHL.Common.Globals;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// 
	/// </summary>
    public partial class WorkflowRole : BusinessModelBase<SAHL.Common.BusinessModel.DAO.WorkflowRole_DAO>, IWorkflowRole
    {
        private ILegalEntity _legalEntity;

        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);            
        }

        /// <summary>
        /// Gets/sets the LegalEntity to which the role applies.
        /// </summary>
        public ILegalEntity LegalEntity
        {
            get
            {
                if (LegalEntityKey == 0)
                    return null;

                // if the LegalEntity object is null or the LegalEntityKey has changed since we loaded the object, 
                // then reload
                if (_legalEntity == null || _legalEntity.Key != LegalEntityKey)
                {
                    LegalEntity_DAO leDao = LegalEntity_DAO.Find(LegalEntityKey);
                    IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    _legalEntity = bmtm.GetMappedType<ILegalEntity>(leDao);
                }
                return _legalEntity;
            }
            set
            {
                _legalEntity = value;
                LegalEntityKey = _legalEntity.Key;
            }
        }
    }
}


