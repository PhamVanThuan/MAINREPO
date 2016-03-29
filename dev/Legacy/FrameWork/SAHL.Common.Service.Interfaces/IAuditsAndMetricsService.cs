using System.Collections;

namespace SAHL.Common.Service.Interfaces
{
    public interface IAuditsAndMetricsService
    {
        /// <summary>
        /// Used to store audit information.
        /// </summary>
        /// <param name="daoEntity">The object being saved or deleted.</param>
        /// <param name="previousState">The previous state of the object (Prior to any changes).</param>
        /// <param name="currentState">The new state of the object.</param>
        void StoreAudit(object daoEntity, IDictionary previousState, IDictionary currentState);
    }
}