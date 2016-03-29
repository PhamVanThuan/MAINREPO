using System.Collections.Generic;

namespace SAHL.WCF.Validation.Engine
{
    public class ModelState
    {
        public bool IsValid
        {
            get { return Errors == null || Errors.Count == 0; }
        }
        public List<ModelError> Errors { get; set; }
    }
}
