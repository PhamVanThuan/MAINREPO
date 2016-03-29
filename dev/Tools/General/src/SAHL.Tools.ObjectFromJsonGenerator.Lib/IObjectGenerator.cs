using System;
namespace SAHL.Tools.ObjectFromJsonGenerator.Lib
{
    public interface IObjectGenerator
    {
        System.Collections.Generic.Dictionary<string, string> GenerateLatestDebugClasses(string connectionString, string MsgSetVersion, string VarSetVersion, string EnumSetVersion);
        void GenerateObjects(string connectionString, string outputPath, string MsgSetVersion, string VarSetVersion, string EnumSetVersion, string defaultNamespace, string buildMode = "Debug");
    }
}
