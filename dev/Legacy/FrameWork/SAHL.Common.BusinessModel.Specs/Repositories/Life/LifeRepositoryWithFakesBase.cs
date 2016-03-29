using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace SAHL.Common.BusinessModel.Specs.Repositories.Life
{
    public class LifeRepositoryWithFakesBase : WithFakes
    {
        protected static ILifeRepository lifeRepository;

        public LifeRepositoryWithFakesBase()
        {
            lifeRepository = An<ILifeRepository>();
        }
    }
}
