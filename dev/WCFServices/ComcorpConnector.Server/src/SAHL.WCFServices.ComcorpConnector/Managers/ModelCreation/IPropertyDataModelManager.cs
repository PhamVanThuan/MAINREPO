using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation
{
    public interface IPropertyDataModelManager
    {
        ComcorpApplicationPropertyDetailsModel PopulateComcorpPropertyDetails(string occupancyType, string propertyType, string titleType, string sectionalTitleUnitNumber,
            string namePropertyRegistered, Property comcorpProperty);
    }
}
