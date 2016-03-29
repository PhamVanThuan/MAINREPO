using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IVendor : IEntityValidation, IBusinessModelObject
    {
        System.Int32 Key
        {
            get;
            set;
        }

        System.String VendorCode
        {
            get;
            set;
        }

        IOrganisationStructure OrganisationStructure
        {
            get;
            set;
        }

        ILegalEntity LegalEntity
        {
            get;
            set;
        }

        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }

        IVendor Parent
        {
            get;
            set;
        }
    }
}
