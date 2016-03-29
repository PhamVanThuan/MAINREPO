using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.CSharp;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.Misc;

namespace SAHL.X2Designer.CodeGen
{
    public interface IRemoteCompile
    {
        string BaseDomainPath { get; set; }

        List<string> UsingStatements { get; set; }

        void AddReferenceDLL(List<ReferenceItem> References);

        void AddSystemReference(List<string> SystemReferences);

        CompilerResults Compile(string Code, string OutputFileName);

        void ExecuteMethod(string MethodName, string TypeName);

        List<ReferenceItem> LoadAssembly(string AssemblyName, string BinaryType, List<ReferenceItem> AlreadyLoaded);

        ReferenceItem LoadGlobalAssembly(string GlobalAssemblyName);

        ReferenceItem LoadAndCopyAssembly(string PathToAssembly);

        List<Type> GetAllTypesInDomain(List<string> UsingStatements);
    }

    [Serializable]
    public class RemoteException : Exception
    {
        public RemoteException() : base() { }

        public RemoteException(string Message) : base(Message) { }

        public RemoteException(string Message, Exception inner) : base(Message, inner) { }

        public RemoteException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [Serializable]
    public class RemoteCompile : IRemoteCompile
    {
        private List<string> KnownSystemDlls = new List<string>();
        private string _BaseDomainPath = "";

        public string BaseDomainPath { get { return _BaseDomainPath; } set { _BaseDomainPath = value; } }

        List<string> ReferenceDLL = new List<string>();
        private List<string> _UsingStatements = new List<string>();

        public RemoteCompile()
        {
            KnownSystemDlls.Add("mscorlib.dll");
            KnownSystemDlls.Add("System.dll");
            KnownSystemDlls.Add("System.Data.dll");
            KnownSystemDlls.Add("System.Xml.dll");
            KnownSystemDlls.Add("System.Configuration.dll");
        }

        public void AddSystemReference(List<string> SystemReferences)
        {
            for (int i = 0; i < SystemReferences.Count; i++)
            {
                if (!ReferenceDLL.Contains(SystemReferences[i]))
                {
                    if (!SystemReferences[i].EndsWith(".dll"))
                        ReferenceDLL.Add(SystemReferences[i] + ".dll");
                    else
                        ReferenceDLL.Add(SystemReferences[i]);
                }
            }
        }

        public void AddReferenceDLL(List<ReferenceItem> References)
        {
            for (int i = 0; i < References.Count; i++)
            {
                if (References[i].isGlobal)
                {
                    this.ReferenceDLL.Add(References[i] + ".dll");
                }
                else
                {
                    if (!ReferenceDLL.Contains(References[i].Name + ".dll"))
                    {
                        string FilePath = "";
                        string FileName = Path.GetFileName(References[i].FullPath);
                        if (MainForm.App == null)
                        {
                            FilePath = Path.GetDirectoryName(References[i].FullPath);
                        }
                        else
                        {
                            FilePath = Helpers.GetBinaryFolderPath(References[i].BinaryType, References[i].FullPath);
                        }

                        string BuildFilePath = string.Format("{0}\\{1}", FilePath, FileName);
                        this.ReferenceDLL.Add(BuildFilePath);
                    }
                }
            }
        }

        public List<string> UsingStatements
        {
            get
            {
                return _UsingStatements;
            }
            set
            {
                _UsingStatements = value;
            }
        }

