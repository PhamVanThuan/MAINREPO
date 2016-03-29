using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.DataAccess
{
	public interface IManagedTransaction : IDisposable
	{
        void Commit();

        void Rollback();
	}
}
