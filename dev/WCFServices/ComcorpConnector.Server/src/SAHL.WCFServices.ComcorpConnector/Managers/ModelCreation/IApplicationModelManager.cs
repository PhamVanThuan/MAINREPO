﻿using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation
{
    public interface IApplicationModelManager
    {
        NewPurchaseApplicationCreationModel PopulateNewPurchaseApplicationCreationModel(Application application);

        SwitchApplicationCreationModel PopulateSwitchApplicationCreationModel(Application application);

        RefinanceApplicationCreationModel PopulateRefinanceApplicationCreationModel(Application application);

        ApplicationCreationModel PopulateApplicationCreationModel(Application comcorpApplication, OfferType applicationType);
    }
}