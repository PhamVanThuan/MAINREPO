using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ApplicationDomain.Events
{
    public class RefinanceApplicationAddedEvent : Event
    {
        public RefinanceApplicationAddedEvent(DateTime date, int applicationNumber, OfferType applicationType, OfferStatus applicationStatus, int? applicationSourceKey, 
            OriginationSource originationSource, decimal estimatedPropertyValue, int term, decimal cashOut, Product product)
            : base(date)
        {
            this.ApplicationNumber = applicationNumber;
            this.ApplicationType = applicationType;
            this.ApplicationStatus = applicationStatus;
            this.ApplicationSourceKey = applicationSourceKey;
            this.OriginationSource = originationSource;
            this.Term = term;
            this.Product = product;
            this.CashOut = cashOut;
            this.EstimatedPropertyValue = estimatedPropertyValue;
        }

        public int ApplicationNumber { get; protected set; }

        public Product Product { get; protected set; }

        public OfferType ApplicationType { get; protected set; }

        public OfferStatus ApplicationStatus { get; protected set; }

        public int? ApplicationSourceKey { get; protected set; }

        public OriginationSource OriginationSource { get; protected set; }

        public int Term { get; protected set; }

        public decimal CashOut { get; protected set; }

        public decimal EstimatedPropertyValue { get; protected set; }
    }
}