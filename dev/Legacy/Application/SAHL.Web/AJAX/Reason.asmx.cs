using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using System.Web.Script.Services;

namespace SAHL.Web.AJAX
{
    /// <summary>
    /// Summary description for Reason
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class Reason : System.Web.Services.WebService
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ReasonTypeKey"></param>
        /// <returns></returns>
        [WebMethod]       
        public List<CustomReasonDefinition> GetReasonDefinitionDescriptions(int ReasonTypeKey, bool sortByDescription)        
        {
            IReasonRepository reasonRepo = RepositoryFactory.GetRepository<IReasonRepository>();

            IReadOnlyEventList<IReasonDefinition> reasonDefinitions = reasonRepo.GetReasonDefinitionsByReasonTypeKey(ReasonTypeKey, sortByDescription);

            List<CustomReasonDefinition> lstDefinitions = new List<CustomReasonDefinition>();

            foreach (IReasonDefinition reasonDef in reasonDefinitions)
            {
                    CustomReasonDefinition def = new CustomReasonDefinition();
                    def.Key =  reasonDef.Key;
                    def.ReasonDescription =  reasonDef.ReasonDescription.Description;
                    def.AllowComment = reasonDef.AllowComment;
                    lstDefinitions.Add(def);            
            }

            return lstDefinitions;            
        }
    }

    /// <summary>
    /// A custom structure required to return the appropriate fields to the calling javascript
    /// </summary>
    public class CustomReasonDefinition
    {
        private int _key;
        private string _reasonDescription;
        private bool _allowComment;

        /// <summary>
        /// 
        /// </summary>
        public int Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string ReasonDescription
        {
            get
            {
                return _reasonDescription;
            }
            set
            {
                _reasonDescription = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool AllowComment
        {
            get
            {
                return _allowComment;
            }
            set
            {
                _allowComment = value;
            }
        }


    }
}
