using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.Capitec.Commands
{
    [AuthorisedCommand(Roles = "User")]
    public class ChangeProvinceDetailsCommand : ServiceCommand, ICapitecServiceCommand
    {
        public ChangeProvinceDetailsCommand(Guid id, string provinceName, int sahlProvinceKey, Guid countryId)
            : base(id)
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