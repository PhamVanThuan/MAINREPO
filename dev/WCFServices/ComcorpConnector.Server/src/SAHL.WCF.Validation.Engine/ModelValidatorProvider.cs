﻿using System.Collections.Generic;

namespace SAHL.WCF.Validation.Engine
{
    public abstract class ModelValidatorProvider
    {
        public abstract IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata);
    }
}
