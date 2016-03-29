using System;
using System.Collections.Generic;
using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Halo.Editors.LegalEntity.Address.UpdateAddressEditor
{
    public class UpdateAddressEditor : IEditor, ISqlStatement<UpdateAddressEditor>
    {
        public void Initialise(BusinessContext businessContext)
        {
            this.LegalEntityAddressKey = businessContext.BusinessKey.Key;

            using (var db = new Db().InReadOnlyAppContext())
            {
                db.SelectOneInto<UpdateAddressEditor>(this, this);
            }
        }

        public long LegalEntityAddressKey { get; set; }

        public int AddressKey { get; set; }

        public string AddressType { get; set; }

        public int AddressTypeKey { get; set; }

        public string AddressFormat { get; set; }

        public int AddressFormatKey { get; set; }

        public DateTime EffectiveDate { get; set; }

        public int IsActive { get; set; }

        public string GetStatement()
        {
            return @"select lea.AddressKey, at.Description as AddressType, at.AddressTypeKey, af.Description as AddressFormat, af.AddressFormatKey, lea.EffectiveDate, Case lea.GeneralStatusKey when 1 then 1 else 0 end as IsActive from [2am].dbo.legalentityaddress lea
                    inner join
	                    [2am].dbo.addresstype at on at.addresstypekey = lea.addresstypekey
                    inner join
	                    [2am].dbo.[address] addr on addr.addresskey = lea.addresskey
                    inner join
	                    [2am].dbo.addressformat af on af.addressformatkey = addr.addressformatkey
                    where
	                    lea.legalentityaddresskey = @LegalEntityAddressKey";
        }

        public IEnumerable<IUIValidationResult> SubmitEditor(IEnumerable<IEditorPageModel> submittedPageModels)
        {
            throw new NotImplementedException();
        }
    }
}