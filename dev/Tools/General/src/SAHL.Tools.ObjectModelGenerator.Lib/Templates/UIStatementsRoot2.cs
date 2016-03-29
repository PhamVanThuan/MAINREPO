namespace SAHL.Tools.ObjectModelGenerator.Lib.Templates
{
    public partial class UIStatementsRoot
    {
        public UIStatementsRoot(string classNamespace)
        {
            this.Namespace = classNamespace;
        }

        public string Namespace { get; protected set; }
    }
}