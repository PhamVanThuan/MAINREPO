using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Services.Interfaces.ApplicationDomain.Events
{
    public class NewPurchaseApplicationAddedEvent : Event
    {
        public NewPurchaseApplicationAddedEvent(DateTime date, int applicationNumber, OfferType applicationType, OfferStatus applicationStatus, int? applicationSourceKey, 
            OriginationSource originationSource, decimal deposit, decimal purchasePrice, int term, Product product)
            : base(date)
        {
            this.ApplicationNumber = applicationNumber;
            this.PurchasePrice = purchasePrice;
            this.Deposit = deposit;
            this.ApplicationType = applicationType;
            this.ApplicationStatus = applicationStatus;
            this.ApplicationSourceKey = applicationSourceKey;
            this.OriginationSource = originationSource;
            this.Term = term;
            this.Product = product;
        }

        public int ApplicationNumber { get; protected set; }

        public decimal PurchasePrice { get; protected set; }

        public decimal Deposit { get; protected set; }

        public Product Product { get; protected set; }

        public OfferType ApplicationType { get; protected set; }

        public OfferStatus ApplicationStatus { get; protected set; }

        public int? ApplicationSourceKey { get; protected set; }

        public OriginationSource OriginationSource { get; protected set; }

        public int Term { get; protected set; }
    }
}