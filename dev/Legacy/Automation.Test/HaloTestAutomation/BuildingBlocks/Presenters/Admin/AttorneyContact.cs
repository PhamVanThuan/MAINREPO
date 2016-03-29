using Common.Constants;
using Common.Enums;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Admin
{
    public class AttorneyContact : AttorneyContactControls
    {
        public void PopulateLegalEntityDetail
            (
                Automation.DataModels.AttorneyContacts attorneyContact
            )
        {
            if (base.ctl00MaintxtFirstName.Exists)
                base.ctl00MaintxtFirstName.Value = attorneyContact.LegalEntity.FirstNames;
            if (base.ctl00MaintxtSurname.Exists)
                base.ctl00MaintxtSurname.Value = attorneyContact.LegalEntity.Surname;

            if (base.ctl00MaintxtTelephoneNumberCODE.Exists)
                base.ctl00MaintxtTelephoneNumberCODE.Value = attorneyContact.LegalEntity.WorkPhoneCode.ToString();
            if (base.ctl00MaintxtTelephoneNumberNUMB.Exists)
                base.ctl00MaintxtTelephoneNumberNUMB.Value = attorneyContact.LegalEntity.WorkPhoneNumber.ToString();

            if (base.ctl00MaintxtFaxNumberCODE.Exists)
                base.ctl00MaintxtFaxNumberCODE.Value = attorneyContact.LegalEntity.FaxCode.ToString();
            if (base.ctl00MaintxtFaxNumberNUMB.Exists)
                base.ctl00MaintxtFaxNumberNUMB.Value = attorneyContact.LegalEntity.FaxNumber.ToString();

            if (base.ctl00MaintxtEmailAddress.Exists)
                base.ctl00MaintxtEmailAddress.Value = attorneyContact.LegalEntity.EmailAddress.ToString();

            if (base.RoleType.Exists)
            {
                switch (attorneyContact.ExternalRoleType)
                {
                    case ExternalRoleTypeEnum.DebtCounselling:
                        base.RoleType.Option(ExternalRoleType.DebtCounselling).Select();
                        break;

                    case ExternalRoleTypeEnum.DeceasedEstates:
                        base.RoleType.Option(ExternalRoleType.DeceasedEstates).Select();
                        break;

                    case ExternalRoleTypeEnum.Foreclosure:
                        base.RoleType.Option(ExternalRoleType.Foreclosure).Select();
                        break;

                    case ExternalRoleTypeEnum.Sequestrations:
                        base.RoleType.Option(ExternalRoleType.Sequestrations).Select();
                        break;

                    case ExternalRoleTypeEnum.WebAccess:
                        base.RoleType.Option(ExternalRoleType.WebAccess).Select();
                        break;
                }
            }
        }

        public void ClickAddLegalEntity()
        {
            base.ctl00MainbtnAdd.Click();
        }

        public void ClickDone()
        {
            base.ctl00MainbtnDone.Click();
        }
    }
}