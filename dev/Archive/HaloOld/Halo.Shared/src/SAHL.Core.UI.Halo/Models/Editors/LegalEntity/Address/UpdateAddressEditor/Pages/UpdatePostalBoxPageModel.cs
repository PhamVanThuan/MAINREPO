using SAHL.Core.Data;
using SAHL.Core.UI.Halo.Editors.Address.Common.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.UI.Halo.Editors.Address.UpdateAddressEditor.Pages
{
    public class UpdatePostalBoxPageModel : PostalBoxPageModel, ISqlStatement<UpdatePostalBoxPageModel>
    {
        public override void Initialise(BusinessModel.BusinessContext businessContext)
        {
            this.LegalEntityAddressKey = businessContext.BusinessKey.Key;

            using (var db = new Db().InReadOnlyAppContext())
            {
                db.SelectOneInto(this, this);
            }
        }

        public long LegalEntityAddressKey { get;  set; }

        public string GetStatement()
        {
            return @"select addr.BoxNumber, po.Description as PostOffice,
                lea.EffectiveDate, 
                Case lea.GeneralStatusKey when 1 then 1 else 0 end as SelectedAddressStatusKey,
                addr.RRR_SuburbDescription as Suburb 
            from [2am].dbo.legalentityaddress lea
            inner join
                [2am].dbo.addresstype at on at.addresstypekey = lea.addresstypekey
            inner join
                [2am].dbo.[address] addr on addr.addresskey = lea.addresskey
            inner join
                [2am].dbo.addressformat af on af.addressformatkey = addr.addressformatkey
            inner join 
                [2am].dbo.PostOffice po on po.PostOfficeKey = addr.PostOfficeKey
            where 
                lea.legalentityaddresskey = @LegalEntityAddressKey";
        }
    }
}
