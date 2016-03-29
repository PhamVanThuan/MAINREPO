using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.PropertyDomain.Commands;
using SAHL.Services.Interfaces.PropertyDomain.Events;
using SAHL.Services.PropertyDomain.Managers;
using System;

namespace SAHL.Services.PropertyDomain.CommandHandlers
{
    public class AddComcorpOfferPropertyDetailsCommandHandler : IServiceCommandHandler<AddComcorpOfferPropertyDetailsCommand>
    {
        private IEventRaiser eventRaiser;
        private IPropertyDataManager propertyDataManager;

        public AddComcorpOfferPropertyDetailsCommandHandler(IPropertyDataManager propertyDataManager, IEventRaiser eventRaiser)
        {
            this.propertyDataManager = propertyDataManager;
            this.eventRaiser = eventRaiser;
        }

        public ISystemMessageCollection HandleCommand(AddComcorpOfferPropertyDetailsCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            ComcorpOfferPropertyDetailsDataModel comcorpOfferPropertyDetailsDataModel = propertyDataManager.FindExistingComcorpOfferPropertyDetails(command.ApplicationNumber);
            if (comcorpOfferPropertyDetailsDataModel == null)
            {
                comcorpOfferPropertyDetailsDataModel = new ComcorpOfferPropertyDetailsDataModel
                    (
                    command.ApplicationNumber,
                    command.ComcorpOfferPropertyDetails.SellerIDNo,
                    command.ComcorpOfferPropertyDetails.SAHLOccupancyType,
                    command.ComcorpOfferPropertyDetails.SAHLPropertyType,
                    command.ComcorpOfferPropertyDetails.SAHLTitleType,
                    command.ComcorpOfferPropertyDetails.SectionalTitleUnitNo,
                    command.ComcorpOfferPropertyDetails.ComplexName,
                    command.ComcorpOfferPropertyDetails.StreetNo,
                    command.ComcorpOfferPropertyDetails.StreetName,
                    command.ComcorpOfferPropertyDetails.Suburb,
                    command.ComcorpOfferPropertyDetails.City,
                    command.ComcorpOfferPropertyDetails.Province,
                    command.ComcorpOfferPropertyDetails.PostalCode,
                    command.ComcorpOfferPropertyDetails.ContactCellphone,
                    command.ComcorpOfferPropertyDetails.ContactName,
                    command.ComcorpOfferPropertyDetails.NamePropertyRegistered,
                    command.ComcorpOfferPropertyDetails.StandErfNo,
                    command.ComcorpOfferPropertyDetails.PortionNo,
                    DateTime.Now,
                    null
                    );

                propertyDataManager.InsertComcorpOfferPropertyDetails(comcorpOfferPropertyDetailsDataModel);
            }
            else
            {
                comcorpOfferPropertyDetailsDataModel.SellerIDNo = command.ComcorpOfferPropertyDetails.SellerIDNo;
                comcorpOfferPropertyDetailsDataModel.SAHLOccupancyType = command.ComcorpOfferPropertyDetails.SAHLOccupancyType;
                comcorpOfferPropertyDetailsDataModel.SAHLPropertyType = command.ComcorpOfferPropertyDetails.SAHLPropertyType;
                comcorpOfferPropertyDetailsDataModel.SAHLTitleType = command.ComcorpOfferPropertyDetails.SAHLTitleType;
                comcorpOfferPropertyDetailsDataModel.SectionalTitleUnitNo = command.ComcorpOfferPropertyDetails.SectionalTitleUnitNo;
                comcorpOfferPropertyDetailsDataModel.ComplexName = command.ComcorpOfferPropertyDetails.ComplexName;
                comcorpOfferPropertyDetailsDataModel.StreetNo = command.ComcorpOfferPropertyDetails.StreetNo;
                comcorpOfferPropertyDetailsDataModel.StreetName = command.ComcorpOfferPropertyDetails.StreetName;
                comcorpOfferPropertyDetailsDataModel.Suburb = command.ComcorpOfferPropertyDetails.Suburb;
                comcorpOfferPropertyDetailsDataModel.City = command.ComcorpOfferPropertyDetails.City;
                comcorpOfferPropertyDetailsDataModel.Province = command.ComcorpOfferPropertyDetails.Province;
                comcorpOfferPropertyDetailsDataModel.PostalCode = command.ComcorpOfferPropertyDetails.PostalCode;
                comcorpOfferPropertyDetailsDataModel.ContactCellphone = command.ComcorpOfferPropertyDetails.ContactCellphone;
                comcorpOfferPropertyDetailsDataModel.ContactName = command.ComcorpOfferPropertyDetails.ContactName;
                comcorpOfferPropertyDetailsDataModel.NamePropertyRegistered = command.ComcorpOfferPropertyDetails.NamePropertyRegistered;
                comcorpOfferPropertyDetailsDataModel.StandErfNo = command.ComcorpOfferPropertyDetails.StandErfNo;
                comcorpOfferPropertyDetailsDataModel.PortionNo = command.ComcorpOfferPropertyDetails.PortionNo;
                comcorpOfferPropertyDetailsDataModel.ChangeDate = DateTime.Now;

                propertyDataManager.UpdateComcorpOfferPropertyDetails(comcorpOfferPropertyDetailsDataModel);
            }

            var comcorpPropertyAddedEvent = new ComcorpOfferPropertyDetailsAddedEvent(DateTime.Now, command.ComcorpOfferPropertyDetails.SellerIDNo,
                command.ComcorpOfferPropertyDetails.SAHLOccupancyType, command.ComcorpOfferPropertyDetails.SAHLPropertyType, command.ComcorpOfferPropertyDetails.SAHLTitleType,
                command.ComcorpOfferPropertyDetails.SectionalTitleUnitNo, command.ComcorpOfferPropertyDetails.ComplexName, command.ComcorpOfferPropertyDetails.StreetNo,
                command.ComcorpOfferPropertyDetails.StreetName, command.ComcorpOfferPropertyDetails.Suburb, command.ComcorpOfferPropertyDetails.City, command.ComcorpOfferPropertyDetails.Province,
                command.ComcorpOfferPropertyDetails.PostalCode, command.ComcorpOfferPropertyDetails.ContactCellphone, command.ComcorpOfferPropertyDetails.ContactName,
                command.ComcorpOfferPropertyDetails.NamePropertyRegistered, command.ComcorpOfferPropertyDetails.StandErfNo, command.ComcorpOfferPropertyDetails.PortionNo);
            eventRaiser.RaiseEvent(DateTime.Now, comcorpPropertyAddedEvent, command.ApplicationNumber, (int)GenericKeyType.ComcorpOfferPropertyDetails, metadata);

            return messages;
        }
    }
}