using System;
using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Halo.Tiles.LegalEntity.Default
{
    public class LegalEntityMajorTileModel : ITileModel
    {
        public string LegalName { get; set; }

        public string IDNumber { get; set; }

        public string RegistrationNumber { get; set; }

        public string LegalEntityStatus { get; set; }

        public string HomephoneNumber { get; set; }

        public string WorkphoneNumber { get; set; }

        public string CellPhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string PostalAddress { get; set; }

        public string DomiciliumAddress { get; set; }

        public string EmailAddress { get; set; }

        public string BankingInstitute { get; set; }
    }
}