namespace SAHL.Common.Service.Interfaces
{
    public interface IResourceService
    {
        //ResourceManager ApplicationResources { get;}
        //ResourceManager FrameworkResources {get;}

        string GetString(string ResourceID);
    }
}