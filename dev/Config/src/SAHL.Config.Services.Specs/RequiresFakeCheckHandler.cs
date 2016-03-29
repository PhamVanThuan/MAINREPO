using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.Core.Services;
using SAHL.Core.SystemMessages;

namespace SAHL.Config.Core.Specs
{
    public interface IRequiresFake1 : IDomainCommandCheck
    {
        int Key { get; }
    }

    public interface IRequiresFake2 : IDomainCommandCheck
    {
        int SomeKey { get; }
    }

    public class RequiresFakeCheckHandler : IDomainCommandCheckHandler<IRequiresFake1>,
                                            IDomainCommandCheckHandler<IRequiresFake2>
    {
        public RequiresFakeCheckHandler()
        {
        }

        public bool ThrowException { get; set; }
        public bool IsRequiresFake1Handled { get; private set; }
        public bool IsRequiresFake2Handled { get; private set; }

        public ISystemMessageCollection HandleCheckCommand(IRequiresFake1 command)
        {
            this.IsRequiresFake1Handled = true;

            if (this.ThrowException) { throw new Exception(); }
            return SystemMessageCollection.Empty();
        }

        public ISystemMessageCollection HandleCheckCommand(IRequiresFake2 command)
        {
            this.IsRequiresFake2Handled = true;

            if (this.ThrowException) { throw new Exception(); }
            return SystemMessageCollection.Empty();
        }
    }
}
