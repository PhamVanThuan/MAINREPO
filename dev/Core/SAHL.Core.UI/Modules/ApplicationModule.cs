namespace SAHL.Core.UI.Modules
{
    public abstract class ApplicationModule : IApplicationModule
    {
        public ApplicationModule(string name)
        {
            this.Name = name;
        }

        public string Name
        {
            get;
            protected set;
        }
    }
}