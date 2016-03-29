using Mono.Cecil;
using SAHL.Tools.TileModelGenerator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.TileModelGenerator.Templates
{
    public partial class TileModelEdit
    {
        int counter = 0;

        public TileModel Model { get; protected set; }

        public TileModelEdit(TileModel model)
        {
            
            this.Model = model;
        }

        public int Counter
        {
            get
            {
                return counter++;
            }
        }

        public string GetToolTip(){
            if (counter % 2==0)
            {
                return "leftToolTip";
            }
            return "rightToolTip";
        }

        public string GetHtmlAttributes(string key){
            var array = Model.GetPropertyAnnotations(key);
            var retVal = array.Count() > 0? array.Select(x=>x.HtmlAttribute).Aggregate((b,a)=> b+' '+a ) : "";
            return retVal;
        }
        
    }
}
