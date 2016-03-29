using System.Collections.Generic;
using System.Linq;
using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Halo.Editors.Address.ChangeDomiciliumAddressEditor.Pages
{
    public class ChangeDomiciliumAddressEditorPage1Model : IEditorPageModel, ISqlStatement<ChangeDomiciliumAddressEditorPage1Model>
    {
        private long LegalEntityAddressKey { get; set; }

        public string CurrentDomiciliumAddress { get; set; }

        public string ElectedDomiciliumAddress { get; set; }

        public string FaxNumber { get; set; }

        public string EmailAddress { get; set; }

        public IEnumerable<KeyValuePair<int, string>> PostalAddresses { get; set; }

        public void Initialise(BusinessContext businessContext)
        {
            LegalEntityAddressKey = businessContext.BusinessKey.Key;

            using (var db = new Db().InReadOnlyAppContext())
            {
                db.SelectOneInto(this, this);

                PostalAddresses = db.Select<KeyValuePair<int, string>>(string.Format(@"SELECT lea2.AddressKey as [Key], [2am].dbo.fGetFormattedAddressDelimited(lea2.AddressKey, 0) as [Value]
                                                                    FROM [2am].dbo.LegalEntityAddress lea1 (NOLOCK)
                                                                    JOIN [2am].dbo.LegalEntityAddress lea2 (NOLOCK) ON lea2.LegalEntityKey = lea1.LegalEntityKey
	                                                                    AND lea2.AddressTypeKey = 2 -- going postal
                                                                    WHERE lea1.LegalEntityAddressKey = {0}", LegalEntityAddressKey)).ToDictionary(x => x.Key, x => x.Value);
            }
        }

        public string GetStatement()
        {
            return string.Format(@"SELECT TOP 1 [2AM].dbo.fGetFormattedAddressDelimited(lea.AddressKey, 0) as ElectedDomiciliumAddress,
		                                    CASE WHEN led.DomiciliumAddressKey IS NOT NULL THEN [2AM].dbo.fGetFormattedAddressDelimited(led.DomiciliumAddressKey, 0) ELSE 'None' END as CurrentDomiciliumAddress,
		                                    le.FaxCode + '-' + le.FaxNumber as FaxNumber, le.EmailAddress
                                    FROM [2am].dbo.LegalEntityAddress lea (NOLOCK)
                                    JOIN [2am].dbo.LegalEntity le (NOLOCK) ON le.LegalEntityKey = lea.LegalEntityKey
                                    LEFT JOIN (SELECT TOP 1 lea.AddressKey as DomiciliumAddressKey, lea.LegalEntityKey
		                                    FROM [2am].dbo.LegalEntityAddress lea (NOLOCK)
		                                    JOIN [2AM].dbo.LegalEntityDomicilium led (NOLOCK) ON led.LegalEntityAddressKey = lea.LegalEntityAddressKey
			                                    AND led.GeneralStatusKey = 1) led ON led.LegalEntityKey = lea.LegalEntityKey
                                    WHERE LegalEntityAddressKey = {0}", LegalEntityAddressKey);
        }
    }
}