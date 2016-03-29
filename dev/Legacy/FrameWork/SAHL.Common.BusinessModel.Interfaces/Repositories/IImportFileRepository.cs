using System;
using System.Collections.Generic;
using System.Data;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IImportFileRepository
    {
        IReadOnlyEventList<IImportFile> GetImportHistory();

        IReadOnlyEventList<IImportLegalEntity> GetImportResultsByFileKey(int fileKey);

        string GetRCSUploadExportPath();

        Boolean ClearRCSUploadExportPath(string uploadExportPath);

        void LoadRCS_CSVFile(string uploadPath, string uploadFile, ref DataTable DT, ref IXsdAbstraction xsdAbs, ref List<string> missingColumns, ref List<string> extraColumns, ref Dictionary<string, List<string>> invalidData);

        string GenerateXML(DataTable DT, ref List<string> missingColumns, ref List<string> extraColumns, ref Dictionary<string, List<string>> invalidData);

        int ImportDataFromXML(string xmlFileName, IImportFile importFile);

        ISimpleNode GetSimpleNode(string InvalidType, IXsdAbstraction xsdAbs);

        IImportFile CreateEmptyImportFile();
    }
}