namespace SAHL.Tools.Workflow.TestFolderGenerator
{
    public partial class GenericBooleanTest
    {
        private bool codeSectionResult;
        private string safeName;
        private string sectionName;
        private string sectionNamePrefix;
        private string workflowName;
        private string parentFolderName;
        private string parentFolderType;

        public GenericBooleanTest(bool codeSectionResult, string safeName, string sectionName, string sectionNamePrefix, string workflowName, string parentFolderName, string parentFolderType)
        {
            this.codeSectionResult = codeSectionResult;
            this.safeName = safeName;
            this.sectionName = sectionName;
            this.sectionNamePrefix = sectionNamePrefix;
            this.workflowName = workflowName;
            this.parentFolderName = parentFolderName;
            this.parentFolderType = parentFolderType;
        }

        public bool CodeSectionResult
        {
            get
            {
                return this.codeSectionResult;
            }
        }

        public string SectionName
        {
            get
            {
                return this.sectionName;
            }
        }

        public string SectionNamePrefix
        {
            get
            {
                return this.sectionNamePrefix;
            }
        }

        public string SafeName
        {
            get
            {
                return this.safeName;
            }
        }

        public string SafeNameLowered
        {
            get
            {
                return this.safeName.ToLower();
            }
        }

        public string WorkflowName
        {
            get
            {
                return this.workflowName;
            }
        }

        public string ParentFolderName
        {
            get
            {
                return this.parentFolderName;
            }
        }

        public string ParentFolderType
        {
            get
            {
                return this.parentFolderType;
            }
        }

        public string ToPropertyCase(string toConvert)
        {
            return toConvert.Substring(0, 1) + toConvert.Substring(1, toConvert.Length - 1);
        }

        public string MakeSafe(string toMakeSafe)
        {
            if (!string.IsNullOrEmpty(toMakeSafe))
            {
                string firstChar = toMakeSafe.Substring(0, 1);
                int value;
                bool isNumber = int.TryParse(firstChar, out value);
                if (isNumber)
                {
                    return "_" + toMakeSafe;
                }
            }

            return toMakeSafe;
        }
    }
}