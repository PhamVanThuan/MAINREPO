namespace SAHL.Tools.ObjectModelGenerator.Lib.Templates
{
    public partial class UIStatements
    {
        public UIStatements(string classNamespace)
        {
            this.Namespace = classNamespace;
        }

        public string Namespace { get; protected set; }

        public string GeneratedStatements { get; set; }
    }
}