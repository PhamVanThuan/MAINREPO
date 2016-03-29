namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IControlRepository
    {
        /// <summary>
        /// Gets the IControl object using the specified control description
        /// </summary>
        /// <param name="controlDescription"></param>
        /// <returns></returns>
        IControl GetControlByDescription(string controlDescription);
    }
}