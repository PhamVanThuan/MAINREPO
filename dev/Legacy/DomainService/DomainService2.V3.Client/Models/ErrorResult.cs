using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService2.V3.Client.Models
{
    public class ErrorResult
    {
        private List<string> errors;

        public IEnumerable<string> Errors 
        {
            get
            {
                return errors;
            } 
        }

        public ErrorResult()
        {
            this.errors = new List<string>();
        }

        public void AddError(string error)
        {
            this.errors.Add(error);
        }
    }
}
