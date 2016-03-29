using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Data;

namespace SAHL.Services.FrontEndTest.Managers.Statements
{
    public class GetFirstThirdPartyInvoiceWithDocumentStatement : ISqlStatement<string>
    {
        public string GetStatement()
        {
            return @"declare @imageIndexData table(ID numeric, STOR numeric, GUID varchar(38), FilePath varchar(8000), FileExists int )

                    insert into @imageIndexData
                    SELECT d.[ID]
                          ,d.[STOR]
                          ,d.[GUID]
                          ,STOR.Folder+'\'+convert(varchar(4), CAST(CAST(archiveDate as datetime) as datetime), 111)+'\'+convert(varchar(2), 
                                           CAST(CAST(archiveDate as datetime) as datetime), 101)+'\'+ CAST(GUID as varchar(255)) as FilePath,
                           0
                    FROM 
                          [ImageIndex].[dbo].[Data] d
                          join [ImageIndex].dbo.STOR on d.STOR = STOR.ID
                    WHERE 
                          STOR = 44
                    ORDER BY NEWID()

                    declare @id numeric, @filepath varchar(8000), @result int

                    DECLARE checkFileExists CURSOR
                    FOR SELECT id, FilePath FROM @imageIndexData
                    OPEN checkFileExists
                    FETCH NEXT FROM checkFileExists INTO @id, @filepath
                    WHILE @@FETCH_STATUS = 0
                    BEGIN
                        EXEC master.dbo.xp_fileexist @filepath, @result OUTPUT
                        update @imageIndexData set FileExists = @result where id =@id
                        FETCH NEXT FROM checkFileExists INTO @id, @filepath
                    END 
                    CLOSE checkFileExists;
                    DEALLOCATE checkFileExists;

                    select top 1 guid from @imageIndexData where FileExists = 1 order by NewID()";
        }

    }
}
