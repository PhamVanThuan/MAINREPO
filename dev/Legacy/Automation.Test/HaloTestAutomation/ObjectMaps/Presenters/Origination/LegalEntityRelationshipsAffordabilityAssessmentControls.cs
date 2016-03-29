using ObjectMaps.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatiN.Core;

namespace ObjectMaps.Presenters.Origination
{
    public abstract class LegalEntityRelationshipsAffordabilityAssessmentControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_ddlRelationshipType")]
        protected SelectList ddlRelationshipType { get; set; }

        [FindBy(Id = "ctl00_Main_btnSubmitButton")]
        protected Button btnAdd { get; set; }
    }
}
