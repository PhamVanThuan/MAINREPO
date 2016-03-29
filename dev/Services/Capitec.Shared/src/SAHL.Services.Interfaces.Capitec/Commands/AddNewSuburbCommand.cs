using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.Capitec.Commands
{
    [AuthorisedCommand(Roles = "User")]
    public class AddNewSuburbCommand : ServiceCommand, ICapitecServiceCommand
    {
        public AddNewSuburbCommand(string suburbName, int sahlSuburbKey, string postalCode, Guid cityId)
        {
            this.SuburbName = suburbName;
            this.SahlSuburbKey = sahlSuburbKey;
            this.PostalCode = postalCode;
            this.CityId = cityId;
        }

        [Required]
        public string SuburbName { get; protected set; }

        [Required]
        public int SahlSuburbKey { get; protected set; }

        [Required]
        public string PostalCode { get; protected set; }

        [Required]
        public Guid CityId { get; protected set; }
    }
}