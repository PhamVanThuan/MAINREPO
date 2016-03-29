using System;
using SAHL.Common.BusinessModel.Interfaces;

namespace DomainService2.Specs.DomainService.Stubs
{
    internal class Valuation : IValuation
    {
        public string Data
        {
            get;
            set;
        }

        public double? HOCConventionalAmount
        {
            get;
            set;
        }

        public IHOCRoof HOCRoof
        {
            get;
            set;
        }

        public double? HOCShingleAmount
        {
            get;
            set;
        }

        public double? HOCThatchAmount
        {
            get;
            set;
        }

        public bool IsActive
        {
            get;
            set;
        }

        public int Key
        {
            get;
            set;
        }

        public IProperty Property
        {
            get;
            set;
        }

        public double? ValuationAmount
        {
            get;
            set;
        }

        public IValuationClassification ValuationClassification
        {
            get;
            set;
        }

        public IValuationDataProviderDataService ValuationDataProviderDataService
        {
            get;
            set;
        }

        public DateTime ValuationDate
        {
            get;
            set;
        }

        public double? ValuationEscalationPercentage
        {
            get;
            set;
        }

        public double? ValuationHOCValue
        {
            get;
            set;
        }

        public double? ValuationMunicipal
        {
            get;
            set;
        }

        public IValuationStatus ValuationStatus
        {
            get;
            set;
        }

        public string ValuationUserID
        {
            get;
            set;
        }

        public IValuator Valuator
        {
            get;
            set;
        }

        public bool ValidateEntity()
        {
            throw new NotImplementedException();
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }
    }
}