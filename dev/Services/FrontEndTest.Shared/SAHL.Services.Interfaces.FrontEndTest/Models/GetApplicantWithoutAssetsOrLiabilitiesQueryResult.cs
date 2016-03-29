﻿using System;

namespace SAHL.Services.Interfaces.FrontEndTest.Models
{
    public class GetApplicantWithoutAssetsOrLiabilitiesQueryResult
    {
        public int OfferRoleKey { get; set; }

        public int LegalEntityKey { get; set; }

        public int OfferKey { get; set; }

        public int OfferRoleTypeKey { get; set; }

        public int GeneralStatusKey { get; set; }

        public DateTime StatusChangeDate { get; set; }
    }
}