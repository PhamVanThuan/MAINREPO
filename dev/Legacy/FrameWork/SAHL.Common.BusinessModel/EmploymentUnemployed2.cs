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
using SAHL.Common.Globals;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// 
	/// </summary>
    public partial class EmploymentUnemployed : Employment, IEmploymentUnemployed
	{
        private IReadOnlyEventList<RemunerationTypes> _supportedRemunerationTypes;

        public override IReadOnlyEventList<RemunerationTypes> SupportedRemunerationTypes
        {
            get
            {
                if (_supportedRemunerationTypes == null)
                    _supportedRemunerationTypes = GetSupportedRemunerationTypes(typeof(EmploymentUnemployedRemunerationTypes));

                return _supportedRemunerationTypes;
            }
        }
	}
}


