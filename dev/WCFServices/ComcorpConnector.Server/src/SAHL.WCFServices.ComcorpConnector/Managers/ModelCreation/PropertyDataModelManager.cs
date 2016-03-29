using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Objects;
using System;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation
{
    public class PropertyDataModelManager : IPropertyDataModelManager
    {
        public ComcorpApplicationPropertyDetailsModel PopulateComcorpPropertyDetails(string occupancyType, string propertyType, string titleType, string sectionalTitleUnitNumber,
            string namePropertyRegistered, Property comcorpProperty)
        {
            ComcorpApplicationPropertyDetailsModel propertyModel = new ComcorpApplicationPropertyDetailsModel(
                    comcorpProperty.SellerIDNo.Trim(), occupancyType.Trim(), propertyType.Trim(), titleType.Trim(), sectionalTitleUnitNumber,
                    comcorpProperty.ComplexName, comcorpProperty.StreetNo, comcorpProperty.StreetName, comcorpProperty.AddressSuburb,
                    comcorpProperty.AddressCity, comcorpProperty.Province, comcorpProperty.PostalCode, comcorpProperty.ContactCellphone,
                    comcorpProperty.ContactName, namePropertyRegistered, comcorpProperty.StandErfNo,  comcorpProperty.PortionNo
                );
            return propertyModel;
        }
    }
}