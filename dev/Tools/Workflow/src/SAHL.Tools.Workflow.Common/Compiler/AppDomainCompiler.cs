using System;
using System.CodeDom.Compiler;
using System.IO;
using Microsoft.CSharp;
using SAHL.Tools.Workflow.Common.CodeGeneration;
using SAHL.Tools.Workflow.Common.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Compiler
{
    public class AppDomainCompiler : MarshalByRefObject, IAppDomainCompiler
    {
        string buildDirectory;
        string outputDirectory;
        string lastSourceCodeCompiled;

        public AppDomainCompiler(string buildDirectory, string outputDirectory)
        {
            this.buildDirectory = buildDirectory;
            this.outputDirectory = outputDirectory;
            if (this.outputDirectory == string.Empty)
            {
                this.outputDirectory = this.buildDirectory;
            }
        }

        public CompilerResults Compile(Process process)
        {
            Generator generator = new Generator();
            string codeFile = generator.GenerateToString(process);

            this.lastSourceCodeCompiled = codeFile;

            string tempCodeFile = Path.Combine(outputDirectory, string.Format("{0}.cs", process.Name.Replace(" ", "")));
			using (FileStream fs = new FileStream(tempCodeFile, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(codeFile);
                    sw.Flush();
                }
            }

			CompilerResults compilerResults = null;
			using (CSharpCodeProvider codeProvider = new CSharpCodeProvider())
			{
				CompilerParameters compilerParameters = new CompilerParameters();
				compilerParameters.GenerateExecutable = false;
				compilerParameters.GenerateInMemory = false;
				compilerParameters.IncludeDebugInformation = true;
				compilerParameters.OutputAssembly = Path.Combine(outputDirectory, string.Format("{0}.dll", process.Name.Replace(" ", "")));

				compilerParameters.TempFiles = new TempFileCollection(buildDirectory, false);

				// add some default system references
				compilerParameters.ReferencedAssemblies.Add("mscorlib.dll");
				compilerParameters.ReferencedAssemblies.Add("System.dll");
				compilerParameters.ReferencedAssemblies.Add("System.Core.dll");
				compilerParameters.ReferencedAssemblies.Add("System.Data.dll");
				compilerParameters.ReferencedAssemblies.Add("System.Xml.dll");
				compilerParameters.ReferencedAssemblies.Add("System.Configuration.dll");

				foreach (AssemblyReference assembly in process.AssemblyReferences)
				{
					string realPath = Path.Combine(buildDirectory, assembly.Name);
					if (File.Exists(realPath))
					{
						compilerParameters.ReferencedAssemblies.Add(realPath);
					}
				}
				compilerResults = codeProvider.CompileAssemblyFromFile(compilerParameters, tempCodeFile);
			}

			return compilerResults;
        }

        public string LastSourceCodeCompiled
        {
            get
            {
                return this.lastSourceCodeCompiled;
            }
        }
	}
}