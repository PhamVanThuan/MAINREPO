﻿using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetActiveClientAddressByAddressFormatQuery : ServiceQuery<LegalEntityAddressDataModel>, IFrontEndTestQuery, ISqlServiceQuery<LegalEntityAddressDataModel>
    {
        public int AddressFormat { get; protected set; }

        public GetActiveClientAddressByAddressFormatQuery(int AddressFormat)
        {
            this.AddressFormat = AddressFormat;
        }
    }
}