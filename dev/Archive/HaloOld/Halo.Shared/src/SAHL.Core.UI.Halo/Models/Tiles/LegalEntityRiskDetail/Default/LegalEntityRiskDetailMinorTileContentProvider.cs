using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Providers.Tiles;

namespace SAHL.Core.UI.Halo.Tiles.LegalEntityRiskDetail.Default
{
    public class LegalEntityRiskDetailMinorTileContentProvider : AbstractSqlTileContentProvider<LegalEntityRiskDetailMinorTileModel>
    {
        public override string GetStatement(BusinessKey businessKey)
        {
            return string.Format(@";
                    WITH XMLNAMESPACES('https://secure.transunion.co.za/TUBureau' AS ""TUBureau"")
                    SELECT TOP 1 
                    itc.ChangeDate AS LastITCDate,
                    	CONVERT(VARCHAR(10), 
                    		itc.ResponseXML.query(N'BureauResponse/TUBureau:EmpiricaEM07/TUBureau:EmpiricaScore/text()')
                    	) AS EmpiricaScore,
                    	ISNULL(ab.BehaviouralScore, 0) AS BaselScore
                    FROM [2am].dbo.OfferRole ofr 
                    JOIN [2am].dbo.LegalEntity le (NOLOCK) ON le.LegalEntityKey = ofr.LegalEntityKey
                    JOIN [2am].dbo.ITC itc (NOLOCK) ON itc.LegalEntityKey = le.LegalEntityKey
                    LEFT JOIN [2am].dbo.[Role] r ON le.LegalEntityKey = r.LegalEntityKey
                    LEFT JOIN [2am].dbo.AccountBaselII ab (NOLOCK) ON r.AccountKey = ab.AccountKey
                    WHERE 
                    ISNUMERIC(CONVERT(VARCHAR(10), ResponseXML.query(N'BureauResponse/TUBureau:EmpiricaEM07/TUBureau:EmpiricaScore/text()'))) = 1
                    AND ofr.LegalEntityKey = {0}
                    ORDER BY itc.ChangeDate DESC", businessKey.Key);
        }
    }
}