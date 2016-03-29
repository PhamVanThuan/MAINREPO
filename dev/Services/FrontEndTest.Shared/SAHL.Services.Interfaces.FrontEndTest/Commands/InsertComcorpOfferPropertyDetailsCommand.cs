using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class InsertComcorpOfferPropertyDetailsCommand : ServiceCommand, IFrontEndTestCommand
    {
        [Required]
        public ComcorpOfferPropertyDetailsDataModel model { get; protected set; }

        public InsertComcorpOfferPropertyDetailsCommand(ComcorpOfferPropertyDetailsDataModel model)
        {
            this.model = model;
        }
    }
}