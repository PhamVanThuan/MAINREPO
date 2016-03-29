using SAHL.Core.Data;
using SAHL.Core.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.UI.Halo.Editors.Address.AcceptDomiciliumEditor.Pages
{
    public class AcceptDomiciliumPageModel : IEditorPageModel, ISqlStatement<AcceptDomiciliumPageModel>
    {
        public long AddressKey { get; set; }
        public string Message { get; set; }
        
        public void Initialise(BusinessModel.BusinessContext businessContext)
        {
            AddressKey = businessContext.BusinessKey.Key;
            using (var db = new Db().InReadOnlyAppContext())
            {
                db.SelectOneInto(this, this);
            }
        }

        public string GetStatement()
        {
            return string.Format(@"SELECT
	                                    CASE
		                                    WHEN ISNULL(led.GeneralStatusKey, 0) = 3
		                                    THEN 'This will activate the domicilium address.'
		                                    WHEN ISNULL(led.GeneralStatusKey, 0) = 1
                                            THEN 'This address is already a domicilium address.'
                                            ELSE 'You cannot activate an address that is not a pending domicilium address.'
	                                    END AS [Message]
                                    FROM [2AM].dbo.LegalEntityAddress lea 
                                    LEFT JOIN [2AM].dbo.LegalEntityDomicilium led on lea.LegalEntityAddresskey = led.LegalEntityAddressKey
                                    WHERE lea.LegalEntityAddressKey = @AddressKey");
        }
    }
}
