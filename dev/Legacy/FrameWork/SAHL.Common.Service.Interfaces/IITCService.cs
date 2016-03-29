using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.Service.Interfaces
{
    public interface IITCService
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="LegalEntity"></param>
        /// <param name="ListAddress"></param>
        /// <param name="SaveITC"></param>
        void DoEnquiry(ILegalEntityNaturalPerson LegalEntity, IList<IAddressStreet> ListAddress, IITC SaveITC);
    }
}