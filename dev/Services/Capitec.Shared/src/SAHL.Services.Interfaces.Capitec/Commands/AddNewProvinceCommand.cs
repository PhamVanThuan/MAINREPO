using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.Capitec.Commands
{
    [AuthorisedCommand(Roles = "User")]
    public class AddNewProvinceCommand : ServiceCommand, ICapitecServiceCommand
    {
        public AddNewProvinceCommand(string provinceName, int sahlProvinceKey, Guid countryId)
        {
            this.ProvinceName = provinceName;
            this.SahlProvinceKey = sahlProvinceKey;
            this.CountryId = countryId;
        }

        [Required]
        public string ProvinceName { get; protected set; }

        [Required]
        public int SahlProvinceKey { get; protected set; }

        [Required]
        public Guid CountryId { get; protected set; }
    }
}