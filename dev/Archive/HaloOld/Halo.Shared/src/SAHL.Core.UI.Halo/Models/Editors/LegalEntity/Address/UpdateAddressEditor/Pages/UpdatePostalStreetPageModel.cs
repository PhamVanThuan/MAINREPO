using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Data;
using SAHL.Core.UI.Halo.Editors.Address.Common.Pages;

namespace SAHL.Core.UI.Halo.Editors.Address.UpdateAddressEditor.Pages
{
    public class UpdatePostalStreetPageModel : PostalStreetPageModel, ISqlStatement<UpdatePostalStreetPageModel>
    {
        public override void Initialise(BusinessModel.BusinessContext businessContext)
        {
            this.LegalEntityAddressKey = businessContext.BusinessKey.Key;

            using (var db = new Db().InReadOnlyAppContext())
            {
                db.SelectOneInto(this, this);
            }
        }

        public long LegalEntityAddressKey { get; protected set; }

        public string GetStatement()
        {
            return @"SELECT addr.UnitNumber,
                    addr.BuildingNumber AS BuildingNo,
                    addr.StreetName,
                    addr.StreetNumber AS StreetNo,
                    addr.BuildingName,
                    lea.AddressKey,
                    lea.EffectiveDate,
                    CASE lea.GeneralStatusKey
                        WHEN 1
                            THEN 1
                        ELSE 0
                        END AS SelectedAddressStatusKey,
                    addr.RRR_SuburbDescription AS Suburb,
                    addr.RRR_CityDescription AS City,
                    c.provinceKey AS SelectedProvinceKey,
                    addr.RRR_PostalCode AS PostalCode
                FROM [2am].dbo.legalentityaddress lea
                INNER JOIN 
                    [2am].dbo.addresstype at ON at.addresstypekey = lea.addresstypekey
                INNER JOIN 
                    [2am].dbo.[address] addr ON addr.addresskey = lea.addresskey
                INNER JOIN 
                    [2am].dbo.addressformat af ON af.addressformatkey = addr.addressformatkey
                INNER JOIN 
                    [2am].dbo.suburb s ON s.suburbkey = addr.suburbkey
                INNER JOIN 
                    [2am].dbo.city c ON c.citykey = s.citykey
                WHERE lea.legalentityaddresskey = @LegalEntityAddressKey";
        }
    }
}
