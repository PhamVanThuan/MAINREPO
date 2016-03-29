using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.Capitec.Commands
{
    [AuthorisedCommand(Roles = "User")]
    public class AddNewCityCommand : ServiceCommand, ICapitecServiceCommand
    {
        public AddNewCityCommand(string cityName, int sahlCityKey, Guid provinceId)
        {
            this.CityName = cityName;
            this.SahlCityKey = sahlCityKey;
            this.ProvinceId = provinceId;
        }

        [Required]
        public string CityName { get; protected set; }

        [Required]
        public int SahlCityKey { get; protected set; }

        [Required]
        public Guid ProvinceId { get; protected set; }
    }
}