        public CompilerResults Compile(string Code, string OutputFileName)
        {
            string Path = "";
            try
            {
                string CurrentFolder = AppDomain.CurrentDomain.BaseDirectory;
                if (!Directory.Exists(CurrentFolder + "\\Build"))
                    Directory.CreateDirectory(CurrentFolder + "\\Build");

                CSharpCodeProvider csc = new CSharpCodeProvider();
                CompilerParameters par = new CompilerParameters();
                par.GenerateExecutable = false;
                par.GenerateInMemory = false;
                par.IncludeDebugInformation = true;
                par.OutputAssembly = this._BaseDomainPath + "\\" + OutputFileName;
                par.TempFiles = new TempFileCollection(this._BaseDomainPath, false);
                for (int i = 0; i < ReferenceDLL.Count; i++)
                {
                    string ReferenceName = ReferenceDLL[i];
                    bool FoundMatch = false;
                    for (int j = 0; j < par.ReferencedAssemblies.Count; j++)
                    {
                        string AddedRef = par.ReferencedAssemblies[j];
                        if (AddedRef.EndsWith(ReferenceName) || (string.Compare(AddedRef, ReferenceName) == 0))
                        {
                            // we already have this asm referenced so dont add again
                            FoundMatch = true;
                            continue;
                        }
                    }
                    if (!FoundMatch)
                    {
                        try
                        {
                            Assembly.LoadFrom(ReferenceDLL[i]);
                            par.ReferencedAssemblies.Add(ReferenceDLL[i]);
                        }
                        catch (FileNotFoundException)
                        {
                            if (!ReferenceDLL[i].StartsWith("System"))
                            {
                                if ((!ReferenceDLL[i].Contains("Microsoft.Practices")))
                                {
                                    Path = BaseDomainPath + "\\" + ReferenceDLL[i];
                                    try
                                    {
                                        Assembly.LoadFile(Path);
                                        par.ReferencedAssemblies.Add(Path);
                                    }
                                    catch
                                    {
                                        int a = 0;
                                    }
                                }
                            }
                            else
                            {
                                if (!par.ReferencedAssemblies.Contains(ReferenceDLL[i]))
                                    par.ReferencedAssemblies.Add(ReferenceDLL[i]);
                            }
                        }
                        catch (Exception ex)
                        {
                            int a = 0;
                            throw;
                        }
                    }
                }

                CompilerResults res = csc.CompileAssemblyFromSource(par, Code);
                if (res.Errors.HasErrors)
                {
                    string s = res.Errors[0].ToString();
                    return res;
                }
                return res;
            }
            catch (Exception ex)
            {
                throw new RemoteException("Error During Compile", ex);
            }
            return null;
        }

        public void ExecuteMethod(string MethodName, string TypeName)
        {
            Assembly a = Assembly.LoadFile(@"C:\Development\RnD\RemoteAppdomainCompile\RemoteAppdomainCompile\bin\Debug\Build\Process.dll");
            Type[] tt = a.GetExportedTypes();
            foreach (Type t in tt)
            {
                object Test = Activator.CreateInstance(t);
                if (t.Name == TypeName)
                    t.InvokeMember(MethodName, BindingFlags.Default | BindingFlags.InvokeMethod, null, Test, null, null);
            }
        }

