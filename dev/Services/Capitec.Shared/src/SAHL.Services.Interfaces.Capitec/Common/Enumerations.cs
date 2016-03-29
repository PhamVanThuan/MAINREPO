namespace SAHL.Services.Interfaces.Capitec.Common
{
    public class Enumerations
    {
        public enum DeclarationTypeEnums
        {
            Yes,
            No
        }

        public enum PhoneNumberContactDetailTypeEnums
        {
            Home,
            Work,
            Fax,
            Mobile
        }

        public enum ContactDetailTypeEnums
        {
            Email,
            Phone,
            SMS
        }

        public enum EmailAddressContactDetailTypeEnums
        {
            Home,
            Work
        }

        public enum AddressTypeEnums
        {
            Residential,
            Postal
        }

        public enum ApplicationStatusEnums
        {
            InProgress = 1,
            NTU = 4,
            Decline = 5,
            PortalDecline = 6
        }
    }
}