namespace SAHL.Common.BusinessModel.Validation
{
    /// <summary>
    /// Provides an interface for objects that require validation.
    /// </summary>
    public interface IEntityValidation
    {
        /// <summary>
        /// Performs immediate validation on the entity.
        /// </summary>
        /// <returns></returns>
        bool ValidateEntity();
    }
}