        public List<ReferenceItem> LoadAssembly(string AssemblyName, string BinaryType, List<ReferenceItem> AlreadyLoaded)
        {
            Dictionary<string, ReferenceItem> References = new Dictionary<string, ReferenceItem>();
            try
            {
                string FileName = Path.GetFileName(AssemblyName);
                string FilePath = "";

                FilePath = Path.GetDirectoryName(AssemblyName);
                string FileNameNoExt = Path.GetFileNameWithoutExtension(AssemblyName);
                string BuildFolder = _BaseDomainPath;

                // copy in the assembly.
                // look at the references and copy in IF version is newer than existing file or if file does not exist.
                try
                {
                    //Assembly asm = Assembly.ReflectionOnlyLoadFrom(AssemblyName);
                    Assembly asm = Assembly.LoadFrom(AssemblyName);
                    string BuildFilePath = string.Format("{0}\\{1}", FilePath, FileName);

                    ReferenceItem ri = new ReferenceItem(asm.FullName, BuildFilePath, false, true, BinaryType);

                    if (BinaryType == "none")
                    {
                        // check if it exists .. only copy it in IF its of a newer version
                        if (File.Exists((FilePath + "\\" + FileName)))
                        {
                            try
                            {
                                Assembly asmTmp = Assembly.ReflectionOnlyLoadFrom((_BaseDomainPath + "\\" + FileName));
                                string tmpVer = ReferenceItem.ReturnVersion(asmTmp.FullName);
                                if (new VersionComparer().Compare(tmpVer, ri.Version) > 0)
                                {
                                    File.Copy(AssemblyName, (_BaseDomainPath + "\\" + FileName), true);
                                }
                            }
                            catch (Exception ex)
                            {
                                string s = ex.ToString();
                            }
                        }
                        else
                        {
                            File.Copy(AssemblyName, (_BaseDomainPath + "\\" + FileName), true);
                        }
                    }

                    // This is no longer needed, because the Assemblies are referenced externally, and no longer encapsulated
                    // CopyReferencedAssemblies(asm, FilePath, References, ri);

                    ExtractUsingStatement(asm);
                    //ri.BinaryType
                    References.Add(ri.FullName, ri);

                    ReferenceItem[] rr = new ReferenceItem[References.Count];
                    References.Values.CopyTo(rr, 0);
                    List<ReferenceItem> Ref = new List<ReferenceItem>(rr);
                    return Ref;
                }
                catch (Exception ex)
                {
                    // wasnt loaded from GAC so copy and load references.
                    string WHy = "";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }

        public ReferenceItem LoadAndCopyAssembly(string AssemblyPath)
        {
            try
            {
                string FileName = Path.GetFileName(AssemblyPath);
                string FilePath = Path.GetDirectoryName(AssemblyPath);
                string FileNameNoExt = Path.GetFileNameWithoutExtension(AssemblyPath);
                string BuildFolder = _BaseDomainPath;

                Assembly asm = Assembly.LoadFrom(AssemblyPath);
                if (!File.Exists(_BaseDomainPath + "\\" + FileName))
                    File.Copy(AssemblyPath, (_BaseDomainPath + "\\" + FileName), true);
                ReferenceItem ri = new ReferenceItem(asm.FullName, FilePath, true, true, "none");
                return ri;
            }
            catch (Exception ex)
            {
                string WHy = "";
            }
            return null;
        }

        public ReferenceItem LoadGlobalAssembly(string GlobalAssemblyName)
        {
            ReferenceItem ri = null;
            SAHL.X2Designer.Misc.GlobalAssemblyCache.IAssemblyName m_AssemblyName;
            SAHL.X2Designer.Misc.GlobalAssemblyCache.AssemblyCache.CreateAssemblyCache();
            SAHL.X2Designer.Misc.GlobalAssemblyCache.IAssemblyEnum assemblyEnumerator = SAHL.X2Designer.Misc.GlobalAssemblyCache.AssemblyCache.CreateGACEnum();
            while (GlobalAssemblyCache.AssemblyCache.GetNextAssembly(assemblyEnumerator, out m_AssemblyName) == 0)
            {
                if (GlobalAssemblyCache.AssemblyCache.GetName(m_AssemblyName) == GlobalAssemblyName)
                {
                    string AsmName = GlobalAssemblyCache.AssemblyCache.GetName(m_AssemblyName);
                    try
                    {
                        Assembly asmTmp = Assembly.LoadWithPartialName(AsmName);
                        ri = new ReferenceItem(asmTmp.FullName, asmTmp.Location, true, true, "none");
                        return ri;
                    }
                    catch (Exception ex)
                    {
                        string s = ex.ToString();
                    }
                }
            }
            return ri;
        }

        private void CopyReferencedAssemblies(Assembly MainAsm, string MainAsmLocationPreCopy,
          Dictionary<string, ReferenceItem> References, ReferenceItem riParent)
        {
            try
            {
                AssemblyName[] names = MainAsm.GetReferencedAssemblies();
                for (int i = 0; i < names.Length; i++)
                {
                    Assembly asm = null;
                    try
                    {
                        string ReferenceName = string.Format("{0}.dll", names[i].Name);
                        if (IsSystemDll(ReferenceName)) continue;
                        asm = Assembly.LoadFrom(ReferenceName);

                        string OrigFilePath = string.Format("{0}\\{1}", MainAsmLocationPreCopy, ReferenceName);
                        string BuildFilePath = string.Format("{0}\\{1}", _BaseDomainPath, ReferenceName);
                        ReferenceItem ri = new ReferenceItem(asm.FullName, BuildFilePath, false, false, "none");

                        if (File.Exists(BuildFilePath))
                        {
                            // check if it exists .. only copy it in IF its of a newer version
                            try
                            {
                                Assembly asmTmp = Assembly.ReflectionOnlyLoadFrom(OrigFilePath);
                                string tmpVer = ReferenceItem.ReturnVersion(asmTmp.FullName);
                                if (new VersionComparer().Compare(tmpVer, ri.Version) >= 0)
                                {
                                    File.Copy(OrigFilePath, BuildFilePath, true);
                                }
                            }
                            catch (Exception ex)
                            {
                                string s = ex.ToString();
                            }
                        }
                        else
                        {
                            File.Copy(OrigFilePath, BuildFilePath, true);
                        }

                        if (!References.ContainsKey(asm.FullName))
                        {
                            // add this item as a reference(child, subreference) of the parent item
                            //riParent.ChildReferences.Add(ri.Name);
                            riParent.ChildReferences.Add(ri);
                            ExtractUsingStatement(asm);
                            References.Add(asm.FullName, ri);
                        }

                        CopyReferencedAssemblies(asm, MainAsmLocationPreCopy, References, ri);
                    }
                    catch (FileNotFoundException fne)
                    {
                        string Ignore = ""; //MessageBox.Show(fne.Message);
                    }
                    catch (Exception ex)
                    {
                        string s = "";
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void LoadReferencedAssemblies(Assembly MainAsm, string MainAsmLocationPreCopy,
          Dictionary<string, ReferenceItem> References, ReferenceItem riParent)
        {
            try
            {
                //if (MainAsm.GlobalAssemblyCache) return;
                AssemblyName[] names = MainAsm.GetReferencedAssemblies();
                for (int i = 0; i < names.Length; i++)
                {
                    Assembly asm = null;
                    try
                    {
                        string ReferenceName = string.Format("{0}.dll", names[i].Name);
                        if (IsSystemDll(ReferenceName)) continue;
                        asm = Assembly.LoadFrom(ReferenceName);
                        //if (asm.GlobalAssemblyCache) continue;

                        string OrigFilePath = string.Format("{0}\\{1}", MainAsmLocationPreCopy, ReferenceName);
                        string BuildFilePath = string.Format("{0}\\{1}", _BaseDomainPath, ReferenceName);

                        ReferenceItem ri = new ReferenceItem(asm.FullName, BuildFilePath, false, false, "none");
                        if (!References.ContainsKey(asm.FullName))
                        {
                            // add this item as a reference(child, subreference) of the parent item
                            //riParent.ChildReferences.Add(ri.Name);
                            riParent.ChildReferences.Add(ri);
                            ExtractUsingStatement(asm);
                            References.Add(asm.FullName, ri);
                        }

                        LoadReferencedAssemblies(asm, MainAsmLocationPreCopy, References, ri);
                    }
                    catch (FileNotFoundException fne)
                    {
                        string Ignore = "";
                    }
                    catch (Exception ex)
                    {
                        string s = "";
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ExtractUsingStatement(Assembly asm)
        {
            foreach (Type t in asm.GetExportedTypes())
            {
                if (!UsingStatements.Contains(t.Namespace) &&
                  (t.Namespace != null) &&
                  (t.Namespace != ""))
                    UsingStatements.Add(t.Namespace);
            }
        }

        private bool IsSystemDll(string Name)
        {
            if (KnownSystemDlls.Contains(Name))
                return true;
            return false;
        }

        public List<Type> GetAllTypesInDomain(List<string> UsingStatements)
        {
            List<Type> Types = new List<Type>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                for (int i = 0; i < UsingStatements.Count; i++)
                {
                    string Statement = UsingStatements[i];
                    if (Statement.Contains("System")) continue;
                    int pos = Statement.IndexOf(".");
                    if (pos != -1)
                    {
                        string holdRoot = Statement.Substring(0, pos);
                        string[] splitUsing = Statement.Split('.');
                        string finalUsing = splitUsing[splitUsing.GetLength(0) - 1];

                        pos = assembly.Location.LastIndexOf("\\");
                        string str = assembly.Location.Substring(pos + 1);
                        for (int a = 0; a < ReferenceDLL.Count; a++)
                        {
                            string FullName = ReferenceDLL[a];
                            if (FullName.Contains(str))
                            {
                                Type[] types = assembly.GetExportedTypes();
                                foreach (Type t in types)
                                {
                                    {
                                        if (!Types.Contains(t))
                                            Types.Add(t);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return Types;
        }
    }
}