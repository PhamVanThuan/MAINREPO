using WatiN.Core;

namespace ObjectMaps
{
    public abstract class AdminPaymentDistributionAgentLegalEntityViewControls : Page
    {
         [FindBy(Id="ctl00_Main_SelectAddress")]
         protected Button ctl00MainSelectAddress { get; set; }

         [FindBy(Id = "ctl00_Main_btnCancelButton")]   
         protected Button ctl00MainbtnCancelButton { get; set; }

         [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblLEType")] 
         protected Span lblLEType { get; set; }

         [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblOrganisationTypeDisplay")]
         protected Span lblOrganisationTypeDisplay { get; set; }

         [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblCORegisteredName")]
         protected Span lblCORegisteredName { get; set; }

         [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblCORegistrationNumber")]
         protected Span lblCORegistrationNumber { get; set; }

         [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblRole")]
         protected Span lblRole { get; set; }

         [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblNatIntroductionDate")]
         protected Span lblNatIntroductionDate { get; set; }

         [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblIDNumber")]
         protected Span lblIDNumber { get; set; }

         [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblSalutation")]
         protected Span lblSalutation { get; set; }

         [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblInitials")]
         protected Span lblInitials { get; set; }

         [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblFirstNames")]
         protected Span lblFirstNames { get; set; }

         [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblSurname")]   
         protected Span lblSurname { get; set; }

         [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblPreferredName")]    
         protected Span lblPreferredName { get; set; }

         [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblGender")]
         protected Span lblGender { get; set; }

         [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblStatus")]
         protected Span lblStatus { get; set; }

         [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblHomePhone")]
         protected Span lblHomePhone { get; set; }

         [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblWorkPhone")]
         protected Span lblWorkPhone { get; set; }

         [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblFaxNumber")]
         protected Span lblFaxNumber { get; set; }

         [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblCellphoneNumber")]
         protected Span lblCellphoneNumber { get; set; }

         [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblEmailAddress")]
         protected Span lblEmailAddress { get; set; }						
    }
}