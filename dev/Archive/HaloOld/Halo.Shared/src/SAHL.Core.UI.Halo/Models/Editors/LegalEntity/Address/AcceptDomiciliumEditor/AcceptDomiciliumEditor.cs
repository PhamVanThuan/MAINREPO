using SAHL.Core.Data;
using SAHL.Core.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.UI.Halo.Editors.Address.AcceptDomiciliumEditor
{
    public class AcceptDomiciliumEditor : IEditor, ISqlStatement<AcceptDomiciliumEditor>
    {
        public long LegalEntityAddressKey { get; set; }
        public bool Valid { get; set; }

        public void Initialise(BusinessModel.BusinessContext businessContext)
        {
            this.LegalEntityAddressKey = businessContext.BusinessKey.Key;
        }

        public IEnumerable<IUIValidationResult> SubmitEditor(IEnumerable<IEditorPageModel> submittedPageModels)
        {
            if (Valid)
            {
                //Activate the domicilium address
            }
            throw new NotImplementedException();
        }

        public string GetStatement()
        {
            return string.Format(@"SELECT
	                                    CASE
		                                    WHEN ISNULL(led.GeneralStatusKey, 0) = 3
		                                    THEN true
		                                    ELSE false
	                                    END AS Valid
                                    FROM [2AM].dbo.LegalEntityAddress lea 
                                    LEFT JOIN [2AM].dbo.LegalEntityDomicilium led on lea.LegalEntityAddresskey = led.LegalEntityAddressKey
                                    WHERE lea.LegalEntityAddressKey = @AddressKey");
        }
    }
}
