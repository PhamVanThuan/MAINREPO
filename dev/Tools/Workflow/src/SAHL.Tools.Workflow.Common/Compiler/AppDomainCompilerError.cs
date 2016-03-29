using System;

namespace SAHL.Tools.Workflow.Common.Compiler
{
    [Serializable]
    public class AppDomainCompilerError : MarshalByRefObject, IAppDomainCompilerError
    {
        public AppDomainCompilerError(string errorNumber, string errorText, string fileName, int line, int column, bool isWarning)
        {
            this.ErrorNumber = errorNumber;
            this.ErrorText = errorText;
            this.FileName = fileName;
            this.Line = line;
            this.Column = column;
            this.IsWarning = IsWarning;
        }

        public string ErrorNumber
        {
            get;
            protected set;
        }

        public string ErrorText
        {
            get;
            protected set;
        }

        public string FileName
        {
            get;
            protected set;
        }

        public int Line
        {
            get;
            protected set;
        }

        public int Column
        {
            get;
            protected set;
        }

        public bool IsWarning
        {
            get;
            protected set;
        }
    }
}