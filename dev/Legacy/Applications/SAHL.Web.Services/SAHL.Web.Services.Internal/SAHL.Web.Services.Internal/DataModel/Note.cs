using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SAHL.Web.Services.Internal.DataModel
{
    [DataContract]
    public class Note
     {
        [DataMember]
        public int GenericKey
        {
            get;
            set;
        }

        [DataMember]
        public int GenericKeyTypeKey
        {
            get;
            set;
        }

         [DataMember]
         public int Key
         {
             get;
             set;
         }

         [DataMember]
         public List<SAHL.Web.Services.Internal.DataModel.NoteDetail> NoteDetails
         {
             get;
             set;
         }
     }
}