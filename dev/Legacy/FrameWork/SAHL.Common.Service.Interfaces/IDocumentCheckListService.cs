using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.Service.Interfaces
{
    public interface IDocumentCheckListService
    {
        /// <summary>
        /// First checks the application for the matching DocumentSet.
        /// A list of DocumentTypes is generated that is required by the application at is current state.
        /// If any documents are no longer required it will be removed, any new items will be added.
        /// There are many instance where a document will appear more than once e.g. ID Document for Main Applicant and Suretor.
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <returns></returns>
        IList<IApplicationDocument> GetList(int ApplicationKey);

        /// <summary>
        /// Enumerates through the list of documents stored for the given application checking for any unchecked items
        /// (not recieved i.e. RecievedDate == NULL)
        /// If there are any unchecked items false will be returned or if all documents have been recieved, true will be returned.
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <returns></returns>
        bool ValidateList(int ApplicationKey);

        /// <summary>
        /// Enumerates through the list of documents stored for the given application checking for any unchecked items
        /// (not recieved i.e. RecievedDate == NULL)
        /// If there are any unchecked items false will be returned or if all documents have been recieved, true will be returned.
        /// This method also will display which documents have not been recieved.
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <returns></returns>
        IList<string> ValidateListWithMessages(int ApplicationKey);

        /// <summary>
        /// This method saves the state (checked/unchecked) and whom the item was updated by in the collection recieved.
        /// </summary>
        /// <param name="OfferDocumentList"></param>
        void SaveList(IList<IApplicationDocument> OfferDocumentList);

        /*
        void UpdateList(int ApplicationKey);

        IEventList<IApplicationDocument> GetList(int ApplicationKey, int OriginationSourceProductKey);

        bool InternalValidate(int ApplicationKey, int OriginationSourceProductKey);

        void UpdateList(int ApplicationKey, int OriginationSourceProductKey);

        bool ValidateList(int ApplicationKey, int OriginationSourceProductKey);

        bool InternalValidate(int ApplicationKey);
        */
    }
}