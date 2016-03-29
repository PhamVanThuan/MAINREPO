using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Services.Capitec.Managers.Lookup;
using System;
using System.Linq;

namespace SAHL.Services.Capitec.Managers.ContactDetail
{
    public class ContactDetailDataManager : IContactDetailDataManager
    {
        private ILookupManager lookupService;
        private IDbFactory dbFactory;
        public ContactDetailDataManager(IDbFactory dbFactory, ILookupManager lookupService)
        {
            this.dbFactory = dbFactory;
            this.lookupService = lookupService;
        }

        public void AddPhoneNumberContactDetail(Guid contactDetailID, Guid phoneNumberContactDetailTypeEnumId, string code, string number)
        {
            Guid contactDetailTypeEnumId = Guid.Parse(ContactDetailTypeEnumDataModel.PHONE);

            ContactDetailDataModel contactDetail = new ContactDetailDataModel(contactDetailID, contactDetailTypeEnumId);

            PhoneNumberContactDetailDataModel phoneNumber = new PhoneNumberContactDetailDataModel(contactDetailID, phoneNumberContactDetailTypeEnumId, code, number);

            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert(contactDetail);
                db.Insert(phoneNumber);
                db.Complete();
            }
        }

        public void AddEmailAddressContactDetail(Guid contactDetailID, Guid emailAddressContactDetailTypeEnumId, string emailAddress)
        {
            Guid contactDetailTypeEnumId = Guid.Parse(ContactDetailTypeEnumDataModel.EMAIL);

            ContactDetailDataModel contactDetail = new ContactDetailDataModel(contactDetailID, contactDetailTypeEnumId);

            EmailAddressContactDetailDataModel homeEmailAddress = new EmailAddressContactDetailDataModel(contactDetailID, emailAddressContactDetailTypeEnumId, emailAddress);

            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert(contactDetail);
                db.Insert(homeEmailAddress);
                db.Complete();
            }
        }

        public void LinkContactDetailToApplicant(Guid applicantID, Guid contactDetailID)
        {
            Guid id = lookupService.GenerateCombGuid();

            ApplicantContactDetailDataModel applicantContactDetail = new ApplicantContactDetailDataModel(id, applicantID, contactDetailID);

            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert(applicantContactDetail);
                db.Complete();
            }
        }
    }
}