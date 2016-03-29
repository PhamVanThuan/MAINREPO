using SAHL.Services.Interfaces.Capitec.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Capitec.Managers.ContactDetail
{
    public interface IContactDetailDataManager
    {
        void AddPhoneNumberContactDetail(Guid contactDetailID, Guid phoneNumberContactDetailTypeEnumId, string code, string number);

        void AddEmailAddressContactDetail(Guid contactDetailID, Guid emailAddressContactDetailType, string emailAddress);

        void LinkContactDetailToApplicant(Guid applicantID, Guid contactDetailID);
    }
}
