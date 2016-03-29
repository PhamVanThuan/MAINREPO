namespace SAHL.Common.Collections.Interfaces
{
    /// <summary>
    /// IDAOObject Interface, maps between a interfaced business object and the actual underlying active record business object.
    /// </summary>
    public interface IDAOObject
    {
        object GetDAOObject();
    }
}