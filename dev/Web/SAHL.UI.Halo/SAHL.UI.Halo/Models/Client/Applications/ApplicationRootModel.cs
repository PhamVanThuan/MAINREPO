using System;

using SAHL.Core.BusinessModel.Enums;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Models.Client.Applications
{
    public class ApplicationRootModel : IHaloTileModel
    {
        public LegalEntityType LegalEntityTypeKey { get; set; }

        public CitizenType CitizenTypeKey { get; set; }

        public string LegalName { get; set; }

        public string IDNumber { get; set; }

        public string PassportNumber { get; set; }

        public string RegistrationNumber { get; set; }

        public string MaritalStatus { get; set; }

        public SAHL.Core.BusinessModel.Enums.LegalEntityType LegalEntityType { get; set; }

        public string LegalEntityStatus { get; set; }

        public string HomephoneNumber { get; set; }

        public string WorkphoneNumber { get; set; }

        public string CellPhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Address { get; set; }

        public string DomiciliumAddress { get; set; }

        public string BankingInstitute { get; set; }

        public SAHL.Core.BusinessModel.Enums.Gender Gender { get; set; }

        public SAHL.Core.BusinessModel.Enums.LegalEntityStatus Status { get; set; }
    }
}
