using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.Data.Models._2AM.Managers
{
    public interface ILinkedKeyDataManager
    {
        void InsertLinkedKey(int key, Guid guid);

        void DeleteLinkedKey(Guid guid);

        int RetrieveLinkedKey(Guid guid);
    }
}
