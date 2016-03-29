using System;
﻿using System.ComponentModel.DataAnnotations;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Services.Interfaces.ApplicationDomain.Models
{
    public class ApplicationAttributeModel
    {
        public ApplicationAttributeModel(int applicationNumber, OfferAttributeType applicationAttributeType)
        {
            this.ApplicationNumber = applicationNumber;
            this.ApplicationAttributeType = applicationAttributeType;
        }

        [Required]
        public int ApplicationNumber { get; protected set; }

        [Required]
        public OfferAttributeType ApplicationAttributeType { get; protected set; }
    }
}